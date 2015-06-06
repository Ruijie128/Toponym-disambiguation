using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpMap;
using SharpMap.Forms;
using SharpMap.Layers;
using SharpMap.Data;
using GeoAPI.Geometries;
using SharpMap.Utilities;
using SharpMap.Rendering;
using GeoAPI;
using System.IO;

namespace shpToWkt
{
    public partial class Form1 : Form
    {
        public static string file;
        public static SharpMap.Layers.VectorLayer vlay = new SharpMap.Layers.VectorLayer("newLayer");
        
        public Form1()
        {
            InitializeComponent();
            
          /*  if (ds == null) return;
            System.Data.DataTable dt = new DataTable();
            System.Data.DataColumn dc = new DataColumn("名字", typeof(System.String));
            dt.Columns.Add(dc);
            foreach (SharpMap.Data.FeatureDataRow row in ds.Tables[0])
            {
                if (row["NAME"].ToString().Contains(this.txtQuery.Text.Trim()))
                {
                    System.Data.DataRow r = dt.NewRow();
                    r[0] = row["NAME"].ToString();
                    dt.Rows.Add(r);
                }
            }
            this.gvResults.DataSource = dt;
            this.gvResults.DataBind();
            //重新生成地图
            this.GenerateMap();*/
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "(*.shp*)|*.shp*";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                
                file = openFileDialog1.FileName;}
           // SharpMap.Layers.VectorLayer vlay = new SharpMap.Layers.VectorLayer("newLayer");
            if (file == null)
            {
                MessageBox.Show("请选择shp文件");
                return;
            }
            vlay.DataSource = new SharpMap.Data.Providers.ShapeFile(file, true);
           // vlay.Render(new Graphics(),mapBox1.Map);
            mapBox1.Map.Layers.Add(vlay);
            mapBox1.Map.ZoomToExtents();

            mapBox1.Refresh();

            
        }

        private void transform_Click(object sender, EventArgs e)
        {

            if (vlay == null) return;
            if (vlay.DataSource == null)
                MessageBox.Show("请先载入shp文件");
            else
            {
                if (!vlay.DataSource.IsOpen) vlay.DataSource.Open();
                SharpMap.Data.Providers.ShapeFile shapefile = (SharpMap.Data.Providers.ShapeFile)vlay.DataSource;
                int geoCount = shapefile.GetFeatureCount();
                string writefile = null;
                saveFileDialog1.Filter = "(*.txt*)|*.txt";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    writefile = saveFileDialog1.FileName;
                }
                if (writefile == null)
                {
                    MessageBox.Show("输出文件不能为空，请正确选择");
                    return;
                }
                FileStream fs = new FileStream(writefile, FileMode.Append);
                StreamWriter sw = new StreamWriter(fs, Encoding.Default);
                for (int i = 0; i < geoCount; i++)
                {

                    GeoAPI.Geometries.IGeometry geo = shapefile.GetGeometryByID(Convert.ToUInt32(i));
                    sw.Write(geo + "\n");
                }

                //   Console.WriteLine(geo);
                vlay.DataSource.Close();



                sw.Close();
                fs.Close();
                MessageBox.Show("文件已转换完毕！");
            }
            
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }
    }
}
