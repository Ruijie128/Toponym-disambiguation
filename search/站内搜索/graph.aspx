<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="graph.aspx.cs" Inherits="站内搜索.graph" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>华盛顿号航母数据分析</title>
      <meta http-equiv="X-UA-Compatible" content ="IE=edge,chrome=1" />
      <meta name="viewport" content="width=device-width, initial-scale=1.0" />
      <link rel="stylesheet" type="text/css" href="http://static.bosonnlp.com/vendor/jquery-ui/themes/smoothness/jquery-ui.min.css" />
      <link rel="stylesheet" type="text/css" href="http://static.bosonnlp.com/vendor/bootstrap/dist/css/bootstrap.min.css" />
      <link rel="stylesheet" type="text/css" href="http://static.bosonnlp.com/stylesheets/global.css?201508288888" />
        
      <link rel="stylesheet" type="text/css" href="http://static.bosonnlp.com/vendor/css-toggle-switch/dist/toggle-switch.min.css" />
      <link rel="stylesheet" type="text/css" href="http://static.bosonnlp.com/stylesheets/demo.css?201508288888" />
    
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
    <div id="page-header" class="nav" role="navigation">
      <div class="container">
        <a href="/" class="logo"><img alt="logo" src="http://localhost:59595/Geosearch_logo.gif" /></a>
        <ul class="navbar-right">
            <li class="txt home"><a href="/CreateIndex.aspx" ><span class="icon"></span>首页</a></li>
            <li class="txt production"><a href="/product" ><span class="icon"></span>时空数据</a></li>
            <li class="txt develper hide"><a href="#" ><span class="icon"></span>开发者</a></li>
            <li class="txt demo"><a href="/graph.aspx"  class="selected" ><span class="icon"></span>数据分析</a></li>
            <li class="txt document"><a href="/wiki.aspx" ><span class="icon"></span>维基</a></li>
        
            <li class="txt document"><a href="/about" ><span class="glyphicon glyphicon-user"></span>关于</a></li>
        
            
      </div>
    </div>
    <div id="content-page">
  
    <div id="sub-navbar">
	    <div class="container">
		 
	    </div>
    </div>
    <div id="demo-page">
	    <div id="demo-container" class="container">
		
		 
		    <div class="main-title">
			    <span class="dot-line-gray"></span>
			    <span class="title">分析结果</span>
		    </div>
		    <div class="demo-content">
			    <div id="sideBar" class="col-xs-3 bs-sidebar" width="100%" role="complementary">
				    <div class="chunk bs-sidebar-chunk">
				      <ul class="nav">
				        <li class=" analysis">
				    	    <a class="hide" href="#overview-analysis"></a>
				    	    <div class="arrow"></div>
				    	    <div class="title">
				    		    <a href="#overview-pie_geo"><span class="icon"></span>地理饼图</a>
				    	    </div>
				    	    <div class="line"></div>
				        </li>
				        <li class=" ner">
				    	    <a class="hide" href="#overview-ner"></a>
				    	    <div class="arrow"></div>
				    	    <div class="title">
				    		    <a href="#overview-ciyun_geo"><span class="icon"></span>词云</a>
				    	    </div>
				    	    <div class="line"></div>
				        </li>
				        <li class=" depend">
				    	    <a class="hide" href="#overview-depend"></a>
				    	    <div class="arrow"></div>
				    	    <div class="title">
				    		    <a href="#overview-pie_geo"><span class="icon"></span>柱形图</a>
				    	    </div>
				    	    <div class="line"></div>
				        </li>
				        <li class=" emotion">
				    	    <a class="hide" href="#overview-emotion"></a>
				    	    <div class="arrow"></div>
				    	    <div class="title">
				    		    <a href="#overview-heatmap_geo"><span class="icon"></span>热力图</a>
				    	    </div>
				    	    <div class="line"></div>
				        </li>
				        <li class=" info">
				    	    <a class="hide" href="#overview-info"></a>
				    	    <div class="arrow"></div>
				    	    <div class="title">
				    		    <a href="#overview-info"><span class="icon"></span>新闻分类</a>
				    	    </div>
				    	    <div class="line"></div>
				        </li>
				        <li class=" key">
				    	    <a class="hide" href="#overview-key"></a>
				    	    <div class="arrow"></div>
				    	    <div class="title">
				    		    <a href="#overview-key"><span class="icon"></span>关键词提取</a>
				    	    </div>
				    	    <div class="line"></div>
				        </li>
				        <li class=" semantic">
				    	    <a class="hide" href="#overview-semantic"></a>
				    	    <div class="arrow"></div>
				    	    <div class="title">
				    		    <a href="#overview-semantic"><span class="icon"></span>语义联想</a>
				    	    </div>
				    	    <div class="line"></div>
				        </li>
				      </ul>
				    </div>
			    </div>
			    <div class="col-xs-9 main-content" role="main">
				    <div id="overview-pie_geo" class="chunk">
					    <div class="col-xs-12">
						    <div class="title">地理分布饼图 </div>
					    </div>
					    <div id = "pie_geo" style="height:400px"></div>
				  
					    <div class="warn-tips hide"><span class="icon"></span><span class="txt"></span></div>
				    </div>

				    <div id="overview-ciyun_geo" class="chunk">
					    <div class="col-xs-12">
						    <div class="title" >词云</div>
					    </div>
                        <div id ="ciyun_geo" style="height:400px"></div>
					       
					    <div class="warn-tips hide"><span class="icon"></span><span class="txt"></span></div>
				    </div>

				    <div id="Div1" class="chunk">
					    <div class="title">柱形图  </div>
                        <div id ="column_geo" style="height:400px"></div> 
					     
					    <div class="warn-tips hide"><span class="icon"></span><span class="txt"></span></div>
				    </div>

				    <div id="overview-heatmap_geo" class="chunk">
					    <div class="title">热力图 </div>
                        <div id ="heatmap_geo" style="height:400px"></div>
					    
					    <div class="warn-tips hide"><span class="icon"></span><span class="txt"></span></div>
				    </div>
				    <div id="overview-info" class="chunk">
					    <div class="title">新闻分类:<a class="docs" href="http://docs.bosonnlp.com/classify.html" target="_blank">查看文档</a></div>
					    <ul class="info-containers">
					    </ul>
					    <div class="title title-other hide">关键词标签:</div>
					    <ul class="keywords-tags hide">
					    </ul>
					    <span class="loading"></span>
					    <div class="warn-tips hide"><span class="icon"></span><span class="txt"></span></div>
				    </div>
				    <div id="overview-key" class="chunk">
					    <div class="title">关键词提取:<a class="docs" href="http://docs.bosonnlp.com/keywords.html" target="_blank">查看文档</a></div>
					    <div class="result"></div>
					    <span class="loading"></span>
					    <div class="warn-tips hide"><span class="icon"></span><span class="txt"></span></div>
				    </div>
				    <div id="overview-semantic" class="chunk">
					    <div class="title">语义联想:<a class="docs" href="http://docs.bosonnlp.com/suggest.html" target="_blank">查看文档</a></div>
					    <div class="result"></div>
					    <span class="loading"></span>
					    <div class="warn-tips hide"><span class="icon"></span><span class="txt"></span></div>
				    </div>
			    </div>
		    </div>
	    </div>
	    <div id="btn-back-top">
		    <span class="icon"></span>
	    </div>
    </div>

    </div>

    <div id="page-footer" >
  <div class="container">
    
   
    <div class="line"></div>
    <div class="copyright">
       
      Copyright 2015 Geosoft. All Rights Reserved.
    </div>
  </div >
