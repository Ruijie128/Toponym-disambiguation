function gridLayer8() {
	//return "http://localhost:8080/examples/json/gridLayer8int.geojson";
	// return "http://localhost:2941/gridLayer8int.geojson";
}

function testGeoIndex(){
   /* result = {}
    cursor = connection.cursor()
    // # sql = "SELECT `place`, `count` FROM `testGeoIndex` order by `place`"
    sql = "SELECT `name`, `fileCount` FROM `militaryfiles`.`geometoryfiles` GF, `sse`.`show_place` SP WHERE GF.geometryID = SP.FeatureID"
    cursor.execute(sql)
    fetchall = cursor.fetchall()
    
    data = []
    if(len(fetchall) != 0):
        for obj in fetchall:
            item = {}
            try:
                p = Place.objects.filter(name=obj[0])[0] # wanglin to do： 针对地中海被分为两处的特殊情况
                item['name'] = p.name
                item['count'] = obj[1]
                item['geocodes8'] = p.geocodes8
                item['barygeocode8'] = p.barygeocode8
                data.append(item)
            except:
                print "cannot find " + obj[0]
    result['data'] = data
    result['status'] = 'ok'    
    return HttpResponse(json.dumps(result), content_type="application/json") */
    // 返回一个json格式的文件
   /* var result = {};
    var data = {};
    var item ;
    item.append({"name":document.getElementById("txtKeyword")}).append({"count": 5});


        Class.ForName("com.mysql.jdbc.Driver");
 
    // catch (Exception e) {}

    Connection con = null;
    Statement sql = null   ;
    ResultSet rs = null;

    try{
        con = DriverManager.getConnection("jdbc:mysql://localhost/scutcs","root","zhangruijie");
        sql = con.createStatement();
        var sqlString = "select geocodes8,barygeocode8 from show_place where name = "+sqlName;
        rs = sql.executeQuery(sqlString);
        if(rs.next())
        {
            item.append({"geocodes8": rs[0], "barygeocode8":rs[1]});
        }
        else
        {
            Console.write("can't find in the database");
        }
    }
    result.append({"data": data, "status":"ok"});
    return HttpResponse(json.dumps(result), content_type="application/json") ; */
    // return "http://localhost:2941/jason.txt";

 }