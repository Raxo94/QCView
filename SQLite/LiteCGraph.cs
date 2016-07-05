//20130204 MARO Max scale on Yield graph set to 100% in "private void SetYieldAxis(Excel.Chart chartpage)"
//20130404 MARO Converted to VS 2012 + reworked to use ms chart instead of excel interop
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Text;
using System.Media;
using System.Drawing.Imaging;
using Config;
using System.IO;
//using SQLite;
using CMYSQLN;
using Stringer;
using System.Globalization;
using System.Windows.Forms.DataVisualization;
using System.Windows.Forms.DataVisualization.Charting;
namespace LiteGraph
{
    enum CVType {Expected, Absolute}; 
    class CGraph
    {
       
        private string AppPath;
        private CStringer users = new CStringer();
        private Chart ichart;
        private string curdate;
        private string curweek;
        private bool showlab;
        private Collection<CParameter> yieldtexts = new Collection<CParameter>();
        private Collection<string> yieldserials = new Collection<string>();
        //Drawing
        public CGraph(Control parent)
        {
            AppPath = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
            CConfig cfg = new CConfig("Users.xml");
            users = cfg.ReadStringer();
        }
        public CGraph(Chart chart, Control parent)
        {
            ichart = chart;
            AppPath = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
            CConfig cfg = new CConfig("Users.xml");
            users = cfg.ReadStringer();
        }
        public CGraph(Chart chart, Control parent, Collection<CParameter> yields)
        {
            yieldtexts = yields;
            foreach (CParameter x in yields)
            {
                yieldserials.Add(x.Name.Split('.')[1]);
            }
            ichart = chart;
            AppPath = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
            CConfig cfg = new CConfig("Users.xml");
            users = cfg.ReadStringer();
            ichart.MouseDoubleClick += ichart_MouseDoubleClick;
        }

        void ichart_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var pos = e.Location;
            var results = ichart.HitTest(pos.X, pos.Y, false, ChartElementType.DataPoint);
            //foreach (var result in results)
            {
                if (results[0].ChartElementType == ChartElementType.DataPoint)
                {
                    DataPoint point = results[0].Object as DataPoint;
                    var xval = (int)point.XValue;
                    var yval = (int)point.YValues[0];
                    
                    CTextInput2 ct = new CTextInput2();
                    string text = ct.Show("Yield Text");
                    if (MessageBox.Show("Lägga till text för punkt " + xval.ToString() + ":" + yval.ToString() + " ?", "Yield Text", MessageBoxButtons.OKCancel) == DialogResult.OK)
                    {
                        CMYSQL sql = new CMYSQL(TableType.bm800_sample);
                        GraphDataSet gd = sql.miGetYield2(curdate);
                        string name = "SNO." + gd.SNO[xval].ToString() + "." + gd.Y[xval].ToString();
                        CConfig cfg = new CConfig("Yieldtext.xml");
                        cfg.Add(name, text);
                    }
                }
            }

            yieldtexts.Clear();
            yieldserials.Clear();
            CConfig cfb = new CConfig("Yieldtext.xml");
            yieldtexts = cfb.Read();

