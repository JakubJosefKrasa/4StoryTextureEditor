
namespace TextureEditor
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.PictureBox1 = new System.Windows.Forms.PictureBox();
            this.TexturesListBox = new System.Windows.Forms.ListBox();
            this.DumpAllTexturesPngButton = new System.Windows.Forms.Button();
            this.SelectFileComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.TexturesInFileCountLabel = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.DeleteUVKeyButton = new System.Windows.Forms.Button();
            this.AddUVKeyButton = new System.Windows.Forms.Button();
            this.SaveUVKeyButton = new System.Windows.Forms.Button();
            this.UVKeysListBox = new System.Windows.Forms.ListBox();
            this.label7 = new System.Windows.Forms.Label();
            this.KeySVTextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.KeySUTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.KeyVTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.KeyRTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.KeyUTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.KeyTickTextBox = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.AddImageButton = new System.Windows.Forms.Button();
            this.TextureImageNotFoundLabel = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.SaveTextureInfoButton = new System.Windows.Forms.Button();
            this.TextureFormatComboBox = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.MipFilterComboBox = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.MipBiasTextBox = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.CurrentTickTextBox = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.TotalTickTextBox = new System.Windows.Forms.TextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.DeleteTextureIdInTexture = new System.Windows.Forms.Button();
            this.AddTextureIdInTexture = new System.Windows.Forms.Button();
            this.SaveTextureIdInTexture = new System.Windows.Forms.Button();
            this.TextureIdsInTextureListBox = new System.Windows.Forms.ListBox();
            this.label13 = new System.Windows.Forms.Label();
            this.TextureIdTextBox = new System.Windows.Forms.TextBox();
            this.AddTextureInFileButton = new System.Windows.Forms.Button();
            this.DeleteTextureFromFileButton = new System.Windows.Forms.Button();
            this.label14 = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.DataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LoadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SearchTextureIdTextBox = new System.Windows.Forms.TextBox();
            this.SearchTextureIdTimer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // PictureBox1
            // 
            this.PictureBox1.BackColor = System.Drawing.SystemColors.Control;
            this.PictureBox1.Location = new System.Drawing.Point(18, 29);
            this.PictureBox1.Name = "PictureBox1";
            this.PictureBox1.Size = new System.Drawing.Size(581, 467);
            this.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.PictureBox1.TabIndex = 1;
            this.PictureBox1.TabStop = false;
            // 
            // TexturesListBox
            // 
            this.TexturesListBox.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.TexturesListBox.FormattingEnabled = true;
            this.TexturesListBox.IntegralHeight = false;
            this.TexturesListBox.ItemHeight = 15;
            this.TexturesListBox.Location = new System.Drawing.Point(8, 86);
            this.TexturesListBox.Name = "TexturesListBox";
            this.TexturesListBox.Size = new System.Drawing.Size(186, 447);
            this.TexturesListBox.TabIndex = 2;
            this.TexturesListBox.SelectedIndexChanged += new System.EventHandler(this.TexturesListBox_SelectedIndexChanged);
            // 
            // DumpAllTexturesPngButton
            // 
            this.DumpAllTexturesPngButton.Enabled = false;
            this.DumpAllTexturesPngButton.Location = new System.Drawing.Point(3, 668);
            this.DumpAllTexturesPngButton.Name = "DumpAllTexturesPngButton";
            this.DumpAllTexturesPngButton.Size = new System.Drawing.Size(191, 24);
            this.DumpAllTexturesPngButton.TabIndex = 3;
            this.DumpAllTexturesPngButton.Text = "Dump All Textures (PNG)";
            this.DumpAllTexturesPngButton.UseVisualStyleBackColor = true;
            this.DumpAllTexturesPngButton.Click += new System.EventHandler(this.DumpAllTexturesPngButton_Click);
            // 
            // SelectFileComboBox
            // 
            this.SelectFileComboBox.Enabled = false;
            this.SelectFileComboBox.FormattingEnabled = true;
            this.SelectFileComboBox.Location = new System.Drawing.Point(73, 33);
            this.SelectFileComboBox.Name = "SelectFileComboBox";
            this.SelectFileComboBox.Size = new System.Drawing.Size(121, 21);
            this.SelectFileComboBox.TabIndex = 4;
            this.SelectFileComboBox.SelectedIndexChanged += new System.EventHandler(this.SelectFileComboBox_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Texture File";
            // 
            // TexturesInFileCountLabel
            // 
            this.TexturesInFileCountLabel.AutoSize = true;
            this.TexturesInFileCountLabel.Location = new System.Drawing.Point(17, 536);
            this.TexturesInFileCountLabel.Name = "TexturesInFileCountLabel";
            this.TexturesInFileCountLabel.Size = new System.Drawing.Size(81, 13);
            this.TexturesInFileCountLabel.TabIndex = 6;
            this.TexturesInFileCountLabel.Text = "Textures in file: ";
            this.TexturesInFileCountLabel.Visible = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.DeleteUVKeyButton);
            this.groupBox1.Controls.Add(this.AddUVKeyButton);
            this.groupBox1.Controls.Add(this.SaveUVKeyButton);
            this.groupBox1.Controls.Add(this.UVKeysListBox);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.KeySVTextBox);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.KeySUTextBox);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.KeyVTextBox);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.KeyRTextBox);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.KeyUTextBox);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.KeyTickTextBox);
            this.groupBox1.Location = new System.Drawing.Point(1258, 60);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(494, 276);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            // 
            // DeleteUVKeyButton
            // 
            this.DeleteUVKeyButton.Location = new System.Drawing.Point(251, 166);
            this.DeleteUVKeyButton.Name = "DeleteUVKeyButton";
            this.DeleteUVKeyButton.Size = new System.Drawing.Size(216, 23);
            this.DeleteUVKeyButton.TabIndex = 22;
            this.DeleteUVKeyButton.Text = "Delete";
            this.DeleteUVKeyButton.UseVisualStyleBackColor = true;
            this.DeleteUVKeyButton.Click += new System.EventHandler(this.DeleteUVKeyButton_Click);
            // 
            // AddUVKeyButton
            // 
            this.AddUVKeyButton.Location = new System.Drawing.Point(251, 137);
            this.AddUVKeyButton.Name = "AddUVKeyButton";
            this.AddUVKeyButton.Size = new System.Drawing.Size(216, 23);
            this.AddUVKeyButton.TabIndex = 21;
            this.AddUVKeyButton.Text = "Add";
            this.AddUVKeyButton.UseVisualStyleBackColor = true;
            this.AddUVKeyButton.Click += new System.EventHandler(this.AddUVKeyButton_Click);
            // 
            // SaveUVKeyButton
            // 
            this.SaveUVKeyButton.Location = new System.Drawing.Point(66, 213);
            this.SaveUVKeyButton.Name = "SaveUVKeyButton";
            this.SaveUVKeyButton.Size = new System.Drawing.Size(119, 23);
            this.SaveUVKeyButton.TabIndex = 20;
            this.SaveUVKeyButton.Text = "Save";
            this.SaveUVKeyButton.UseVisualStyleBackColor = true;
            this.SaveUVKeyButton.Click += new System.EventHandler(this.SaveUVKeyButton_Click);
            // 
            // UVKeysListBox
            // 
            this.UVKeysListBox.FormattingEnabled = true;
            this.UVKeysListBox.Location = new System.Drawing.Point(251, 34);
            this.UVKeysListBox.Name = "UVKeysListBox";
            this.UVKeysListBox.Size = new System.Drawing.Size(216, 95);
            this.UVKeysListBox.TabIndex = 12;
            this.UVKeysListBox.SelectedIndexChanged += new System.EventHandler(this.UVKeysListBox_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(17, 168);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(42, 13);
            this.label7.TabIndex = 11;
            this.label7.Text = "Key SV";
            // 
            // KeySVTextBox
            // 
            this.KeySVTextBox.Location = new System.Drawing.Point(66, 165);
            this.KeySVTextBox.Name = "KeySVTextBox";
            this.KeySVTextBox.Size = new System.Drawing.Size(119, 20);
            this.KeySVTextBox.TabIndex = 10;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(17, 142);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(43, 13);
            this.label6.TabIndex = 9;
            this.label6.Text = "Key SU";
            // 
            // KeySUTextBox
            // 
            this.KeySUTextBox.Location = new System.Drawing.Point(66, 139);
            this.KeySUTextBox.Name = "KeySUTextBox";
            this.KeySUTextBox.Size = new System.Drawing.Size(119, 20);
            this.KeySUTextBox.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 90);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Key V";
            // 
            // KeyVTextBox
            // 
            this.KeyVTextBox.Location = new System.Drawing.Point(66, 87);
            this.KeyVTextBox.Name = "KeyVTextBox";
            this.KeyVTextBox.Size = new System.Drawing.Size(119, 20);
            this.KeyVTextBox.TabIndex = 6;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(17, 116);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(36, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Key R";
            // 
            // KeyRTextBox
            // 
            this.KeyRTextBox.Location = new System.Drawing.Point(66, 113);
            this.KeyRTextBox.Name = "KeyRTextBox";
            this.KeyRTextBox.Size = new System.Drawing.Size(119, 20);
            this.KeyRTextBox.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 64);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Key U";
            // 
            // KeyUTextBox
            // 
            this.KeyUTextBox.Location = new System.Drawing.Point(66, 61);
            this.KeyUTextBox.Name = "KeyUTextBox";
            this.KeyUTextBox.Size = new System.Drawing.Size(119, 20);
            this.KeyUTextBox.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(28, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Tick";
            // 
            // KeyTickTextBox
            // 
            this.KeyTickTextBox.Location = new System.Drawing.Point(66, 35);
            this.KeyTickTextBox.Name = "KeyTickTextBox";
            this.KeyTickTextBox.Size = new System.Drawing.Size(119, 20);
            this.KeyTickTextBox.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.AddImageButton);
            this.groupBox2.Controls.Add(this.TextureImageNotFoundLabel);
            this.groupBox2.Controls.Add(this.PictureBox1);
            this.groupBox2.Location = new System.Drawing.Point(556, 60);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(676, 573);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Texture Image";
            // 
            // AddImageButton
            // 
            this.AddImageButton.Location = new System.Drawing.Point(534, 544);
            this.AddImageButton.Name = "AddImageButton";
            this.AddImageButton.Size = new System.Drawing.Size(126, 23);
            this.AddImageButton.TabIndex = 14;
            this.AddImageButton.Text = "Add image";
            this.AddImageButton.UseVisualStyleBackColor = true;
            this.AddImageButton.Click += new System.EventHandler(this.AddImageButton_Click);
            // 
            // TextureImageNotFoundLabel
            // 
            this.TextureImageNotFoundLabel.AutoSize = true;
            this.TextureImageNotFoundLabel.Font = new System.Drawing.Font("Arial Black", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.TextureImageNotFoundLabel.ForeColor = System.Drawing.Color.Red;
            this.TextureImageNotFoundLabel.Location = new System.Drawing.Point(25, 515);
            this.TextureImageNotFoundLabel.Name = "TextureImageNotFoundLabel";
            this.TextureImageNotFoundLabel.Size = new System.Drawing.Size(317, 27);
            this.TextureImageNotFoundLabel.TabIndex = 13;
            this.TextureImageNotFoundLabel.Text = "TextureImageNotFoundLabel";
            this.TextureImageNotFoundLabel.Visible = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.SaveTextureInfoButton);
            this.groupBox3.Controls.Add(this.TextureFormatComboBox);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.MipFilterComboBox);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.MipBiasTextBox);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.CurrentTickTextBox);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.TotalTickTextBox);
            this.groupBox3.Location = new System.Drawing.Point(236, 485);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(299, 216);
            this.groupBox3.TabIndex = 9;
            this.groupBox3.TabStop = false;
            // 
            // SaveTextureInfoButton
            // 
            this.SaveTextureInfoButton.Location = new System.Drawing.Point(88, 171);
            this.SaveTextureInfoButton.Name = "SaveTextureInfoButton";
            this.SaveTextureInfoButton.Size = new System.Drawing.Size(182, 23);
            this.SaveTextureInfoButton.TabIndex = 19;
            this.SaveTextureInfoButton.Text = "Save";
            this.SaveTextureInfoButton.UseVisualStyleBackColor = true;
            this.SaveTextureInfoButton.Click += new System.EventHandler(this.SaveTextureInfoButton_Click);
            // 
            // TextureFormatComboBox
            // 
            this.TextureFormatComboBox.FormattingEnabled = true;
            this.TextureFormatComboBox.Items.AddRange(new object[] {
            "D3DTEXF_NONE"});
            this.TextureFormatComboBox.Location = new System.Drawing.Point(88, 127);
            this.TextureFormatComboBox.Name = "TextureFormatComboBox";
            this.TextureFormatComboBox.Size = new System.Drawing.Size(182, 21);
            this.TextureFormatComboBox.TabIndex = 18;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(17, 130);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(39, 13);
            this.label12.TabIndex = 17;
            this.label12.Text = "Format";
            // 
            // MipFilterComboBox
            // 
            this.MipFilterComboBox.FormattingEnabled = true;
            this.MipFilterComboBox.Items.AddRange(new object[] {
            "D3DTEXF_NONE"});
            this.MipFilterComboBox.Location = new System.Drawing.Point(88, 100);
            this.MipFilterComboBox.Name = "MipFilterComboBox";
            this.MipFilterComboBox.Size = new System.Drawing.Size(182, 21);
            this.MipFilterComboBox.TabIndex = 16;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(17, 74);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(47, 13);
            this.label8.TabIndex = 15;
            this.label8.Text = "Mip Bias";
            // 
            // MipBiasTextBox
            // 
            this.MipBiasTextBox.Location = new System.Drawing.Point(88, 71);
            this.MipBiasTextBox.Name = "MipBiasTextBox";
            this.MipBiasTextBox.Size = new System.Drawing.Size(119, 20);
            this.MipBiasTextBox.TabIndex = 14;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(17, 103);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(49, 13);
            this.label9.TabIndex = 13;
            this.label9.Text = "Mip Filter";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(17, 48);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(65, 13);
            this.label10.TabIndex = 11;
            this.label10.Text = "Current Tick";
            // 
            // CurrentTickTextBox
            // 
            this.CurrentTickTextBox.Location = new System.Drawing.Point(88, 45);
            this.CurrentTickTextBox.Name = "CurrentTickTextBox";
            this.CurrentTickTextBox.Size = new System.Drawing.Size(119, 20);
            this.CurrentTickTextBox.TabIndex = 10;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(17, 22);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(55, 13);
            this.label11.TabIndex = 9;
            this.label11.Text = "Total Tick";
            // 
            // TotalTickTextBox
            // 
            this.TotalTickTextBox.Location = new System.Drawing.Point(88, 19);
            this.TotalTickTextBox.Name = "TotalTickTextBox";
            this.TotalTickTextBox.Size = new System.Drawing.Size(119, 20);
            this.TotalTickTextBox.TabIndex = 8;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.DeleteTextureIdInTexture);
            this.groupBox4.Controls.Add(this.AddTextureIdInTexture);
            this.groupBox4.Controls.Add(this.SaveTextureIdInTexture);
            this.groupBox4.Controls.Add(this.TextureIdsInTextureListBox);
            this.groupBox4.Controls.Add(this.label13);
            this.groupBox4.Controls.Add(this.TextureIdTextBox);
            this.groupBox4.Location = new System.Drawing.Point(236, 60);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(299, 422);
            this.groupBox4.TabIndex = 10;
            this.groupBox4.TabStop = false;
            // 
            // DeleteTextureIdInTexture
            // 
            this.DeleteTextureIdInTexture.Location = new System.Drawing.Point(38, 335);
            this.DeleteTextureIdInTexture.Name = "DeleteTextureIdInTexture";
            this.DeleteTextureIdInTexture.Size = new System.Drawing.Size(209, 23);
            this.DeleteTextureIdInTexture.TabIndex = 22;
            this.DeleteTextureIdInTexture.Text = "Delete";
            this.DeleteTextureIdInTexture.UseVisualStyleBackColor = true;
            this.DeleteTextureIdInTexture.Click += new System.EventHandler(this.DeleteTextureIdInTexture_Click);
            // 
            // AddTextureIdInTexture
            // 
            this.AddTextureIdInTexture.Location = new System.Drawing.Point(38, 306);
            this.AddTextureIdInTexture.Name = "AddTextureIdInTexture";
            this.AddTextureIdInTexture.Size = new System.Drawing.Size(209, 23);
            this.AddTextureIdInTexture.TabIndex = 21;
            this.AddTextureIdInTexture.Text = "Add";
            this.AddTextureIdInTexture.UseVisualStyleBackColor = true;
            this.AddTextureIdInTexture.Click += new System.EventHandler(this.AddTextureIdInTexture_Click);
            // 
            // SaveTextureIdInTexture
            // 
            this.SaveTextureIdInTexture.Location = new System.Drawing.Point(38, 364);
            this.SaveTextureIdInTexture.Name = "SaveTextureIdInTexture";
            this.SaveTextureIdInTexture.Size = new System.Drawing.Size(209, 23);
            this.SaveTextureIdInTexture.TabIndex = 20;
            this.SaveTextureIdInTexture.Text = "Save";
            this.SaveTextureIdInTexture.UseVisualStyleBackColor = true;
            this.SaveTextureIdInTexture.Click += new System.EventHandler(this.SaveTextureIdInTexture_Click);
            // 
            // TextureIdsInTextureListBox
            // 
            this.TextureIdsInTextureListBox.FormattingEnabled = true;
            this.TextureIdsInTextureListBox.Location = new System.Drawing.Point(38, 62);
            this.TextureIdsInTextureListBox.Name = "TextureIdsInTextureListBox";
            this.TextureIdsInTextureListBox.Size = new System.Drawing.Size(209, 238);
            this.TextureIdsInTextureListBox.TabIndex = 4;
            this.TextureIdsInTextureListBox.SelectedIndexChanged += new System.EventHandler(this.TextureIdsInTextureListBox_SelectedIndexChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(35, 26);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(57, 13);
            this.label13.TabIndex = 3;
            this.label13.Text = "Texture ID";
            // 
            // TextureIdTextBox
            // 
            this.TextureIdTextBox.Location = new System.Drawing.Point(98, 23);
            this.TextureIdTextBox.Name = "TextureIdTextBox";
            this.TextureIdTextBox.Size = new System.Drawing.Size(149, 20);
            this.TextureIdTextBox.TabIndex = 2;
            // 
            // AddTextureInFileButton
            // 
            this.AddTextureInFileButton.Location = new System.Drawing.Point(3, 559);
            this.AddTextureInFileButton.Name = "AddTextureInFileButton";
            this.AddTextureInFileButton.Size = new System.Drawing.Size(191, 23);
            this.AddTextureInFileButton.TabIndex = 22;
            this.AddTextureInFileButton.Text = "Add";
            this.AddTextureInFileButton.UseVisualStyleBackColor = true;
            this.AddTextureInFileButton.Click += new System.EventHandler(this.AddTextureInFileButton_Click);
            // 
            // DeleteTextureFromFileButton
            // 
            this.DeleteTextureFromFileButton.Location = new System.Drawing.Point(3, 588);
            this.DeleteTextureFromFileButton.Name = "DeleteTextureFromFileButton";
            this.DeleteTextureFromFileButton.Size = new System.Drawing.Size(191, 23);
            this.DeleteTextureFromFileButton.TabIndex = 23;
            this.DeleteTextureFromFileButton.Text = "Delete";
            this.DeleteTextureFromFileButton.UseVisualStyleBackColor = true;
            this.DeleteTextureFromFileButton.Click += new System.EventHandler(this.DeleteTextureFromFileButton_Click);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(1667, 706);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(85, 13);
            this.label14.TabIndex = 25;
            this.label14.Text = "Created by pizza";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.DataToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1794, 24);
            this.menuStrip1.TabIndex = 26;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // DataToolStripMenuItem
            // 
            this.DataToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LoadToolStripMenuItem,
            this.SaveToolStripMenuItem});
            this.DataToolStripMenuItem.Name = "DataToolStripMenuItem";
            this.DataToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.DataToolStripMenuItem.Text = "Data";
            // 
            // LoadToolStripMenuItem
            // 
            this.LoadToolStripMenuItem.Name = "LoadToolStripMenuItem";
            this.LoadToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.LoadToolStripMenuItem.Text = "Load";
            this.LoadToolStripMenuItem.Click += new System.EventHandler(this.LoadToolStripMenuItem1_Click);
            // 
            // SaveToolStripMenuItem
            // 
            this.SaveToolStripMenuItem.Name = "SaveToolStripMenuItem";
            this.SaveToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.SaveToolStripMenuItem.Text = "Save";
            this.SaveToolStripMenuItem.Click += new System.EventHandler(this.SaveToolStripMenuItem_Click);
            // 
            // SearchTextureIdTextBox
            // 
            this.SearchTextureIdTextBox.Location = new System.Drawing.Point(8, 60);
            this.SearchTextureIdTextBox.Name = "SearchTextureIdTextBox";
            this.SearchTextureIdTextBox.Size = new System.Drawing.Size(186, 20);
            this.SearchTextureIdTextBox.TabIndex = 27;
            this.SearchTextureIdTextBox.TextChanged += new System.EventHandler(this.SearchTextureIdTextBox_TextChanged);
            // 
            // SearchTextureIdTimer
            // 
            this.SearchTextureIdTimer.Interval = 500;
            this.SearchTextureIdTimer.Tick += new System.EventHandler(this.SearchTextureIdTimer_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1794, 728);
            this.Controls.Add(this.SearchTextureIdTextBox);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.DeleteTextureFromFileButton);
            this.Controls.Add(this.AddTextureInFileButton);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.TexturesInFileCountLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.SelectFileComboBox);
            this.Controls.Add(this.DumpAllTexturesPngButton);
            this.Controls.Add(this.TexturesListBox);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "4Story Texture Editor by pizza";
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox PictureBox1;
        private System.Windows.Forms.ListBox TexturesListBox;
        private System.Windows.Forms.Button DumpAllTexturesPngButton;
        private System.Windows.Forms.ComboBox SelectFileComboBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label TexturesInFileCountLabel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox KeySVTextBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox KeySUTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox KeyVTextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox KeyRTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox KeyUTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox KeyTickTextBox;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox MipFilterComboBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox MipBiasTextBox;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox CurrentTickTextBox;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox TotalTickTextBox;
        private System.Windows.Forms.ListBox UVKeysListBox;
        private System.Windows.Forms.Label TextureImageNotFoundLabel;
        private System.Windows.Forms.ComboBox TextureFormatComboBox;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button DeleteUVKeyButton;
        private System.Windows.Forms.Button AddUVKeyButton;
        private System.Windows.Forms.Button SaveUVKeyButton;
        private System.Windows.Forms.Button SaveTextureInfoButton;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button DeleteTextureIdInTexture;
        private System.Windows.Forms.Button AddTextureIdInTexture;
        private System.Windows.Forms.Button SaveTextureIdInTexture;
        private System.Windows.Forms.ListBox TextureIdsInTextureListBox;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox TextureIdTextBox;
        private System.Windows.Forms.Button AddTextureInFileButton;
        private System.Windows.Forms.Button DeleteTextureFromFileButton;
        private System.Windows.Forms.Button AddImageButton;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem DataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem LoadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SaveToolStripMenuItem;
        private System.Windows.Forms.TextBox SearchTextureIdTextBox;
        private System.Windows.Forms.Timer SearchTextureIdTimer;
    }
}

