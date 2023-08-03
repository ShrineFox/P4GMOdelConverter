using AmicitiaLibrary.FileSystems.AMD;
using AtlusFileSystemLibrary;
using AtlusFileSystemLibrary.Common.IO;
using AtlusFileSystemLibrary.FileSystems.PAK;
using DarkUI.Forms;
using ShrineFox.IO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace P4GMOdel
{
    public partial class MainForm : DarkForm
    {

        //Extract TM2 textures from GMO
        public static void ExtractTextures(string path)
        {
            //Get texture names
            List<Tuple<int, string>> textureNames = new List<Tuple<int, string>>();
            byte[] fileBytes = File.ReadAllBytes(path);
            int offset = 0;

            for (int i = 0; i < fileBytes.Count(); i++)
            {
                try
                {
                    offset = FindSequence(File.ReadAllBytes(path), i, Encoding.ASCII.GetBytes(".tm2"));
                    i = offset;
                    using (TGE.IO.EndianBinaryReader reader = new TGE.IO.EndianBinaryReader(File.OpenRead(path), TGE.IO.Endianness.BigEndian))
                    {
                        int dim = 40;
                        if (i > dim) dim = 20;
                        reader.BaseStream.Position = i - dim;
                        byte[] nameBytes = reader.ReadBytes(dim + 4);
                        string name = Encoding.UTF8.GetString(nameBytes);
                        textureNames.Add(new Tuple<int, string>(offset, name.Substring(name.LastIndexOf('\0') + 1)));
                    }
                }
                catch
                {
                    break;
                }
            }

            if (textureNames.Count > 0)
            {
                //Get textures and write new files
                offset = 0;
                for (int i = 0; i < textureNames.Count(); i++)
                {
                    if (!File.Exists($"{Path.GetDirectoryName(path)}\\textures\\{textureNames[i].Item2}"))
                    {
                        try
                        {
                            
                            int size = 0;
                            offset = FindSequence(File.ReadAllBytes(path), textureNames[i].Item1, Encoding.ASCII.GetBytes("TIM2"));
                            using (TGE.IO.EndianBinaryReader reader = new TGE.IO.EndianBinaryReader(File.OpenRead(path), TGE.IO.Endianness.LittleEndian))
                            {
                                //Get Size
                                reader.BaseStream.Position = offset - 8;
                                size = reader.ReadInt32();
                                reader.BaseStream.Position = offset + 4;
                                //Get Image Data
                                Directory.CreateDirectory($"{Path.GetDirectoryName(path)}\\textures");
                                string newFile = $"{Path.GetDirectoryName(path)}\\textures\\{textureNames[i].Item2}";
                                reader.BaseStream.Position = offset;
                                File.WriteAllBytes(newFile, reader.ReadBytes(size));
                            }                            
                        }
                        catch
                        {
                            break;
                        }
                    }
                }
            }
        }

        private static int FindSequence(byte[] array, int start, byte[] sequence)
        {
            int end = array.Length - sequence.Length; // past here no match is possible
            byte firstByte = sequence[0]; // cached to tell compiler there's no aliasing

            while (start < end)
            {
                // scan for first byte only. compiler-friendly.
                if (array[start] == firstByte)
                {
                    // scan for rest of sequence
                    for (int offset = 1; offset < sequence.Length; ++offset)
                    {
                        if (array[start + offset] != sequence[offset])
                        {
                            break; // mismatch? continue scanning with next byte
                        }
                        else if (offset == sequence.Length - 1)
                        {
                            return start; // all bytes matched!
                        }
                    }
                }
                ++start;
            }

            // end of array reached without match
            return -1;
        }

        //Run tool to convert between GMO and GMS
        public static void GMOTool(string path, bool extract)
        {
            Process cmd = new Process();
            cmd.StartInfo.FileName = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\Dependencies\\GMOTool\\GMOTool.exe";
            cmd.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            cmd.StartInfo.Arguments = $"\"{path}\"";
            if (extract)
                cmd.StartInfo.Arguments += " -E";
            else
                cmd.StartInfo.Arguments += " -PSV";
            if (File.Exists(cmd.StartInfo.FileName))
            {
                cmd.Start();
                cmd.WaitForExit();
            }
            else
                MessageBox.Show($"Error: Could not find executable: .\\Dependencies\\GMOTool\\GMOTool.exe");
        }

        //Run program to convert model to GMO directly
        public void GMOConv(string modelFile, bool exportGmo = false)
        {
            Process cmd = new Process();
            cmd.StartInfo.FileName = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\Dependencies\\GMO\\GmoConv.exe";
            //cmd.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            cmd.StartInfo.Arguments = $"\"{modelFile}\"";
            if (File.Exists(cmd.StartInfo.FileName))
            {
                if (modelFile.ToLower().EndsWith(".gmo"))
                    ExtractTextures(modelFile);

                string expectedOutFile = Path.Combine(Path.GetDirectoryName(modelFile), Path.GetFileNameWithoutExtension(modelFile));
                if (exportGmo)
                    expectedOutFile += ".gmo";
                else
                    expectedOutFile += ".gms";

                cmd.StartInfo.Arguments += $" -o {expectedOutFile}";
                cmd.Start();
                using (FileSys.WaitForFile(expectedOutFile)) { }
                cmd.WaitForExit();

                if (File.Exists(expectedOutFile))
                {
                    if (exportGmo)
                        MessageBox.Show($"GMO Successfully exported to: {expectedOutFile}");
                    else
                        ProcessFile(expectedOutFile);
                }
                else
                    MessageBox.Show($"Output file found: {expectedOutFile}" +
                        $"\n\nFile may not have been generated due to a GMOConv error.");
            }
            else
                MessageBox.Show($"Error: Could not find executable: .\\Dependencies\\GMO\\GmoConv.exe");

        }

        // Run program to convert referenced textures to TM2
        public static void GIMConv(string texture)
        {
            Process cmd = new Process();
            cmd.StartInfo.FileName = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\Dependencies\\GIM\\GimConv.exe";
            cmd.StartInfo.Arguments = $"\"{texture}\" -o \"{texture}.tm2\"";
            if (File.Exists(cmd.StartInfo.FileName))
            {
                cmd.Start();
                int x = 0;
                using (FileSys.WaitForFile($"{texture}.tm2", FileMode.Open, FileAccess.ReadWrite, FileShare.None)) { };
                cmd.WaitForExit();
            }
            else
                MessageBox.Show($"Error: Could not find .\\Dependencies\\GIM\\GIMConv.exe!");
        }

        // Run program to view newly generated GMO file
        public static void GMOView(string model)
        {
            Process cmd = new Process();
            cmd.StartInfo.FileName = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\Dependencies\\GMO\\GmoView.exe";
            cmd.StartInfo.Arguments = $"\"{model}\"";
            if (File.Exists(cmd.StartInfo.FileName))
                cmd.Start();
            else
                MessageBox.Show($"Error: Could not find .\\Dependencies\\GMO\\GmoView.exe!");

        }

        //Run model through Noesis for viewing
        public static void Noesis(string args)
        {
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = $"\"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\Dependencies\\Noesis\\Noesis.exe\"";
            startInfo.Arguments = args;
            process.StartInfo = startInfo;
            if (File.Exists(startInfo.FileName))
                process.Start();
            else
                MessageBox.Show($"Error: Could not find .\\Dependencies\\Noesis\\Noesis.exe!");

        }

        //Run model through Noesis for optimizing
        public static void NoesisOptimize(string path, string args)
        {
            using (var cmdProcess = new Process())
            {
                string tempPath = GetTemporaryPath(path);
                cmdProcess.StartInfo.UseShellExecute = true;
                cmdProcess.StartInfo.WorkingDirectory = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\Dependencies\\Noesis";
                cmdProcess.StartInfo.FileName = @"C:\Windows\System32\cmd.exe";
                cmdProcess.StartInfo.Arguments = "/k " + $"Noesis.exe ?cmode \"{path}\" \"{tempPath}.fbx\" {args}";
                cmdProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                if (File.Exists(Path.Combine(cmdProcess.StartInfo.WorkingDirectory, "Noesis.exe")))
                {
                    cmdProcess.Start();
                    int x = 0;
                    using (FileSys.WaitForFile($"{tempPath}.fbx", FileMode.Open, FileAccess.ReadWrite, FileShare.None)) { };
                    cmdProcess.Kill();
                }
                else
                    MessageBox.Show($"Error: Could not find .\\Dependencies\\Noesis\\Noesis.exe!");
            }
        }

        //Run TGE's tool to fix GMO files for PC
        public static void GMOFixTool(string path)
        {
            Process cmd = new Process();
            cmd.StartInfo.FileName = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\Dependencies\\p4gpc-gmofix\\p4gpc-gmofix.exe";
            cmd.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            cmd.StartInfo.Arguments = $"\"{path}\"";
            if (File.Exists(cmd.StartInfo.FileName))
            {
                cmd.Start();
                cmd.WaitForExit();
            }
            else
                MessageBox.Show($"Error: Could not find .\\Dependencies\\p4gpc-gmofix\\p4gpc-gmofix.exe!");
        }

        //Convert model to FBX through Noesis commandline using specified settings
        public static string CreateOptimizedFBX(string path, string args = "")
        {
            string path_NoExt = FileSys.GetExtensionlessPath(path);

            NoesisOptimize(path, args);
            return $"{path_NoExt}.fbx";
        }

        //Create GMO from GMS
        public static void CreateGMO(string output, Model model)
        {
            string tempPath = GetTemporaryPath(model.Path);
            File.WriteAllText(tempPath + ".gms", Model.Serialize(model));
            using (FileSys.WaitForFile(tempPath + ".gms", FileMode.Open, FileAccess.ReadWrite, FileShare.None)) { };
            if (File.Exists(tempPath + ".gms"))
            {
                //Create new GMO
                GMOTool(tempPath + ".gms", false);
                using (FileSys.WaitForFile(tempPath + ".gmo", FileMode.Open, FileAccess.ReadWrite, FileShare.None)) { };
                if (settings.FixForPC)
                    GMOFixTool(tempPath + ".gmo");

                if (output.ToLower().EndsWith(".amd"))
                {
                    using (FileSys.WaitForFile(tempPath + ".gmo", FileMode.Open, FileAccess.ReadWrite, FileShare.None)) { };
                    if (File.Exists(output))
                    {
                        //Replace GMO data in AMD if it already exists
                        AmdFile amd = new AmdFile(output);
                        foreach (var chunk in amd.Chunks)
                        {
                            if (chunk.Tag.ToUpper().Equals("MODEL_DATA"))
                                chunk.Data = File.ReadAllBytes(tempPath + ".gmo");
                        }
                        amd.Save(output);
                    }
                    else
                    {
                        //Create new AMD containing new GMO
                        AmdFile amd = new AmdFile();
                        AmdChunk chunk = new AmdChunk() { Data = File.ReadAllBytes(tempPath + ".gmo"), Flags = 257, Tag = "MODEL_DATA" };
                        amd.Chunks.Add(chunk);
                        amd.Save(output);
                    }
                }
                else if (output.ToLower().EndsWith(".pac"))
                {
                    PAKFileSystem pak = new PAKFileSystem();
                    if (File.Exists(output))
                    {
                        //Replace GMO data in PAC
                        if (PAKFileSystem.TryOpen(output, out pak))
                        {
                            //If PAC already contains AMD...
                            bool hasAMD = false;
                            foreach (var pakFile in pak.EnumerateFiles().ToList())
                            {
                                if (pakFile.ToLower().EndsWith(".amd"))
                                {
                                    //Replace GMO data in AMD
                                    AmdFile amd = new AmdFile(pak.OpenFile(pakFile));
                                    foreach (var chunk in amd.Chunks)
                                    {
                                        if (chunk.Tag.ToUpper().Equals("MODEL_DATA"))
                                            chunk.Data = File.ReadAllBytes(tempPath + ".gmo");
                                    }
                                    amd.Save($"{tempPath}.amd");
                                    using (FileSys.WaitForFile(tempPath + ".amd", FileMode.Open, FileAccess.ReadWrite, FileShare.None)) { };
                                    hasAMD = true;
                                    pak.AddFile(model.Name + ".AMD", $"{tempPath}.amd", ConflictPolicy.Replace);
                                }
                            }
                            if (!hasAMD)
                                pak.AddFile(model.Name + ".gmo", tempPath + ".gmo", ConflictPolicy.Replace);
                        }
                        else
                            MessageBox.Show("Failed to open PAC for GMO replacement!");
                        //Save new PAC
                        pak.Save(output + "_new");
                        using (FileSys.WaitForFile(output + "_new", FileMode.Open, FileAccess.ReadWrite, FileShare.None)) { };
                    }
                    else
                    {
                        DialogResult result = MessageBox.Show("Is your new PAC for P4G? If not, it will not contain an AMD.", "P4G PAC?", MessageBoxButtons.YesNo);
                        if (result == DialogResult.Yes)
                        {
                            //If GMO is for P4G put it in new AMD inside PAC
                            AmdFile amd = new AmdFile();
                            AmdChunk chunk = new AmdChunk() { Data = File.ReadAllBytes(tempPath + ".gmo"), Flags = 257, Tag = "MODEL_DATA" };
                            amd.Chunks.Add(chunk);
                            amd.Save($"{tempPath}.amd");
                            using (FileSys.WaitForFile(tempPath + ".amd", FileMode.Open, FileAccess.ReadWrite, FileShare.None)) { };
                            pak.AddFile(model.Name + ".AMD", tempPath + ".amd", ConflictPolicy.Replace);
                        }
                        else
                            pak.AddFile(model.Name + ".gmo", tempPath + ".gmo", ConflictPolicy.Replace); //Add GMO to PAC
                    }
                    //Save PAC
                    pak.Save(output);
                    using (FileSys.WaitForFile(output, FileMode.Open, FileAccess.ReadWrite, FileShare.None)) { };
                }
                else
                    File.Copy(tempPath + ".gmo", output, true); //Save GMO (overwrite)
            }
            else
            {
                MessageBox.Show("Failed to create temporary GMS!");
            }
        }

        public static string GetTemporaryPath(string path)
        {
            //Create temporary directory
            string tempDir = Path.Combine(Path.GetDirectoryName(path), "temp");
            if (Directory.Exists(tempDir))
                Directory.Delete(tempDir, true);
            Directory.CreateDirectory(tempDir);
            return Path.Combine(tempDir, Path.GetFileNameWithoutExtension(path));
        }
    }
}
