using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using Lucene.Net.Store;
using System.IO;
using Lucene.Net.Index;

using Lucene.Net.Analysis.PanGu;
using System.Net;
using System.Text;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using mshtml;
using Lucene.Net.Documents;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using 站内搜索.Test;
using PanGu;
using Lucene.Net.Analysis;
using System.Data;
using MySql.Data.MySqlClient;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace 站内搜索
{
    public partial class CreateIndex : System.Web.UI.Page
    {
        private ILog logger = LogManager.GetLogger(typeof(CreateIndex));
        public string kw = string.Empty;
        public int count = 0;//文档数目
        //RenderToHTML为输出的分页控件<a>..<a>
        protected string RenderToHTML { get; set; }
        public class DataJson
        {
            public struct Data
            {
                public string name;
                public int data_count;
                public string geocodes8;
                public string barygeocode8;
            }
            public Data data; 
            public string status;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            //时间控件
            myRegisterTime.Attributes.Add("onfocus", "javascript:dateSelector()");
            //加载热词

            hotwordsRepeater.DataSource = new Dao.KeywordDao().GetHotWords();
            hotwordsRepeater.DataBind();
            kw = Request["kw"];
            if (string.IsNullOrWhiteSpace(kw))
            {
                return;
            }
            //处理：将用户的搜索记录加入数据库，方便统计热词
            Model.SerachKeyword model = new Model.SerachKeyword();
            model.Keyword = kw;
            model.SearchDateTime = DateTime.Now;
            model.ClinetAddress = Request.UserHostAddress;

            new Dao.KeywordDao().Add(model);
            //分页控件
            MyPage pager = new MyPage();
            pager.TryParseCurrentPageIndex(Request["pagenum"]);
            //超链接href属性
            pager.UrlFormat = "CreateIndex.aspx?pagenum={n}&kw=" + Server.UrlEncode(kw);
            
            int startRowIndex = (pager.CurrentPageIndex - 1) * pager.PageSize;


            int totalCount = -1;
        //    index(); //暂时注释  不建立索引
            List<SearchResult> list = search(kw);
            show_place();

            List<SearchResult> targetList = Test.DateViewer.CaculateCount(DateTime.Parse("2009-01-01"), DateTime.Parse("2010-01-01"), list);
            Test.DateViewer.newsCountForMonth(targetList);
          //  List<SearchResult> list = DoSearch(startRowIndex,pager.S,out totalCount);
            pager.TotalCount = 5;
            RenderToHTML = pager.RenderToHTML();
            dataRepeater.DataSource = list;
            dataRepeater.DataBind();
        }

        protected void Calendar1_SelectionChanged(object sender, EventArgs e)
        {

            myRegisterTime.Text = Calendar1.SelectedDate.ToString("yyyy-MM-dd");

        }

        private void show_place()
        {
            DataJson daJson = new DataJson();
            daJson.data.name = kw.ToString();
            daJson.data.data_count = count;
            MySqlConnection mysql = new MySqlConnection("Database=gazetteer;Data Source=127.0.0.1;User Id=root;Password=zhangruijie;pooling=false;CharSet=utf8;port=3306");
            string sqlStr = "select geocodes8,barygeocode8 from show_place where name = '" + kw+"'";
            mysql.Open();
            MySqlCommand comm = new MySqlCommand(sqlStr, mysql);
            MySqlDataReader reader = comm.ExecuteReader();
            if (reader.Read())
            {
                daJson.data.geocodes8 = reader[0].ToString();
                daJson.data.barygeocode8 = reader[1].ToString();
            }

            mysql.Close();
            daJson.status = "ok";

          //  string sw = "{ \n" +"data:"+"{"+"name:"+das+"}"+ "\n }";
            string text = JsonConvert.SerializeObject(daJson);
            FileStream fs = new FileStream(@"E:\users\zhang Ruijie\codes\Toponym-disambiguation\search\站内搜索\jason.txt", FileMode.Create);
            StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
            sw.Write(text);
            sw.Close();
            fs.Close();
        }

        private List<SearchResult> search(string content) 
        {
            string indexPath = "E:/Index";
            //Directory directory = FSDirectory.Open(new File(indexPath));
            FSDirectory directory = FSDirectory.Open(new DirectoryInfo(indexPath), new NoLockFactory());
           /* bool isUpdate = IndexReader.IndexExists(indexPath);
            if (isUpdate)
            {
                if (IndexWriter.IsLocked(indexPath))
                {
                    IndexWriter.Unlock(indexPath);
                }
            }*/
       //     IndexReader reader = IndexReader.Open(directory, true);
            IndexReader reader = DirectoryReader.Open(directory);
            IndexSearcher search = new IndexSearcher(reader);
            QueryParser parser = new QueryParser(Lucene.Net.Util.Version.LUCENE_29,"content", new PanGuAnalyzer());
            Query query = parser.Parse(content);

            TopDocs topdocs = search.Search(query, 1000);
            ScoreDoc[] scoreDocs = topdocs.scoreDocs;
            List<SearchResult> list = new List<SearchResult>();
            logger.Debug("查询结果总数---" + topdocs.totalHits + "  最大的评分--" + topdocs.GetMaxScore());
            count = topdocs.totalHits;
         //   Console.WriteLine("查询结果总数---" + topdocs.totalHits + "  最大的评分--" + topdocs.GetMaxScore());
            for (int i = 0; i < scoreDocs.Length; i++)
            {
                int doc = scoreDocs[i].doc;
                Document document = search.Doc(doc);
               // Console.WriteLine("id--" + scoreDocs[i].doc + "---scors--" + scoreDocs[i].score + "---uri--" + document.Get("link"));
                string number = scoreDocs[i].doc.ToString();
                string score = scoreDocs[i].score.ToString();
                string uri = document.Get("link");
                string date = document.Get("publishtime").ToString();
                string title = "标题：" + document.Get("title").ToString() + document.Get("publishtime").ToString();
              //  SearchResult searcher = new SearchResult() { Number = number, Score = score, BodyPreview = Preview(body, kw) };
                SearchResult searcher = new SearchResult() { Number = number, Score = score, Uri = uri, Title = title, Date = date };
                list.Add(searcher);
            }
            
           
           
            reader.Close();
            return list;
        }
        private List<SearchResult> DoSearch(int startRowIndex,int pageSize,out int totalCount)
        {
            string indexPath = "E:/Index";
            FSDirectory directory = FSDirectory.Open(new DirectoryInfo(indexPath), new NoLockFactory());
            IndexReader reader = IndexReader.Open(directory, true);
            //IndexSearcher是进行搜索的类
            IndexSearcher searcher = new IndexSearcher(reader);
            PhraseQuery query = new PhraseQuery();
            
            foreach (string word in CommonHelper.SplitWord(kw))
            {
                query.Add(new Term("body", word));
            }
            query.SetSlop(100);//相聚100以内才算是查询到
            TopScoreDocCollector collector = TopScoreDocCollector.create(1024, true);//最大1024条记录
            searcher.Search(query, null, collector);
            totalCount = collector.GetTotalHits();//返回总条数
            ScoreDoc[] docs = collector.TopDocs(startRowIndex, pageSize).scoreDocs;//分页,下标应该从0开始吧，0是第一条记录
            List<SearchResult> list = new List<SearchResult>();
            for (int i = 0; i < docs.Length; i++)
            {
                int docID = docs[i].doc;//取文档的编号，这个是主键，lucene.net分配
                //检索结果中只有文档的id，如果要取Document，则需要Doc再去取
                //降低内容占用
                Document doc = searcher.Doc(docID);
                string number = doc.Get("number");
                string title = doc.Get("title");
                string body = doc.Get("body");

                SearchResult searchResult = new SearchResult() { Number = number, Score = title, Uri = Preview(body, kw) };
                list.Add(searchResult);

            }
            return list;
        }


        protected void index()
        {
            //索引库的位置
            logger.Debug("开始建立索引");
            string indexPath = "E:/index";
            Analyzer analyzer = new PanGuAnalyzer();
            IndexWriter indexwriter = new IndexWriter(indexPath, analyzer, true);
            string conn = "Server=localhost;Port=5432;UserId=postgres;Password=ZHANGRUIJIE;Database=websitedata;";
            using (DataTable dt = Dao.SqlHelper.ExecuteDataTable("select uri,content, title,publishtime from data_sina "))
            {
                foreach (DataRow dr in dt.Rows)
                {
                    indexContent(dr["uri"].ToString(), dr["content"].ToString(), dr["title"].ToString(), dr["publishtime"].ToString(), indexwriter);
                   // indexContent(dr["uri"].ToString(), dr["content"].ToString, indexwriter);
                }
            }
            indexwriter.Optimize();
            indexwriter.Close();
            logger.Debug("全部索引完毕");
        }
        private void indexContent(string uri, string content, string title, string publishtime, IndexWriter writer)
        {
            try
            {
                Document doc = new Document();
                
        //        IndexReader reader = IndexReader.Open(@"E:/Index", true); 
                Byte[] uriBy = new Byte[1];
                uriBy[0] = 1;

                Byte[] contentBy = new Byte[1];
                contentBy[0] = 1;
              //  Field indexlink = new Field("link", uriBy, Field.Store.YES);
         //       Field indexcontent = new Field("content", contentBy, Field.Store.YES);
                doc.Add(new Field("link", uri, Field.Store.YES, Field.Index.TOKENIZED));
                doc.Add(new Field("content", content, Field.Store.YES, Field.Index.TOKENIZED));
                doc.Add(new Field("title", title, Field.Store.YES, Field.Index.TOKENIZED));
                doc.Add(new Field("publishtime", publishtime, Field.Store.YES, Field.Index.TOKENIZED));
               // Field[] fs = new Field[]{"uri", "content"};
               // doc.Add(indexlink);
                //doc.Add(indexcontent);

                writer.AddDocument(doc);
            }

            catch (FileNotFoundException fnfe)
            {

            }
        }

   /*     protected void searchButton_Click(object sender, EventArgs e)
        {
            //索引库的位置
            string indexPath = "E:/index";
            FSDirectory directory = FSDirectory.Open(new DirectoryInfo(indexPath),new NativeFSLockFactory());
            bool isUpdate = IndexReader.IndexExists(directory);
            if (isUpdate)
            {
                if (IndexWriter.IsLocked(directory))
                {
                    IndexWriter.Unlock(directory);
                }
            }
            IndexWriter write = new IndexWriter(directory, new PanGuAnalyzer(), !isUpdate, IndexWriter.MaxFieldLength.UNLIMITED);

            WebClient wc = new WebClient();
            wc.Encoding = Encoding.UTF8;
            int maxID = GetMaxID();
            for (int i = 1; i <=maxID ; i++)
            {
                string url="http://localhost:8080/showtopic-" + i + ".aspx";
                string html = wc.DownloadString(url);
                HTMLDocumentClass doc = new HTMLDocumentClass();

                doc.designMode = "on";
                doc.IHTMLDocument2_write(html);
                doc.close();

                string title = doc.title;
                string body = doc.body.innerText;

                write.DeleteDocuments(new Term("number",i.ToString()));

                Document document = new Document();
                document.Add(new Field("number",i.ToString(),Field.Store.YES,Field.Index.NOT_ANALYZED));
                document.Add(new Field("title",title,Field.Store.YES,Field.Index.NOT_ANALYZED));
                document.Add(new Field("body",body,Field.Store.YES,Field.Index.ANALYZED,Field.TermVector.WITH_POSITIONS_OFFSETS));
                write.AddDocument(document);
                logger.Debug("索引"+i.ToString()+"完毕");

            }
            write.Close();
            directory.Close();
            logger.Debug("全部索引完毕");

        }*/
        private int GetMaxID()
        {
            XDocument xdoc = XDocument.Load("Http://localhost:8080/tools/rss.aspx");
            XElement channel = xdoc.Root.Element("channel");
            XElement fitstItem = channel.Elements("item").First();
            XElement link = fitstItem.Element("link");
            Match match = Regex.Match(link.Value, @"http://localhost:8080/showtopic-(\d+)\.aspx");
            string id = match.Groups[1].Value;
            return Convert.ToInt32(id);
        }
        private string Preview(string body,string keyword)
        {
            PanGu.HighLight.SimpleHTMLFormatter simpleHTMLFormatter = new PanGu.HighLight.SimpleHTMLFormatter("<font color=\"Red\">","</font>");
            PanGu.HighLight.Highlighter highlighter = new PanGu.HighLight.Highlighter(simpleHTMLFormatter, new Segment());
            highlighter.FragmentSize = 100;
            string bodyPreview = highlighter.GetBestFragment(keyword, body);
            return bodyPreview;
        }

       
    }
}