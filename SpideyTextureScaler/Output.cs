using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpideyTextureScaler
{
    internal class Output : TextureBase
    {
        public Output()
        {
            Name = "Output";
        }

        public override bool Read(out string output, out int errorrow, out int errorcol)
        {
            // nothing to do
            output = "";
            errorrow = 0;
            errorcol = -1;
            Ready = true;
            return true;
        }

        internal void Generate(Source tex, DDS dds, bool testmode, bool ignoreformat, out string output, out int errorrow, out int errorcol)
        {
            output = "";
            errorrow = 0;
            errorcol = -1;

            // pre-flights
            if (dds.Width < tex.Width || dds.Height < tex.Height)
            {
                output += "Replacement image is smaller than source.\r\n";
                errorcol = 1;
                return;
            }

            if (tex.BytesPerPixel != dds.BytesPerPixel)
            {
                output += "Bytes per pixel is different between files, formats are incompatible.\r\n";
                errorcol = 6;
                return;
            }

            if (tex.aspect != dds.aspect)
            {
                output += "Aspect ratio is different between files.\r\n";
                errorcol = 2;
                return;
            }

            if (!ignoreformat && tex.Format != dds.Format)
            {
                if (MessageBox.Show("Files have different DXGI formats.\r\n\r\nAre you sure you want to continue?",
                    "DXGI format mismatch", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                {
                    output += "Canceled due to DDS format mismatch.\r\n";
                    errorcol = 9;
                    return;
                }
            }

            Width = dds.Width;
            Height = dds.Height;
            sd_width = tex.sd_width;
            sd_height = tex.sd_height;
            Size = tex.Size;
            Mipmaps = tex.Mipmaps;
            uint extrasdmipmaps = 0;
            uint extrasdmipsize = 0;

            uint scalefactor = (uint)(Math.Log((uint)dds.Width / tex.sd_width) / Math.Log(2));
            uint sizeincrease = 0;
            for (int i = (int)scalefactor; i > 0; i--)
                sizeincrease += (uint)(tex.basemipsize << (2 * i));

            if (tex.HDSize > 0)
            {
                HDMipmaps = scalefactor;
                HDSize = sizeincrease;
            }
            else
            {
                HDMipmaps = 0;
                HDSize = 0;
                Size += sizeincrease;
                extrasdmipmaps = scalefactor;
                extrasdmipsize = sizeincrease;
                sd_width <<= (int)extrasdmipmaps;
                sd_height <<= (int)extrasdmipmaps;
            }

            if (dds.Mipmaps < HDMipmaps + extrasdmipmaps + tex.Mipmaps)
            {
                output += $"Not enough mipmaps in DDS file to replace this texture (needs {HDMipmaps + extrasdmipmaps + tex.Mipmaps})\r\n";
                errorrow = 1;
                return;
            }

            byte[] hdmips, extrasdmips, sdmips;
            using (var fs = File.Open(dds.Filename, FileMode.Open, FileAccess.Read))
            using (var br = new BinaryReader(fs))
            {
                fs.Seek(dds.dataoffset, SeekOrigin.Begin);
                hdmips = br.ReadBytes((int)HDSize);
                extrasdmips = br.ReadBytes((int)(extrasdmipsize));
                sdmips = br.ReadBytes((int)tex.Size);
            }

            if (HDMipmaps > 0)
            {
                string hdtexturefile = Path.ChangeExtension(Filename, "hd.texture");
                using (var fs = File.Open(hdtexturefile, FileMode.Create))
                using (var bw = new BinaryWriter(fs))
                {
                    bw.Write(hdmips);
                }
                output += $"Wrote {hdtexturefile} ({HDSize} bytes)\r\n";
            }

            Mipmaps += extrasdmipmaps;
            using (var fs = File.Open(Filename, FileMode.Create))
            using (var bw = new BinaryWriter(fs))
            {
                bw.Write(tex.header);
                bw.Write((uint)Size);
                bw.Write((uint)(HDSize));
                bw.Write((ushort)dds.Width);
                bw.Write((ushort)dds.Height);
                bw.Write((ushort)sd_width);
                bw.Write((ushort)sd_height);
                bw.Write(tex.textureheader.Skip(16).Take(14).ToArray());
                bw.Write((byte)Mipmaps);
                bw.Write(tex.textureheader[24]);
                bw.Write((byte)HDMipmaps);
                bw.Write(tex.textureheader.Skip(33).ToArray());

                bw.Write(extrasdmips);
                if (testmode)
                    // keep original sd mipmaps
                    bw.Write(tex.mipmaps);
                else 
                    bw.Write(sdmips);

                output += $"Wrote {Filename} ({fs.Position} bytes)\r\n";
            }

            output += $"Finished ({DateTime.Now})\n";
            return;

        }
    }
}
