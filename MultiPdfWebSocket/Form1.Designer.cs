﻿
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.axPDFView1 = new AxPDFVIEWLib.AxPDFView();
            ((System.ComponentModel.ISupportInitialize)(this.axPDFView1)).BeginInit();
            this.SuspendLayout();
            // 
            // axPDFView1
            // 
            this.axPDFView1.Enabled = true;
            this.axPDFView1.Location = new System.Drawing.Point(0, 1);
            this.axPDFView1.Name = "axPDFView1";
            this.axPDFView1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axPDFView1.OcxState")));
            this.axPDFView1.Size = new System.Drawing.Size(1288, 712);
            this.axPDFView1.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1284, 711);
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
            this.ResumeLayout(false);

        }

        #endregion

        private AxPDFVIEWLib.AxPDFView axPDFView1;
    }
}
