using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Collections;
using System.Data.OleDb;
using System.IO;
using JuliService.library;

namespace JuliService
{
    

    class Program
    {
        public static EvidentialReason _rr = new EvidentialReason();
        public static int count = 3628;
        static int[,] distance = new int[count, count];
        public static int MAX = 99999;
       
        public static int polyline = 0;
        static void Main(string[] args)
        {
            //在初始化的时候读取所有的相邻关系以及包含关系  完善关系的存储结构
            string connStr = "Database='administrativeregion';Data Source='localhost';User Id='root';Password='zhangruijie';charset='utf8';pooling=true";
            MySqlConnection mysql = new MySqlConnection(connStr);
            

            #region  创建距离图
            ///summary
            ///
            ArrayList firstList = new ArrayList();
            ArrayList secondList = new ArrayList();
            ArrayList firstFeatureIDList = new ArrayList(); ArrayList secondFeatureIDList = new ArrayList();


            
           /* StreamReader xiangLinRel = new StreamReader("E:\\省市\\xianglin.txt", Encoding.Default);
            string xianglinStr = xiangLinRel.ReadLine();
            
            while (xianglinStr!=null)
            {
                string[] newStr = xianglinStr.Split(',');
                firstList.Add(int.Parse(newStr[2]));
                secondList.Add(int.Parse(newStr[3]));
                xianglinStr = xiangLinRel.ReadLine();
            }

            xiangLinRel.Close();

            ArrayList firstFeatureIDList = new ArrayList(); ArrayList secondFeatureIDList = new ArrayList();
            for (int i = 0; i < firstList.Count; i++)
            {
                mysql.Open();
                string sqlFirstStr = "select featureID from vocabulary where qydm =" + Convert.ToInt32(firstList[i]);
                MySqlCommand sqlFirstComm = new MySqlCommand(sqlFirstStr, mysql);
                MySqlDataReader sqlFirstReader = sqlFirstComm.ExecuteReader();
                while (sqlFirstReader.Read())
                {
                    firstFeatureIDList.Add(Convert.ToInt32(sqlFirstReader[0]));
                }
                mysql.Close();

                mysql.Open();
                string sqlSecondStr = "select featureID from vocabulary where qydm =" + Convert.ToInt32(secondList[i]);
                MySqlCommand sqlSecondComm = new MySqlCommand(sqlSecondStr, mysql);
                MySqlDataReader sqlSecondReader = sqlSecondComm.ExecuteReader();
                while (sqlSecondReader.Read())
                {
                    secondFeatureIDList.Add(Convert.ToInt32(sqlSecondReader[0]));
                }
                mysql.Close();
            }*/
            mysql.Open();
            string xiangLinStr = "select featureID, RelatedFeatureID from relationship where RelationType = 2 and featureID< 3628 and RelatedFeatureID<3628";
            MySqlCommand xiangLinComm = new MySqlCommand(xiangLinStr, mysql);
            MySqlDataReader xiangLinReader = xiangLinComm.ExecuteReader();
            while (xiangLinReader.Read())
            {
                firstFeatureIDList.Add(Convert.ToInt32(xiangLinReader[0]));
                secondFeatureIDList.Add(Convert.ToInt32(xiangLinReader[1]));
            }
            mysql.Close();

            

            //同时录入包含关系
            ArrayList thirdList = new ArrayList(); ArrayList fourthList = new ArrayList();
            mysql.Open();
            string baohanStr = "select featureID, RelatedFeatureID from relationship where RelationType = 1 and featureID< 3628 and RelatedFeatureID<3628";
            MySqlCommand baohanComm = new MySqlCommand(baohanStr, mysql);
            MySqlDataReader baohanReader = baohanComm.ExecuteReader();
            while (baohanReader.Read())
            {
                    thirdList.Add(Convert.ToInt32(baohanReader[0]));
                    fourthList.Add(Convert.ToInt32(baohanReader[1]));
             }
              mysql.Close();

              List<int> tempList = new List<int>();
            //ZHUSHI
              for (int i = 0; i < count; i++)
              {
                  distance[0, i] = MAX;
                  distance[i, 0] = MAX;
              }
                  for (int i = 1; i < count; i++)
                  {

                      for (int j = 1; j < count; j++)
                      {
                          if (i == j)
                              distance[i, j] = 0;
                          else
                              distance[i, j] = MAX;
                      }
                  }
              
                  for (int i = 0; i < firstFeatureIDList.Count; i++)
                  {
                      distance[Convert.ToInt32(firstFeatureIDList[i]),Convert.ToInt32(secondFeatureIDList[i])] = 1;
                  }

              for (int i = 0; i < thirdList.Count; i++)
              {
                  distance[Convert.ToInt32(thirdList[i]), Convert.ToInt32(fourthList[i])] = 0;
              }
            

            #endregion

              #region 手动测试集

              Console.WriteLine("选择证据合成算法。其中Y is Yager, A is Avg, D is DS. 请输入简写");
              string evidenceStr = Console.ReadLine();
              switch (evidenceStr)
              {
                  case "Y":
                      _rr.AggregateType = AggregateType.Yager;
                      break;
                  case "A":
                      _rr.AggregateType = AggregateType.Avg;
                      break;
                  default:
                      _rr.AggregateType = AggregateType.DS;
                      break;
              }
            
              Console.WriteLine("是否输入测试数据，是的话选择“Y”, 否则选择“NO”");
              string strReadeConso = Console.ReadLine();
             // List<List<double>> evidenDistance = new List<List<double>>();
              List<double> tempUn = new List<double>();
              while (strReadeConso == "Y")
              {
                  Console.WriteLine("请输入需要计算的地名，请首先输入测试重名地名，并以逗号隔开：");
                  string strTest = Console.ReadLine();
                  string[] temp = strTest.Split('，');
                  ArrayList testDupName = DupName(temp[0], mysql);
                  Console.WriteLine("计算得到的距离矩阵为：");
                  for (int i = 0; i < temp.Length; i++)
                      Console.Write(temp[i]+"  ");
                  Console.Write("\n");
                  //List<double> tempForList = new List<double>();
                  int tempCount=testDupName.Count+1;
                  double[][] evidenDistance = new double[temp.Length-1][];
                  for(int i=0;i<temp.Length-1;i++)
                      evidenDistance[i] = new double[tempCount];
                  ArrayList tempSet = new ArrayList();
                  tempUn.Clear();double tempForR=1;
                  for (int i = 1; i < temp.Length; i++)
                  {
                      tempForR = 1;
                      //tempForList.Clear();
                      int j;
                      for ( j = 0; j < testDupName.Count; j++)
                      {
                          mysql.Open();
                          string testFeatureSqlStr = "select featureID from vocabulary where Term like '%" + temp[i] + "%' and featureID< 3628";
                          MySqlCommand testFeatureIDComm = new MySqlCommand(testFeatureSqlStr, mysql);
                          MySqlDataReader testFeatureReader = testFeatureIDComm.ExecuteReader();
                          int testFeatureID = 0;
                          while (testFeatureReader.Read())
                          {
                              testFeatureID = Convert.ToInt32(testFeatureReader[0]);
                          }
                          mysql.Close();
                          ArrayList testDistance = calcuDist(Convert.ToInt32(testDupName[j]), testFeatureID, mysql);
                          tempForR -= rel(Convert.ToInt32(testDistance[2])) / (testDupName.Count + 1);
                          evidenDistance[i-1][j] = Math.Round(Convert.ToDouble(rel(Convert.ToInt32(testDistance[2])) / (testDupName.Count + 1)),3);
                      }
                      evidenDistance[i-1][j]= Math.Round(tempForR,3);
                  }

                  string name_string = null;
                  for (int i = 0; i < testDupName.Count; i++)
                  {
                       Console.Write(temp[0] + "  ");
                       for (int j = 1; j < temp.Length; j++)
                       {
                           Console.Write(evidenDistance[j-1][i] + "  " );
                       }
                      // tempSet.Add(temp[0]+testDupName[i]);
                       tempSet.Add(Convert.ToString((char)('a'+i)));
                      name_string+=tempSet[i];
                       Console.Write("\n");
                  }
           //       tempSet.Add("uncertain");
                  tempSet.Add(name_string);
                   /*   for (int i = 0; i < testDupName.Count; i++)
                      {
                          Console.Write(temp[0] + "  ");
                          tempForList.Clear();
                          tempForR = 1;
                          for (int j = 1; j < temp.Length; j++)
                          {
                              mysql.Open();
                              string testFeatureSqlStr = "select featureID from vocabulary where Term like '%" + temp[j] + "%'";
                              MySqlCommand testFeatureIDComm = new MySqlCommand(testFeatureSqlStr, mysql);
                              MySqlDataReader testFeatureReader = testFeatureIDComm.ExecuteReader();
                              int testFeatureID = 0;
                              while (testFeatureReader.Read())
                              {
                                  testFeatureID = Convert.ToInt32(testFeatureReader[0]);
                              }
                              mysql.Close();
                              ArrayList testDistance = calcuDist(Convert.ToInt32(testDupName[i]), testFeatureID, mysql);
                              Console.Write(testDistance[2] + "  ");
                              tempForR -= rel(Convert.ToInt32(testDistance[2])) / (testDupName.Count + 1);
                              tempForList.Add(rel(Convert.ToInt32(testDistance[2])));

                              // Console.Write(rel(Convert.ToInt32(testDistance[2])) + "  ");
                          }
                          // tempForList.Add(tempForR);

                          tempSet.Add(testDupName[i] + temp[0]);
                          evidenDistance.Add(tempForList);
                          Console.Write("\n");
                      }
                  tempUn.Add(tempForR);
                  evidenDistance.Add(tempUn);*/
                  /*
                    _rr.SetData(distance_1, distance_2, distance_1.Count, 2, tempSet);
                  string[] element = _rr.GetElements();
                  
                  if (_rr.Reasoning())
                  {
                      double[] result = _rr.GetResult();
                      
                      string value = null;
                      for (int j = 0; j < _rr.ElementCount; j++)
                      {
                          value += "m({" + element[j] + "})=" + (result[j] / _rr.Scale).ToString("#0.000") + "\r\n";
                      }
                      swR.Write(value);
                      
                  }
                   */

                  _rr.SetData(evidenDistance, testDupName.Count+1, temp.Length-1 , tempSet);
                  string[] element = _rr.GetElements();
                  if (_rr.Reasoning())
                  {
                      double[] result = _rr.GetResult();

                      string value = null;
                      for (int j = 0; j < _rr.ElementCount; j++)
                      {
                          value += "m({" + element[j] + "})=" + (result[j] / _rr.Scale).ToString("#0.000") + "\r\n";
                      }
                     // swR.Write(value);
                      Console.WriteLine(value);

                  }
              }
              #endregion

              #region 计算测试集合

              
             

              StreamReader testStreamReader = new StreamReader("E:\\users\\zhang Ruijie\\中国省市数据\\qu.txt", Encoding.Default);
              string testStr = testStreamReader.ReadLine();
              ArrayList testFirstList = new ArrayList();
              int endPoint1 = 870, endPoint2 = 869; 
              while (testStr != null)
              {
                  testFirstList.Add(testStr);
                  testStr = testStreamReader.ReadLine();
              }
              testStreamReader.Close();
              Double distance1 = 0, distance2 = 0; string strNew; strNew = null;
              ArrayList  testFeatureIDList = new ArrayList(); ArrayList nameList = new ArrayList();
              ArrayList distance_1 = new ArrayList(), distance_2 = new ArrayList();
             // ArrayList tempSet = new ArrayList();
              StreamWriter swR = new StreamWriter("C:\\result.txt");
              for (int i = 0; i < testFirstList.Count; i++)
              {
                  distance_1.Clear(); distance_2.Clear();
                  strNew += "start" + "\r\n";
                  Double sum1 = 1, sum2 = 1;
                  ArrayList tempName = DupName(Convert.ToString(testFirstList[i]), mysql);
                  //  tempSet.Clear();
                  strNew += tempName.Count + 1 + " " + 2 + "\r\n";
                  for (int j = 0; j < tempName.Count; j++)
                  {
                      testFeatureIDList.Add(Convert.ToInt32(DupName(Convert.ToString(testFirstList[i]), mysql)[j]));
                      nameList.Add(Convert.ToString(testFirstList[i]));

                      distance1 = rel(Convert.ToInt32(calcuDist(Convert.ToInt32(tempName[j]), endPoint1, mysql)[2]));
                      distance2 = rel(Convert.ToInt32(calcuDist(Convert.ToInt32(tempName[j]), endPoint2, mysql)[2]));
                      strNew += testFirstList[i] + Convert.ToString(tempName[j]) + " " + distance1 + " " + distance2 + "\r\n";
                      distance_1.Add(distance1); distance_2.Add(distance2);
                   //   tempSet.Add(testFirstList[i] + Convert.ToString(tempName[j]));
                      sum1 -= distance1; sum2 -= distance2;
                  }
                  strNew += "UNcertain" + " " + sum1 + " " + sum2 + "\r\n";
                  distance_1.Add(sum1); distance_2.Add(sum2);
                //  tempSet.Add(testFirstList[i] + "UNcertain");
               //   _rr.SetData(distance_1, distance_2, distance_1.Count, 2, tempSet);
                  string[] element = _rr.GetElements();
                  
                  if (_rr.Reasoning())
                  {
                      double[] result = _rr.GetResult();
                      
                      string value = null;
                      for (int j = 0; j < _rr.ElementCount; j++)
                      {
                          value += "m({" + element[j] + "})=" + (result[j] / _rr.Scale).ToString("#0.000") + "\r\n";
                      }
                      swR.Write(value);
                      
                  }
                  
              }
              swR.Close();
              string filename = "C:\\service.txt";
              StreamWriter sw = new StreamWriter(filename);
              sw.Write(strNew);
              sw.Close();
              
              

              
              ArrayList rDistance = new ArrayList(); 
           /* for (int i = 0; i < testFeatureIDList.Count; i++)
            {
               // rDistance= calcuDist(Convert.ToInt32(testFeatureIDList[i]), endPoint, mysql);
                strNew += rDistance[0]+Convert.ToString(nameList[i]) + "和" + rDistance[1] + "之间的距离是" + rDistance[ 2] + "\r\n";
            }
            */
            
            
            #endregion


            


        }
        private static ArrayList calcuDist(int fromNum, int toNum, MySqlConnection mysql)
        {
            ArrayList rDistance=new ArrayList();
            int beginCate = 0, endCate = 0;
            string cateBeginStr = "select Category from vocabulary where featureID =" + fromNum;
            mysql.Open();
            MySqlCommand cateBeginComm = new MySqlCommand(cateBeginStr, mysql);
            MySqlDataReader cateBeginReader = cateBeginComm.ExecuteReader();
            while (cateBeginReader.Read())
            {
                beginCate = Convert.ToInt16(cateBeginReader[0]);
            }
            mysql.Close();

            string cateEndStr = "select Category from vocabulary where featureID =" + toNum;
            mysql.Open();
            MySqlCommand cateEndComm = new MySqlCommand(cateEndStr, mysql);
            MySqlDataReader cateEndReader = cateEndComm.ExecuteReader();
            while (cateEndReader.Read())
            {
                endCate = Convert.ToInt16(cateEndReader[0]);
            }
            mysql.Close();

            if (beginCate == endCate)
            {
                int tempdistance = Dijkstra(fromNum, toNum);
                rDistance.Add(fromNum);
                rDistance.Add(toNum);
                rDistance.Add(tempdistance);
            }
            else if (Math.Abs(beginCate - endCate) == 1)
            {
                int temNum = 0; int distanceNum = 0;
                if (beginCate > endCate)
                {
                    temNum = toNum;
                    distanceNum = fromNum;
                }
                else
                {
                    temNum = fromNum;
                    distanceNum = toNum;
                }
                mysql.Open();
                ArrayList uperList_1 = new ArrayList();
                string sqlUperStr_1 = "select featureID from relationship where RelatedFeatureID =" + Convert.ToInt32(temNum) + " and RelationType=1  and featureID< 3628 and RelatedFeatureID<3628";
                MySqlCommand sqlUperComm_1 = new MySqlCommand(sqlUperStr_1, mysql);
                MySqlDataReader sqlUperReader_1 = sqlUperComm_1.ExecuteReader();
                while (sqlUperReader_1.Read())
                {
                    uperList_1.Add(Convert.ToInt16(sqlUperReader_1[0]));
                }
                mysql.Close();

                int distance; distance = 999999;
                for (int m = 0; m < uperList_1.Count; m++)
                {
                    int temp;
                    temp = Dijkstra(distanceNum, Convert.ToInt32(uperList_1[m]));
                    if (temp < distance)
                        distance = temp;
                }

                /* mysql.Open();
                int lowerFeatureID_1; lowerFeatureID_1 = 0;
                string sqlLowerStr_1 = "select RelatedFeatureID from relationship where featureID =" + fromNum + " and RelationType=1  and featureID< 3628 and RelatedFeatureID<3628";
                MySqlCommand sqlLowerComm_1 = new MySqlCommand(sqlLowerStr_1, mysql);
                MySqlDataReader sqlLowerReader_1 = sqlLowerComm_1.ExecuteReader();
                while (sqlLowerReader_1.Read())
                {
                    lowerFeatureID_1 = Convert.ToInt16(sqlLowerReader_1[0]);
                }
                mysql.Close();
                int lowerDis;
                lowerDis = Dijkstra(lowerFeatureID_1, toNum);
                int minDistance = distance;
                if (distance > lowerDis)
                    minDistance = lowerDis;
                else
                    minDistance = distance;8*/

                rDistance.Add(fromNum);
                rDistance.Add(toNum);
                rDistance.Add(distance);
               // rDistance.Add(rel(minDistance));
            }

            else if (Math.Abs(beginCate - endCate) == 2)
            {
                int temNum = 0; int distanceNum = 0;
                if (beginCate > endCate)
                {
                    temNum = toNum;
                    distanceNum = fromNum;
                }
                else
                {
                    temNum = fromNum;
                    distanceNum = toNum;
                }
                mysql.Open();
                ArrayList uperList_1 = new ArrayList();
                string sqlUperStr_1 = "select featureID from relationship where RelatedFeatureID =" + Convert.ToInt32(temNum) + " and RelationType=1  and featureID< 3628 and RelatedFeatureID<3628";
                MySqlCommand sqlUperComm_1 = new MySqlCommand(sqlUperStr_1, mysql);
                MySqlDataReader sqlUperReader_1 = sqlUperComm_1.ExecuteReader();
                while (sqlUperReader_1.Read())
                {
                    uperList_1.Add(Convert.ToInt16(sqlUperReader_1[0]));
                }
                mysql.Close();

                ArrayList nextUperList = new ArrayList();
                for (int m = 0; m < uperList_1.Count; m++)
                {
                    mysql.Open();
                    string sqlNextUperStr_1 = "select featureID from relationship where RelatedFeatureID =" + uperList_1[m] + " and RelationType=1  and featureID< 3628 and RelatedFeatureID<3628";
                    MySqlCommand sqlNextUperComm = new MySqlCommand(sqlNextUperStr_1, mysql);
                    MySqlDataReader sqlNextReader = sqlNextUperComm.ExecuteReader();
                    while (sqlNextReader.Read())
                    {
                        nextUperList.Add(Convert.ToInt32(sqlNextReader[0]));
                    }
                    mysql.Close();
                }


                int distance; distance = 999999;
                for (int m = 0; m < nextUperList.Count; m++)
                {
                    int temp;
                    temp = Dijkstra(distanceNum, Convert.ToInt32(nextUperList[m]));
                    if (temp < distance)
                        distance = temp;
                }

             /*   mysql.Open();
                int lowerFeatureID_1; lowerFeatureID_1 = 0;
                string sqlLowerStr_1 = "select RelatedFeatureID from relationship where featureID =" + fromNum + " and RelationType=1  and featureID< 3628 and RelatedFeatureID<3628";
                MySqlCommand sqlLowerComm_1 = new MySqlCommand(sqlLowerStr_1, mysql);
                MySqlDataReader sqlLowerReader_1 = sqlLowerComm_1.ExecuteReader();
                while (sqlLowerReader_1.Read())
                {
                    lowerFeatureID_1 = Convert.ToInt16(sqlLowerReader_1[0]);
                }
                mysql.Close();

                mysql.Open();
                int lowerNextFeatureID_1; lowerNextFeatureID_1 = 0;
                string sqlNextLowerStr_1 = "select RelatedFeatureID from relationship where featureID =" + lowerFeatureID_1 + " and RelationType=1  and featureID< 3628 and RelatedFeatureID<3628";
                MySqlCommand sqlNextLowerComm_1 = new MySqlCommand(sqlNextLowerStr_1, mysql);
                MySqlDataReader sqlNextLowerReader_1 = sqlNextLowerComm_1.ExecuteReader();
                while (sqlNextLowerReader_1.Read())
                {
                    lowerNextFeatureID_1 = Convert.ToInt16(sqlNextLowerReader_1[0]);
                }
                mysql.Close();

                int lowerDis;
                lowerDis = Dijkstra(lowerNextFeatureID_1,toNum);
                int minDistance = distance;
                if (distance > lowerDis)
                    minDistance = lowerDis;
                else
                    minDistance = distance;*/

                rDistance.Add(fromNum);
                rDistance.Add(toNum);
                rDistance.Add(distance);
            }
            return rDistance;
        }

