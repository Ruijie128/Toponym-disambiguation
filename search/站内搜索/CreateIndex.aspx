<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CreateIndex.aspx.cs" Inherits="站内搜索.CreateIndex" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>数据检索</title>
    <link href="css/ui-lightness/jquery-ui-1.8.2.custom.css" rel="stylesheet" type="text/css" />
    <script src="js/jquery-1.4.2.min.js" type="text/javascript"></script>
    <script src="js/jquery-ui-1.8.2.custom.min.js" type="text/javascript"></script>
    <!--meta http-equiv='Content-Type' content='text/html; charset=utf-8'-->
    <meta name="viewport" content="initial-scale=1.0, maximum-scale=1.0, minimum-scale=1.0, user-scalable=no, width=device-width" />
        
     <script type="text/javascript" src="static/easyui/js/jquery.min.js"></script>      
     <script type="text/javascript" src="static/easyui/js/jquery.easyui.min.js"></script>
     <script type="text/javascript" src="static/easyui/js/easyui-lang-zh_CN.js"></script>
     <link rel="stylesheet" type="text/css" href="static/easyui/css/easyui-gray.css">
        
     <script type="text/javascript" src="static/ol/build/ol-v3.1.1.js"></script>
     <link rel="stylesheet" type="text/css" href="static/ol/css/ol.css" />
    <link rel="stylesheet" type="text/css" href="http://static.bosonnlp.com/vendor/jquery-ui/themes/smoothness/jquery-ui.min.css" />
    <link rel="stylesheet" type="text/css" href="http://static.bosonnlp.com/vendor/bootstrap/dist/css/bootstrap.min.css" />
    <link rel="stylesheet" type="text/css" href="http://static.bosonnlp.com/stylesheets/global.css?201508288888" />
        
    <link rel="stylesheet" type="text/css" href="http://static.bosonnlp.com/vendor/css-toggle-switch/dist/toggle-switch.min.css" />
    <link rel="stylesheet" type="text/css" href="http://static.bosonnlp.com/stylesheets/demo.css?201508288888" />
        
        <!--script type="text/javascript" src="js/main.js"></!--script>
        <script type="text/javascript" src="js/request.js"></script-->
        <link rel='stylesheet' type='text/css' href="css/main.css" />
    <script type="text/javascript">
        $(function () {
            $("#txtKeyword").autocomplete(
            {   source: "SearchSuggestion.ashx",
                select: function (event, ui) { $("#txtKeyword").val(ui.item.value); $("#form1").submit(); }
            });
        });
        function getAbsolutePosition( element) {
            var point = { x: element.offsetLeft, y: element.offsetBottom };
            if (element.offsetParent) {
                var parentPoint = this.getAbsolutePosition(element.offsetParent);
                point.x += parentPoint.x;
                point.y += parentPoint.y;
            }
            return point;
        }
      
        function divonClick() {
            document.getElementById("caDiv").style.display = "none";
        }
    </script>
    <script type="text/css" src='https://api.mapbox.com/mapbox.js/v2.2.3/mapbox.js'></script>
    <link href='https://api.mapbox.com/mapbox.js/v2.2.3/mapbox.css' rel='stylesheet' />
    <style type="text/css">
      body { margin:0; padding:0; }
      #map { position:absolute;  width:100%; }
    </style>
     <script type="text/javascript">
         var _hmt = _hmt || [];
         (function () {
             var hm = document.createElement("script");
             hm.src = "//hm.baidu.com/hm.js?6629d4aae357d8d3e47238c93f1e1d78";
             var s = document.getElementsByTagName("script")[0];
             s.parentNode.insertBefore(hm, s);
         })();
        </script>
     <style type="text/css">
        .pager {
	        TEXT-ALIGN: center; PADDING-BOTTOM: 3px; PADDING-LEFT: 0px; PADDING-RIGHT: 0px; FLOAT: right; PADDING-TOP: 3px
        }
        .pager A {
	        BORDER-BOTTOM: #ccc 1px solid; TEXT-ALIGN: left; BORDER-LEFT: #ccc 1px solid; PADDING-BOTTOM: 3px; LINE-HEIGHT: 26px; MARGIN: 0px 2px; OUTLINE-STYLE: none; PADDING-LEFT: 5px; PADDING-RIGHT: 5px; BACKGROUND: #fff; COLOR: #000; FONT-SIZE: 12px; BORDER-TOP: #ccc 1px solid; BORDER-RIGHT: #ccc 1px solid; TEXT-DECORATION: none; PADDING-TOP: 4px
        }
        .pager A:hover {
	        BORDER-BOTTOM: #f80 1px solid; BORDER-LEFT: #f80 1px solid; COLOR: #f80; BORDER-TOP: #f80 1px solid; BORDER-RIGHT: #f80 1px solid; TEXT-DECORATION: underline
        }
        .pager A:focus {
                        
	        -moz-outline-style: none
        }
        .pager SPAN {
	        BORDER-BOTTOM-STYLE: none; TEXT-ALIGN: left; PADDING-BOTTOM: 4px; LINE-HEIGHT: 26px; BORDER-RIGHT-STYLE: none; MARGIN: 1px 2px; PADDING-LEFT: 6px; PADDING-RIGHT: 6px; BORDER-TOP-STYLE: none; BACKGROUND: #f80; COLOR: #fff; FONT-SIZE: 12px; BORDER-LEFT-STYLE: none; PADDING-TOP: 5px
        }
    </style>
        <style type="text/css">
        #hotwordsUL li{float:left;margin-left:150px;list-style-type:none;}
            #txtKeyword
            {
                width: 354px;
                margin-left: 0px;
            }
    </style>
