﻿using System;
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
using AtlusFileSystemLibrary;
using AtlusFileSystemLibrary.FileSystems.PAK;
using AtlusFileSystemLibrary.Common.IO;
using AmicitiaLibrary.FileSystems.AMD;
using System.Runtime.InteropServices;
using System.Collections;
using Rainbow.ImgLib;
using Rainbow.ImgLib.Formats.Implementation;
using ShrineFox.IO;

namespace P4GMOdel
{
    public partial class MainForm : Form
    {
        Model model;
        public static Panel panel_GMOView;
        public static Process process_GMOView;

        public MainForm(string[] args)
        {
            InitializeComponent();
            settings.Load();

            // Add Model Viewer Panel
            panel_GMOView = new Panel() { BackColor = Color.FromArgb(255, 60, 63, 65), Dock = DockStyle.Fill };
            tlp_Main.Controls.Add(panel_GMOView, 1, 1);

            // Close model viewer on program exit
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnProcessExit);

            // Wait for form to appear before using commandline arguments
            if (args.Length > 0 && File.Exists(args[0]))
                model.Path = args[0];
            this.Shown += new System.EventHandler(this.Form_Shown);
        }

        private void OnProcessExit(object sender, EventArgs e)
        {
            // Attempt to close any open GMOView process on exit
            process_GMOView.Close();
        }

        private void Form_Shown(object sender, EventArgs e)
        {
            // Open file from arguments once form has finished loading
            if (!string.IsNullOrEmpty(model.Path))
                OpenFile(model.Path);
        }
    }
}