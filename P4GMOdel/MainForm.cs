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
using System.Runtime.InteropServices;
using System.Collections;

namespace P4GMOdel
{
    public partial class MainForm : Form
    {
        SettingsForm.Settings settings;
        Model model;
        DarkTreeNode lastSelectedTreeNode;
        public static IntPtr panelHandle;
        public static int formWidth;
        public static int formHeight;
        public static bool dataChanged;
        public static bool viewerUpdated;

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
            panelHandle = panel_GMOView.Handle;
            viewerUpdated = false;
            DataChanged(false);
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnProcessExit);
        }

        private void OnProcessExit(object sender, EventArgs e)
        {
            ModelViewer.CloseProcess();
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
                    Tools.GMOConv(model.Path, settings); //Convert FBX directly to GMO without fixes first
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
                    Tools.GMOTool(model.Path, true, settings); //Create MDS
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
                    ModelViewer.Update(model, settings);
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
            if (darkTreeView_Main.Nodes.Count > 0)
            {
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
        }

        private void TreeView_MouseClick(object sender, MouseEventArgs e)
        {
            //Update object if changed
            UpdatePropertyGrid();
            //Update modelviewer and treeview if "Model" clicked
            if (lastSelectedTreeNode.Text == "Model" && dataChanged)
            {
                RefreshTreeview();
                if (!viewerUpdated)
                    ModelViewer.Update(model, settings);
            }
            //Show context menu if right clicked
            if (e.Button.Equals(MouseButtons.Right))
                darkContextMenu_RightClick.Show(this, new Point(e.X + ((Control)sender).Left + 4, e.Y + ((Control)sender).Top + 4));
        }

        private void UpdatePropertyGrid()
        {
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
            }
        }

        private void TreeView_SelectedNodesChanged(object sender, EventArgs e)
        {
            //Update object if changed
            UpdatePropertyGrid();
        }

        private void Refresh(object sender, EventArgs e)
        {
            RefreshTreeview();
        }

        private void PropertyGrid_ValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            DataChanged(true);
        }


        private void MainForm_Resize(object sender, EventArgs e)
        {
            int formWidth = Width;
            int formHeight = Height;
            if (ModelViewer.sessions.Count > 0)
            {
                Process pHandle = (Process)ModelViewer.sessions["GMOView"];
                ModelViewer.MoveWindow(pHandle.MainWindowHandle, 0, 0, Width, Height, true);
            }
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
            DataChanged(false);
            ModelViewer.CloseProcess();
        }

        private void Save_Click(object sender, EventArgs e)
        {
            if (model.Path.ToLower().EndsWith(".mds"))
            {
                File.WriteAllText(model.Path, Model.Serialize(model, settings));
                MessageBox.Show("MDS file saved!");
                DataChanged(false);
            }
        }

        private void SaveAs_Click(object sender, EventArgs e)
        {
            CommonSaveFileDialog dialog = new CommonSaveFileDialog();
            dialog.Filters.Add(new CommonFileDialogFilter("MDS", "*.mds"));
            dialog.Title = "Save MDS...";
            dialog.DefaultFileName = $"{Path.GetFileNameWithoutExtension(model.Path)}.mds";
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                File.WriteAllText(dialog.FileName, Model.Serialize(model, settings));
                MessageBox.Show("MDS file saved!");
                DataChanged(false);
            }
        }

        public static bool ConfirmDelete()
        {
            if (dataChanged)
            {
                DialogResult result = MessageBox.Show("Any changes you're currently working on will be lost! Are you sure?", "Unsaved Changes", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                    return true;
                else
                    return false;
            }
            else
                return true;
        }

        private void Open_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.Filters.Add(new CommonFileDialogFilter("All Types", "*.mds, *.gmo, *.fbx, *.dae, *.smd, *.amd, *.pac"));
            dialog.Filters.Add(new CommonFileDialogFilter("GMO Data", "*.mds"));
            dialog.Filters.Add(new CommonFileDialogFilter("GMO Model", "*.gmo"));
            dialog.Filters.Add(new CommonFileDialogFilter("Assimp Model", "*.fbx, *.dae, *.smd"));
            dialog.Filters.Add(new CommonFileDialogFilter("P4G Model Container", "*.amd"));
            dialog.Filters.Add(new CommonFileDialogFilter("Atlus Archive", "*.pac"));
            dialog.Title = "Open Model or Archive...";
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                OpenFile(dialog.FileName);
        }

        private void Export_Click(object sender, EventArgs e)
        {
            CommonSaveFileDialog dialog = new CommonSaveFileDialog();
            dialog.Filters.Add(new CommonFileDialogFilter("GMO Model", "*.gmo"));
            dialog.Filters.Add(new CommonFileDialogFilter("P4G Model Container", "*.amd"));
            dialog.Filters.Add(new CommonFileDialogFilter("Atlus Archive", "*.pac"));
            dialog.Title = "Save Model or Archive...";
            dialog.DefaultFileName = $"{Path.GetFileNameWithoutExtension(model.Path)}.gmo";
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

        /* CONTEXT MENU */
        private void MoveUp_Click(object sender, EventArgs e)
        {
            int index = 0;
            switch (lastSelectedTreeNode.ParentNode.Text)
            {
                case "Bones":
                    Bone selectedBone = model.Bones.First(x => x.Equals(lastSelectedTreeNode.Tag));
                    index = model.Bones.IndexOf(selectedBone);
                    if (index - 1 > -1)
                    {
                        model.Bones.RemoveAt(index);
                        model.Bones.Insert(index - 1, selectedBone);
                    }
                    break;
                case "Parts":
                    Part selectedPart = model.Parts.First(x => x.Equals(lastSelectedTreeNode.Tag));
                    index = model.Parts.IndexOf(selectedPart);
                    if (index - 1 > -1)
                    {
                        model.Parts.RemoveAt(index);
                        model.Parts.Insert(index - 1, selectedPart);
                    }
                    break;
                case "Materials":
                    Material selectedMat = model.Materials.First(x => x.Equals(lastSelectedTreeNode.Tag));
                    index = model.Materials.IndexOf(selectedMat);
                    if (index - 1 > -1)
                    {
                        model.Materials.RemoveAt(index);
                        model.Materials.Insert(index - 1, selectedMat);
                    }
                    break;
                case "Textures":
                    Texture selectedTex = model.Textures.First(x => x.Equals(lastSelectedTreeNode.Tag));
                    index = model.Textures.IndexOf(selectedTex);
                    if (index - 1 > -1)
                    {
                        model.Textures.RemoveAt(index);
                        model.Textures.Insert(index - 1, selectedTex);
                    }
                    break;
                case "Animations":
                    Animation selectedAnim = model.Animations.First(x => x.Equals(lastSelectedTreeNode.Tag));
                    index = model.Animations.IndexOf(selectedAnim);
                    if (index - 1 > -1)
                    {
                        model.Animations.RemoveAt(index);
                        model.Animations.Insert(index - 1, selectedAnim);
                    }
                    break;
                default:
                    return;
            }
            RefreshTreeview();
            DataChanged(true);
        }

        private void MoveDown_Click(object sender, EventArgs e)
        {
            int index = 0;
            switch (lastSelectedTreeNode.ParentNode.Text)
            {
                case "Bones":
                    Bone selectedBone = model.Bones.First(x => x.Equals(lastSelectedTreeNode.Tag));
                    index = model.Bones.IndexOf(selectedBone);
                    if (index + 1 < model.Bones.Count())
                    {
                        model.Bones.RemoveAt(index);
                        model.Bones.Insert(index + 1, selectedBone);
                    }
                    break;
                case "Parts":
                    Part selectedPart = model.Parts.First(x => x.Equals(lastSelectedTreeNode.Tag));
                    index = model.Parts.IndexOf(selectedPart);
                    if (index + 1 < model.Parts.Count())
                    {
                        model.Parts.RemoveAt(index);
                        model.Parts.Insert(index + 1, selectedPart);
                    }
                    break;
                case "Materials":
                    Material selectedMat = model.Materials.First(x => x.Equals(lastSelectedTreeNode.Tag));
                    index = model.Materials.IndexOf(selectedMat);
                    if (index + 1 < model.Materials.Count())
                    {
                        model.Materials.RemoveAt(index);
                        model.Materials.Insert(index + 1, selectedMat);
                    }
                    break;
                case "Textures":
                    Texture selectedTex = model.Textures.First(x => x.Equals(lastSelectedTreeNode.Tag));
                    index = model.Textures.IndexOf(selectedTex);
                    if (index + 1 < model.Textures.Count())
                    {
                        model.Textures.RemoveAt(index);
                        model.Textures.Insert(index + 1, selectedTex);
                    }
                    break;
                case "Animations":
                    Animation selectedAnim = model.Animations.First(x => x.Equals(lastSelectedTreeNode.Tag));
                    index = model.Animations.IndexOf(selectedAnim);
                    if (index + 1 < model.Animations.Count())
                    {
                        model.Animations.RemoveAt(index);
                        model.Animations.Insert(index + 1, selectedAnim);
                    }
                    break;
                default:
                    return;
            }
            RefreshTreeview();
            DataChanged(true);
        }

        private void ExportElement_Click(object sender, EventArgs e)
        {
            Model export = new Model();
            //Export model object as MDS
            switch (lastSelectedTreeNode.Text)
            {
                case "Model":
                    export = model;
                    break;
                case "BlindData":
                    export.BlindData = model.BlindData;
                    break;
                case "Bones":
                    export.Bones = model.Bones;
                    break;
                case "Parts":
                    export.Parts = model.Parts;
                    break;
                case "Materials":
                    export.Materials = model.Materials;
                    break;
                case "Textures":
                    export.Textures = model.Textures;
                    break;
                case "Animations":
                    export.Animations = model.Animations;
                    break;
                default:
                    //Export individual elements as mds
                    switch (lastSelectedTreeNode.ParentNode.Text)
                    {
                        case "Bones":
                            export.Bones.Add(model.Bones.First(x => x.Equals(lastSelectedTreeNode.Tag)));
                            break;
                        case "Parts":
                            export.Parts.Add(model.Parts.First(x => x.Equals(lastSelectedTreeNode.Tag)));
                            break;
                        case "Materials":
                            export.Materials.Add(model.Materials.First(x => x.Equals(lastSelectedTreeNode.Tag)));
                            break;
                        case "Textures":
                            export.Textures.Add(model.Textures.First(x => x.Equals(lastSelectedTreeNode.Tag)));
                            break;
                        case "Animations":
                            export.Animations.Add(model.Animations.First(x => x.Equals(lastSelectedTreeNode.Tag)));
                            break;
                        default:
                            return;
                    }
                    break;
            }
            //Save MDS as...
            CommonSaveFileDialog dialog = new CommonSaveFileDialog();
            dialog.Filters.Add(new CommonFileDialogFilter("MDS", "*.mds"));
            dialog.Title = "Save MDS...";
            dialog.DefaultFileName = $"{lastSelectedTreeNode.Text}.mds";
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                File.WriteAllText(dialog.FileName, Model.Serialize(export, settings));
        }

        private void Add_Click(object sender, EventArgs e)
        {
            Model import = Model.Import(settings, lastSelectedTreeNode.Text);
            //Add selected object
            switch (lastSelectedTreeNode.Text)
            {
                case "Bones":
                    model.Bones.Add(import.Bones.First());
                    break;
                case "Parts":
                    model.Parts.Add(import.Parts.First());
                    break;
                case "Materials":
                    model.Materials.Add(import.Materials.First());
                    break;
                case "Textures":
                    model.Textures.Add(import.Textures.First());
                    break;
                case "Animations":
                    model.Animations.Add(import.Animations.First());
                    break;
                default:
                    switch (lastSelectedTreeNode.ParentNode.Text)
                    {
                        case "Bones":
                            model.Bones.Add(import.Bones.First());
                            break;
                        case "Parts":
                            model.Parts.Add(import.Parts.First());
                            break;
                        case "Materials":
                            model.Materials.Add(import.Materials.First());
                            break;
                        case "Textures":
                            model.Textures.Add(import.Textures.First());
                            break;
                        case "Animations":
                            model.Animations.Add(import.Animations.First());
                            break;
                        default:
                            return;
                    }
                    break;
            }
            RefreshTreeview();
            DataChanged(true);
        }

        private void Replace_Click(object sender, EventArgs e)
        {
            Model import = Model.Import(settings, lastSelectedTreeNode.Text);
            //Replace selected object
            int index = 0;
            switch (lastSelectedTreeNode.Text)
            {
                case "Model":
                    model = import;
                    break;
                case "BlindData":
                    model.BlindData = import.BlindData;
                    break;
                case "Bones":
                    model.Bones = import.Bones;
                    break;
                case "Parts":
                    model.Parts = import.Parts;
                    break;
                case "Materials":
                    model.Materials = import.Materials;
                    break;
                case "Textures":
                    model.Textures = import.Textures;
                    break;
                case "Animations":
                    model.Animations = import.Animations;
                    break;
                default:
                    //Replace individual element
                    switch (lastSelectedTreeNode.ParentNode.Text)
                    {
                        case "Bones":
                            index = model.Bones.IndexOf((Bone)lastSelectedTreeNode.Tag);
                            model.Bones.RemoveAt(index);
                            model.Bones.Insert(index, import.Bones.First());
                            break;
                        case "Parts":
                            index = model.Parts.IndexOf((Part)lastSelectedTreeNode.Tag);
                            model.Parts.RemoveAt(index);
                            model.Parts.Insert(index, import.Parts.First());
                            break;
                        case "Materials":
                            index = model.Materials.IndexOf((Material)lastSelectedTreeNode.Tag);
                            model.Materials.RemoveAt(index);
                            model.Materials.Insert(index, import.Materials.First());
                            break;
                        case "Textures":
                            index = model.Textures.IndexOf((Texture)lastSelectedTreeNode.Tag);
                            model.Textures.RemoveAt(index);
                            model.Textures.Insert(index, import.Textures.First());
                            break;
                        case "Animations":
                            index = model.Animations.IndexOf((Animation)lastSelectedTreeNode.Tag);
                            model.Animations.RemoveAt(index);
                            model.Animations.Insert(index, import.Animations.First());
                            break;
                        default:
                            return;
                    }
                    break;
            }
            RefreshTreeview();
            DataChanged(true);
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            switch (lastSelectedTreeNode.ParentNode.Text)
            {
                case "Bones":
                    model.Bones.RemoveAt(model.Bones.IndexOf((Bone)lastSelectedTreeNode.Tag));
                    break;
                case "Parts":
                    model.Parts.RemoveAt(model.Parts.IndexOf((Part)lastSelectedTreeNode.Tag));
                    break;
                case "Materials":
                    model.Materials.RemoveAt(model.Materials.IndexOf((Material)lastSelectedTreeNode.Tag));
                    break;
                case "Textures":
                    model.Textures.RemoveAt(model.Textures.IndexOf((Texture)lastSelectedTreeNode.Tag));
                    break;
                case "Animations":
                    model.Animations.RemoveAt(model.Animations.IndexOf((Animation)lastSelectedTreeNode.Tag));
                    break;
                default:
                    return;
            }
            RefreshTreeview();
            DataChanged(true);
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

        public void DataChanged(bool changed)
        {
            if (changed)
            {
                dataChanged = true; //Remember that changes have been made
                if (!Text.Contains("*"))
                    Text += "*"; //Add asterisk to window title if it isn't there already
                viewerUpdated = false; //Update model viewer next time treeview is refreshed
            }
            else
            {
                dataChanged = false; //No changes have been made since last save
                Text = Text.Replace("*",""); //Remove any asterisk from window title
            }
            
        }
    }
}
