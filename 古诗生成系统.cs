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
    public partial class 古诗生成系统 : Form
    {
        private static string[] sen;
        public string fName;

        public 古诗生成系统()
        {
            InitializeComponent();
            //删除除了欢迎页以外的标签页
            向导标签页.TabPages.Remove(读取现代文);
            向导标签页.TabPages.Remove(设置格律);
            向导标签页.TabPages.Remove(设置韵部);
            向导标签页.TabPages.Remove(准备生成);
            向导标签页.TabPages.Remove(生成中);
            向导标签页.TabPages.Remove(结果显示);
            向导标签页.TabPages.Remove(朗诵);
        }

        private void tab0下一步_Click(object sender, EventArgs e)
        {
            //添加下一个标签页
            向导标签页.TabPages.Add(读取现代文);
            //删除现在标签页
            向导标签页.TabPages.Remove(欢迎);
            //转向下一个标签页
            向导标签页.SelectTab("读取现代文");
        }

        private void tab1上一步_Click(object sender, EventArgs e)
        {
            //添加下一个标签页
            向导标签页.TabPages.Add(欢迎);
            //删除现在标签页
            向导标签页.TabPages.Remove(读取现代文);
            //转向下一个标签页
            向导标签页.SelectTab("欢迎");
        }

        private void tab1下一步_Click(object sender, EventArgs e)
        {
            if (现代文文本框.Text.Length != 0)
            {
                //添加下一个标签页
                向导标签页.TabPages.Add(设置格律);
                //删除现在标签页
                向导标签页.TabPages.Remove(读取现代文);
                //转向下一个标签页
                向导标签页.SelectTab("设置格律");
                
            }
            else
            {
                MessageBox.Show("文本不能为空");
            }
        }

        private void 读取文本_Click(object sender, EventArgs e)
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
                现代文文本框.Text = readText.ReadToEnd();
                fName = textFileDialog.SafeFileName;
                string name = fName.Substring(0, fName.LastIndexOf("."));//去掉了后缀名 
                //FileInfo myFile = new FileInfo(textFileDialog.FileName);
                //fName = myFile.FileName;//myFile.FileName为所需无路径文件名
                现代文.Deal(name);
                
            }
        }

        private void tab2上一步_Click(object sender, EventArgs e)
        {
            //添加下一个标签页
            向导标签页.TabPages.Add(读取现代文);
            //删除现在标签页
            向导标签页.TabPages.Remove(设置格律);
            //转向下一个标签页
            向导标签页.SelectTab("读取现代文");
        }

        private void tab2下一步_Click(object sender, EventArgs e)
        {
            if (仄起偏格.Checked == true || 仄起正格.Checked == true 
                || 平起偏格.Checked == true || 平起正格.Checked == true)
            {
                ///
                if (遗传算法框架.遗传算法框架.HanziBase == null)
                {
                    MessageBox.Show("没有汉字库，请输入汉字库");
                    return;
                }

                if (仄起正格.Checked)
                {
                    遗传算法框架.遗传算法框架.PingZeCheck = 1;
                }
                else if (平起正格.Checked)
                {
                    遗传算法框架.遗传算法框架.PingZeCheck = 2;
                }
                else if (仄起偏格.Checked)
                {
                    遗传算法框架.遗传算法框架.PingZeCheck = 3;
                }
                else if (平起偏格.Checked)
                {
                    遗传算法框架.遗传算法框架.PingZeCheck = 4;
                }
                ///
                //添加下一个标签页
                向导标签页.TabPages.Add(设置韵部);
                //删除现在标签页
                向导标签页.TabPages.Remove(设置格律);
                //转向下一个标签页
                向导标签页.SelectTab("设置韵部");
            }
            else
            {
                MessageBox.Show("请选择一个格律");
            }
        }

        private void tab3上一步_Click(object sender, EventArgs e)
        {
            //添加下一个标签页
            向导标签页.TabPages.Add(设置格律);
            //删除现在标签页
            向导标签页.TabPages.Remove(设置韵部);
            //转向下一个标签页
            向导标签页.SelectTab("设置格律");
        }

        private void tab3下一步_Click(object sender, EventArgs e)
        {
            //韵部的编号，此为数据库编号减1，为-1时表示用户未选择
            int YunIndex = 选择韵部.SelectedIndex;
            if (YunIndex != -1)
            {
                遗传算法框架.遗传算法框架.YunBu = YunIndex + 1;
                //添加下一个标签页
                向导标签页.TabPages.Add(准备生成);
                //删除现在标签页
                向导标签页.TabPages.Remove(设置韵部);
                //转向下一个标签页
                向导标签页.SelectTab("准备生成");
            }
            else
            {
                MessageBox.Show("请选择一个韵部");
            }
        }

        private void tab4上一步_Click(object sender, EventArgs e)
        {
            //添加下一个标签页
            向导标签页.TabPages.Add(设置韵部);
            //删除现在标签页
            向导标签页.TabPages.Remove(准备生成);
            //转向下一个标签页
            向导标签页.SelectTab("设置韵部");
        }

        private void 查看_Click(object sender, EventArgs e)
        {
            查看关键词 lookupform = new 查看关键词();
            lookupform.Show();
        }

        private void tab4下一步_Click(object sender, EventArgs e)
        {
            //添加下一个标签页
            向导标签页.TabPages.Add(生成中);
            //删除现在标签页
            向导标签页.TabPages.Remove(准备生成);
            //转向下一个标签页
            向导标签页.SelectTab("生成中");

            生成等候条.Increment(33);

            sen = new string[100];
            try
            {
                sen = 遗传算法框架.遗传算法框架.G.GeneAlgorithm();
                生成等候条.Increment(66);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }

            生成等候条.Hide();
            正在生成说明.Text = "生成完成";
            tab5下一步.Show();
        }

        private void tab5下一步_Click(object sender, EventArgs e)
        {
            //添加下一个标签页
            向导标签页.TabPages.Add(结果显示);
            //删除现在标签页
            向导标签页.TabPages.Remove(生成中);
            //转向下一个标签页
            向导标签页.SelectTab("结果显示");
            //向结果显示框加入
            foreach (string s in sen)
            {
                结果显示框.Items.Add(s);
            }
            
        }

        private void 结果显示框_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked)
            {
                foreach (int i in 结果显示框.CheckedIndices)
                    结果显示框.SetItemCheckState(i, CheckState.Unchecked);
            }
        }

        private void tab7上一步_Click(object sender, EventArgs e)
        {
            //添加下一个标签页
            向导标签页.TabPages.Add(结果显示);
            //删除现在标签页
            向导标签页.TabPages.Remove(朗诵);
            //转向下一个标签页
            向导标签页.SelectTab("结果显示");
        }

        private void tab6下一步_Click(object sender, EventArgs e)
        {
            foreach (int i in 结果显示框.CheckedIndices)
            {
                if (结果显示框.GetItemChecked(i))
                {
                    //添加下一个标签页
                    向导标签页.TabPages.Add(朗诵);
                    //删除现在标签页
                    向导标签页.TabPages.Remove(结果显示);
                    //转向下一个标签页
                    向导标签页.SelectTab("朗诵");
                    return;
                }
            }
            MessageBox.Show("请进行选择");
        }

        private void tab7下一步_Click(object sender, EventArgs e)
        {
            //杀死自己
            Environment.Exit(0);
        }

        private void 欢迎_Click(object sender, EventArgs e)
        {

        }

        private void 欢迎词2_Click(object sender, EventArgs e)
        {

        }
    }
}
