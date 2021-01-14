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
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization;
using DarkUI.Controls;

namespace P4GModelConverter
{
    public partial class MainForm : Form
    {
        List<string> newLines = new List<string>();
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
        SettingsForm.Settings settings;
        Model model;

        public MainForm()
        {
            InitializeComponent();
            //Load settings
            if (File.Exists("settings.yml"))
            {
                var deserializer = new DeserializerBuilder().WithNamingConvention(UnderscoredNamingConvention.Instance).Build();
                settings = deserializer.Deserialize<SettingsForm.Settings>(File.ReadAllText("settings.yml"));
            }
            else
            {
                settings = new SettingsForm.Settings();
            }
        }

        private void Open_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.Filters.Add(new CommonFileDialogFilter("P4G Model", "*.mds, *.gmo, *.amd, *.pac, *.fbx, *.dae, *.smd"));
            dialog.Title = "Choose Model File...";
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                //Re-initialize variables and form
                model = new Model();
                model.Path = dialog.FileName;
                string extensionlessPath = Path.Combine(Path.GetDirectoryName(model.Path), Path.GetFileNameWithoutExtension(model.Path));
                darkTreeView_Main.Nodes.Clear();

                //Open file
                if (Path.GetExtension(model.Path.ToUpper()) == ".PAC")
                {
                    //Extract AMD or GMO
                    //Set path to amd or gmo path
                    model.Path = extensionlessPath + ".gmo";
                }
                if (Path.GetExtension(model.Path.ToUpper()) == ".AMD")
                {
                    //Extract GMO
                    //Set path to gmo path
                    model.Path = extensionlessPath + ".gmo";
                }
                if (Path.GetExtension(model.Path).ToUpper() == ".DAE" || Path.GetExtension(model.Path).ToUpper() == ".SMD" || Path.GetExtension(model.Path).ToUpper() == ".FBX")
                {
                    if (settings.ConvertToFBX || Path.GetExtension(model.Path).ToUpper() != ".FBX")
                    {
                        model.Path = Tools.CreateOptimizedFBX(model.Path, settings); //Optimize FBX with settings or convert to FBX
                    }
                    if (settings.ConvertToGMO)
                    {
                        Tools.GMOConv(model.Path); //Convert FBX directly to GMO without fixes first
                        model.Path = extensionlessPath + ".gmo";
                    }
                }
                if (Path.GetExtension(model.Path).ToUpper() == ".GMO" || Path.GetExtension(model.Path).ToUpper() == ".FBX")
                {
                    if (settings.ExtractTextures && Path.GetExtension(model.Path).ToUpper() == ".GMO")
                        Tools.ExtractTextures(model.Path); //Extract TM2 Textures
                    Tools.GMOTool(model.Path, true); //Create MDS
                    model.Path = extensionlessPath + ".mds";
                }
                if (Path.GetExtension(model.Path.ToLower()) == ".mds")
                {
                    var mdsLines = File.ReadAllLines(model.Path).ToList();
                    model = Model.Deserialize(model, mdsLines.ToArray(), settings);
                }
            }
            //Load treeview
            RefreshTreeview();
        }

        private void RefreshTreeview()
        {
            darkTreeView_Main.Nodes.Add(new DarkTreeNode() { Text = "Model" });
            darkTreeView_Main.Nodes.First().Nodes.Add(new DarkTreeNode() { Text = "BlindData" });
            darkTreeView_Main.Nodes.First().Nodes.Add(new DarkTreeNode() { Text = "Bones" });
            foreach (Bone bone in model.Bones)
            {
                darkTreeView_Main.Nodes.First().Nodes.First(x => x.Text == "Bones").Nodes.Add(new DarkTreeNode() { Text = bone.Name });
            }
            darkTreeView_Main.Nodes.First().Nodes.Add(new DarkTreeNode() { Text = "Parts" });
            foreach (Part part in model.Parts)
            {
                darkTreeView_Main.Nodes.First().Nodes.First(x => x.Text == "Parts").Nodes.Add(new DarkTreeNode() { Text = part.Name });
            }
            darkTreeView_Main.Nodes.First().Nodes.Add(new DarkTreeNode() { Text = "Materials" });
            foreach (Material material in model.Materials)
            {
                darkTreeView_Main.Nodes.First().Nodes.First(x => x.Text == "Materials").Nodes.Add(new DarkTreeNode() { Text = material.Name });
            }
            darkTreeView_Main.Nodes.First().Nodes.Add(new DarkTreeNode() { Text = "Animations" });
            foreach (Animation animation in model.Animations)
            {
                darkTreeView_Main.Nodes.First().Nodes.First(x => x.Text == "Animations").Nodes.Add(new DarkTreeNode() { Text = animation.Name });
            }
        }

        private void Settings_Click(object sender, EventArgs e)
        {
            using (var dialog = new SettingsForm())
            {
                if (dialog.ShowDialog() != DialogResult.OK)
                    return;
                settings = dialog.Result.ResultSettings;
                //Save new settings to file
                var serializer = new SerializerBuilder().WithNamingConvention(CamelCaseNamingConvention.Instance).Build();
                var yaml = serializer.Serialize(settings);
                File.WriteAllText("settings.yml", yaml);
            }
        }
    }
}
