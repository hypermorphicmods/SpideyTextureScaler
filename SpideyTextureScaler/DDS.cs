using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpideyTextureScaler
{
    internal class DDS : TextureBase
    {
        public long dataoffset;

        public DDS()
        {
            Name = "DDS";
        }

        public override bool Read(out string output, out int errorrow, out int errorcol)
        {
            output = "";
            errorrow = 1;
            errorcol = -1;

            using (var fs = File.Open(Filename, FileMode.Open, FileAccess.Read))
            using (BinaryReader br = new BinaryReader(fs))
            {
                if (br.ReadUInt32() != 542327876)
                {
                    output += "Not a DDS file\r\n";
                    errorcol = 1;
                    return false;
                }

                var flags = (DDS_Flags) br.ReadUInt32();

                br.ReadUInt32();
                Height = br.ReadUInt32();
                Width = br.ReadUInt32();
                if (Height * Width == 0 ||
                    // power of 2 trick
                    (Height & (Height - 1)) != 0 ||
                    (Width & (Width - 1)) != 0)
                {
                    output += "Must be a square texture and a power of 2\r\n";
                    errorcol = 2;
                    return false;
                }
                aspect = (int)(Math.Log((double)Width / (double)Height) / Math.Log(2));

                // avoid Microsoft's pitch / linearsize screw-up
                br.ReadUInt32();

                // depth
                br.ReadUInt32();
                Mipmaps = br.ReadUInt32();

                fs.Seek(0x54, SeekOrigin.Begin);
                bool hasDX10Header = br.ReadUInt32() == 808540228;
                fs.Seek(0x80, SeekOrigin.Begin);

                if (hasDX10Header)
                {
                    Format = (DXGI_FORMAT?)br.ReadUInt32();
                    fs.Seek(0x94, SeekOrigin.Begin);
                }

                dataoffset = fs.Position;

                // calculate based on remaining data
                Size = (uint)(fs.Length - fs.Position);
                BytesPerPixel = Math.Pow(2, Math.Floor(Math.Log((double)Size) / Math.Log(2))) / Width / Height;

                output += $"DDS loaded\r\n";
                Ready = true;
                return true;
            }
        }

        public bool Write(byte[] hdmipmaps, byte[] mipmaps, out string output)
        {
            // just assume everything has been set correctly!
            output = "";

            using (var fs = File.Open(Filename, FileMode.Create))
            using (var bw = new BinaryWriter(fs))
            {
                bw.Write(Encoding.ASCII.GetBytes("DDS "));
                bw.Write((uint)0x7c);
                bw.Write((uint)(DDS_Flags.DDSD_CAPS | DDS_Flags.DDSD_HEIGHT | DDS_Flags.DDSD_WIDTH | DDS_Flags.DDSD_PIXELFORMAT | DDS_Flags.DDSD_LINEARSIZE | DDS_Flags.DDSD_MIPMAPCOUNT));
                bw.Write((uint)Height);
                bw.Write((uint)Width);
                // linearsize
                bw.Write((uint)(hdmipmaps is null ? basemipsize : basemipsize * 1 << (2 * (int)HDMipmaps)));
                // depth
                bw.Write((uint)0);
                bw.Write((uint)((uint)Mipmaps + (uint)HDMipmaps));
                // reserved
                bw.Write(new byte[11 * 4]);
                
                // pixelformat
                bw.Write((uint)32);
                // FourCC
                bw.Write((uint)4);
                bw.Write(Encoding.ASCII.GetBytes("DX10"));
                bw.Write(new byte[5 * 4]);

                // caps
                bw.Write((uint)(DDS_Caps.DDSCAPS_TEXTURE | (Mipmaps + HDMipmaps > 0 ? DDS_Caps.DDSCAPS_COMPLEX | DDS_Caps.DDSCAPS_MIPMAP : 0)));
                // caps2-4, reserved
                bw.Write(new byte[4 * 4]);

                // DXT10 header
                bw.Write((uint)Format);
                // dimension - 2d or 3d
                bw.Write((uint)(Height > 1 ? 3 : 2));
                // misc
                bw.Write((uint)0);
                // arraySize
                bw.Write((uint)1);
                // misc flags - DDS_ALPHA_MODE_UNKNOWN
                bw.Write((uint)0);

                if (hdmipmaps is not null)
                    bw.Write(hdmipmaps);

                bw.Write(mipmaps);

                output += $"Wrote {fs.Position} bytes to: {Filename}\r\n";
            }

            return true;
        }
    }
}