            foreach (CParameter x in yieldtexts)
            {
                yieldserials.Add(x.Name.Split('.')[1]);
            }
            FillYield(curdate, curweek, showlab);
        }
        public Image GetImage(string name)
        { 
            string path = AppPath + "\\" + name;
            if (File.Exists(path))
                File.Delete(path);
            ichart.SaveImage(path, System.Drawing.Imaging.ImageFormat.Png);
            Image result =  Image.FromFile(path);
            return result;
        }
        //Statistics
        public void FillYield(string date, string weeks, bool showlabel)
        {
            curdate = date;
            curweek = weeks;
            showlab = showlabel;

            CMYSQL sql = new CMYSQL(TableType.bm800_sample);
            GraphDataSet gd = sql.miGetYield2(date);

            ichart.Series[0].Points.Clear();
            ichart.Series[0].MarkerColor = Color.DarkBlue;
            ichart.Series[1].Points.Clear();
            ichart.Series[1].MarkerColor = Color.Orange;
            ichart.ChartAreas[0].AxisX.Minimum = 0;
            ichart.Titles.Clear();
            ichart.Titles.Add("Yield - " + weeks + " veckor");
            
            string cursno = "";
            int i = 0;
            foreach (double y in gd.Y)
            {
                ichart.Series[0].Points.AddXY(i, y);
                if (showlabel)
                {
                    cursno = gd.SNO[i].ToString();
                    if (yieldserials.Contains(cursno) && yieldtexts[yieldserials.IndexOf(cursno)].Name.Split('.')[2] == y.ToString())
                    {
                        ichart.Series[0].Points[ichart.Series[0].Points.Count - 1].IsValueShownAsLabel = true;
                        ichart.Series[0].Points[ichart.Series[0].Points.Count - 1].Label = yieldtexts[yieldserials.IndexOf(cursno)].Value;
                    }
                }
                ichart.Series[1].Points.AddXY(i, 75);
                i++;
            }
        }
        public void FillCV(string date, string weeks, CVType cvtype)
        {
            CMYSQL sql = new CMYSQL(TableType.bm800_sample);
            CConfig cfg = new CConfig();
            string limit = "";

            //get data 
            Collection<GraphDataSet> gd = sql.miGetGraphDataCV(date);
            Collection<Color> colors = new Collection<Color>();
            colors.Add(Color.Red);
            colors.Add(Color.Red);
            colors.Add(Color.Orange);
            colors.Add(Color.Orange);
            colors.Add(Color.Black);
            colors.Add(Color.Black);
            colors.Add(Color.DarkGray);
            colors.Add(Color.DarkGray);
            colors.Add(Color.Blue);
            colors.Add(Color.Blue);
            colors.Add(Color.Yellow);
            colors.Add(Color.Yellow);

            if (cvtype == CVType.Expected) //select if limit series should be expected or absolute limits
                limit = "Exp";
            else
                limit = "Lim";

            string[] names = { "RBC", "RBC_" + limit, "MCV", "MCV_" + limit,
                               "PLT", "PLT_" + limit, "MPV", "MPV_" + limit,
                               "HGB", "HGB_" + limit, "WBC", "WBC_" + limit};
            double[] expected = new double[6] { cfg.ReadD(limit + "_CVRBC"), cfg.ReadD(limit + "_CVMCV"), 
                                                cfg.ReadD(limit + "_CVPLT"), cfg.ReadD(limit + "_CVMPV"), 
                                                cfg.ReadD(limit + "_CVHGB"), cfg.ReadD(limit + "_CVWBC")};

            //clear all old data in chart
            foreach (Series x in ichart.Series)
                x.Points.Clear();
            ichart.Series.Clear();

            ichart.Titles.Clear();
            ichart.Titles.Add("CV - förväntat gränsvärde " + weeks + " veckor");

            //create new series for all parameters
            int i = 0;
            foreach (string x in names)
            {
                Series series = new Series(x);
                if (i % 2 == 0)//first is parameter, next is expected value
                    series.ChartType = SeriesChartType.Line;
                else
                    series.ChartType = SeriesChartType.Point;
                series.Color = colors[i];
                series.MarkerSize = 2;
                series.MarkerStyle = MarkerStyle.None;
                ichart.Series.Add(series);
                i++;
            }

            //fill all series with data
            for (int k = 0; k < gd[0].Y.Count; k++)
            {
                ichart.Series["RBC"].Points.AddY(gd[0].Y[k]);
                ichart.Series["RBC_" + limit].Points.AddY(expected[0]);

                ichart.Series["MCV"].Points.AddY(gd[1].Y[k]);
                ichart.Series["MCV_" + limit].Points.AddY(expected[1]);

                ichart.Series["PLT"].Points.AddY(gd[2].Y[k]);
                ichart.Series["PLT_" + limit].Points.AddY(expected[2]);

                ichart.Series["MPV"].Points.AddY(gd[3].Y[k]);
                ichart.Series["MPV_" + limit].Points.AddY(expected[3]);

                ichart.Series["HGB"].Points.AddY(gd[4].Y[k]);
                ichart.Series["HGB_" + limit].Points.AddY(expected[4]);

                ichart.Series["WBC"].Points.AddY(gd[5].Y[k]);
                ichart.Series["WBC_" + limit].Points.AddY(expected[5]);
            }

        }
        public void FillInstrument(string date, string weeks, CVType cvtype)
        {
            CMYSQL sql = new CMYSQL(TableType.bm800_sample);
            CConfig cfg = new CConfig();

            //get data 
            Collection<GraphDataSet> gd = new Collection<GraphDataSet>();
            gd.Add(sql.miGetSEQnbrs(date, "M-Series"));
            gd.Add(sql.miGetSEQnbrs(date, "Alfa"));
            gd.Add(sql.miGetSEQnbrs(date, "Exigo"));
            Collection<Color> colors = new Collection<Color>();
            colors.Add(Color.Blue);
            colors.Add(Color.Red);
            colors.Add(Color.Green);
            /*
            colors.Add(Color.Black);
            colors.Add(Color.Black);
            */
            string[] names = { "M-Series", "Alfa", "Exigo"};

            //clear all old data in chart
            foreach (Series x in ichart.Series)
                x.Points.Clear();
            ichart.Series.Clear();

            ichart.Titles.Clear();
            ichart.Titles.Add("Instrument - SEQ/Instrument " + weeks + " veckor");

            //create new series for all parameters
            Series series = new Series(names[0]);
            series.ChartType = SeriesChartType.Point;
            series.Color = colors[0];
            series.MarkerSize = 4;
            series.MarkerStyle = MarkerStyle.None;
            ichart.Series.Add(series);
            series = new Series(names[1]);
            series.ChartType = SeriesChartType.Point;
            series.Color = colors[1];
            series.MarkerSize = 4;
            series.MarkerStyle = MarkerStyle.None;
            ichart.Series.Add(series);
            series = new Series(names[2]);
            series.ChartType = SeriesChartType.Point;
            series.Color = colors[2];
            series.MarkerSize = 4;
            series.MarkerStyle = MarkerStyle.None;
            ichart.Series.Add(series);

            //fill all series with data
            for (int k = 0; k < gd[0].Y.Count; k++)
            {
                ichart.Series["M-Series"].Points.AddY(gd[0].Y[k]);
            }
            for (int k = 0; k < gd[1].Y.Count; k++)
            {
                ichart.Series["Alfa"].Points.AddY(gd[1].Y[k]);
            }
            for (int k = 0; k < gd[2].Y.Count; k++)
            {
                ichart.Series["Exigo"].Points.AddY(gd[2].Y[k]);
            }

        }
        public void FillProblem(string date, string weeks)
        {
            CMYSQL sql = new CMYSQL(TableType.bm800_sample);

            int count = 0;
            int val = 0;

            CStringer ids = sql.miGetIDs(date);
            string noprobs = sql.miGetNoProblem(date);
            double noproblems = double.Parse(noprobs);
            int yield = (int)((noproblems / (double)ids.Items.Count) * 100.0);

            val = sql.miGetIssues("CompChange", date);
            count += val;
            double component = val;

            val = sql.miGetIssues("BlanksTomany", date);
            count += val;
            double dirty = val;

            val = sql.miGetReCalConf("Repro", date);
            count += val;
            double repro = val;

            val = sql.miGetReCalConf("ReCalib", date);
            count += val;
            double recalib = val;

            val = sql.miGetReCalConf("Confirm", date);
            count += val;
            double confirm = val;

            string[] xValues = { "Frisläpta Instrument", "Inga Problem", "Yield(%)", "Komponent", "Smutsig", "Repro", "Omkalibrering", "Konf/Nivåer", "Antal Fel" };
            double[] yValues = { ids.Items.Count, noproblems, yield, component, dirty, repro, recalib, confirm, count };

            SetChart("Felutfall - " + weeks + " veckor", xValues, yValues, 20);
        }

