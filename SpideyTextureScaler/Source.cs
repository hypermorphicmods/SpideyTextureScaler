using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpideyTextureScaler
{
    internal class Source : TextureBase
    {
        public byte[] header;
        public byte[] textureheader;
        public List<byte[]> mipmaps;
        public string hdfilename;
        public bool exportable;
        public List<uint> resourceids;

        public Source()
        {
            Name = "Source";
            resourceids = new List<uint>()
            {
                // Texture resource IDs for:
                // Marvel's Spider-Man Remastered
                // Marvel's Spider-Man: Miles Morales
                0x5C4580B9,
                // Ratchet & Clank: Rift Apart
                0x8F53A199,
            };
        }

        public override bool Read(out string output, out int errorrow, out int errorcol)
        {
            output = "";
            errorrow = 0;
            errorcol = -1;
            exportable = false;

            using (var fs = File.Open(Filename, FileMode.Open, FileAccess.Read))
            using (BinaryReader br = new BinaryReader(fs))
            {
                if (!resourceids.Contains(br.ReadUInt32()) ||
                    fs.Seek(32, SeekOrigin.Current) < 1 ||
                    br.ReadUInt32() != 1145132081 ||
                    !resourceids.Contains(br.ReadUInt32()))
                {
                    output += "Not a texture asset.  Please import the lowest resolution copy.\r\n";
                    errorcol = 1;
                    return false;
                };

                br.ReadUInt32();
                if (br.ReadUInt32() != 1)
                {
                    output += "Multiple sections not implemented.  Woops\r\n";
                    errorcol = 1;
                    return false;
                }

                if (br.ReadUInt32() != 1323185555)
                {
                    output += "Unexpected section type\r\n";
                    errorcol = 1;
                    return false;
                }
                var offset = br.ReadUInt32();
                var size = br.ReadUInt32();

                fs.Seek(0, SeekOrigin.Begin);
                header = br.ReadBytes((int)offset + 36);
                textureheader = br.ReadBytes((int)size);

                fs.Seek((int)offset + 36, SeekOrigin.Begin);
                Size = br.ReadUInt32();
                HDSize = br.ReadUInt32();
                HDMipmaps = 0;
                Mipmaps = 0;
                Width = br.ReadUInt16();
                Height = br.ReadUInt16();
                sd_width = br.ReadUInt16();
                sd_height = br.ReadUInt16();
                ArrayCount = br.ReadUInt16();
                br.ReadByte();
                var channels = br.ReadByte();
                var dxgi_format = br.ReadUInt16();
                Format = (DXGI_FORMAT?)dxgi_format;
                br.ReadBytes(8);
                Mipmaps = br.ReadByte();
                br.ReadByte();
                HDMipmaps = br.ReadByte();

                Cubemaps = 1;
                int expectedsize = CalculateExpectedSize();
                if (expectedsize == 0)
                {
                    output += $"Support for DXGI format not implemented: {Format}\r\n";
                    errorcol = 1;
                    return false;
                }

                if (expectedsize * ArrayCount != (HDSize + Size))
                {
                    if (expectedsize * ArrayCount * 6 == (HDSize + Size))
                        Cubemaps = 6;
                    else
                    {
                        output += "Image data size does not match expected\r\n";
                        errorcol = 1;
                        return false;
                    }
                }

                aspect = (int)(Math.Log((double)Width / (double)Height) / Math.Log(2));

                fs.Seek(11, SeekOrigin.Current);
                mipmaps = new();
                for (int i = 0; i < Images; i++)
                    mipmaps.Add(br.ReadBytes((int)(Size / Images)));

                hdfilename = Path.ChangeExtension(Filename, ".hd.texture");
                string hdtxt;
                if (HDSize == 0)
                {
                    hdtxt = "single-part texture";
                    hdfilename = "";
                }
                else if (File.Exists(hdfilename))
                    hdtxt = "hd part found";
                else if (File.Exists(hdfilename.Replace(".hd.texture", "_hd.texture")))
                {
                    hdfilename = hdfilename.Replace(".hd.texture", "_hd.texture");
                    hdtxt = "found SpiderTex style _hd file";
                }
                else
                {
                    hdtxt = "hd part MISSING";
                    hdfilename = "";
                }
                var arraytxt = Images > 1 ? $"with {ArrayCount} packed {(Cubemaps > 1 ? "cubemaps" : "textures")} " : "";
                output += $"Source {arraytxt}loaded ({hdtxt})\r\n";

                if (hdfilename != "")
                {
                    var hdfilesize = new FileInfo(hdfilename).Length;
                    if (hdfilesize != HDSize)
                    {
                        output += $"HD component is the wrong size (expected {HDSize} bytes, got {hdfilesize})\r\n";
                        errorcol = 8;
                        return false;
                    }
                }

                Ready = errorcol == -1;
                return true;
            }
        }
    }
}
