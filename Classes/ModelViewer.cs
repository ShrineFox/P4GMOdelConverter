using DarkUI.Forms;
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

namespace P4GMOdel
{
    public partial class MainForm : DarkForm
    {

        public static void BuildTempModel(Model model)
        {
            if (model != null && File.Exists(model.Path))
            {
                //Save temporary gms
                string tempPath = GetTemporaryPath(model.Path);
                File.WriteAllText(tempPath + ".gms", Model.Serialize(model));
                using (FileSys.WaitForFile(tempPath + ".gms")) { };

                //Attempt to generate temporary gmo
                GMOTool(tempPath + ".gms", false);
                using (FileSys.WaitForFile($"{tempPath}.gms")) { };

                //Reload model viewer with temporary GMO
                using (FileSys.WaitForFile(tempPath + ".gmo")) { };
                UpdateModelViewer(tempPath + ".gmo");
            }
        }

        public static void UpdateModelViewer(string gmoPath)
        {
            if (settings.UseModelViewer && settings.UseGMOView)
            {
                if (process_GMOView != null)
                    process_GMOView.Close();

                string gmoView = ".\\Dependencies\\GMO\\GmoView.exe";

                // Load and dock in form
                process_GMOView = Window.Mount(gmoView, panel_GMOView, gmoPath);

                //Improve GMOView appearance
                RotateModel();
                ToggleLighting();
                ToggleAnimatedBG();
                IncreaseSize();
                PositionHigher();
                FixAspectRatio();
            }
        }

        public static void RotateModel()
        {
            for (int i = 0; i < 18; i++)
                Simulate.Events().Click(WindowsInput.Events.KeyCode.Left);
        }

        public static void ToggleLighting()
        {
            Simulate.Events().Click(WindowsInput.Events.KeyCode.F1);
        }

        public static void ToggleWireframeBG()
        {
            Simulate.Events().Click(WindowsInput.Events.KeyCode.F7);
        }

        public static void ToggleAnimatedBG()
        {
            Simulate.Events().Click(WindowsInput.Events.KeyCode.F5);
        }

        public static void FixAspectRatio()
        {
            if (MainForm.process_GMOView != null)
            {
                ShowWindow(MainForm.process_GMOView.MainWindowHandle, SW_MAXIMIZE);
                SetForegroundWindow(MainForm.process_GMOView.MainWindowHandle);
                SetFocus(MainForm.process_GMOView.MainWindowHandle);
                Simulate.Events().Click(WindowsInput.Events.KeyCode.F8);
                Simulate.Events().Click(WindowsInput.Events.KeyCode.F8);
            }
        }

        public static void IncreaseSize()
        {
            Simulate.Events().Click(WindowsInput.Events.KeyCode.D3);
            for (int i = 0; i < 12; i++)
                Simulate.Events().Click(WindowsInput.Events.KeyCode.Down);
            for (int i = 0; i < 7; i++)
                Simulate.Events().Click(WindowsInput.Events.KeyCode.Right);
            Simulate.Events().Click(WindowsInput.Events.KeyCode.D1);
        }

        public static void PositionHigher()
        {
            for (int i = 0; i < 3; i++)
                Simulate.Events().ClickChord(WindowsInput.Events.KeyCode.LShift, WindowsInput.Events.KeyCode.Up);
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
