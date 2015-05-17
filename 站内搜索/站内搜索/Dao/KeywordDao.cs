using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Npgsql;
using Mono.Security;

namespace 站内搜索.Dao
{
    public class KeywordDao
    {
        public IEnumerable<Model.SearchSum> GetSuggestion(string kw)
        {
            DataTable dt = SqlHelper.ExecuteDataTable(@"select top 5 Keyword,count(*) as searchcount  from keywords 
where datediff(day,searchdatetime,getdate())<7
and keyword like @keyword
group by Keyword 
order by count(*) desc", new NpgsqlParameter("@keyword", "%" + kw + "%"));

            List<Model.SearchSum> list = new List<Model.SearchSum>();
            if (dt != null && dt.Rows != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    Model.SearchSum oneModel = new Model.SearchSum();
                    oneModel.Keyword = Convert.ToString(row["keyword"]);
                    oneModel.SearchCount = Convert.ToInt32(row["SearchCount"]);
                    list.Add(oneModel);
                }
            }
            return list;
        }
        public IEnumerable<Model.SearchSum> GetHotWords()
        { 
            //缓存
            var data=HttpRuntime.Cache["hotwords"];
            if (data==null)
            {
                IEnumerable<Model.SearchSum> hotWords = DoSelect();
                HttpRuntime.Cache.Insert("hotwords",hotWords,null,DateTime.Now.AddMilliseconds(30),TimeSpan.Zero );
                return hotWords;
            }
            return (IEnumerable<Model.SearchSum>)data;
        }
        private IEnumerable<Model.SearchSum> DoSelect()
        {
          //  DataTable dt = SqlHelper.ExecuteDataTable(@"
//select top 5 Keyword,count(*) as searchcount  from keywords 
//where datediff(day,searchdatetime,getdate())<7
//group by Keyword 
//order by count(*) desc ");
            string selectSql = "select keyword,count from keywords";
            DataTable dt = SqlHelper.ExecuteDataTable(selectSql);
            List<Model.SearchSum> list = new List<Model.SearchSum>();
            if (dt!=null&&dt.Rows!=null&&dt.Rows.Count>0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    Model.SearchSum oneModel=new Model.SearchSum ();
                    oneModel.Keyword = Convert.ToString(row["keyword"]);
                    oneModel.SearchCount = Convert.ToInt32(row["count"]);
                    list.Add(oneModel);
                }
            }
            return list;
        }

        public int  Add
           (Model.SerachKeyword searchKeyword)
        {
            string sql = "INSERT INTO keywords_c (searchdatetime, keyword, clientaddress)  VALUES (@searchdatetime, @keyword, @clientaddress)";
            NpgsqlParameter[] para = new NpgsqlParameter[]
					{
						new NpgsqlParameter("@searchdatetime",searchKeyword.SearchDateTime),
						new NpgsqlParameter("@keyword", searchKeyword.Keyword),
						new NpgsqlParameter("@clientaddress", searchKeyword.ClinetAddress),
					};
            return SqlHelper.ExecuteNonQuery(sql, para);

           
        }

       
    }
}