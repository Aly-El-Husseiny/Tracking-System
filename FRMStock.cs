using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using DevExpress.XtraEditors;

namespace Cutting
{
    public partial class FRMStock : DevExpress.XtraEditors.XtraForm
    {

        TrackingDataContext trackdb = new TrackingDataContext();
        public FRMStock()
        {
            InitializeComponent();
        }
        private void RefreshStock()
        {
            DGV_Glass_Req.DataSource = "";

            var BstStock = from id in trackdb.ProdBalances
                           join idd in trackdb.GlassTypes on id.Glass_ID equals idd.Glass_ID
                          // join iddd in trackdb.ITEMs on new { id.OC_ID, id.Item_ID, id.Pos } equals new { iddd.OC_ID, iddd.Item_ID, iddd.Pos }
                           //join depar in trackdb.Departments on id.Recived_From equals depar.Departmet_ID
                          // join Bat in trackdb.ProdBalances on id.BalancePatchNo equals Bat.BalancePatchNo
                           //join siz in trackdb.GlassStocks on new { id.Glass_ID, id.Size_No } equals new { siz.Glass_ID, siz.Size_No }
                           where id.Approved == false// && id.QTY_ToDo > 0// && id.Recived_From == 1

                           select new
                           {
                               Batch_No = id.BalancePatchNo,
                               Glass_Type = idd.Glass_Type,
                               //Sheet_Size = siz.Width / 1000 + " X " + siz.Height / 1000,
                               Sheet_QTY = id.QTY,
                               //WorkOrder = id.OC_ID,
                               //Item = id.Item_ID,
                               //Width = iddd.Width,
                               //Height = iddd.Hieght,
                               //QTY_TO_DO = id.QTY_ToDo,
                               //Recieved_From = depar.Department_Name,
                               //Trak_ID = id.Track_ID,
                               //Date = id.Date,


                           };

            DGV_Glass_Req.DataSource = BstStock;

            trackdb.SubmitChanges();

            DGV_Glass_Req.Columns[1].DefaultCellStyle.BackColor = Color.Pink;
            DGV_Glass_Req.Columns[2].DefaultCellStyle.BackColor = Color.Pink;
            for (int i = 0; i < DGV_Glass_Req.Rows.Count; i++)
            {
                if (i % 2 == 0)
                {
                    DGV_Glass_Req.Rows[i].DefaultCellStyle.BackColor = Color.LightBlue;
                }
            }
        }


        private void FRMStock_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'trackingDataSet1.GlassType' table. You can move, or remove it, as needed.
            this.glassTypeTableAdapter.Fill(this.trackingDataSet1.GlassType);
            RefreshStock();

        }

