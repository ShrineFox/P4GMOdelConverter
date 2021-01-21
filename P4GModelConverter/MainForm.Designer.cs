namespace P4GModelConverter
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.propertyGrid_Main = new System.Windows.Forms.PropertyGrid();
            this.darkTreeView_Main = new DarkUI.Controls.DarkTreeView();
            this.darkMenuStrip_Main = new DarkUI.Controls.DarkMenuStrip();
            this.toolStripMenuItem_File = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_New = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_Open = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_Save = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_SaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_Close = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_Exit = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_Settings = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_Refresh = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_Help = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_About = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_Tutorial = new System.Windows.Forms.ToolStripMenuItem();
            this.darkContextMenu_RightClick = new DarkUI.Controls.DarkContextMenu();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.replaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveUpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveDownToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.darkMenuStrip_Main.SuspendLayout();
            this.darkContextMenu_RightClick.SuspendLayout();
            this.SuspendLayout();
            // 
            // propertyGrid_Main
            // 
            this.propertyGrid_Main.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyGrid_Main.CategorySplitterColor = System.Drawing.SystemColors.ControlDarkDark;
            this.propertyGrid_Main.DisabledItemForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.propertyGrid_Main.HelpBackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.propertyGrid_Main.HelpBorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.propertyGrid_Main.HelpForeColor = System.Drawing.SystemColors.Control;
            this.propertyGrid_Main.LineColor = System.Drawing.SystemColors.ControlDarkDark;
            this.propertyGrid_Main.Location = new System.Drawing.Point(451, 48);
            this.propertyGrid_Main.Margin = new System.Windows.Forms.Padding(1, 3, 3, 3);
            this.propertyGrid_Main.Name = "propertyGrid_Main";
            this.propertyGrid_Main.SelectedItemWithFocusBackColor = System.Drawing.Color.SteelBlue;
            this.propertyGrid_Main.SelectedItemWithFocusForeColor = System.Drawing.Color.Azure;
            this.propertyGrid_Main.Size = new System.Drawing.Size(576, 277);
            this.propertyGrid_Main.TabIndex = 2;
            this.propertyGrid_Main.ToolbarVisible = false;
            this.propertyGrid_Main.ViewBackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.propertyGrid_Main.ViewForeColor = System.Drawing.SystemColors.Window;
            // 
            // darkTreeView_Main
            // 
            this.darkTreeView_Main.AllowDrop = true;
            this.darkTreeView_Main.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.darkTreeView_Main.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.darkTreeView_Main.Location = new System.Drawing.Point(12, 48);
            this.darkTreeView_Main.MaxDragChange = 20;
            this.darkTreeView_Main.Name = "darkTreeView_Main";
            this.darkTreeView_Main.Size = new System.Drawing.Size(435, 662);
            this.darkTreeView_Main.TabIndex = 1;
            this.darkTreeView_Main.Text = "darkTreeView_Main";
            this.darkTreeView_Main.MouseClick += new System.Windows.Forms.MouseEventHandler(this.TreeView_MouseClick);
            // 
            // darkMenuStrip_Main
            // 
            this.darkMenuStrip_Main.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.darkMenuStrip_Main.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.darkMenuStrip_Main.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.darkMenuStrip_Main.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem_File,
            this.toolStripMenuItem_Settings,
            this.toolStripMenuItem_Refresh,
            this.toolStripMenuItem_Help});
            this.darkMenuStrip_Main.Location = new System.Drawing.Point(0, 0);
            this.darkMenuStrip_Main.Name = "darkMenuStrip_Main";
            this.darkMenuStrip_Main.Padding = new System.Windows.Forms.Padding(3, 2, 0, 2);
            this.darkMenuStrip_Main.Size = new System.Drawing.Size(1039, 28);
            this.darkMenuStrip_Main.TabIndex = 0;
            this.darkMenuStrip_Main.Text = "darkMenuStrip1";
            // 
            // toolStripMenuItem_File
            // 
            this.toolStripMenuItem_File.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.toolStripMenuItem_File.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem_New,
            this.toolStripMenuItem_Open,
            this.toolStripMenuItem_Save,
            this.toolStripMenuItem_SaveAs,
            this.toolStripMenuItem_Close,
            this.toolStripMenuItem_Exit});
            this.toolStripMenuItem_File.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.toolStripMenuItem_File.Name = "toolStripMenuItem_File";
            this.toolStripMenuItem_File.Size = new System.Drawing.Size(46, 24);
            this.toolStripMenuItem_File.Text = "File";
            // 
            // toolStripMenuItem_New
            // 
            this.toolStripMenuItem_New.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.toolStripMenuItem_New.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.toolStripMenuItem_New.Name = "toolStripMenuItem_New";
            this.toolStripMenuItem_New.Size = new System.Drawing.Size(224, 26);
            this.toolStripMenuItem_New.Text = "New";
            // 
            // toolStripMenuItem_Open
            // 
            this.toolStripMenuItem_Open.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.toolStripMenuItem_Open.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.toolStripMenuItem_Open.Name = "toolStripMenuItem_Open";
            this.toolStripMenuItem_Open.Size = new System.Drawing.Size(224, 26);
            this.toolStripMenuItem_Open.Text = "Open";
            this.toolStripMenuItem_Open.Click += new System.EventHandler(this.Open_Click);
            // 
            // toolStripMenuItem_Save
            // 
            this.toolStripMenuItem_Save.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.toolStripMenuItem_Save.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.toolStripMenuItem_Save.Name = "toolStripMenuItem_Save";
            this.toolStripMenuItem_Save.Size = new System.Drawing.Size(224, 26);
            this.toolStripMenuItem_Save.Text = "Save";
            // 
            // toolStripMenuItem_SaveAs
            // 
            this.toolStripMenuItem_SaveAs.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.toolStripMenuItem_SaveAs.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.toolStripMenuItem_SaveAs.Name = "toolStripMenuItem_SaveAs";
            this.toolStripMenuItem_SaveAs.Size = new System.Drawing.Size(224, 26);
            this.toolStripMenuItem_SaveAs.Text = "Save As...";
            // 
            // toolStripMenuItem_Close
            // 
            this.toolStripMenuItem_Close.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.toolStripMenuItem_Close.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.toolStripMenuItem_Close.Name = "toolStripMenuItem_Close";
            this.toolStripMenuItem_Close.Size = new System.Drawing.Size(224, 26);
            this.toolStripMenuItem_Close.Text = "Close";
            // 
            // toolStripMenuItem_Exit
            // 
            this.toolStripMenuItem_Exit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.toolStripMenuItem_Exit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.toolStripMenuItem_Exit.Name = "toolStripMenuItem_Exit";
            this.toolStripMenuItem_Exit.Size = new System.Drawing.Size(224, 26);
            this.toolStripMenuItem_Exit.Text = "Exit";
            // 
            // toolStripMenuItem_Settings
            // 
            this.toolStripMenuItem_Settings.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.toolStripMenuItem_Settings.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.toolStripMenuItem_Settings.Name = "toolStripMenuItem_Settings";
            this.toolStripMenuItem_Settings.Size = new System.Drawing.Size(76, 24);
            this.toolStripMenuItem_Settings.Text = "Settings";
            this.toolStripMenuItem_Settings.Click += new System.EventHandler(this.Settings_Click);
            // 
            // toolStripMenuItem_Refresh
            // 
            this.toolStripMenuItem_Refresh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.toolStripMenuItem_Refresh.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.toolStripMenuItem_Refresh.Name = "toolStripMenuItem_Refresh";
            this.toolStripMenuItem_Refresh.Size = new System.Drawing.Size(72, 24);
            this.toolStripMenuItem_Refresh.Text = "Refresh";
            this.toolStripMenuItem_Refresh.Click += new System.EventHandler(this.Refresh);
            // 
            // toolStripMenuItem_Help
            // 
            this.toolStripMenuItem_Help.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.toolStripMenuItem_Help.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem_About,
            this.toolStripMenuItem_Tutorial});
            this.toolStripMenuItem_Help.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.toolStripMenuItem_Help.Name = "toolStripMenuItem_Help";
            this.toolStripMenuItem_Help.Size = new System.Drawing.Size(55, 24);
            this.toolStripMenuItem_Help.Text = "Help";
            // 
            // toolStripMenuItem_About
            // 
            this.toolStripMenuItem_About.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.toolStripMenuItem_About.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.toolStripMenuItem_About.Name = "toolStripMenuItem_About";
            this.toolStripMenuItem_About.Size = new System.Drawing.Size(224, 26);
            this.toolStripMenuItem_About.Text = "About";
            // 
            // toolStripMenuItem_Tutorial
            // 
            this.toolStripMenuItem_Tutorial.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.toolStripMenuItem_Tutorial.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.toolStripMenuItem_Tutorial.Name = "toolStripMenuItem_Tutorial";
            this.toolStripMenuItem_Tutorial.Size = new System.Drawing.Size(224, 26);
            this.toolStripMenuItem_Tutorial.Text = "Tutorial";
            // 
            // darkContextMenu_RightClick
            // 
            this.darkContextMenu_RightClick.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.darkContextMenu_RightClick.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.darkContextMenu_RightClick.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.darkContextMenu_RightClick.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.moveUpToolStripMenuItem,
            this.moveDownToolStripMenuItem,
            this.exportToolStripMenuItem,
            this.importToolStripMenuItem,
            this.replaceToolStripMenuItem,
            this.deleteToolStripMenuItem});
            this.darkContextMenu_RightClick.Name = "darkContextMenu_RightClick";
            this.darkContextMenu_RightClick.Size = new System.Drawing.Size(159, 148);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.exportToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(158, 24);
            this.exportToolStripMenuItem.Text = "Export";
            // 
            // importToolStripMenuItem
            // 
            this.importToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.importToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.importToolStripMenuItem.Name = "importToolStripMenuItem";
            this.importToolStripMenuItem.Size = new System.Drawing.Size(158, 24);
            this.importToolStripMenuItem.Text = "Import";
            // 
            // replaceToolStripMenuItem
            // 
            this.replaceToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.replaceToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.replaceToolStripMenuItem.Name = "replaceToolStripMenuItem";
            this.replaceToolStripMenuItem.Size = new System.Drawing.Size(158, 24);
            this.replaceToolStripMenuItem.Text = "Replace";
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.deleteToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(158, 24);
            this.deleteToolStripMenuItem.Text = "Delete";
            // 
            // moveUpToolStripMenuItem
            // 
            this.moveUpToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.moveUpToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.moveUpToolStripMenuItem.Name = "moveUpToolStripMenuItem";
            this.moveUpToolStripMenuItem.Size = new System.Drawing.Size(158, 24);
            this.moveUpToolStripMenuItem.Text = "Move Up";
            // 
            // moveDownToolStripMenuItem
            // 
            this.moveDownToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.moveDownToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.moveDownToolStripMenuItem.Name = "moveDownToolStripMenuItem";
            this.moveDownToolStripMenuItem.Size = new System.Drawing.Size(158, 24);
            this.moveDownToolStripMenuItem.Text = "Move Down";
            // 
            // MainForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(1039, 722);
            this.Controls.Add(this.darkTreeView_Main);
            this.Controls.Add(this.propertyGrid_Main);
            this.Controls.Add(this.darkMenuStrip_Main);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.darkMenuStrip_Main;
            this.Margin = new System.Windows.Forms.Padding(5);
            this.MinimumSize = new System.Drawing.Size(877, 689);
            this.Name = "MainForm";
            this.Text = "P4GMOdel";
            this.darkMenuStrip_Main.ResumeLayout(false);
            this.darkMenuStrip_Main.PerformLayout();
            this.darkContextMenu_RightClick.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_Animations;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_Add;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_Remove;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_Rename;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBox_Rename;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tutorialToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
        private System.Windows.Forms.PropertyGrid propertyGrid_Main;
        private DarkUI.Controls.DarkTreeView darkTreeView_Main;
        private DarkUI.Controls.DarkMenuStrip darkMenuStrip_Main;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_File;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_New;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_Open;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_Save;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_Close;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_Exit;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_Settings;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_Refresh;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_Help;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_About;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_Tutorial;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_SaveAs;
        private DarkUI.Controls.DarkContextMenu darkContextMenu_RightClick;
        private System.Windows.Forms.ToolStripMenuItem moveUpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moveDownToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem replaceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
    }
}

