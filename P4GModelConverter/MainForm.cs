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
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Reflection;

namespace P4GModelConverter
{
    public partial class MainForm : Form
    {
        List<string> newLines = new List<string>();
        List<Bone> bones = new List<Bone>();
        List<Part> parts = new List<Part>();
        List<Material> materials = new List<Material>();
        List<Tuple<string, string>> boneDrawPartPairs = new List<Tuple<string, string>>();
        List<List<string>> animations = new List<List<string>>();
        string extensionlessPath;
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
                Rotate = rotate;
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
            public string Rotate { get; set; }
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
            CreateMDS(path);
        }

        private void CreateMDS(string path)
        {
            //Re-initialize variables
            newLines = new List<string>();
            bones = new List<Bone>();
            parts = new List<Part>();
            materials = new List<Material>();
            boneDrawPartPairs = new List<Tuple<string, string>>();
            extensionlessPath = Path.Combine(Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path));

            //Extract if file exists
            if (File.Exists(path))
            {
                if (Path.GetExtension(path).ToUpper() == ".PAC")
                {
                    //todo: extract AMD
                    path = extensionlessPath + ".AMD";
                }
                if (Path.GetExtension(path).ToUpper() == ".AMD")
                {
                    //todo: extract GMO
                    path = extensionlessPath + ".GMO";
                }
                if (Path.GetExtension(path).ToUpper() == ".FBX")
                {
                    string mdsPath = extensionlessPath + ".mds";
                    if (chkBox_GMOtoFBX.Checked)
                    {
                        GMOConv(path); //Convert FBX directly to GMO without fixes first (optional, improves FBX support)
                        GMOTool(extensionlessPath + ".gmo", true); //Create MDS from converted GMO
                    }
                    else
                        GMOTool(path, true); //Create MDS from FBX
                    while (!File.Exists(mdsPath)) { } //Wait for mds to be created before continuing
                    FixMDS(mdsPath); //Rewrite MDS with working materials/drawparts
                    newLines = newLines.Where(m => !string.IsNullOrEmpty(m)).ToList(); //Remove empty strings from MDS
                    ConvertTextures(); //Auto-Convert textures to TM2

                    if (chkBox_Animations.Checked) //Add loaded anims to end of MDS
                    {
                        newLines.RemoveAt(newLines.Count - 1);
                        foreach (var animation in animations)
                            foreach (var line in animation)
                                newLines.Add(line);
                        newLines.Add("}");
                    }
                }
                else if (Path.GetExtension(path).ToUpper() == ".GMO")
                {
                    if (chkBox_Extract.Checked)
                        ExtractTextures(path); //Extract TM2 textures (optional)
                    GMOTool(path, true); //Create MDS from GMO
                    string mdsPath = extensionlessPath + ".mds";
                    while (!File.Exists(mdsPath)) { } //Wait for mds to be created before continuing
                    FixMDS(mdsPath); //Rewrite MDS with working materials/drawparts
                    newLines = newLines.Where(m => !string.IsNullOrEmpty(m)).ToList(); //Remove empty strings from MDS
                }
                ReorderAnimations(); //Swap animation order based on listbox
                string newMDSPath = $"{extensionlessPath}_p4g.mds";
                if (File.Exists(newMDSPath))
                    File.Delete(newMDSPath);
                File.AppendAllLines(newMDSPath, newLines);

                lbl_AnimationsLoaded.Text = $"{animations.Count} Animations Loaded";
                if (animations.Count > 0) //Allow animation mds to be exported if any are found
                    btn_ExportAnim.Enabled = true;
                
            }
        }

        //Convert textures to tm2 and replace lines referencing other formats with new path
        private void ConvertTextures()
        {
            for (int i = 0; i < newLines.Count; i++)
            {
                if (newLines[i].StartsWith("\t\tFileName") && !newLines[i].Contains(".tm2"))
                {
                    string textureName = newLines[i].Replace("\t\tFileName \"", "").Replace("\"", "");
                    GIMConv(textureName);
                    newLines[i] = $"\t\tFileName \"{textureName}.tm2\"";
                }
            }
        }

        private void GMOConv(string model)
        {
            Process cmd = new Process();
            cmd.StartInfo.FileName = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\GMO\\GmoConv.exe";
            //cmd.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            cmd.StartInfo.Arguments = $"\"{model}\"";
            cmd.Start();
            cmd.WaitForExit();
        }

        private void GIMConv(string texture)
        {
            Process cmd = new Process();
            cmd.StartInfo.FileName = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\GIM\\GimConv.exe";
            //cmd.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            cmd.StartInfo.Arguments = $"\"{texture}\" -o \"{texture}.tm2\"";
            cmd.Start();
            cmd.WaitForExit();
        }

        private void GMOView(string model)
        {
            Process cmd = new Process();
            cmd.StartInfo.FileName = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\GMO\\GmoView.exe";
            //cmd.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            cmd.StartInfo.Arguments = $"\"{model}\"";
            cmd.Start();
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

            if (textureNames.Count > 0)
            {
                if (File.Exists($"{Path.GetDirectoryName(path)}\\{textureNames[0]}"))
                    return;

                //Get textures and write files
                offset = 0;
                for (int i = 0; i < textureNames.Count(); i++)
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
        }

        //Create GMO from MDS
        private void btn_Create_DragDrop(object sender, DragEventArgs e)
        {
            string path = ((string[])e.Data.GetData(DataFormats.FileDrop, false))[0];
            CreateGMO(path); 
        }

        private void CreateGMO(string path)
        {
            if (File.Exists(path) && Path.GetExtension(path).ToUpper() == ".MDS")
            {
                //Make sure MDS isn't just exported animation data
                if (File.ReadAllLines(path)[0] == ".MDS 0.95")
                {
                    extensionlessPath = Path.Combine(Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path));
                    GMOTool(path, false);
                    if (chkBox_PCFix.Checked)
                        GMOFixTool(extensionlessPath + ".gmo");

                    if (File.Exists(extensionlessPath + ".AMD"))
                    {
                        //Todo: REPLACE GMO DATA IN AMD
                    }
                    if (File.Exists(extensionlessPath + ".PAC"))
                    {
                        //Todo: AUTO REPACK AMD INTO PAC
                    }
                }

                if (chkBox_ViewGMO.Checked)
                    GMOView(extensionlessPath + ".gmo");
            }
        }

        private void GMOTool(string path, bool extract)
        {
            Process cmd = new Process();
            cmd.StartInfo.FileName = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\GMOTool.exe";
            //cmd.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            cmd.StartInfo.Arguments = $"\"{path}\"";
            if (extract)
                cmd.StartInfo.Arguments += " -E";
            else
                cmd.StartInfo.Arguments += " -PSV";
            cmd.Start();
            cmd.WaitForExit();
        }

        private void GMOFixTool(string path)
        {
            Process cmd = new Process();
            cmd.StartInfo.FileName = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\p4gpc-gmofix.exe";
            //cmd.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            cmd.StartInfo.Arguments = $"\"{path}\"";
            cmd.Start();
            cmd.WaitForExit();
        }

        private void FixMDS(string path)
        {
            //Lines from the original file and new collection
            string[] lines = File.ReadAllLines(path);
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
                        bone.Name = lines[x].Replace("\tBone \"", "").Replace("\" {", "").Replace("_", " ").Replace(" Bone", "_Bone");
                        if (bone.Name == "player root_Bone")
                            bone.Name = "player_root_Bone";
                        x++;
                        while (!lines[x].StartsWith("\t}"))
                        {
                            if (lines[x].StartsWith("\t\tBoundingBox"))
                                bone.BoundingBox = lines[x];
                            if (lines[x].StartsWith("\t\tTranslate"))
                                bone.Translate = lines[x];
                            if (lines[x].StartsWith("\t\tRotate"))
                                bone.Rotate = lines[x];
                            if (lines[x].StartsWith("\t\tParentBone"))
                                bone.ParentBone = lines[x].Replace("_", " ").Replace(" Bone", "_Bone").Replace("player root_Bone","player_root_Bone");
                            if (lines[x].StartsWith("\t\tScale"))
                                bone.Scale = lines[x];
                            if (lines[x].StartsWith("\t\tBlindData"))
                                bone.BlindData = lines[x];
                            if (lines[x].StartsWith("\t\tBlendBones"))
                                bone.BlendBones = lines[x].Replace("_", " ").Replace(" Bone", "_Bone").Replace("player root_Bone", "player_root_Bone");
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
                        if (!chkBox_Dummy.Checked)
                            RewriteMaterials();
                        else
                            newLines.Add("\tMaterial \"Dummy\" {\n\t\tDiffuse 1.000000 1.000000 1.000000 1.000000\n\t\tAmbient 1.000000 1.000000 1.000000 1.000000\n\t\tReflection 0.000000\n\t\tRefraction 1.000000\n\t\tBump 0.000000\n\t\tBlindData \"transAlgo\" 4\n\t\tLayer \"dummy_layer\" {\n\t\t\tBlendFunc ADD SRC_ALPHA INV_SRC_ALPHA\n\t\t}\n\t}");
                        //Write the rest of the file normally from source (textures, anims...)
                        cutoff = int.MaxValue;
                    }
                }

                //Add line to new collection if it doesn't need to be replaced
                if (addLine && i < cutoff)
                {
                    newLines.Add(lines[i]);
                }
                    
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
                newLines.Add(bones[w].Rotate);
                if (bones[w].Scale != null)
                    newLines.Add(bones[w].Scale);
                else
                    newLines.Add("\t\tScale 1.000000 1.000000 1.000000");
                if (bones[w].Name == txt_WpnBone.Text)
                    bones[w].BlindData = "\t\tBlindData \"per3Helper\" 500 1092542719 1085565955 1065731131 -1082972548 1048875842 -1104161467 1024833421 0 0";
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
                    if (!chkBox_Dummy.Checked)
                        newLines.Add(parts[w].Meshes[z]);
                    else
                        newLines.Add(Regex.Replace(parts[w].Meshes[z], "SetMaterial \".*\"", "SetMaterial \"Dummy\""));
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
                if (materials[w].BlendFunc != null && materials[w].BlendFunc.Contains("ADD SRC_ALPHA ONE"))
                    newLines.Add("\t\tRenderState CULL_FACE 0");
                newLines.Add(materials[w].Diffuse);
                if (materials[w].Diffuse == null)
                    newLines.Add("\t\tDiffuse 0.800000 0.800000 0.800000 1.000000");
                newLines.Add(materials[w].Ambient);
                if (materials[w].Ambient == null)
                    newLines.Add("\t\tAmbient 0.800000 0.800000 0.800000 1.000000");
                newLines.Add(materials[w].Reflection);
                if (materials[w].Reflection == null)
                    newLines.Add("\t\tReflection 0.000000");
                newLines.Add(materials[w].Refraction);
                if (materials[w].Refraction == null)
                    newLines.Add("\t\tRefraction 1.000000");
                newLines.Add(materials[w].Bump);
                if (materials[w].Bump == null)
                    newLines.Add("\t\tBump 0.000000");
                if (materials[w].BlendFunc != null && materials[w].BlendFunc.Contains("ADD SRC_ALPHA ONE"))
                    newLines.Add("\t\tBlindData \"transAlgo\" 1");
                else
                    newLines.Add("\t\tBlindData \"transAlgo\" 4");

                newLines.Add($"\t\tLayer \"layer - 1\" {{");

                newLines.Add(materials[w].SetTexture);
                newLines.Add("\t\t\tMapType Diffuse");
                newLines.Add("\t\t\tMapFactor 1.000000");
                if (materials[w].BlendFunc != null)
                    newLines.Add(materials[w].BlendFunc);
                else
                    newLines.Add("\t\t\tBlendFunc ADD SRC_ALPHA INV_SRC_ALPHA");
                if (materials[w].BlendFunc != null && materials[w].BlendFunc.Contains("ADD SRC_ALPHA ONE"))
                    newLines.Add("\t\t\tTexWrap CLAMP CLAMP\n\t\t\tTexGen NORMAL\n\t\t\tTexMatrix \\\n\t\t\t\t1.000000 0.000000 0.000000 0.000000 \\\n\t\t\t\t0.000000 1.000000 0.000000 0.000000 \\\n\t\t\t\t0.000000 0.000000 1.000000 0.000000 \\\n\t\t\t\t0.000000 0.000000 0.000000 1.000000");

                newLines.Add("\t\t}");
                newLines.Add("\t}");
            }
        }

        //If listbox order has changed, reorder animations to match
        private void ReorderAnimations()
        {
            animations = new List<List<string>>();
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

        private void btn_Create_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void btn_Extract_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void btn_Extract_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.Filters.Add(new CommonFileDialogFilter("GMO Model", "*.gmo"));
            dialog.Filters.Add(new CommonFileDialogFilter("FBX Model", "*.fbx"));
            dialog.Title = "Load Model...";
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                CreateMDS(dialog.FileName);
            }
        }

        private void btn_Create_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.Filters.Add(new CommonFileDialogFilter("MDS File", "*.mds"));
            dialog.Title = "Load MDS File...";
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                CreateGMO(dialog.FileName);
            }
        }

        private void btn_ExportAnim_Click(object sender, EventArgs e)
        {
            if (animations.Count > 0)
            {
                CommonSaveFileDialog dialog = new CommonSaveFileDialog();
                dialog.Filters.Add(new CommonFileDialogFilter("MDS File", "*.mds"));
                dialog.Title = "Save Animation Set...";
                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    if (File.Exists(dialog.FileName))
                        File.Delete(dialog.FileName);
                    foreach(var animation in animations)
                        File.AppendAllLines(dialog.FileName, animation);
                    File.AppendAllText(dialog.FileName, "\n}");
                }
            }
        }

        private void btn_ImportAnim_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.Filters.Add(new CommonFileDialogFilter("MDS File", "*.mds"));
            dialog.Title = "Load Animation Set...";
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                newLines = File.ReadAllLines(dialog.FileName).ToList();
                ReorderAnimations();
                lbl_AnimationsLoaded.Text = $"{animations.Count()} Animations Loaded";
                if (animations.Count > 0)
                    btn_ExportAnim.Enabled = true;
            }
            
        }
    }
}
