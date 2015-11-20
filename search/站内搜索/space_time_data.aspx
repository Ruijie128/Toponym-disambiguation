<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="space_time_data.aspx.cs" Inherits="站内搜索.space_time_data" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>时空四元组</title>
    
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
        
     <link href = "./table.css" rel='stylesheet' /> 
         <script>
             var _hmt = _hmt || [];
             (function () {
                 var hm = document.createElement("script");
                 hm.src = "//hm.baidu.com/hm.js?6629d4aae357d8d3e47238c93f1e1d78";
                 var s = document.getElementsByTagName("script")[0];
                 s.parentNode.insertBefore(hm, s);
             })();
        </script>
</head>
<body>
     

    <form id="form1" runat="server">
    <div>
        
    </div>
    </form>

    <h2>华盛顿号时空四元组</h2>
    <table class="bordered">
        <thead>
            <tr>
                <th>ID</th>
                <th>ObjectID</th>
                <th>ObjectName</th>
                <th>DateTime</th>        
                <th>DateTime2</th>
                <th>ToDateTime</th>
                <th>TimeFlag</th>        
                <th>PublishTime</th>
                <th>FeatureID</th>        
                <th>FeatureName</th>
                <th>Movement</th>
                <th>MovementName</th>        
                <th>EventID</th>
                <th>EventName</th>
                <th>SourceID</th>        
                <th>Source</th>
                <th>PreID</th>
                <th>Text</th>        
                <th>anchaorFlag</th>
            </tr>
        </thead>
        <%= selectData() %>>

    </table>
</body>
</html>
