using DarkUI.Controls;
using DarkUI.Forms;
using Microsoft.WindowsAPICodePack.Dialogs;
using ShrineFox.IO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace P4GMOdel
{
    public partial class MainForm : DarkForm
    {
        DarkTreeNode lastSelectedTreeNode;
        public static bool dataChanged = false;
        public static bool viewerUpdated = false;

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
                Text = Text.Replace("*", ""); //Remove any asterisk from window title
            }

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

            RestoreNodeSelection();
        }

        private void RestoreNodeSelection()
        {
            if (darkTreeView_Main.Nodes.Count > 0)
            {
                //Expand Model node and focus on treeview to show highlight
                darkTreeView_Main.Focus();
                darkTreeView_Main.Nodes[0].Expanded = true;
                //Set last selected node to Model by default
                if (lastSelectedTreeNode == null)
                    lastSelectedTreeNode = darkTreeView_Main.Nodes[0];
                //Expand nodes above selection
                var node = GetNodeFromPath(darkTreeView_Main.Nodes[0], lastSelectedTreeNode.FullPath);
                if (node != null)
                {
                    //Reselect and scroll to it
                    darkTreeView_Main.SelectedNodes[0] = node;
                    if (darkTreeView_Main.SelectedNodes[0].ParentNode != null)
                        darkTreeView_Main.SelectedNodes[0].ParentNode.Expanded = true;
                    darkTreeView_Main.SelectedNodes[0].Expanded = true;
                    darkTreeView_Main.SelectedNodes[0].EnsureVisible();
                    //Update PropertyGrid
                    propertyGrid_Main.SelectedObject = node.Tag;
                    propertyGrid_Main.Update();
                }
            }
        }

        private void TreeView_MouseClick(object sender, MouseEventArgs e)
        {
            if (lastSelectedTreeNode == null)
                return;

            //Update object if changed
            UpdatePropertyGrid();

            //Replace modelviewer with texture view if "Texture" subnode clicked
            if (lastSelectedTreeNode.ParentNode != null && lastSelectedTreeNode.ParentNode.Text == "Textures")
            {
                Texture texture = (Texture)lastSelectedTreeNode.Tag;
                if (File.Exists(texture.FileName))
                {
                    using (Stream s = File.Open(texture.FileName, FileMode.Open))
                    {
                        //TIM2TextureSerializer serializer = new TIM2TextureSerializer();
                        Exe.CloseProcess("GMOView"); //Hide model viewer
                        panel_ModelViewer.BackgroundImageLayout = ImageLayout.Zoom;
                        //panel_ModelViewer.BackgroundImage = serializer.Open(s).GetImage(); //Decode TM2 to viewer
                        viewerUpdated = false; //Reload model viewer next chance we get
                    }
                }
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
            BuildTempModel(model);
        }

        private void PropertyGrid_ValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            string newValue = e.ChangedItem.Value.ToString();
            string oldValue = e.OldValue.ToString();
            string propertyName = e.ChangedItem.Label;

            //Update all instances of value across model
            switch (lastSelectedTreeNode.ParentNode.Text)
            {
                case "Bones":
                    if (propertyName == "ParentBone")
                        foreach (Bone bone in model.Bones)
                            if (bone.ParentBone == oldValue)
                                bone.ParentBone = newValue;
                    if (propertyName == "Name")
                        foreach (Animation anim in model.Animations)
                            for (int i = 0; i < anim.Animate.Count; i++)
                                anim.Animate[i] = anim.Animate[i].Replace($"\"Bone::{oldValue}\"", $"\"Bone::{newValue}\"");
                    break;
                case "Parts":
                    if (propertyName == "Name")
                        foreach (Bone bone in model.Bones)
                            for (int i = 0; i < bone.DrawParts.Count; i++)
                                if (bone.DrawParts[i] == oldValue)
                                    bone.DrawParts[i] = newValue;
                    break;
                case "Materials":
                    if (propertyName == "Name")
                        foreach (Part part in model.Parts)
                            for (int i = 0; i < part.Meshes.Count; i++)
                                if (part.Meshes[i].SetMaterial == oldValue)
                                    part.Meshes[i].SetMaterial = newValue;
                    break;
                case "Textures":
                    if (propertyName == "Name")
                        foreach (Material mat in model.Materials)
                            for (int i = 0; i < mat.Layers.Count; i++)
                                if (mat.Layers[i].SetTexture == oldValue)
                                    mat.Layers[i].SetTexture = newValue;
                    break;
                default:
                    return;
            }

            DataChanged(true);
        }


        private void MainForm_Resize(object sender, EventArgs e)
        {
            //if (process_GMOView != null)
                //ModelViewer.MoveWindow(process_GMOView.MainWindowHandle, 0, 0, panel_ModelViewer.Width, panel_ModelViewer.Height, true);
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

        /* CONTEXT MENU */

        private void MoveUp_Click(object sender, EventArgs e)
        {
            if (lastSelectedTreeNode.ParentNode == null)
                return;

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
            if (lastSelectedTreeNode.ParentNode == null)
                return;

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
            if (lastSelectedTreeNode.ParentNode == null)
                return;

            Model export = new Model();
            //Export model object
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
                    //Export individual elements as gms
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

            CommonSaveFileDialog dialog = new CommonSaveFileDialog();
            dialog.Title = "Save Data as...";
            dialog.Filters.Add(new CommonFileDialogFilter("GMO Data", "*.gms"));
            if (lastSelectedTreeNode.Text == "Textures" || lastSelectedTreeNode.ParentNode.Text == "Textures")
            {
                dialog.Filters.Add(new CommonFileDialogFilter("TM2", "*.tm2"));
                dialog.Filters.Add(new CommonFileDialogFilter("PNG", "*.png"));
                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    if (Path.GetExtension(dialog.FileName).ToLower() == ".gms")
                        File.WriteAllText(dialog.FileName, Model.Serialize(export));
                    else if (Path.GetExtension(dialog.FileName).ToLower() == ".tm2")
                    {
                        if (export.Textures.Count > 1)
                            foreach (var texture in export.Textures)
                                File.Copy(texture.FileName, Path.Combine(Path.GetDirectoryName(dialog.FileName), Path.GetFileName(texture.FileName)));
                        else if (export.Textures.Count == 1)
                            File.Copy(export.Textures[0].FileName, dialog.FileName);
                    }
                    else if (Path.GetExtension(dialog.FileName).ToLower() == ".png")
                    {
                        foreach (var texture in export.Textures)
                        {
                            using (Stream s = File.Open(texture.FileName, FileMode.Open))
                            {
                                //Image newImage = serializer.Open(s).GetImage();
                                //newImage.Save(dialog.FileName);
                            }
                        }

                    }
                    MessageBox.Show($"Exported element as {Path.GetFileName(dialog.FileName)}!");
                }
            }
            else
            {
                dialog.DefaultFileName = $"{lastSelectedTreeNode.Text}.gms";
                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                    File.WriteAllText(dialog.FileName, Model.Serialize(export));
                MessageBox.Show($"Exported element as {Path.GetFileName(dialog.FileName)}!");
            }
        }

        private void Add_Click(object sender, EventArgs e)
        {
            Model import = Model.Import(lastSelectedTreeNode.Text);
            if (import == new Model())
                return;
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
            if (lastSelectedTreeNode.ParentNode == null)
                return;

            List<Texture> importTex = new List<Texture>();
            int index = 0;
            if (lastSelectedTreeNode.Text == "Textures")
            {
                importTex = Texture.Import(false);
                if (importTex == new List<Texture>())
                    return;

                //Replace textures node with selected replacement
                model.Textures = importTex;
            }
            else if (lastSelectedTreeNode.ParentNode.Text == "Textures")
            {
                importTex = Texture.Import(false);
                if (importTex == new List<Texture>())
                    return;

                //Add texture from import in place of the selected one
                index = model.Textures.IndexOf((Texture)lastSelectedTreeNode.Tag);
                model.Textures.RemoveAt(index);
                importTex[0].Name = lastSelectedTreeNode.Text;
                model.Textures.Insert(index, importTex[0]);
            }
            else
            {
                Model import = Model.Import(lastSelectedTreeNode.Text);
                if (import == new Model())
                    return;

                //Replace selected node
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
                        //Replace individual item
                        switch (lastSelectedTreeNode.ParentNode.Text)
                        {
                            case "Bones":
                                index = model.Bones.IndexOf((Bone)lastSelectedTreeNode.Tag);
                                model.Bones.RemoveAt(index);
                                import.Bones.First().Name = lastSelectedTreeNode.Text;
                                model.Bones.Insert(index, import.Bones.First());
                                break;
                            case "Parts":
                                index = model.Parts.IndexOf((Part)lastSelectedTreeNode.Tag);
                                model.Parts.RemoveAt(index);
                                import.Parts.First().Name = lastSelectedTreeNode.Text;
                                model.Parts.Insert(index, import.Parts.First());
                                break;
                            case "Materials":
                                index = model.Materials.IndexOf((Material)lastSelectedTreeNode.Tag);
                                model.Materials.RemoveAt(index);
                                import.Materials.First().Name = lastSelectedTreeNode.Text;
                                model.Materials.Insert(index, import.Materials.First());
                                break;
                            case "Animations":
                                index = model.Animations.IndexOf((Animation)lastSelectedTreeNode.Tag);
                                model.Animations.RemoveAt(index);
                                import.Animations.First().Name = lastSelectedTreeNode.Text;
                                model.Animations.Insert(index, import.Animations.First());
                                break;
                            default:
                                return;
                        }
                        break;
                }
                toolStripMenuItem_Refresh.PerformClick();
            }
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            if (lastSelectedTreeNode.ParentNode == null)
                return;

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
