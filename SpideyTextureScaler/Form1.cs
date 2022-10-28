using System.Reflection;

namespace SpideyTextureScaler
{
    public partial class Form1 : Form
    {
        Program program;
        string lastsourcedir, lastddsdir, lastoutputdir;

        public Form1(Program p)
        {
            InitializeComponent();
            program = p;

            p.texturestats[0].ResetVisible();
            p.texturestats[1].ResetVisible();
            p.texturestats[2].ResetVisible();
            sourcelabel.DataBindings.Add("Text", p.texturestats[0], nameof(TextureBase.Filename));
            ddslabel.DataBindings.Add("Text", p.texturestats[1], nameof(TextureBase.Filename));
            outputlabel.DataBindings.Add("Text", p.texturestats[2], nameof(TextureBase.Filename));
            texturestatsBindingSource.DataSource = program.texturestats;
            this.Text = $"SpideyTextureScaler v{Assembly.GetExecutingAssembly().GetName().Version.ToString(3)}";
        }

        private void UpdateControls()
        {
            if (program.texturestats[0].Ready && program.texturestats[0].ArrayCount > 0 && program.texturestats[1].Ready && 
                program.texturestats[1].ArrayCount < program.texturestats[0].ArrayCount)
            {
                program.texturestats[1].Ready = false;
                MarkError(1, 6);
            }
            generatebutton.Enabled = program.texturestats[0].Ready && program.texturestats[1].Ready && program.texturestats[2].Ready;

            if (program.texturestats[0].Ready && program.texturestats[1].Ready && program.texturestats[0].HDSize > 0)
            {
                var scale = (int)(Math.Floor(Math.Log((float)program.texturestats[1].basemipsize / (float)program.texturestats[0].basemipsize) / Math.Log(2.0f)) / 2.0f);
                extrasdctl.Maximum = scale;
                extrasdctl.Minimum = 0;
                extrasdctl.Enabled = true;
                extrasdctl_ValueChanged(null, null);
            }
            else
                extrasdctl.Enabled = false;
        }

        private void extrasdctl_ValueChanged(object sender, EventArgs e)
        {
            hdlabel.Text = $"({extrasdctl.Maximum - extrasdctl.Value} HD)";
        }

        private void sourcebutton_Click(object sender, EventArgs e)
        {
            var f = new OpenFileDialog();
            f.Filter = "Low or high res texture|*.texture";
            if (lastsourcedir is not null)
                f.InitialDirectory = lastsourcedir;
            var obj = (Source)program.texturestats[0];
            obj.ResetVisible();
            ClearErrorRow(dataGridView1.Rows[0]);
            string output;
            int errorrow = 0;
            int errorcol = -1;

            this.Text = $"SpideyTextureScaler v{Assembly.GetExecutingAssembly().GetName().Version.ToString(3)}";
            saveddsbutton.Enabled = false;
            if (f.ShowDialog() == DialogResult.OK)
            {
                program.texturestats[0] = obj = new Source();

                obj.Filename = f.FileName;
                lastsourcedir = Path.GetDirectoryName(f.FileName) + @"\";
                if (obj.Filename.ToLower().EndsWith(".hd.texture") || obj.Filename.ToLower().EndsWith("_hd.texture"))
                {
                    var h = obj.Filename.Substring(0, obj.Filename.Length - ".hd.texture".Length) + ".texture";
                    if (File.Exists(h))
                        obj.Filename = h;
                }
                if (obj.Read(out output, out errorrow, out errorcol))
                {
                    saveddsbutton.Enabled = true;
                    this.Text = $"{Path.GetFileNameWithoutExtension(Path.GetFileName(obj.Filename))} - SpideyTextureScaler v{Assembly.GetExecutingAssembly().GetName().Version.ToString(3)}";
                }
                ddsfilenamelabel.Text = Path.GetFileName(Path.ChangeExtension(obj.Filename, (obj.ArrayCount > 1 ? ".Ax.dds" : ".dds")));
                saveddsbutton.Text = obj.ArrayCount > 1 ? "Save multiple .dds" : "Save as .dds";

                outputbox.Text = output;
            }

            dataGridView1.Refresh();
            dataGridView1.AutoResizeColumns();
            sourcelabel.DataBindings.Clear();
            sourcelabel.DataBindings.Add("Text", obj, nameof(TextureBase.Filename));
            MarkError(errorrow, errorcol);
            UpdateControls();
        }