        private void toolStripRefresh_Click(object sender, EventArgs e)
        {
            RefreshStock();
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var track = from id in trackdb.Tracks
                        join idd in trackdb.GlassTypes on id.Glass_ID equals idd.Glass_ID
                        join iddd in trackdb.ITEMs on new { id.OC_ID, id.Item_ID, id.Pos } equals new { iddd.OC_ID, iddd.Item_ID, iddd.Pos }
                        join idDEP in trackdb.Departments on id.Departmet_ID equals idDEP.Departmet_ID
                        where id.BalancePatchNo == int.Parse(txtBatchDetails.Text) &&id.Departmet_ID==1 && id.Send_To==10
                        select new
                        {

                            WorkOrder = id.OC_ID,
                            Item = id.Item_ID,
                            Width = iddd.Width,
                            Height = iddd.Hieght,
                            GlassType = idd.Glass_Type,
                            Item_Qty = id.QTY_Send,
                            Trak_ID = id.Track_ID
                        };
            DGVBatDetails.DataSource = track;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {

            var approved = from app in trackdb.ProdBalances
                           where app.BalancePatchNo == int.Parse(DGV_Glass_Req.SelectedRows[0].Cells[0].Value.ToString())
                           select app;
            foreach (ProdBalance app in approved)
            {
                app.Approved = true;
                trackdb.SubmitChanges();

            }
            var work = from up in trackdb.Tracks
                       join iddd in trackdb.ITEMs on new { up.OC_ID, up.Item_ID, up.Pos } equals new { iddd.OC_ID, iddd.Item_ID, iddd.Pos }
                       join dep in trackdb.Departments on up.Departmet_ID equals dep.Departmet_ID
                       join bat in trackdb.BalancePatches on up.BalancePatchNo  equals bat.BalancePatchNo
                       where up.Departmet_ID==10 && up.QTY_ToDo >0 && up.BalancePatchNo == int.Parse(DGV_Glass_Req.SelectedRows[0].Cells[0].Value.ToString())

                       select up;
           

            foreach (Track up in work)
            {
                up.QTY_Send = up.QTY_ToDo;
                up.QTY_ToDo = 0;
                

                up.Send_To = 1;

                Track add = new Track();
                add.OC_ID = up.OC_ID;
                add.Item_ID = up.Item_ID;
                add.Pos = up.Pos;
                add.Glass_ID = up.Glass_ID;
                add.QTY_Recive = up.QTY_Send;
                add.QTY_ToDo = up.QTY_Send;
                //add.QTY_Send = 0:
                add.Recived_From = 10;
                add.Balance = true;
                add.Date = DateTime.Today;
                add.Shape = up.Shape;
                add.Step = up.Step;
                add.Departmet_ID = 1;
                add.Track_ID_Parent = up.Track_ID;


                trackdb.Tracks.InsertOnSubmit(add);

                trackdb.SubmitChanges();
                RefreshStock();

            }
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {

            System.Diagnostics.Process.Start("explorer.exe", @"\\SERVER\Work Orders_From 30000");
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("calc");
        }

        private void copyAlltoClipboard()
        {

            DGV_batch_Detail.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            DGV_batch_Detail.MultiSelect = true;
            DGV_batch_Detail.SelectAll();
            DataObject dataObj = DGV_batch_Detail.GetClipboardContent();
            if (dataObj != null)
                Clipboard.SetDataObject(dataObj);

        }
        private void btnExcelcut_Click(object sender, EventArgs e)
        {
            copyAlltoClipboard();
            Microsoft.Office.Interop.Excel.Application xlexcel;
            Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
            Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;
            xlexcel = new Excel.Application();
            xlexcel.Visible = true;
            xlWorkBook = xlexcel.Workbooks.Add(misValue);
            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
            Excel.Range CR = (Excel.Range)xlWorkSheet.Cells[1, 1];
            CR.Select();
           // xlWorkSheet.PasteSpecial(CR, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, true);
        }

        private void btnFinishcut_Click(object sender, EventArgs e)
        {
           
            var batch_Detail = from id in trackdb.Tracks
                           join idd in trackdb.GlassTypes on id.Glass_ID equals idd.Glass_ID
                           join iddd in trackdb.ITEMs on new { id.OC_ID, id.Item_ID, id.Pos } equals new { iddd.OC_ID, iddd.Item_ID, iddd.Pos }
                           join depar in trackdb.Departments on id.Recived_From equals depar.Departmet_ID
                           join Bat in trackdb.ProdBalances on id.BalancePatchNo equals Bat.BalancePatchNo
                           //join siz in trackdb.GlassStocks on new { Bat.Glass_ID, Bat.Size_No } equals new { siz.Glass_ID, siz.Size_No }
                           where id.Date <= dateTostk.Value && id.Date >= datefromstk.Value && id.Recived_From == 10

                           select new
                           {
                               Batch_No = Bat.BalancePatchNo,
                               Glass_Type = idd.Glass_Type,
                               //Sheet_Size = siz.Width / 1000 + " X " + siz.Height / 1000,
                               WorkOrder = id.OC_ID,
                               Sheet_QTY = Bat.QTY,
                               Item = id.Item_ID,
                               Width = iddd.Width,
                               Height = iddd.Hieght,
                               GlassType = idd.Glass_Type,
                               QTY_TO_DO = id.QTY_ToDo,
                               Recieved_From = depar.Department_Name,
                               Trak_ID = id.Track_ID,
                               Date = id.Date,
                           };
                           DGV_batch_Detail.DataSource = batch_Detail;
            for (int i = 0; i < DGV_batch_Detail.Rows.Count; i++)
            {
                if (i % 2 == 0)
                {
                    DGV_batch_Detail.Rows[i].DefaultCellStyle.BackColor = Color.LightBlue;
                }
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            Utility.onlynumber(e);
        }
    }
}