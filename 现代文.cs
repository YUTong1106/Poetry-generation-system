using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace 依据现代文的古诗生成系统
{
    public class 现代文
    {
        public static void Deal(string fName)
        {

            //string inin = input.Text.ToString().Trim();
            string seg = "segtag -G -nn " + fName + ".txt " + fName + "_tag.txt";
            string ngr = "ngram-count -text " + fName + "_tag.txt " + "-write " + fName + "_tagcount.txt" + " -order 1";
            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";//设置要启动的应用程序名
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = false;
            p.Start();
            p.StandardInput.WriteLine("E:");
            p.StandardInput.WriteLine("cd E:\\大创\\依据现代文的古诗生成系统 遗传算法已加\\依据现代文的古诗生成系统\\依据现代文的古诗生成系统\\bin\\Debug");
            p.StandardInput.WriteLine(seg);
            p.StandardInput.WriteLine(ngr);
            p.WaitForExit(500);
            p.Close();
            InOrderRW(fName + "_tagcount.txt");
            //this.Close();
        }
        static void InOrderRW(string path)
        {
            int fflag_down = 5, fflag_up = 20;              //设置阈值
            String line;
            string inPath = path;
            string outPath = "getInOrderFrom_" + path;
            var table = new DataTable("text");
            table.Columns.Add("vocabulary", typeof(string));
            table.Columns.Add("freq", typeof(double));
            StreamReader sr = new StreamReader(inPath, Encoding.GetEncoding("utf-8"));
            while ((line = sr.ReadLine()) != null)
            {
                if ((line.Contains("n")) || (line.Contains("v")) || (line.Contains("a")))
                {
                    var strs = System.Text.RegularExpressions.Regex.Split(line, @"\s+");
                    var voc = line.Substring(0, line.IndexOf("/"));
                    int temp = Convert.ToInt32(strs[1]);
                    if ((temp <= fflag_up) && (temp >= fflag_down))
                    {
                        if (strs.Length > 1)
                        {
                            var row = table.NewRow();
                            row["vocabulary"] = voc;
                            row["freq"] = strs[1];
                            table.Rows.Add(row);
                        }
                    }
                }
            }

            StreamWriter sw = new StreamWriter(outPath, false, Encoding.GetEncoding("utf-8"));
            var tableCopy = table.Copy();
            var dv = table.DefaultView;
            dv.Sort = "freq";
            tableCopy = dv.ToTable();
            for (int i = 0; i < tableCopy.Rows.Count; i++)
            {
                string stemp = tableCopy.Rows[i]["vocabulary"].ToString();// + "  " + tableCopy.Rows[i]["freq"].ToString() ;
                sw.WriteLine(stemp);
            }
            sr.Close();
            sw.Close();
            MessageBox.Show("处理完毕！");
        }
    }
}
