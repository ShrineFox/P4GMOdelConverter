using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using TGE.IO;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Reflection;
using System.Runtime.Remoting.Contexts;
using System.Threading;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization;
using DarkUI.Controls;
using AtlusFileSystemLibrary;
using AtlusFileSystemLibrary.FileSystems.PAK;
using AtlusFileSystemLibrary.Common.IO;
using AmicitiaLibrary.FileSystems.AMD;

namespace P4GMOdel
{
    public partial class MainForm : Form
    {
        List<List<string>> animationPresets = new List<List<string>>() { //null, p4g protag, p4g party, p4g persona, p4g culprit, p3p protag, p3p party, p3p persona, p3p strega
            new List<string>{ "" },
            new List<string> { "Idle", "Idle 2", "Run", "Attack", "Attack Critical", "Placeholder 4", "Persona Change", "Persona Summon 1", "Persona Summon 2", "Persona Summon 3", "Persona Summon 4", "Guard", "Dodge", "Low HP", "Damaged", "Miss Attack", "Knocked Down", "Down", "Get Back Up", "Killed", "Revived", "Use Item", "Victory", "Pushed Out of the Way", "Placeholder 23", "Helped Up", "Placeholder 25", "Idle (Duplicate)" },
            new List<string>{ "Idle (Active)", "Idle 2", "Run", "Attack", "Attack Critical", "Special Attack 1", "Special Attack 2", "Persona Summon 1", "Persona Summon 2", "Persona Summon 3", "Persona Summon 4", "Guard", "Dodge", "Low HP", "Damaged", "Miss Attack", "Knocked Down", "Down", "Get Back Up", "Killed", "Revived", "Use Item", "Victory", "Knock Out of the Way", "Help Up Party Member", "Helped Up", "Yell At Party Member", "Idle (Still)", "Group Summon 1", "Group Summon 2", "Group Summon 3", "Group Summon 4" },
            new List<string>{ "Physical Attack", "Magic Attack", "Physical Attack", "Magic Attack", "Idle" },
            new List<string>{ "Idle", "Low HP", "Damaged", "Run", "Attack", "Placeholder 4", "Dialog Animation", "Miss Attack", "Knocked Down", "Down", "Get Back Up", "Killed", "Persona Summon 1", "Persona Summon 2", "Persona Summon 3", "Persona Summon 4", "Dodge", "Idle 2" },
            new List<string>{ "Idle", "Low HP", "Damaged", "Run", "Attack", "Attack 2 Critical", "Attack 3 Critical", "Miss Attack", "Knocked Down", "Down", "Get Back Up", "Persona Summon 1", "Persona Summon 2", "Persona Summon 3", "Persona Summon 4", "Idle 2", "Use Item", "Dodge", "Revived", "Victory", "Killed", "Fusion Attack", "Guard", "Knock Out of the Way" },
            new List<string>{ "Physical Attack", "Magic Attack", "Idle", "Magic attack" },
            new List<string>{ "Idle", "Low HP", "Damaged", "Run", "Attack", "Placeholder 4", "Killed", "Miss Attack", "Knocked Down", "Down", "Get Back Up", "Persona Summon 1", "Persona Summon 2", "Persona Summon 3", "Persona Summon 4", "Idle 2", "Placeholder 15", "Low HP (Duplicate)", "Dodge" }
        };
        SettingsForm.Settings settings;
        Model model;
        DarkTreeNode lastSelectedTreeNode;
        

        public MainForm()
        {
            InitializeComponent();
            //Load settings
            if (File.Exists("settings.yml"))
            {
                var deserializer = new DeserializerBuilder().WithNamingConvention(PascalCaseNamingConvention.Instance).Build();
                settings = deserializer.Deserialize<SettingsForm.Settings>(File.ReadAllText("settings.yml"));
            }
            else
                settings = new SettingsForm.Settings();
        }

