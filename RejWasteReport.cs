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
    public partial class RejWasteReport : Form
    {
        public RejWasteReport()
        {
            InitializeComponent();
        }

        private void RejWasteReport_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'DataSet2.DataTable1' table. You can move, or remove it, as needed.
            
        }

        private void btn_RejShow_Click(object sender, EventArgs e)
        {
            this.DataTable1TableAdapter.Fill(DataSet2.DataTable1, dtRejFromDate.Value.Date.ToShortDateString(), dtRejToDate.Value.Date.ToShortDateString());
            this.reportViewer1.RefreshReport();

            ReportParameter[] parms = new ReportParameter[2];
            parms[0] = new ReportParameter("fromDate", dtRejFromDate.Value.ToShortDateString());
            parms[1] = new ReportParameter("toDate", dtRejToDate.Value.ToShortDateString());
            this.reportViewer1.LocalReport.SetParameters(parms);
            this.reportViewer1.RefreshReport();
        }
    }
}
