using Microsoft.WindowsAPICodePack.Dialogs;
using ShrineFox.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization;
using System.IO;

namespace P4GMOdel
{
    public partial class MainForm : Form
    {
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
            Exe.CloseProcess("GMOView");
        }

        private void Save_Click(object sender, EventArgs e)
        {
            if (model.Path.ToLower().EndsWith(".mds"))
            {
                File.WriteAllText(model.Path, Model.Serialize(model));
                MessageBox.Show("MDS file saved!");
                DataChanged(false);
            }
        }

        private void SaveAs_Click(object sender, EventArgs e)
        {
            CommonSaveFileDialog dialog = new CommonSaveFileDialog();
            dialog.Filters.Add(new CommonFileDialogFilter("GMO Data", "*.mds"));
            dialog.Title = "Save MDS...";
            dialog.DefaultFileName = $"{Path.GetFileNameWithoutExtension(model.Path)}.mds";
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                File.WriteAllText(dialog.FileName, Model.Serialize(model));
                model.Path = dialog.FileName;
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
            {
                Tools.CreateGMO(dialog.FileName, model);
                MessageBox.Show($"Exported model as {Path.GetFileName(dialog.FileName)}!");
            }
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
            }
        }

    }
}