</div>
<input type="hidden" id="hidden-referer" value="" />

        <input type="hidden" value=" 0 " id="hidden_auth" />
    </body>
</html>
<script type="text/javascript" src="http://static.bosonnlp.com/vendor/requirejs/require.min.js"></script>
<script type="text/javascript" src="http://static.bosonnlp.com/javascripts/require.config.js?201508288888"></script>
<script type="text/javascript" src="_static/jquery.js"></script>
<script type="text/javascript" src = "example/www2/js/echarts-all.js"></script>
    <script type="text/javascript" src = "./pieData.js"></script>
<script type="text/javascript" src="./pieGeo.js"></script>


<script>require(['demo'], function () {})</script>



   <script type="text/javascript" src = "example/www2/js/echarts-all.js"></script>
    <!--script type="text/javascript" src = "./pieData.js"></script-->
    <!--script type="text/javascript" src = "example/www2/js/dist//chart/pie.js"></script-->
    <!-- 制作饼图-->
       <script >
           var myChart = echarts.init(document.getElementById('pie_geo'));
           var data = new Array();
           var count = <%=count%>;
           <%
            for(int i =0; i<count; i++) {
           %>
           var temp = {
               "name" : <%= '"'+pie_data[i].name+ '"'%>,
               "value" : <%= pie_data[i].value%>
               };
          
           data.push(temp);
           <%
             }    
           %>

           var option = {

               title: {
                   text: '地域分布',
                   subtext: '华盛顿号',
                   x: 'center'
               },
               tooltip: {
                   trigger: 'item',
                   formatter: "{a} <br/>{b} : {c} ({d}%)"
               },
               legend: {
                   orient: 'vertical',
                   x: 'left',
                   data: ['南海', '东海', '西太平洋', '釜山', '神奈川']
               },
               toolbox: {
                   show: true,
                   feature: {
                       mark: { show: true },
                       dataView: { show: true, readOnly: false },
                       magicType: {
                           show: true,
                           type: ['pie', 'funnel'],
                           option: {
                               funnel: {
                                   x: '25%',
                                   width: '50%',
                                   funnelAlign: 'left',
                                   max: 1548
                               }
                           }
                       },
                       restore: { show: true },
                       saveAsImage: { show: true }
                   }
               },
               calculable: true,
               series: [
                   {
                       name: '地域分布',
                       type: 'pie',
                       radius: '55%',
                       center: ['50%', '60%'],

                       data: data
                       // data:[
                       //     {value:335, name:'神奈川'},
                       //     {value:310, name:'韩国釜山'},
                       //     {value:234, name:'东海'},
                       //     {value:135, name:'西太平洋'},
                       //     {value:1548, name:'南海'}
                       // ]
                   }
               ]
           };
           myChart.setOption(option);
    </script>
    <!-- 制作柱形图-->
    <script >
        var myChart = echarts.init(document.getElementById('column_geo'));
        
        var option = {
            tooltip : {
                trigger: 'axis'
            },
            toolbox: {
                show : true,
                feature : {
                    mark : {show: true},
                    dataView : {show: true, readOnly: false},
                    magicType: {show: true, type: ['line', 'bar']},
                    restore : {show: true},
                    saveAsImage : {show: true}
                }
            },
            calculable : true,
            legend: {
                data:['数目']
            },
            xAxis : [
                {
                    type : 'category',
                    data : ['1月','2月','3月','4月','5月','6月','7月','8月','9月','10月','11月','12月']
                }
            ],
            yAxis : [
                {
                    type : 'value',
                    name : '数目',
                    axisLabel : {
                        formatter: '{value} '
                    }
                },
                {
                    type : 'value',
                    name : '数目',
                    axisLabel : {
                        formatter: '{value} '
                    }
                }
            ],
            series : [

                {
                    name:'数目',
                    type:'bar',
                    data:[26,29,81,78,55, 132,107,82,46,35,29,34]
                },
       
                {
                    name:'数目',
                    type:'line',
                    yAxisIndex: 1,
                    data:[26,29,81,78,55, 132,107,82,46,35,29,34]
                }
            ]
        };
                    
               
        myChart.setOption(option);
    </script>
    <!-- 制作词云-->
    <script type="text/javascript" src = "example/www/js/echarts.js"></script>
    <script type="text/javascript" src = "example/www/js/chart/wordCloud.js"></script>
    <script>
        require.config({
            paths:{
                echarts: 'example/www/js/',
            }
        });
        require([
            'echarts',
            'echarts/chart/wordCloud'
        ],
            function(echarts) {
                var myChart = echarts.init(document.getElementById('ciyun_geo'));
                var data = new Array();
                var count = <%=count%>;
                <%
                 for(int i =0; i<count; i++) {
                     %>
                var temp = {
                    "name" : <%= '"'+pie_data[i].name+ '"'%>,
                                "value" : <%= pie_data[i].value%> *100,
                                itemStyle: createRandomItemStyle()
                            };
          
                data.push(temp);
                     <%
                     }    
                %>
                option = {
                    title: {
                        text: '事件显示',
                        link: 'http://www.google.com/trends/hottrends'
                    },
                    tooltip: {
                        show: true
                    },
                    series: [{
                        name: '事件显示',
                        type: 'wordCloud',
                        size: ['80%', '80%'],
                        textRotation : [0, 45, 90, -45],
                        textPadding: 0,
                        autoSize: {
                            enable: true,
                            minSize: 14
                        },
                        data: data
                    }]
                };


                function createRandomItemStyle() {
                    return {
                        normal: {
                            color: 'rgb(' + [
                                Math.round(Math.random() * 160),
                                Math.round(Math.random() * 160),
                                Math.round(Math.random() * 160)
                            ].join(',') + ')'
                        }
                    };
                }

                myChart.setOption(option);
            })
    </script>
  
    <!-- 热力图-->

    <script type="text/javascript" src = "example/www/js/echarts.js"></script>
    <script type="text/javascript" src = "example/www/js/chart/heatmap.js"></script>
    <script type="text/javascript" src = "example/www/js/chart/map.js"></script>
    <script >
        require.config({
            paths:{
                echarts: 'example/www/js/',
            }
        });
        require([
            'echarts',
            'echarts/chart/heatmap',
            'echarts/chart/map'
        ],
            function(echarts) {

                var myChart = echarts.init(document.getElementById('heatmap_geo'));
                var heatData = [];
                 <%
                 for(int i =0; i<count; i++) {
                     %>
                heatData.push([
                       <%=   pointList[i][0] %>,
                                <%=   pointList[i][1] %>,
                                <%=   pointList[i][2] %>
                        ]);
                     <%
                     }    
                %>
                
               
                option = {
                    backgroundColor: '#1b1b1b',
                    title : {
                        text: '热力图结合地图',
                        x:'center',
                        textStyle: {
                            color: 'white'
                        }
                    },
                    tooltip : {
                        trigger: 'item',
                        formatter: '{b}'
                    },
                    toolbox: {
                        show : true,
                        orient : 'vertical',
                        x: 'right',
                        y: 'center',
                        feature : {
                            mark : {show: true},
                            dataView : {show: true, readOnly: false},
                            restore : {show: true},
                            saveAsImage : {show: true}
                        }
                    },
                    series : [
                        {
                            name: '北京',
                            type: 'map',
                            mapType: 'world',
                            roam: true,
                            hoverable: false,
                            data:[],
                            heatmap: {
                                minAlpha: 0.1,
                                data: heatData
                            },
                            itemStyle: {
                                normal: {
                                    borderColor:'rgba(100,149,237,0.6)',
                                    borderWidth:0.5,
                                    areaStyle: {
                                        color: '#1b1b1b'
                                    }
                                }
                            }
                        }
                    ]
                };
                    
                    
               
                myChart.setOption(option);
            })
    </script>
</body>
</html>
