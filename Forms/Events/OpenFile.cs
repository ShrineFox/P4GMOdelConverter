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
using AmicitiaLibrary.FileSystems.AMD;
using AtlusFileSystemLibrary.Common.IO;
using AtlusFileSystemLibrary.FileSystems.PAK;
using DarkUI.Forms;

namespace P4GMOdel
{
    public partial class MainForm : DarkForm
    {
        private void OpenFile(string path)
        {
            // Set the default window title and disable controls until data is loaded
            InitializeForm();
            // Convert the file to GMS in the temp dir and then load data into editor
            ProcessFile(path);
        }

        private void InitializeForm()
        {
            model = new Model();
            model.Path = "";
            lastSelectedTreeNode = null;
            this.Text = "P4GMOdel";
            toolStripMenuItem_Save.Enabled = false;
            toolStripMenuItem_SaveAs.Enabled = false;
            toolStripMenuItem_Export.Enabled = false;
        }

        public void ProcessFile(string path)
        {
            string extension = Path.GetExtension(path).ToLower();

            switch (extension)
            {
                case ".pac":
                    ExtractPAC(path);
                    break;
                case ".amd":
                    ExtractAMD(path);
                    break;
                case ".dae":
                case ".smd":
                    NoesisOptimizeFbx(path);
                    break;
                case ".fbx":
                    if (settings.OptimizeFbxWithNoesis)
                        NoesisOptimizeFbx(path);
                    else
                        GMOConv(path);
                    break;
                case ".gmo":
                    GMOConv(path);
                    break;
                case ".gms":
                    LoadDataIntoEditor(path);
                    break;
                default:
                    break;
            }
        }

        private static string ExtractPAC(string path)
        {
            string path_NoExt = FileSys.GetExtensionlessPath(path);
            string outPath = "";

            // Attempt to extract .AMD or .GMO file from .PAC
            PAKFileSystem pak = new PAKFileSystem();
            if (PAKFileSystem.TryOpen(path, out pak))
            {
                bool extracted = false;
                foreach (var pakFile in pak.EnumerateFiles())
                {
                    if (Path.GetExtension(pakFile).ToLower() == ".amd")
                    {
                        outPath = path_NoExt + ".amd";
                        using (var stream = FileUtils.Create(outPath))
                        using (var inputStream = pak.OpenFile(pakFile))
                            inputStream.CopyTo(stream);
                        extracted = true;
                    }
                    else if (pakFile.ToUpper().Equals("MODEL_DATA"))
                    {
                        outPath = path_NoExt + ".gmo";
                        using (var stream = FileUtils.Create(outPath))
                        using (var inputStream = pak.OpenFile(pakFile))
                            inputStream.CopyTo(stream);
                        extracted = true;
                    }
                }
                if (!extracted)
                    MessageBox.Show("Could not find AMD or GMO model data in PAC archive!");
                else
                    using (FileSys.WaitForFile(outPath)) { }
            }
            else
                MessageBox.Show("Failed to open PAC archive!");

            return outPath;
        }

        private static string ExtractAMD(string path)
        {
            string path_NoExt = FileSys.GetExtensionlessPath(path);
            string outPath = "";

            // Attempt to extract .GMO from .AMD
            if (File.Exists(path))
            {
                bool extracted = false;
                AmdFile amd = new AmdFile(path);
                foreach (var chunk in amd.Chunks)
                {
                    if (chunk.Tag.ToUpper().Equals("MODEL_DATA"))
                    {
                        outPath = path_NoExt + ".gmo";
                        File.WriteAllBytes(outPath, chunk.Data);
                        extracted = true;
                    }
                }
                if (!extracted)
                    MessageBox.Show("Could not find GMO model data in AMD archive!");
                else
                    using (FileSys.WaitForFile(outPath)) { }
            }
            else
                MessageBox.Show("Failed to open AMD archive!");

            return outPath;
        }

        private void LoadDataIntoEditor(string path)
        {
            model.Path = path;
            var gmsLines = File.ReadAllLines(path).ToList();
            model = Model.Deserialize(model, gmsLines.ToArray());
            RefreshTreeview();
            BuildTempModel(model);
            this.Text = "P4GMOdel - " + Path.GetFileName(path);
            toolStripMenuItem_Save.Enabled = true;
            toolStripMenuItem_SaveAs.Enabled = true;
            toolStripMenuItem_Export.Enabled = true;
        }
    }
}
