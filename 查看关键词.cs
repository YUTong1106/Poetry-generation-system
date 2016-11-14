using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace 依据现代文的古诗生成系统
{
    public partial class 查看关键词 : Form
    {
        public 查看关键词()
        {
            InitializeComponent();
        }

        private void 关闭查看关键词_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void 关键词文本框_TextChanged(object sender, EventArgs e)
        {
            //打开一个选择文本文件的对话框
            OpenFileDialog textFileDialog = new OpenFileDialog();
            //textFileDialog.InitialDirectory = "D://";
            textFileDialog.Filter = "文本文件|*.txt";
            textFileDialog.RestoreDirectory = false;
            //读取文本内容，放到文本框里
            if (textFileDialog.ShowDialog() == DialogResult.OK)
            {
                StreamReader readText = new StreamReader(textFileDialog.FileName, Encoding.GetEncoding("GB2312"));
                关键词文本框.Text = readText.ReadToEnd();
                

            }
        }
    }
}
