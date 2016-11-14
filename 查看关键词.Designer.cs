namespace 依据现代文的古诗生成系统
{
    partial class 查看关键词
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
            this.关键词文本框 = new System.Windows.Forms.RichTextBox();
            this.关闭查看关键词 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // 关键词文本框
            // 
            this.关键词文本框.Location = new System.Drawing.Point(12, 12);
            this.关键词文本框.Name = "关键词文本框";
            this.关键词文本框.Size = new System.Drawing.Size(260, 208);
            this.关键词文本框.TabIndex = 0;
            this.关键词文本框.Text = "";
            this.关键词文本框.TextChanged += new System.EventHandler(this.关键词文本框_TextChanged);
            // 
            // 关闭查看关键词
            // 
            this.关闭查看关键词.Location = new System.Drawing.Point(197, 226);
            this.关闭查看关键词.Name = "关闭查看关键词";
            this.关闭查看关键词.Size = new System.Drawing.Size(75, 23);
            this.关闭查看关键词.TabIndex = 1;
            this.关闭查看关键词.Text = "关闭";
            this.关闭查看关键词.UseVisualStyleBackColor = true;
            this.关闭查看关键词.Click += new System.EventHandler(this.关闭查看关键词_Click);
            // 
            // 查看关键词
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.ControlBox = false;
            this.Controls.Add(this.关闭查看关键词);
            this.Controls.Add(this.关键词文本框);
            this.Name = "查看关键词";
            this.Text = "查看关键词";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox 关键词文本框;
        private System.Windows.Forms.Button 关闭查看关键词;
    }
}