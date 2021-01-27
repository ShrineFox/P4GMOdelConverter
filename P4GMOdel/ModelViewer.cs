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
using System.Threading.Tasks;
using System.Windows.Forms;

namespace P4GModelConverter
{
    class ModelViewer
    {
        public static void LoadModel(string path, MainForm mainForm, IntPtr panelHandle)
        {
            //Load GMOView
            mainForm.WindowState = FormWindowState.Maximized;
            var process = new Process();
            process.StartInfo.FileName = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\Tools\\GMO\\GmoView.exe";
            process.StartInfo.Arguments = path;
            process.Start();
            process.WaitForInputIdle();
            sessions.Add("GMOView", process);
            SetParent(process.MainWindowHandle, panelHandle);
            ShowWindow(process.MainWindowHandle, SW_MAXIMIZE);
            Process pHandle = (Process)sessions["GMOView"];
            SetForegroundWindow(pHandle.MainWindowHandle);
            SetFocus(pHandle.MainWindowHandle);
            MoveWindow(pHandle.MainWindowHandle, 0, 0, mainForm.Width, mainForm.Height, true);
            //Get Menu Bar
            IntPtr HMENU = GetMenu(pHandle.MainWindowHandle);
            //Get Item Count
            int count = GetMenuItemCount(HMENU);
            //loop & remove
            for (int i = 0; i < count; i++)
                RemoveMenu(HMENU, 0, (MF_BYPOSITION | MF_REMOVE));
            //Force redraw
            DrawMenuBar(pHandle.MainWindowHandle);
            //Remove title
            SetWindowLong(pHandle.MainWindowHandle, GWL_STYLE, WS_VISIBLE);
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

        const int GWL_STYLE = (-16);
        private const int SW_MAXIMIZE = 3;
        public static uint MF_BYPOSITION = 0x400;
        public static uint MF_REMOVE = 0x1000;
        const uint WS_VISIBLE = 0x10000000;
        public static Hashtable sessions = new Hashtable();
    }
}
