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

namespace P4GMOdel
{
    public partial class MainForm : Form
    {
        private void OpenFile(string path)
        {
            InitializeForm();
            ProcessFile(path);
        }

        private void InitializeForm()
        {
            model = new Model();
            model.Path = null;
            lastSelectedTreeNode = null;
            this.Text = "P4GMOdel";
        }

        private void ProcessFile(string path)
        {
            // Get file extension (lowercase)
            string ext = Path.GetExtension(path).ToLower();

            switch (ext)
            {
                case ".pac":
                    ProcessFile(ExtractPAC(path));
                    break;
                case ".amd":
                    ProcessFile(ExtractAMD(path));
                    break;
                case ".dae":
                case ".smd":
                    ProcessFile(ConvertToFBX(path));
                    break;
                case ".fbx":
                    ProcessFile(ConvertToGMS(path));
                    break;
                case ".gms":
                    LoadDataIntoEditor(path);
                    break;
                default:
                    break;

            }
        }

        private string ExtractPAC(string path)
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
                    using (Tools.WaitForFile(outPath, FileMode.Open, FileAccess.ReadWrite, FileShare.None)) { };
            }
            else
                MessageBox.Show("Failed to open PAC archive!");

            return outPath;
        }

        private string ExtractAMD(string path)
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
                    using (Tools.WaitForFile(outPath, FileMode.Open, FileAccess.ReadWrite, FileShare.None)) { };
            }
            else
                MessageBox.Show("Failed to open AMD archive!");

            return outPath;
        }

        private string ConvertToGMS(string path)
        {
            return Tools.GMOTool(path, true);
        }

        private string ConvertToGMO(string path)
        {
            return Tools.GMOConv(model.Path);
        }

        private string ConvertToFBX(string path)
        {
            return Tools.CreateOptimizedFBX(model.Path);
        }

        private void LoadDataIntoEditor(string path)
        {
            if (File.Exists(path))
            {
                var gmsLines = File.ReadAllLines(path).ToList();
                model = Model.Deserialize(model, gmsLines.ToArray());
                RefreshTreeview();
                ModelViewer.Update(model);
                this.Text = "P4GMOdel - " + Path.GetFileName(path);
                toolStripMenuItem_Save.Enabled = true;
                toolStripMenuItem_SaveAs.Enabled = true;
                toolStripMenuItem_Export.Enabled = true;
            }
            else
            {
                toolStripMenuItem_Save.Enabled = false;
                toolStripMenuItem_SaveAs.Enabled = false;
                toolStripMenuItem_Export.Enabled = false;
                MessageBox.Show("No .MDS file found! One may not have been generated due to a GMOTool error.");
            }
        }

        
    }
}