        private void saveddsbutton_Click(object sender, EventArgs e)
        {
            var tex = (Source)program.texturestats[0];
            var savedds = new DDS();
            savedds.Filename = Path.ChangeExtension(tex.Filename, ".dds");
            savedds.Mipmaps = tex.Mipmaps;
            savedds.ArrayCount = tex.ArrayCount;
            savedds.Cubemaps = tex.Cubemaps;
            savedds.Format = tex.Format;
            savedds.basemipsize = tex.basemipsize;
            byte[] hdmips = null;
            string output;

            savedds.HDMipmaps = 0;
            savedds.Width = tex.Width;
            savedds.Height = tex.Height;
            if (tex.HDSize > 0)
            {
                if (tex.hdfilename == "" || !File.Exists(tex.hdfilename))
                {
                    if (MessageBox.Show($"No corresponding .hd.texture file found.\r\n\r\nDo you want to proceed with the low resolution texture ({tex.sd_width} x {tex.sd_height})?",
                    "Alert",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.No)
                    {
                        return;
                    }

                    savedds.Width = (uint)tex.sd_width;
                    savedds.Height = (uint)tex.sd_height;
                }
                else
                {
                    savedds.HDMipmaps = tex.HDMipmaps;
                    hdmips = File.ReadAllBytes(tex.hdfilename);
                }
            }

            savedds.Write(hdmips, tex.mipmaps, out output);
            outputbox.Text = output;
        }

        private void ddsbutton_Click(object sender, EventArgs e)
        {
            var f = new OpenFileDialog();
            f.Filter = program.texturestats[0].ArrayCount > 1  ? "DirectDraw Surface Array (*.A0.dds)|*.A0.dds" : "DirectDraw Surface (*.dds)|*.dds";
            if (lastddsdir is not null)
                f.InitialDirectory = lastddsdir;
            var obj = (DDS)program.texturestats[1];
            obj.ResetVisible();
            ClearErrorRow(dataGridView1.Rows[1]);
            string output;
            int errorrow = 0;
            int errorcol = -1;

            if (f.ShowDialog() == DialogResult.OK)
            {
                program.texturestats[1] = obj = new DDS();

                obj.Filename = f.FileName;
                lastddsdir = Path.GetDirectoryName(f.FileName) + @"\";
                obj.Read(out output, out errorrow, out errorcol);
                obj.ArrayCount = 1;
                if (program.texturestats[2].Filename == Output.defaultfilelabel)
                {
                    var suggestfn = Path.ChangeExtension(f.FileName, ".texture");
                    if (suggestfn != program.texturestats[0].Filename)
                    {
                        program.texturestats[2].Filename = suggestfn;
                        program.texturestats[2].Ready = true;
                        outputlabel.DataBindings.Clear();
                        outputlabel.DataBindings.Add("Text", program.texturestats[2], nameof(TextureBase.Filename));
                    }
                }
                
                if (obj.Filename.ToLower().EndsWith(".a0.dds"))
                {
                    var stub = obj.Filename.Substring(0, obj.Filename.Length - ".a0.dds".Length);
                    for (obj.ArrayCount = 1; obj.ArrayCount < 32; obj.ArrayCount++)
                    {
                        if (!File.Exists($"{stub}.A{obj.ArrayCount}.dds"))
                            break;
                    }
                    output += $"({obj.ArrayCount} textures available)\r\n";
                }
                outputbox.Text = output;
            }

            dataGridView1.Refresh();
            dataGridView1.AutoResizeColumns();
            ddslabel.DataBindings.Clear();
            ddslabel.DataBindings.Add("Text", obj, nameof(TextureBase.Filename));
            MarkError(errorrow, errorcol);
            UpdateControls();
        }

        private void outputbutton_Click(object sender, EventArgs e)
        {
            var f = new SaveFileDialog();
            f.Filter = "Modified texture|*.texture";
            if (lastoutputdir is not null)
                f.InitialDirectory = Path.GetDirectoryName(lastoutputdir);
            var obj = (Output)program.texturestats[2];
            obj.ResetVisible();

            if (f.ShowDialog() == DialogResult.OK)
            {
                lastoutputdir = Path.GetDirectoryName(f.FileName) + @"\";
                program.texturestats[2] = obj = new Output();
                if (obj.Filename != program.texturestats[0].Filename)
                {
                    obj.Filename = f.FileName;
                    obj.Ready = true;

                    outputbox.Text = "";
                }
                else
                    outputbox.Text = "Output file can't be the same as input file.";
            }

            dataGridView1.Refresh();
            dataGridView1.AutoResizeColumns();
            outputlabel.DataBindings.Clear();
            outputlabel.DataBindings.Add("Text", obj, nameof(TextureBase.Filename));
            UpdateControls();
        }

        private void generatebutton_Click(object sender, EventArgs e)
        {
            outputbox.Text = "";
            string output;
            var obj = (Output)program.texturestats[2];
            var savefn = obj.Filename;
            obj.ResetVisible();
            obj.Filename = savefn;
            ClearErrorRow(dataGridView1.Rows[2]);

            int errorrow;
            int errorcol;

            for (int i = 0; i < 3; i++)
            {
                program.texturestats[i].Ready = false;

                if (program.texturestats[i].Read(out output, out errorrow, out errorcol))
                    program.texturestats[i].Ready = true;

                MarkError(errorrow, errorcol);
                outputbox.Text += output;
            }

            UpdateControls();
            dataGridView1.Refresh();
            if (!generatebutton.Enabled)
                return;

            var ddss = new List<DDS>() { (DDS)(program.texturestats[1]) };
            var stub = ddss[0].Filename.Substring(0, ddss[0].Filename.Length - ".a0.dds".Length);
            for (int i = 1; i < program.texturestats[0].ArrayCount ; i++)
            {
                ddss.Add(new DDS());
                ddss[i].Filename = $"{stub}.A{i}.dds";
                if (!ddss[i].Read(out output, out errorrow, out errorcol))
                {
                    MarkError(errorrow, errorcol);
                    outputbox.Text += output;
                    return;
                }
                outputbox.Text += output;
            }
            ((Output)program.texturestats[2]).Generate(
                (Source)(program.texturestats[0]),
                ddss, 
                testmode.Checked, 
                ignoreformat.Checked ? true : null,
                (uint)extrasdctl.Value,
                out output, out errorrow, out errorcol);

            outputbox.AppendText(output);
            dataGridView1.Refresh();
            dataGridView1.AutoResizeColumns();
            MarkError(errorrow, errorcol);
        }

        private void MarkError(int errorrow, int errorcol)
        {
            if (errorcol < 0)
                return;

            dataGridView1.Rows[errorrow].Cells[errorcol].ErrorText = "Error";
        }

        private void ClearErrors(DataGridView d)
        {
            // seems like there should already be a function for this
            foreach (DataGridViewRow row in d.Rows)
                ClearErrorRow(row);
        }

        private void ClearErrorRow(DataGridViewRow row)
        {
            foreach (DataGridViewCell c in row.Cells)
            {
                c.ErrorText = "";
            }
        }
    }
}
