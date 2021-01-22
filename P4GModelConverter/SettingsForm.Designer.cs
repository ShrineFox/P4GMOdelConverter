
namespace P4GModelConverter
{
    partial class SettingsForm
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
            this.groupBox_Import = new DarkUI.Controls.DarkGroupBox();
            this.txtBox_AdditionalFBXOptions = new DarkUI.Controls.DarkTextBox();
            this.chkBox_AsciiFBX = new DarkUI.Controls.DarkCheckBox();
            this.label1 = new DarkUI.Controls.DarkLabel();
            this.chkBox_ExtractTextures = new DarkUI.Controls.DarkCheckBox();
            this.chkBox_ConvertToGMO = new DarkUI.Controls.DarkCheckBox();
            this.chkBox_OldFBXExport = new DarkUI.Controls.DarkCheckBox();
            this.chkBox_ConvertToFBX = new DarkUI.Controls.DarkCheckBox();
            this.groupBox_Conversion = new DarkUI.Controls.DarkGroupBox();
            this.chkBox_AutoConvertTex = new DarkUI.Controls.DarkCheckBox();
            this.lbl_WpnBone = new DarkUI.Controls.DarkLabel();
            this.chkBox_UseDummyMaterials = new DarkUI.Controls.DarkCheckBox();
            this.chkBox_LoadAnimations = new DarkUI.Controls.DarkCheckBox();
            this.txt_WeaponBoneName = new DarkUI.Controls.DarkTextBox();
            this.chkBox_RenameBones = new DarkUI.Controls.DarkCheckBox();
            this.groupBox_Output = new DarkUI.Controls.DarkGroupBox();
            this.comboBox_PreviewWith = new System.Windows.Forms.ComboBox();
            this.chkBox_PreviewOutputGMO = new DarkUI.Controls.DarkCheckBox();
            this.chkBox_FixForPC = new DarkUI.Controls.DarkCheckBox();
            this.btn_Save = new DarkUI.Controls.DarkButton();
            this.btn_Cancel = new DarkUI.Controls.DarkButton();
            this.groupBox_Import.SuspendLayout();
            this.groupBox_Conversion.SuspendLayout();
            this.groupBox_Output.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox_Import
            // 
            this.groupBox_Import.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.groupBox_Import.Controls.Add(this.txtBox_AdditionalFBXOptions);
            this.groupBox_Import.Controls.Add(this.chkBox_AsciiFBX);
            this.groupBox_Import.Controls.Add(this.label1);
            this.groupBox_Import.Controls.Add(this.chkBox_ExtractTextures);
            this.groupBox_Import.Controls.Add(this.chkBox_ConvertToGMO);
            this.groupBox_Import.Controls.Add(this.chkBox_OldFBXExport);
            this.groupBox_Import.Controls.Add(this.chkBox_ConvertToFBX);
            this.groupBox_Import.Location = new System.Drawing.Point(12, 12);
            this.groupBox_Import.Name = "groupBox_Import";
            this.groupBox_Import.Size = new System.Drawing.Size(200, 244);
            this.groupBox_Import.TabIndex = 0;
            this.groupBox_Import.TabStop = false;
            this.groupBox_Import.Text = "Input Settings";
            // 
            // txtBox_AdditionalFBXOptions
            // 
            this.txtBox_AdditionalFBXOptions.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.txtBox_AdditionalFBXOptions.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBox_AdditionalFBXOptions.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.txtBox_AdditionalFBXOptions.Location = new System.Drawing.Point(30, 118);
            this.txtBox_AdditionalFBXOptions.Name = "txtBox_AdditionalFBXOptions";
            this.txtBox_AdditionalFBXOptions.Size = new System.Drawing.Size(163, 22);
            this.txtBox_AdditionalFBXOptions.TabIndex = 6;
            // 
            // chkBox_AsciiFBX
            // 
            this.chkBox_AsciiFBX.AutoSize = true;
            this.chkBox_AsciiFBX.Enabled = false;
            this.chkBox_AsciiFBX.Location = new System.Drawing.Point(7, 76);
            this.chkBox_AsciiFBX.Margin = new System.Windows.Forms.Padding(4);
            this.chkBox_AsciiFBX.Name = "chkBox_AsciiFBX";
            this.chkBox_AsciiFBX.Size = new System.Drawing.Size(165, 21);
            this.chkBox_AsciiFBX.TabIndex = 4;
            this.chkBox_AsciiFBX.Text = "-fbxascii (mds output)";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(65)))));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.label1.Location = new System.Drawing.Point(27, 97);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(127, 17);
            this.label1.TabIndex = 5;
            this.label1.Text = "Additional Options:";
            // 
            // chkBox_ExtractTextures
            // 
            this.chkBox_ExtractTextures.AutoSize = true;
            this.chkBox_ExtractTextures.Location = new System.Drawing.Point(7, 163);
            this.chkBox_ExtractTextures.Margin = new System.Windows.Forms.Padding(4);
            this.chkBox_ExtractTextures.Name = "chkBox_ExtractTextures";
            this.chkBox_ExtractTextures.Size = new System.Drawing.Size(167, 21);
            this.chkBox_ExtractTextures.TabIndex = 8;
            this.chkBox_ExtractTextures.Text = "Extract TIM2 Textures";
            // 
            // chkBox_ConvertToGMO
            // 
            this.chkBox_ConvertToGMO.AutoSize = true;
            this.chkBox_ConvertToGMO.Location = new System.Drawing.Point(7, 143);
            this.chkBox_ConvertToGMO.Margin = new System.Windows.Forms.Padding(4);
            this.chkBox_ConvertToGMO.Name = "chkBox_ConvertToGMO";
            this.chkBox_ConvertToGMO.Size = new System.Drawing.Size(162, 21);
            this.chkBox_ConvertToGMO.TabIndex = 7;
            this.chkBox_ConvertToGMO.Text = "Convert FBX to GMO\r\n";
            // 
            // chkBox_OldFBXExport
            // 
            this.chkBox_OldFBXExport.AutoSize = true;
            this.chkBox_OldFBXExport.Enabled = false;
            this.chkBox_OldFBXExport.Location = new System.Drawing.Point(7, 56);
            this.chkBox_OldFBXExport.Margin = new System.Windows.Forms.Padding(4);
            this.chkBox_OldFBXExport.Name = "chkBox_OldFBXExport";
            this.chkBox_OldFBXExport.Size = new System.Drawing.Size(186, 21);
            this.chkBox_OldFBXExport.TabIndex = 3;
            this.chkBox_OldFBXExport.Text = "-fbxoldexport (animation)";
            // 
            // chkBox_ConvertToFBX
            // 
            this.chkBox_ConvertToFBX.AutoSize = true;
            this.chkBox_ConvertToFBX.Location = new System.Drawing.Point(7, 36);
            this.chkBox_ConvertToFBX.Margin = new System.Windows.Forms.Padding(4);
            this.chkBox_ConvertToFBX.Name = "chkBox_ConvertToFBX";
            this.chkBox_ConvertToFBX.Size = new System.Drawing.Size(189, 21);
            this.chkBox_ConvertToFBX.TabIndex = 2;
            this.chkBox_ConvertToFBX.Text = "Convert to FBX w/ Noesis";
            this.chkBox_ConvertToFBX.CheckedChanged += new System.EventHandler(this.ConvertToFBX_Checked);
            // 
            // groupBox_Conversion
            // 
            this.groupBox_Conversion.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.groupBox_Conversion.Controls.Add(this.chkBox_AutoConvertTex);
            this.groupBox_Conversion.Controls.Add(this.lbl_WpnBone);
            this.groupBox_Conversion.Controls.Add(this.chkBox_UseDummyMaterials);
            this.groupBox_Conversion.Controls.Add(this.chkBox_LoadAnimations);
            this.groupBox_Conversion.Controls.Add(this.txt_WeaponBoneName);
            this.groupBox_Conversion.Controls.Add(this.chkBox_RenameBones);
            this.groupBox_Conversion.Location = new System.Drawing.Point(218, 12);
            this.groupBox_Conversion.Name = "groupBox_Conversion";
            this.groupBox_Conversion.Size = new System.Drawing.Size(200, 244);
            this.groupBox_Conversion.TabIndex = 9;
            this.groupBox_Conversion.TabStop = false;
            this.groupBox_Conversion.Text = "Conversion Settings";
            // 
            // chkBox_AutoConvertTex
            // 
            this.chkBox_AutoConvertTex.AutoSize = true;
            this.chkBox_AutoConvertTex.Location = new System.Drawing.Point(10, 36);
            this.chkBox_AutoConvertTex.Margin = new System.Windows.Forms.Padding(4);
            this.chkBox_AutoConvertTex.Name = "chkBox_AutoConvertTex";
            this.chkBox_AutoConvertTex.Size = new System.Drawing.Size(186, 21);
            this.chkBox_AutoConvertTex.TabIndex = 10;
            this.chkBox_AutoConvertTex.Text = "Convert Textures to TM2";
            // 
            // lbl_WpnBone
            // 
            this.lbl_WpnBone.AutoSize = true;
            this.lbl_WpnBone.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(65)))));
            this.lbl_WpnBone.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.lbl_WpnBone.Location = new System.Drawing.Point(7, 156);
            this.lbl_WpnBone.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_WpnBone.Name = "lbl_WpnBone";
            this.lbl_WpnBone.Size = new System.Drawing.Size(143, 17);
            this.lbl_WpnBone.TabIndex = 14;
            this.lbl_WpnBone.Text = "Weapon Bone Name:";
            // 
            // chkBox_UseDummyMaterials
            // 
            this.chkBox_UseDummyMaterials.AutoSize = true;
            this.chkBox_UseDummyMaterials.Location = new System.Drawing.Point(10, 76);
            this.chkBox_UseDummyMaterials.Margin = new System.Windows.Forms.Padding(4);
            this.chkBox_UseDummyMaterials.Name = "chkBox_UseDummyMaterials";
            this.chkBox_UseDummyMaterials.Size = new System.Drawing.Size(167, 21);
            this.chkBox_UseDummyMaterials.TabIndex = 12;
            this.chkBox_UseDummyMaterials.Text = "Use Dummy Materials";
            // 
            // chkBox_LoadAnimations
            // 
            this.chkBox_LoadAnimations.AutoSize = true;
            this.chkBox_LoadAnimations.Location = new System.Drawing.Point(10, 97);
            this.chkBox_LoadAnimations.Margin = new System.Windows.Forms.Padding(4);
            this.chkBox_LoadAnimations.Name = "chkBox_LoadAnimations";
            this.chkBox_LoadAnimations.Size = new System.Drawing.Size(135, 21);
            this.chkBox_LoadAnimations.TabIndex = 13;
            this.chkBox_LoadAnimations.Text = "Load Animations";
            // 
            // txt_WeaponBoneName
            // 
            this.txt_WeaponBoneName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.txt_WeaponBoneName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_WeaponBoneName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.txt_WeaponBoneName.Location = new System.Drawing.Point(10, 176);
            this.txt_WeaponBoneName.Name = "txt_WeaponBoneName";
            this.txt_WeaponBoneName.Size = new System.Drawing.Size(149, 22);
            this.txt_WeaponBoneName.TabIndex = 15;
            this.txt_WeaponBoneName.Text = "Bip01_L_Hand_Bone";
            // 
            // chkBox_RenameBones
            // 
            this.chkBox_RenameBones.AutoSize = true;
            this.chkBox_RenameBones.Location = new System.Drawing.Point(10, 56);
            this.chkBox_RenameBones.Margin = new System.Windows.Forms.Padding(4);
            this.chkBox_RenameBones.Name = "chkBox_RenameBones";
            this.chkBox_RenameBones.Size = new System.Drawing.Size(173, 21);
            this.chkBox_RenameBones.TabIndex = 11;
            this.chkBox_RenameBones.Text = "Reformat Bone Names";
            // 
            // groupBox_Output
            // 
            this.groupBox_Output.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.groupBox_Output.Controls.Add(this.comboBox_PreviewWith);
            this.groupBox_Output.Controls.Add(this.chkBox_PreviewOutputGMO);
            this.groupBox_Output.Controls.Add(this.chkBox_FixForPC);
            this.groupBox_Output.Location = new System.Drawing.Point(424, 12);
            this.groupBox_Output.Name = "groupBox_Output";
            this.groupBox_Output.Size = new System.Drawing.Size(200, 244);
            this.groupBox_Output.TabIndex = 16;
            this.groupBox_Output.TabStop = false;
            this.groupBox_Output.Text = "Output Settings";
            // 
            // comboBox_PreviewWith
            // 
            this.comboBox_PreviewWith.AutoCompleteCustomSource.AddRange(new string[] {
            "Noesis",
            "GMOView"});
            this.comboBox_PreviewWith.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_PreviewWith.Enabled = false;
            this.comboBox_PreviewWith.FormattingEnabled = true;
            this.comboBox_PreviewWith.Items.AddRange(new object[] {
            "Noesis",
            "GMOView"});
            this.comboBox_PreviewWith.Location = new System.Drawing.Point(30, 76);
            this.comboBox_PreviewWith.Name = "comboBox_PreviewWith";
            this.comboBox_PreviewWith.Size = new System.Drawing.Size(124, 24);
            this.comboBox_PreviewWith.TabIndex = 19;
            // 
            // chkBox_PreviewOutputGMO
            // 
            this.chkBox_PreviewOutputGMO.AutoSize = true;
            this.chkBox_PreviewOutputGMO.Location = new System.Drawing.Point(7, 56);
            this.chkBox_PreviewOutputGMO.Margin = new System.Windows.Forms.Padding(4);
            this.chkBox_PreviewOutputGMO.Name = "chkBox_PreviewOutputGMO";
            this.chkBox_PreviewOutputGMO.Size = new System.Drawing.Size(164, 21);
            this.chkBox_PreviewOutputGMO.TabIndex = 18;
            this.chkBox_PreviewOutputGMO.Text = "Preview New GMO w/";
            this.chkBox_PreviewOutputGMO.CheckedChanged += new System.EventHandler(this.PreviewOutputGMO_Checked);
            // 
            // chkBox_FixForPC
            // 
            this.chkBox_FixForPC.AutoSize = true;
            this.chkBox_FixForPC.Location = new System.Drawing.Point(7, 36);
            this.chkBox_FixForPC.Margin = new System.Windows.Forms.Padding(4);
            this.chkBox_FixForPC.Name = "chkBox_FixForPC";
            this.chkBox_FixForPC.Size = new System.Drawing.Size(174, 21);
            this.chkBox_FixForPC.TabIndex = 17;
            this.chkBox_FixForPC.Text = "Fix Output GMO for PC";
            // 
            // btn_Save
            // 
            this.btn_Save.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btn_Save.Location = new System.Drawing.Point(549, 262);
            this.btn_Save.Name = "btn_Save";
            this.btn_Save.Padding = new System.Windows.Forms.Padding(5);
            this.btn_Save.Size = new System.Drawing.Size(75, 30);
            this.btn_Save.TabIndex = 21;
            this.btn_Save.Text = "Save";
            this.btn_Save.Click += new System.EventHandler(this.Save_Click);
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_Cancel.Location = new System.Drawing.Point(468, 262);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Padding = new System.Windows.Forms.Padding(5);
            this.btn_Cancel.Size = new System.Drawing.Size(75, 30);
            this.btn_Cancel.TabIndex = 20;
            this.btn_Cancel.Text = "Cancel";
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.ClientSize = new System.Drawing.Size(639, 300);
            this.Controls.Add(this.btn_Cancel);
            this.Controls.Add(this.btn_Save);
            this.Controls.Add(this.groupBox_Output);
            this.Controls.Add(this.groupBox_Conversion);
            this.Controls.Add(this.groupBox_Import);
            this.Name = "SettingsForm";
            this.Text = "P4GMOdel Settings";
            this.groupBox_Import.ResumeLayout(false);
            this.groupBox_Import.PerformLayout();
            this.groupBox_Conversion.ResumeLayout(false);
            this.groupBox_Conversion.PerformLayout();
            this.groupBox_Output.ResumeLayout(false);
            this.groupBox_Output.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public DarkUI.Controls.DarkGroupBox groupBox_Import;
        public DarkUI.Controls.DarkGroupBox groupBox_Conversion;
        public DarkUI.Controls.DarkGroupBox groupBox_Output;
        public DarkUI.Controls.DarkButton btn_Save;
        public DarkUI.Controls.DarkTextBox txtBox_AdditionalFBXOptions;
        public DarkUI.Controls.DarkCheckBox chkBox_AsciiFBX;
        public DarkUI.Controls.DarkLabel label1;
        public DarkUI.Controls.DarkCheckBox chkBox_ExtractTextures;
        public DarkUI.Controls.DarkCheckBox chkBox_ConvertToGMO;
        public DarkUI.Controls.DarkCheckBox chkBox_OldFBXExport;
        public DarkUI.Controls.DarkCheckBox chkBox_ConvertToFBX;
        public DarkUI.Controls.DarkCheckBox chkBox_AutoConvertTex;
        public DarkUI.Controls.DarkLabel lbl_WpnBone;
        public DarkUI.Controls.DarkCheckBox chkBox_UseDummyMaterials;
        public DarkUI.Controls.DarkCheckBox chkBox_LoadAnimations;
        public DarkUI.Controls.DarkTextBox txt_WeaponBoneName;
        public DarkUI.Controls.DarkCheckBox chkBox_RenameBones;
        public System.Windows.Forms.ComboBox comboBox_PreviewWith;
        public DarkUI.Controls.DarkCheckBox chkBox_PreviewOutputGMO;
        public DarkUI.Controls.DarkCheckBox chkBox_FixForPC;
        public DarkUI.Controls.DarkButton btn_Cancel;
    }
}