        public void FillComponents(string date, string weeks)
        {
            CMYSQL sql = new CMYSQL(TableType.bm800_sample);
            CStringer comps = sql.miComponents(date);
            int dummy = 1;
            List<string> sortedcomps = new List<string>();
            foreach (string x in comps.Items)
            {
                if (!int.TryParse(x, out dummy))
                {
                    if (!x.Contains("Ej Inkluderad")) //x is new db version with component name info
                    {
                        string[] temp = x.Split(':');
                        if (temp.Length == 1) //only one component is replaced
                        {
                            string[] tp = x.Split('-');
                            
                            if (tp.Length >= 3)
                                sortedcomps.Add(tp[1].Trim() + "\r\n" + tp[2]);
                            else
                                sortedcomps.Add(tp[0]);
                        }
                        else //more than one component is replaced
                        {
                            foreach (string y in temp)
                            {
                                string[] tp = y.Split('-');

                                if (tp.Length >= 3)
                                    sortedcomps.Add(tp[1].Trim() + "\r\n" + tp[2]);
                                else
                                    sortedcomps.Add(tp[0]);
                            }
                        }
                    }
                    else //x is not yet implemented in component list 
                    {
                        sortedcomps.Add("Okänd");
                    }
                }
                else
                {
                    //x is old version with number of replaced components
                    for (int i = 0; i < dummy; i++)
                        sortedcomps.Add("Okänd");
                }
                
            }
            sortedcomps.Sort();
            ComponentList cp = new ComponentList();
            foreach(string x in sortedcomps)
            {
                //Get list with distinct names and count
                cp.Add(x);
            }

            int idx = 0;
            string[] xValues = new string[cp.Items.Count];
            double[] yValues = new double[cp.Items.Count];
            foreach (string x in cp.Items)
            {
                xValues[idx] = x;
                yValues[idx] = cp.Count[idx];
                idx++;
            }

            SetChart("Felutfall - komponenter - " + weeks + " veckor", xValues, yValues, 100);
        }
        
