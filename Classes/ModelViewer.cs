using P4GMOdel;
using ShrineFox.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsInput;
using WindowsInput.Native;
using static P4GMOdel.MainForm;

namespace P4GMOdel
{
    class ModelViewer
    {
        public static void LoadModel(string gmoPath)
        {
            if (MainForm.process_GMOView != null)
                MainForm.process_GMOView.Close();

            if (settings.UseModelViewer)
            {
                string gmoView = ".\\Tools\\GMO\\GmoView.exe";

                // Load and dock in form
                MainForm.process_GMOView = Window.Mount(gmoView, MainForm.panel_GMOView, gmoPath);

                //Improve GMOView appearance
                RotateModel();
                ToggleLighting();
                //ToggleWireframeBG();
                ToggleAnimatedBG();
                IncreaseSize();
                PositionHigher();
                FixAspectRatio();
            }
        }

        public static void Update(Model model)
        {
            if (model != null && File.Exists(model.Path))
            {
                int x = 0;
                //Save temporary mds
                string tempPath = Tools.GetTemporaryPath(model.Path);
                File.WriteAllText(tempPath + ".mds", Model.Serialize(model));
                using (Tools.WaitForFile(tempPath + ".mds", FileMode.Open, FileAccess.ReadWrite, FileShare.None)) { };
                //Attempt to generate temporary gmo
                Tools.GMOTool(tempPath + ".mds", false);
                using (Tools.WaitForFile($"{tempPath}.mds", FileMode.Open, FileAccess.ReadWrite, FileShare.None)) { };
                //Reload model viewer with temporary GMO
                using (Tools.WaitForFile(tempPath + ".gmo", FileMode.Open, FileAccess.ReadWrite, FileShare.None)) { };
                LoadModel(tempPath + ".gmo");
            }
            MainForm.viewerUpdated = true;
        }

        public static void RotateModel()
        {
            InputSimulator s = new InputSimulator();
            for (int i = 0; i < 18; i++)
                s.Keyboard.KeyPress(VirtualKeyCode.LEFT);
        }

        public static void ToggleLighting()
        {
            InputSimulator s = new InputSimulator();
            s.Keyboard.KeyPress(VirtualKeyCode.F1);
        }

        public static void ToggleWireframeBG()
        {
            InputSimulator s = new InputSimulator();
            s.Keyboard.KeyPress(VirtualKeyCode.F7);
        }

        public static void ToggleAnimatedBG()
        {
            InputSimulator s = new InputSimulator();
            s.Keyboard.KeyPress(VirtualKeyCode.F5);
        }

        public static void FixAspectRatio()
        {
            if (MainForm.process_GMOView != null)
            {
                ShowWindow(MainForm.process_GMOView.MainWindowHandle, SW_MAXIMIZE);
                SetForegroundWindow(MainForm.process_GMOView.MainWindowHandle);
                SetFocus(MainForm.process_GMOView.MainWindowHandle);
                InputSimulator s = new InputSimulator();
                s.Keyboard.KeyPress(VirtualKeyCode.F8);
                s.Keyboard.KeyPress(VirtualKeyCode.F8);
            }
        }

        public static void IncreaseSize()
        {
            InputSimulator s = new InputSimulator();
            s.Keyboard.KeyPress(VirtualKeyCode.VK_3);
            for (int i = 0; i < 12; i++)
                s.Keyboard.KeyPress(VirtualKeyCode.DOWN);
            for (int i = 0; i < 7; i++)
                s.Keyboard.KeyPress(VirtualKeyCode.RIGHT);
            s.Keyboard.KeyPress(VirtualKeyCode.VK_1);
        }

        public static void PositionHigher()
        {
            InputSimulator s = new InputSimulator();
            for (int i = 0; i < 3; i++)
                s.Keyboard.ModifiedKeyStroke(VirtualKeyCode.SHIFT, VirtualKeyCode.UP);
        }

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr SetFocus(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);
        [DllImport("user32.dll")]
        static extern IntPtr GetMenu(IntPtr hWnd);
        [DllImport("user32.dll")]
        static extern int GetMenuItemCount(IntPtr hMenu);
        [DllImport("user32.dll")]
        static extern bool DrawMenuBar(IntPtr hWnd);
        [DllImport("user32.dll")]
        static extern bool RemoveMenu(IntPtr hMenu, uint uPosition, uint uFlags);
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, uint wParam, uint lParam);
        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool DestroyWindow(IntPtr hwnd);

        const int GWL_STYLE = (-16);
        private const int SW_MAXIMIZE = 3;
        private const int SW_MINIMIZE = 2;
        private const int SW_NORMALE = 1;
        public static uint MF_BYPOSITION = 0x400;
        public static uint MF_REMOVE = 0x1000;
        const uint WS_VISIBLE = 0x10000000;
    }
}
