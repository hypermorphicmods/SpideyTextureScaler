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
        public byte[] mipmaps;
        public int sd_mipmap_count;
        public int hd_mipmap_count;
        public byte extra_hd_mipmaps;

        public Source()
        {
            Name = "Extracted";
        }

        public override bool Read(out string output, out int errorrow, out int errorcol)
        {
            output = "";
            errorrow = 0;
            errorcol = -1;

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
                if (HDSize == 0)
                {
                    output += "No HD texture found...\r\n";
                    errorcol = 1;
                    return false;
                }

                HDMipmaps = 0;
                int maxmipexp = (int)Math.Floor(Math.Log((double)HDSize) / Math.Log(2));
                int s = (int)HDSize;
                for (int i = maxmipexp; s >= 1 << i && (s & (1 << i)) > 0; i -= 2)
                {
                    HDMipmaps++;
                    s -= 1 << i;
                }
                Mipmaps = 0;
                s = (int)Size;
                basemipsize = 1 << (int)Math.Floor(Math.Log(s) / Math.Log(2));
                
                for (int i = maxmipexp - (2 * (int)HDMipmaps); s >= 1 << i && (s & (1 << i)) > 0; i -= 2)
                {
                    Mipmaps++;
                    s -= 1 << i;
                }

                Width = br.ReadUInt16();
                Height = br.ReadUInt16();
                sd_width = br.ReadUInt16();
                sd_height = br.ReadUInt16();
                BytesPerPixel = (uint)(basemipsize / sd_width / sd_height);
                br.ReadUInt16();
                br.ReadByte();
                var channels = br.ReadByte();
                var dxgi_format = br.ReadUInt16();
                Format = (DXGI_FORMAT?)dxgi_format;

                br.ReadByte();
                sd_mipmap_count = br.ReadByte();
                if (sd_mipmap_count > 0 && sd_mipmap_count != Mipmaps)
                {
                    output += "Mipmap count discrepancy\r\n";
                    errorcol = 4;
                    return false;
                }
                br.ReadByte();
                hd_mipmap_count = br.ReadByte();
                if (hd_mipmap_count > 0 && hd_mipmap_count != HDMipmaps)
                {
                    output += "HDMipmap count discrepancy\r\n";
                    errorcol = 5;
                    return false;
                }

                fs.Seek(6, SeekOrigin.Current);
                extra_hd_mipmaps = br.ReadByte();
                if (hd_mipmap_count == 0 && extra_hd_mipmaps != HDMipmaps)
                {
                    output += "Extra mipmap count discrepancy\r\n";
                    errorcol = 5;
                    return false;
                }

                fs.Seek(11, SeekOrigin.Current);
                mipmaps = br.ReadBytes((int)Size);

                output += $"Source loaded\r\n";
                return true;
            }
        }
    }
}
