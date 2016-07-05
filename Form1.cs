/* 20130204 MARO quick and dirty fix for year overlap issue in "public string GetYearWeek(int pastweeks)"
 
    if (timespan.TotalDays == 28)
        if (week == 1)
            theTime = theTime.AddYears(1);
*/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Config;
using Stringer;
//using SQLite;
using Graph;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using CMYSQLN;

namespace QCP_Viewer
{
    public partial class Form1 : Form
    {
        CStringer tables = new CStringer();
        CGraph gProblem, gCV, gYield, gUser, gComponents;
        bool selectionweek = true;
        Collection<string> yieldtexts = new Collection<string>();
        public Form1()
        {
            InitializeComponent();
            Application.DoEvents();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            CConfig cfg = new CConfig("Yieldtext.xml");
            gProblem = new CGraph(chProblem, this.tpProblem);
            gComponents = new CGraph(chComponents, this.tpKomponenter);
            gYield = new CGraph(chYield, this.tpYield, cfg.Read());
            gUser = new CGraph(chUser, this.tpUser);
            gCV = new CGraph(chCV, this.tpCV);

            for (int i = 1; i <= 52; i++)
                cbWeeks.Items.Add(i);
            cbWeeks.SelectedIndex = 3;

            int years = DateTime.Now.Year - 2000 - 6;
            for (int i = 1; i <= years; i++)
                cbYears.Items.Add(i);
        }
        private void cbWeeks_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectionweek = true;
            string date = GetYearWeek(int.Parse(cbWeeks.SelectedItem.ToString()));
            string weeks = cbWeeks.SelectedItem.ToString();
            if (tcSelector.SelectedTab.Name == "tpProblem")
            {
                //iProblem = true;
                gProblem.FillProblem(date, weeks);
            }
            if (tcSelector.SelectedTab.Name == "tpKomponenter")
            {
                //iProblem = true;
                gComponents.FillComponents(date, weeks);
            }
            if (tcSelector.SelectedTab.Name == "tpUser")
            {
                //iUser = true;
                gUser.FillUser(date, weeks);
            }
            if (tcSelector.SelectedTab.Name == "tpCV")
            {
                //iCV = true;
                if (cbCVLimits.Checked)
                    gCV.FillCV(date, weeks, CVType.Absolute);
                else
                    gCV.FillCV(date, weeks, CVType.Expected);
            }
            if (tcSelector.SelectedTab.Name == "tpYield")
            {
                //iYield = true;
                gYield.FillYield(date, weeks, cbShowlabels.Checked);
            }
        }
        private void cbYears_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectionweek = false;
            int iweeks = int.Parse(cbYears.SelectedItem.ToString()) * 52;
            string date = GetYearWeek(iweeks);
            if (tcSelector.SelectedTab.Name == "tpProblem")
            {
                //iProblem = true;
                gProblem.FillProblem(date, iweeks.ToString());
            }
            if (tcSelector.SelectedTab.Name == "tpKomponenter")
            {
                //iProblem = true;
                gComponents.FillComponents(date, iweeks.ToString());
            }
            if (tcSelector.SelectedTab.Name == "tpUser")
            {
                //iUser = true;
                gUser.FillUser(date, iweeks.ToString());
            }
            if (tcSelector.SelectedTab.Name == "tpCV")
            {
                //iCV = true;
                if (cbCVLimits.Checked)
                    gCV.FillCV(date, iweeks.ToString(), CVType.Absolute);
                else
                    gCV.FillCV(date, iweeks.ToString(), CVType.Expected);
            }
            if (tcSelector.SelectedTab.Name == "tpYield")
            {
                //iYield = true;
                CMYSQL sql = new CMYSQL(TableType.None);
                gYield.FillYield(date, iweeks.ToString(), cbShowlabels.Checked);
            }
        }
        private void tcSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (selectionweek)
                cbWeeks_SelectedIndexChanged(null, null);
            else
                cbYears_SelectedIndexChanged(null, null);
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CConfig cfg = new CConfig();
            cfg.Show();
        }

        public string GetYearWeek(int pastweeks)
        {
            System.DateTime theTime = System.DateTime.Now;
            System.TimeSpan timespan = new TimeSpan(7, 0, 0, 0);
            theTime = theTime.Subtract(timespan);
            timespan = new TimeSpan(pastweeks * 7, 0, 0, 0);
            theTime = theTime.Subtract(timespan);

            string sweek;
            int week = GetWeek(theTime);
            
            if (week < 10)
                sweek = "0" + week.ToString();
            else
                sweek = week.ToString();
            
            if (timespan.TotalDays == 28)
                if (week == 1)
                    theTime = theTime.AddYears(1);
            
            return theTime.Year.ToString().Substring(2, 2) + sweek;
        }
        private int GetWeek(DateTime date)
        {
            const int JAN = 1;
            const int DEC = 12;
            const int LASTDAYOFDEC = 31;
            const int FIRSTDAYOFJAN = 1;
            const int THURSDAY = 4;
            bool ThursdayFlag = false;

            int DayOfYear = date.DayOfYear;

            int StartWeekDayOfYear = (int)(new DateTime(date.Year, JAN, FIRSTDAYOFJAN)).DayOfWeek;
            int EndWeekDayOfYear = (int)(new DateTime(date.Year, DEC, LASTDAYOFDEC)).DayOfWeek;

            if (StartWeekDayOfYear == 0)
                StartWeekDayOfYear = 7;
            if (EndWeekDayOfYear == 0)
                EndWeekDayOfYear = 7;

            int DaysInFirstWeek = 8 - (StartWeekDayOfYear);
            int DaysInLastWeek = 8 - (EndWeekDayOfYear);

            if (StartWeekDayOfYear == THURSDAY || EndWeekDayOfYear == THURSDAY)
                ThursdayFlag = true;

            int FullWeeks = (int)Math.Ceiling((DayOfYear - (DaysInFirstWeek)) / 7.0);

            int WeekNumber = FullWeeks;

            if (DaysInFirstWeek >= THURSDAY)
                WeekNumber = WeekNumber + 1;

            if (WeekNumber > 52 && !ThursdayFlag)
                WeekNumber = 1;

            if (WeekNumber == 0)
                WeekNumber = GetWeek(new DateTime(date.Year - 1, DEC, LASTDAYOFDEC));

            return WeekNumber;
        }

        private void cbCVLimits_CheckedChanged(object sender, EventArgs e)
        {
            if (selectionweek)
                cbWeeks_SelectedIndexChanged(null, null);
            else
                cbYears_SelectedIndexChanged(null, null);
        }
        
        private void tsExportPDF_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sf = new SaveFileDialog())
            {
                sf.Filter = "PDF File|*.pdf";
                sf.Title = "Exportera till PDF";
                if (sf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string dir = sf.FileName;
                    string date = GetYearWeek(52);
                    Control crtl = new Control();
                    //CGraph pdfGraph = new CGraph(chYield, this.tpYield, cfg.Read());
                    gYield.FillYield(date, "52", true);
                    iTextSharp.text.Image imgYield = iTextSharp.text.Image.GetInstance(gYield.GetImage("img_yi.png"), BaseColor.WHITE);
                    imgYield.ScaleToFit(540f, 300f);
                    imgYield.SpacingBefore = 5f;
                    imgYield.SpacingAfter = 5f;
                    imgYield.Alignment = Element.ALIGN_CENTER;

                    date = GetYearWeek(4);
                    gProblem.FillProblem(date, "4");
                    iTextSharp.text.Image imgQC = iTextSharp.text.Image.GetInstance(gProblem.GetImage("img_qc.png"), BaseColor.WHITE);
                    imgQC.ScaleToFit(540f, 300f);
                    imgQC.SpacingBefore = 5f;
                    imgQC.SpacingAfter = 5f;
                    imgQC.Alignment = Element.ALIGN_CENTER;

                    Document pdfDoc = new Document(PageSize.A4);
                    try
                    {
                        PdfWriter.GetInstance(pdfDoc, new FileStream(dir, FileMode.Create));
                        pdfDoc.Open();
                        pdfDoc.Add(new Paragraph("Boule Medical " + DateTime.Now.ToShortDateString()));
                        pdfDoc.Add(new Paragraph("Sammanställning QC"));
                        pdfDoc.Add(new Paragraph("\r\n\r\n\r\n"));
                        pdfDoc.Add(imgYield);
                        pdfDoc.Add(imgQC);
                    }
                    catch (DocumentException de)
                    {
                        MessageBox.Show(de.Message);
                    }
                    catch (IOException ioe)
                    {
                        MessageBox.Show(ioe.Message);
                    }
                    pdfDoc.Close();
                    try
                    {
                        System.Diagnostics.Process.Start(dir);
                    }
                    catch 
                    {
                        MessageBox.Show("Could not open " + dir);
                    }
                }
            }
        }

        private void cbShowlabels_CheckedChanged(object sender, EventArgs e)
        {
            if (selectionweek)
                cbWeeks_SelectedIndexChanged(null, null);
            else
                cbYears_SelectedIndexChanged(null, null);
        }

        private void chProblem_Click(object sender, EventArgs e)
        {

        }
        //2012-02-21T12:52:03
    }//end class
}//end namespace
