﻿using System;
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
            Name = "Modded";
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
                    output += "Texture widths and heights must be a power of 2\r\n";
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
                int maxmipexp = (int)Math.Floor(Math.Log((double)Size) / Math.Log(2));
                basemipsize = 1 << maxmipexp;
                BytesPerPixel = (float)basemipsize / Width / Height;

                output += $"DDS loaded\r\n";
                Ready = true;
                return true;
            }
        }

        public bool Write(byte[] hdmipmaps, List<byte[]> mipmaps, out string output)
        {
           if (ArrayCount > 1)
            {
                output = "";
                bool ret = false;
                for (int i = 0; i < ArrayCount; i++)
                {
                    string o2;
                    ret |= WriteSingle(
                        hdmipmaps is null ? null : hdmipmaps.Skip(i * hdmipmaps.Length / (int)ArrayCount).Take(hdmipmaps.Length / (int)ArrayCount).ToArray(),
                        mipmaps.Skip(i * mipmaps.Count / (int)ArrayCount).Take(mipmaps.Count / (int)ArrayCount).ToList(),
                        i, out o2);
                    output += o2;
                }
                output += "\r\n";
                return ret;
            }
            else
                return WriteSingle(hdmipmaps, mipmaps, -1, out output);
        }

        public bool WriteSingle(byte[] hdmipmaps, List<byte[]> mipmaps, int image, out string output)
        {
            // just assume everything has been set correctly!
            output = "";
            string fn = Filename;
            if (image > -1)
                fn = Path.ChangeExtension(fn, $".A{image}.dds");

            using (var fs = File.Open(fn, FileMode.Create))
            using (var bw = new BinaryWriter(fs))
            {
                bw.Write(Encoding.ASCII.GetBytes("DDS "));
                bw.Write((uint)0x7c);
                bw.Write((uint)(DDS_Flags.DDSD_CAPS | DDS_Flags.DDSD_HEIGHT | DDS_Flags.DDSD_WIDTH | DDS_Flags.DDSD_PIXELFORMAT | DDS_Flags.DDSD_LINEARSIZE | DDS_Flags.DDSD_MIPMAPCOUNT));
                bw.Write((uint)Height);
                bw.Write((uint)Width);
                // linearsize
                bw.Write((uint)((hdmipmaps is null ? basemipsize: basemipsize * 1 << (2 * (int)HDMipmaps)) / Cubemaps));
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
                // caps2
                bw.Write((uint)(Cubemaps > 1 ? DDS_Caps2.DDS_CUBEMAP_ALLFACES : 0));
                // caps3-4, reserved
                bw.Write(new byte[3 * 4]);

                // DXT10 header
                bw.Write((uint)Format);
                // dimension - 2d or 3d
                bw.Write((uint)(Height > 1 ? 3 : 2));
                // misc
                bw.Write((uint)(Cubemaps > 1 ? miscFlag.DDS_RESOURCE_MISC_TEXTURECUBE : 0));
                // arraySize
                bw.Write((uint)1);
                // misc flags - DDS_ALPHA_MODE_UNKNOWN
                bw.Write((uint)0);

                for (int i = 0; i < Cubemaps; i++)
                {
                    if (hdmipmaps is not null)
                        bw.Write(hdmipmaps.Skip((int)(i * hdmipmaps.Length / Cubemaps)).Take((int)(hdmipmaps.Length / Cubemaps)).ToArray());

                    bw.Write(mipmaps[i]);
                }

                output += $"Wrote {fs.Position} bytes to: {fn}\r\n";
            }

            return true;
        }
    }
}
