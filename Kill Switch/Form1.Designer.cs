namespace Kill_Switch
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lockCheckbox = new CheckBox();
            shutdownCheckbox = new CheckBox();
            freezeCheckbox = new CheckBox();
            SuspendLayout();
            // 
            // lockCheckbox
            // 
            lockCheckbox.AutoSize = true;
            lockCheckbox.Location = new Point(44, 39);
            lockCheckbox.Name = "lockCheckbox";
            lockCheckbox.Size = new Size(51, 19);
            lockCheckbox.TabIndex = 0;
            lockCheckbox.Text = "Lock";
            lockCheckbox.UseVisualStyleBackColor = true;
            // 
            // shutdownCheckbox
            // 
            shutdownCheckbox.AutoSize = true;
            shutdownCheckbox.Location = new Point(44, 89);
            shutdownCheckbox.Name = "shutdownCheckbox";
            shutdownCheckbox.Size = new Size(80, 19);
            shutdownCheckbox.TabIndex = 1;
            shutdownCheckbox.Text = "Shutdown";
            shutdownCheckbox.UseVisualStyleBackColor = true;
            // 
            // freezeCheckbox
            // 
            freezeCheckbox.AutoSize = true;
            freezeCheckbox.Location = new Point(44, 64);
            freezeCheckbox.Name = "freezeCheckbox";
            freezeCheckbox.Size = new Size(95, 19);
            freezeCheckbox.TabIndex = 2;
            freezeCheckbox.Text = "Screen freeze";
            freezeCheckbox.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(freezeCheckbox);
            Controls.Add(shutdownCheckbox);
            Controls.Add(lockCheckbox);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private CheckBox lockCheckbox;
        private CheckBox shutdownCheckbox;
        private CheckBox freezeCheckbox;
    }
}
