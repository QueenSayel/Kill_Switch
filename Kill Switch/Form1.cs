using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Kill_Switch
{
    public partial class Form1 : Form
    {
        // Constants for hotkey modifiers and the F12 key
        const int MOD_ALT = 0x1;
        const int VK_F12 = 0x7B;

        // Import RegisterHotKey and UnregisterHotKey from User32.dll
        [DllImport("user32.dll")]
        public static extern int RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);

        [DllImport("user32.dll")]
        public static extern int UnregisterHotKey(IntPtr hWnd, int id);

        // Import Windows API to lock the workstation
        [DllImport("user32.dll")]
        public static extern bool LockWorkStation();

        // Variables for the keyboard hook
        private IntPtr ptrHook;
        private LowLevelKeyboardProc objKeyboardProcess;

        // Structure for low-level keyboard input event
        [StructLayout(LayoutKind.Sequential)]
        private struct KBDLLHOOKSTRUCT
        {
            public Keys key;
            public int scanCode;
            public int flags;
            public int time;
            public IntPtr extra;
        }

        // Delegate for the keyboard hook procedure
        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int id, LowLevelKeyboardProc callback, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool UnhookWindowsHookEx(IntPtr hook);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hook, int nCode, IntPtr wp, IntPtr lp);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string name);

        public Form1()
        {
            InitializeComponent();
            RegisterHotkey();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (keyboardCheckbox.Checked)
            {
                // Activate keyboard hook to block certain keypresses
                ProcessModule objCurrentModule = Process.GetCurrentProcess().MainModule;
                objKeyboardProcess = new LowLevelKeyboardProc(CaptureKey);
                ptrHook = SetWindowsHookEx(13, objKeyboardProcess, GetModuleHandle(objCurrentModule.ModuleName), 0);
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            UnregisterHotKey(this.Handle, 1);

            // Release the keyboard hook when the form is closed
            if (ptrHook != IntPtr.Zero)
            {
                UnhookWindowsHookEx(ptrHook);
                ptrHook = IntPtr.Zero;
            }
        }

        // Register the hotkey (Alt + F12)
        private void RegisterHotkey()
        {
            RegisterHotKey(this.Handle, 1, MOD_ALT, VK_F12);
        }

        // Override WndProc to capture hotkey events
        protected override void WndProc(ref Message m)
        {
            const int WM_HOTKEY = 0x0312;

            if (m.Msg == WM_HOTKEY)
            {
                OnHotkeyPressed();
            }

            base.WndProc(ref m);
        }

        // Trigger lockdown actions when the hotkey is pressed
        private void OnHotkeyPressed()
        {
            if (lockCheckbox.Checked)
            {
                LockComputer();
            }

            if (freezeCheckbox.Checked)
            {
                BlankScreen();
            }

            if (shutdownCheckbox.Checked)
            {
                ShutdownComputer();
            }

            if (keyboardCheckbox.Checked)
            {
                // Reapply the keyboard hook if not already active
                if (ptrHook == IntPtr.Zero)
                {
                    ProcessModule objCurrentModule = Process.GetCurrentProcess().MainModule;
                    objKeyboardProcess = new LowLevelKeyboardProc(CaptureKey);
                    ptrHook = SetWindowsHookEx(13, objKeyboardProcess, GetModuleHandle(objCurrentModule.ModuleName), 0);
                }
            }
            else
            {
                // Remove the hook if checkbox is unchecked
                if (ptrHook != IntPtr.Zero)
                {
                    UnhookWindowsHookEx(ptrHook);
                    ptrHook = IntPtr.Zero;
                }
            }
        }

        // Lock the workstation
        private void LockComputer()
        {
            LockWorkStation();
        }

        // Freeze the screen (display a blank screen)
        private void BlankScreen()
        {
            foreach (var screen in Screen.AllScreens)
            {
                Form blankForm = new Form
                {
                    FormBorderStyle = FormBorderStyle.None,
                    WindowState = FormWindowState.Normal,
                    TopMost = true,
                    BackColor = System.Drawing.Color.Black,
                    StartPosition = FormStartPosition.Manual,
                    Bounds = screen.Bounds
                };

                blankForm.Show();
                blankForm.WindowState = FormWindowState.Maximized;
            }
        }

        // Shut down the system immediately
        private void ShutdownComputer()
        {
            Process.Start("shutdown", "/s /f /t 0");
        }

        // Capture and block certain key combinations
        private IntPtr CaptureKey(int nCode, IntPtr wp, IntPtr lp)
        {
            if (nCode >= 0)
            {
                // Block all keys by returning (IntPtr)1 for any key press
                return (IntPtr)1;
            }

            // Pass through other messages if nCode < 0
            return CallNextHookEx(ptrHook, nCode, wp, lp);
        }

        private bool HasAltModifier(int flags)
        {
            return (flags & 0x20) == 0x20;
        }
    }
}