        private static ArrayList DupName(string nameStr, MySqlConnection newSql)
        {
            ArrayList dupname = new ArrayList();
            string nameSqlStr = "select featureID from vocabulary where Term like '%" + nameStr + "%' and featureID< 3628 ";
            newSql.Open();
            MySqlCommand nameSqlComm = new MySqlCommand(nameSqlStr, newSql);
            MySqlDataReader nameReader = nameSqlComm.ExecuteReader();
            while (nameReader.Read())
            {
                dupname.Add(Convert.ToInt32(nameReader[0]));
            }
            newSql.Close();
            return dupname;
        }

        ///Dijkstra 算法计算距离
        private static int Dijkstra(int fromNum, int toNum)
        {
            
            int[] visited=new int[count];
            for (int i = 0; i < count; i++)
            {
                visited[i] = 0;
            }
            int[] minDistance = new int[count];
            int MinEdge, Vertex;
            int k = 0, t = 0;
            int[,] path=new int[4*count, 2];
            int[,] order=new int[4*count, 2];
            for (int i = 0; i < 4 * count; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    order[i, j] = 0;
                }
            }
            int Edges = 0;
            visited[fromNum] = 1;
            minDistance[0] = MAX;
            for (int i = 1; i < count; i++)
                minDistance[i] = distance[fromNum,i];
            minDistance[fromNum] = 0;
            while (Edges < (count - 1))
            {
                Edges++;
                MinEdge = MAX;
                Vertex = 0;
                for (int j = 0; j < count; j++)
                {
                    if (visited[j] == 0 && (MinEdge > minDistance[j]))
                    {
                        Vertex = j;
                        MinEdge = minDistance[j];
                        path[k, t] = j;
                        t++;
                        path[k, t] = Vertex;
                        t--; k++;
                    }
                }

                visited[Vertex] = 1;
                for (int j = 0; j < count; j++)
                {
                    if (visited[j] == 0 && (minDistance[Vertex] + distance[Vertex,j] < minDistance[j]))
                    {
                        minDistance[j] = minDistance[Vertex] + distance[Vertex,j];
                        path[k, t] = j;
                        t++;
                        path[k, t] = Vertex;
                        t--; k++;
                    }
                }
            }

            for (int i = 0; i < k; i++)
            {
                if (path[i, 0] != path[i, 1])
                    order[path[i, 0], 0] = path[i, 1];
            }

            for (int i = 0; i < count; i++)
            {
                if (order[i, 0] == 0)
                    order[i, 0] = fromNum;
            }

            return minDistance[toNum];
        }

        private static double rel(int num)
        {
            /// R=1.25 p=1 a=1 ds=1
            double p = 1, a = 0.8, r = 1.25;
            double relation = Math.Round( p / (a * Math.Pow(r, num + 2)), 3);
            return relation;
        }
    }
}
