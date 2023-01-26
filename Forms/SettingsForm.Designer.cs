
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
            this.chk_OptimizeForVita = new DarkUI.Controls.DarkCheckBox();
            this.chk_UseModelViewer = new DarkUI.Controls.DarkCheckBox();
            this.chk_FixForPC = new DarkUI.Controls.DarkCheckBox();
            this.btn_Save = new DarkUI.Controls.DarkButton();
            this.btn_Cancel = new DarkUI.Controls.DarkButton();
            this.SuspendLayout();
            // 
            // chk_OptimizeForVita
            // 
            this.chk_OptimizeForVita.AutoSize = true;
            this.chk_OptimizeForVita.Location = new System.Drawing.Point(13, 13);
            this.chk_OptimizeForVita.Margin = new System.Windows.Forms.Padding(4);
            this.chk_OptimizeForVita.Name = "chk_OptimizeForVita";
            this.chk_OptimizeForVita.Size = new System.Drawing.Size(125, 20);
            this.chk_OptimizeForVita.TabIndex = 10;
            this.chk_OptimizeForVita.Text = "Optimize for Vita";
            // 
            // chk_UseModelViewer
            // 
            this.chk_UseModelViewer.AutoSize = true;
            this.chk_UseModelViewer.Location = new System.Drawing.Point(13, 68);
            this.chk_UseModelViewer.Margin = new System.Windows.Forms.Padding(4);
            this.chk_UseModelViewer.Name = "chk_UseModelViewer";
            this.chk_UseModelViewer.Size = new System.Drawing.Size(139, 20);
            this.chk_UseModelViewer.TabIndex = 18;
            this.chk_UseModelViewer.Text = "Use Model Viewer";
            // 
            // chk_FixForPC
            // 
            this.chk_FixForPC.AutoSize = true;
            this.chk_FixForPC.Location = new System.Drawing.Point(13, 41);
            this.chk_FixForPC.Margin = new System.Windows.Forms.Padding(4);
            this.chk_FixForPC.Name = "chk_FixForPC";
            this.chk_FixForPC.Size = new System.Drawing.Size(160, 20);
            this.chk_FixForPC.TabIndex = 17;
            this.chk_FixForPC.Text = "Fix Output GMO for PC";
            // 
            // btn_Save
            // 
            this.btn_Save.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btn_Save.Location = new System.Drawing.Point(151, 113);
            this.btn_Save.Name = "btn_Save";
            this.btn_Save.Padding = new System.Windows.Forms.Padding(5);
            this.btn_Save.Size = new System.Drawing.Size(85, 42);
            this.btn_Save.TabIndex = 21;
            this.btn_Save.Text = "Save";
            this.btn_Save.Click += new System.EventHandler(this.Save_Click);
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_Cancel.Location = new System.Drawing.Point(53, 113);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Padding = new System.Windows.Forms.Padding(5);
            this.btn_Cancel.Size = new System.Drawing.Size(85, 42);
            this.btn_Cancel.TabIndex = 20;
            this.btn_Cancel.Text = "Cancel";
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.ClientSize = new System.Drawing.Size(248, 167);
            this.Controls.Add(this.chk_UseModelViewer);
            this.Controls.Add(this.chk_OptimizeForVita);
            this.Controls.Add(this.chk_FixForPC);
            this.Controls.Add(this.btn_Cancel);
            this.Controls.Add(this.btn_Save);
            this.Name = "SettingsForm";
            this.Text = "P4GMOdel Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public DarkUI.Controls.DarkButton btn_Save;
        public DarkUI.Controls.DarkCheckBox chk_OptimizeForVita;
        public DarkUI.Controls.DarkCheckBox chk_FixForPC;
        public DarkUI.Controls.DarkButton btn_Cancel;
        public DarkUI.Controls.DarkCheckBox chk_UseModelViewer;
    }
}
