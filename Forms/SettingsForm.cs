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
            chk_UseModelViewer.Checked = settings.UseModelViewer;
        }

        private void Save_Click(object sender, EventArgs e)
        {
            settings.FixForPC = chk_FixForPC.Checked;
            settings.OptimizeForVita = chk_OptimizeForVita.Checked;
            settings.UseModelViewer = chk_UseModelViewer.Checked;

            settings.Save();
        }
    }
}
