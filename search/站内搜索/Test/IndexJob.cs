using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quartz;
using Lucene.Net.Store;
using Lucene.Net.Index;
using System.IO;
using System.Net;
using mshtml;
using Lucene.Net.Documents;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using log4net;
using System.Text;
using Lucene.Net.Analysis.PanGu;
using System.Configuration;

namespace 站内搜索.Text
{
    public class IndexJob:IJob
    {
        private ILog logger = LogManager.GetLogger(typeof(IndexJob));
        public void Execute(JobExecutionContext context)
        {
            try
            {
                logger.Debug("索引开始");
                CreateIndex();
                logger.Debug("索引结束");
            }
            catch (Exception ex)
            {
                logger.Debug("启动索引任务异常", ex);
            }
        }
        private void CreateIndex()
        {
            //索引库存放在这个文件夹里
            string indexPath = ConfigurationManager.AppSettings["pathIndex"];
            //Directory表示索引文件保存的地方，是抽象类，两个子类FSDirectory表示文件中，RAMDirectory 表示存储在内存中
            FSDirectory directory = FSDirectory.Open(new DirectoryInfo(indexPath), new NativeFSLockFactory());
            //判断目录directory是否是一个索引目录。
            bool isUpdate = IndexReader.IndexExists(directory);
            logger.Debug("索引库存在状态:"+isUpdate);
            if (isUpdate)
            {
                if (IndexWriter.IsLocked(directory))
                {
                    IndexWriter.Unlock(directory);
                }
            }
            //第三个参数为是否创建索引文件夹,Bool Create,如果为True，则新创建的索引会覆盖掉原来的索引文件，反之，则不必创建,更新即可。
            IndexWriter write = new IndexWriter(directory, new PanGuAnalyzer(), !isUpdate, IndexWriter.MaxFieldLength.UNLIMITED);

            WebClient wc = new WebClient();
            //编码，防止乱码
            wc.Encoding = Encoding.UTF8;
            int maxID;
            try
            {
                //读取rss，获得第一个item中的链接的编号部分就是最大的帖子编号
                maxID = GetMaxID();
            }
            catch (WebException webEx)
            {
                logger.Error("获得最大帖子号出错",webEx);
                return;
                
            }
            for (int i = 1; i <= maxID; i++)
            {
                try
                {
                    string url = "http://localhost:8080/showtopic-" + i + ".aspx";
                    logger.Debug("开始下载:"+url);
                    string html = wc.DownloadString(url);
                    HTMLDocumentClass doc = new HTMLDocumentClass();

                    doc.designMode = "on";//不让解析引擎尝试去执行
                    doc.IHTMLDocument2_write(html);
                    doc.close();

                    string title = doc.title;
                    string body = doc.body.innerText;
                    //为避免重复索引，先输出number=i的记录，在重新添加
                    write.DeleteDocuments(new Term("number", i.ToString()));

                    Document document = new Document();
                    //Field为字段，只有对全文检索的字段才分词，Field.Store是否存储
                    document.Add(new Field("number", i.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                    document.Add(new Field("title", title, Field.Store.YES, Field.Index.NOT_ANALYZED));
                    document.Add(new Field("body", body, Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.WITH_POSITIONS_OFFSETS));
                    write.AddDocument(document);
                    logger.Debug("索引" + i.ToString() + "完毕");
                }
                catch (WebException webEx)
                {

                    logger.Error("下载"+i.ToString()+"失败",webEx);
                }

            }
            write.Close();
            directory.Close();
            logger.Debug("全部索引完毕");
        }
        //取最大帖子号
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

       
    }
}