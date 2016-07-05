using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;
using Config;
using System.Drawing;
using System.IO;
using Stringer;
using MySql.Data.MySqlClient;
namespace CMYSQLN
{
    enum ORDER { ASC, DESC};
    class CMYSQL
    {
        private CConfigConnection cfg = new CConfigConnection();
        /*
        public CMYSQL()
        {
            CConfig sort = new CConfig();
            Sorter = sort.Read("Sort_Order", ':');
        }*/
        public CMYSQL(TableType type){}
       
        //Get Parameters
        public int miGetTestedInstruments(string user, string week)
        {
            string query = "SELECT id FROM " + cfg.Table_Manual + " WHERE User='" + user + "'" + 
                           " AND Week>'" + week + "'";
            return ExecuteReaderC(cfg.ConnectionM, query).Items.Count;
        }
        public int miGetSamplesPerInstrument(string user, string week)
        {
            string query = "SELECT DISTINCT Serial FROM " + cfg.Table_Manual + " WHERE User='" + user + "'" +
                           " AND Week>'" + week + "'";
            double count = 0;
            double ncount = 0;
            double val = 0;
            foreach (string x in ExecuteReaderC(cfg.ConnectionM, query).Items)
            {
                ncount = (double)InfoRowCountS(x);
                val += ncount;
                count++;
            }
            if (count > 0)
                val = val / count;
            else
                val = 0;
            return (int)val;
        }
        public string miGetNoProblem(string weeks)
        {
            string query = "SELECT id FROM " + cfg.Table_Manual + " WHERE Week>'" + weeks + "'" +
                            " AND BlanksTomany IS NULL AND CompChange IS NULL";
            return ExecuteReaderC(cfg.ConnectionM, query).Items.Count.ToString(); 
        }
        public int miGetIssues(string column, string weeks)
        {
            string query;
            if (column != "BlanksTomany")
                query = "SELECT " + column + " FROM " + cfg.Table_Manual + " WHERE Week>'" + weeks + "'" +
                        " AND CompChange IS NOT NULL";
            else
                query = "SELECT " + column + " FROM " + cfg.Table_Manual + " WHERE Week>'" + weeks + "'";

            int count = 0;
            int iout = 1;
            foreach (string x in ExecuteReaderC(cfg.ConnectionM, query).Items)
            {
                iout = 1;
                if (int.TryParse(x, out iout))
                    count += iout;
                else if (x != null)
                {
                    string[] tp = x.Split(':');
                    count += tp.Length;
                }
                else
                    MessageBox.Show("que?");

            }

            return count;
        }
        public int miGetReCalConf(string column, string weeks)
        {
            string query;
            query = "SELECT " + column + " FROM " + cfg.Table_Manual + " WHERE Week>'" + weeks + "'";

            int count = 0;
            int iout;
            foreach (string x in ExecuteReaderC(cfg.ConnectionM, query).Items)
            {
                if (int.TryParse(x, out iout) || (x != ""))
                    count += iout;
                else
                    MessageBox.Show("que?");

            }

            return count;
        }

        //Get Graphdata
        public Collection<GraphDataSet> miGetGraphDataCV(string weeks)
        {
            string query = "SELECT CVRBC_AVG50, CVMCV_AVG50, CVPLT_AVG50, " +
                            "CVMPV_AVG50, CVHGB_AVG50, CVWBC_AVG50" +
                            " FROM " + cfg.Table_Manual + " WHERE Week>'" + weeks
                            + "' AND Model !='exigo' AND Model !='EOS' AND CVPLT IS NOT NULL ORDER BY id";
            return ExecuteReaderGDS(cfg.ConnectionM, query);
        }
        public GraphDataSet miGetYield(string weeks)
        {
            string query = "SELECT Serial, Yield FROM " + cfg.Table_Manual + " WHERE Week>'" + weeks
                            + "' ORDER BY id";
            GraphDataSet result = ExecuteReaderGDSS(cfg.ConnectionM, query, "Yield")[0];
            result.limit = 75;
            return result;
        }
        public GraphDataSet miGetYield2(string weeks)
        {
            string query = "SELECT Serial, Yield FROM " + cfg.Table_Manual + " WHERE Week>'" + weeks
                            + "' ORDER BY id";
            GraphDataSet result = ExecuteReaderGDSS(cfg.ConnectionM, query, "Yield", "Serial")[0];
            result.limit = 75;
            return result;
        }
        public GraphDataSet miGetSEQnbrs(string weeks, string model)
        {
            string query = "SELECT Serial, Week, SEQnbrs FROM " + cfg.Table_Manual + " WHERE Week>'" + weeks
                            + "' AND Model = '" + model + "' AND Extra IS NULL ORDER BY id";
            GraphDataSet result = ExecuteReaderGDSSWeek(cfg.ConnectionM, query, "SEQnbrs")[0];
            result.limit = 75;
            return result;
        }
        public CStringer miComponents(string weeks)
        {
            string query = "SELECT CompChange FROM " + cfg.Table_Manual + " WHERE Week>'" + weeks
                            + "' AND CompChange IS NOT NULL ORDER BY id";
            return ExecuteReaderC(cfg.ConnectionM, query);
        }

