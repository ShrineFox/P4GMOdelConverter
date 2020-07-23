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
using System.Runtime.Remoting.Contexts;
using System.Threading;

namespace P4GModelConverter
{
    public partial class MainForm : Form
    {
        List<string> newLines = new List<string>();
        List<Bone> bones = new List<Bone>();
        List<Part> parts = new List<Part>();
        List<Material> materials = new List<Material>();
        List<Tuple<string, string>> boneDrawPartPairs = new List<Tuple<string, string>>();
        List<Animation> animations = new List<Animation>();
        List<Texture> textures = new List<Texture>();
        string extensionlessPath;
        string p4gMDSpath;
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
        bool presetChanged;
        bool updateMDS;
        int selectedRow;

        public MainForm()
        {
            InitializeComponent();
            comboBox_Preset.SelectedIndex = 0;
            comboBox_Preview.SelectedIndex = 0;
        }

        /*
         * Type collections
         */

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
        public class Texture
        {
            public Texture() { }
            public Texture(string name, string filename)
            {
                Name = name;
                FileName = filename;
            }
            public string Name { get; set; }
            public string FileName { get; set; }
        }
        public class Animation
        {
            public Animation() { }
            public Animation(string name, string frameloop, string framerate, List<string> animate, List<List<string>> fcurve)
            {
                Name = name;
                FrameLoop = frameloop;
                FrameRate = framerate;
                Animate = animate;
                FCurve = fcurve;
            }
            public string Name { get; set; }
            public string FrameLoop { get; set; }
            public string FrameRate { get; set; }
            public List<string> Animate { get; set; }
            public List<List<string>> FCurve { get; set; }
        }

        /*
         * FBX/GMO/MDS Conversion
         */

        //Load GMO or FBX via drag and drop
        private void btn_Extract_DragDrop(object sender, DragEventArgs e)
        {
            string path = ((string[])e.Data.GetData(DataFormats.FileDrop, false))[0];
            CreateMDS(path);
        }

        //Load MDS via drag and drop for applying fixes
        private void btn_Fix_DragDrop(object sender, DragEventArgs e)
        {
            string path = ((string[])e.Data.GetData(DataFormats.FileDrop, false))[0];
            FixMDS(path); //Rewrite MDS with working materials/drawparts
        }

        //Load MDS via drag and drop for GMO creation
        private void btn_Create_DragDrop(object sender, DragEventArgs e)
        {
            string path = ((string[])e.Data.GetData(DataFormats.FileDrop, false))[0];
            CreateGMO(path);
        }

