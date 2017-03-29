namespace TemplatePrinter
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblSourceImgInfo = new System.Windows.Forms.Label();
            this.pbxSourceImage = new System.Windows.Forms.PictureBox();
            this.btnOpenImg = new System.Windows.Forms.Button();
            this.txtSourceImgPath = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblPageSize = new System.Windows.Forms.Label();
            this.cboPageSizes = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cboPrinters = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.chkLandscape = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbxSourceImage)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblSourceImgInfo);
            this.groupBox1.Controls.Add(this.pbxSourceImage);
            this.groupBox1.Controls.Add(this.btnOpenImg);
            this.groupBox1.Controls.Add(this.txtSourceImgPath);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(270, 288);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Template Image";
            // 
            // lblSourceImgInfo
            // 
            this.lblSourceImgInfo.AutoSize = true;
            this.lblSourceImgInfo.Location = new System.Drawing.Point(6, 239);
            this.lblSourceImgInfo.Name = "lblSourceImgInfo";
            this.lblSourceImgInfo.Size = new System.Drawing.Size(93, 13);
            this.lblSourceImgInfo.TabIndex = 2;
            this.lblSourceImgInfo.Text = "Image Dimensions";
            // 
            // pbxSourceImage
            // 
            this.pbxSourceImage.Location = new System.Drawing.Point(6, 43);
            this.pbxSourceImage.Name = "pbxSourceImage";
            this.pbxSourceImage.Size = new System.Drawing.Size(256, 192);
            this.pbxSourceImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbxSourceImage.TabIndex = 1;
            this.pbxSourceImage.TabStop = false;
            // 
            // btnOpenImg
            // 
            this.btnOpenImg.Location = new System.Drawing.Point(211, 17);
            this.btnOpenImg.Name = "btnOpenImg";
            this.btnOpenImg.Size = new System.Drawing.Size(52, 23);
            this.btnOpenImg.TabIndex = 1;
            this.btnOpenImg.Text = "Browse";
            this.btnOpenImg.UseVisualStyleBackColor = true;
            this.btnOpenImg.Click += new System.EventHandler(this.btnOpenImg_Click);
            // 
            // txtSourceImgPath
            // 
            this.txtSourceImgPath.BackColor = System.Drawing.SystemColors.Window;
            this.txtSourceImgPath.Location = new System.Drawing.Point(6, 18);
            this.txtSourceImgPath.Name = "txtSourceImgPath";
            this.txtSourceImgPath.ReadOnly = true;
            this.txtSourceImgPath.Size = new System.Drawing.Size(202, 20);
            this.txtSourceImgPath.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.radioButton2);
            this.groupBox2.Controls.Add(this.chkLandscape);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Controls.Add(this.lblPageSize);
            this.groupBox2.Controls.Add(this.cboPageSizes);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.cboPrinters);
            this.groupBox2.Location = new System.Drawing.Point(288, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(307, 126);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Printer Config";
            // 
            // lblPageSize
            // 
            this.lblPageSize.AutoSize = true;
            this.lblPageSize.Location = new System.Drawing.Point(3, 64);
            this.lblPageSize.Name = "lblPageSize";
            this.lblPageSize.Size = new System.Drawing.Size(92, 13);
            this.lblPageSize.TabIndex = 3;
            this.lblPageSize.Text = "Paper Dimensions";
            // 
            // cboPageSizes
            // 
            this.cboPageSizes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPageSizes.FormattingEnabled = true;
            this.cboPageSizes.Location = new System.Drawing.Point(67, 40);
            this.cboPageSizes.Name = "cboPageSizes";
            this.cboPageSizes.Size = new System.Drawing.Size(234, 21);
            this.cboPageSizes.TabIndex = 2;
            this.cboPageSizes.SelectedIndexChanged += new System.EventHandler(this.cboPageSizes_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Paper Size";
            // 
            // cboPrinters
            // 
            this.cboPrinters.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cboPrinters.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboPrinters.FormattingEnabled = true;
            this.cboPrinters.Location = new System.Drawing.Point(6, 17);
            this.cboPrinters.Name = "cboPrinters";
            this.cboPrinters.Size = new System.Drawing.Size(295, 21);
            this.cboPrinters.TabIndex = 0;
            this.cboPrinters.SelectedIndexChanged += new System.EventHandler(this.cboPrinters_SelectedIndexChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(226, 97);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Preview";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.Location = new System.Drawing.Point(288, 144);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(331, 276);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 84);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Orientation";
            // 
            // chkLandscape
            // 
            this.chkLandscape.AutoSize = true;
            this.chkLandscape.Location = new System.Drawing.Point(67, 82);
            this.chkLandscape.Name = "chkLandscape";
            this.chkLandscape.Size = new System.Drawing.Size(78, 17);
            this.chkLandscape.TabIndex = 6;
            this.chkLandscape.Text = "Landscape";
            this.chkLandscape.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Checked = true;
            this.radioButton2.Location = new System.Drawing.Point(151, 82);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(58, 17);
            this.radioButton2.TabIndex = 7;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "Portrait";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(631, 432);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbxSourceImage)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblSourceImgInfo;
        private System.Windows.Forms.PictureBox pbxSourceImage;
        private System.Windows.Forms.Button btnOpenImg;
        private System.Windows.Forms.TextBox txtSourceImgPath;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox cboPrinters;
        private System.Windows.Forms.ComboBox cboPageSizes;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblPageSize;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton chkLandscape;
        private System.Windows.Forms.Label label2;
    }
}

