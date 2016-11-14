using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;

namespace 遗传算法框架
{
    public class GeneticAlgorithm
    {
        /// <summary>
        /// 每一代字符串的数目
        /// </summary>
        private const int Max = 500;
        /// <summary>
        /// 每个字符串包含的字符数，五言，count=20
        /// </summary>
        private const int count = 20;
        /// <summary>
        /// 保持最优解不变的代数，过了这个值算法结束
        /// </summary>
        private const int lastTimes = 1;
        /// <summary>
        /// 语言模型的权重。语言模型的生成值在0-120之间，一般30多
        /// </summary>
        private const int weight1 = 1;
        /// <summary>
        /// 共现率互信息的权重。值在400左右
        /// </summary>
        private const int weight2 = 1;
        /// <summary>
        /// 获取数据库的位置
        /// </summary>
        private const string DBstring = @"作诗.accdb";
        private AccessDB ac;
        /// <summary>
        /// 平仄表
        /// </summary>
        private char[] PingZeTable;
        /// <summary>
        /// 初始化字符串
        /// </summary>
        public string[] Initsentences = new string[Max];
        /// <summary>
        /// 当代
        /// </summary>
        private string[] sentences = new string[Max];
        /// <summary>
        /// 下一代
        /// </summary>
        private string[] sentencesNext = new string[Max];
        /// <summary>
        /// 适应性，适应性是累加的，单独一句的适应性要通过和上一句的适应性相减算出来
        /// </summary>
        private double[] adaptability = new double[Max];
        /// <summary>
        /// 本代最佳适应性
        /// </summary>
        private double best;
        /// <summary>
        /// 下一代最佳适应性
        /// </summary>
        private double bestLast;
        /// <summary>
        /// 本代适应性的和
        /// </summary>
        private double sum_adapt = 0;
        /// <summary>
        /// 上一代适应性的和
        /// </summary>
        private double sum_adapt_last = 0;
        /// <summary>
        /// 记录演化了多少代
        /// </summary>
        public long gen;
        /// <summary>
        /// 初始化以及演化时需要的随机数
        /// </summary>
        private Random rnd = new Random();
        /// <summary>
        /// 突变率，百分数
        /// </summary>
        private const int change = 50;
        /// <summary>
        /// 汉字库可容纳的汉字数
        /// </summary>
        private int hanziNum;
        public int HanziNum
        {
            get { return hanziNum; }
            set { hanziNum = value; }
        }
        /// <summary>
        /// 汉字库
        /// </summary>
        private char[] hanziBase;
        /// <summary>
        /// 汉字库
        /// </summary>
        public char[] HanziBase
        {
            get { return hanziBase; }
            set { hanziBase = value; }
        }


