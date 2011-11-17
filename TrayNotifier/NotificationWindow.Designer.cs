namespace TrayNotifier
{
    partial class NotificationWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NotificationWindow));
            this.notificationIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.SuspendLayout();
            // 
            // notificationIcon
            // 
            this.notificationIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notificationIcon.Icon")));
            this.notificationIcon.Visible = true;
            // 
            // NotificationWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(116, 22);
            this.ControlBox = false;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NotificationWindow";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Tray Notification Window";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NotifyIcon notificationIcon;
    }
}