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

        public Source()
        {
            Name = "Extracted";
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
                if (br.ReadUInt32() != 1548058809 ||
                    fs.Seek(32, SeekOrigin.Current) < 1 ||
                    br.ReadUInt32() != 1145132081 ||
                    br.ReadUInt32() != 1548058809)
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
                Images = br.ReadUInt16();

                int s = (int)(HDSize / Images);
                int maxmipexp = (int)Math.Floor(Math.Log(s) / Math.Log(2));
                if (HDSize > 0)
                {
                    for (int i = maxmipexp; s >= 1 << i && (s & (1 << i)) > 0; i -= 2)
                    {
                        HDMipmaps++;
                        s -= 1 << i;
                    }
                }

                s = (int)(Size / Images);
                maxmipexp = (int)Math.Floor(Math.Log(s) / Math.Log(2));
                basemipsize = 1 << maxmipexp;
                for (int i = maxmipexp; s >= 1 << i && (s & (1 << i)) > 0; i -= 2)
                {
                    Mipmaps++;
                    s -= 1 << i;
                }

                BytesPerPixel = Math.Pow(2, (Math.Floor(Math.Log((double)basemipsize / sd_width / sd_height) / Math.Log(2))));
                aspect = (int)(Math.Log((double)Width / (double)Height) / Math.Log(2));

                br.ReadByte();
                var channels = br.ReadByte();
                var dxgi_format = br.ReadUInt16();
                Format = (DXGI_FORMAT?)dxgi_format;
                br.ReadBytes(8);
                if (Mipmaps != br.ReadByte())
                {
                    output += "Mipmap count discrepancy\r\n";
                    errorcol = 4;
                    return false;
                }
                br.ReadByte();
                if (HDMipmaps != br.ReadByte())
                {
                    output += "HDMipmap count discrepancy\r\n";
                    errorcol = 5;
                    return false;
                }

                fs.Seek(11, SeekOrigin.Current);
                mipmaps = new();
                for (int i = 0; i < Images; i++)
                    mipmaps.Add(br.ReadBytes((int)(Size / Images)));

                hdfilename = Path.ChangeExtension(Filename, ".hd.texture");
                string hdtxt;
                if (File.Exists(hdfilename))
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
                var arraytxt = Images > 1 ? $"with {Images} packed textures " : "";
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