        //Master ID's
        public CStringer miGetIDs(string weeks)
        {
            string query = "SELECT id FROM " + cfg.Table_Manual + " WHERE Week>'" + weeks + "'";
            MessageBox.Show(query);
            return ExecuteReaderC(cfg.ConnectionM, query);
        }

        //Execute Query
        private Collection<GraphDataSet> ExecuteReaderGDS(string connection, string query)
        {
            DataTable dt = new DataTable();
            GraphDataSet temp;
            Collection<GraphDataSet> result = new Collection<GraphDataSet>();
            
            try
            {
                MySqlConnection cnn = new MySqlConnection(connection);
                cnn.Open();
                MySqlCommand mycommand = new MySqlCommand(query, cnn);
                MySqlDataAdapter da = new MySqlDataAdapter(mycommand);
                da.Fill(dt);
                cnn.Close();
                string lastval;
                foreach (DataColumn x in dt.Columns)
                {
                    lastval = "0";
                    temp = new GraphDataSet(x.ColumnName);
                    foreach (DataRow row in dt.Rows)
                    {
                        if ((row[x.ColumnName] != System.DBNull.Value))
                        {
                            lastval = (string)row[x.ColumnName].ToString(); 
                            temp.Add(lastval);

                        }
                        else
                        {
                            temp.Add(lastval);
                        }

                        
                    }
                    result.Add(temp);
                }
            }
            catch (MySqlException e) { MessageBox.Show(e.Message); }
            return result;
        }