        private void OpenFile(string path)
        {
            if (model != null && !ConfirmDelete())
                return;

            //Re-initialize variables and form
            model = new Model();
            model.Path = path;
            lastSelectedTreeNode = null;
            string extensionlessPath = Path.Combine(Path.GetDirectoryName(model.Path), Path.GetFileNameWithoutExtension(model.Path));

            //Open file
            if (Path.GetExtension(model.Path.ToLower()) == ".pac")
            {
                //Extract AMD or GMO from PAC
                PAKFileSystem pak = new PAKFileSystem();
                if (PAKFileSystem.TryOpen(model.Path, out pak))
                {
                    bool extracted = false;
                    foreach (var pakFile in pak.EnumerateFiles())
                    {
                        if (pakFile.ToLower().EndsWith(".amd"))
                        {
                            model.Path = extensionlessPath + ".amd";
                            using (var stream = FileUtils.Create(model.Path))
                            using (var inputStream = pak.OpenFile(pakFile))
                                inputStream.CopyTo(stream);
                            extracted = true;
                        }
                        else if (pakFile.ToUpper().Equals("MODEL_DATA"))
                        {
                            model.Path = extensionlessPath + ".gmo";
                            using (var stream = FileUtils.Create(model.Path))
                            using (var inputStream = pak.OpenFile(pakFile))
                                inputStream.CopyTo(stream);
                            extracted = true;
                        }
                    }
                    if (!extracted)
                        MessageBox.Show("Could not find AMD or GMO model data in PAC archive!");
                    else
                        while (!File.Exists(model.Path)) { Thread.Sleep(100); }
                }
                else
                    MessageBox.Show("Failed to open PAC archive!");
            }
            if (Path.GetExtension(model.Path.ToUpper()) == ".AMD")
            {
                //Extract GMO
                if (File.Exists(model.Path))
                {
                    bool extracted = false;
                    AmdFile amd = new AmdFile(model.Path);
                    foreach (var chunk in amd.Chunks)
                    {
                        if (chunk.Tag.ToUpper().Equals("MODEL_DATA"))
                        {
                            model.Path = extensionlessPath + ".gmo";
                            File.WriteAllBytes(model.Path, chunk.Data);
                            extracted = true;
                        }
                    }
                    if (!extracted)
                        MessageBox.Show("Could not find GMO model data in AMD archive!");
                    else
                        while (!File.Exists(model.Path)) { Thread.Sleep(100); }
                }
                else
                    MessageBox.Show("Failed to open AMD archive!");
            }
            if (Path.GetExtension(model.Path).ToLower() == ".dae" || Path.GetExtension(model.Path).ToLower() == ".smd" || Path.GetExtension(model.Path).ToLower() == ".fbx")
            {
                if (settings.ConvertToFBX || Path.GetExtension(model.Path).ToLower() != ".fbx")
                    model.Path = Tools.CreateOptimizedFBX(model.Path, settings); //Optimize FBX with settings or convert to FBX
                if (settings.ConvertToGMO)
                {
                    Tools.GMOConv(model.Path); //Convert FBX directly to GMO without fixes first
                    model.Path = extensionlessPath + ".gmo";
                }
            }
            if (Path.GetExtension(model.Path).ToLower() == ".gmo" || Path.GetExtension(model.Path).ToLower() == ".fbx")
            {
                if (!File.Exists(model.Path))
                    MessageBox.Show("Error: Model file was not found!");
                else
                {
                    if (settings.ExtractTextures && Path.GetExtension(model.Path).ToLower() == ".gmo")
                        Tools.ExtractTextures(model.Path); //Extract TM2 Textures
                    Tools.GMOTool(model.Path, true); //Create MDS
                    model.Path = extensionlessPath + ".mds";
                }
            }
            if (Path.GetExtension(model.Path.ToLower()) == ".mds")
            {
                if (!File.Exists(model.Path))
                {
                    toolStripMenuItem_Save.Enabled = false;
                    toolStripMenuItem_SaveAs.Enabled = false;
                    toolStripMenuItem_Export.Enabled = false;
                    MessageBox.Show("Error: No .MDS file found! One may not have been generated due to an error with GMOTool.");
                }
                else
                {
                    var mdsLines = File.ReadAllLines(model.Path).ToList();
                    model = Model.Deserialize(model, mdsLines.ToArray(), settings);
                    RefreshTreeview();
                    toolStripMenuItem_Save.Enabled = true;
                    toolStripMenuItem_SaveAs.Enabled = true;
                    toolStripMenuItem_Export.Enabled = true;
                }
            }
            //Update filename in title
            this.Text = "P4GMOdel - " + Path.GetFileName(model.Path);
        }

