namespace QCP_Viewer
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea4 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend4 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series5 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea5 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend5 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series6 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsExportPDF = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.cbWeeks = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.cbYears = new System.Windows.Forms.ToolStripComboBox();
            this.printImage = new System.Drawing.Printing.PrintDocument();
            this.printDialogImage = new System.Windows.Forms.PrintDialog();
            this.tpCV = new System.Windows.Forms.TabPage();
            this.cbCVLimits = new System.Windows.Forms.CheckBox();
            this.chCV = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tpUser = new System.Windows.Forms.TabPage();
            this.chUser = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tpYield = new System.Windows.Forms.TabPage();
            this.cbShowlabels = new System.Windows.Forms.CheckBox();
            this.chYield = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tpProblem = new System.Windows.Forms.TabPage();
            this.chProblem = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tcSelector = new System.Windows.Forms.TabControl();
            this.tpKomponenter = new System.Windows.Forms.TabPage();
            this.chComponents = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.toolStrip1.SuspendLayout();
            this.tpCV.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chCV)).BeginInit();
            this.tpUser.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chUser)).BeginInit();
            this.tpYield.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chYield)).BeginInit();
            this.tpProblem.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chProblem)).BeginInit();
            this.tcSelector.SuspendLayout();
            this.tpKomponenter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chComponents)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.toolStrip1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.toolStrip1.GripMargin = new System.Windows.Forms.Padding(5);
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownButton1,
            this.toolStripSeparator1,
            this.toolStripLabel1,
            this.cbWeeks,
            this.toolStripLabel2,
            this.cbYears});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Padding = new System.Windows.Forms.Padding(0);
            this.toolStrip1.Size = new System.Drawing.Size(922, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem,
            this.toolStripSeparator2,
            this.tsExportPDF});
            this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(49, 22);
            this.toolStripDropDownButton1.Text = "Tools";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Image = global::QCP_Viewer.Properties.Resources.OptionsHS;
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.optionsToolStripMenuItem.Text = "Options";
            this.optionsToolStripMenuItem.Click += new System.EventHandler(this.optionsToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(137, 6);
            // 
            // tsExportPDF
            // 
            this.tsExportPDF.Image = global::QCP_Viewer.Properties.Resources.pdf_icon_16;
            this.tsExportPDF.Name = "tsExportPDF";
            this.tsExportPDF.Size = new System.Drawing.Size(140, 22);
            this.tsExportPDF.Text = "Export PDF...";
            this.tsExportPDF.Click += new System.EventHandler(this.tsExportPDF_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(43, 22);
            this.toolStripLabel1.Text = "Veckor";
            // 
            // cbWeeks
            // 
            this.cbWeeks.Name = "cbWeeks";
            this.cbWeeks.Size = new System.Drawing.Size(75, 25);
            this.cbWeeks.SelectedIndexChanged += new System.EventHandler(this.cbWeeks_SelectedIndexChanged);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(19, 22);
            this.toolStripLabel2.Text = "År";
            // 
            // cbYears
            // 
            this.cbYears.Name = "cbYears";
            this.cbYears.Size = new System.Drawing.Size(75, 25);
            this.cbYears.SelectedIndexChanged += new System.EventHandler(this.cbYears_SelectedIndexChanged);
            // 
            // printDialogImage
            // 
            this.printDialogImage.Document = this.printImage;
            this.printDialogImage.UseEXDialog = true;
            // 
            // tpCV
            // 
            this.tpCV.BackColor = System.Drawing.Color.White;
            this.tpCV.Controls.Add(this.cbCVLimits);
            this.tpCV.Controls.Add(this.chCV);
            this.tpCV.Location = new System.Drawing.Point(4, 22);
            this.tpCV.Name = "tpCV";
            this.tpCV.Padding = new System.Windows.Forms.Padding(3);
            this.tpCV.Size = new System.Drawing.Size(908, 467);
            this.tpCV.TabIndex = 3;
            this.tpCV.Text = "CV - Open Tube";
            // 
            // cbCVLimits
            // 
            this.cbCVLimits.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cbCVLimits.AutoSize = true;
            this.cbCVLimits.BackColor = System.Drawing.Color.White;
            this.cbCVLimits.Location = new System.Drawing.Point(802, 405);
            this.cbCVLimits.Name = "cbCVLimits";
            this.cbCVLimits.Size = new System.Drawing.Size(61, 17);
            this.cbCVLimits.TabIndex = 0;
            this.cbCVLimits.Text = "   Limits";
            this.cbCVLimits.UseVisualStyleBackColor = false;
            this.cbCVLimits.CheckedChanged += new System.EventHandler(this.cbCVLimits_CheckedChanged);
            // 
            // chCV
            // 
            chartArea1.AxisY.Title = "CV [rullande medel 50 Instrument]";
            chartArea1.Name = "ChartArea1";
            this.chCV.ChartAreas.Add(chartArea1);
            this.chCV.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = "Legend1";
            this.chCV.Legends.Add(legend1);
            this.chCV.Location = new System.Drawing.Point(3, 3);
            this.chCV.Name = "chCV";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chCV.Series.Add(series1);
            this.chCV.Size = new System.Drawing.Size(902, 461);
            this.chCV.TabIndex = 1;
            this.chCV.Text = "chart1";
            // 
            // tpUser
            // 
            this.tpUser.BackColor = System.Drawing.Color.White;
            this.tpUser.Controls.Add(this.chUser);
            this.tpUser.Location = new System.Drawing.Point(4, 22);
            this.tpUser.Name = "tpUser";
            this.tpUser.Padding = new System.Windows.Forms.Padding(3);
            this.tpUser.Size = new System.Drawing.Size(908, 467);
            this.tpUser.TabIndex = 4;
            this.tpUser.Text = "Produktion";
            // 
            // chUser
            // 
            chartArea2.Name = "ChartArea1";
            this.chUser.ChartAreas.Add(chartArea2);
            this.chUser.Dock = System.Windows.Forms.DockStyle.Fill;
            legend2.Name = "Legend1";
            this.chUser.Legends.Add(legend2);
            this.chUser.Location = new System.Drawing.Point(3, 3);
            this.chUser.Name = "chUser";
            series2.ChartArea = "ChartArea1";
            series2.IsValueShownAsLabel = true;
            series2.Legend = "Legend1";
            series2.Name = "Antal";
            this.chUser.Series.Add(series2);
            this.chUser.Size = new System.Drawing.Size(902, 461);
            this.chUser.TabIndex = 0;
            this.chUser.Text = "chart1";
            // 
            // tpYield
            // 
            this.tpYield.BackColor = System.Drawing.Color.White;
            this.tpYield.Controls.Add(this.cbShowlabels);
            this.tpYield.Controls.Add(this.chYield);
            this.tpYield.Location = new System.Drawing.Point(4, 22);
            this.tpYield.Name = "tpYield";
            this.tpYield.Padding = new System.Windows.Forms.Padding(3);
            this.tpYield.Size = new System.Drawing.Size(908, 467);
            this.tpYield.TabIndex = 2;
            this.tpYield.Text = "Yield - Instrument";
            // 
            // cbShowlabels
            // 
            this.cbShowlabels.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cbShowlabels.AutoSize = true;
            this.cbShowlabels.BackColor = System.Drawing.Color.White;
            this.cbShowlabels.Checked = true;
            this.cbShowlabels.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbShowlabels.Location = new System.Drawing.Point(796, 387);
            this.cbShowlabels.Name = "cbShowlabels";
            this.cbShowlabels.Size = new System.Drawing.Size(88, 17);
            this.cbShowlabels.TabIndex = 2;
            this.cbShowlabels.Text = "Visa Etiketter";
            this.cbShowlabels.UseVisualStyleBackColor = false;
            this.cbShowlabels.CheckedChanged += new System.EventHandler(this.cbShowlabels_CheckedChanged);
            // 
            // chYield
            // 
            this.chYield.BorderlineColor = System.Drawing.Color.Black;
            chartArea3.AxisX.Title = "Antal Instrument";
            chartArea3.AxisY.Maximum = 100D;
            chartArea3.AxisY.Title = "% [rullande medel 50 Instrument]";
            chartArea3.Name = "ChartArea1";
            this.chYield.ChartAreas.Add(chartArea3);
            this.chYield.Dock = System.Windows.Forms.DockStyle.Fill;
            legend3.Name = "Legend1";
            this.chYield.Legends.Add(legend3);
            this.chYield.Location = new System.Drawing.Point(3, 3);
            this.chYield.Name = "chYield";
            this.chYield.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.None;
            series3.ChartArea = "ChartArea1";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series3.Legend = "Legend1";
            series3.MarkerBorderColor = System.Drawing.Color.Blue;
            series3.MarkerColor = System.Drawing.Color.Blue;
            series3.Name = "Yield";
            series3.SmartLabelStyle.MaxMovingDistance = 100D;
            series4.ChartArea = "ChartArea1";
            series4.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series4.Legend = "Legend1";
            series4.MarkerSize = 3;
            series4.Name = "Målvärde";
            this.chYield.Series.Add(series3);
            this.chYield.Series.Add(series4);
            this.chYield.Size = new System.Drawing.Size(902, 461);
            this.chYield.TabIndex = 1;
            this.chYield.Text = "chart1";
            // 
            // tpProblem
            // 
            this.tpProblem.BackColor = System.Drawing.Color.White;
            this.tpProblem.Controls.Add(this.chProblem);
            this.tpProblem.Location = new System.Drawing.Point(4, 22);
            this.tpProblem.Name = "tpProblem";
            this.tpProblem.Padding = new System.Windows.Forms.Padding(3);
            this.tpProblem.Size = new System.Drawing.Size(908, 467);
            this.tpProblem.TabIndex = 1;
            this.tpProblem.Text = "Problemuppföljning";
            // 
            // chProblem
            // 
            chartArea4.Name = "ChartArea1";
            this.chProblem.ChartAreas.Add(chartArea4);
            this.chProblem.Dock = System.Windows.Forms.DockStyle.Fill;
            legend4.Name = "Legend1";
            this.chProblem.Legends.Add(legend4);
            this.chProblem.Location = new System.Drawing.Point(3, 3);
            this.chProblem.Name = "chProblem";
            series5.ChartArea = "ChartArea1";
            series5.IsValueShownAsLabel = true;
            series5.Legend = "Legend1";
            series5.Name = "Antal";
            this.chProblem.Series.Add(series5);
            this.chProblem.Size = new System.Drawing.Size(902, 461);
            this.chProblem.TabIndex = 0;
            this.chProblem.Text = "chart1";
            this.chProblem.Click += new System.EventHandler(this.chProblem_Click);
            // 
            // tcSelector
            // 
            this.tcSelector.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tcSelector.Controls.Add(this.tpProblem);
            this.tcSelector.Controls.Add(this.tpKomponenter);
            this.tcSelector.Controls.Add(this.tpYield);
            this.tcSelector.Controls.Add(this.tpUser);
            this.tcSelector.Controls.Add(this.tpCV);
            this.tcSelector.Location = new System.Drawing.Point(5, 28);
            this.tcSelector.Name = "tcSelector";
            this.tcSelector.SelectedIndex = 0;
            this.tcSelector.Size = new System.Drawing.Size(916, 493);
            this.tcSelector.TabIndex = 17;
            this.tcSelector.SelectedIndexChanged += new System.EventHandler(this.tcSelector_SelectedIndexChanged);
            // 
            // tpKomponenter
            // 
            this.tpKomponenter.BackColor = System.Drawing.Color.White;
            this.tpKomponenter.Controls.Add(this.chComponents);
            this.tpKomponenter.Location = new System.Drawing.Point(4, 22);
            this.tpKomponenter.Name = "tpKomponenter";
            this.tpKomponenter.Padding = new System.Windows.Forms.Padding(3);
            this.tpKomponenter.Size = new System.Drawing.Size(908, 467);
            this.tpKomponenter.TabIndex = 8;
            this.tpKomponenter.Text = "Komponenter";
            // 
            // chComponents
            // 
            chartArea5.Name = "ChartArea1";
            this.chComponents.ChartAreas.Add(chartArea5);
            this.chComponents.Dock = System.Windows.Forms.DockStyle.Fill;
            legend5.Name = "Legend1";
            this.chComponents.Legends.Add(legend5);
            this.chComponents.Location = new System.Drawing.Point(3, 3);
            this.chComponents.Name = "chComponents";
            series6.ChartArea = "ChartArea1";
            series6.IsValueShownAsLabel = true;
            series6.Legend = "Legend1";
            series6.Name = "Antal";
            this.chComponents.Series.Add(series6);
            this.chComponents.Size = new System.Drawing.Size(902, 461);
            this.chComponents.TabIndex = 0;
            this.chComponents.Text = "chart1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(922, 522);
            this.Controls.Add(this.tcSelector);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "QC-View 0.x";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.tpCV.ResumeLayout(false);
            this.tpCV.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chCV)).EndInit();
            this.tpUser.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chUser)).EndInit();
            this.tpYield.ResumeLayout(false);
            this.tpYield.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chYield)).EndInit();
            this.tpProblem.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chProblem)).EndInit();
            this.tcSelector.ResumeLayout(false);
            this.tpKomponenter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chComponents)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripComboBox cbWeeks;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripComboBox cbYears;
        private System.Drawing.Printing.PrintDocument printImage;
        private System.Windows.Forms.PrintDialog printDialogImage;
        private System.Windows.Forms.ToolStripMenuItem tsExportPDF;
        private System.Windows.Forms.TabPage tpCV;
        private System.Windows.Forms.CheckBox cbCVLimits;
        private System.Windows.Forms.TabPage tpUser;
        private System.Windows.Forms.TabPage tpYield;
        private System.Windows.Forms.TabPage tpProblem;
        private System.Windows.Forms.TabControl tcSelector;
        private System.Windows.Forms.TabPage tpKomponenter;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.DataVisualization.Charting.Chart chProblem;
        private System.Windows.Forms.DataVisualization.Charting.Chart chComponents;
        private System.Windows.Forms.DataVisualization.Charting.Chart chYield;
        private System.Windows.Forms.DataVisualization.Charting.Chart chUser;
        private System.Windows.Forms.DataVisualization.Charting.Chart chCV;
        private System.Windows.Forms.CheckBox cbShowlabels;
    }
}

