using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using 站内搜索.data;
using 站内搜索.db;
using System.IO;
using MySql.Data.MySqlClient;

namespace 站内搜索
{
    public partial class graph : System.Web.UI.Page
    {
        public static List<pieData> pie_data = new List<pieData>();
        public int count;
        public static List<List<double>> pointList = new List<List<double>>();
        protected void Page_Load(object sender, EventArgs e)
        {
            readLocation();
            columnFunction();

            count = pie_data.Count;
        }

        // 创建柱形图
        public static void columnFunction()
        {
            var selectPlacesStr = "select FeatureName from ts_spatial_data order by FeatureName";
            ArrayList placeList = dbFunction.SelectFromDB(selectPlacesStr);

            for (int i = 0; i < placeList.Count; i++)
            {
                pieData temp = new pieData();
                temp.value = 1;
                temp.name = placeList[i].ToString();
                pie_data.Add(temp);
            }
            pie_data = caculateList(pie_data);
        }


        // 对list 进行统计
        public static List<pieData> caculateList(List<pieData> list)
        {
            List<pieData> temp = new List<pieData>();

            for (int i = 0; i < list.Count; i++)
            {
                for (int j = i + 1; j < list.Count; j++)
                {
                    if (list[i].name.Equals(list[j].name))
                    {
                        list[i].value++;
                        list.RemoveAt(j);
                        j--;
                    }
                }
            }
            return removeNull(list);
        }

        // 去除list null
        public static List<pieData> removeNull(List<pieData> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].name.Equals(""))
                    list.RemoveAt(i);
            }
            return list;
        }

        // 读取经纬度数据
        public static void readLocation()
        {
            MySqlConnection mysqlConn = new MySqlConnection("Database=gazetteer;Data Source=127.0.0.1;User Id=root;Password=zhangruijie;pooling=false;CharSet=utf8;port=3306");
            mysqlConn.Open();
            string sqlStr = "select lat, lon, count from location  " ;
            MySqlCommand comm = new MySqlCommand(sqlStr, mysqlConn);
            MySqlDataReader read = comm.ExecuteReader();
            while (read.Read())
            {
                List<double> point = new List<double>();
                point.Add(Convert.ToDouble(read[1]));
                point.Add(Convert.ToDouble(read[0]));
                point.Add(Convert.ToInt32(read[2]));
                pointList.Add(point);
                // targetResultCount = Convert.ToInt32(read[2]);
            }
            mysqlConn.Close();
        }
    }
}