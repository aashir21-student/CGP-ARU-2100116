namespace GrafPack
{
    partial class GrafPack
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
            this.drawingPanel = new System.Windows.Forms.Panel();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.mainMenuStrip = new System.Windows.Forms.MenuStrip();
            this.createMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.squareMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.circleMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.triangleMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.transformMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rotateMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reflectMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reflectHorizontalMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reflectVerticalMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.drawingPanel.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.mainMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // drawingPanel
            // 
            this.drawingPanel.Controls.Add(this.statusStrip);
            this.drawingPanel.Controls.Add(this.mainMenuStrip);
            this.drawingPanel.Location = new System.Drawing.Point(1, 1);
            this.drawingPanel.Name = "drawingPanel";
            this.drawingPanel.Size = new System.Drawing.Size(983, 658);
            this.drawingPanel.TabIndex = 0;
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 636);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(983, 22);
            this.statusStrip.TabIndex = 1;
            this.statusStrip.Text = "statusStrip1";
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(39, 17);
            this.statusLabel.Text = "Ready";
            // 
            // mainMenuStrip
            // 
            this.mainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createMenuItem,
            this.selectMenuItem,
            this.transformMenuItem,
            this.deleteMenuItem,
            this.exitMenuItem});
            this.mainMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.mainMenuStrip.Name = "mainMenuStrip";
            this.mainMenuStrip.Size = new System.Drawing.Size(983, 24);
            this.mainMenuStrip.TabIndex = 0;
            this.mainMenuStrip.Text = "menuStrip1";
            // 
            // createMenuItem
            // 
            this.createMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.squareMenuItem,
            this.circleMenuItem,
            this.triangleMenuItem});
            this.createMenuItem.Name = "createMenuItem";
            this.createMenuItem.Size = new System.Drawing.Size(53, 20);
            this.createMenuItem.Text = "Create";
            // 
            // squareMenuItem
            // 
            this.squareMenuItem.Name = "squareMenuItem";
            this.squareMenuItem.Size = new System.Drawing.Size(115, 22);
            this.squareMenuItem.Text = "Square";
            // 
            // circleMenuItem
            // 
            this.circleMenuItem.Name = "circleMenuItem";
            this.circleMenuItem.Size = new System.Drawing.Size(115, 22);
            this.circleMenuItem.Text = "Circle";
            // 
            // triangleMenuItem
            // 
            this.triangleMenuItem.Name = "triangleMenuItem";
            this.triangleMenuItem.Size = new System.Drawing.Size(115, 22);
            this.triangleMenuItem.Text = "Triangle";
            // 
            // selectMenuItem
            // 
            this.selectMenuItem.Name = "selectMenuItem";
            this.selectMenuItem.Size = new System.Drawing.Size(50, 20);
            this.selectMenuItem.Text = "Select";
            // 
            // transformMenuItem
            // 
            this.transformMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.moveMenuItem,
            this.rotateMenuItem,
            this.reflectMenuItem});
            this.transformMenuItem.Name = "transformMenuItem";
            this.transformMenuItem.Size = new System.Drawing.Size(72, 20);
            this.transformMenuItem.Text = "Transform";
            // 
            // moveMenuItem
            // 
            this.moveMenuItem.Name = "moveMenuItem";
            this.moveMenuItem.Size = new System.Drawing.Size(180, 22);
            this.moveMenuItem.Text = "Move";
            // 
            // rotateMenuItem
            // 
            this.rotateMenuItem.Name = "rotateMenuItem";
            this.rotateMenuItem.Size = new System.Drawing.Size(180, 22);
            this.rotateMenuItem.Text = "Rotate";
            // 
            // deleteMenuItem
            // 
            this.deleteMenuItem.Name = "deleteMenuItem";
            this.deleteMenuItem.Size = new System.Drawing.Size(52, 20);
            this.deleteMenuItem.Text = "Delete";
            // 
            // exitMenuItem
            // 
            this.exitMenuItem.Name = "exitMenuItem";
            this.exitMenuItem.Size = new System.Drawing.Size(38, 20);
            this.exitMenuItem.Text = "Exit";
            // 
            // reflectMenuItem
            // 
            this.reflectMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.reflectHorizontalMenuItem,
            this.reflectVerticalMenuItem});
            this.reflectMenuItem.Name = "reflectMenuItem";
            this.reflectMenuItem.Size = new System.Drawing.Size(180, 22);
            this.reflectMenuItem.Text = "Reflect";
            // 
            // reflectHorizontalMenuItem
            // 
            this.reflectHorizontalMenuItem.Name = "reflectHorizontalMenuItem";
            this.reflectHorizontalMenuItem.Size = new System.Drawing.Size(180, 22);
            this.reflectHorizontalMenuItem.Text = "Reflect Horizontally";
            // 
            // reflectVerticalMenuItem
            // 
            this.reflectVerticalMenuItem.Name = "reflectVerticalMenuItem";
            this.reflectVerticalMenuItem.Size = new System.Drawing.Size(180, 22);
            this.reflectVerticalMenuItem.Text = "Reflect Vertically";
            // 
            // GrafPack
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 661);
            this.Controls.Add(this.drawingPanel);
            this.MainMenuStrip = this.mainMenuStrip;
            this.Name = "GrafPack";
            this.Text = "GrafPack";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.GrafPack_Load);
            this.drawingPanel.ResumeLayout(false);
            this.drawingPanel.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.mainMenuStrip.ResumeLayout(false);
            this.mainMenuStrip.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel drawingPanel;
        private System.Windows.Forms.MenuStrip mainMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem createMenuItem;
        private System.Windows.Forms.ToolStripMenuItem squareMenuItem;
        private System.Windows.Forms.ToolStripMenuItem circleMenuItem;
        private System.Windows.Forms.ToolStripMenuItem triangleMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectMenuItem;
        private System.Windows.Forms.ToolStripMenuItem transformMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moveMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rotateMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.ToolStripMenuItem reflectMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reflectHorizontalMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reflectVerticalMenuItem;
    }
}

