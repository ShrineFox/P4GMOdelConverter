
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
            SuspendLayout();
            // 
            // chk_OptimizeForVita
            // 
            chk_OptimizeForVita.AutoSize = true;
            chk_OptimizeForVita.Location = new Point(13, 16);
            chk_OptimizeForVita.Margin = new Padding(4, 5, 4, 5);
            chk_OptimizeForVita.Name = "chk_OptimizeForVita";
            chk_OptimizeForVita.Size = new Size(145, 24);
            chk_OptimizeForVita.TabIndex = 10;
            chk_OptimizeForVita.Text = "Optimize for Vita";
            // 
            // chk_UseModelViewer
            // 
            chk_UseModelViewer.AutoSize = true;
            chk_UseModelViewer.Location = new Point(13, 85);
            chk_UseModelViewer.Margin = new Padding(4, 5, 4, 5);
            chk_UseModelViewer.Name = "chk_UseModelViewer";
            chk_UseModelViewer.Size = new Size(151, 24);
            chk_UseModelViewer.TabIndex = 18;
            chk_UseModelViewer.Text = "Use Model Viewer";
            // 
            // chk_FixForPC
            // 
            chk_FixForPC.AutoSize = true;
            chk_FixForPC.Location = new Point(13, 51);
            chk_FixForPC.Margin = new Padding(4, 5, 4, 5);
            chk_FixForPC.Name = "chk_FixForPC";
            chk_FixForPC.Size = new Size(181, 24);
            chk_FixForPC.TabIndex = 17;
            chk_FixForPC.Text = "Fix Output GMO for PC";
            // 
            // btn_Save
            // 
            btn_Save.DialogResult = DialogResult.OK;
            btn_Save.Location = new Point(151, 141);
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
            btn_Cancel.Location = new Point(53, 141);
            btn_Cancel.Margin = new Padding(3, 4, 3, 4);
            btn_Cancel.Name = "btn_Cancel";
            btn_Cancel.Padding = new Padding(5, 6, 5, 6);
            btn_Cancel.Size = new Size(85, 52);
            btn_Cancel.TabIndex = 20;
            btn_Cancel.Text = "Cancel";
            // 
            // SettingsForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(60, 63, 65);
            ClientSize = new Size(248, 209);
            Controls.Add(chk_UseModelViewer);
            Controls.Add(chk_OptimizeForVita);
            Controls.Add(chk_FixForPC);
            Controls.Add(btn_Cancel);
            Controls.Add(btn_Save);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(3, 4, 3, 4);
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
    }
}
