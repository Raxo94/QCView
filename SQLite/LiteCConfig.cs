/*
 *  CConfig 05
 *  
 *  2010-12-21 MARO Initial revision
 *  2011-02-08 MARO Bugfix - keeping old value when exiting editbox without change
 *  2011-03-09 MARO Added public change function for non editbox updates of Config.xml
 *  2011-08-06 MARO Added Exist - Checks if config file exists
 *                  Bugfix on public Add function
 *  2011-10-21 MARO Bugfix in New
 *  2012-01-27 MARO Replace two lists with one gridview
 *  2013-04-05 MARO Major cleanup - removed all references not being used in QC-View + GUI fix for VS2008/2012 differences
 *  
 *  Public Functions:   void Show(IWin32Window owner)
 *                      Collection<LiteCParameter> Read()
 *                      String Read(String parametername)
 *                      Collection<String> GetParameternames()
 *  Added class:        class LiteCParameter
 *  
 *  Use: Edit config file using inbuilt editor
 *                      CConfig config = new CConfig();
 *                      config.Show();
 *                      
 *  Use: Get single or multiple parameter values
 *                      CConfig config = new CConfig("");
 *                      
 *                      String ParaName = "Data_Source"; //parameter name is known, read single value
 *                      String ParaValue = config.Read(ParaName);                       
 *                      
 *                      foreach(String x in config.Read()) //read all parameter values (listboxXXX is assumed)
 *                      {
 *                          listboxParaName.Items.Add(x.Name);
 *                          listboxParaValue.Items.Add(x.Value);
 *                      }
*/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using Stringer;


namespace LiteConfig
{
    enum TableType { bm800_sample, bm800_settings, bm800_instrlog, bm800_caliblog, manual_instrlog, None }
    class LiteCConfig //reads the xml
    {
        private Collection<LiteCParameter> Parameterlist = new Collection<LiteCParameter>();
        private LiteCParameter Parameter = new LiteCParameter();
        private Form configForm;
        private Button buttonClose;
        private Button buttonCancel;
        private DataGridView dgView;
        private String xmlfilename = "Config.xml";
        private String xmlPath = Path.GetDirectoryName(Application.ExecutablePath) + "\\";
        public LiteCConfig() { }

        public LiteCConfig(String filename) 
        {
            xmlPath = "";
            xmlfilename = filename; 
        }

        void buttonCancel_Click(object sender, EventArgs e) { configForm.Close(); }
        void buttonClose_Click(object sender, EventArgs e)
        {
            SaveAll();
            configForm.Close();
        }
        void dgView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                LiteCConfig del = new LiteCConfig(xmlfilename);
                String param = dgView.SelectedCells[0].Value.ToString();

