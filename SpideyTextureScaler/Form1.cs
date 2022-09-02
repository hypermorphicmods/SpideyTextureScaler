using System.Reflection;

namespace SpideyTextureScaler
{
    public partial class Form1 : Form
    {
        Program program;

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
        }

        private void UpdateGenerateButton()
        {
            generatebutton.Enabled = program.texturestats[0].Ready && program.texturestats[1].Ready && program.texturestats[2].Ready;
        }

        private void sourcebutton_Click(object sender, EventArgs e)
        {
            var f = new OpenFileDialog();
            f.Filter = "Low res extracted texture|*.texture";
            var obj = (Source)program.texturestats[0];
            obj.ResetVisible();
            ClearErrorRow(dataGridView1.Rows[0]);
            string output;
            int errorrow = 0;
            int errorcol = -1;

            this.Text = "SpideyTextureScaler";
            saveddsbutton.Enabled = false;
            if (f.ShowDialog() == DialogResult.OK)
            {
                program.texturestats[0] = obj = new Source();

                obj.Filename = f.FileName;
                ddsfilenamelabel.Text = Path.GetFileName(Path.ChangeExtension(obj.Filename, ".dds"));
                if (obj.Read(out output, out errorrow, out errorcol))
                {
                    saveddsbutton.Enabled = true;
                    this.Text = $"{Path.GetFileNameWithoutExtension(Path.GetFileName(obj.Filename))} - SpideyTextureScaler";
                }

                outputbox.Text = output;
            }

            dataGridView1.Refresh();
            dataGridView1.AutoResizeColumns();
            sourcelabel.DataBindings.Clear();
            sourcelabel.DataBindings.Add("Text", obj, nameof(TextureBase.Filename));
            MarkError(errorrow, errorcol);
            UpdateGenerateButton();
        }

        private void saveddsbutton_Click(object sender, EventArgs e)
        {
            var tex = (Source)program.texturestats[0];
            var savedds = new DDS();
            savedds.Filename = Path.ChangeExtension(tex.Filename, ".dds");
            savedds.Mipmaps = tex.Mipmaps;
            savedds.Format = tex.Format;
            savedds.basemipsize = tex.basemipsize;
            byte[] hdmips = null;
            string output;

            savedds.HDMipmaps = 0;
            savedds.Width = tex.Width;
            savedds.Height = tex.Height;
            if (tex.HDSize > 0)
            {
                if (!File.Exists(tex.hdfilename))
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
            f.Filter = "DirectDraw Surface (*.dds)|*.dds";
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
                obj.Read(out output, out errorrow, out errorcol);
                outputbox.Text = output;

            }

            dataGridView1.Refresh();
            dataGridView1.AutoResizeColumns();
            ddslabel.DataBindings.Clear();
            ddslabel.DataBindings.Add("Text", obj, nameof(TextureBase.Filename));
            MarkError(errorrow, errorcol);
            UpdateGenerateButton();
        }

        private void outputbutton_Click(object sender, EventArgs e)
        {
            var f = new SaveFileDialog();
            f.Filter = "High res mod texture|*.texture";
            var obj = (Output)program.texturestats[2];
            obj.ResetVisible();

            if (f.ShowDialog() == DialogResult.OK)
            {
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
            UpdateGenerateButton();
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

            UpdateGenerateButton();
            dataGridView1.Refresh();
            if (!generatebutton.Enabled)
                return;

            ((Output)program.texturestats[2]).Generate(
                (Source)(program.texturestats[0]),
                (DDS)(program.texturestats[1]), 
                testmode.Checked, 
                ignoreformat.Checked,
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
