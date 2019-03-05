using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using TGE.IO;

namespace P4GModelConverter
{
    public partial class MainForm : Form
    {
        List<string> newLines = new List<string>();
        List<List<string>> bones = new List<List<string>>();
        List<List<string>> meshes = new List<List<string>>();
        List<List<string>> drawArrays = new List<List<string>>();

        List<string> materialNames = new List<string>();
        List<string> mapNames = new List<string>();

        List<List<string>> animations = new List<List<string>>();
        string[] lines;
        bool addLine;

        public MainForm()
        {
            InitializeComponent();
        }

        //Extract GMO or FBX model as MDS
        private void btn_Extract_DragDrop(object sender, DragEventArgs e)
        {
            string path = ((string[])e.Data.GetData(DataFormats.FileDrop, false))[0];
            if (File.Exists(path) && (Path.GetExtension(path).ToUpper() == ".GMO" || Path.GetExtension(path).ToUpper() == ".FBX"))
            {
                if (chkBox_Extract.Checked)
                    ExtractTextures(path);
                GMOTool(path, true);
                string mdsPath = $"{Path.GetDirectoryName(path)}\\{Path.GetFileNameWithoutExtension(path)}.mds";
                while (!File.Exists(mdsPath)) { }
                FixMDS(mdsPath);
                ReorderAnimations();
                string newMDSPath = $"{Path.GetDirectoryName(path)}\\{Path.GetFileNameWithoutExtension(path)}_p4g.mds";
                if (File.Exists(newMDSPath))
                    File.Delete(newMDSPath);
                File.AppendAllLines(newMDSPath, newLines);
            }
                
        }

        //Extract textures from GMO
        private void ExtractTextures(string path)
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

            if (File.Exists($"{Path.GetDirectoryName(path)}\\{textureNames[0]}"))
                return;