        //Prevent double reccords
        private Collection<GraphDataSet> ExecuteReaderGDSS(string connection, string query, string column)
        {
            DataTable dt = new DataTable();
            GraphDataSet temp;
            Collection<GraphDataSet> result = new Collection<GraphDataSet>();

            try
            {
                MySqlConnection cnn = new MySqlConnection(connection);
                cnn.Open();
                MySqlCommand mycommand = new MySqlCommand(query, cnn);
                MySqlDataAdapter da = new MySqlDataAdapter(mycommand);
                da.Fill(dt);
                cnn.Close();
                
                string lastval;
                lastval = "0";
                temp = new GraphDataSet(column);
                if (dt.Rows.Count > 0)
                {
                    lastval = (string)dt.Rows[0][column].ToString();
                    temp.Add(lastval);
                    string lastserial = dt.Rows[0]["Serial"].ToString();
                    foreach (DataRow row in dt.Rows)
                    {
                        if ((row[column] != System.DBNull.Value))
                        {
                            if (row["Serial"].ToString() == lastserial)
                                temp.Y[temp.Y.Count - 1] = double.Parse(row[column].ToString());
                            else
                            {
                                lastval = (string)row[column].ToString();
                                temp.Add(lastval);
                            }
                        }
                        else
                        {
                            temp.Add(lastval);
                        }
                        lastserial = row["Serial"].ToString();
                    }
                }
                else
                {
                    temp.Name = "empty";
                    temp.Add("0");
                    temp.Add("0");
                    temp.Add("0");
                    temp.Add("0");
                }
                result.Add(temp);
            }
            catch (MySqlException e) { MessageBox.Show(e.Message); }
            return result;
        }
        private Collection<GraphDataSet> ExecuteReaderGDSS(string connection, string query, string column1, string column2)
        {
            DataTable dt = new DataTable();
            GraphDataSet temp;
            Collection<GraphDataSet> result = new Collection<GraphDataSet>();

            try
            {
                MySqlConnection cnn = new MySqlConnection(connection);
                cnn.Open();
                MySqlCommand mycommand = new MySqlCommand(query, cnn);
                MySqlDataAdapter da = new MySqlDataAdapter(mycommand);
                da.Fill(dt);
                cnn.Close();

                string lastval;
                string sno = "";
                lastval = "0";
                temp = new GraphDataSet(column1);
                if (dt.Rows.Count > 0)
                {
                    lastval = (string)dt.Rows[0][column1].ToString();
                    sno = dt.Rows[0][column2].ToString(); 
                    temp.Add(lastval, sno);
                    string lastserial = dt.Rows[0]["Serial"].ToString();
                    foreach (DataRow row in dt.Rows)
                    {
                        if ((row[column1] != System.DBNull.Value))
                        {
                            if (row["Serial"].ToString() == lastserial)
                            {
                                temp.Y[temp.Y.Count - 1] = double.Parse(row[column1].ToString());
                                temp.SNO[temp.Y.Count - 1] = int.Parse(dt.Rows[0][column2].ToString());
                            }
                            else
                            {
                                lastval = (string)row[column1].ToString();
                                sno = row[column2].ToString();
                                temp.Add(lastval, sno);
                            }
                        }
                        else
                        {
                            temp.Add(lastval, sno);
                        }
                        lastserial = row["Serial"].ToString();
                    }
                }
                else
                {
                    temp.Name = "empty";
                    temp.Add("0");
                    temp.Add("0");
                    temp.Add("0");
                    temp.Add("0");
                }
                result.Add(temp);
            }
            catch (MySqlException e) { MessageBox.Show(e.Message); }
            return result;
        }
        private string WeekToDate(string week)
        {
            string date = "20" + week.ElementAt(0).ToString() + week.ElementAt(1).ToString() + "-";
            double wk = double.Parse(week.ElementAt(2).ToString() + week.ElementAt(3).ToString());
            double val = ((int)((12.0 / 53.0) * wk));
            if (val < 10)
                date += "0" + val.ToString();
            else
                date += val.ToString();

            return date + "-01";
        }
        private Collection<GraphDataSet> ExecuteReaderGDSSWeek(string connection, string query, string column)
        {
            DataTable dt = new DataTable();
            GraphDataSet temp;
            Collection<GraphDataSet> result = new Collection<GraphDataSet>();

            try
            {
                MySqlConnection cnn = new MySqlConnection(connection);
                cnn.Open();
                MySqlCommand mycommand = new MySqlCommand(query, cnn);
                MySqlDataAdapter da = new MySqlDataAdapter(mycommand);
                da.Fill(dt);
                cnn.Close();

                string lastval;
                lastval = "0";
                temp = new GraphDataSet(column);
                if (dt.Rows.Count > 0)
                {
                    lastval = (string)dt.Rows[0][column].ToString();
                    string lastserial = dt.Rows[0]["Serial"].ToString();
                    temp.AddWeek(lastval, dt.Rows[0]["Week"].ToString());
                    foreach (DataRow row in dt.Rows)
                    {
                        if ((row[column] != System.DBNull.Value))
                        {
                            if (row["Serial"].ToString() == lastserial)
                                temp.Y[temp.Y.Count - 1] = double.Parse(row[column].ToString());
                            else
                            {
                                lastval = (string)row[column].ToString();
                                temp.AddWeek(lastval, row["Week"].ToString());
                            }
                        }
                        else
                        {
                            temp.AddWeek(lastval, row["Week"].ToString());
                        }
                        lastserial = row["Serial"].ToString();
                    }
                }
                else
                {
                    temp.Name = "empty";
                    temp.AddWeek("0", "0");
                    temp.AddWeek("0", "0");
                    temp.AddWeek("0", "0");
                    temp.AddWeek("0", "0");
                }
                result.Add(temp);
            }
            catch (MySqlException e) { MessageBox.Show(e.Message); }
            return result;
        }
        private CStringer ExecuteReaderC(string connection, string query)
        {
            DataTable dt = new DataTable();
            CStringer result = new CStringer();
            try
            {
                MySqlConnection cnn = new MySqlConnection(connection);
                cnn.Open();
                MySqlCommand mycommand = new MySqlCommand(query, cnn);
                MySqlDataReader reader = mycommand.ExecuteReader();
                dt.Load(reader);
                reader.Close();
                cnn.Close();

                foreach (DataColumn x in dt.Columns)
                    foreach (DataRow row in dt.Rows)
                        if ((row[x.ColumnName] != System.DBNull.Value))
                            result.Add((string)row[x.ColumnName].ToString()); 

            }
            catch (MySqlException e) { MessageBox.Show(e.Message); }
            return result;
        }

        //DB Info
        public int InfoRowCountS(string serial)
        {
            DataTable dt = new DataTable();
            int result = 0;
            try
            {
                MySqlConnection cnn = new MySqlConnection(cfg.Connection);
                cnn.Open();
                string query = "SELECT master_id FROM bm800_sample_instrinfo WHERE instrinfo_SNO='" + serial + "'";
                MySqlCommand mycommand = new MySqlCommand(query, cnn);
                MySqlDataReader reader = mycommand.ExecuteReader();
                dt.Load(reader);
                result = dt.Rows.Count;
                reader.Close();
                cnn.Close();
            }
            catch (MySqlException se) { MessageBox.Show(se.Message); }
            return result;
        }

    }//end class
}//end namespace

