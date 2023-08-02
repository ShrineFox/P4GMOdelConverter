using DarkUI.Forms;
using P4GMOdel.Properties;
using ShrineFox.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Configuration;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YamlDotNet.Serialization;

namespace P4GMOdel
{
    public partial class MainForm : DarkForm
    {
        public static Settings settings = new Settings();

        public class Settings
        {
            public bool OptimizeForVita { get; set; } = false;
            public bool FixForPC { get; set; } = true;
            public bool UseModelViewer { get; set; } = true;

            public void Save()
            {
                File.WriteAllText(".\\settings.yml", new SerializerBuilder().Build().Serialize(settings));
            }

            public void Load()
            {
                if (File.Exists(".\\settings.yml"))
                {
                    settings = new DeserializerBuilder().Build().Deserialize<Settings>(File.ReadAllText(".\\settings.yml"));
                }
            }
        }
    }
}
