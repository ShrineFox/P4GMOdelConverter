using AmicitiaLibrary.FileSystems.AMD;
using AtlusFileSystemLibrary;
using AtlusFileSystemLibrary.Common.IO;
using AtlusFileSystemLibrary.FileSystems.PAK;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TGE.IO;
using static AtlusFileSystemLibrary.ConflictPolicy;

namespace P4GMOdel
{
    public class Tools
    {

        //Extract TM2 textures from GMO
        public static void ExtractTextures(string path)
        {
            //Get texture names
            List<string> textureNames = new List<string>();
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
                        textureNames.Add(name.Substring(name.LastIndexOf('\0') + 1));
                    }
                }
                catch
                {
                    break;
                }
            }

            if (textureNames.Count > 0)
            {
                if (File.Exists($"{Path.GetDirectoryName(path)}\\textures\\{textureNames[0]}"))
                    return;

                //Get textures and write files
                offset = 0;
                for (int i = 0; i < textureNames.Count(); i++)
                {
                    try
                    {
                        offset = FindSequence(File.ReadAllBytes(path), offset, Encoding.ASCII.GetBytes("TIM2"));
                        Directory.CreateDirectory($"{Path.GetDirectoryName(path)}\\textures");
                        string newFile = $"{Path.GetDirectoryName(path)}\\textures\\{textureNames[i]}";
                        using (TGE.IO.EndianBinaryReader reader = new TGE.IO.EndianBinaryReader(File.OpenRead(path), TGE.IO.Endianness.BigEndian))
                        {
                            reader.BaseStream.Position = offset;
                            reader.ReadBytes(16);
                            int size = reader.ReadInt32() + 16;

                            reader.BaseStream.Position = offset;
                            File.WriteAllBytes(newFile, reader.ReadBytes(size));
                            offset++;
                        }
                    }
                    catch
                    {
                        break;
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

        //Run tool to convert between GMO and MDS
        public static void GMOTool(string path, bool extract)
        {
            Process cmd = new Process();
            cmd.StartInfo.FileName = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\Tools\\GMOTool\\GMOTool.exe";
            //cmd.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
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
                MessageBox.Show($"Error: Could not find .\\Tools\\GMOTool\\GMOTool.exe!");
        }

        //Run program to convert FBX to GMO directly
        public static void GMOConv(string fbx)
        {
            Process cmd = new Process();
            cmd.StartInfo.FileName = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\Tools\\GMO\\GmoConv.exe";
            //cmd.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            cmd.StartInfo.Arguments = $"\"{fbx}\"";
            if (File.Exists(cmd.StartInfo.FileName))
            {
                cmd.Start();
                int x = 0;
                while (!File.Exists($"{Path.Combine(Path.GetDirectoryName(fbx), Path.GetFileNameWithoutExtension(fbx))}.gmo")) { Thread.Sleep(1000); x++; if (x == 15) return; }
                cmd.WaitForExit();
            }
            else
                MessageBox.Show($"Error: Could not find .\\Tools\\GMO\\GmoConv.exe!");

        }

        //Run program to convert referenced textures to TM2
        public static void GIMConv(string texture)
        {
            Process cmd = new Process();
            cmd.StartInfo.FileName = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\Tools\\GIM\\GimConv.exe";
            //cmd.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            cmd.StartInfo.Arguments = $"\"{texture}\" -o \"{texture}.tm2\"";
            if (File.Exists(cmd.StartInfo.FileName))
            {
                cmd.Start();
                int x = 0;
                while (!File.Exists($"{texture}.tm2")) { Thread.Sleep(1000); x++; if (x == 15) return; }
                cmd.WaitForExit();
            }
            else
                MessageBox.Show($"Error: Could not find .\\Tools\\GIM\\GIMConv.exe!");

        }

        //Run program to view newly generated GMO file
        public static void GMOView(string model)
        {
            Process cmd = new Process();
            cmd.StartInfo.FileName = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\Tools\\GMO\\GmoView.exe";
            //cmd.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            cmd.StartInfo.Arguments = $"\"{model}\"";
            if (File.Exists(cmd.StartInfo.FileName))
                cmd.Start();
            else
                MessageBox.Show($"Error: Could not find .\\Tools\\GMO\\GmoView.exe!");

        }

        //Run model through Noesis for viewing
        public static void Noesis(string args)
        {
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.FileName = $"\"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\Tools\\Noesis\\Noesis.exe\"";
            startInfo.Arguments = args;
            process.StartInfo = startInfo;
            if (File.Exists(startInfo.FileName))
                process.Start();
            else
                MessageBox.Show($"Error: Could not find .\\Tools\\Noesis\\Noesis.exe!");

        }

        //Run model through Noesis for optimizing
        public static void NoesisOptimize(string path, string args)
        {
            using (var cmdProcess = new Process())
            {
                string extensionlessPath = Path.Combine(Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path));
                cmdProcess.StartInfo.UseShellExecute = true;
                cmdProcess.StartInfo.WorkingDirectory = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\Tools\\Noesis";
                cmdProcess.StartInfo.FileName = @"C:\Windows\System32\cmd.exe";
                cmdProcess.StartInfo.Arguments = "/k " + $"Noesis.exe ?cmode \"{path}\" \"{extensionlessPath}_noesis.fbx\" {args}";
                cmdProcess.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                if (File.Exists(Path.Combine(cmdProcess.StartInfo.WorkingDirectory, "Noesis.exe")))
                {
                    cmdProcess.Start();
                    int x = 0;
                    while (!File.Exists($"{extensionlessPath}_noesis.fbx")) { Thread.Sleep(1000); x++; if (x == 15) return; }
                    cmdProcess.Kill();
                }
                else
                    MessageBox.Show($"Error: Could not find .\\Tools\\Noesis\\Noesis.exe!");
            }
        }

        //Run TGE's tool to fix GMO files for PC
        public static void GMOFixTool(string path)
        {
            Process cmd = new Process();
            cmd.StartInfo.FileName = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\Tools\\p4gpc-gmofix\\p4gpc-gmofix.exe";
            //cmd.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            cmd.StartInfo.Arguments = $"\"{path}\"";
            if (File.Exists(cmd.StartInfo.FileName))
            {
                cmd.Start();
                cmd.WaitForExit();
            }
            else
                MessageBox.Show($"Error: Could not find .\\Tools\\p4gpc-gmofix\\p4gpc-gmofix.exe!");
        }

        //Convert model to FBX through Noesis commandline using specified settings
        public static string CreateOptimizedFBX(string path, SettingsForm.Settings settings)
        {
            string extensionlessPath = Path.Combine(Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path));
            string args = "";
            if (settings.OldFBXExport)
                args += "-fbxoldexport ";
            if (settings.AsciiFBX)
                args += "-fbxascii ";
            NoesisOptimize(path, $"{args} {settings.AdditionalFBXOptions}");
            extensionlessPath += "_noesis";
            return $"{extensionlessPath}.fbx";
        }

        //Create GMO from MDS
        public static void CreateGMO(string output, Model model, SettingsForm.Settings settings)
        {
            //Create temporary directory
            string tempDir = Path.Combine(Path.GetDirectoryName(model.Path), "temp");
            if (Directory.Exists(tempDir))
                Directory.Delete(tempDir, true);
            Directory.CreateDirectory(tempDir);
            string tempPath = Path.Combine(tempDir, Path.GetFileNameWithoutExtension(model.Path));
            File.WriteAllText(tempPath + ".mds", Model.Serialize(model, settings));

            if (File.Exists(tempPath + ".mds"))
            {
                //Create new GMO
                GMOTool(tempPath + ".mds", false);
                if (settings.FixForPC)
                    GMOFixTool(tempPath + ".gmo");

                if (output.ToLower().EndsWith(".amd"))
                {
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
                        AmdChunk chunk = new AmdChunk() { Data = File.ReadAllBytes(tempPath + ".gmo") };
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
                            foreach (var pakFile in pak.EnumerateFiles())
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
                                    amd.Save("temp.amd");
                                    hasAMD = true;
                                    pak.AddFile(model.Name + ".AMD", "temp.amd", ConflictPolicy.Replace);
                                }
                            }
                            if (!hasAMD)
                                pak.AddFile(model.Name + ".gmo", tempPath + ".gmo", ConflictPolicy.Replace);
                            //Save new PAC
                            pak.Save(output);
                        }
                        else
                            MessageBox.Show("Failed to open PAC for GMO replacement!");
                    }
                    else
                    {
                        DialogResult result = MessageBox.Show("Is your new PAC for P4G? If not, it will not contain an AMD.", "P4G PAC?", MessageBoxButtons.YesNo);
                        if (result == DialogResult.Yes)
                        {
                            //If GMO is for P4G put it in new AMD inside PAC
                            AmdFile amd = new AmdFile();
                            AmdChunk chunk = new AmdChunk() { Data = File.ReadAllBytes(tempPath + ".gmo") };
                            amd.Chunks.Add(chunk);
                            amd.Save("temp.amd");
                            pak.AddFile(model.Name + ".AMD", tempPath + ".amd", ConflictPolicy.Replace);
                        }
                        else
                            pak.AddFile(model.Name + ".gmo", tempPath + ".gmo", ConflictPolicy.Replace); //Add GMO to PAC
                    }
                    //Save PAC
                    pak.Save(output);
                }
                else
                    File.Copy(tempPath + ".gmo", output, true); //Save GMO (overwrite)
            }
            else
            {
                MessageBox.Show("Failed to create temporary MDS!");
            }
        }

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
    }
}
