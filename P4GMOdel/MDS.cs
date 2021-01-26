using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace P4GMOdel
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
        public Part(string name, string boundingBox, List<Mesh> meshes, List<string> arrays)
        {
            Name = name;
            BoundingBox = boundingBox;
            Meshes = meshes;
            Arrays = arrays;
        }
        public string Name { get; set; }
        public string BoundingBox { get; set; }
        public List<Mesh> Meshes { get; set; } = new List<Mesh>();
        public List<string> Arrays { get; set; } = new List<string>();
    }
    public class Mesh
    {
        public Mesh() { }
        public Mesh(string name, string setMaterial, string blendSubset, string drawArrays)
        {
            Name = name;
            SetMaterial = setMaterial;
            BlendSubset = blendSubset;
            DrawArrays = drawArrays;
        }
        public string Name { get; set; }
        public string SetMaterial { get; set; }
        public string BlendSubset { get; set; }
        public string DrawArrays { get; set; }
    }

    public class Material
    {
        public Material() { }
        public Material(string name, string renderState, string diffuse, string ambient, string reflection, string refraction, string bump, string blindData, List<Layer> layers)
        {
            RenderState = renderState;
            Name = name;
            Diffuse = diffuse;
            Ambient = ambient;
            Reflection = reflection;
            Refraction = refraction;
            Bump = bump;
            BlindData = blindData;
            Layers = layers;
        }
        public string Name { get; set; }
        public string RenderState { get; set; }
        public string Diffuse { get; set; }
        public string Ambient { get; set; }
        public string Reflection { get; set; }
        public string Refraction { get; set; }
        public string Bump { get; set; }
        public string BlindData { get; set; }
        public List<Layer> Layers { get; set; }
    }

    public class Layer
    {
        public Layer() { }
        public Layer(string name, string setTexture, string mapType, string mapFactor, string blendFunc, string texWrap, string texGen, string texMatrix)
        {
            Name = name;
            SetTexture = setTexture;
            MapType = mapType;
            MapFactor = mapFactor;
            BlendFunc = blendFunc;
            TexWrap = texWrap;
            TexGen = texGen;
            TexMatrix = texMatrix;
        }
        public string Name { get; set; }
        public string SetTexture { get; set; }
        public string MapType { get; set; }
        public string MapFactor { get; set; }
        public string BlendFunc { get; set; }
        public string TexWrap { get; set; }
        public string TexGen { get; set; }
        public string TexMatrix { get; set; }
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
                            bone.BoundingBox = lines[x].Replace("\t\tBoundingBox ", "");
                        if (lines[x].StartsWith("\t\tTranslate"))
                            bone.Translate = lines[x].Replace("\t\tTranslate ", "");
                        if (lines[x].StartsWith("\t\tRotate"))
                            bone.Rotate = lines[x].Replace("\t\tRotateZYX ", "");
                        if (lines[x].StartsWith("\t\tParentBone"))
                        {
                            if (settings.RenameBones)
                                bone.ParentBone = SanitizeBoneName(lines[x]).Replace("\t", "").Replace("\"", "");
                            else
                                bone.ParentBone = lines[x].Replace("\t\tParentBone ", "").Replace("\"", "").Trim();
                        }
                        if (lines[x].StartsWith("\t\tScale"))
                            bone.Scale = lines[x].Replace("\t\tScale ", "");
                        if (lines[x].StartsWith("\t\tBlindData"))
                            bone.BlindData = lines[x].Replace("\t\tBlindData ", "");
                        if (lines[x].StartsWith("\t\tBlendBones"))
                        {
                            if (settings.RenameBones)
                                bone.BlendBones = SanitizeBoneName(lines[x]).Replace("\t", "");
                            else
                                bone.BlendBones = lines[x].Replace("\t\tBlendBones ", "");
                        }
                        if (lines[x].StartsWith("\t\tDrawPart"))
                            bone.DrawParts.Add(lines[x].Replace("\t\tDrawPart \"", "").Replace("\"", "").Trim());
                        if (lines[x].StartsWith("\t\tBlendOffsets"))
                        {
                            string blendOffsets = lines[x].Replace("\t\tBlendOffsets ", "");
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
                    part.Name = lines[x].Replace("\tPart \"", "").Replace("{", "").Replace("\"", "").Trim();
                    x++;
                    while (!lines[x].StartsWith("\t}"))
                    {
                        if (lines[x].Contains("\t\tMesh"))
                        {
                            Mesh mesh = new Mesh();
                            mesh.Name = lines[x].Replace("\t\tMesh ", "").Replace("\"", "").Replace("{", "").Trim();
                            x++;
                            while (!lines[x].Contains("}"))
                            {
                                if (lines[x].StartsWith("\t\tSetMaterial"))
                                    mesh.SetMaterial = lines[x].Replace("\t\tSetMaterial ", "").Replace("\"", "");
                                if (lines[x].StartsWith("\t\tBlendSubset"))
                                    mesh.BlendSubset = lines[x].Replace("\t\tBlendSubset ", "");
                                if (lines[x].StartsWith("\t\tDrawArrays"))
                                    mesh.DrawArrays = lines[x].Replace("\t\tDrawArrays ", "");
                                x++;
                            }
                            part.Meshes.Add(mesh);
                        }
                        else if (lines[x].StartsWith("\t\tArrays"))
                        {
                            string array = lines[x].Replace("\t\tArrays ", "");
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
                            part.BoundingBox = lines[x].Replace("\t\tBoundingBox ", "");
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
                    mat.Name = lines[x].Replace("\tMaterial", "").Replace("{", "").Replace("\"","").Trim();
                    x++;
                    List<Layer> layers = new List<Layer>();
                    while (!lines[x].StartsWith("\t}"))
                    {
                        if (lines[x].StartsWith("\t\tRenderState"))
                            mat.RenderState = lines[x].Replace("\t\tRenderState ", "");
                        if (lines[x].StartsWith("\t\tDiffuse"))
                            mat.Diffuse = lines[x].Replace("\t\tDiffuse ", "");
                        if (lines[x].StartsWith("\t\tAmbient"))
                            mat.Ambient = lines[x].Replace("\t\tAmbient ", "");
                        if (lines[x].StartsWith("\t\tReflection"))
                            mat.Reflection = lines[x].Replace("\t\tReflection ", "");
                        if (lines[x].StartsWith("\t\tRefraction"))
                            mat.Refraction = lines[x].Replace("\t\tRefraction ", "");
                        if (lines[x].StartsWith("\t\tBump"))
                            mat.Bump = lines[x].Replace("\t", "");
                        if (lines[x].StartsWith("\t\tBlindData"))
                            mat.BlindData = lines[x].Replace("\t\tBlindData ", "");
                        if (lines[x].StartsWith("\t\tLayer"))
                        {
                            mat.Layers = new List<Layer>();
                            Layer layer = new Layer();
                            layer.Name = lines[x].Replace("\t\tLayer ", "").Replace("\"","").Replace("{", "").Trim();
                            x++;
                            while (!lines[x].Contains("}"))
                            {
                                if (lines[x].StartsWith("\t\t\tSetTexture"))
                                    layer.SetTexture = lines[x].Replace("\t\t\tSetTexture ", "").Replace("\"", "");
                                if (lines[x].StartsWith("\t\t\tMapType"))
                                    layer.MapType = lines[x].Replace("\t\t\tMapType ", "");
                                if (lines[x].StartsWith("\t\t\tMapFactor"))
                                    layer.MapFactor = lines[x].Replace("\t\t\tMapFactor ", "");
                                if (lines[x].StartsWith("\t\t\tBlendFunc"))
                                    layer.BlendFunc = lines[x].Replace("\t\t\tBlendFunc ", "");
                                if (lines[x].StartsWith("\t\t\tTexWrap"))
                                    layer.TexWrap = lines[x].Replace("\t\t\tTexWrap ", "");
                                if (lines[x].StartsWith("\t\t\tTexGen"))
                                    layer.TexGen = lines[x].Replace("\t\t\tTexGen ", "");
                                if (lines[x].StartsWith("\t\t\tTexMatrix"))
                                {
                                    string texMatrix = lines[x].Replace("\t\t\tTexMatrix ", "");
                                    x++;
                                    while (lines[x].StartsWith("\t\t\t"))
                                    {
                                        texMatrix += "\n" + lines[x].Replace("\t", "");
                                        x++;
                                    }
                                    layer.TexMatrix = texMatrix;
                                }
                                x++;
                            }
                            mat.Layers.Add(layer);
                        }
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
                            anim.FrameLoop = lines[x].Replace("\t\tFrameLoop ", "");
                        if (lines[x].StartsWith("\t\tFrameRate"))
                            anim.FrameRate = lines[x].Replace("\t\tFrameRate ", "");
                        if (lines[x].StartsWith("\t\tAnimate"))
                            animateLines.Add(lines[x].Replace("\t\tAnimate ", ""));
                        if (lines[x].StartsWith("\t\tFCurve"))
                        {
                            var fcurveLines = new List<string>();
                            for (int w = x; w < lines.Count(); w++)
                            {
                                fcurveLines.Add(lines[w].Replace("\t\tFCurve ", ""));
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
                //model = RewriteMaterials(model);
                model = RewriteTextures(model, settings);
            }
            else
            {
                List<Material> dummyMaterials = new List<Material>();
                Material dummy = new Material();
                dummy.Name = "Dummy";
                dummy.Ambient = "1.000000 1.000000 1.000000 1.000000";
                dummy.BlindData = "\"transAlgo\" 4";
                dummy.Bump = "0.000000";
                dummy.Diffuse = "1.000000 1.000000 1.000000 1.000000";
                dummy.Reflection = "0.000000";
                dummy.Refraction = "1.000000";
                dummy.Layers.Add(new Layer { BlendFunc = "ADD SRC_ALPHA INV_SRC_ALPH" });
                dummyMaterials.Add(dummy);
                model.Materials = dummyMaterials;
            }
            return model;
        }
        public static string Serialize(Model model, SettingsForm.Settings settings)
        {
            List<string> lines = new List<string>();
            lines.Add(".MDS 0.95\n"); //Header
            if (!string.IsNullOrEmpty(model.BlindData))
                lines.Add($"BlindData {model.BlindData}\n"); //BlindData
            lines.Add($"Model \"{model.Name}\" {{");
                if (!string.IsNullOrEmpty(model.BoundingBox))
                    lines.Add($"\tBoundingBox {model.BoundingBox}"); //BoundingBox
                foreach (var bone in model.Bones)
                {
                    lines.Add($"\tBone \"{bone.Name}\" {{"); //Bone
                        if (!string.IsNullOrEmpty(bone.ParentBone))
                            lines.Add($"\t\tParentBone \"{bone.ParentBone}\""); //ParentBone
                        if (!string.IsNullOrEmpty(bone.BlendBones))
                            lines.Add($"\t\tBlendBones {bone.BlendBones}"); //BlendBones
                        if (!string.IsNullOrEmpty(bone.BlendOffsets))
                            lines.Add($"\t\tBlendOffsets {bone.BlendOffsets}"); //BlendOffsets
                        if (!string.IsNullOrEmpty(bone.Translate))
                            lines.Add($"\t\tTranslate {bone.Translate}"); //Translate
                        if (!string.IsNullOrEmpty(bone.Rotate))
                            lines.Add($"\t\tRotateZYX {bone.Rotate}"); //RotateZYX
                        if (!string.IsNullOrEmpty(bone.Scale))
                            lines.Add($"\t\tScale {bone.Scale}"); //Scale
                        foreach (var drawPart in bone.DrawParts)
                        {
                            if (!string.IsNullOrEmpty(drawPart))
                                lines.Add($"\t\tDrawPart \"{drawPart}\""); //DrawPart
                        }
                        if (!string.IsNullOrEmpty(bone.BlindData))
                            lines.Add($"\t\tBlindData {bone.BlindData}"); //BlindData
                    lines.Add("\t}");
                }
                foreach (var part in model.Parts)
                {
                    lines.Add($"\tPart \"{part.Name}\" {{"); //Part
                        if (!string.IsNullOrEmpty(part.BoundingBox))
                            lines.Add($"\t\tBoundingBox {part.BoundingBox}"); //BoundingBox
                        foreach (var mesh in part.Meshes)
                        {
                            if (!string.IsNullOrEmpty(mesh.Name))
                                lines.Add($"\t\tMesh \"{mesh.Name}\" {{"); //Mesh
                                if (!string.IsNullOrEmpty(mesh.SetMaterial))
                                    lines.Add($"\t\t\tSetMaterial \"{mesh.SetMaterial}\""); //SetMaterial
                                if (!string.IsNullOrEmpty(mesh.BlendSubset))
                                    lines.Add($"\t\t\tBlendSubset {mesh.BlendSubset}"); //BlendSubset
                                if (!string.IsNullOrEmpty(mesh.DrawArrays))
                                    lines.Add($"\t\t\tDrawArrays {mesh.BlendSubset}"); //DrawArrays
                            lines.Add("\t\t}");
                        }
                        foreach (var arrays in part.Arrays)
                        {
                            if (!string.IsNullOrEmpty(arrays))
                                lines.Add($"\t\tArrays {arrays}"); //Arrays
                        }
                    lines.Add("\t}");
                }
                foreach (var material in model.Materials)
                {
                    lines.Add($"\tMaterial \"{material.Name}\" {{"); //Material
                        if (!string.IsNullOrEmpty(material.Diffuse))
                            lines.Add($"\t\tDiffuse {material.Diffuse}"); //Diffuse
                        if (!string.IsNullOrEmpty(material.Ambient))
                            lines.Add($"\t\tAmbient {material.Ambient}"); //Ambient
                        if (!string.IsNullOrEmpty(material.Reflection))
                            lines.Add($"\t\tReflection {material.Reflection}"); //Reflection
                        if (!string.IsNullOrEmpty(material.Refraction))
                            lines.Add($"\t\tRefraction {material.Refraction}"); //Refraction
                        if (!string.IsNullOrEmpty(material.Bump))
                            lines.Add($"\t\tBump {material.Bump}"); //Bump
                        if (!string.IsNullOrEmpty(material.BlindData))
                            lines.Add($"\t\tBlindData {material.BlindData}"); //BlindData
                        foreach (var layer in material.Layers)
                        {
                            lines.Add($"\t\tLayer \"{layer.Name}\" {{"); //Layer
                            if (!string.IsNullOrEmpty(layer.SetTexture))
                                lines.Add($"\t\t\tSetTexture {layer.SetTexture}"); //SetTexture
                            if (!string.IsNullOrEmpty(layer.MapType))
                                lines.Add($"\t\t\tMapType {layer.MapType}"); //MapType
                            if (!string.IsNullOrEmpty(layer.MapFactor))
                                lines.Add($"\t\t\tMapFactor {layer.MapFactor}"); //MapFactor
                            if (!string.IsNullOrEmpty(layer.BlendFunc))
                                lines.Add($"\t\t\tBlendFunc {layer.BlendFunc}"); //BlendFunc
                            if (!string.IsNullOrEmpty(layer.TexWrap))
                                lines.Add($"\t\t\tTexWrap {layer.TexWrap}"); //TexWrap
                            if (!string.IsNullOrEmpty(layer.TexGen))
                                lines.Add($"\t\t\tTexGen {layer.TexGen}"); //TexGen
                            if (!string.IsNullOrEmpty(layer.TexMatrix))
                                lines.Add($"\t\t\tTexMatrix {layer.TexMatrix}"); //TexMatrix
                            lines.Add("\t\t}");
                        }
                    lines.Add("\t}");
                }
                foreach (var texture in model.Textures)
                {
                    lines.Add($"\tTexture \"{texture.Name}\" {{"); //Texture
                    lines.Add($"\t\tFileName \"{texture.FileName}\""); //FileName
                    lines.Add("\t}");
                }
                foreach (var animation in model.Animations)
                {
                    lines.Add($"\tMotion \"{animation.Name}\" {{"); //Motion
                    lines.Add($"\t\tFrameLoop {animation.FrameLoop}"); //FrameLoop
                    lines.Add($"\t\tFrameRate {animation.FrameRate}"); //FrameRate
                    lines.Add($"\t\tAnimate {animation.Animate}"); //Animate
                    lines.Add("\t}");
                }
            lines.Add("}");
            return String.Join("\n", lines);
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
                    string firstBone = model.Bones.First(b => b.DrawParts.Any(a => a.Contains(model.Parts[w].Name.Trim()))).Name; //TODO: figure out where that space is coming from
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
                    if (settings.UseDummyMaterials)
                        model.Parts[w].Meshes[z].SetMaterial = "Dummy";
                    newPart.Meshes.Add(model.Parts[w].Meshes[z]);
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
                //BlendFunc
                if (model.Materials[w].Layers[0] != null && model.Materials[w].Layers[0].BlendFunc.Contains("ADD SRC_ALPHA ONE"))
                    model.Materials[w].Layers[0].BlendFunc = "\"transAlgo\" 1";
                else
                    model.Materials[w].Layers[0].BlendFunc = "\"transAlgo\" 4";
                //Diffuse
                if (model.Materials[w].Diffuse != null)
                    model.Materials[w].Diffuse = "0.800000 0.800000 0.800000 1.000000";
                //Ambient
                if (model.Materials[w].Ambient != null)
                    model.Materials[w].Ambient = "0.800000 0.800000 0.800000 1.000000";
                //Reflection
                if (model.Materials[w].Reflection != null)
                    model.Materials[w].Reflection = "0.000000";
                //Refraction
                if (model.Materials[w].Refraction != null)
                    model.Materials[w].Refraction = "1.000000";
                //Bump
                if (model.Materials[w].Bump != null)
                    model.Materials[w].Bump = "0.000000";
                //Ambient
                if (model.Materials[w].Ambient != null)
                    model.Materials[w].Ambient = "0.800000 0.800000 0.800000 1.000000";
            }
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