        //Write new MDS file from GMO or FBX
        private void CreateMDS(string path)
        {
            //Re-initialize variables
            newLines = new List<string>();
            bones = new List<Bone>();
            parts = new List<Part>();
            materials = new List<Material>();
            textures = new List<Texture>();
            if (!chkBox_Animations.Checked)
            {
                animations = new List<Animation>();
                lbl_AnimationsLoaded.Text = "No Animations Loaded";
                dataGridView_AnimationOrder.Rows.Clear();
            }
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
                    path = extensionlessPath + ".gmo";
                }
                if(Path.GetExtension(path).ToUpper() == ".DAE" || Path.GetExtension(path).ToUpper() == ".SMD" || Path.GetExtension(path).ToUpper() == ".FBX")
                {
                    if (chkBox_FBXOptimize.Checked || Path.GetExtension(path).ToUpper() != ".FBX")
                    {
                        path = CreateOptimizedFBX(path); //Optimize FBX with settings or convert to FBX
                    }
                    if (chkBox_FBXtoGMO.Checked)
                    {
                        GMOConv(path); //Convert FBX directly to GMO without fixes first
                        path = extensionlessPath + ".gmo";
                    }
                }
                if (Path.GetExtension(path).ToUpper() == ".GMO" || Path.GetExtension(path).ToUpper() == ".FBX")
                {
                    if (chkBox_ExtractTex.Checked && Path.GetExtension(path).ToUpper() == ".GMO")
                        ExtractTextures(path); //Extract TM2 Textures
                    GMOTool(path, true); //Create MDS
                }
            }
        }
        
        //Convert model to FBX through Noesis commandline using specified settings
        private string CreateOptimizedFBX(string path)
        {
            string args = "";
            if (chkBox_fbxoldexport.Checked)
                args += "-fbxoldexport ";
            if (chkBox_fbxascii.Checked)
                args += "-fbxascii ";
            NoesisOptimize(path, $"{args} {txtBox_NoesisOptions.Text}");
            extensionlessPath += "_noesis";
            return $"{extensionlessPath}.fbx";
        }

        //Create GMO from MDS
        private void CreateGMO(string path)
        {
            if (File.Exists(path) && Path.GetExtension(path).ToUpper() == ".MDS")
            {
                extensionlessPath = Path.Combine(Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path));
                UpdateAnimationDataGridView();
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
            {
                if (comboBox_Preview.SelectedIndex == 0)
                    Noesis($"\"{extensionlessPath + ".gmo"}\"");
                else if (comboBox_Preview.SelectedIndex == 1)
                    GMOView(extensionlessPath + ".gmo");
            }
                
        }

        //Save data from MDS to objects for bones/parts/materials/motion etc.
        private void ReadMDS(string path)
        {
            //Lines from the original file and new collection
            string[] lines = File.ReadAllLines(path);
            newLines = new List<string>();

            newLines.Add(".MDS 0.95");
            if (extensionlessPath != null)
                newLines.Add($"Model \"{extensionlessPath.Split('\\').Last()}\" {{");
            else
                newLines.Add("Model \"Model\" {");
            newLines.Add("BoundingBox -64.149666 -42.046124 -11.595747 81.043938 181.061172 18.696728");

            for (int i = 0; i < lines.Count(); i++)
            {
                //Organize lines in objects for rewriting later
                if (lines[i].StartsWith("\tBone"))
                {
                    int x = i;
                    //Add bone data to bone list
                    Bone bone = new Bone();
                    bone.Name = lines[x].Replace("\tBone \"", "").Replace("\" {", "");
                    if (chkBox_RenameBones.Checked)
                        bone.Name = SanitizeBoneName(bone.Name);
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
                        {
                            if (chkBox_RenameBones.Checked)
                                bone.ParentBone = SanitizeBoneName(lines[x]);
                            else
                                bone.ParentBone = lines[x];
                        }
                        if (lines[x].StartsWith("\t\tScale"))
                            bone.Scale = lines[x];
                        if (lines[x].StartsWith("\t\tBlindData"))
                            bone.BlindData = lines[x];
                        if (lines[x].StartsWith("\t\tBlendBones"))
                        {
                            if (chkBox_RenameBones.Checked)
                                bone.BlendBones = SanitizeBoneName(lines[x]);
                            else
                                bone.BlendBones = lines[x];
                        }
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
                    mat.Name = lines[x].Replace("\tMaterial \"", "").Replace("\" {", "");
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
                    var tex = new Texture();
                    tex.Name = lines[i].Replace("\tTexture \"", "").Replace("\" {", "");
                    tex.FileName = lines[i + 1].Replace("\t\tFileName \"", "").Replace("\"", "");
                    textures.Add(tex);
                }
                else if (lines[i].StartsWith("\tMotion") && chkBox_Animations.Checked)
                {
                    var anim = new Animation();
                    var animateLines = new List<string>();
                    var fcurves = new List<List<string>>();
                    anim.Name = lines[i].Replace("\tMotion \"", "").Replace("\" {", "");

                    int x = i + 1;
                    while (x < lines.Count() && lines[x] != "\t}")
                    {
                        if (lines[x].StartsWith("\t\tFrameLoop"))
                            anim.FrameLoop = lines[x];
                        if (lines[x].StartsWith("\t\tFrameRate"))
                            anim.FrameRate = lines[x];
                        if (lines[x].StartsWith("\t\tAnimate"))
                            animateLines.Add(lines[x]);
                        if (lines[x].StartsWith("\t\tFCurve"))
                        {
                            var fcurveLines = new List<string>();
                            for (int w = x; w < lines.Count(); w++)
                            {
                                fcurveLines.Add(lines[w]);
                                if (lines[w].StartsWith("\t\t}"))
                                    break;
                            }
                            fcurves.Add(fcurveLines);
                        }
                        x++;
                    }
                    anim.Animate = animateLines;
                    anim.FCurve = fcurves;
                    animations.Add(anim);
                }
            }
        }

        private string SanitizeBoneName(string name)
        {
            name = name.Replace("_", " ");
            name = name.Replace("player ", "player_");
            if (!name.Contains(" Bone"))
            {
                name = name.Replace("\"", " Bone\"");
                name = name.Replace("  Bone\"", "\"");
            }
            name = name.Replace(" Bone", "_Bone");
            return name;
        }

        //Reformat data for writing to new mds file
        private void FixMDS(string path)
        {
            ReadMDS(path);
            MatchBonesAndDrawParts();
            RewriteBones();
            RewriteParts();
            if (!chkBox_Dummy.Checked)
            {
                RewriteMaterials();
                RewriteTextures(path);
            }
            else
                newLines.Add("\tMaterial \"Dummy\" {\n\t\tDiffuse 1.000000 1.000000 1.000000 1.000000\n\t\tAmbient 1.000000 1.000000 1.000000 1.000000\n\t\tReflection 0.000000\n\t\tRefraction 1.000000\n\t\tBump 0.000000\n\t\tBlindData \"transAlgo\" 4\n\t\tLayer \"dummy_layer\" {\n\t\t\tBlendFunc ADD SRC_ALPHA INV_SRC_ALPHA\n\t\t}\n\t}");
            CreateAnimationRows();
            UpdateAnimationDataGridView();
            newLines.AddRange(WriteAnimations());
            newLines = newLines.Where(m => !string.IsNullOrEmpty(m)).ToList(); //Remove empty strings from MDS
            extensionlessPath = Path.Combine(Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path));
            p4gMDSpath = $"{extensionlessPath}_p4g.mds";
            if (File.Exists(p4gMDSpath))
                File.Delete(p4gMDSpath);
            File.AppendAllLines(p4gMDSpath, newLines);
            btn_Update.Enabled = true;
            chkBox_AutoUpdate.Enabled = true;
        }

        //Add (newly named) part and corresponding bone to list
        private void MatchBonesAndDrawParts()
        {
            for (int w = 0; w < parts.Count; w++)
            {
                for (int z = 0; z < parts[w].Meshes.Count; z++)
                {
                    string splitName = $"{parts[w].Name}_{z}";
                    string firstBone = bones.First(b => b.DrawParts.Any(a => a.Contains(parts[w].Name))).Name;
                    Tuple<string, string> boneDrawPartPair = new Tuple<string, string>(firstBone, splitName);
                    boneDrawPartPairs.Add(boneDrawPartPair);
                }
            }
        }

        //Add bone data to text file lines when possible
        private void RewriteBones()
        {
            for (int w = 0; w < bones.Count; w++)
            {
                newLines.Add($"\tBone \"{bones[w].Name}\" {{");
                if (bones[w].BoundingBox != null)
                    newLines.Add(bones[w].BoundingBox);
                if (bones[w].ParentBone != null)
                    newLines.Add(bones[w].ParentBone);
                if (bones[w].BlendBones != null)
                    newLines.Add(bones[w].BlendBones);
                if (bones[w].BlendOffsets != null)
                    newLines.Add(bones[w].BlendOffsets);
                if (bones[w].Translate != null)
                    newLines.Add(bones[w].Translate);
                if (bones[w].Rotate != null)
                    newLines.Add(bones[w].Rotate);
                if (bones[w].Scale != null)
                    newLines.Add(bones[w].Scale);
                else
                    newLines.Add("\t\tScale 1.000000 1.000000 1.000000");
                if (bones[w].Name == txt_WpnBone.Text)
                    bones[w].BlindData = "\t\tBlindData \"per3Helper\" 500 1092542719 1085565955 1065731131 -1082972548 1048875842 -1104161467 1024833421 0 0";
                if (bones[w].BlindData != null)
                    newLines.Add(bones[w].BlindData);
                foreach (var drawPartPair in boneDrawPartPairs.Where(p => p.Item1.Equals(bones[w].Name)))
                    newLines.Add($"\t\tDrawPart \"{drawPartPair.Item2}\"");
                newLines.Add("\t}");
            }
        }

        //Format text so that there's only one mesh/array pair per "part"
        private void RewriteParts()
        {
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

        //Format materials so that they all use one layer and map/blend type
        private void RewriteMaterials()
        {
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

        //Rename and convert textures depending on settings
        private void RewriteTextures(string path)
        {
            foreach (var texture in textures)
            {
                if (texture.FileName.StartsWith("\\\\")) //fix for DAE texture paths
                    texture.FileName = texture.FileName.Replace("\\\\", "").Insert(1, ":");
                //Set up texture output path
                string newTexPath = $"{Path.GetDirectoryName(path)}\\textures\\{new DirectoryInfo(texture.FileName).Name}";
                Directory.CreateDirectory($"{Path.GetDirectoryName(path)}\\textures");

                newLines.Add($"\tTexture \"{texture.Name}\" {{");
                //Convert, move and edit path of non-tm2 textures
                if (!texture.FileName.ToLower().Contains(".tm2") && chkBox_AutoConvertTex.Checked)
                {
                    if (File.Exists(texture.FileName))
                        GIMConv(texture.FileName);
                    if (!File.Exists(newTexPath + ".tm2") && File.Exists(texture.FileName))
                        File.Move(texture.FileName + ".tm2", newTexPath + ".tm2");
                    newLines.Add($"\t\tFileName \"textures\\{new DirectoryInfo(texture.FileName).Name}.tm2\"");
                }
                else //Move and edit path of tm2 textures
                {
                    if (!File.Exists(newTexPath))
                        if (File.Exists(texture.FileName))
                            File.Move(texture.FileName, newTexPath);
                    newLines.Add($"\t\tFileName \"textures\\{texture.FileName.Split('\\').Last()}\"");
                }
                newLines.Add($"\t}}");
            }
        }

        /*
         * Animation Management
         */

        //Reorder animations to match listbox order
        private void ReorderAnimations()
        {
            //If there's animations and animations checkbox is checked
            if (animations.Count > 0 && chkBox_Animations.Checked)
            {
                //Reorder animations by name in listbox order
                var newAnims = new List<Animation>();
                //Re-order animations based on order in listbox
                for (int i = 0; i < dataGridView_AnimationOrder.Rows.Count; i++)
                {
                    newAnims.Add(animations.FirstOrDefault(x => x.Name == dataGridView_AnimationOrder.Rows[i].Cells[1].Value.ToString()));
                }
                animations = newAnims.Where(x => x != null).ToList();
                //Use preset bone names if option is selected
                if (presetChanged && comboBox_Preset.SelectedIndex > 0)
                {
                    for (int i = 0; i < animations.Count; i++)
                    {
                        if (animations[i] != null && animationPresets[comboBox_Preset.SelectedIndex].Count > i)
                        {
                            animations[i].Name = animationPresets[comboBox_Preset.SelectedIndex][i];
                            if (animations[i].FCurve == null || animations[i].FCurve.Count == 0)
                                animations[i].Name += " (EMPTY)";
                            dataGridView_AnimationOrder.Rows[i].Cells[0].Value = i + 1;
                            dataGridView_AnimationOrder.Rows[i].Cells[1].Value = animations[i].Name;
                        }
                    }
                }
                //For each mds file matching the latest input, rewrite with new animation order
                if (updateMDS || chkBox_AutoUpdate.Checked)
                {
                    if (p4gMDSpath == null)
                    {
                        CommonOpenFileDialog dialog = new CommonOpenFileDialog();
                        dialog.Filters.Add(new CommonFileDialogFilter("MDS File", "*.mds"));
                        dialog.Title = "Choose MDS to add animations to...";
                        if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                            p4gMDSpath = dialog.FileName;
                    }
                    
                    var mdsLines = File.ReadAllLines(p4gMDSpath).ToList(); //Get all lines up to first animation
                    mdsLines.Remove("}");
                    int index = mdsLines.FindIndex(x => x.StartsWith("\tMotion \""));

                    if (index == -1)
                        index = mdsLines.Count;

                    mdsLines = mdsLines.Take(index).ToList();
                    File.Delete(p4gMDSpath);
                    mdsLines.AddRange(WriteAnimations());
                    File.WriteAllLines(p4gMDSpath, mdsLines.ToArray());
                }
            }
        }

        //Rewrite animation to add to end of mds
        private List<string> WriteAnimations()
        {
            List<string> animationStrings = new List<string>();
            foreach (var animation in animations)
            {
                animationStrings.Add($"\tMotion \"{animation.Name}\" {{");
                if (animation.FrameLoop != null)
                    animationStrings.Add(animation.FrameLoop);
                if (animation.FrameRate != null)
                    animationStrings.Add(animation.FrameRate);
                if (animation.FrameRate != null && animation.Animate.Count > 0)
                {
                    foreach (var line in animation.Animate)
                        animationStrings.Add(line);
                }
                if (animation.FCurve != null && animation.FCurve.Count > 0)
                {
                    foreach (var fcurve in animation.FCurve)
                        foreach (var line in fcurve)
                            animationStrings.Add(line);
                }
                animationStrings.Add("\t}");
            }

            animationStrings.Add("}");
            return animationStrings;
        }

        //Refresh listbox with latest animations (after reordering them)
        private void UpdateAnimationDataGridView()
        {
            if (animations.Count > 0)
            {
                if (dataGridView_AnimationOrder.Rows.Count > 0)
                    ReorderAnimations();
                btn_ExportAnim.Enabled = true;
                lbl_AnimationsLoaded.Text = $"{animations.Count} Animations Loaded";
            }
            else
            {
                btn_ExportAnim.Enabled = false;
                lbl_AnimationsLoaded.Text = $"No Animations Loaded";
            }
        }

        //Refresh listbox
        private void btn_Update_Click(object sender, EventArgs e)
        {
            updateMDS = true;
            UpdateAnimationDataGridView();
            updateMDS = false;
        }

        //Move animation up
        private void Up_Click(object sender, EventArgs e)
        {
            MoveItem(-1);
        }

        //Move animation down
        private void Down_Click(object sender, EventArgs e)
        {
            MoveItem(1);
        }

        //Move animation row up/down depending on button pressed
        public void MoveItem(int direction)
        {
            // Checking selected item
            if (dataGridView_AnimationOrder.SelectedRows == null || dataGridView_AnimationOrder.SelectedRows.Count <= 0)
                return; // No selected item - nothing to do

            // Calculate new index using move direction
            int newIndex = dataGridView_AnimationOrder.SelectedRows[0].Index + direction;

            // Checking bounds of the range
            if (newIndex < 0 || newIndex >= dataGridView_AnimationOrder.Rows.Count)
                return; // Index out of range - nothing to do

            object selected = dataGridView_AnimationOrder.SelectedRows[0];

            // Removing removable element
            dataGridView_AnimationOrder.Rows.Remove((DataGridViewRow)selected);
            // Insert it in new position
            dataGridView_AnimationOrder.Rows.Insert(newIndex, (DataGridViewRow)selected);
            // Restore selection
            dataGridView_AnimationOrder.Rows[newIndex].Selected = true;
            //Scroll to selection
            if (newIndex > 1)
                dataGridView_AnimationOrder.FirstDisplayedScrollingRowIndex = newIndex - 2;
            else
                dataGridView_AnimationOrder.FirstDisplayedScrollingRowIndex = 0;

            //Make sure row numbers are in order
            RefreshRowNumbers();
            if (chkBox_AutoUpdate.Checked)
                UpdateAnimationDataGridView(); //Optionally rewrite MDS with changes
        }

        //Clear Animations datagridview and repopulate with current animation collection
        private void CreateAnimationRows()
        {
            dataGridView_AnimationOrder.Rows.Clear();
            for (int i = 0; i < animations.Count; i++)
            {
                int newRowIndex = dataGridView_AnimationOrder.Rows.Add();
                DataGridViewRow row = dataGridView_AnimationOrder.Rows[newRowIndex];
                row.Cells[0].Value = $"{newRowIndex + 1}";
                row.Cells[1].Value = animations[i].Name;
            }
        }

        //Update Animation datagridview's row numbering after rows have been moved
        private void RefreshRowNumbers()
        {
            // Redo animation row numbering
            for (int i = 0; i < dataGridView_AnimationOrder.Rows.Count; i++)
            {
                dataGridView_AnimationOrder.Rows[i].Cells[0].Value = i + 1;
            }
        }

        /*
         * Buttons
         */

        private void btn_Create_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void btn_Fix_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void btn_Extract_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        //Create MDS from chosen GMO or FBX
        private void btn_Extract_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.Filters.Add(new CommonFileDialogFilter("GMO Model", "*.gmo"));
            dialog.Filters.Add(new CommonFileDialogFilter("FBX Model", "*.fbx"));
            dialog.Filters.Add(new CommonFileDialogFilter("DAE Model", "*.dae"));
            dialog.Filters.Add(new CommonFileDialogFilter("SMD Model", "*.smd"));
            dialog.Filters.Add(new CommonFileDialogFilter("Atlus PAC", "*.pac,*.bin,*.amd"));
            dialog.Title = "Load Model...";
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                CreateMDS(dialog.FileName);
        }

        //Fix MDS for use with P4G
        private void btn_Fix_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.Filters.Add(new CommonFileDialogFilter("MDS File", "*.mds"));
            dialog.Title = "Load MDS File...";
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                FixMDS(dialog.FileName); //Rewrite MDS with working materials/drawparts
        }

        //Create GMO from chosen MDS
        private void btn_Create_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.Filters.Add(new CommonFileDialogFilter("MDS File", "*.mds"));
            dialog.Title = "Load MDS File...";
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                CreateGMO(dialog.FileName);
        }

        //Save animation set to MDS file
        private void btn_ExportAnim_Click(object sender, EventArgs e)
        {
            UpdateAnimationDataGridView();
            if (animations.Count > 0)
            {
                CommonSaveFileDialog dialog = new CommonSaveFileDialog();
                dialog.Filters.Add(new CommonFileDialogFilter("MDS File", "*.mds"));
                dialog.Title = "Save Animation Set...";
                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    string fileName = dialog.FileName;
                    if (!fileName.ToLower().EndsWith(".mds"))
                        fileName = fileName + ".mds";

                    if (File.Exists(fileName))
                        File.Delete(fileName);
                    File.AppendAllLines(fileName, WriteAnimations());
                }
            }
        }

        //Load animation set from MDS file
        private void btn_ImportAnim_Click(object sender, EventArgs e)
        {
            ChooseAnimationSet();
            CreateAnimationRows();
            UpdateAnimationDataGridView();
        }

        //Update listbox with new animation names if preset is selected
        private void comboBox_Preset_SelectedIndexChanged(object sender, EventArgs e)
        {
            presetChanged = true;
            UpdateAnimationDataGridView();
            presetChanged = false;
        }
        //Add, remove, or rename selected animation
        private void menuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Text == "Add")
            {
                //TODO: Add single animation importing
            }
            else if (e.ClickedItem.Text == "Remove")
            {
                //TODO: Add removal
            }
            else if (e.ClickedItem.Text == "Rename")
            {
                //TODO: Add renaming
            }
        }

        //Enable/disable import button depending on if animations will be read
        private void chkBox_Animations_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBox_Animations.Checked)
                btn_ImportAnim.Enabled = true;
            else
                btn_ImportAnim.Enabled = false;
        }

        //Show the context menu if an animation cell is right clicked
        private void dataGridView_AnimationOrder_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            // Ignore if a column or row header is clicked
            if (e.RowIndex != -1 && e.ColumnIndex != -1)
            {
                if (e.Button == MouseButtons.Right)
                {
                    DataGridViewCell clickedCell = (sender as DataGridView).Rows[e.RowIndex].Cells[e.ColumnIndex];

                    // Here you can do whatever you want with the cell
                    dataGridView_AnimationOrder.CurrentCell = clickedCell;  // Select the clicked cell, for instance

                    // Get mouse position relative to the vehicles grid
                    var relativeMousePosition = dataGridView_AnimationOrder.PointToClient(Cursor.Position);

                    // Show the context menu
                    this.contextMenuStrip_Animations.Show(dataGridView_AnimationOrder, relativeMousePosition);

                    //Set the row number
                    selectedRow = e.RowIndex;
                }
            }
        }

        //Add animations from MDS or model
        private void toolStripMenuItem_Add_Click(object sender, EventArgs e)
        {
            var oldAnimations = animations;
            ChooseAnimationSet();
            foreach (var animation in animations)
                oldAnimations.Add(animation);
            animations = oldAnimations;
            CreateAnimationRows();
            UpdateAnimationDataGridView();
        }

        //Delete selected animation
        private void toolStripMenuItem_Remove_Click(object sender, EventArgs e)
        {
            animations.RemoveAt(selectedRow);
            CreateAnimationRows();
            UpdateAnimationDataGridView();
        }

        //Rename selected animation
        private void toolStripTextBox_Rename_TextChanged(object sender, EventArgs e)
        {
            animations[selectedRow].Name = toolStripTextBox_Rename.Text;
            toolStripTextBox_Rename.Text = "";
            CreateAnimationRows();
            UpdateAnimationDataGridView();
        }

        private void chkBox_ViewGMO_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBox_ViewGMO.Checked)
                comboBox_Preview.Enabled = true;
            else
                comboBox_Preview.Enabled = false;
        }

        private void chkBox_FBXOptimize_CheckedChanged(object sender, EventArgs e)
        {
            if (!chkBox_FBXOptimize.Checked)
            {
                chkBox_fbxoldexport.Enabled = false;
                chkBox_fbxascii.Enabled = false;
                txtBox_NoesisOptions.Enabled = false;
            }
            else
            {
                chkBox_fbxoldexport.Enabled = true;
                chkBox_fbxascii.Enabled = true;
                txtBox_NoesisOptions.Enabled = true;
            }
        }

        private void chkBox_Dummy_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBox_Dummy.Checked)
                chkBox_AutoConvertTex.Checked = false;
        }

        /*
         * UTILITIES
         */
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

        //Extract TM2 textures from GMO
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

        //Read animations from input MDS or model
        private void ChooseAnimationSet()
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.Filters.Add(new CommonFileDialogFilter("MDS File", "*.mds"));
            dialog.Filters.Add(new CommonFileDialogFilter("FBX Animated Model", "*.fbx"));
            dialog.Filters.Add(new CommonFileDialogFilter("DAE Animated Model", "*.dae"));
            dialog.Filters.Add(new CommonFileDialogFilter("SMD Animated Model", "*.smd"));
            dialog.Title = "Load Animation Set...";
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                animations = new List<Animation>();
                dataGridView_AnimationOrder.Rows.Clear(); //Remove old animation set
                string mdsPath = dialog.FileName;
                string ext = Path.GetExtension(mdsPath).ToUpper();
                if (ext != ".MDS")
                {
                    NoesisOptimize(mdsPath, "-fbxoldexport"); //Create optimized FBX
                    string extensionless = Path.Combine(Path.GetDirectoryName(mdsPath), Path.GetFileNameWithoutExtension(mdsPath));
                    if (chkBox_FBXtoGMO.Checked)
                    {
                        GMOConv(extensionless + "_noesis.fbx"); //Convert FBX directly to GMO
                        GMOTool(extensionless + "_noesis.gmo", true); //Create MDS
                    }
                    else
                        GMOTool(extensionless + "_noesis.fbx", true); //Create MDS
                    mdsPath = extensionless + "_noesis.mds";
                }
                ReadMDS(mdsPath); //Read MDS with animations
            }
        }

        /*
         * COMMANDLINE TOOLS
         */

        //Run tool to convert between GMO and MDS
        private void GMOTool(string path, bool extract)
        {
            Process cmd = new Process();
            cmd.StartInfo.FileName = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\Tools\\GMOTool\\GMOTool.exe";
            //cmd.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            cmd.StartInfo.Arguments = $"\"{path}\"";
            if (extract)
                cmd.StartInfo.Arguments += " -E";
            else
                cmd.StartInfo.Arguments += " -PSV";
            cmd.Start();
            cmd.WaitForExit();
        }

        //Run program to convert FBX to GMO directly
        private void GMOConv(string fbx)
        {
            Process cmd = new Process();
            cmd.StartInfo.FileName = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\Tools\\GMO\\GmoConv.exe";
            //cmd.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            cmd.StartInfo.Arguments = $"\"{fbx}\"";
            cmd.Start();
            int x = 0;
            while (!File.Exists($"{Path.Combine(Path.GetDirectoryName(fbx), Path.GetFileNameWithoutExtension(fbx))}.gmo")) { Thread.Sleep(1000); x++; if (x == 15) return; }
            cmd.WaitForExit();
        }

        //Run program to convert referenced textures to TM2
        private void GIMConv(string texture)
        {
            Process cmd = new Process();
            cmd.StartInfo.FileName = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\Tools\\GIM\\GimConv.exe";
            //cmd.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            cmd.StartInfo.Arguments = $"\"{texture}\" -o \"{texture}.tm2\"";
            cmd.Start();
            int x = 0;
            while (!File.Exists($"{texture}.tm2")) { Thread.Sleep(1000); x++; if (x == 15) return; }
            cmd.WaitForExit();
        }

        //Run program to view newly generated GMO file
        private void GMOView(string model)
        {
            Process cmd = new Process();
            cmd.StartInfo.FileName = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\Tools\\GMO\\GmoView.exe";
            //cmd.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            cmd.StartInfo.Arguments = $"\"{model}\"";
            cmd.Start();
        }

        //Run model through Noesis for viewing
        private void Noesis(string args)
        {
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.FileName = $"\"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\Tools\\Noesis\\Noesis.exe\"";
            startInfo.Arguments = args;
            process.StartInfo = startInfo;
            process.Start();
        }

        //Run model through Noesis for optimizing
        private void NoesisOptimize(string path, string args)
        {
            using (var cmdProcess = new Process())
            {
                cmdProcess.StartInfo.UseShellExecute = true;
                cmdProcess.StartInfo.WorkingDirectory = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\Tools\\Noesis";
                cmdProcess.StartInfo.FileName = @"C:\Windows\System32\cmd.exe";
                cmdProcess.StartInfo.Arguments = "/k " + $"Noesis.exe ?cmode \"{path}\" \"{extensionlessPath}_noesis.fbx\" {args}";
                cmdProcess.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                cmdProcess.Start();
                int x = 0;
                while (!File.Exists($"{extensionlessPath}_noesis.fbx")) { Thread.Sleep(1000); x++; if (x == 15) return; }
                cmdProcess.Kill();
            }
        }

        //Run TGE's tool to fix GMO files for PC
        private void GMOFixTool(string path)
        {
            Process cmd = new Process();
            cmd.StartInfo.FileName = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\Tools\\p4gpc-gmofix\\p4gpc-gmofix.exe";
            //cmd.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            cmd.StartInfo.Arguments = $"\"{path}\"";
            cmd.Start();
            cmd.WaitForExit();
        }
    }
}
