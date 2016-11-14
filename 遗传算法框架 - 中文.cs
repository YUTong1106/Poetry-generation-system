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

namespace 遗传算法框架
{
    public class 遗传算法框架
    {
        public static GeneticAlgorithm G;
        private static char[] hanziBase;
        public static char[] HanziBase
        {
            set { hanziBase = value; }
            get { return hanziBase; }
        }
        /// <summary>
        /// 确定诗句的平仄
        /// 1：仄起正格
        /// 2：平起正格
        /// 3：仄起偏格
        /// 4：平起偏格
        /// </summary>
        public static int PingZeCheck;

        public static int YunBu;


    }
}
