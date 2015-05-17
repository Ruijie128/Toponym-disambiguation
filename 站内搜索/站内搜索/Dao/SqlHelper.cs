using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using Npgsql;
using Mono.Security;

namespace 站内搜索.Dao
{
    class SqlHelper
    {
        public static readonly string connstr =
            ConfigurationManager.ConnectionStrings["dbconnStr"].ConnectionString;

        public static int ExecuteNonQuery(string cmdText,
            params NpgsqlParameter[] parameters)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(connstr))
            {
                conn.Open();
                using (NpgsqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = cmdText;
                    cmd.Parameters.AddRange(parameters);
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        public static object ExecuteScalar(string cmdText,
            params NpgsqlParameter[] parameters)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(connstr))
            {
                conn.Open();
                using (NpgsqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = cmdText;
                    cmd.Parameters.AddRange(parameters);
                    return cmd.ExecuteScalar();
                }
            }
        }

        public static DataTable ExecuteDataTable(string cmdText,
            params NpgsqlParameter[] parameters)
        {
         //   string tempstr = "Server=localhost;Port=5432;UserId=postgres;Password=ZHANGRUIJIE;Database=websitedata;";
            using (NpgsqlConnection conn = new NpgsqlConnection(connstr))
            {
              //  conn.Close();
                conn.Open();
                using (NpgsqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = cmdText;
                    cmd.Parameters.AddRange(parameters);
                    using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        
                        adapter.Fill(dt);
                        return dt;
                    }
                }
            }
        }

        public static NpgsqlDataReader ExecuteDataReader(string cmdText,
            params NpgsqlParameter[] parameters)
        {
            NpgsqlConnection conn = new NpgsqlConnection(connstr);
            conn.Open();
            using (NpgsqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = cmdText;
                cmd.Parameters.AddRange(parameters);
                return cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
        }

        public static int ExecuteStoredProcedure(string procName,
            params NpgsqlParameter[] parameters)
        {
            NpgsqlConnection conn = new NpgsqlConnection(connstr);
            conn.Open();
            using (NpgsqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = procName;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddRange(parameters);
                return cmd.ExecuteNonQuery();
            }
        }
    }
}
