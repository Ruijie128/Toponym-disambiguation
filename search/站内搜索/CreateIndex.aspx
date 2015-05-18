<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CreateIndex.aspx.cs" Inherits="站内搜索.CreateIndex" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="css/ui-lightness/jquery-ui-1.8.2.custom.css" rel="stylesheet" type="text/css" />
    <script src="js/jquery-1.4.2.min.js" type="text/javascript"></script>
    <script src="js/jquery-ui-1.8.2.custom.min.js" type="text/javascript"></script>
    <!--meta http-equiv='Content-Type' content='text/html; charset=utf-8'-->
    <meta name="viewport" content="initial-scale=1.0, maximum-scale=1.0, minimum-scale=1.0, user-scalable=no, width=device-width">
        
     <script type="text/javascript" src="static/easyui/js/jquery.min.js"></script>      
     <script type="text/javascript" src="static/easyui/js/jquery.easyui.min.js"></script>
     <script type="text/javascript" src="static/easyui/js/easyui-lang-zh_CN.js"></script>
     <link rel="stylesheet" type="text/css" href="static/easyui/css/easyui-gray.css">
        
     <script type="text/javascript" src="static/ol/build/ol-v3.1.1.js"></script>
     <link rel="stylesheet" type="text/css" href="static/ol/css/ol.css">
        
        <script type="text/javascript" src="js/main.js"></script>
        <script type="text/javascript" src="js/request.js"></script>
        <link rel='stylesheet' type='text/css' href="css/main.css" />
    <script type="text/javascript">
        $(function () {
            $("#txtKeyword").autocomplete(
            {   source: "SearchSuggestion.ashx",
                select: function (event, ui) { $("#txtKeyword").val(ui.item.value); $("#form1").submit(); }
            });
        });
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
<body>
    <form id="form1">
    <div align="center">
        <img src="http://incubator.apache.org/lucene.net/images/lucene-medium.png"/>
    </div>
    <br />
    <br />
    <div align="center">
        <input type="text" id="txtKeyword" name="kw" value='<%=kw %>'/>
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
  
      
         <div data-options="region:'center'" style="overflow: hidden">  
            <div id="map">
                <div id="popup" class="ol-popup">
                    <a href="#" id="popup-closer" class="ol-popup-closer"></a>
                    <div id="popup-content"></div>
                </div>
            </div>
            <!-- <div id="map-rightmenu" class="easyui-menu" style="width:120px;">
                <div data-options="iconCls:'icon-add'" id="map-rightmenu-add" >添加</div>
            </div>
            <div id="map-dlg" class="easyui-dialog" title="添加" data-options="iconCls:'icon-add',closed:true,"
                 style="width:450px;height:auto;padding:10px">
                <table id="addTbl" >    
                </table>
                <a href="#" id="submit-btn" class="easyui-linkbutton" data-options="iconCls:'icon-ok'" style="float:right;">提交</a>
            </div> -->
            <div id="map-tab" class="easyui-tabs" data-options="tools:'#tab-tools',tabPosition:'left',headerWidth:80,tabWidth:80,tabHeight:30" style="position: absolute; top:200px;right:0px;">
                <div title="图层管理" style="padding:5px;">
                    <div id="layertree">
                      
                    </div>
                </div>
                <div title="测试" style="padding:5px;">
                    <a href="#" class="easyui-linkbutton" data-options="" id="testGeoIndex">测试地理实体倒排文件</a>
                </div>
                <div title="搜索结果" style="padding:5px;">
                    <div id="">
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
		            </ul>
		            </FooterTemplate>
		        </asp:Repeater>
		        <br />
		        <div class="pager"><%=RenderToHTML%></div>
                    </div>
                </div>
            </div>
            <div id="tab-tools"> 
                <a href="javascript:void(0)" id="tab-collapse-btn" class="easyui-linkbutton" data-options="plain:true,iconCls:'icon-arrow-right'"></a>
            </div>  
        </div>
        <div data-options="region:'south',collapsible:false" style="height:20px;">
           <div class="top-center">军事项目组</div>
        </div>
    

    </form>
</body>
</html>
