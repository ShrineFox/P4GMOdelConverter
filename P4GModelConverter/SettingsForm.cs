using System;
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

namespace P4GModelConverter
{
    public partial class SettingsForm : Form
    {
        public ResultValue Result { get; private set; }
        public Settings settings;

        public SettingsForm()
        {
            InitializeComponent();
            //Load settings
            if (File.Exists("settings.yml"))
            {
                var deserializer = new DeserializerBuilder().WithNamingConvention(UnderscoredNamingConvention.Instance).Build();
                settings = deserializer.Deserialize<Settings>(File.ReadAllText("settings.yml"));
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
            public bool AutoConvertTex { get; set; } = true;
            public bool RenameBones { get; set; } = true;
            public bool UseDummyMaterials { get; set; } = false;
            public bool LoadAnimations { get; set; } = true;
            public string WeaponBoneName { get; set; } = "Bip01_L_Hand_Bone";
            // Output
            public bool FixForPC { get; set; } = true;
            public bool PreviewOutputGMO { get; set; } = false;
            public string PreviewWith { get; set; } = "GMOView";
        }


        private void Save_Click(object sender, EventArgs e)
        {
            Result = new ResultValue(this);
        }

        public class ResultValue
        {
            private readonly SettingsForm mParent;

            internal ResultValue(SettingsForm parent)
            {
                mParent = parent;
            }

            public Settings ResultSettings => new Settings {
                // Input
                ConvertToFBX = mParent.chkBox_ConvertToFBX.Checked,
                OldFBXExport = mParent.chkBox_OldFBXExport.Checked,
                AsciiFBX = mParent.chkBox_AsciiFBX.Checked,
                AdditionalFBXOptions = mParent.txtBox_AdditionalFBXOptions.Text,
                ConvertToGMO = mParent.chkBox_ConvertToGMO.Checked,
                ExtractTextures = mParent.chkBox_ExtractTextures.Checked,

                // Conversion
                AutoConvertTex = mParent.chkBox_AutoConvertTex.Checked,
                RenameBones = mParent.chkBox_RenameBones.Checked,
                UseDummyMaterials = mParent.chkBox_UseDummyMaterials.Checked,
                LoadAnimations = mParent.chkBox_LoadAnimations.Checked,
                WeaponBoneName = mParent.txt_WeaponBoneName.Text,

                // Output
                FixForPC = mParent.chkBox_FixForPC.Checked,
                PreviewOutputGMO = mParent.chkBox_PreviewOutputGMO.Checked,
                PreviewWith = mParent.comboBox_PreviewWith.SelectedItem.ToString(),
            };
        }
    }
}