                del.DeleteParameter(param);
                dgView.Rows.Clear();
                foreach (LiteCParameter x in del.Read())
                {
                    dgView.Rows.Add();
                    dgView.Rows[dgView.Rows.Count - 2].Cells[0].Value = x.Name;
                    dgView.Rows[dgView.Rows.Count - 2].Cells[1].Value = x.Value;
                }
            }
        }
        /// <summary>
        /// Show editor for config management
        /// </summary>
        public void Show()
        {
            configForm = new Form();
            buttonClose = new Button();
            buttonCancel = new Button();
            dgView = new DataGridView();

            dgView.Parent = configForm;
            dgView.BringToFront();
            dgView.RowHeadersVisible = false;
            buttonCancel.Parent = configForm;
            buttonClose.Parent = configForm;

            configForm.Text = "Config Editor";
            configForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            configForm.MaximizeBox = false;
            configForm.MinimizeBox = false;
            configForm.StartPosition = FormStartPosition.CenterParent;

            Parameterlist.Clear();
            SetSize();

            dgView.Rows.Clear();
            try
            {
                Parameterlist = Read();
            }
            catch
            {
                MessageBox.Show("File Not Found, creating SQL default");
                InitSQLDefaults();
                Parameterlist = Read();
            }

            dgView.Columns.Add("dgName", "Name");
            dgView.Columns.Add("dgValue", "Value");
            dgView.Columns["dgName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgView.Columns["dgName"].FillWeight = 40;
            dgView.Columns["dgValue"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgView.Columns["dgValue"].FillWeight = 60;

            foreach (LiteCParameter x in Parameterlist)
            {
                dgView.Rows.Add();
                dgView.Rows[dgView.Rows.Count - 2].Cells[0].Value = x.Name;
                dgView.Rows[dgView.Rows.Count - 2].Cells[1].Value = x.Value;
            }

            buttonClose.Click += new EventHandler(buttonClose_Click);
            buttonCancel.Click += new EventHandler(buttonCancel_Click);
            dgView.KeyDown += new KeyEventHandler(dgView_KeyDown);

            configForm.ShowDialog();

            configForm.Dispose();
            buttonClose.Dispose();
            buttonCancel.Dispose();
            dgView.Dispose();
            GC.Collect();
        }
        private void SetSize()
        {
            dgView.Top = 5;
            dgView.Left = 5;
            dgView.Width = 800;
            dgView.Height = 600;

            buttonClose.Top = dgView.Bottom + 10;
            buttonClose.Height = buttonClose.Height + 5;
            buttonClose.Left = dgView.Right - buttonClose.Width;
            buttonClose.Text = "Save/Close";

            buttonCancel.Top = dgView.Bottom + 10;
            buttonCancel.Height = buttonClose.Height;
            buttonCancel.Left = buttonClose.Left - 80;
            buttonCancel.Text = "Cancel";

            configForm.Height = buttonClose.Bottom + 45;
            configForm.Width = dgView.Right + 20;
        }
        /// <summary>
        /// Create new Config.xml file to application root folder. 
        /// Before creating file, fill this object with data: Parametername, Parametervalue -> Add()
        /// </summary>
        private void Create()
        {
            XmlWriterSettings wSettings = new XmlWriterSettings();
            wSettings.Indent = true;
            XmlWriter xw = XmlWriter.Create(xmlfilename, wSettings);
            xw.WriteStartDocument();

            // Write root node
            xw.WriteStartElement("Config");

            //Write each node from parameterlist
            foreach (LiteCParameter x in Parameterlist)
            {
                xw.WriteStartElement(x.Name);
                xw.WriteString(x.Value);
                xw.WriteEndElement();
            }
            xw.WriteEndElement();

            // Close the document
            xw.WriteEndDocument();

            // Flush the write
            xw.Flush();
            xw.Close();
        }
 
        /// <summary>
        /// Check if config file exists, if not creates default
        /// </summary>
        /// <param name="warn">if true show message box that no config was found</param>
        public bool Exist()
        {
            String filename = xmlPath + xmlfilename;
            bool result = false;
            if (File.Exists(filename))
                result = true;
            return result;
        }
        public void DeleteParameter(String parametername)
        {
            Parameterlist = Read();
            int index = 0;
            for (int i = 0; i < Parameterlist.Count; i++)
            {
                if (Parameterlist[i].Name == parametername)
                    index = i;
            }
            Parameter.Name = parametername;
            Parameterlist.RemoveAt(index);
            Create();
            Parameterlist.Clear();
            Parameterlist = Read();

        }
        private void SaveAll()
        {
            try { File.Delete(xmlfilename); }
            catch (IOException ie) { MessageBox.Show(ie.Message); }
            Parameterlist.Clear();
            foreach (DataGridViewRow x in dgView.Rows)
            {
                if (!((x.Cells[0].Value == null) || (x.Cells[1].Value == null)))
                {
                    try
                    {
                        Add(x.Cells[0].Value.ToString(), x.Cells[1].Value.ToString());
                    }
                    catch {  }
                }
            }
        }
        /// <summary>
        /// Add parameter name and value
        /// </summary>
        /// <param name="parametername">Name of parameter to change</param>
        /// <param name="value">New value</param>
        public void Add(String parametername, String value)
        {
            if (parametername != "")
            {
                Parameterlist.Clear();
                Parameterlist = Read();

                Parameter.Name = parametername;
                Parameter.Value = value;
                Parameterlist.Add(Parameter);
                Create();

                Parameterlist.Clear();
                Parameterlist = Read();
            }
        }
        /// <summary>
        /// Change a single parameter in config file
        /// </summary>
        /// <param name="parametername">Name of parameter to change</param>
        /// <param name="value">New value</param>
        public void Change(String parametername, String value)
        {
            String filename = xmlPath + xmlfilename;
            FileStream reader = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.None);
            XmlNode xnode;
            System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();

            try { xmlDoc.Load(reader); }
            catch (XmlException xe) { LogException(xe.Message); }
            reader.Close();

            System.Xml.XmlNodeList NodeList = xmlDoc.GetElementsByTagName("Config");

            xnode = NodeList[0].SelectSingleNode(parametername);
            xnode.InnerText = value;

            XmlTextWriter tw = new XmlTextWriter(filename, Encoding.ASCII);
            try
            {
                tw.Formatting = Formatting.Indented;
                xmlDoc.Save(tw);
            }
            finally
            {
                tw.Close();
            }

        }
        /// <summary>
        /// Gets all config data in a LiteCParameter collection 
        /// </summary>
        /// <returns>All parameters and values in config file</returns>
        public Collection<LiteCParameter> Read()
        {
            LiteCParameter temp;
            Collection<LiteCParameter> result = new Collection<LiteCParameter>();

            XmlReaderSettings xmlsettings = new XmlReaderSettings();
            xmlsettings.ConformanceLevel = ConformanceLevel.Fragment;
            xmlsettings.IgnoreWhitespace = true;
            xmlsettings.IgnoreComments = true;
            XmlNode xNode;
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                XmlReader reader = XmlReader.Create(xmlPath + xmlfilename, xmlsettings);
                xmlDoc.Load(reader);
                reader.Close();
            }
            catch (XmlException xe) { LogException(xe.Message); }

            xNode = xmlDoc.SelectSingleNode("Config");
            foreach (XmlNode x in xNode.ChildNodes)
            {
                temp = new LiteCParameter();
                temp.Name = x.Name;
                temp.Value = x.InnerText;
                result.Add(temp);
            }
            return result;
        }
        public CStringer ReadStringer()
        {
            CStringer result = new CStringer();

            XmlReaderSettings xmlsettings = new XmlReaderSettings();
            xmlsettings.ConformanceLevel = ConformanceLevel.Fragment;
            xmlsettings.IgnoreWhitespace = true;
            xmlsettings.IgnoreComments = true;
            XmlNode xNode;
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                XmlReader reader = XmlReader.Create(xmlPath + xmlfilename, xmlsettings);
                xmlDoc.Load(reader);
                reader.Close();
            }
            catch (XmlException xe) { LogException(xe.Message); }

            xNode = xmlDoc.SelectSingleNode("Config");
            foreach (XmlNode x in xNode.ChildNodes)
                result.Add(x.InnerText);

            return result;
        }
        /// <summary>
        /// Gets multiple config values separated by delimiter for single parameter
        /// </summary>
        /// <param name="parametername">name of required parameter</param>
        /// <param name="delimiter">multiple data is separated by delimiter</param>
        public Collection<String> Read(String parametername, char delimiter)
        {
            String[] temp;
            Collection<String> result = new Collection<String>();

            XmlReaderSettings xmlsettings = new XmlReaderSettings();
            xmlsettings.ConformanceLevel = ConformanceLevel.Fragment;
            xmlsettings.IgnoreWhitespace = true;
            xmlsettings.IgnoreComments = true;
            XmlNode xNode;
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                XmlReader reader = XmlReader.Create(xmlPath + xmlfilename, xmlsettings);
                xmlDoc.Load(reader);
                reader.Close();
            }
            catch (XmlException xe) { LogException(xe.Message); }

            xNode = xmlDoc.SelectSingleNode("Config");
            xNode.SelectSingleNode(parametername);
            temp = xNode[parametername].InnerText.Split(delimiter);

            foreach (String x in temp)
                result.Add(x);

            return result;
        }

        /// <summary>
        /// Gets config value for a single parameter
        /// </summary>
        /// <param name="parametername">name of required parameter</param>
        /// <returns>parameter value</returns>
        public String Read(String parametername)
        {
            String result = "Error:WtongParameterName";

            XmlReaderSettings xmlsettings = new XmlReaderSettings();
            xmlsettings.ConformanceLevel = ConformanceLevel.Fragment;
            xmlsettings.IgnoreWhitespace = true;
            xmlsettings.IgnoreComments = true;
            XmlNode xNode;
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                XmlReader reader = XmlReader.Create(xmlPath + xmlfilename, xmlsettings);
                xmlDoc.Load(reader);
                reader.Close();
            }
            catch (XmlException xe) { LogException(xe.Message); }

            xNode = xmlDoc.SelectSingleNode("Config");
            xNode.SelectSingleNode(parametername);
            result = xNode[parametername].InnerText;

            return result;
        }
        public Color ReadC(String parametername)
        {
            String result = "Error:WrongParameterName";

            XmlReaderSettings xmlsettings = new XmlReaderSettings();
            xmlsettings.ConformanceLevel = ConformanceLevel.Fragment;
            xmlsettings.IgnoreWhitespace = true;
            xmlsettings.IgnoreComments = true;
            XmlNode xNode;
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                XmlReader reader = XmlReader.Create(xmlPath + xmlfilename, xmlsettings);
                xmlDoc.Load(reader);
                reader.Close();
            }
            catch (XmlException xe) { LogException(xe.Message); }

            xNode = xmlDoc.SelectSingleNode("Config");
            xNode.SelectSingleNode(parametername);
            result = xNode[parametername].InnerText;
            if (result == "White")
                return Color.White;
            else if (result == "Gray")
                return Color.Gray;
            else if (result == "Blue")
                return Color.Blue;
            else
                return Color.Black;
        }
        public double ReadD(String parametername)
        {
            String result = "Error:WrongParameterName";

            XmlReaderSettings xmlsettings = new XmlReaderSettings();
            xmlsettings.ConformanceLevel = ConformanceLevel.Fragment;
            xmlsettings.IgnoreWhitespace = true;
            xmlsettings.IgnoreComments = true;
            XmlNode xNode;
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                XmlReader reader = XmlReader.Create(xmlPath + xmlfilename, xmlsettings);
                xmlDoc.Load(reader);
                reader.Close();
            }
            catch (XmlException xe) { LogException(xe.Message); }

            xNode = xmlDoc.SelectSingleNode("Config");
            xNode.SelectSingleNode(parametername);
            result = xNode[parametername].InnerText;
            return double.Parse(result.Replace('.', ','));
        }

        public void InitSQLDefaults()
        {
            Add("Server", "SERVER=localhost;");
            Add("Database", "DATABASE=bm800_testi;");
            Add("User_Id", "UID=root;");
            Add("Password", "PASSWORD=Skynet;");
            Add("Main_Directory", Path.GetDirectoryName(Application.ExecutablePath));
        }
        private void LogException(object msg)
        {
            using (StreamWriter sw = new StreamWriter("Logi.txt", true))
            {
                sw.Write(DateTime.Now + "\t" + msg + "\r\n");
            }
        }
        public string GetMainFolder() { return Read("Main_Directory"); }
        public void SetMainFolder(string value) { Change("Main_Directory", value); }

    }//End Class

    public class LiteCParameter
    {
        public String Name = "";
        public String Value = "";
        
        /// <summary>
        /// A single set of config parameter (Name, Value)
        /// </summary>
        public LiteCParameter() { }
    }//End Class

    public class GraphDataSet
    {
        public String Name;
        public double limit;
        public Collection<double> Y = new Collection<double>();
        public Collection<int> SNO = new Collection<int>();
        public Collection<int> Week = new Collection<int>();
        public GraphDataSet(String name)
        {
            Name = name;
        }
        public void Add(string y)
        {
            string val = y.Replace(".", ",");
            double temp;
            if (double.TryParse(val, out temp)) { Y.Add(temp); }
            else { Y.Add(0); }
        }
        public void Add(string y, string sno)
        {
            string val = y.Replace(".", ",");
            double temp;
            if (double.TryParse(val, out temp)) { Y.Add(temp); }
            else { Y.Add(0); }
            SNO.Add(int.Parse(sno)); 
        }
        public void AddWeek(string y, string week)
        {
            string val = y.Replace(".", ",");
            double temp;
            if (double.TryParse(val, out temp)) { Y.Add(temp); }
            else { Y.Add(0); }
            Week.Add(int.Parse(week));
        }
        public override string ToString()
        {
            string result = "";
            for (int i = 0; i < Y.Count; i++)
            {
                result += Y[i].ToString("N1") + "\t";
            }
            return result;
        }
        public string ToPresentation()
        {
            string result = "";
            for (int i = 0; i < Y.Count; i++)
            {
                if (i < SNO.Count)
                    result += SNO[i].ToString() + ":" + Y[i].ToString("N1") + "\t";
                else
                    result += "Null:" + Y[i].ToString("N1") + "\t";
            }
            return result;
        }
    }

    public class CTextInput
    {
        private Form textform = new Form();
        private TextBox textbox = new TextBox();
        private bool IsName = false;
        public String Text = "";

        /// <summary>
        /// Displays a textbox for text input
        /// </summary>
        public CTextInput() 
        {
            textbox.KeyDown += new KeyEventHandler(textbox_KeyDown);
        }
        ~CTextInput()
        { 
            
        }
        void textbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                if (textbox.Text.Contains(" ") && IsName)
                {
                    MessageBox.Show(textbox, "Replacing space characters with underscore");
                    textbox.Text = textbox.Text.Replace(" ", "_");
                }
                Text = textbox.Text;
                textbox.Clear();
                textform.Close();
            }
        }

        public void Show(Control owner, bool isname, String name, String oldtext)
        {
            int width = 250;
            int height = 48;
            if (oldtext.Length >= 50)
            {
                height = 4*48;
                textbox.Multiline = true;
                textbox.Height = height - 25;
            }
            IsName = isname;
            Text = oldtext;
            textbox.Parent = textform;
            textbox.Text = oldtext;
            textform.FormBorderStyle = FormBorderStyle.FixedDialog;
            textform.MaximizeBox = false;
            textform.MinimizeBox = false;
            textform.StartPosition = FormStartPosition.CenterParent;
            textform.Text = "Parameter " + name;
            textform.Width = width + 4;
            textform.Height = height;
            textbox.Width = width;
            textform.ShowDialog(owner);
        }

    }//End Class

    public class CTextInput2
    {
        private Form textform = new Form();
        private TextBox textbox = new TextBox();
        private Button btOK = new Button();
        public String Text = "";

        /// <summary>
        /// Displays a textbox for text input
        /// </summary>
        public CTextInput2()
        {
            btOK.Click +=btOK_Click;
        }

        void btOK_Click(object sender, EventArgs e)
        {
            Text = textbox.Text;
            textbox.Clear();
            textform.Close();
        }
        public string Show(String name)
        {
            int width = 300;
            int height = 130;
            Text = "";
            textform.FormBorderStyle = FormBorderStyle.FixedDialog;
            textform.MaximizeBox = false;
            textform.MinimizeBox = false;
            textform.StartPosition = FormStartPosition.CenterParent;
            textform.Text = name;
            textform.Width = width + 4;
            textform.Height = height;
            textbox.Parent = textform;
            textbox.Multiline = true;
            textbox.Dock = DockStyle.Top;
            textbox.Height = textbox.Height * 3;
            textbox.Text = "";
            btOK.Parent = textform;
            btOK.Left = (int)(textform.Width / 2.0) - (int)(btOK.Width / 2.0);
            btOK.Top = textbox.Bottom + 5;
            btOK.Text = "OK";
            textform.ShowDialog();
            return Text;
        }

    }//End Class

    public class LiteCConfigConnection
    {
        public String Table_Manual = "";
        public String Server = "";
        public String Database = "";
        public String Database_Manual = "";
        public String Table_Yieldlabel = "";
        public String User_Id = "";
        public String Password = "";
        public String Connection { get { return @"Data Source=D:\arbete\WorkingCopy\TestDataBase.db;" + "Version = 3; New = True; Compress = True;"; /*Server + Database + User_Id + Password;*/ } }
        public String ConnectionM { get { return @"Data Source=D:\arbete\WorkingCopy\TestDataBase.db;" + "Version = 3; New = True; Compress = True;";/*return Server + Database_Manual + User_Id + Password;*/ } }
        public bool useSqlite;
        public LiteCConfigConnection()
        {
            useSqlite = false;
            LiteCConfig cfg = new LiteCConfig();
            if (!cfg.Exist())
                cfg.InitSQLDefaults();

            Server = "temp";
            Database = "temp";
            Database_Manual = "temp";
            User_Id = "temp";
            Password = "Temp";
            Table_Manual = "TestTransfer";
            Table_Yieldlabel = "Temp";
        }
    }//end class
}//End Namespace
