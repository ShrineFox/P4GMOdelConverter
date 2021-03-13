using AmicitiaLibrary.FileSystems.AMD;
using AtlusFileSystemLibrary;
using AtlusFileSystemLibrary.Common.IO;
using AtlusFileSystemLibrary.FileSystems.PAK;
using FreeImageAPI;
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

        //Run tool to convert between GMO and MDS
        public static void GMOTool(string path, bool extract, SettingsForm.Settings settings)
        {
            Process cmd = new Process();
            cmd.StartInfo.FileName = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\Tools\\GMOTool\\GMOTool.exe";
            if (!settings.ShowConsoleWindows)
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
                MessageBox.Show($"Error: Could not find .\\Tools\\GMOTool\\GMOTool.exe!");
        }

        //Run program to convert FBX to GMO directly
        public static void GMOConv(string fbx, SettingsForm.Settings settings)
        {
            Process cmd = new Process();
            cmd.StartInfo.FileName = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\Tools\\GMO\\GmoConv.exe";
            if (!settings.ShowConsoleWindows)
                cmd.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            cmd.StartInfo.Arguments = $"\"{fbx}\"";
            if (File.Exists(cmd.StartInfo.FileName))
            {
                cmd.Start();
                int x = 0;
                using (Tools.WaitForFile($"{Path.Combine(Path.GetDirectoryName(fbx), Path.GetFileNameWithoutExtension(fbx))}.gmo", FileMode.Open, FileAccess.ReadWrite, FileShare.None)) { };
                cmd.WaitForExit();
            }
            else
                MessageBox.Show($"Error: Could not find .\\Tools\\GMO\\GmoConv.exe!");

        }

        //Run program to convert referenced textures to TM2
        public static void GIMConv(string texture, SettingsForm.Settings settings)
        {
            Process cmd = new Process();
            cmd.StartInfo.FileName = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\Tools\\GIM\\GimConv.exe";
            if (!settings.ShowConsoleWindows)
                cmd.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            cmd.StartInfo.Arguments = $"\"{texture}\" -o \"{texture}.tm2\"";
            if (File.Exists(cmd.StartInfo.FileName))
            {
                cmd.Start();
                int x = 0;
                using (WaitForFile($"{texture}.tm2", FileMode.Open, FileAccess.ReadWrite, FileShare.None)) { };
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
            startInfo.FileName = $"\"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\Tools\\Noesis\\Noesis.exe\"";
            startInfo.Arguments = args;
            process.StartInfo = startInfo;
            if (File.Exists(startInfo.FileName))
                process.Start();
            else
                MessageBox.Show($"Error: Could not find .\\Tools\\Noesis\\Noesis.exe!");

        }

        //Run model through Noesis for optimizing
        public static void NoesisOptimize(string path, string args, SettingsForm.Settings settings)
        {
            using (var cmdProcess = new Process())
            {
                string tempPath = GetTemporaryPath(path);
                cmdProcess.StartInfo.UseShellExecute = true;
                cmdProcess.StartInfo.WorkingDirectory = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\Tools\\Noesis";
                cmdProcess.StartInfo.FileName = @"C:\Windows\System32\cmd.exe";
                cmdProcess.StartInfo.Arguments = "/k " + $"Noesis.exe ?cmode \"{path}\" \"{tempPath}.fbx\" {args}";
                if (!settings.ShowConsoleWindows)
                    cmdProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                if (File.Exists(Path.Combine(cmdProcess.StartInfo.WorkingDirectory, "Noesis.exe")))
                {
                    cmdProcess.Start();
                    int x = 0;
                    using (WaitForFile($"{tempPath}.fbx", FileMode.Open, FileAccess.ReadWrite, FileShare.None)) { };
                    cmdProcess.Kill();
                }
                else
                    MessageBox.Show($"Error: Could not find .\\Tools\\Noesis\\Noesis.exe!");
            }
        }

        //Run TGE's tool to fix GMO files for PC
        public static void GMOFixTool(string path, SettingsForm.Settings settings)
        {
            Process cmd = new Process();
            cmd.StartInfo.FileName = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\Tools\\p4gpc-gmofix\\p4gpc-gmofix.exe";
            if (!settings.ShowConsoleWindows)
                cmd.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
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
            NoesisOptimize(path, $"{args} {settings.AdditionalFBXOptions}", settings);
            return $"{extensionlessPath}.fbx";
        }

        //Create GMO from MDS
        public static void CreateGMO(string output, Model model, SettingsForm.Settings settings)
        {
            string tempPath = GetTemporaryPath(model.Path);
            File.WriteAllText(tempPath + ".mds", Model.Serialize(model, settings));
            using (WaitForFile(tempPath + ".mds", FileMode.Open, FileAccess.ReadWrite, FileShare.None)) { };
            if (File.Exists(tempPath + ".mds"))
            {
                //Create new GMO
                GMOTool(tempPath + ".mds", false, settings);
                using (WaitForFile(tempPath + ".gmo", FileMode.Open, FileAccess.ReadWrite, FileShare.None)) { };
                if (settings.FixForPC)
                    GMOFixTool(tempPath + ".gmo", settings);

                if (output.ToLower().EndsWith(".amd"))
                {
                    using (WaitForFile(tempPath + ".gmo", FileMode.Open, FileAccess.ReadWrite, FileShare.None)) { };
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
                                    using (WaitForFile(tempPath + ".amd", FileMode.Open, FileAccess.ReadWrite, FileShare.None)) { };
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
                        using (WaitForFile(output + "_new", FileMode.Open, FileAccess.ReadWrite, FileShare.None)) { };
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
                            using (WaitForFile(tempPath + ".amd", FileMode.Open, FileAccess.ReadWrite, FileShare.None)) { };
                            pak.AddFile(model.Name + ".AMD", tempPath + ".amd", ConflictPolicy.Replace);
                        }
                        else
                            pak.AddFile(model.Name + ".gmo", tempPath + ".gmo", ConflictPolicy.Replace); //Add GMO to PAC
                    }
                    //Save PAC
                    pak.Save(output);
                    using (WaitForFile(output, FileMode.Open, FileAccess.ReadWrite, FileShare.None)) { };
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

        public static string GetTemporaryPath(string path)
        {
            //Create temporary directory
            string tempDir = Path.Combine(Path.GetDirectoryName(path), "temp");
            if (Directory.Exists(tempDir))
                Directory.Delete(tempDir, true);
            Directory.CreateDirectory(tempDir);
            return Path.Combine(tempDir, Path.GetFileNameWithoutExtension(path));
        }

        public static FileStream WaitForFile(string fullPath, FileMode mode, FileAccess access, FileShare share)
        {
            for (int numTries = 0; numTries < 10; numTries++)
            {
                FileStream fs = null;
                try
                {
                    fs = new FileStream(fullPath, mode, access, share);
                    return fs;
                }
                catch (IOException)
                {
                    if (fs != null)
                    {
                        fs.Dispose();
                    }
                    Thread.Sleep(200);
                }
            }

            return null;
        }

        public static void Create8bppPng(string input, string output)
        {
            FIBITMAP hDIB24bpp = FreeImage.LoadEx(input);
            if (!hDIB24bpp.IsNull)
            {
                FIBITMAP hDIB8bpp = FreeImage.ColorQuantize(hDIB24bpp, FREE_IMAGE_QUANTIZE.FIQ_WUQUANT);
                Palette palette = new Palette(hDIB8bpp);
                byte[] transparency = new byte[256];
                for (int i = 0; i < 256; i++)
                {
                    transparency[i] = 0xFF;
                    if (palette[i].rgbGreen >= 0xFE && palette[i].rgbBlue == 0x00 && palette[i].rgbRed == 0x00)
                    {
                        transparency[i] = 0x00;
                    }
                }
                FreeImage.SetTransparencyTable(hDIB8bpp, transparency);
                FreeImage.SaveEx(hDIB8bpp, output);

                FreeImage.UnloadEx(ref hDIB24bpp);
                FreeImage.UnloadEx(ref hDIB8bpp);
            }
        }
    }
}
