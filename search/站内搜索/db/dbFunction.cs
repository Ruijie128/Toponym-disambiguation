using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using System.Collections;

namespace 站内搜索.db
{
    public class dbFunction
    {
        public static string mysqlStr = "Database=gazetteer;Data Source=127.0.0.1;User Id=root;Password=zhangruijie;pooling=false;CharSet=utf8;port=3306";
        public static MySqlConnection mysqlConn = new MySqlConnection(mysqlStr);



        public static ArrayList SelectFromDB(string sqlStr)
        {
            mysqlConn.Open();
            ArrayList list = new ArrayList();
            MySqlCommand mysqlComm = new MySqlCommand(sqlStr, mysqlConn);
            MySqlDataReader reader = mysqlComm.ExecuteReader();
            while (reader.Read())
            {
                list.Add(reader[0]);
            }
            reader.Close();
            mysqlConn.Close();
            return list;
        }
    }
}