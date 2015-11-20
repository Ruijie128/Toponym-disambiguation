using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;
using System.Text;

namespace 站内搜索
{
    public partial class space_time_data : System.Web.UI.Page
    {
        public static List<List<string>> spatial_data = new List<List<string>>();
        protected void Page_Load(object sender, EventArgs e)
        {
            selectData();
        }

        public static string selectData()
        {
            string mysqlStr = "Database=gazetteer;Data Source=127.0.0.1;User Id=root;Password=zhangruijie;pooling=false;CharSet=utf8;port=3306";
            MySqlConnection mysqlConn = new MySqlConnection(mysqlStr);
            string str = "select * from ts_spatial_data";
            mysqlConn.Open();
            MySqlCommand comm = new MySqlCommand(str, mysqlConn);
            MySqlDataAdapter adapter = new MySqlDataAdapter(comm);
            DataTable table = new DataTable();
            adapter.Fill(table);
            StringBuilder strBuilder = new StringBuilder();
            
            foreach (DataRow dr in table.Rows)
            {
                strBuilder.AppendLine("\t<tr>");
                strBuilder.AppendLine("\t\t<td>" + dr["ID"].ToString() + "</td>");
                strBuilder.AppendLine("\t\t<td>" + dr["ObjectID"].ToString() + "</td>");
                strBuilder.AppendLine("\t\t<td>" + dr["ObjectName"].ToString() + "</td>");
                strBuilder.AppendLine("\t\t<td>" + dr["DateTime"].ToString() + "</td>");
                strBuilder.AppendLine("\t\t<td>" + dr["DateTime2"].ToString() + "</td>");
                strBuilder.AppendLine("\t\t<td>" + dr["ToDateTime"].ToString() + "</td>");
                strBuilder.AppendLine("\t\t<td>" + dr["TimeFlag"].ToString() + "</td>");
                strBuilder.AppendLine("\t\t<td>" + dr["PublishTime"].ToString() + "</td>");
                strBuilder.AppendLine("\t\t<td>" + dr["FeatureID"].ToString() + "</td>");
                strBuilder.AppendLine("\t\t<td>" + dr["FeatureName"].ToString() + "</td>");
                strBuilder.AppendLine("\t\t<td>" + dr["Movement"].ToString() + "</td>");
                strBuilder.AppendLine("\t\t<td>" + dr["MovementName"].ToString() + "</td>");
                strBuilder.AppendLine("\t\t<td>" + dr["EventID"].ToString() + "</td>");
                strBuilder.AppendLine("\t\t<td>" + dr["EventName"].ToString() + "</td>");
                strBuilder.AppendLine("\t\t<td>" + dr["SourceID"].ToString() + "</td>");
                strBuilder.AppendLine("\t\t<td>" + dr["Source"].ToString() + "</td>");
                strBuilder.AppendLine("\t\t<td>" + dr["PreID"].ToString() + "</td>");
                strBuilder.AppendLine("\t\t<td>" + dr["Text"].ToString() + "</td>");
                strBuilder.AppendLine("\t\t<td>" + dr["anchaorFlag"].ToString() + "</td>");
                strBuilder.AppendLine("</tr>");
            }
             
            mysqlConn.Close();
            return strBuilder.ToString();
        }
    }
}