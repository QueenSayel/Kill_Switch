using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Kill_Switch
{
    public partial class Form1 : Form
    {
        // Constants for hotkey modifiers and the F12 key
        const int MOD_ALT = 0x1; // Alt key modifier
        const int VK_F12 = 0x7B; // Virtual key code for F12

        // Import RegisterHotKey and UnregisterHotKey from User32.dll
        [DllImport("user32.dll")]
        public static extern int RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);

        [DllImport("user32.dll")]
        public static extern int UnregisterHotKey(IntPtr hWnd, int id);

        // Import Windows API to lock the workstation
        [DllImport("user32.dll")]
        public static extern bool LockWorkStation();

        public Form1()
        {
            InitializeComponent();
            RegisterHotkey(); // Register the hotkey when the form is loaded
        }

        // Register the hotkey (Alt + F12)
        private void RegisterHotkey()
        {
            RegisterHotKey(this.Handle, 1, MOD_ALT, VK_F12); // Register Alt + F12
        }

        // Unregister the hotkey when the form is closed
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            UnregisterHotKey(this.Handle, 1); // Unregister the hotkey when the form is closed
        }

        // Override WndProc to capture hotkey events
        protected override void WndProc(ref Message m)
        {
            const int WM_HOTKEY = 0x0312; // Message ID for hotkey events

            // If the hotkey is pressed, trigger the lockdown functionality
            if (m.Msg == WM_HOTKEY)
            {
                OnHotkeyPressed();
            }

            base.WndProc(ref m);
        }

        // Trigger lockdown actions when the hotkey is pressed
        private void OnHotkeyPressed()
        {
            // Check the state of the checkboxes and execute the corresponding actions
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
        }

        // Lock the workstation
        private void LockComputer()
        {
            LockWorkStation(); // Lock the computer using Windows API
        }

        // Freeze the screen (display a blank screen)
        private void BlankScreen()
        {
            Form blankForm = new Form();
            blankForm.FormBorderStyle = FormBorderStyle.None;
            blankForm.WindowState = FormWindowState.Maximized;
            blankForm.TopMost = true;
            blankForm.BackColor = System.Drawing.Color.Black;
            blankForm.Show();
        }

        // Shut down the system immediately
        private void ShutdownComputer()
        {
            Process.Start("shutdown", "/s /f /t 0"); // Forced shutdown
        }
    }
}
