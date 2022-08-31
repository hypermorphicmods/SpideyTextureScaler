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
            return true;
        }

        internal void Generate(Source tex, DDS dds, bool testmode, out string output, out int errorrow, out int errorcol)
        {
            output = "";
            errorrow = 0;
            errorcol = -1;

            if (dds.Width < tex.Width || dds.Height < tex.Height)
            {
                output += "Replacement image is smaller than source\r\n";
                errorcol = 1;
                return;
            }

            if (tex.BytesPerPixel != dds.BytesPerPixel)
            {
                output += "Bytes per pixel is different between files, formats are incompatible\r\n";
                errorcol = 6;
                return;
            }

            if (tex.Format != dds.Format)
            {
                if (MessageBox.Show("Files have different DXGI formats and are probably incompatible.\r\n\r\nAre you sure you want to continue?",
                    "DXGI format mismatch", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                {
                    output += "Canceled due to wrong DDS format.\r\n";
                    errorcol = 9;
                    return;
                }
            }


            HDMipmaps = (uint)(Math.Log((uint)dds.Width / tex.sd_width) / Math.Log(2));
            HDSize = 0;
            for (int i = (int)HDMipmaps; i > 0; i--)
                HDSize += (uint)(tex.basemipsize << (2 * i));

            if (dds.Mipmaps < HDMipmaps + tex.Mipmaps)
            {
                output += $"Not enough mipmaps in DDS file to replace this texture (needs {HDMipmaps + tex.Mipmaps})\r\n";
                errorrow = 1;
                return;
            }

            byte[] hdmips, sdmips;
            using (var fs = File.Open(dds.Filename, FileMode.Open, FileAccess.Read))
            using (var br = new BinaryReader(fs))
            {
                fs.Seek(dds.dataoffset, SeekOrigin.Begin);
                hdmips = br.ReadBytes((int)HDSize);
                sdmips = br.ReadBytes((int)tex.Size);
            }

            string hdtexturefile = Path.ChangeExtension(Filename, "hd.texture");
            using (var fs = File.Open(hdtexturefile, FileMode.Create))
            using (var bw = new BinaryWriter(fs))
            {
                bw.Write(hdmips);
            }
            output += $"Wrote {hdtexturefile} ({HDSize} bytes)\r\n";


            using (var fs = File.Open(Filename, FileMode.Create))
            using (var bw = new BinaryWriter(fs))
            {
                bw.Write(tex.header);
                bw.Write((uint)tex.Size);
                bw.Write((uint)HDSize);
                bw.Write((ushort)dds.Width);
                bw.Write((ushort)dds.Height);
                bw.Write(tex.textureheader.Skip(12).Take(11).ToArray());
                bw.Write((byte)tex.sd_mipmap_count);
                bw.Write(tex.textureheader[24]);
                if (tex.hd_mipmap_count > 0)
                {
                    // this is a guess....?
                    // record the new number
                    bw.Write((byte)(HDMipmaps));
                    bw.Write(tex.textureheader.Skip(26).Take(6).ToArray());
                    // extrahdmipmaps - include this or make it zero??
                    // try 0 since it's already accounted for
                    bw.Write((byte)0);
                }
                else
                {
                    bw.Write(tex.textureheader.Skip(25).Take(7).ToArray());
                    // extrahdmipmaps - this has been tested
                    bw.Write((byte)HDMipmaps);
                }
                bw.Write(tex.textureheader.Skip(33).ToArray());

                if (testmode)
                    // keep original sd mipmaps
                    bw.Write(tex.mipmaps);
                else 
                    bw.Write(sdmips);

                Mipmaps = tex.Mipmaps;
                Size = (uint)sdmips.Length;

                output += $"Wrote {Filename} ({fs.Position} bytes)\r\n";
            }

            output += $"Finished ({DateTime.Now})\n";
            return;

        }
    }
}
