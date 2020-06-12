using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security;
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
        List<Bone> bones = new List<Bone>();
        List<Part> parts = new List<Part>();
        List<Material> materials = new List<Material>();
        List<Tuple<string, string>> boneDrawPartPairs = new List<Tuple<string, string>>();
        string[] lines;
        bool addLine;

        public MainForm()
        {
            InitializeComponent();
        }

        public class Bone
        {
            public Bone() { }
            public Bone(string name, string translate, string rotate, string scale, List<string> drawParts, string blindData, string blendBones, string blendOffsets, string parent, string boundingBox)
            {
                Name = name;
                ParentBone = parent;
                Translate = translate;
                RotateZYX = rotate;
                Scale = scale;
                DrawParts = drawParts;
                BlindData = blindData;
                BlendBones = blendBones;
                BlendOffsets = blendOffsets;
                BoundingBox = boundingBox;
            }
            public string Name { get; set; }
            public string ParentBone { get; set; }
            public string Translate { get; set; }
            public string RotateZYX { get; set; }
            public string Scale { get; set; }
            public List<string> DrawParts { get; set; } = new List<string>();
            public string BlindData { get; set; }
            public string BlendBones { get; set; }
            public string BlendOffsets { get; set; }
            public string BoundingBox { get; set; }
        }

        public class Part
        {
            public Part() { }
            public Part(string name, string boundingBox, List<string> meshes, List<string> arrays)
            {
                Name = name;
                BoundingBox = boundingBox;
                Meshes = meshes;
                Arrays = arrays;
            }
            public string Name { get; set; }
            public string BoundingBox { get; set; }
            public List<string> Meshes { get; set; } = new List<string>();
            public List<string> Arrays { get; set; } = new List<string>();
        }

        public class Material
        {
            public Material() { }
            public Material(string name, string diffuse, string ambient, string reflection, string refraction, string bump, string blindData, string setTexture, string blendFunc)
            {
                Name = name;
                Diffuse = diffuse;
                Ambient = ambient;
                Reflection = reflection;
                Refraction = refraction;
                Bump = bump;
                BlindData = blindData;
                SetTexture = setTexture;
                BlendFunc = blendFunc;
            }
            public string Name { get; set; }
            public string Diffuse { get; set; }
            public string Ambient { get; set; }
            public string Reflection { get; set; }
            public string Refraction { get; set; }
            public string Bump { get; set; }
            public string BlindData { get; set; }
            public string SetTexture { get; set; }
            public string BlendFunc { get; set; }
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
                newLines = newLines.Where(m => !string.IsNullOrEmpty(m)).ToList();
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
                        int x = i;
                        //Add bone data to bone list
                        Bone bone = new Bone();
                        bone.Name = lines[x].Replace("\tBone \"", "").Replace("\" {", "");
                        x++;
                        while (!lines[x].StartsWith("\t}"))
                        {
                            if (lines[x].StartsWith("\t\tBoundingBox"))
                                bone.BoundingBox = lines[x];
                            if (lines[x].StartsWith("\t\tTranslate"))
                                bone.Translate = lines[x];
                            if (lines[x].StartsWith("\t\tRotateZYX"))
                                bone.RotateZYX = lines[x];
                            if (lines[x].StartsWith("\t\tParentBone"))
                                bone.ParentBone = lines[x];
                            if (lines[x].StartsWith("\t\tScale"))
                                bone.Scale = lines[x];
                            if (lines[x].StartsWith("\t\tBlindData"))
                                bone.BlindData = lines[x];
                            if (lines[x].StartsWith("\t\tBlendBones"))
                                bone.BlendBones = lines[x];
                            if (lines[x].StartsWith("\t\tDrawPart"))
                                bone.DrawParts.Add(lines[x]);
                            if (lines[x].StartsWith("\t\tBlendOffsets"))
                            {
                                string blendOffsets = lines[x];
                                x++;
                                while (lines[x].StartsWith("\t\t\t"))
                                {
                                    blendOffsets += "\n" + lines[x];
                                    x++;
                                }
                                bone.BlendOffsets = blendOffsets;
                            }
                            else
                                x++;
                        }
                        bones.Add(bone);
                    }
                    else if (lines[i].Contains("\tPart"))
                    {
                        int x = i;
                        //Add part data to part list
                        Part part = new Part();
                        part.Name = lines[x].Replace("\tPart \"", "").Replace("\" {", "");
                        x++;
                        while (!lines[x].StartsWith("\t}"))
                        {
                            Console.WriteLine(lines[x]);
                            if (lines[x].Contains("\t\tMesh"))
                            {
                                string mesh = lines[x];
                                x++;
                                while (!lines[x].Contains("}"))
                                {
                                    mesh += "\n" + lines[x];
                                    x++;
                                }
                                mesh += "\n\t\t}";
                                part.Meshes.Add(mesh);
                            }
                            else if (lines[x].StartsWith("\t\tArrays"))
                            {
                                string array = lines[x];
                                x++;
                                while (!lines[x].Contains("}"))
                                {
                                    array += "\n" + lines[x];
                                    x++;
                                }
                                array += "\n\t\t}";
                                part.Arrays.Add(array);
                            }
                            else if (lines[x].StartsWith("\t\tBoundingBox"))
                            {
                                part.BoundingBox = lines[x];
                                x++;
                            }
                            else
                                x++;
                        }
                        parts.Add(part);
                    }
                    else if (lines[i].StartsWith("\tMaterial"))
                    {
                        int x = i;
                        //Add material data to material list
                        Material mat = new Material();
                        mat.Name = lines[x].Replace("\tMaterial \"","").Replace("\" {","");
                        x++;
                        while (!lines[x].StartsWith("\t}"))
                        {
                            if (lines[x].StartsWith("\t\tDiffuse"))
                                mat.Diffuse = lines[x];
                            if (lines[x].StartsWith("\t\tAmbient"))
                                mat.Ambient = lines[x];
                            if (lines[x].StartsWith("\t\tReflection"))
                                mat.Reflection = lines[x];
                            if (lines[x].StartsWith("\t\tRefraction"))
                                mat.Refraction = lines[x];
                            if (lines[x].StartsWith("\t\tBump"))
                                mat.Bump = lines[x];
                            if (lines[x].StartsWith("\t\tBlindData"))
                                mat.BlindData = lines[x];
                            if (lines[x].StartsWith("\t\t\tSetTexture"))
                                mat.SetTexture = lines[x];
                            if (lines[x].StartsWith("\t\t\tBlendFunc"))
                                mat.BlendFunc = lines[x];
                            x++;
                        }
                        materials.Add(mat);
                    }
                    else if (lines[i].StartsWith("\tTexture"))
                    {
                        //Write new bones/meshes/arrays/materials to file
                        MatchBonesAndDrawParts();
                        RewriteBones();
                        RewriteParts();
                        RewriteMaterials();
                        //Write the rest of the file normally from source (textures, anims...)
                        cutoff = int.MaxValue;
                    }
                }

                //Add line to new collection if it doesn't need to be replaced
                if (addLine && i < cutoff)
                    newLines.Add(lines[i]);
            }
        }

        private void MatchBonesAndDrawParts()
        {
            for (int w = 0; w < parts.Count; w++)
            {
                for (int z = 0; z < parts[w].Meshes.Count; z++)
                {
                    //Add (newly named) part and corresponding bone to list
                    string splitName = $"{parts[w].Name}_{z}";
                    string firstBone = bones.First(b => b.DrawParts.Any(a => a.Contains(parts[w].Name))).Name;
                    Tuple<string, string> boneDrawPartPair = new Tuple<string, string>(firstBone, splitName);
                    boneDrawPartPairs.Add(boneDrawPartPair);
                }
            }
        }

        private void RewriteBones()
        {
            for (int w = 0; w < bones.Count; w++)
            {
                newLines.Add($"\tBone \"{bones[w].Name}\" {{");
                newLines.Add(bones[w].BoundingBox);
                newLines.Add(bones[w].ParentBone);
                newLines.Add(bones[w].BlendBones);
                newLines.Add(bones[w].BlendOffsets);
                newLines.Add(bones[w].Translate);
                newLines.Add(bones[w].RotateZYX);
                newLines.Add(bones[w].Scale);
                newLines.Add(bones[w].BlindData);
                foreach (var drawPartPair in boneDrawPartPairs.Where(p => p.Item1.Equals(bones[w].Name)))
                    newLines.Add($"\t\tDrawPart \"{drawPartPair.Item2}\"");
                newLines.Add("\t}");
            }
        }

        private void RewriteParts()
        {
            //Format text so that there's only one mesh/array pair per "part"
            for (int w = 0; w < parts.Count; w++)
            {
                for (int z = 0; z < parts[w].Meshes.Count; z++)
                {
                    newLines.Add($"\tPart \"{parts[w].Name}_{z}\" {{");
                    newLines.Add(parts[w].BoundingBox);
                    newLines.Add(parts[w].Meshes[z]);
                    newLines.Add(parts[w].Arrays[z]);
                    newLines.Add("\t}");
                }
            }
            
        }

        private void RewriteMaterials()
        {
            //Format materials so that they all use one layer and map/blend type
            for (int w = 0; w < materials.Count(); w++)
            {
                newLines.Add($"\tMaterial \"{materials[w].Name}\" {{");

                newLines.Add(materials[w].Diffuse);
                newLines.Add(materials[w].Ambient);
                newLines.Add(materials[w].Reflection);
                newLines.Add(materials[w].Refraction);
                newLines.Add(materials[w].Bump);
                newLines.Add($"\t\tLayer \"layer - 1\" {{");

                newLines.Add(materials[w].SetTexture);
                newLines.Add("\t\t\tMapType Diffuse");
                newLines.Add("\t\t\tMapFactor 1.000000");
                newLines.Add(materials[w].BlendFunc);
                if (materials[w].BlendFunc.Contains("ADD SRC_ALPHA ONE"))
                    newLines.Add("\t\t\tTexWrap CLAMP CLAMP\n\t\t\tTexGen NORMAL\n\t\t\tTexMatrix \\\n\t\t\t1.000000 0.000000 0.000000 0.000000 \\\n\t\t\t0.000000 1.000000 0.000000 0.000000 \\\n\t\t\t0.000000 0.000000 1.000000 0.000000 \\\n\t\t\t0.000000 0.000000 0.000000 1.000000");

                newLines.Add("\t\t}");
                newLines.Add("\t}");
            }
        }

        //If listbox order has changed, reorder animations to match
        private void ReorderAnimations()
        {
            List<List<string>> animations = new List<List<string>>();
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
