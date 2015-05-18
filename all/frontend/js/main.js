$(document).ready(function(){
    /*
     * 地图数据
     */
    var osm = new ol.layer.Tile({
        name: "OpenStreetMap",
        source: new ol.source.OSM()
    })
    var i, ii;
    var arcgisstyles = [
      'World_Imagery',
      'World_Physical_Map',
      'World_Shaded_Relief',
      'World_Street_Map',
      'World_Terrain_Base',
      'World_Topo_Map',
    ];
    var arcgislayers = [];    
    for (i = 0, ii = arcgisstyles.length; i < ii; ++i) {
      arcgislayers.push( new ol.layer.Tile({
          name: "ArcGIS: " + arcgisstyles[i],
          visible: false,
          source: new ol.source.XYZ({                 
                url: 'http://server.arcgisonline.com/ArcGIS/rest/services/' +
                    arcgisstyles[i] + '/MapServer/tile/{z}/{y}/{x}',
                wrapX: false
            })
        }));
    }
    
    var stamenstyles = [
        "watercolor",
        'terrain-labels',
        'toner',
    ]
    var stamenlayers = [];    
    for (i = 0, ii = stamenstyles.length; i < ii; ++i) {
      stamenlayers.push( new ol.layer.Tile({
          name: "StamenMap: " + stamenstyles[i],
          visible: false,
          source: new ol.source.Stamen({
                layer: stamenstyles[i]
          })
        }));
    }
    
    /*
     * 基础控件
     */ 
    var mousePositionControl = new ol.control.MousePosition({
        coordinateFormat: ol.coordinate.createStringXY(4),
        projection: 'EPSG:4326',
        // comment the following two lines to have the mouse position
        // be placed within the map.
        className: 'custom-mouse-position',
        target: document.getElementById('mouse-position'),
        undefinedHTML: '&nbsp;'
    }); 
    var zoomslider = new ol.control.ZoomSlider(); 
    var scaleLine = new ol.control.ScaleLine({
          units: 'metric'
    });
    
    
    /*
     * 层级控件
     */
    var DIFF = 3;//网格层级 = 地图层级+DIFF
    window.app = {};
    var app = window.app;
    app.geosotLayerControl = function (opt_options) {
        var options = opt_options || {};
        
        var span = document.createElement('span');
        span.id = "geosot-layer";
        var zoomlevel = map.getView().getZoom();
        zoomlevel = 5;
        span.innerHTML = "网格层级：" + (zoomlevel + DIFF);
      
        var element = document.createElement('div');
        element.className = 'geosot-layer';
        element.appendChild(span);
        
        ol.control.Control.call(this, {
            element: element,
            target: options.target
        });
    };
    ol.inherits(app.geosotLayerControl, ol.control.Control);
  
   
   /*
    * 创建地图
    */
    var map = new ol.Map({
        target: 'map',
        layers: [
            new ol.layer.Group({
                name: "基础底图数据",
                layers: [osm].concat(arcgislayers).concat(stamenlayers)
            }),
        ],
        view: new ol.View({
            // center: ol.proj.transform([117.194, 39.120], 'EPSG:4326', 'EPSG:3857'),
            center: ol.proj.transform([80, 20], 'EPSG:4326', 'EPSG:3857'),
            zoom: 2,
            minZoom: 1,
            maxZoom: 19
        }),
        controls: ol.control.defaults({
                attributionOptions: /** @type {olx.control.AttributionOptions} */ ({
                collapsible: false
            })
        }).extend([mousePositionControl, zoomslider, scaleLine,
            new ol.control.ZoomToExtent({
              extent: [
                -2.0037507067161843E7, -1.997186888040859E7,
                2.0037507067161843E7, 1.9971868880408563E7
              ]
            })
        ])
    });
    map.addControl(new app.geosotLayerControl());
    map.getView().on('change:resolution', function() {
        var zoomlevel = map.getView().getZoom();
        zoomlevel = 5;
        $("#geosot-layer").html("网格层级：" + (zoomlevel + DIFF));
    });


     /*
     * 捕捉地图显示变化的事件，鼠标弹起时传回当前可见区域的经纬度范围、层级（分辨率），传回后台
     * 后台根据传回的经纬度范围，计算当前需要显示的网格集合，生成geojson文件，传回前端
     */
    var gridLayer = new ol.layer.Vector({
        name: "GeoSOT网格",
        style: new ol.style.Style({
            stroke: new ol.style.Stroke({
              color: 'black',
              width: 0.2
            })
          })
    });
    map.addLayer(gridLayer);
    
    function wrapLon(value) {
      // var worlds = Math.floor((value + 180) / 360);
      // return value - (worlds * 360);
      var value;
      if(value>180)
        value = 180;
      else if(value<-180)
        value = -180;
      return value;
    }
    
    var url = gridLayer8();; //向后端发送请求：网格文件数据
    var tempSource = new ol.source.GeoJSON({
        projection: 'EPSG:3857',
        url: url
    });
    
    gridLayer.setSource(tempSource);
        
    //右侧折叠工具栏
    $("#map-tab").tabs({
        width:450,
        height:$("#map").height()-30,
        onSelect: function(title,index){
               $("#map-tab").width(450);
        }
    });
    $('#tab-collapse-btn').bind('click', function(){
        if($("#map-tab").width()==80) {
            $("#map-tab").width(450);
            $('#tab-collapse-btn').linkbutton({
                iconCls: 'icon-arrow-right'
            });
        }            
        else {
            $("#map-tab").width(80);
            $('#tab-collapse-btn').linkbutton({
                iconCls: 'icon-arrow-left'
            });
        }
    }); 
    setTimeout(function() {
        $("#map-tab").width(80);
        $("#resultTbl").datagrid({
            height:$("#map").height()-85,
        });
    }, 200); 
    
    //随着窗口的放大缩小，重置地图和右侧折叠工具栏，需要设置延迟
    window.onresize = function()
    {
        setTimeout(function() {
           map.updateSize();
           }, 200);
        setTimeout(function() {
           $("#map-tab").tabs({
                height:$("#map").height()-30,
            });
           $("#resultTbl").datagrid({
                height:$("#map").height()-85,
            });
           }, 300);
    };
    
    /**
     * Popup
     */
    var container = document.getElementById('popup');
    var content = document.getElementById('popup-content');
    var closer = document.getElementById('popup-closer');
    closer.onclick = function() {
        container.style.display = 'none';
        closer.blur();
        return false;
    };
    
    var popup = new ol.Overlay({
        element: container
    });
    map.addOverlay(popup);
    
   /*
     * 图层管理 
     */
    //生成html                    
    function createLayerControl(layerNo, layer) {
        var li, visibleLabel;
        li = $("<li></li>").attr("id", "layer" + layerNo); 
        visibleLabel = $("<label class='checkbox'></label>").attr("for", "visible" + layerNo);
        visibleLabel.append($('<input class="visible" type="checkbox"/>').attr("id", "visible" + layerNo));
        visibleLabel.append(layer.get("name"));
        li.append(visibleLabel);
        return li;
    }
    var layertree = $("#layertree");
    var ul = $("<ul></ul>");
    map.getLayers().forEach(function(layer, i) {      
        li = createLayerControl(i, layer);        
        if (layer instanceof ol.layer.Group) {
            subul = $("<ul></ul>");
            layer.getLayers().forEach(function(sublayer, j) {
                subul.append(createLayerControl(''+i+j ,sublayer));
            });
            li.append(subul);
        }
        ul.append(li);
    });
    layertree.append(ul);
    //绑定
    function bindInputs(layerid, layer) {
      new ol.dom.Input($(layerid + ' .visible')[0])
          .bindTo('checked', layer, 'visible');
    }
    map.getLayers().forEach(function(layer, i) {
      bindInputs('#layer' + i, layer);
      if (layer instanceof ol.layer.Group) {
        layer.getLayers().forEach(function(sublayer, j) {
          bindInputs('#layer' + i + j, sublayer);
        });
      }
    });
    // $('#layertree li > label').click(function() {
       // $(this).siblings('ul').toggle();
    // }).siblings('ul').hide();


    
    /*
     * 鼠标左击事件处理
     */
     map.on('click', function(evt) {
        var coordinate = evt.coordinate;
        var hdms = ol.coordinate.toStringHDMS(ol.proj.transform(
            coordinate, 'EPSG:3857', 'EPSG:4326'));
    
        popup.setPosition(coordinate);
        container.style.display = 'none';

        // var feature = gridLayer.getSource().getClosestFeatureToCoordinate(coordinate);
        map.forEachFeatureAtPixel(evt.pixel, function(feature, layer) {
            if(layer&&layer.getVisible()&&(layer.get("name") == "testGeoIndexLayer")) {
                if(feature) {
                    content.innerHTML = feature.getId();
                    container.style.display = 'block';
                }
            }
/*             if(layer&&layer.getVisible()&&(layer.get("name") == "GeoSOT网格")) {
                if(feature) {
                    content.innerHTML = feature.getId();
                    container.style.display = 'block';
                }
            }  */
        });
       
    });
    
    var testGeoIndexLayer = null;
    $("#testGeoIndex").click(function(){
        $.ajax({
           url: "testGeoIndex/", //向后端发送请求：倒排索引数据
           type: "GET",      
           success: getPlaces,
           error: function(){
               alert("error");
            }
        });        
    });
    
    function getColor(value, min, max){
        if(min == max)
            value = 0;
        else
            value = (value - min)/(max - min);
        //value from 0 to 1
        var hue=((1-value)*120).toString(10);
        return ["hsla(",hue,",100%,50%,0.4)"].join("");
    }
    function getPlaces(data, textStatus) {
        if(data["status"] != "ok")
            alert("error");
        
        if(testGeoIndexLayer != null)
            map.removeLayer(testGeoIndexLayer);
        
        var vectorSource = new ol.source.GeoJSON(
            /** @type {olx.source.GeoJSONOptions} */ ({
            object: {
                'type': 'FeatureCollection',
                'crs': {
                  'type': 'name',
                  'properties': {
                    'name': 'EPSG:3857'
                  }
                },
                'features': []
              }
        }));
        
        
        var testdata = data["data"];
        var min = 0;
        var max = 0;
        for(var i=0; i<testdata.length; i++) {
            var item = testdata[i];        
            var geometries = new Array();
            var geocodes = item["geocodes8"].split(",");
            var geocode;
            for(var j=0; j<geocodes.length; j++) {
                geocode = parseInt(geocodes[j]);
                geometries.push(gridLayer.getSource().getFeatureById(geocode).getGeometry());
            }
            var feature = new ol.Feature(new ol.geom.GeometryCollection(geometries));
            feature.setId(item["name"] + ": " + item["count"]);
            feature.setProperties({"count": item["count"]});
            if(item["count"]<min) min = item["count"];
            if(item["count"]>max) max = item["count"];
            vectorSource.addFeature(feature);
            
            BaryGeocodeCenter = ol.extent.getCenter(gridLayer.getSource().getFeatureById(item['barygeocode8']).getGeometry().getExtent());           
            var feature = new ol.Feature(new ol.geom.Point((BaryGeocodeCenter)));
            feature.setId(item["name"] + ": " + item["count"]);
            vectorSource.addFeature(feature);
            
        } 
        
        var highlightStyleCachePoint = {};
        var highlightStyleCache = {};
        var iconStyleCache = {};
        testGeoIndexLayer = new ol.layer.Vector({
            name:   "testGeoIndexLayer",
            source: vectorSource,
            style:  function(feature, resolution) {
                        if(feature.getGeometry().getType()=='Point') {
                            var text = resolution < 50000 ? feature.getId() : '';
                            if (!highlightStyleCachePoint[text]) {
                                highlightStyleCachePoint[text] = [new ol.style.Style({
                                    text: new ol.style.Text({
                                      font: '12px Calibri,sans-serif',
                                      text: text,
                                      fill: new ol.style.Fill({
                                        color: '#000'
                                      })/*,
                                      stroke: new ol.style.Stroke({
                                        color: 'yellow',
                                        width: 2
                                      })*/
                                    }),                                     
                                    fill: new ol.style.Fill({
                                        color: 'rgba(199,21,133,0.5)'
                                    })                                    
                                })];
                            }
                            return highlightStyleCachePoint[text];
                        }
                        else {
                            var count = feature.getProperties()["count"];
                            var text = count + '';
                            if (!highlightStyleCache[text]) {
                                highlightStyleCache[text] = [new ol.style.Style({
                                    stroke: new ol.style.Stroke({
                                        color: getColor(count,min,max),
                                        width: 2
                                    }),
                                    fill: new ol.style.Fill({
                                        color: getColor(count,min,max)
                                    })                                    
                                })];
                            }
                            return highlightStyleCache[text];
                        }                        
                    }
        }); 
        
        map.addLayer(testGeoIndexLayer);
        //交互：点击事件
        var select = new ol.interaction.Select({
            layers:[testGeoIndexLayer],
            style: new ol.style.Style({
                stroke: new ol.style.Stroke({
                  color: 'orange',
                  width: 3
                })
              })
            });
        map.addInteraction(select);
    } 
     
    
});




