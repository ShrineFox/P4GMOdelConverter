
namespace P4GMOdel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            chk_OptimizeForVita = new DarkUI.Controls.DarkCheckBox();
            chk_UseModelViewer = new DarkUI.Controls.DarkCheckBox();
            chk_FixForPC = new DarkUI.Controls.DarkCheckBox();
            btn_Save = new DarkUI.Controls.DarkButton();
            btn_Cancel = new DarkUI.Controls.DarkButton();
            chk_OptimizeFbxWithNoesis = new DarkUI.Controls.DarkCheckBox();
            txt_NoesisArgs = new DarkUI.Controls.DarkTextBox();
            lbl_NoesisArgs = new DarkUI.Controls.DarkLabel();
            comboBox_ModelViewer = new DarkUI.Controls.DarkComboBox();
            SuspendLayout();
            // 
            // chk_OptimizeForVita
            // 
            chk_OptimizeForVita.AutoSize = true;
            chk_OptimizeForVita.Location = new Point(226, 13);
            chk_OptimizeForVita.Margin = new Padding(4, 5, 4, 5);
            chk_OptimizeForVita.Name = "chk_OptimizeForVita";
            chk_OptimizeForVita.Size = new Size(183, 24);
            chk_OptimizeForVita.TabIndex = 10;
            chk_OptimizeForVita.Text = "Optimize GMO for Vita";
            // 
            // chk_UseModelViewer
            // 
            chk_UseModelViewer.AutoSize = true;
            chk_UseModelViewer.Location = new Point(13, 48);
            chk_UseModelViewer.Margin = new Padding(4, 5, 4, 5);
            chk_UseModelViewer.Name = "chk_UseModelViewer";
            chk_UseModelViewer.Size = new Size(154, 24);
            chk_UseModelViewer.TabIndex = 18;
            chk_UseModelViewer.Text = "Use Model Viewer:";
            // 
            // chk_FixForPC
            // 
            chk_FixForPC.AutoSize = true;
            chk_FixForPC.Location = new Point(13, 14);
            chk_FixForPC.Margin = new Padding(4, 5, 4, 5);
            chk_FixForPC.Name = "chk_FixForPC";
            chk_FixForPC.Size = new Size(181, 24);
            chk_FixForPC.TabIndex = 17;
            chk_FixForPC.Text = "Fix Output GMO for PC";
            // 
            // btn_Save
            // 
            btn_Save.DialogResult = DialogResult.OK;
            btn_Save.Location = new Point(382, 138);
            btn_Save.Margin = new Padding(3, 4, 3, 4);
            btn_Save.Name = "btn_Save";
            btn_Save.Padding = new Padding(5, 6, 5, 6);
            btn_Save.Size = new Size(85, 52);
            btn_Save.TabIndex = 21;
            btn_Save.Text = "Save";
            btn_Save.Click += Save_Click;
            // 
            // btn_Cancel
            // 
            btn_Cancel.DialogResult = DialogResult.Cancel;
            btn_Cancel.Location = new Point(284, 138);
            btn_Cancel.Margin = new Padding(3, 4, 3, 4);
            btn_Cancel.Name = "btn_Cancel";
            btn_Cancel.Padding = new Padding(5, 6, 5, 6);
            btn_Cancel.Size = new Size(85, 52);
            btn_Cancel.TabIndex = 20;
            btn_Cancel.Text = "Cancel";
            // 
            // chk_OptimizeFbxWithNoesis
            // 
            chk_OptimizeFbxWithNoesis.AutoSize = true;
            chk_OptimizeFbxWithNoesis.Location = new Point(226, 47);
            chk_OptimizeFbxWithNoesis.Margin = new Padding(4, 5, 4, 5);
            chk_OptimizeFbxWithNoesis.Name = "chk_OptimizeFbxWithNoesis";
            chk_OptimizeFbxWithNoesis.Size = new Size(190, 24);
            chk_OptimizeFbxWithNoesis.TabIndex = 22;
            chk_OptimizeFbxWithNoesis.Text = "Optimize FBX w/ Noesis";
            // 
            // txt_NoesisArgs
            // 
            txt_NoesisArgs.BackColor = Color.FromArgb(69, 73, 74);
            txt_NoesisArgs.BorderStyle = BorderStyle.FixedSingle;
            txt_NoesisArgs.ForeColor = Color.FromArgb(220, 220, 220);
            txt_NoesisArgs.Location = new Point(275, 77);
            txt_NoesisArgs.Name = "txt_NoesisArgs";
            txt_NoesisArgs.Size = new Size(195, 27);
            txt_NoesisArgs.TabIndex = 23;
            // 
            // lbl_NoesisArgs
            // 
            lbl_NoesisArgs.AutoSize = true;
            lbl_NoesisArgs.ForeColor = Color.FromArgb(220, 220, 220);
            lbl_NoesisArgs.Location = new Point(231, 79);
            lbl_NoesisArgs.Name = "lbl_NoesisArgs";
            lbl_NoesisArgs.Size = new Size(42, 20);
            lbl_NoesisArgs.TabIndex = 24;
            lbl_NoesisArgs.Text = "Args:";
            // 
            // comboBox_ModelViewer
            // 
            comboBox_ModelViewer.BackColor = Color.FromArgb(69, 73, 74);
            comboBox_ModelViewer.BorderColor = Color.Empty;
            comboBox_ModelViewer.BorderStyle = ButtonBorderStyle.Solid;
            comboBox_ModelViewer.ButtonColor = Color.FromArgb(43, 43, 43);
            comboBox_ModelViewer.ButtonIcon = (Bitmap)resources.GetObject("comboBox_ModelViewer.ButtonIcon");
            comboBox_ModelViewer.DrawDropdownHoverOutline = false;
            comboBox_ModelViewer.DrawFocusRectangle = false;
            comboBox_ModelViewer.DrawMode = DrawMode.OwnerDrawVariable;
            comboBox_ModelViewer.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox_ModelViewer.FlatStyle = FlatStyle.Flat;
            comboBox_ModelViewer.ForeColor = Color.Gainsboro;
            comboBox_ModelViewer.FormattingEnabled = true;
            comboBox_ModelViewer.Items.AddRange(new object[] { "GMOView", "Noesis" });
            comboBox_ModelViewer.Location = new Point(31, 77);
            comboBox_ModelViewer.Name = "comboBox_ModelViewer";
            comboBox_ModelViewer.Size = new Size(136, 28);
            comboBox_ModelViewer.TabIndex = 25;
            comboBox_ModelViewer.Text = "GMOView";
            comboBox_ModelViewer.TextPadding = new Padding(2);
            // 
            // SettingsForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(60, 63, 65);
            ClientSize = new Size(482, 203);
            Controls.Add(comboBox_ModelViewer);
            Controls.Add(lbl_NoesisArgs);
            Controls.Add(txt_NoesisArgs);
            Controls.Add(chk_OptimizeFbxWithNoesis);
            Controls.Add(chk_UseModelViewer);
            Controls.Add(chk_OptimizeForVita);
            Controls.Add(chk_FixForPC);
            Controls.Add(btn_Cancel);
            Controls.Add(btn_Save);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(3, 4, 3, 4);
            MaximumSize = new Size(500, 250);
            MinimumSize = new Size(500, 250);
            Name = "SettingsForm";
            Text = "P4GMOdel Settings";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        public DarkUI.Controls.DarkButton btn_Save;
        public DarkUI.Controls.DarkCheckBox chk_OptimizeForVita;
        public DarkUI.Controls.DarkCheckBox chk_FixForPC;
        public DarkUI.Controls.DarkButton btn_Cancel;
        public DarkUI.Controls.DarkCheckBox chk_UseModelViewer;
        public DarkUI.Controls.DarkCheckBox chk_OptimizeFbxWithNoesis;
        private DarkUI.Controls.DarkTextBox txt_NoesisArgs;
        private DarkUI.Controls.DarkLabel lbl_NoesisArgs;
        private DarkUI.Controls.DarkComboBox comboBox_ModelViewer;
    }
}
