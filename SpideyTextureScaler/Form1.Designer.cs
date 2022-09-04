namespace SpideyTextureScaler
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.sourcebutton = new System.Windows.Forms.Button();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.ddsbutton = new System.Windows.Forms.Button();
            this.outputbutton = new System.Windows.Forms.Button();
            this.generatebutton = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.readyDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.widthDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.heightDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mipmapsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.hDMipmapsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Images = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BytesPerPixel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sizeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.hDSizeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.formatDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.texturestatsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.statusgroup = new System.Windows.Forms.GroupBox();
            this.outputbox = new System.Windows.Forms.TextBox();
            this.sourcelabel = new System.Windows.Forms.Label();
            this.ddslabel = new System.Windows.Forms.Label();
            this.outputlabel = new System.Windows.Forms.Label();
            this.testmode = new System.Windows.Forms.CheckBox();
            this.saveddsbutton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ddsfilenamelabel = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.ignoreformat = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.texturestatsBindingSource)).BeginInit();
            this.statusgroup.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // sourcebutton
            // 
            this.sourcebutton.Location = new System.Drawing.Point(24, 40);
            this.sourcebutton.Name = "sourcebutton";
            this.sourcebutton.Size = new System.Drawing.Size(272, 48);
            this.sourcebutton.TabIndex = 0;
            this.sourcebutton.Text = "Import original .texture";
            this.sourcebutton.UseVisualStyleBackColor = true;
            this.sourcebutton.Click += new System.EventHandler(this.sourcebutton_Click);
            // 
            // bindingSource1
            // 
            this.bindingSource1.DataSource = typeof(SpideyTextureScaler.Program);
            // 
            // ddsbutton
            // 
            this.ddsbutton.Location = new System.Drawing.Point(24, 40);
            this.ddsbutton.Name = "ddsbutton";
            this.ddsbutton.Size = new System.Drawing.Size(272, 48);
            this.ddsbutton.TabIndex = 2;
            this.ddsbutton.Text = "Choose modded .dds texture";
            this.ddsbutton.UseVisualStyleBackColor = true;
            this.ddsbutton.Click += new System.EventHandler(this.ddsbutton_Click);
            // 
            // outputbutton
            // 
            this.outputbutton.Location = new System.Drawing.Point(24, 34);
            this.outputbutton.Name = "outputbutton";
            this.outputbutton.Size = new System.Drawing.Size(272, 48);
            this.outputbutton.TabIndex = 4;
            this.outputbutton.Text = "Choose output filename";
            this.outputbutton.UseVisualStyleBackColor = true;
            this.outputbutton.Click += new System.EventHandler(this.outputbutton_Click);
            // 
            // generatebutton
            // 
            this.generatebutton.Enabled = false;
            this.generatebutton.Location = new System.Drawing.Point(88, 112);
            this.generatebutton.Name = "generatebutton";
            this.generatebutton.Size = new System.Drawing.Size(208, 80);
            this.generatebutton.TabIndex = 6;
            this.generatebutton.Text = "Generate";
            this.generatebutton.UseVisualStyleBackColor = true;
            this.generatebutton.Click += new System.EventHandler(this.generatebutton_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.nameDataGridViewTextBoxColumn,
            this.readyDataGridViewCheckBoxColumn,
            this.widthDataGridViewTextBoxColumn,
            this.heightDataGridViewTextBoxColumn,
            this.mipmapsDataGridViewTextBoxColumn,
            this.hDMipmapsDataGridViewTextBoxColumn,
            this.Images,
            this.BytesPerPixel,
            this.sizeDataGridViewTextBoxColumn,
            this.hDSizeDataGridViewTextBoxColumn,
            this.formatDataGridViewTextBoxColumn});
            this.dataGridView1.DataSource = this.texturestatsBindingSource;
            this.dataGridView1.EnableHeadersVisualStyles = false;
            this.dataGridView1.Location = new System.Drawing.Point(24, 680);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowHeadersWidth = 72;
            this.dataGridView1.RowTemplate.Height = 37;
            this.dataGridView1.ShowEditingIcon = false;
            this.dataGridView1.Size = new System.Drawing.Size(1440, 192);
            this.dataGridView1.TabIndex = 7;
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn.HeaderText = "";
            this.nameDataGridViewTextBoxColumn.MinimumWidth = 9;
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            this.nameDataGridViewTextBoxColumn.ReadOnly = true;
            this.nameDataGridViewTextBoxColumn.Width = 33;
            // 
            // readyDataGridViewCheckBoxColumn
            // 
            this.readyDataGridViewCheckBoxColumn.DataPropertyName = "Ready";
            this.readyDataGridViewCheckBoxColumn.HeaderText = "Ready";
            this.readyDataGridViewCheckBoxColumn.MinimumWidth = 9;
            this.readyDataGridViewCheckBoxColumn.Name = "readyDataGridViewCheckBoxColumn";
            this.readyDataGridViewCheckBoxColumn.ReadOnly = true;
            this.readyDataGridViewCheckBoxColumn.Width = 75;
            // 
            // widthDataGridViewTextBoxColumn
            // 
            this.widthDataGridViewTextBoxColumn.DataPropertyName = "Width";
            this.widthDataGridViewTextBoxColumn.HeaderText = "Width";
            this.widthDataGridViewTextBoxColumn.MinimumWidth = 9;
            this.widthDataGridViewTextBoxColumn.Name = "widthDataGridViewTextBoxColumn";
            this.widthDataGridViewTextBoxColumn.ReadOnly = true;
            this.widthDataGridViewTextBoxColumn.Width = 110;
            // 
            // heightDataGridViewTextBoxColumn
            // 
            this.heightDataGridViewTextBoxColumn.DataPropertyName = "Height";
            this.heightDataGridViewTextBoxColumn.HeaderText = "Height";
            this.heightDataGridViewTextBoxColumn.MinimumWidth = 9;
            this.heightDataGridViewTextBoxColumn.Name = "heightDataGridViewTextBoxColumn";
            this.heightDataGridViewTextBoxColumn.ReadOnly = true;
            this.heightDataGridViewTextBoxColumn.Width = 116;
            // 
            // mipmapsDataGridViewTextBoxColumn
            // 
            this.mipmapsDataGridViewTextBoxColumn.DataPropertyName = "Mipmaps";
            this.mipmapsDataGridViewTextBoxColumn.HeaderText = "Mipmaps";
            this.mipmapsDataGridViewTextBoxColumn.MinimumWidth = 9;
            this.mipmapsDataGridViewTextBoxColumn.Name = "mipmapsDataGridViewTextBoxColumn";
            this.mipmapsDataGridViewTextBoxColumn.ReadOnly = true;
            this.mipmapsDataGridViewTextBoxColumn.Width = 140;
            // 
            // hDMipmapsDataGridViewTextBoxColumn
            // 
            this.hDMipmapsDataGridViewTextBoxColumn.DataPropertyName = "HDMipmaps";
            this.hDMipmapsDataGridViewTextBoxColumn.HeaderText = "HDMipmaps";
            this.hDMipmapsDataGridViewTextBoxColumn.MinimumWidth = 9;
            this.hDMipmapsDataGridViewTextBoxColumn.Name = "hDMipmapsDataGridViewTextBoxColumn";
            this.hDMipmapsDataGridViewTextBoxColumn.ReadOnly = true;
            this.hDMipmapsDataGridViewTextBoxColumn.Width = 170;
            // 
            // Images
            // 
            this.Images.DataPropertyName = "Images";
            this.Images.HeaderText = "Images";
            this.Images.MinimumWidth = 9;
            this.Images.Name = "Images";
            this.Images.ReadOnly = true;
            this.Images.Width = 121;
            // 
            // BytesPerPixel
            // 
            this.BytesPerPixel.DataPropertyName = "BytesPerPixel";
            this.BytesPerPixel.HeaderText = "BytesPerPixel";
            this.BytesPerPixel.MinimumWidth = 9;
            this.BytesPerPixel.Name = "BytesPerPixel";
            this.BytesPerPixel.ReadOnly = true;
            this.BytesPerPixel.Width = 175;
            // 
            // sizeDataGridViewTextBoxColumn
            // 
            this.sizeDataGridViewTextBoxColumn.DataPropertyName = "Size";
            this.sizeDataGridViewTextBoxColumn.HeaderText = "Size";
            this.sizeDataGridViewTextBoxColumn.MinimumWidth = 9;
            this.sizeDataGridViewTextBoxColumn.Name = "sizeDataGridViewTextBoxColumn";
            this.sizeDataGridViewTextBoxColumn.ReadOnly = true;
            this.sizeDataGridViewTextBoxColumn.Width = 91;
            // 
            // hDSizeDataGridViewTextBoxColumn
            // 
            this.hDSizeDataGridViewTextBoxColumn.DataPropertyName = "HDSize";
            this.hDSizeDataGridViewTextBoxColumn.HeaderText = "HDSize";
            this.hDSizeDataGridViewTextBoxColumn.MinimumWidth = 9;
            this.hDSizeDataGridViewTextBoxColumn.Name = "hDSizeDataGridViewTextBoxColumn";
            this.hDSizeDataGridViewTextBoxColumn.ReadOnly = true;
            this.hDSizeDataGridViewTextBoxColumn.Width = 121;
            // 
            // formatDataGridViewTextBoxColumn
            // 
            this.formatDataGridViewTextBoxColumn.DataPropertyName = "Format";
            this.formatDataGridViewTextBoxColumn.HeaderText = "Format";
            this.formatDataGridViewTextBoxColumn.MinimumWidth = 9;
            this.formatDataGridViewTextBoxColumn.Name = "formatDataGridViewTextBoxColumn";
            this.formatDataGridViewTextBoxColumn.ReadOnly = true;
            this.formatDataGridViewTextBoxColumn.Width = 119;
            // 
            // texturestatsBindingSource
            // 
            this.texturestatsBindingSource.DataMember = "texturestats";
            this.texturestatsBindingSource.DataSource = this.bindingSource1;
            // 
            // statusgroup
            // 
            this.statusgroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.statusgroup.Controls.Add(this.outputbox);
            this.statusgroup.Location = new System.Drawing.Point(376, 88);
            this.statusgroup.Name = "statusgroup";
            this.statusgroup.Size = new System.Drawing.Size(1048, 224);
            this.statusgroup.TabIndex = 8;
            this.statusgroup.TabStop = false;
            this.statusgroup.Text = "Log";
            // 
            // outputbox
            // 
            this.outputbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.outputbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.outputbox.Location = new System.Drawing.Point(16, 40);
            this.outputbox.Multiline = true;
            this.outputbox.Name = "outputbox";
            this.outputbox.ReadOnly = true;
            this.outputbox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.outputbox.Size = new System.Drawing.Size(1016, 160);
            this.outputbox.TabIndex = 0;
            // 
            // sourcelabel
            // 
            this.sourcelabel.AutoSize = true;
            this.sourcelabel.Location = new System.Drawing.Point(376, 45);
            this.sourcelabel.Name = "sourcelabel";
            this.sourcelabel.Size = new System.Drawing.Size(148, 30);
            this.sourcelabel.TabIndex = 9;
            this.sourcelabel.Text = "Choose a file...";
            // 
            // ddslabel
            // 
            this.ddslabel.AutoSize = true;
            this.ddslabel.Location = new System.Drawing.Point(376, 45);
            this.ddslabel.Name = "ddslabel";
            this.ddslabel.Size = new System.Drawing.Size(148, 30);
            this.ddslabel.TabIndex = 9;
            this.ddslabel.Text = "Choose a file...";
            // 
            // outputlabel
            // 
            this.outputlabel.AutoSize = true;
            this.outputlabel.Location = new System.Drawing.Point(376, 40);
            this.outputlabel.Name = "outputlabel";
            this.outputlabel.Size = new System.Drawing.Size(148, 30);
            this.outputlabel.TabIndex = 9;
            this.outputlabel.Text = "Choose a file...";
            // 
            // testmode
            // 
            this.testmode.AutoSize = true;
            this.testmode.Location = new System.Drawing.Point(24, 224);
            this.testmode.Name = "testmode";
            this.testmode.Size = new System.Drawing.Size(337, 34);
            this.testmode.TabIndex = 10;
            this.testmode.Text = "Test (don\'t replace SD mipmaps)";
            this.testmode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.testmode.UseVisualStyleBackColor = true;
            // 
            // saveddsbutton
            // 
            this.saveddsbutton.Enabled = false;
            this.saveddsbutton.Location = new System.Drawing.Point(88, 104);
            this.saveddsbutton.Name = "saveddsbutton";
            this.saveddsbutton.Size = new System.Drawing.Size(208, 48);
            this.saveddsbutton.TabIndex = 0;
            this.saveddsbutton.Text = "Save as .dds";
            this.saveddsbutton.UseVisualStyleBackColor = true;
            this.saveddsbutton.Click += new System.EventHandler(this.saveddsbutton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.ddsfilenamelabel);
            this.groupBox1.Controls.Add(this.sourcelabel);
            this.groupBox1.Controls.Add(this.saveddsbutton);
            this.groupBox1.Controls.Add(this.sourcebutton);
            this.groupBox1.Location = new System.Drawing.Point(24, 24);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1440, 168);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Unmodified Source";
            // 
            // ddsfilenamelabel
            // 
            this.ddsfilenamelabel.AutoSize = true;
            this.ddsfilenamelabel.Location = new System.Drawing.Point(376, 113);
            this.ddsfilenamelabel.Name = "ddsfilenamelabel";
            this.ddsfilenamelabel.Size = new System.Drawing.Size(0, 30);
            this.ddsfilenamelabel.TabIndex = 9;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.ddslabel);
            this.groupBox2.Controls.Add(this.ddsbutton);
            this.groupBox2.Location = new System.Drawing.Point(24, 200);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1440, 120);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Modified texture";
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.ignoreformat);
            this.groupBox3.Controls.Add(this.testmode);
            this.groupBox3.Controls.Add(this.outputlabel);
            this.groupBox3.Controls.Add(this.statusgroup);
            this.groupBox3.Controls.Add(this.generatebutton);
            this.groupBox3.Controls.Add(this.outputbutton);
            this.groupBox3.Location = new System.Drawing.Point(24, 328);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(1440, 336);
            this.groupBox3.TabIndex = 13;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Modified Output";
            // 
            // ignoreformat
            // 
            this.ignoreformat.AutoSize = true;
            this.ignoreformat.Location = new System.Drawing.Point(24, 263);
            this.ignoreformat.Name = "ignoreformat";
            this.ignoreformat.Size = new System.Drawing.Size(269, 34);
            this.ignoreformat.TabIndex = 10;
            this.ignoreformat.Text = "Ignore Format difference";
            this.ignoreformat.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ignoreformat.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 30F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1484, 899);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.dataGridView1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "SpideyTextureScaler";
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.texturestatsBindingSource)).EndInit();
            this.statusgroup.ResumeLayout(false);
            this.statusgroup.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Button sourcebutton;
        private Button ddsbutton;
        private Button outputbutton;
        private Button generatebutton;
        private BindingSource bindingSource1;
        private DataGridView dataGridView1;
        private BindingSource texturestatsBindingSource;
        private GroupBox statusgroup;
        private TextBox outputbox;
        private Label sourcelabel;
        private Label ddslabel;
        private Label outputlabel;
        private CheckBox testmode;
        private Button saveddsbutton;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private GroupBox groupBox3;
        private Label ddsfilenamelabel;
        private CheckBox ignoreformat;
        private DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private DataGridViewCheckBoxColumn readyDataGridViewCheckBoxColumn;
        private DataGridViewTextBoxColumn widthDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn heightDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn mipmapsDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn hDMipmapsDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn Images;
        private DataGridViewTextBoxColumn BytesPerPixel;
        private DataGridViewTextBoxColumn sizeDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn hDSizeDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn formatDataGridViewTextBoxColumn;
    }
}
