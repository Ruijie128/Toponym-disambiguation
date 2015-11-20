using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;

namespace 站内搜索.Test
{
    public class DateViewer
    {
        // 在一段时间内 对新闻文本进行展示
        public static List<SearchResult> CaculateCount(DateTime fromTime, DateTime toTime, List<SearchResult> list)
        {
          //  fromTime = DateTime.Parse("2010-01-01");
        //    toTime = DateTime.Parse("2010-12-31");

            //首先找到在目标日期中的所有新闻文本
            List<SearchResult> targetList = new List<SearchResult>();
            for (int i = 0; i < list.Count; i++)
            {
                if ((DateTime.Parse(list[i].Date) > fromTime) && (DateTime.Parse(list[i].Date) < toTime))
                {
                    targetList.Add(list[i]);
                }
            }

            return targetList;
        }

        // 按照月份进行分割，计算每个月份的新闻文本数目
        public static void newsCountForMonth(List<SearchResult> list)
        {
            List<int> dateList = new List<int>();
            for(int i=0;i<list.Count;i++)
            {
                DateTime dt = DateTime.Parse(list[i].Date);
                dateList.Add(Convert.ToInt32(dt.Month));
            }
            List<int> newsCount = new List<int>();
            for (int i = 1; i < 13; i++)
            {
               // int month = i;
                newsCount.Add(dateList.Count(month => month == i));
            }
            writeIntoFiles(newsCount);
        }

        //将数据写入.txv 文件以画图
        public static void writeIntoFiles(List<int> list)
        {
            string header = "month	number";
            FileStream fs = new FileStream(@"E:\users\zhang Ruijie\codes\Toponym-disambiguation\search\站内搜索\data.tsv", FileMode.Create);
            StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
            string january = "1	"+list[0].ToString();
            string february = "2	" + list[1].ToString();
            string march = "3	" + list[2].ToString();
            string april = "4	" + list[3].ToString();
            string may = "5	" + list[4].ToString();
            string june = "6	" + list[5].ToString();
            string july = "7	" + list[6].ToString();
            string august = "8	" + list[7].ToString();
            string september = "9	" + list[8].ToString();
            string october = "10	" + list[9].ToString();
            string november = "11	" + list[10].ToString();
            string december = "12	" + list[11].ToString();
            sw.WriteLine(header);
            sw.WriteLine(january);
            sw.WriteLine(february);
            sw.WriteLine(march);
            sw.WriteLine(april);
            sw.WriteLine(may);
            sw.WriteLine(june);
            sw.WriteLine(july);
            sw.WriteLine(august);
            sw.WriteLine(september);
            sw.WriteLine(october);
            sw.WriteLine(november);
            sw.WriteLine(december);
            sw.Close();
            fs.Close();
        }
    }
}