        public void FillUser(string date, string weeks)
        {
            CMYSQL sql = new CMYSQL(TableType.bm800_sample);
            string[] names = { "", "Instrument", "Samp/Instr" };
            ColumnGraphSet data1 = new ColumnGraphSet();
            data1.Add(users);

            for (int i = 0; i < data1.Data[0].Items.Count; i++)
            {
                data1.Add(sql.miGetTestedInstruments(data1.Data[0].Items[i].ToString(), date).ToString());
            }
            data1.Merge();
            for (int i = 0; i < data1.Data[0].Items.Count; i++)
            {
                if (data1.Data[1].Items[i].ToString() == "0")
                {
                    data1.Remove(i);
                    i--;
                }
            }

            string[] xValues = data1.GetNames();
            double[] yValues = data1.GetInstruments();

            SetChart("Produktion - " + weeks + " veckor", xValues, yValues, 100);
            
        }
        private void SetChart(string title, string[] xvalues, double[] yvalues, int interval)
        {
            ichart.Titles.Clear();
            ichart.Titles.Add(title);

            ichart.ChartAreas[0].AxisX.Interval = 1;
            ichart.ChartAreas[0].AxisX.MajorGrid.Interval = interval;
            ichart.Series[0].Points.DataBindXY(xvalues, yvalues);
        }
    }// end class
    class ColumnGraphSet
    {
        public Collection<CStringer> Data = new Collection<CStringer>();
        public int ItemsCount = 0;
        public int SubItemsCount = 0;
        private CStringer temp = new CStringer();
        public ColumnGraphSet() {}
        public void Add(ColumnGraphSet items) 
        {
            foreach (CStringer x in items.Data) 
            {
                Data.Add(x);
                ItemsCount ++;
            }
            SubItemsCount = Data[0].Items.Count();
        }
        public void Add(CStringer item) 
        {
            Data.Add(item);
            ItemsCount++;
            SubItemsCount = item.Items.Count();
        }
        public void Add(string value) 
        {
            temp.Add(value);
        }
        public void Merge() 
        {
            Data.Add(temp);
            temp = new CStringer();
            ItemsCount++;
            SubItemsCount = Data[0].Items.Count();
        }
        public void Clear()
        {
            Data.Clear();
            temp.Items.Clear();
            ItemsCount = 0;
            SubItemsCount = 0;
        }
        public void ClearSubItems()
        {
            temp = new CStringer();
            SubItemsCount = Data[0].Items.Count();
        }
        public void Remove(int index)
        {
            Data[0].RemoveAt(index);
            Data[1].RemoveAt(index);
            SubItemsCount--;
        }
        public override string ToString()
        {
            string result = "";
            int i = 0;
            foreach (CStringer x in Data) 
            { 
                result += "Item" + i.ToString() + "\r\n" + x.ToString("\t") + "\r\n\r\n";
                i++;
            }
            return result;
        }
        public int GetNum()
        {
            return Data[0].Items.Count;
        }
        public string[] GetNames()
        {
            string[] result = new string[Data[0].Items.Count];
            int i = 0;
            foreach (String x in Data[0].Items)
            {
                result[i] = x;
                i++;
            }
            return result;
        }
        public double[] GetInstruments()
        {
            double[] result = new double[Data[1].Items.Count];
            int i = 0;
            foreach (String x in Data[1].Items)
            {
                result[i] = double.Parse(x);
                i++;
            }
            return result;
        }
    }//end class
    class ComponentList
    {
        public Collection<string> Items = new Collection<string>();
        public Collection<int> Count = new Collection<int>();
        private int IndexOf(string name)
        {
            int i = -1;
            foreach (string x in Items)
            {
                i++;
                if (x == name)
                    return i;
            }
            i = -1;
            return i;
        }
        public void Add(string name)
        {
            int idx = IndexOf(name);
            if (idx == -1)
            {
                Items.Add(name);
                Count.Add(1);
            }
            else
                Count[idx]++;
        }
        public override string ToString() 
        {
            string result = "";
            int count = 0;
            foreach (string x in Items)
            {
                result += x + "\t" + Count[count].ToString() + "\r\n";
                count++;
            }
            return result;
        }
    }//end class
}//end namespace
