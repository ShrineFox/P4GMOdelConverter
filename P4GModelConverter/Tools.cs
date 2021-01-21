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

namespace P4GModelConverter
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
                    using (EndianBinaryReader reader = new EndianBinaryReader(File.OpenRead(path), Endianness.BigEndian))
                    {
                        reader.BaseStream.Position = i - 20;
                        byte[] nameBytes = reader.ReadBytes(24);
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
                        using (EndianBinaryReader reader = new EndianBinaryReader(File.OpenRead(path), Endianness.BigEndian))
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
        public static void CreateGMO(string path, SettingsForm.Settings settings)
        {
            string extensionlessPath = Path.Combine(Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path));
            if (File.Exists(path) && Path.GetExtension(path).ToUpper() == ".MDS")
            {
                GMOTool(path, false);
                if (settings.FixForPC)
                    GMOFixTool(extensionlessPath + ".gmo");

                if (File.Exists(extensionlessPath + ".AMD"))
                {
                    //Todo: AUTO REPLACE GMO DATA IN AMD
                }
                if (File.Exists(extensionlessPath + ".PAC"))
                {
                    //Todo: AUTO REPACK AMD INTO PAC
                }
            }

            if (settings.PreviewOutputGMO)
            {
                if (settings.PreviewWith == "Noesis")
                    Noesis($"\"{extensionlessPath + ".gmo"}\"");
                else if (settings.PreviewWith == "GMOView")
                    GMOView(extensionlessPath + ".gmo");
            }

        }
    }
}