</head>
<body >
         <div id="page-header" class="nav" role="navigation">
      <div class="container">
        <a href="/" class="logo"><img alt="logo" src="http://localhost:2941/Geosearch_logo.gif"  /></a>
        <ul class="navbar-right">
            <li class="txt home"><a href="/CreateIndex.aspx"   class="selected"><span class="icon"></span>首页</a></li>
            <li class="txt production"><a href="/space_time_data.aspx" ><span class="icon"></span>时空数据</a></li>
            <li class="txt develper hide"><a href="#" ><span class="icon"></span>开发者</a></li>
            <li class="txt demo"><a href="/graph.aspx"  ><span class="icon"></span>数据分析</a></li>
            <li class="txt document"><a href="/wiki.aspx" ><span class="icon"></span>维基</a></li>
        
            <li class="txt document"><a href="/about" ><span class="glyphicon glyphicon-user"></span>关于</a></li>
        
            
      </div>
    </div>

    <form id="form1" runat="server">
        <div id="sub-navbar">
	        <div class="container">
		 
	        </div>
        </div>
    <div align="center">
        <br />
        <br />
        <input type="text" id="txtKeyword" name="kw" value='<%=kw %>' placeholder="请输入要检索的地理实体"></input>
        <input type="text" id="txtShip" name="kwShip" value='<%=kwShip %>' placeholder="请输入要检索的航母名称"></input>
        <input type="text" id="fromDate" name="fromDate" value='<%=fromDate %>' placeholder="请输入查询起始时间 年-月-日"></input>
        <input type="text" id="toDate" name="toDate" value='<%=toDate %>' placeholder="请输入查询终止时间 年-月-日 "></input>
        <%-- <asp:Button ID="createIndexButton" runat="server" onclick="searchButton_Click" 
            Text="创建索引库" />--%>
        <input type="submit" name="searchButton" value="搜索" style="width: 91px" /><br />

    </div>
    
    <br />
    <!--ul id="hotwordsUL">
          <asp:Repeater ID="hotwordsRepeater" runat="server">
            <ItemTemplate>
                <li><a href='CreateIndex.aspx?kw=<%#Eval("Keyword") %>'><%#Eval("Keyword") %></a></li>
            </ItemTemplate>
          </asp:Repeater>
    </!--ul-->
    &nbsp;<br />
  <div id = "left" style="float:left; width:30%; background:#ccc; position:absolute; height:100%" >
  
        <div title="搜索结果" style="padding:5px;">
                    <div id=" ">
                        <text id="shouResult">显示搜索结果</text>
                        <asp:Repeater ID="dataRepeater" runat="server" EnableViewState="true">
                        <HeaderTemplate>
                            <ul>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <li>
                                <a href='<%#Eval("Uri") %>'><%#Eval("Uri") %></a>
                                <br />
                                <%#Eval("Score") %>
                                <%#Eval("Title") %>
                            </li>
                        </ItemTemplate>
                        <FooterTemplate>
		                <ul> </ul>
		            </FooterTemplate>
                    </asp:Repeater>
                    <br />
                    <div class="pager"><%=RenderToHTML%></div>
                    </div>
                </div>
        </div>
       <div id="right" style="float:right;background:#ccc; left:309px; width:70%; height:100%">
         <div data-options="region:'center'" style="overflow: hidden">  
            <div id="map"></div>
             
            <div id="tab-tools"> 
                <a href="javascript:void(0)" id="tab-collapse-btn" class="easyui-linkbutton" data-options="plain:true,iconCls:'icon-arrow-right'"></a>
            </div>  
        </div>
           </div>
        <div data-options="region:'south',collapsible:false" style="height:20px;">
           <div class="top-center">军事项目组</div>
        </div>
    

    </form>

    <!-- 加载地图-->
    <script type="text/javascript" src='https://api.mapbox.com/mapbox.js/v2.2.3/mapbox.js'></script>
    <link  type="text/css" href='https://api.mapbox.com/mapbox.js/v2.2.3/mapbox.css' rel='stylesheet' />
    <script type="text/javascript">
        L.mapbox.accessToken = 'pk.eyJ1IjoicmFjaGVsNTciLCJhIjoiY2lnd2g1M3ZkMHQybnd5bTA5MDJkNDA1byJ9.sX1DqMxQpSjrXaNE2x39hg';
        var map = L.mapbox.map('map', 'mapbox.streets')
            .setView([<%=lat%>, <%=lon%>], 3);

        var popupContent = "地名： " + document.getElementById("txtKeyword").value + " </br>" +"文档数目："+ <%=targetResultCount %>;
        L.marker([<%=lat%>, <%=lon%>], {
            icon: L.mapbox.marker.icon({
                'marker-size': 'large',
                'marker-symbol': 'bus',
                'marker-color': '#fa0'
            })
        }).addTo(map).bindPopup(popupContent).openPopup();
    </script>

</body>
</html>
