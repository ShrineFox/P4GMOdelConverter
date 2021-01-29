using P4GMOdel;
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

namespace P4GMOdel
{
    class ModelViewer
    {
        public static void LoadModel(string path)
        {
            //Close existing GMOView process
            CloseProcess();
            //Load GMOView
            Process process = new Process();
            process.StartInfo.FileName = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\Tools\\GMO\\GmoView.exe";
            process.StartInfo.Arguments = path;
            process.Start();
            process.WaitForInputIdle();
            sessions.Add("GMOView", process);
            //Add GMOView to form and focus on it
            SetParent(process.MainWindowHandle, MainForm.panelHandle);
            ShowWindow(process.MainWindowHandle, SW_MINIMIZE);
            SetForegroundWindow(process.MainWindowHandle);
            SetFocus(process.MainWindowHandle);
            MoveWindow(process.MainWindowHandle, 0, 0, MainForm.formWidth, MainForm.formHeight, true);
            //Remove menubar
            IntPtr HMENU = GetMenu(process.MainWindowHandle);
            int count = GetMenuItemCount(HMENU);
            for (int i = 0; i < count; i++)
                RemoveMenu(HMENU, 0, (MF_BYPOSITION | MF_REMOVE));
            DrawMenuBar(process.MainWindowHandle);
            //Remove title
            SetWindowLong(process.MainWindowHandle, GWL_STYLE, WS_VISIBLE);
            //Improve GMOView appearance
            RotateModel();
            ToggleLighting();
            //ToggleWireframeBG();
            ToggleAnimatedBG();
            IncreaseSize();
            PositionHigher();
            FixAspectRatio();
        }

        public static void Update(Model model, SettingsForm.Settings settings)
        {
            if (model != null && File.Exists(model.Path))
            {
                //Save temporary mds
                string tempPath = Tools.GetTemporaryPath(model.Path);
                File.WriteAllText(tempPath + ".mds", Model.Serialize(model, settings));
                using (WaitForFile(tempPath + ".mds", FileMode.Open, FileAccess.ReadWrite, FileShare.None)) { };
                //Attempt to generate temporary gmo
                Tools.GMOTool(tempPath + ".mds", false, settings);
                //Reload model viewer with temporary GMO
                using (WaitForFile(tempPath + ".gmo", FileMode.Open, FileAccess.ReadWrite, FileShare.None)) { };
                LoadModel(tempPath + ".gmo");
                MainForm.viewerUpdated = true;
            }
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
            Process p = (Process)sessions["GMOView"];
            ShowWindow(p.MainWindowHandle, SW_MAXIMIZE);
            SetForegroundWindow(p.MainWindowHandle);
            SetFocus(p.MainWindowHandle);
            InputSimulator s = new InputSimulator();
            s.Keyboard.KeyPress(VirtualKeyCode.F8);
            s.Keyboard.KeyPress(VirtualKeyCode.F8);
        }

        public static void CloseProcess()
        {
            foreach (Process p in Process.GetProcessesByName("GMOView"))
                p.Kill();
            sessions.Clear();
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
        public static Hashtable sessions = new Hashtable();

        public static FileStream WaitForFile(string fullPath, FileMode mode, FileAccess access, FileShare share)
        {
            for (int numTries = 0; numTries < 10; numTries++)
            {
                FileStream fs = null;
                try
                {
                    fs = new FileStream(fullPath, mode, access, share);
                    return fs;
                }
                catch (IOException)
                {
                    if (fs != null)
                    {
                        fs.Dispose();
                    }
                    Thread.Sleep(200);
                }
            }

            return null;
        }
    }
}
