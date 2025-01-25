using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;

namespace Cutting
{
    public partial class DepWasteReport : Form
    {
        

        public DepWasteReport()
        {
            InitializeComponent();
        }

        private void DepWasteReport_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'DataSet1.DataTable1' table. You can move, or remove it, as needed.
            //this.DataTable1TableAdapter.Fill(this.DataSet1.DataTable1);

            //this.reportViewer1.RefreshReport();
        }

        private void btn_DepShow_Click(object sender, EventArgs e)
        {
            this.DataTable1TableAdapter.Fill(DataSet1.DataTable1,dtDepFromDate.Value.Date.ToShortDateString(),dtDepToDate.Value.Date.ToShortDateString());
           this.reportViewer1.RefreshReport();

            ReportParameter[] parms = new ReportParameter[2];
            parms[0] = new ReportParameter("fromDate", dtDepFromDate.Value.ToShortDateString());
            parms[1] = new ReportParameter("toDate", dtDepToDate.Value.ToShortDateString());
            this.reportViewer1.LocalReport.SetParameters(parms);
            this.reportViewer1.RefreshReport();
        }
    }
}
