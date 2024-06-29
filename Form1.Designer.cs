namespace WinFormsApp1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            sampleControl = new Editor.Controls.Graphic();
            SuspendLayout();
            // 
            // sampleControl
            // 
            sampleControl.Dock = DockStyle.Fill;
            sampleControl.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            sampleControl.GraphicsProfile = Microsoft.Xna.Framework.Graphics.GraphicsProfile.HiDef;
            sampleControl.Location = new Point(0, 0);
            sampleControl.MouseHoverUpdatesOnly = false;
            sampleControl.Name = "sampleControl";
            sampleControl.Size = new Size(800, 600);
            sampleControl.TabIndex = 0;
            sampleControl.Text = "Sample Control";
            sampleControl.KeyDown += sampleControl_KeyDown;
            sampleControl.KeyUp += sampleControl_KeyUp;
            sampleControl.MouseDown += sampleControl_MouseDown;
            sampleControl.MouseMove += sampleControl_MouseMove;
            sampleControl.MouseUp += sampleControl_MouseUp;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 600);
            Controls.Add(sampleControl);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Cyan";
            ResumeLayout(false);
        }

        #endregion

        private Editor.Controls.Graphic sampleControl;
    }
}