        private void RefreshTreeview()
        {
            //Clear PropertryGridView
            propertyGrid_Main.SelectedObject = new Object();
            propertyGrid_Main.Update();
            //Clear treeview and add serialized model elements to it
            if (model != null)
            {
                darkTreeView_Main.Nodes.Clear();
                darkTreeView_Main.Nodes.Add(new DarkTreeNode() { Text = "Model", Tag = model });
                darkTreeView_Main.Nodes.First().Nodes.Add(new DarkTreeNode() { Text = "BlindData", Tag = model.BlindData });
                darkTreeView_Main.Nodes.First().Nodes.Add(new DarkTreeNode() { Text = "Bones", Tag = model.Bones });
                foreach (Bone bone in model.Bones)
                    darkTreeView_Main.Nodes.First().Nodes.First(x => x.Text == "Bones").Nodes.Add(new DarkTreeNode() { Text = bone.Name, Tag = bone });
                darkTreeView_Main.Nodes.First().Nodes.Add(new DarkTreeNode() { Text = "Parts", Tag = model.Parts });
                foreach (Part part in model.Parts)
                    darkTreeView_Main.Nodes.First().Nodes.First(x => x.Text == "Parts").Nodes.Add(new DarkTreeNode() { Text = part.Name, Tag = part });
                darkTreeView_Main.Nodes.First().Nodes.Add(new DarkTreeNode() { Text = "Materials", Tag = model.Materials });
                foreach (Material material in model.Materials)
                    darkTreeView_Main.Nodes.First().Nodes.First(x => x.Text == "Materials").Nodes.Add(new DarkTreeNode() { Text = material.Name, Tag = material });
                darkTreeView_Main.Nodes.First().Nodes.Add(new DarkTreeNode() { Text = "Textures", Tag = model.Textures });
                foreach (Texture texture in model.Textures)
                    darkTreeView_Main.Nodes.First().Nodes.First(x => x.Text == "Textures").Nodes.Add(new DarkTreeNode() { Text = texture.Name, Tag = texture });
                darkTreeView_Main.Nodes.First().Nodes.Add(new DarkTreeNode() { Text = "Animations", Tag = model.Animations });
                foreach (Animation animation in model.Animations)
                    darkTreeView_Main.Nodes.First().Nodes.First(x => x.Text == "Animations").Nodes.Add(new DarkTreeNode() { Text = animation.Name, Tag = animation });
            }
            //Expand Model node
            darkTreeView_Main.Nodes[0].Expanded = true;
            //Set last selected node to Model by default
            if (lastSelectedTreeNode == null)
                lastSelectedTreeNode = darkTreeView_Main.Nodes[0];
            //Expand nodes above selection
            var node = GetNodeFromPath(darkTreeView_Main.Nodes[0], lastSelectedTreeNode.FullPath);
            if (node != null)
            {
                if (node.ParentNode != null)
                    node.ParentNode.Expanded = true;
                if (node.ParentNode.ParentNode != null)
                    node.ParentNode.ParentNode.Expanded = true;
                //Reselect and scroll to it
                darkTreeView_Main.SelectedNodes[0] = node;
                darkTreeView_Main.SelectedNodes[0].EnsureVisible();
                //Update PropertyGrid
                propertyGrid_Main.SelectedObject = node.Tag;
                propertyGrid_Main.Update();
            }
        }

