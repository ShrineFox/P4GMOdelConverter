using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace P4GModelConverter
{
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
        public static void Export()
        {

        }
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

    public class Model
    {
        public string Name { get; set; } = "Model";
        public string BoundingBox { get; set; } = "";
        public string BlindData { get; set; } = "";
        public List<Bone> Bones { get; set; } = new List<Bone>();
        public List<Part> Parts { get; set; } = new List<Part>();
        public List<Tuple<string, string>> BoneDrawPartPairs { get; set; } = new List<Tuple<string, string>>();
        public List<Material> Materials { get; set; } = new List<Material>();
        public List<Texture> Textures { get; set; } = new List<Texture>();
        public List<Animation> Animations { get; set; } = new List<Animation>();
        public string Path { get; set; } = "";
        public static Model Deserialize(Model model, string[] lines, SettingsForm.Settings settings)
        {
            //Organize lines into objects
            for (int i = 0; i < lines.Count(); i++)
            {
                if (lines[i].StartsWith("BlindData"))
                    model.BlindData = lines[i].Replace("BlindData ", "");
                else if (lines[i].StartsWith("\tBone"))
                {
                    int x = i;
                    //Add bone data to bone list
                    Bone bone = new Bone();
                    bone.Name = lines[x].Replace("\tBone \"", "").Replace("\" {", "");
                    if (settings.RenameBones)
                        bone.Name = SanitizeBoneName(bone.Name);
                    x++;
                    while (!lines[x].StartsWith("\t}"))
                    {
                        if (lines[x].StartsWith("\t\tBoundingBox"))
                            bone.BoundingBox = lines[x].Replace("\t","");
                        if (lines[x].StartsWith("\t\tTranslate"))
                            bone.Translate = lines[x].Replace("\t", "");
                        if (lines[x].StartsWith("\t\tRotate"))
                            bone.Rotate = lines[x].Replace("\t", "");
                        if (lines[x].StartsWith("\t\tParentBone"))
                        {
                            if (settings.RenameBones)
                                bone.ParentBone = SanitizeBoneName(lines[x]).Replace("\t", "");
                            else
                                bone.ParentBone = lines[x].Replace("\t", "");
                        }
                        if (lines[x].StartsWith("\t\tScale"))
                            bone.Scale = lines[x].Replace("\t", "");
                        if (lines[x].StartsWith("\t\tBlindData"))
                            bone.BlindData = lines[x].Replace("\t", "");
                        if (lines[x].StartsWith("\t\tBlendBones"))
                        {
                            if (settings.RenameBones)
                                bone.BlendBones = SanitizeBoneName(lines[x]).Replace("\t", "");
                            else
                                bone.BlendBones = lines[x].Replace("\t", "");
                        }
                        if (lines[x].StartsWith("\t\tDrawPart"))
                            bone.DrawParts.Add(lines[x].Replace("\t", ""));
                        if (lines[x].StartsWith("\t\tBlendOffsets"))
                        {
                            string blendOffsets = lines[x].Replace("\t", "");
                            x++;
                            while (lines[x].StartsWith("\t\t\t"))
                            {
                                blendOffsets += "\n" + lines[x].Replace("\t", "");
                                x++;
                            }
                            bone.BlendOffsets = blendOffsets;
                        }
                        else
                            x++;
                    }
                    model.Bones.Add(bone);
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
                            string mesh = lines[x].Replace("\t", "");
                            x++;
                            while (!lines[x].Contains("}"))
                            {
                                mesh += "\n" + lines[x].Replace("\t", "");
                                x++;
                            }
                            mesh += "\n\t\t}";
                            part.Meshes.Add(mesh);
                        }
                        else if (lines[x].StartsWith("\t\tArrays"))
                        {
                            string array = lines[x].Replace("\t", "");
                            x++;
                            while (!lines[x].Contains("}"))
                            {
                                array += "\n" + lines[x].Replace("\t", "");
                                x++;
                            }
                            array += "\n\t\t}";
                            part.Arrays.Add(array);
                        }
                        else if (lines[x].StartsWith("\t\tBoundingBox"))
                        {
                            part.BoundingBox = lines[x].Replace("\t", "");
                            x++;
                        }
                        else
                            x++;
                    }
                    model.Parts.Add(part);
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
                            mat.Diffuse = lines[x].Replace("\t", "");
                        if (lines[x].StartsWith("\t\tAmbient"))
                            mat.Ambient = lines[x].Replace("\t", "");
                        if (lines[x].StartsWith("\t\tReflection"))
                            mat.Reflection = lines[x].Replace("\t", "");
                        if (lines[x].StartsWith("\t\tRefraction"))
                            mat.Refraction = lines[x].Replace("\t", "");
                        if (lines[x].StartsWith("\t\tBump"))
                            mat.Bump = lines[x].Replace("\t", "");
                        if (lines[x].StartsWith("\t\tBlindData"))
                            mat.BlindData = lines[x].Replace("\t", "");
                        if (lines[x].StartsWith("\t\t\tSetTexture"))
                            mat.SetTexture = lines[x].Replace("\t", "");
                        if (lines[x].StartsWith("\t\t\tBlendFunc"))
                            mat.BlendFunc = lines[x].Replace("\t", "");
                        x++;
                    }
                    model.Materials.Add(mat);
                }
                else if (lines[i].StartsWith("\tTexture"))
                {
                    var tex = new Texture();
                    tex.Name = lines[i].Replace("\tTexture \"", "").Replace("\" {", "");
                    tex.FileName = lines[i + 1].Replace("\t\tFileName \"", "").Replace("\"", "");
                    model.Textures.Add(tex);
                }
                else if (lines[i].StartsWith("\tMotion") && settings.LoadAnimations)
                {
                    var anim = new Animation();
                    var animateLines = new List<string>();
                    var fcurves = new List<List<string>>();
                    anim.Name = lines[i].Replace("\tMotion \"", "").Replace("\" {", "");

                    int x = i + 1;
                    while (x < lines.Count() && lines[x] != "\t}")
                    {
                        if (lines[x].StartsWith("\t\tFrameLoop"))
                            anim.FrameLoop = lines[x].Replace("\t", "");
                        if (lines[x].StartsWith("\t\tFrameRate"))
                            anim.FrameRate = lines[x].Replace("\t", "");
                        if (lines[x].StartsWith("\t\tAnimate"))
                            animateLines.Add(lines[x].Replace("\t", ""));
                        if (lines[x].StartsWith("\t\tFCurve"))
                        {
                            var fcurveLines = new List<string>();
                            for (int w = x; w < lines.Count(); w++)
                            {
                                fcurveLines.Add(lines[w].Replace("\t", ""));
                                if (lines[w].StartsWith("\t\t}"))
                                    break;
                            }
                            fcurves.Add(fcurveLines);
                        }
                        x++;
                    }
                    anim.Animate = animateLines;
                    anim.FCurve = fcurves;
                    model.Animations.Add(anim);
                }
            }
            //Optimize data
            model = MatchBonesAndDrawParts(model);
            model = RewriteBones(model, settings);
            model = RewriteParts(model, settings);
            if (!settings.UseDummyMaterials) 
            {
                model = RewriteMaterials(model);
                model = RewriteTextures(model, settings);
            }
            else
            {
                List<Material> dummyMaterials = new List<Material>();
                Material dummy = new Material();
                dummy.Name = "Dummy";
                dummy.Ambient = "1.000000 1.000000 1.000000 1.000000";
                dummy.BlindData = "\"transAlgo\" 4";
                dummy.BlendFunc = "ADD SRC_ALPHA INV_SRC_ALPH";
                dummy.Bump = "0.000000";
                dummy.Diffuse = "1.000000 1.000000 1.000000 1.000000";
                dummy.Reflection = "0.000000";
                dummy.Refraction = "1.000000";
                dummyMaterials.Add(dummy);
                model.Materials = dummyMaterials;
            }
            return model;
        }
        private static string SanitizeBoneName(string name)
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
        private static Model MatchBonesAndDrawParts(Model model)
        {
            //Add part and corresponding bone to list
            for (int w = 0; w < model.Parts.Count; w++)
            {
                for (int z = 0; z < model.Parts[w].Meshes.Count; z++)
                {
                    string splitName = $"{model.Parts[w].Name}_{z}";
                    string firstBone = model.Bones.First(b => b.DrawParts.Any(a => a.Contains(model.Parts[w].Name))).Name;
                    Tuple<string, string> boneDrawPartPair = new Tuple<string, string>(firstBone, splitName);
                    model.BoneDrawPartPairs.Add(boneDrawPartPair);
                }
            }
            return model;
        }
        private static Model RewriteBones(Model model, SettingsForm.Settings settings)
        {
            //Add BlindData from settings & match drawParts to bones
            for (int w = 0; w < model.Bones.Count; w++)
            {
                if (model.Bones[w].Name == settings.WeaponBoneName)
                    model.Bones[w].BlindData = "\t\tBlindData \"per3Helper\" 500 1092542719 1085565955 1065731131 -1082972548 1048875842 -1104161467 1024833421 0 0";
                foreach (var drawPartPair in model.BoneDrawPartPairs.Where(p => p.Item1.Equals(model.Bones[w].Name)))
                    model.Bones[w].DrawParts.Add(drawPartPair.Item2);
            }
            return model;
        }

        private static Model RewriteParts(Model model, SettingsForm.Settings settings)
        {
            //Format text so that there's only one mesh/array pair per "part"
            List<Part> newParts = new List<Part>();
            for (int w = 0; w < model.Parts.Count; w++)
            {
                for (int z = 0; z < model.Parts[w].Meshes.Count; z++)
                {
                    Part newPart = new Part();
                    newPart.Name = $"{model.Parts[w].Name}_{z}";
                    newPart.BoundingBox = model.Parts[w].BoundingBox;
                    if (!settings.UseDummyMaterials)
                        newPart.Meshes.Add(model.Parts[w].Meshes[z]);
                    else
                        newPart.Meshes.Add(Regex.Replace(model.Parts[w].Meshes[z], "SetMaterial \".*\"", "SetMaterial \"Dummy\""));
                    newPart.Arrays.Add(model.Parts[w].Arrays[z]);
                    newParts.Add(newPart);
                }
            }
            model.Parts = newParts; //Replace old parts list with new one
            return model;
        }
        private static Model RewriteMaterials(Model model)
        {
            //Format materials so that they all use one layer and map/blend type
            List<Material> newMaterials = new List<Material>();
            for (int w = 0; w < model.Materials.Count(); w++)
            {
                Material newMaterial = new Material();
                newMaterial.Name = model.Materials[w].Name;
                //BlendFunc
                if (model.Materials[w].BlendFunc != null && model.Materials[w].BlendFunc.Contains("ADD SRC_ALPHA ONE"))
                    newMaterial.BlendFunc = "\"transAlgo\" 1";
                else
                    newMaterial.BlendFunc = "\"transAlgo\" 4";
                //Diffuse
                if (model.Materials[w].Diffuse != null)
                    newMaterial.Diffuse = "0.800000 0.800000 0.800000 1.000000";
                else
                    newMaterial.Diffuse = model.Materials[w].Diffuse;
                //Ambient
                if (model.Materials[w].Ambient != null)
                    newMaterial.Ambient = "0.800000 0.800000 0.800000 1.000000";
                else
                    newMaterial.Ambient = model.Materials[w].Ambient;
                //Reflection
                if (model.Materials[w].Reflection != null)
                    newMaterial.Reflection = "0.000000";
                else
                    newMaterial.Reflection = model.Materials[w].Reflection;
                //Refraction
                if (model.Materials[w].Refraction != null)
                    newMaterial.Refraction = "1.000000";
                else
                    newMaterial.Refraction = model.Materials[w].Refraction;
                //Bump
                if (model.Materials[w].Bump != null)
                    newMaterial.Bump = "0.000000";
                else
                    newMaterial.Bump = model.Materials[w].Bump;
                //BlendFunc
                if (model.Materials[w].Ambient != null)
                    newMaterial.Ambient = "0.800000 0.800000 0.800000 1.000000";
                else
                    newMaterial.Ambient = model.Materials[w].Ambient;
                newMaterials.Add(newMaterial);
            }
            model.Materials = newMaterials; //Replace old mats list with new one
            return model;
        }

        private static Model RewriteTextures(Model model, SettingsForm.Settings settings)
        {
            //Rename and convert textures depending on settings
            foreach (var texture in model.Textures)
            {
                //fix for DAE texture paths
                if (texture.FileName.StartsWith("\\\\"))
                    texture.FileName = texture.FileName.Replace("\\\\", "").Insert(1, ":");
                //Set up texture output path
                string newTexPath = System.IO.Path.Combine(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(model.Path),"textures"), new DirectoryInfo(texture.FileName).Name);
                Directory.CreateDirectory(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(model.Path), "textures"));

                //Convert, move and edit path of non-tm2 textures
                if (!texture.FileName.ToLower().Contains(".tm2") && settings.AutoConvertTex)
                {
                    if (File.Exists(texture.FileName))
                        Tools.GIMConv(texture.FileName);
                    if (!File.Exists(newTexPath + ".tm2") && File.Exists(texture.FileName))
                        File.Move(texture.FileName + ".tm2", newTexPath + ".tm2");
                    texture.FileName = System.IO.Path.Combine("textures", new DirectoryInfo(texture.FileName).Name + ".tm2");
                }
                else //Move and edit path of tm2 textures
                {
                    if (!File.Exists(newTexPath))
                        if (File.Exists(texture.FileName))
                            File.Move(texture.FileName, newTexPath);
                    texture.FileName = System.IO.Path.Combine("textures", texture.FileName.Split('\\').Last());
                }
            }
            return model;
        }
    }
}
