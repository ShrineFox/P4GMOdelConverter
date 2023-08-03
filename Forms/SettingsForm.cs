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
using static P4GMOdel.MainForm;

namespace P4GMOdel
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
            chk_FixForPC.Checked = settings.FixForPC;
            chk_OptimizeForVita.Checked = settings.OptimizeForVita;
            chk_OptimizeFbxWithNoesis.Checked = settings.OptimizeFbxWithNoesis;
            txt_NoesisArgs.Text = settings.NoesisArgs;
            chk_UseModelViewer.Checked = settings.UseModelViewer;
            if (!settings.UseGMOView)
                comboBox_ModelViewer.SelectedIndex = 1;
        }

        private void Save_Click(object sender, EventArgs e)
        {
            settings.FixForPC = chk_FixForPC.Checked;
            settings.OptimizeForVita = chk_OptimizeForVita.Checked;
            settings.OptimizeFbxWithNoesis = chk_OptimizeFbxWithNoesis.Checked;
            settings.NoesisArgs = txt_NoesisArgs.Text;
            settings.UseModelViewer = chk_UseModelViewer.Checked;
            settings.UseGMOView = (comboBox_ModelViewer.SelectedIndex == 0);

            settings.Save();
        }
    }
}
