﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YamlDotNet;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace P4GMOdel
{
    public partial class SettingsForm : Form
    {
        public Settings settings;

        public SettingsForm()
        {
            InitializeComponent();
            //Load settings
            if (File.Exists("settings.yml"))
            {
                var deserializer = new DeserializerBuilder().WithNamingConvention(PascalCaseNamingConvention.Instance).Build();
                settings = deserializer.Deserialize<Settings>(File.ReadAllText("settings.yml"));
                chkBox_ConvertToFBX.Checked = settings.ConvertToFBX;
                chkBox_OldFBXExport.Checked = settings.OldFBXExport;
                chkBox_AsciiFBX.Checked = settings.AsciiFBX;
                txtBox_AdditionalFBXOptions.Text = settings.AdditionalFBXOptions;
                chkBox_ConvertToGMO.Checked = settings.ConvertToGMO;
                chkBox_ExtractTextures.Checked = settings.ExtractTextures;
                chkBox_OptimizeForVita.Checked = settings.OptimizeForVita;
                chkBox_RenameBones.Checked = settings.RenameBones;
                chkBox_UseDummyMaterials.Checked = settings.UseDummyMaterials;
                chkBox_LoadAnimations.Checked = settings.LoadAnimations;
                txt_WeaponBoneName.Text = settings.WeaponBoneName;
                chkBox_FixForPC.Checked = settings.FixForPC;
                chkBox_ShowConsoleWindows.Checked = settings.ShowConsoleWindows;
            }
            else
            {
                settings = new Settings();
            }
        }

        public class Settings
        {
            // Input
            public bool ConvertToFBX { get; set; } = false;
            public bool OldFBXExport { get; set; } = false;
            public bool AsciiFBX { get; set; } = false;
            public string AdditionalFBXOptions { get; set; } = "";
            public bool ConvertToGMO { get; set; } = false;
            public bool ExtractTextures { get; set; } = true;
            // Conversion
            public bool OptimizeForVita { get; set; } = true;
            public bool RenameBones { get; set; } = true;
            public bool UseDummyMaterials { get; set; } = false;
            public bool LoadAnimations { get; set; } = true;
            public string WeaponBoneName { get; set; } = "Bip01_L_Hand_Bone";
            // Output
            public bool FixForPC { get; set; } = true;
            public bool ShowConsoleWindows { get; set; } = false;
        }


        private void Save_Click(object sender, EventArgs e)
        {
            settings.ConvertToFBX = chkBox_ConvertToFBX.Checked;
            settings.OldFBXExport = chkBox_OldFBXExport.Checked;
            settings.AsciiFBX = chkBox_AsciiFBX.Checked;
            settings.AdditionalFBXOptions = txtBox_AdditionalFBXOptions.Text;
            settings.ConvertToGMO = chkBox_ConvertToGMO.Checked;
            settings.ExtractTextures = chkBox_ExtractTextures.Checked;

            settings.OptimizeForVita = chkBox_OptimizeForVita.Checked;
            settings.RenameBones = chkBox_RenameBones.Checked;
            settings.UseDummyMaterials = chkBox_UseDummyMaterials.Checked;
            settings.LoadAnimations = chkBox_LoadAnimations.Checked;
            settings.WeaponBoneName = txt_WeaponBoneName.Text;

            settings.FixForPC = chkBox_FixForPC.Checked;
            settings.ShowConsoleWindows = chkBox_ShowConsoleWindows.Checked;

            var serializer = new SerializerBuilder().WithNamingConvention(PascalCaseNamingConvention.Instance).Build();
            var yaml = serializer.Serialize(settings);
            File.WriteAllText("settings.yml", yaml);
        }

        private void ConvertToFBX_Checked(object sender, EventArgs e)
        {
            if (chkBox_ConvertToFBX.Checked)
            {
                chkBox_OldFBXExport.Enabled = true;
                chkBox_AsciiFBX.Enabled = true;
                txtBox_AdditionalFBXOptions.Enabled = true;
            }
            else 
            {
                chkBox_OldFBXExport.Enabled = false;
                chkBox_AsciiFBX.Enabled = false;
                txtBox_AdditionalFBXOptions.Enabled = false;
            }
        }
    }
}
