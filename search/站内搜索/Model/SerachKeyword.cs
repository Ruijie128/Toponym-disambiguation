using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace 站内搜索.Model
{
    public class SerachKeyword
    {
        public int KeywordID { get; set; }
        public string Keyword { get; set; }
        public string ShipKeyword { get; set; }
        public DateTime SearchDateTime { get; set; }
        public string ClinetAddress { get; set; }
    }
}