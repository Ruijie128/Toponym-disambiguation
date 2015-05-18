def gridLayer8(request):
    return render_to_response("show/gridLayer8int.geojson", content_type='application/json')
    # json_data = open('../gridLayer8.geojson')
    # result = json.dumps(json_data)
    # json_data.close()
    # return HttpResponse(json.dumps(result), content_type="application/json") 
    
    
    
def testGeoIndex(request):
    result = {}
    cursor = connection.cursor()
    # sql = "SELECT `place`, `count` FROM `testGeoIndex` order by `place`"
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
    return HttpResponse(json.dumps(result), content_type="application/json") 