        private void TreeView_MouseClick(object sender, MouseEventArgs e)
        {
            //Update object if changed
            propertyGrid_Main.SelectedObject = propertyGrid_Main.SelectedObject;

            var selectedNodes = darkTreeView_Main.SelectedNodes;
            if (selectedNodes.Count > 0)
            {
                lastSelectedTreeNode = selectedNodes[0];
                //Assign selected object to propertygrid
                propertyGrid_Main.SelectedObject = lastSelectedTreeNode.Tag;
                //Show right click menu
                string[] hideOptions = new string[] { "Model", "BlindData", "Bones", "Parts", "Materials", "Textures", "Animations" };
                if (!hideOptions.Any(x => x.Equals(lastSelectedTreeNode.Text)))
                {
                    moveUpToolStripMenuItem.Visible = true;
                    moveDownToolStripMenuItem.Visible = true;
                }
                else
                {
                    moveUpToolStripMenuItem.Visible = false;
                    moveDownToolStripMenuItem.Visible = false;
                }
                if (e.Button.Equals(MouseButtons.Right))
                    darkContextMenu_RightClick.Show(this, new Point(e.X + ((Control)sender).Left + 4, e.Y + ((Control)sender).Top + 4));
            }
        }

        private void Refresh(object sender, EventArgs e)
        {
            RefreshTreeview();
        }

        /* DRAG AND DROP */

        private void Treeview_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void Treeview_DragDrop(object sender, DragEventArgs e)
        {
            string[] fileList = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            if (fileList.Length > 0)
                OpenFile(fileList[0]);
        }

        /* TOOLSTRIP */

        private void New_Click(object sender, EventArgs e)
        {
            if (model != null && !ConfirmDelete())
                return;
            model = new Model();
            lastSelectedTreeNode = null;
            RefreshTreeview();
            toolStripMenuItem_Save.Enabled = true;
            toolStripMenuItem_SaveAs.Enabled = true;
            toolStripMenuItem_Export.Enabled = true;
        }

        private void Save_Click(object sender, EventArgs e)
        {
            if (model.Path.ToLower().EndsWith(".mds"))
            {
                File.WriteAllText(Model.Serialize(model, settings), model.Path);
                MessageBox.Show("MDS file saved!");
            }
        }

        private void SaveAs_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.Filters.Add(new CommonFileDialogFilter("MDS", "*.mds"));
            dialog.Title = "Save MDS...";
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                File.WriteAllText(dialog.FileName, Model.Serialize(model, settings));
                MessageBox.Show("MDS file saved!");
            }
        }

        public static bool ConfirmDelete()
        {
            DialogResult result = MessageBox.Show("Any changes you're currently working on will be lost! Are you sure?", "Unsaved Changes", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
                return true;
            else
                return false;
        }

        private void Open_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.Filters.Add(new CommonFileDialogFilter("P4G Model or Archive", "*.mds, *.gmo, *.amd, *.pac, *.fbx, *.dae, *.smd"));
            dialog.Title = "Open Model or Archive...";
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                OpenFile(dialog.FileName);
        }

        private void Export_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.Filters.Add(new CommonFileDialogFilter("P4G Model or Archive", "*.gmo, *.amd, *.pac"));
            dialog.Title = "Save Model or Archive...";
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                Tools.CreateGMO(dialog.FileName, model, settings);
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            if (model != null && !ConfirmDelete())
                return;
            Environment.Exit(0);
        }

        private void Settings_Click(object sender, EventArgs e)
        {
            using (var dialog = new SettingsForm())
            {
                if (dialog.ShowDialog() != DialogResult.OK)
                    return;
                var deserializer = new DeserializerBuilder().WithNamingConvention(PascalCaseNamingConvention.Instance).Build();
                settings = deserializer.Deserialize<SettingsForm.Settings>(File.ReadAllText("settings.yml"));
            }
        }

        /* UTILITIES */
        public DarkTreeNode GetNodeFromPath(DarkTreeNode node, string path)
        {
            DarkTreeNode foundNode = null;
            foreach (DarkTreeNode tn in node.Nodes)
            {
                if (tn.FullPath == path)
                    return tn;
                else if (tn.Nodes.Count > 0)
                    foundNode = GetNodeFromPath(tn, path);
                if (foundNode != null)
                    return foundNode;
            }
            return null;
        }
    }
}
