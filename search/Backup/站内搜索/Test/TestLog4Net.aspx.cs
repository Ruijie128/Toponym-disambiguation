using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace 站内搜索.Test
{
    public partial class TestLog4Net : System.Web.UI.Page
    {
        private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(TestLog4Net));
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            //log4net.LogManager.GetLogger(typeof(TestLog4Net)).Debug("我的Log4NetOk啦");
            //logger.Debug("hao");
            //logger.Error("cuowu1le1",new Exception());一个异常对象
        }
    }
}