using DarkUI.Forms;

namespace P4GMOdel
{
    partial class MainForm : DarkForm
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
            propertyGrid_Main = new PropertyGrid();
            darkTreeView_Main = new DarkUI.Controls.DarkTreeView();
            darkMenuStrip_Main = new DarkUI.Controls.DarkMenuStrip();
            toolStripMenuItem_File = new ToolStripMenuItem();
            toolStripMenuItem_New = new ToolStripMenuItem();
            toolStripMenuItem_Open = new ToolStripMenuItem();
            toolStripMenuItem_Save = new ToolStripMenuItem();
            toolStripMenuItem_SaveAs = new ToolStripMenuItem();
            toolStripMenuItem_Export = new ToolStripMenuItem();
            toolStripMenuItem_Exit = new ToolStripMenuItem();
            toolStripMenuItem_Settings = new ToolStripMenuItem();
            toolStripMenuItem_Refresh = new ToolStripMenuItem();
            toolStripMenuItem_Help = new ToolStripMenuItem();
            toolStripMenuItem_About = new ToolStripMenuItem();
            toolStripMenuItem_Tutorial = new ToolStripMenuItem();
            animationPresetsToolStripMenuItem = new ToolStripMenuItem();
            applyNamesToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem_ApplyP4GProtag = new ToolStripMenuItem();
            toolStripMenuItem_ApplyP4GParty = new ToolStripMenuItem();
            toolStripMenuItem_ApplyP4GPersona = new ToolStripMenuItem();
            toolStripMenuItem_ApplyP4GCulprit = new ToolStripMenuItem();
            toolStripMenuItem_ApplyP3PProtag = new ToolStripMenuItem();
            toolStripMenuItem_ApplyP3PParty = new ToolStripMenuItem();
            toolStripMenuItem_ApplyP3PPersona = new ToolStripMenuItem();
            toolStripMenuItem_ApplyP3PStrega = new ToolStripMenuItem();
            toolStripMenuItem_ReorderP3P = new ToolStripMenuItem();
            toolStripMenuItem_ReorderP4GProtag = new ToolStripMenuItem();
            toolStripMenuItem_ReorderP4GParty = new ToolStripMenuItem();
            toolStripMenuItem_ReorderP4GPersona = new ToolStripMenuItem();
            toolStripMenuItem_ReorderP4GCulprit = new ToolStripMenuItem();
            toolStripMenuItem_ReorderP3PProtag = new ToolStripMenuItem();
            toolStripMenuItem_ReorderP3PParty = new ToolStripMenuItem();
            toolStripMenuItem_ReorderP3PPersona = new ToolStripMenuItem();
            toolStripMenuItem_ReorderP3PStrega = new ToolStripMenuItem();
            darkContextMenu_RightClick = new DarkUI.Controls.DarkContextMenu();
            moveUpToolStripMenuItem = new ToolStripMenuItem();
            moveDownToolStripMenuItem = new ToolStripMenuItem();
            exportToolStripMenuItem = new ToolStripMenuItem();
            addToolStripMenuItem = new ToolStripMenuItem();
            replaceToolStripMenuItem = new ToolStripMenuItem();
            deleteToolStripMenuItem = new ToolStripMenuItem();
            tlp_Main = new TableLayoutPanel();
            darkMenuStrip_Main.SuspendLayout();
            darkContextMenu_RightClick.SuspendLayout();
            tlp_Main.SuspendLayout();
            SuspendLayout();
            // 
            // propertyGrid_Main
            // 
            propertyGrid_Main.CategoryForeColor = Color.Silver;
            propertyGrid_Main.CategorySplitterColor = Color.FromArgb(60, 63, 65);
            propertyGrid_Main.CommandsBorderColor = Color.FromArgb(60, 63, 65);
            propertyGrid_Main.CommandsDisabledLinkColor = Color.FromArgb(60, 63, 65);
            propertyGrid_Main.CommandsForeColor = Color.Silver;
            propertyGrid_Main.CommandsVisibleIfAvailable = false;
            propertyGrid_Main.DisabledItemForeColor = Color.FromArgb(60, 63, 65);
            propertyGrid_Main.Dock = DockStyle.Fill;
            propertyGrid_Main.HelpBackColor = Color.FromArgb(60, 63, 65);
            propertyGrid_Main.HelpBorderColor = Color.FromArgb(60, 63, 65);
            propertyGrid_Main.HelpForeColor = SystemColors.Control;
            propertyGrid_Main.HelpVisible = false;
            propertyGrid_Main.LineColor = Color.FromArgb(60, 63, 65);
            propertyGrid_Main.Location = new Point(312, 4);
            propertyGrid_Main.Margin = new Padding(1, 4, 3, 4);
            propertyGrid_Main.Name = "propertyGrid_Main";
            propertyGrid_Main.SelectedItemWithFocusBackColor = Color.SteelBlue;
            propertyGrid_Main.SelectedItemWithFocusForeColor = Color.Azure;
            propertyGrid_Main.Size = new Size(724, 254);
            propertyGrid_Main.TabIndex = 2;
            propertyGrid_Main.ToolbarVisible = false;
            propertyGrid_Main.ViewBackColor = Color.FromArgb(60, 63, 65);
            propertyGrid_Main.ViewBorderColor = Color.FromArgb(60, 63, 65);
            propertyGrid_Main.ViewForeColor = SystemColors.Window;
            propertyGrid_Main.PropertyValueChanged += PropertyGrid_ValueChanged;
            // 
            // darkTreeView_Main
            // 
            darkTreeView_Main.AllowDrop = true;
            darkTreeView_Main.BackColor = Color.FromArgb(60, 63, 65);
            darkTreeView_Main.Dock = DockStyle.Fill;
            darkTreeView_Main.Location = new Point(3, 4);
            darkTreeView_Main.Margin = new Padding(3, 4, 3, 4);
            darkTreeView_Main.MaxDragChange = 20;
            darkTreeView_Main.Name = "darkTreeView_Main";
            tlp_Main.SetRowSpan(darkTreeView_Main, 2);
            darkTreeView_Main.Size = new Size(305, 866);
            darkTreeView_Main.TabIndex = 1;
            darkTreeView_Main.Text = "darkTreeView_Main";
            darkTreeView_Main.SelectedNodesChanged += TreeView_SelectedNodesChanged;
            darkTreeView_Main.DragDrop += Treeview_DragDrop;
            darkTreeView_Main.DragEnter += Treeview_DragEnter;
            darkTreeView_Main.MouseClick += TreeView_MouseClick;
            // 
            // darkMenuStrip_Main
            // 
            darkMenuStrip_Main.BackColor = Color.FromArgb(60, 63, 65);
            darkMenuStrip_Main.ForeColor = Color.FromArgb(220, 220, 220);
            darkMenuStrip_Main.ImageScalingSize = new Size(20, 20);
            darkMenuStrip_Main.Items.AddRange(new ToolStripItem[] { toolStripMenuItem_File, toolStripMenuItem_Settings, toolStripMenuItem_Refresh, toolStripMenuItem_Help, animationPresetsToolStripMenuItem });
            darkMenuStrip_Main.Location = new Point(0, 0);
            darkMenuStrip_Main.Name = "darkMenuStrip_Main";
            darkMenuStrip_Main.Padding = new Padding(3, 2, 0, 2);
            darkMenuStrip_Main.Size = new Size(1039, 28);
            darkMenuStrip_Main.TabIndex = 0;
            darkMenuStrip_Main.Text = "darkMenuStrip1";
            // 
            // toolStripMenuItem_File
            // 
            toolStripMenuItem_File.BackColor = Color.FromArgb(60, 63, 65);
            toolStripMenuItem_File.DropDownItems.AddRange(new ToolStripItem[] { toolStripMenuItem_New, toolStripMenuItem_Open, toolStripMenuItem_Save, toolStripMenuItem_SaveAs, toolStripMenuItem_Export, toolStripMenuItem_Exit });
            toolStripMenuItem_File.ForeColor = Color.FromArgb(220, 220, 220);
            toolStripMenuItem_File.Name = "toolStripMenuItem_File";
            toolStripMenuItem_File.Size = new Size(46, 24);
            toolStripMenuItem_File.Text = "File";
            // 
            // toolStripMenuItem_New
            // 
            toolStripMenuItem_New.BackColor = Color.FromArgb(60, 63, 65);
            toolStripMenuItem_New.ForeColor = Color.FromArgb(220, 220, 220);
            toolStripMenuItem_New.Name = "toolStripMenuItem_New";
            toolStripMenuItem_New.Size = new Size(152, 26);
            toolStripMenuItem_New.Text = "New";
            toolStripMenuItem_New.Click += New_Click;
            // 
            // toolStripMenuItem_Open
            // 
            toolStripMenuItem_Open.BackColor = Color.FromArgb(60, 63, 65);
            toolStripMenuItem_Open.ForeColor = Color.FromArgb(220, 220, 220);
            toolStripMenuItem_Open.Name = "toolStripMenuItem_Open";
            toolStripMenuItem_Open.Size = new Size(152, 26);
            toolStripMenuItem_Open.Text = "Open";
            toolStripMenuItem_Open.Click += Open_Click;
            // 
            // toolStripMenuItem_Save
            // 
            toolStripMenuItem_Save.BackColor = Color.FromArgb(60, 63, 65);
            toolStripMenuItem_Save.Enabled = false;
            toolStripMenuItem_Save.ForeColor = Color.FromArgb(153, 153, 153);
            toolStripMenuItem_Save.Name = "toolStripMenuItem_Save";
            toolStripMenuItem_Save.Size = new Size(152, 26);
            toolStripMenuItem_Save.Text = "Save";
            toolStripMenuItem_Save.Click += Save_Click;
            // 
            // toolStripMenuItem_SaveAs
            // 
            toolStripMenuItem_SaveAs.BackColor = Color.FromArgb(60, 63, 65);
            toolStripMenuItem_SaveAs.Enabled = false;
            toolStripMenuItem_SaveAs.ForeColor = Color.FromArgb(153, 153, 153);
            toolStripMenuItem_SaveAs.Name = "toolStripMenuItem_SaveAs";
            toolStripMenuItem_SaveAs.Size = new Size(152, 26);
            toolStripMenuItem_SaveAs.Text = "Save As...";
            toolStripMenuItem_SaveAs.Click += SaveAs_Click;
            // 
            // toolStripMenuItem_Export
            // 
            toolStripMenuItem_Export.BackColor = Color.FromArgb(60, 63, 65);
            toolStripMenuItem_Export.Enabled = false;
            toolStripMenuItem_Export.ForeColor = Color.FromArgb(153, 153, 153);
            toolStripMenuItem_Export.Name = "toolStripMenuItem_Export";
            toolStripMenuItem_Export.Size = new Size(152, 26);
            toolStripMenuItem_Export.Text = "Export";
            toolStripMenuItem_Export.Click += Export_Click;
            // 
            // toolStripMenuItem_Exit
            // 
            toolStripMenuItem_Exit.BackColor = Color.FromArgb(60, 63, 65);
            toolStripMenuItem_Exit.ForeColor = Color.FromArgb(220, 220, 220);
            toolStripMenuItem_Exit.Name = "toolStripMenuItem_Exit";
            toolStripMenuItem_Exit.Size = new Size(152, 26);
            toolStripMenuItem_Exit.Text = "Exit";
            toolStripMenuItem_Exit.Click += Exit_Click;
            // 
            // toolStripMenuItem_Settings
            // 
            toolStripMenuItem_Settings.BackColor = Color.FromArgb(60, 63, 65);
            toolStripMenuItem_Settings.ForeColor = Color.FromArgb(220, 220, 220);
            toolStripMenuItem_Settings.Name = "toolStripMenuItem_Settings";
            toolStripMenuItem_Settings.Size = new Size(76, 24);
            toolStripMenuItem_Settings.Text = "Settings";
            toolStripMenuItem_Settings.Click += Settings_Click;
            // 
            // toolStripMenuItem_Refresh
            // 
            toolStripMenuItem_Refresh.BackColor = Color.FromArgb(60, 63, 65);
            toolStripMenuItem_Refresh.ForeColor = Color.FromArgb(220, 220, 220);
            toolStripMenuItem_Refresh.Name = "toolStripMenuItem_Refresh";
            toolStripMenuItem_Refresh.Size = new Size(72, 24);
            toolStripMenuItem_Refresh.Text = "Refresh";
            toolStripMenuItem_Refresh.Click += Refresh;
            // 
            // toolStripMenuItem_Help
            // 
            toolStripMenuItem_Help.BackColor = Color.FromArgb(60, 63, 65);
            toolStripMenuItem_Help.DropDownItems.AddRange(new ToolStripItem[] { toolStripMenuItem_About, toolStripMenuItem_Tutorial });
            toolStripMenuItem_Help.ForeColor = Color.FromArgb(220, 220, 220);
            toolStripMenuItem_Help.Name = "toolStripMenuItem_Help";
            toolStripMenuItem_Help.Size = new Size(55, 24);
            toolStripMenuItem_Help.Text = "Help";
            // 
            // toolStripMenuItem_About
            // 
            toolStripMenuItem_About.BackColor = Color.FromArgb(60, 63, 65);
            toolStripMenuItem_About.ForeColor = Color.FromArgb(220, 220, 220);
            toolStripMenuItem_About.Name = "toolStripMenuItem_About";
            toolStripMenuItem_About.Size = new Size(143, 26);
            toolStripMenuItem_About.Text = "About";
            // 
            // toolStripMenuItem_Tutorial
            // 
            toolStripMenuItem_Tutorial.BackColor = Color.FromArgb(60, 63, 65);
            toolStripMenuItem_Tutorial.ForeColor = Color.FromArgb(220, 220, 220);
            toolStripMenuItem_Tutorial.Name = "toolStripMenuItem_Tutorial";
            toolStripMenuItem_Tutorial.Size = new Size(143, 26);
            toolStripMenuItem_Tutorial.Text = "Tutorial";
            // 
            // animationPresetsToolStripMenuItem
            // 
            animationPresetsToolStripMenuItem.BackColor = Color.FromArgb(60, 63, 65);
            animationPresetsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { applyNamesToolStripMenuItem, toolStripMenuItem_ReorderP3P });
            animationPresetsToolStripMenuItem.ForeColor = Color.FromArgb(220, 220, 220);
            animationPresetsToolStripMenuItem.Name = "animationPresetsToolStripMenuItem";
            animationPresetsToolStripMenuItem.Size = new Size(142, 24);
            animationPresetsToolStripMenuItem.Text = "Animation Presets";
            // 
            // applyNamesToolStripMenuItem
            // 
            applyNamesToolStripMenuItem.BackColor = Color.FromArgb(60, 63, 65);
            applyNamesToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { toolStripMenuItem_ApplyP4GProtag, toolStripMenuItem_ApplyP4GParty, toolStripMenuItem_ApplyP4GPersona, toolStripMenuItem_ApplyP4GCulprit, toolStripMenuItem_ApplyP3PProtag, toolStripMenuItem_ApplyP3PParty, toolStripMenuItem_ApplyP3PPersona, toolStripMenuItem_ApplyP3PStrega });
            applyNamesToolStripMenuItem.ForeColor = Color.FromArgb(220, 220, 220);
            applyNamesToolStripMenuItem.Name = "applyNamesToolStripMenuItem";
            applyNamesToolStripMenuItem.Size = new Size(190, 26);
            applyNamesToolStripMenuItem.Text = "Apply Names...";
            // 
            // toolStripMenuItem_ApplyP4GProtag
            // 
            toolStripMenuItem_ApplyP4GProtag.BackColor = Color.FromArgb(60, 63, 65);
            toolStripMenuItem_ApplyP4GProtag.ForeColor = Color.FromArgb(220, 220, 220);
            toolStripMenuItem_ApplyP4GProtag.Name = "toolStripMenuItem_ApplyP4GProtag";
            toolStripMenuItem_ApplyP4GProtag.Size = new Size(173, 26);
            toolStripMenuItem_ApplyP4GProtag.Text = "P4G Protag";
            toolStripMenuItem_ApplyP4GProtag.Click += ApplyP4GProtag_Click;
            // 
            // toolStripMenuItem_ApplyP4GParty
            // 
            toolStripMenuItem_ApplyP4GParty.BackColor = Color.FromArgb(60, 63, 65);
            toolStripMenuItem_ApplyP4GParty.ForeColor = Color.FromArgb(220, 220, 220);
            toolStripMenuItem_ApplyP4GParty.Name = "toolStripMenuItem_ApplyP4GParty";
            toolStripMenuItem_ApplyP4GParty.Size = new Size(173, 26);
            toolStripMenuItem_ApplyP4GParty.Text = "P4G Party";
            toolStripMenuItem_ApplyP4GParty.Click += ApplyP4GParty_Click;
            // 
            // toolStripMenuItem_ApplyP4GPersona
            // 
            toolStripMenuItem_ApplyP4GPersona.BackColor = Color.FromArgb(60, 63, 65);
            toolStripMenuItem_ApplyP4GPersona.ForeColor = Color.FromArgb(220, 220, 220);
            toolStripMenuItem_ApplyP4GPersona.Name = "toolStripMenuItem_ApplyP4GPersona";
            toolStripMenuItem_ApplyP4GPersona.Size = new Size(173, 26);
            toolStripMenuItem_ApplyP4GPersona.Text = "P4G Persona";
            toolStripMenuItem_ApplyP4GPersona.Click += ApplyP4GPersona_Click;
            // 
            // toolStripMenuItem_ApplyP4GCulprit
            // 
            toolStripMenuItem_ApplyP4GCulprit.BackColor = Color.FromArgb(60, 63, 65);
            toolStripMenuItem_ApplyP4GCulprit.ForeColor = Color.FromArgb(220, 220, 220);
            toolStripMenuItem_ApplyP4GCulprit.Name = "toolStripMenuItem_ApplyP4GCulprit";
            toolStripMenuItem_ApplyP4GCulprit.Size = new Size(173, 26);
            toolStripMenuItem_ApplyP4GCulprit.Text = "P4G Culprit";
            toolStripMenuItem_ApplyP4GCulprit.Click += ApplyP4GCulprit_Click;
            // 
            // toolStripMenuItem_ApplyP3PProtag
            // 
            toolStripMenuItem_ApplyP3PProtag.BackColor = Color.FromArgb(60, 63, 65);
            toolStripMenuItem_ApplyP3PProtag.ForeColor = Color.FromArgb(220, 220, 220);
            toolStripMenuItem_ApplyP3PProtag.Name = "toolStripMenuItem_ApplyP3PProtag";
            toolStripMenuItem_ApplyP3PProtag.Size = new Size(173, 26);
            toolStripMenuItem_ApplyP3PProtag.Text = "P3P Protag";
            toolStripMenuItem_ApplyP3PProtag.Click += ApplyP3PProtag_Click;
            // 
            // toolStripMenuItem_ApplyP3PParty
            // 
            toolStripMenuItem_ApplyP3PParty.BackColor = Color.FromArgb(60, 63, 65);
            toolStripMenuItem_ApplyP3PParty.ForeColor = Color.FromArgb(220, 220, 220);
            toolStripMenuItem_ApplyP3PParty.Name = "toolStripMenuItem_ApplyP3PParty";
            toolStripMenuItem_ApplyP3PParty.Size = new Size(173, 26);
            toolStripMenuItem_ApplyP3PParty.Text = "P3P Party";
            toolStripMenuItem_ApplyP3PParty.Click += ApplyP3PParty_Click;
            // 
            // toolStripMenuItem_ApplyP3PPersona
            // 
            toolStripMenuItem_ApplyP3PPersona.BackColor = Color.FromArgb(60, 63, 65);
            toolStripMenuItem_ApplyP3PPersona.ForeColor = Color.FromArgb(220, 220, 220);
            toolStripMenuItem_ApplyP3PPersona.Name = "toolStripMenuItem_ApplyP3PPersona";
            toolStripMenuItem_ApplyP3PPersona.Size = new Size(173, 26);
            toolStripMenuItem_ApplyP3PPersona.Text = "P3P Persona";
            toolStripMenuItem_ApplyP3PPersona.Click += ApplyP3PPersona_Click;
            // 
            // toolStripMenuItem_ApplyP3PStrega
            // 
            toolStripMenuItem_ApplyP3PStrega.BackColor = Color.FromArgb(60, 63, 65);
            toolStripMenuItem_ApplyP3PStrega.ForeColor = Color.FromArgb(220, 220, 220);
            toolStripMenuItem_ApplyP3PStrega.Name = "toolStripMenuItem_ApplyP3PStrega";
            toolStripMenuItem_ApplyP3PStrega.Size = new Size(173, 26);
            toolStripMenuItem_ApplyP3PStrega.Text = "P3P Strega";
            toolStripMenuItem_ApplyP3PStrega.Click += ApplyP3PStrega_Click;
            // 
            // toolStripMenuItem_ReorderP3P
            // 
            toolStripMenuItem_ReorderP3P.BackColor = Color.FromArgb(60, 63, 65);
            toolStripMenuItem_ReorderP3P.DropDownItems.AddRange(new ToolStripItem[] { toolStripMenuItem_ReorderP4GProtag, toolStripMenuItem_ReorderP4GParty, toolStripMenuItem_ReorderP4GPersona, toolStripMenuItem_ReorderP4GCulprit, toolStripMenuItem_ReorderP3PProtag, toolStripMenuItem_ReorderP3PParty, toolStripMenuItem_ReorderP3PPersona, toolStripMenuItem_ReorderP3PStrega });
            toolStripMenuItem_ReorderP3P.ForeColor = Color.FromArgb(220, 220, 220);
            toolStripMenuItem_ReorderP3P.Name = "toolStripMenuItem_ReorderP3P";
            toolStripMenuItem_ReorderP3P.Size = new Size(190, 26);
            toolStripMenuItem_ReorderP3P.Text = "Reorder To...";
            // 
            // toolStripMenuItem_ReorderP4GProtag
            // 
            toolStripMenuItem_ReorderP4GProtag.BackColor = Color.FromArgb(60, 63, 65);
            toolStripMenuItem_ReorderP4GProtag.ForeColor = Color.FromArgb(220, 220, 220);
            toolStripMenuItem_ReorderP4GProtag.Name = "toolStripMenuItem_ReorderP4GProtag";
            toolStripMenuItem_ReorderP4GProtag.Size = new Size(175, 26);
            toolStripMenuItem_ReorderP4GProtag.Text = "P4G Protag";
            toolStripMenuItem_ReorderP4GProtag.Click += ReorderP4GProtag_Click;
            // 
            // toolStripMenuItem_ReorderP4GParty
            // 
            toolStripMenuItem_ReorderP4GParty.BackColor = Color.FromArgb(60, 63, 65);
            toolStripMenuItem_ReorderP4GParty.ForeColor = Color.FromArgb(220, 220, 220);
            toolStripMenuItem_ReorderP4GParty.Name = "toolStripMenuItem_ReorderP4GParty";
            toolStripMenuItem_ReorderP4GParty.Size = new Size(175, 26);
            toolStripMenuItem_ReorderP4GParty.Text = "P4G Party";
            toolStripMenuItem_ReorderP4GParty.Click += ReorderP4GParty_Click;
            // 
            // toolStripMenuItem_ReorderP4GPersona
            // 
            toolStripMenuItem_ReorderP4GPersona.BackColor = Color.FromArgb(60, 63, 65);
            toolStripMenuItem_ReorderP4GPersona.ForeColor = Color.FromArgb(220, 220, 220);
            toolStripMenuItem_ReorderP4GPersona.Name = "toolStripMenuItem_ReorderP4GPersona";
            toolStripMenuItem_ReorderP4GPersona.Size = new Size(175, 26);
            toolStripMenuItem_ReorderP4GPersona.Text = "P4G Persona";
            toolStripMenuItem_ReorderP4GPersona.Click += ReorderP4GPersona_Click;
            // 
            // toolStripMenuItem_ReorderP4GCulprit
            // 
            toolStripMenuItem_ReorderP4GCulprit.BackColor = Color.FromArgb(60, 63, 65);
            toolStripMenuItem_ReorderP4GCulprit.ForeColor = Color.FromArgb(220, 220, 220);
            toolStripMenuItem_ReorderP4GCulprit.Name = "toolStripMenuItem_ReorderP4GCulprit";
            toolStripMenuItem_ReorderP4GCulprit.Size = new Size(175, 26);
            toolStripMenuItem_ReorderP4GCulprit.Text = "P4G Culprpit";
            toolStripMenuItem_ReorderP4GCulprit.Click += ReorderP4GCulprit_Click;
            // 
            // toolStripMenuItem_ReorderP3PProtag
            // 
            toolStripMenuItem_ReorderP3PProtag.BackColor = Color.FromArgb(60, 63, 65);
            toolStripMenuItem_ReorderP3PProtag.ForeColor = Color.FromArgb(220, 220, 220);
            toolStripMenuItem_ReorderP3PProtag.Name = "toolStripMenuItem_ReorderP3PProtag";
            toolStripMenuItem_ReorderP3PProtag.Size = new Size(175, 26);
            toolStripMenuItem_ReorderP3PProtag.Text = "P3P Protag";
            toolStripMenuItem_ReorderP3PProtag.Click += ReorderP3PProtag_Click;
            // 
            // toolStripMenuItem_ReorderP3PParty
            // 
            toolStripMenuItem_ReorderP3PParty.BackColor = Color.FromArgb(60, 63, 65);
            toolStripMenuItem_ReorderP3PParty.ForeColor = Color.FromArgb(220, 220, 220);
            toolStripMenuItem_ReorderP3PParty.Name = "toolStripMenuItem_ReorderP3PParty";
            toolStripMenuItem_ReorderP3PParty.Size = new Size(175, 26);
            toolStripMenuItem_ReorderP3PParty.Text = "P3P Party";
            toolStripMenuItem_ReorderP3PParty.Click += ReorderP3PParty_Click;
            // 
            // toolStripMenuItem_ReorderP3PPersona
            // 
            toolStripMenuItem_ReorderP3PPersona.BackColor = Color.FromArgb(60, 63, 65);
            toolStripMenuItem_ReorderP3PPersona.ForeColor = Color.FromArgb(220, 220, 220);
            toolStripMenuItem_ReorderP3PPersona.Name = "toolStripMenuItem_ReorderP3PPersona";
            toolStripMenuItem_ReorderP3PPersona.Size = new Size(175, 26);
            toolStripMenuItem_ReorderP3PPersona.Text = "P3P Persona";
            toolStripMenuItem_ReorderP3PPersona.Click += ReorderP3PPersona_Click;
            // 
            // toolStripMenuItem_ReorderP3PStrega
            // 
            toolStripMenuItem_ReorderP3PStrega.BackColor = Color.FromArgb(60, 63, 65);
            toolStripMenuItem_ReorderP3PStrega.ForeColor = Color.FromArgb(220, 220, 220);
            toolStripMenuItem_ReorderP3PStrega.Name = "toolStripMenuItem_ReorderP3PStrega";
            toolStripMenuItem_ReorderP3PStrega.Size = new Size(175, 26);
            toolStripMenuItem_ReorderP3PStrega.Text = "P3P Strega";
            toolStripMenuItem_ReorderP3PStrega.Click += ReorderP3PStrega_Click;
            // 
            // darkContextMenu_RightClick
            // 
            darkContextMenu_RightClick.BackColor = Color.FromArgb(60, 63, 65);
            darkContextMenu_RightClick.ForeColor = Color.FromArgb(220, 220, 220);
            darkContextMenu_RightClick.ImageScalingSize = new Size(20, 20);
            darkContextMenu_RightClick.Items.AddRange(new ToolStripItem[] { moveUpToolStripMenuItem, moveDownToolStripMenuItem, exportToolStripMenuItem, addToolStripMenuItem, replaceToolStripMenuItem, deleteToolStripMenuItem });
            darkContextMenu_RightClick.Name = "darkContextMenu_RightClick";
            darkContextMenu_RightClick.Size = new Size(159, 148);
            // 
            // moveUpToolStripMenuItem
            // 
            moveUpToolStripMenuItem.BackColor = Color.FromArgb(60, 63, 65);
            moveUpToolStripMenuItem.ForeColor = Color.FromArgb(220, 220, 220);
            moveUpToolStripMenuItem.Name = "moveUpToolStripMenuItem";
            moveUpToolStripMenuItem.Size = new Size(158, 24);
            moveUpToolStripMenuItem.Text = "Move Up";
            moveUpToolStripMenuItem.Visible = false;
            moveUpToolStripMenuItem.Click += MoveUp_Click;
            // 
            // moveDownToolStripMenuItem
            // 
            moveDownToolStripMenuItem.BackColor = Color.FromArgb(60, 63, 65);
            moveDownToolStripMenuItem.ForeColor = Color.FromArgb(220, 220, 220);
            moveDownToolStripMenuItem.Name = "moveDownToolStripMenuItem";
            moveDownToolStripMenuItem.Size = new Size(158, 24);
            moveDownToolStripMenuItem.Text = "Move Down";
            moveDownToolStripMenuItem.Visible = false;
            moveDownToolStripMenuItem.Click += MoveDown_Click;
            // 
            // exportToolStripMenuItem
            // 
            exportToolStripMenuItem.BackColor = Color.FromArgb(60, 63, 65);
            exportToolStripMenuItem.ForeColor = Color.FromArgb(220, 220, 220);
            exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            exportToolStripMenuItem.Size = new Size(158, 24);
            exportToolStripMenuItem.Text = "Export";
            exportToolStripMenuItem.Click += ExportElement_Click;
            // 
            // addToolStripMenuItem
            // 
            addToolStripMenuItem.BackColor = Color.FromArgb(60, 63, 65);
            addToolStripMenuItem.ForeColor = Color.FromArgb(220, 220, 220);
            addToolStripMenuItem.Name = "addToolStripMenuItem";
            addToolStripMenuItem.Size = new Size(158, 24);
            addToolStripMenuItem.Text = "Add";
            addToolStripMenuItem.Click += Add_Click;
            // 
            // replaceToolStripMenuItem
            // 
            replaceToolStripMenuItem.BackColor = Color.FromArgb(60, 63, 65);
            replaceToolStripMenuItem.ForeColor = Color.FromArgb(220, 220, 220);
            replaceToolStripMenuItem.Name = "replaceToolStripMenuItem";
            replaceToolStripMenuItem.Size = new Size(158, 24);
            replaceToolStripMenuItem.Text = "Replace";
            replaceToolStripMenuItem.Click += Replace_Click;
            // 
            // deleteToolStripMenuItem
            // 
            deleteToolStripMenuItem.BackColor = Color.FromArgb(60, 63, 65);
            deleteToolStripMenuItem.ForeColor = Color.FromArgb(220, 220, 220);
            deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            deleteToolStripMenuItem.Size = new Size(158, 24);
            deleteToolStripMenuItem.Text = "Delete";
            deleteToolStripMenuItem.Click += Delete_Click;
            // 
            // tlp_Main
            // 
            tlp_Main.ColumnCount = 2;
            tlp_Main.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            tlp_Main.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));
            tlp_Main.Controls.Add(darkTreeView_Main, 0, 0);
            tlp_Main.Controls.Add(propertyGrid_Main, 1, 0);
            tlp_Main.Dock = DockStyle.Fill;
            tlp_Main.Location = new Point(0, 28);
            tlp_Main.Margin = new Padding(3, 4, 3, 4);
            tlp_Main.Name = "tlp_Main";
            tlp_Main.RowCount = 2;
            tlp_Main.RowStyles.Add(new RowStyle(SizeType.Percent, 30F));
            tlp_Main.RowStyles.Add(new RowStyle(SizeType.Percent, 70F));
            tlp_Main.Size = new Size(1039, 874);
            tlp_Main.TabIndex = 4;
            // 
            // MainForm
            // 
            AllowDrop = true;
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(60, 63, 65);
            ClientSize = new Size(1039, 902);
            Controls.Add(tlp_Main);
            Controls.Add(darkMenuStrip_Main);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = darkMenuStrip_Main;
            Margin = new Padding(5, 6, 5, 6);
            MinimumSize = new Size(877, 849);
            Name = "MainForm";
            Text = "P4GMOdel";
            Resize += MainForm_Resize;
            darkMenuStrip_Main.ResumeLayout(false);
            darkMenuStrip_Main.PerformLayout();
            darkContextMenu_RightClick.ResumeLayout(false);
            tlp_Main.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private ContextMenuStrip contextMenuStrip_Animations;
        private ToolStripMenuItem toolStripMenuItem_Add;
        private ToolStripMenuItem toolStripMenuItem_Remove;
        private ToolStripMenuItem toolStripMenuItem_Rename;
        private ToolStripTextBox toolStripTextBox_Rename;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem newToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem saveAsToolStripMenuItem;
        private ToolStripMenuItem closeToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem settingsToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private ToolStripMenuItem tutorialToolStripMenuItem;
        private ToolStripMenuItem refreshToolStripMenuItem;
        private PropertyGrid propertyGrid_Main;
        private DarkUI.Controls.DarkTreeView darkTreeView_Main;
        private DarkUI.Controls.DarkMenuStrip darkMenuStrip_Main;
        private ToolStripMenuItem toolStripMenuItem_File;
        private ToolStripMenuItem toolStripMenuItem_New;
        private ToolStripMenuItem toolStripMenuItem_Open;
        private ToolStripMenuItem toolStripMenuItem_Save;
        private ToolStripMenuItem toolStripMenuItem_Exit;
        private ToolStripMenuItem toolStripMenuItem_Settings;
        private ToolStripMenuItem toolStripMenuItem_Refresh;
        private ToolStripMenuItem toolStripMenuItem_Help;
        private ToolStripMenuItem toolStripMenuItem_About;
        private ToolStripMenuItem toolStripMenuItem_Tutorial;
        private ToolStripMenuItem toolStripMenuItem_SaveAs;
        private DarkUI.Controls.DarkContextMenu darkContextMenu_RightClick;
        private ToolStripMenuItem moveUpToolStripMenuItem;
        private ToolStripMenuItem moveDownToolStripMenuItem;
        private ToolStripMenuItem exportToolStripMenuItem;
        private ToolStripMenuItem addToolStripMenuItem;
        private ToolStripMenuItem replaceToolStripMenuItem;
        private ToolStripMenuItem deleteToolStripMenuItem;
        private ToolStripMenuItem toolStripMenuItem_Export;
        private ToolStripMenuItem animationPresetsToolStripMenuItem;
        private ToolStripMenuItem toolStripMenuItem_ReorderP3P;
        private ToolStripMenuItem applyNamesToolStripMenuItem;
        private ToolStripMenuItem toolStripMenuItem_ApplyP4GProtag;
        private ToolStripMenuItem toolStripMenuItem_ApplyP4GParty;
        private ToolStripMenuItem toolStripMenuItem_ApplyP4GPersona;
        private ToolStripMenuItem toolStripMenuItem_ApplyP4GCulprit;
        private ToolStripMenuItem toolStripMenuItem_ApplyP3PProtag;
        private ToolStripMenuItem toolStripMenuItem_ApplyP3PParty;
        private ToolStripMenuItem toolStripMenuItem_ApplyP3PPersona;
        private ToolStripMenuItem toolStripMenuItem_ApplyP3PStrega;
        private ToolStripMenuItem toolStripMenuItem_ReorderP4GProtag;
        private ToolStripMenuItem toolStripMenuItem_ReorderP4GParty;
        private ToolStripMenuItem toolStripMenuItem_ReorderP4GPersona;
        private ToolStripMenuItem toolStripMenuItem_ReorderP4GCulprit;
        private ToolStripMenuItem toolStripMenuItem_ReorderP3PProtag;
        private ToolStripMenuItem toolStripMenuItem_ReorderP3PParty;
        private ToolStripMenuItem toolStripMenuItem_ReorderP3PPersona;
        private ToolStripMenuItem toolStripMenuItem_ReorderP3PStrega;
        private TableLayoutPanel tlp_Main;
    }
}

