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
using System.Windows.Shell;

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
            string exePath = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\Dependencies\\GMOTool\\GMOTool.exe";
            string args = $"\"{path}\"";
            if (extract)
                args += " -E";
            else
                args += " -PSV";

            Exe.Run(exePath, args);
        }

        //Run program to convert model to GMO directly
        public void GMOConv(string modelFile, bool exportGmo = false)
        {
            string exePath = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\Dependencies\\GMO\\GmoConv.exe";
            string expectedOutFile = Path.Combine(Path.GetDirectoryName(modelFile), Path.GetFileNameWithoutExtension(modelFile));
            if (exportGmo)
                expectedOutFile += ".gmo";
            else
                expectedOutFile += ".gms";
            string args = $"\"{modelFile}\" -o {expectedOutFile}\"";

            if (modelFile.ToLower().EndsWith(".gmo"))
                ExtractTextures(modelFile);

            Exe.Run(exePath, args);

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

        // Run program to convert referenced textures to TM2
        public static void GIMConv(string texture)
        {
            string exePath = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\Dependencies\\GIM\\GimConv.exe";
            string args = $"\"{texture}\" -o \"{texture}.tm2\"";
            Exe.Run(exePath, args);

        }

        //Run model through Noesis for optimizing
        public void NoesisOptimizeFbx(string path)
        {
            string exePath = $"C:\\Windows\\System32\\cmd.exe";
            string expectedOutFile = $"{GetTemporaryPath(path)}.fbx";
            string args = $"/k \"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\Dependencies\\Noesis\\Noesis.exe\" ?cmode \"{path}\" \"{expectedOutFile}\" {settings.NoesisArgs}";

            Exe.Run(exePath, args);

            if (File.Exists(expectedOutFile))
                ProcessFile(expectedOutFile);
            else
                MessageBox.Show($"Error: Noesis failed to produce an optimized FBX!");
        }

        //Run TGE's tool to fix GMO files for PC
        public static void GMOFixTool(string path)
        {
            string exePath = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\Dependencies\\p4gpc-gmofix\\p4gpc-gmofix.exe";
            string args = $"\"{path}\"";
            Exe.Run(exePath, args);
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