            //Get textures and write files
            offset = 0;
            for (int i = 0; i < fileBytes.Count(); i++)
            {
                try
                {
                    offset = FindSequence(File.ReadAllBytes(path), offset, Encoding.ASCII.GetBytes("TIM2"));
                    string newFile = $"{Path.GetDirectoryName(path)}\\{textureNames[i]}";
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

        //Create GMO from MDS
        private void btn_Create_DragDrop(object sender, DragEventArgs e)
        {
            string path = ((string[])e.Data.GetData(DataFormats.FileDrop, false))[0];
            if (File.Exists(path) && Path.GetExtension(path).ToUpper() == ".MDS")
            {
                GMOTool(path, false);
            }
                
        }

        private void GMOTool(string path, bool extract)
        {
            Process cmd = new Process();
            cmd.StartInfo.FileName = "GMOTool.exe";
            //cmd.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            cmd.StartInfo.Arguments = $"\"{path}\"";
            if (extract)
                cmd.StartInfo.Arguments += " -E";
            cmd.Start();
            cmd.WaitForExit();
        }

        private void FixMDS(string path)
        {
            //Initialize Old Lists
            bones = new List<List<string>>();
            meshes = new List<List<string>>();
            drawArrays = new List<List<string>>();
            materialNames = new List<string>();
            mapNames = new List<string>();
            animations = new List<List<string>>();
            //Lines from the original file and new collection
            lines = File.ReadAllLines(path);
            newLines = new List<string>();
            //Used to disable adding lines even while addLine is true
            int cutoff = int.MaxValue;

            for (int i = 0; i < lines.Count(); i++)
            {
                addLine = true;

                //Don't add any lines past this automatically
                if (lines[i].StartsWith("\tBone") || lines[i].Contains("\tPart"))
                    cutoff = i;

                if (i >= cutoff)
                {
                    if (lines[i].StartsWith("\tBone"))
                    {
                        List<string> bone = new List<string>();
                        bone.Add(lines[i]);
                        int x = i + 1;
                        while (x < lines.Count())
                        {
                            if (lines[x].Contains("\tBone") || lines[x].Contains("\t\tMesh") || lines[x].Contains("\tPart"))
                            {
                                bones.Add(bone);
                                break;
                            }
                            bone.Add(lines[x]);
                            x++;
                        }
                    }
                    else if (lines[i].Contains("\t\tMesh"))
                    {
                        List<string> mesh = new List<string>();
                        mesh.Add(lines[i]);
                        int x = i + 1;
                        while (x < lines.Count())
                        {
                            if (lines[x].Contains("\t\tMesh") || lines[x].Contains("\t\tArrays"))
                            {
                                meshes.Add(mesh);
                                break;
                            }
                            mesh.Add(lines[x]);
                            x++;
                        }
                    }
                    else if (lines[i].StartsWith("\t\tArrays"))
                    {
                        List<string> array = new List<string>();
                        array.Add(lines[i]);
                        int x = i + 1;
                        while (x < lines.Count())
                        {
                            if (lines[x].Contains("\t\tArrays") || lines[x].Contains("\tMaterial") || lines[x].Contains("\t}"))
                            {
                                drawArrays.Add(array);
                                break;
                            }
                            array.Add(lines[x]);
                            x++;
                        }
                    }
                    else if (lines[i].StartsWith("\tMaterial"))
                    {
                        //Add material name to material name list
                        materialNames.Add(GetSubstringByString("\tMaterial \"", "\" {", lines[i]));
                    }
                    else if (lines[i].StartsWith("\t\t\tSetTexture"))
                    {
                        //Add map name to map name list
                        string remainingLines = String.Concat(lines.Skip(i).ToArray());
                        string mapString = GetSubstringByString("\t\t\tSetTexture \"", "\"\t", remainingLines);
                        mapNames.Add(mapString);
                    }
                    else if (lines[i].StartsWith("\tTexture"))
                    {
                        //Write bones/meshes/arrays/materials in correct order
                        RewriteBones();
                        RewriteParts();
                        RewriteMaterials();
                        //Write the rest of the file normally (textures, anims...)
                        cutoff = int.MaxValue;
                    }
                }

                //Add line to new collection if it doesn't need to be replaced
                if (addLine && i < cutoff)
                    newLines.Add(lines[i]);
            }
        }

        private void RewriteBones()
        {
            if (bones.Count <= 0)
            {
                return;
            }
            //Add drawPart to body bone (usually the third, or user can manually move it to appropriate bone)
            for (int i = 0; i < bones.Count(); i++)
            {
                if (i != 2)
                {
                    foreach (var line in bones[i])
                    {
                        if (!line.Contains("DrawPart"))
                            newLines.Add(line);
                    }
                }
                else
                {
                    foreach (var line in bones[2])
                    {
                        if (line != "\t}" && !line.Contains("DrawPart"))
                            newLines.Add(line);
                    }
                    for (int x = 0; x < meshes.Count(); x++)
                    {
                        newLines.Add($"\t\tDrawPart \"mesh{x}\"");
                    }
                    newLines.Add("\t}");
                }
            }
        }

        //If listbox order has changed, reorder animations to match
        private void ReorderAnimations()
        {
            //Keep track of which line animations start at
            int animLine = 0;
            //Make a list of animations and strings per animation
            for (int i = 0; i < newLines.Count(); i++)
            {
                if (newLines[i].StartsWith("\tMotion "))
                {
                    if (animLine == 0)
                    {
                        animLine = i;
                    }
                    List<string> motion = new List<string>();
                    motion.Add(newLines[i]);

                    int x = i + 1;
                    while (x < newLines.Count())
                    {
                        if (newLines[x].StartsWith("\tMotion ") || newLines[x].StartsWith("}"))
                        {
                            animations.Add(motion);
                            break;
                        }
                        motion.Add(newLines[x]);
                        x++;
                    }
                    
                }
            }

            //If there are any animations present, reorder them and add to end of file
            if (animLine > 0)
            {
                newLines = newLines.Take(animLine).ToList();

                List<List<string>> newAnims = new List<List<string>>();
                if (chkBox_Animations.Checked)
                {
                    foreach (string animOrder in listBox_AnimationOrder.Items)
                    {
                        int animOrderValue = Convert.ToInt32(animOrder);
                        if (animOrderValue <= animations.Count() - 1)
                            newAnims.Add(animations[animOrderValue]);
                    }
                }
                animations = newAnims;
                foreach (var animation in animations)
                {
                    foreach (var line in animation)
                        newLines.Add(line);
                }
                newLines.Add("}");
            }
        }

        private void RewriteParts()
        {
            for (int i = 0; i < meshes.Count(); i++)
            {
                //Use Part Name
                //newLines.Add($"\tPart \"{GetSubstringByString("\"", "_Mesh", meshes[i][0])}\" {{");

                //Part
                newLines.Add($"\tPart \"mesh{i}\" {{");

                //Mesh and drawArray for part
                foreach (var line in meshes[i])
                    newLines.Add(line);
                foreach (var line in drawArrays[i])
                    newLines.Add(line);

                //End part
                newLines.Add("\t\t}");
                newLines.Add("\t}");

            }
            
        }

        private void RewriteMaterials()
        {
            for (int i = 0; i < materialNames.Count(); i++)
            {
                newLines.Add($"\tMaterial \"{materialNames[i]}\" {{");

                newLines.Add("\t\tDiffuse 0.705882 0.705882 0.705882 1.000000");
                newLines.Add("\t\tAmbient 0.705882 0.705882 0.705882 1.000000");
                newLines.Add("\t\tReflection 0.000000");
                newLines.Add("\t\tRefraction 1.000000");
                newLines.Add("\t\tBump 0.000000");
                newLines.Add("\t\tBump 0.000000");
                newLines.Add($"\t\tLayer \"layer - {i}\" {{");

                newLines.Add($"\t\t\tSetTexture \"{mapNames[i]}\"");
                newLines.Add("\t\t\tMapType Diffuse");
                newLines.Add("\t\t\tMapFactor 1.000000");
                newLines.Add("\t\t\tBlendFunc ADD SRC_ALPHA INV_SRC_ALPHA");

                newLines.Add("\t\t}");
                newLines.Add("\t}");
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

        //Adds the replacement string to newlineslist at specified line and skips the rest until the last line
        public void ReplaceRange(int first, int last, int i, string newString)
        {
            if (i >= first - 1 && i <= last - 1)
            {
                addLine = false;
                if (i == first - 1)
                {
                    string[] SplitLines = newString.Split('\n');
                    foreach (string line in SplitLines)
                        newLines.Add(line);
                }
            }
        }

        //Get string value between two string values
        public static string GetSubstringByString(string a, string b, string c)
        {
            return c.Substring((c.IndexOf(a) + a.Length), (c.IndexOf(b) - c.IndexOf(a) - a.Length));
        }

        private void btn_Create_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void btn_Extract_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        public void MoveItem(int direction)
        {
            // Checking selected item
            if (listBox_AnimationOrder.SelectedItem == null || listBox_AnimationOrder.SelectedIndex < 0)
                return; // No selected item - nothing to do

            // Calculate new index using move direction
            int newIndex = listBox_AnimationOrder.SelectedIndex + direction;

            // Checking bounds of the range
            if (newIndex < 0 || newIndex >= listBox_AnimationOrder.Items.Count)
                return; // Index out of range - nothing to do

            object selected = listBox_AnimationOrder.SelectedItem;

            // Removing removable element
            listBox_AnimationOrder.Items.Remove(selected);
            // Insert it in new position
            listBox_AnimationOrder.Items.Insert(newIndex, selected);
            // Restore selection
            listBox_AnimationOrder.SetSelected(newIndex, true);
        }

        private void Up_Click(object sender, EventArgs e)
        {
            MoveItem(-1);
        }

        private void Down_Click(object sender, EventArgs e)
        {
            MoveItem(1);
        }
    }
}