        /// <summary>
        /// 给定汉字和位置，检查是否符合平仄要求
        /// </summary>
        /// <param name="letter">给定的汉字</param>
        /// <param name="pos">汉字的位置[0-19]</param>
        /// <returns></returns>
        private bool PingZePositionCheck(char letter, int pos)
        {
            string s = ac.excuteScalar("SELECT 平仄 FROM 广韵_简体字表zxl_平仄韵 WHERE 字 = \"" + letter.ToString() + "\";");
            try
            {
                if (s[0] == PingZeTable[pos] || s[0] == 'a')
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            //有的字没有收录在平仄表中，放弃这个字
            catch (IndexOutOfRangeException e)
            {
                return false;
            }
        }


        private bool YunBuCheck(char letter)
        {
            string s = ac.excuteScalar("SELECT 平水韵ID列表 FROM 广韵_简体字表zxl_平仄韵 WHERE 字 = \"" + letter.ToString() + "\";");
            if (s == "")
                return false;
            //去掉首尾的分号
            s = s.Trim(';');
            //把韵部的编号提取出来
            string[] ss = s.Split(';');
            //得到用户选择的韵部
            string Yuns = 遗传算法框架.YunBu.ToString();
            //比较韵部
            foreach (string i in ss)
            {
                //匹配到，返回真
                if (Yuns == i)
                    return true;
            }
            return false;
        }


        /// <summary>
        /// 随机生成汉字字符串
        /// </summary>
        public void Initialize()
        {
            for (int i = 0; i < Max; ++i)
            {
                char[] letters = new char[count];
                //从汉字库生成随机汉字
                for (int j = 0; j < count; j++)
                {
                    int letter = RandomNumber(rnd, 0, hanziNum - 1);
                    //正格
                    if (遗传算法框架.PingZeCheck == 1 || 遗传算法框架.PingZeCheck == 2)
                    {
                        //第一、二、四末尾押韵
                        if (j == 4 || j == 9 || j == 19)
                        {
                            //押韵通过
                            if (YunBuCheck(hanziBase[letter]))
                            {
                                letters[j] = hanziBase[letter];
                            }
                            else
                            {
                                j--;
                                continue;
                            }
                        }
                        //其余位置满足平仄
                        else
                        {
                            if (PingZePositionCheck(hanziBase[letter], j))
                            {
                                letters[j] = hanziBase[letter];
                            }
                            else
                            {
                                j--;
                                continue;
                            }
                        }
                    }
                    //偏格
                    else
                    {
                        //第二、四末尾押韵
                        if (j == 9 || j == 19)
                        {
                            //押韵通过
                            if (YunBuCheck(hanziBase[letter]))
                            {
                                letters[j] = hanziBase[letter];
                            }
                            else
                            {
                                j--;
                                continue;
                            }
                        }
                        //其余位置满足平仄
                        else
                        {
                            if (PingZePositionCheck(hanziBase[letter], j))
                            {
                                letters[j] = hanziBase[letter];
                            }
                            else
                            {
                                j--;
                                continue;
                            }
                        }
                    }
                }
                //第i个汉字字符串（诗句）生成完成
                sentences[i] = new String(letters);
            }
        }//Initialize
        /// <summary>
        /// 生成大于等于up，小于等于down之间的随机数
        /// </summary>
        /// <param name="rnd"></param>
        /// <param name="up"></param>
        /// <param name="down"></param>
        /// <returns></returns>
        private int RandomNumber(Random rnd, int up, int down)
        {
            return rnd.Next(up, down + 1);
        }//RandomNumber
        /// <summary>
        /// 评价适应性
        /// </summary>
        private void Evaluate()
        {
            //adaptability是每个句子的权重叠加，方便之后的轮盘赌
            adaptability[0] = 0;
            //-------------------------------------------------------------------
            //-------------------------------------------------------------------
            //----------------------下面是语言模型部分-----------------------------
            //-------------------------------------------------------------------
            //-------------------------------------------------------------------
            double[] adapt1 = new double[Max];//语言模型的各个适应度
            StreamWriter wtext = new StreamWriter("text.txt", false, Encoding.GetEncoding("gb2312"));
            //个体句子循环
            for (int i = 0; i < Max; ++i)
            {
                //把每个句子输出到文本，字之间用空格隔开，每句一行
                for (int j = 0; j < 4; ++j)
                {
                    //拆开，5句一行
                    for (int k = 5 * j; k < (5 * j) + 5; ++k)
                    {
                        wtext.Write(sentences[i][k]);
                        wtext.Write(" ");
                    }
                    wtext.WriteLine();
                }
                //wtext.WriteLine(sentences[i][19]);
            }//for
            wtext.Close();
            //以下开始计算text.txt中文本的适应性，保存到文件writer.txt
            StreamWriter sw = new StreamWriter("writer.txt", false, Encoding.GetEncoding("gb2312")); //利用输出重定向截获计算结果并写文档！

            System.Diagnostics.Process process = new System.Diagnostics.Process();
            string exe_path = @".";
            process.StartInfo.FileName = exe_path + "\\ngram.exe";
            process.StartInfo.WorkingDirectory = exe_path;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.StartInfo.RedirectStandardOutput = true;  //输出重定向
            process.StartInfo.UseShellExecute = false;         //输出重定向时，必须的
            //sOrder:几元，可以是2或者3。lmfile：语言模型文件，用LMzi_6DL.lm sDebug取1
            string sOrder = "2";
            string LMFile = "LMzi_6DL.lm";
            string sDebug = "1";
            process.StartInfo.Arguments = " -ppl text.txt -order " + sOrder + " -lm " + LMFile + " -debug " + sDebug;  //无法直接使用DOS命令中的"> text_ppl.txt"来获取计算结果

            process.Start();

            System.IO.StreamReader reader = process.StandardOutput; // 截取输出流 
            string line = reader.ReadLine(); // 每次读取一行 
            while (!reader.EndOfStream)
            {
                sw.Write(line + "\n");  //利用输出重定向截获计算结果并写文档！
                line = reader.ReadLine();
            }
            sw.Write(line + "\n");  //最后一行
            process.WaitForExit();
            process.Close();
            sw.Close();
            //计算适应性完毕

            //以下开始读取writer.txt中的适应性
            //四个一组，装到adaptability里
            StreamReader adpr = new StreamReader("writer.txt");
            string adpLine;
            int adpi = 0;
            double adp = 0;
            int in_group = 0;
            while ((adpLine = adpr.ReadLine()) != null)
            {
                //循环结束条件：writer开始总结
                if (adpLine.IndexOf("file text.txt") != -1)
                    break;
                //寻找适应性数值标志
                int adpPos = adpLine.IndexOf("logprob= ");
                //没有在该行找到适应性数值，就寻找下一行
                if (adpPos == -1)
                    continue;
                //截取适应性数值中的数字部分
                //adpPos为适应性数字开始位置
                adpPos = adpPos + 9;
                //adpEnd为适应性数字结束位置
                int adpEnd = adpLine.IndexOf(" ", adpPos);
                adpLine = adpLine.Substring(adpPos, adpEnd - adpPos);
                adp = double.Parse(adpLine);
                //adp是负数，一般不会小于-30，需要转为正数并且数值越大表示越好。所以加一个常数30.
                adp += 30;
                if (adp < 0)
                    adp = 0;
                //因为拆成4句，所以合并装入adapt1
                if (in_group == 0 || in_group >= 4)
                {
                    adapt1[adpi] = adp;
                    in_group = 1;
                    adpi++;
                }
                else
                {
                    adapt1[adpi - 1] += adp;
                    in_group++;
                }
            }
            adpr.Close();
            //------------------------------------------------------------
            //------------------------------------------------------------
            //----------------------上面是语言模型部分----------------------
            //------------------------------------------------------------
            //------------------------下面是共现部分-----------------------
            //------------------------------------------------------------
            //------------------------------------------------------------
            double[] adapt2 = new double[Max];
            try
            {
                if (!ac.FindTable("单联字共现频度_6库汇总"))
                {
                    throw new Exception("没有单联字共现频度_6库汇总表");
                }
                if (!(ac.FindField("单联字共现频度_6库汇总", "字1")
                    && ac.FindField("单联字共现频度_6库汇总", "字2")
                    && ac.FindField("单联字共现频度_6库汇总", "互信息I")))
                {
                    throw new Exception("表信息不全");
                }
                for (int sentencei = 0; sentencei < Max; ++sentencei)
                {
                    double adp2 = 0.0;
                    for (int i = 0; i < 10; ++i)
                    {
                        char word1 = sentences[sentencei][i];
                        for (int j = i + 1; j < 10; ++j)
                        {
                            char word2 = sentences[sentencei][j];
                            string adpstr = ac.excuteScalar("SELECT 互信息I FROM 单联字共现频度_6库汇总 WHERE 字1 = \"" + word1.ToString() + "\" AND 字2 = \"" + word2.ToString() + "\";");
                            if (adpstr == "")
                                continue;
                            adp2 += double.Parse(adpstr);
                        }
                    }
                    for (int i = 10; i < 20; ++i)
                    {
                        char word1 = sentences[sentencei][i];
                        for (int j = i + 1; j < 20; ++j)
                        {
                            char word2 = sentences[sentencei][j];
                            string adpstr = ac.excuteScalar("SELECT 互信息I FROM 单联字共现频度_6库汇总 WHERE 字1 = \"" + word1.ToString() + "\" AND 字2 = \"" + word2.ToString() + "\";");
                            if (adpstr == "")
                                continue;
                            adp2 += double.Parse(adpstr);
                        }
                    }
                    adapt2[sentencei] = adp2;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            //-------------------------------------------------------------
            //-------------------------------------------------------------
            //-------------------上面是共现部分-----------------------------
            //-------------------------------------------------------------
            //--------------------下面是合并--------------------------------
            //-------------------------------------------------------------
            //-------------------------------------------------------------
            adaptability[0] = 0;
            for (int i = 0; i < Max; i++)
            {
                if (i == 0)
                {
                    adaptability[i] = weight1 * adapt1[i] + weight2 * adapt2[i];
                }
                else
                {
                    adaptability[i] = adaptability[i - 1] + weight1 * adapt1[i] + weight2 * adapt2[i];
                }
            }
        }//Evaluate
        /// <summary>
        /// 精英主义，返回下一代已生成的数+1
        /// </summary>
        /// <returns>返回下一代已生成的数+1</returns>
        private int Elitism()
        {
            double[] temp = new double[Max + 1];
            int[] num = new int[Max];
            int i, j;
            bestLast = best;
            //先找到适应性最好的那个（擂台法）
            for (i = 0; i < Max; ++i)
            {
                if (i == 0)
                {
                    temp[0] = adaptability[0];
                    temp[Max] = adaptability[0];
                }
                else
                {
                    temp[i] = adaptability[i] - adaptability[i - 1];
                    if (temp[Max] < temp[i])
                        temp[Max] = temp[i];
                }
            }//for
            best = temp[Max];
            for (i = 0, j = 0; i < Max; ++i)
            {
                if (temp[i] == temp[Max])
                {
                    char[] letters = new char[count];
                    for (int k = 0; k < count; ++k)
                    {
                        letters[k] = sentences[i][k];
                    }
                    sentencesNext[j] = new String(letters);//直接进入下一代
                    ++j;
                }
            }
            return j;
        }//Elitism
        /// <summary>
        /// 交叉和变异
        /// </summary>
        /// <param name="start"></param>
        private void Cross_n_Mutation(int start)
        {
            int i = start;
            while (i < Max)
            {
                int fit1 = RandomNumber(rnd, 0, (int)adaptability[Max - 1]);
                int fit2 = RandomNumber(rnd, 0, (int)adaptability[Max - 1]);
                int num1 = 0, num2 = 0;
                for (int j = 0, limit1 = 0, limit2 = 0; j < Max; ++j)//轮盘赌
                {
                    if (fit1 < adaptability[j] && limit1 == 0)
                    {
                        num1 = j;
                        limit1 = 1;
                    }
                    if (fit2 < adaptability[j] && limit2 == 0)
                    {
                        num2 = j;
                        limit2 = 1;
                    }
                    if (limit1 == 1 && limit2 == 1)
                        break;
                }//for
                int cp = RandomNumber(rnd, 0, count + 1);
                char[] letters = new char[count];
                for (int j = 0; j < count; ++j)//交叉，每个父母每次只生一个孩子
                {
                    if (j < cp)
                        letters[j] = sentences[num1][j];
                    else
                        letters[j] = sentences[num2][j];
                }//for
                for (int j = 0; j < count; ++j)//变异，每位有一定概率变异
                {
                    int prop = RandomNumber(rnd, 0, 100);
                    if (prop < change)//确定变异
                    {
                        int cletter = RandomNumber(rnd, 0, hanziNum - 1);
                        if (遗传算法框架.PingZeCheck == 1 || 遗传算法框架.PingZeCheck == 2)
                        {
                            //第一、二、四末尾押韵
                            if (j == 4 || j == 9 || j == 19)
                            {
                                //押韵通过
                                while (!(YunBuCheck(hanziBase[cletter])))
                                {
                                    cletter = RandomNumber(rnd, 0, hanziNum - 1);
                                }
                            }
                            //其余位置满足平仄
                            else
                            {
                                while (!(PingZePositionCheck(hanziBase[cletter], j)))
                                {
                                    cletter = RandomNumber(rnd, 0, hanziNum - 1);
                                }
                            }
                        }
                        //偏格
                        else
                        {
                            //第二、四末尾押韵
                            if (j == 9 || j == 19)
                            {
                                //押韵通过
                                while (!(YunBuCheck(hanziBase[cletter])))
                                {
                                    cletter = RandomNumber(rnd, 0, hanziNum - 1);
                                }
                            }
                            //其余位置满足平仄
                            else
                            {
                                while (!(PingZePositionCheck(hanziBase[cletter], j)))
                                {
                                    cletter = RandomNumber(rnd, 0, hanziNum - 1);
                                }
                            }
                        }
                        letters[j] = hanziBase[cletter];
                    }//if
                }//for
                sentencesNext[i] = new String(letters);
                ++i;
            }//while
            for (i = 0; i < Max; ++i)
            {
                char[] letters = new char[count];
                for (int j = 0; j < count; ++j)
                {
                    letters[j] = sentencesNext[i][j];
                }
                sentences[i] = new String(letters);
            }
        }
        /// <summary>
        /// 计算两代之间变化率
        /// </summary>
        /// <returns></returns>
        private double adapt_changed_percent()
        {
            double temp1 = sum_adapt - sum_adapt_last;
            double temp = temp1 / bestLast;
            return temp;
        }//adapt_changed_percent


        /// <summary>
        /// 初始化平仄表
        /// </summary>
        private void InitializePingZeTable()
        {
            //仄起正格
            if (遗传算法框架.PingZeCheck == 1)
            {
                PingZeTable = new char[] {'z', 'z', 'z', 'p', 'p', 
                'p', 'p', 'z', 'z', 'p', 
                'p', 'p', 'p', 'z', 'z', 
                'z', 'z', 'z', 'p', 'p'};
            }
            //平起正格
            else if (遗传算法框架.PingZeCheck == 2)
            {
                PingZeTable = new char[] {'p', 'p', 'z', 'z', 'p', 
                'z', 'z', 'z', 'p', 'p', 
                'z', 'z', 'p', 'p', 'z', 
                'p', 'p', 'z', 'z', 'p'};
            }
            //仄起偏格
            else if (遗传算法框架.PingZeCheck == 3)
            {
                PingZeTable = new char[] {'z', 'z', 'p', 'p', 'z', 
                'p', 'p', 'z', 'z', 'p', 
                'p', 'p', 'p', 'z', 'z', 
                'z', 'z', 'z', 'p', 'p'};
            }
            //平起偏格
            else if (遗传算法框架.PingZeCheck == 4)
            {
                PingZeTable = new char[] {'p', 'p', 'p', 'z', 'z', 
                'z', 'z', 'z', 'p', 'p', 
                'z', 'z', 'p', 'p', 'z', 
                'p', 'p', 'z', 'z', 'p'};
            }
            else
            {
                throw new Exception("平仄格式错误");
            }
        }


        /// <summary>
        /// 从这里开始
        /// </summary>
        /// <returns>遗传算法最后一代的字符串数组</returns>
        public string[] GeneAlgorithm()
        {

            ac = new AccessDB(DBstring);

            int times = 0;
            //导入汉字库
            hanziBase = 遗传算法框架.HanziBase;

            //初始化平仄表
            InitializePingZeTable();

            //初始化诗句
            Initialize();
            //把sentences里的内容copy到Initsentences里
            for (int i = 0; i < Max; ++i)
            {
                char[] letters = new char[count];
                for (int j = 0; j < count; ++j)
                {
                    letters[j] = sentences[i][j];
                }
                Initsentences[i] = new String(letters);
            }
            double percent;
            
            do
            {
                gen++;
                sum_adapt_last = adaptability[Max - 1];
                Evaluate();
                sum_adapt = adaptability[Max - 1];
                StreamWriter logw = new StreamWriter("log.txt", true, Encoding.GetEncoding("GB2312"));
                logw.WriteLine((gen).ToString());
                logw.WriteLine(sum_adapt.ToString());
                logw.Close();
                int start = Elitism();
                Cross_n_Mutation(start);
                percent = adapt_changed_percent();
                if (System.Math.Abs(percent) < 0.01)
                    times++;
                else
                    times = 0;
                //输出到文本文件
                if (File.Exists("诗.txt"))
                {
                    File.Delete("诗.txt");
                }
                StreamWriter sw = new StreamWriter("诗.txt", true, Encoding.GetEncoding("GB2312"));
                for (int i = 0; i < 100; ++i)
                {
                    sw.WriteLine(sentences[i]);
                }
                sw.Close();
                Application.DoEvents();
            } while (times < lastTimes);
            
            return sentences;
        }//GeneAlgorithm
    }
}
