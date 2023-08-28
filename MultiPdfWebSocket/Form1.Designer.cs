
namespace MultiPdfWebSocket
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.axPDFView1 = new AxPDFVIEWLib.AxPDFView();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.显示插件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.退出服务ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.隐藏插件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.隐藏插件ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.axPDFView1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // axPDFView1
            // 
            this.axPDFView1.Enabled = true;
            this.axPDFView1.Location = new System.Drawing.Point(-4, 1);
            this.axPDFView1.Name = "axPDFView1";
            this.axPDFView1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axPDFView1.OcxState")));
            this.axPDFView1.Size = new System.Drawing.Size(1168, 689);
            this.axPDFView1.TabIndex = 0;
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "MultiPdfWebSocket";
            this.notifyIcon1.Visible = true;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.显示插件ToolStripMenuItem,
            this.隐藏插件ToolStripMenuItem,
            this.隐藏插件ToolStripMenuItem1,
            this.退出服务ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(181, 92);
            this.contextMenuStrip1.Click += new System.EventHandler(this.显示插件ToolStripMenuItem_Click);
            // 
            // 显示插件ToolStripMenuItem
            // 
            this.显示插件ToolStripMenuItem.Name = "显示插件ToolStripMenuItem";
            this.显示插件ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.显示插件ToolStripMenuItem.Text = "显示插件";
            this.显示插件ToolStripMenuItem.Click += new System.EventHandler(this.显示插件ToolStripMenuItem_Click);
            // 
            // 退出服务ToolStripMenuItem
            // 
            this.退出服务ToolStripMenuItem.Name = "退出服务ToolStripMenuItem";
            this.退出服务ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.退出服务ToolStripMenuItem.Text = "退出服务";
            this.退出服务ToolStripMenuItem.Click += new System.EventHandler(this.退出服务ToolStripMenuItem_Click);
            // 
            // 隐藏插件ToolStripMenuItem
            // 
            this.隐藏插件ToolStripMenuItem.Name = "隐藏插件ToolStripMenuItem";
            this.隐藏插件ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.隐藏插件ToolStripMenuItem.Text = "隐藏插件";
            // 
            // 隐藏插件ToolStripMenuItem1
            // 
            this.隐藏插件ToolStripMenuItem1.Name = "隐藏插件ToolStripMenuItem1";
            this.隐藏插件ToolStripMenuItem1.Size = new System.Drawing.Size(180, 22);
            this.隐藏插件ToolStripMenuItem1.Text = "隐藏插件";
            this.隐藏插件ToolStripMenuItem1.Click += new System.EventHandler(this.隐藏插件ToolStripMenuItem1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 711);
            this.ControlBox = false;
            this.Controls.Add(this.axPDFView1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "MultiPdfWebSocket";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            ((System.ComponentModel.ISupportInitialize)(this.axPDFView1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public AxPDFVIEWLib.AxPDFView axPDFView1;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 显示插件ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 退出服务ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 隐藏插件ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 隐藏插件ToolStripMenuItem1;
    }
}

