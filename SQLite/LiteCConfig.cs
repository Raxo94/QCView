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

namespace QCP_Viewer.SQLite
{

    public class CParameter
    {
        public String Name = "";
        public String Value = "";

        /// <summary>
        /// A single set of config parameter (Name, Value)
        /// </summary>
        public CParameter() { }


    }//End Class



    class LiteCConfig
    {
        private Collection<CParameter> Parameterlist = new Collection<CParameter>();
        private CParameter Parameter = new CParameter();
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
        private void Create()
        {
            XmlWriterSettings wSettings = new XmlWriterSettings();
            wSettings.Indent = true;
            XmlWriter xw = XmlWriter.Create(xmlfilename, wSettings);
            xw.WriteStartDocument();

            // Write root node
            xw.WriteStartElement("Config");

            //Write each node from parameterlist
            foreach (CParameter x in Parameterlist)
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
        public Collection<CParameter> Read()
        {
            CParameter temp;
            Collection<CParameter> result = new Collection<CParameter>();

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
            catch (XmlException xe) { Console.Write(xe.Message); }

            xNode = xmlDoc.SelectSingleNode("Config");
            foreach (XmlNode x in xNode.ChildNodes)
            {
                temp = new CParameter();
                temp.Name = x.Name;
                temp.Value = x.InnerText;
                result.Add(temp);
            }
            return result;
        }
        public void InitSQLDefaults()
        {
            Add("Server", "SERVER=localhost;");
            Add("Database", "DATABASE=bm800_testi;");
            Add("User_Id", "UID=root;");
            Add("Password", "PASSWORD=Skynet;");
            Add("Main_Directory", Path.GetDirectoryName(Application.ExecutablePath));
        }
    }

    
}
