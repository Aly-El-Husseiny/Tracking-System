using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Cutting
{
    public partial class FRM_dispatch : Form
    {
        TrackingDataContext trackdb = new TrackingDataContext();
        public FRM_dispatch()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            Frm_Reject newMDIChild = new Frm_Reject();
            newMDIChild.MdiParent = this.MdiParent;
            newMDIChild.Show();
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            FrmBarCode newMDIChild = new FrmBarCode();
            newMDIChild.MdiParent = this.MdiParent;
            newMDIChild.Show();
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe", @"\\SERVER\Work Orders_From 28000");

        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("calc");
        }

        private void btn_data_ok_Click(object sender, EventArgs e)
        {
            DepData add = new DepData();
            add.Date = DateTime.Today;
            add.Department_ID = 7;
            add.Man_power = int.Parse(txt_Dis_Man.Text);
            add.ProdTime = int.Parse(txt_Dis_prodT.Text);
            add.ProdDown = int.Parse(txt_Dis_downT.Text);
            add.ReasonDwn = txt_Dis_downRes.Text;
            add.Remark = txt_Dis_remark.Text;
            txt_Dis_Man.Enabled = false;
            txt_Dis_downRes.Enabled = false;
            txt_Dis_downT.Enabled = false;
            txt_Dis_prodT.Enabled = false;
            txt_Dis_remark.Enabled = false;
            btn_data_ok.Enabled = false;

            trackdb.DepDatas.InsertOnSubmit(add);
            trackdb.SubmitChanges();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            txt_Dis_downRes.Text = "";
            txt_Dis_downT.Text = "";
            txt_Dis_Man.Text = "";
            txt_Dis_prodT.Text = "";
            txt_Dis_remark.Text = "";
        }


        private void FRM_dispatch_Load(object sender, EventArgs e)
        {

        }

        private void toolStripLG_Click(object sender, EventArgs e)
        {
            toolStripIGu.Checked = false;
            toolStripLG.Checked = true;
            toolStripSG.Checked = false;

            txtOrderFilter.Enabled = true;
            txtOrderFilter.Text = "";

            DGV_Dis_todo.DataSource = "";

            var dispatch = from id in trackdb.Tracks
                           join iddd in trackdb.ITEMs on new { id.OC_ID, id.Item_ID, id.Pos } equals new { iddd.OC_ID, iddd.Item_ID, iddd.Pos }
                           join depar in trackdb.Departments on id.Recived_From equals depar.Departmet_ID
                           where id.Departmet_ID == 7 && id.QTY_ToDo > 0 && id.Recived_From == 5

                           select new
                           {

                               WorkOrder = id.OC_ID,
                               Item = id.Item_ID,
                               Width = iddd.Width,
                               Height = iddd.Hieght,
                               GlassType = id.LG,
                               QTY_TO_Work = id.QTY_ToDo,
                               Recieved_From = depar.Department_Name,
                               Trak_ID = id.Track_ID,
                               Date = id.Date


                           };

            DGV_Dis_todo.DataSource = dispatch;

            trackdb.SubmitChanges();

            DGV_Dis_todo.Columns[5].DefaultCellStyle.BackColor = Color.Pink;
            for (int i = 0; i < DGV_Dis_todo.Rows.Count; i++)
            {
                if (i % 2 == 0)
                {
                    DGV_Dis_todo.Rows[i].DefaultCellStyle.BackColor = Color.LightBlue;
                }
            }

            int sumqty_grind = 0;
            for (int i = 0; i < DGV_Dis_todo.Rows.Count; ++i)
            {
                sumqty_grind += Convert.ToInt32(DGV_Dis_todo.Rows[i].Cells[5].Value);

            }
            txt_work_Qty.Text = sumqty_grind.ToString();

            double sumArea_grind = 0;
            for (int i = 0; i < DGV_Dis_todo.Rows.Count; ++i)
            {
                int qty = Convert.ToInt32(DGV_Dis_todo.Rows[i].Cells[5].Value);
                double w = Convert.ToDouble(DGV_Dis_todo.Rows[i].Cells[2].Value);
                double h = Convert.ToDouble(DGV_Dis_todo.Rows[i].Cells[3].Value);
                double raw1 = (qty * w * h) / 1000000;
                sumArea_grind += Convert.ToDouble(raw1);

            }
            txt_tarea_Dis.Text = sumArea_grind.ToString();
        }

        private void toolStripIGu_Click(object sender, EventArgs e)
        {
            toolStripIGu.Checked = true;
            toolStripLG.Checked = false;
            toolStripSG.Checked = false;

            txtOrderFilter.Enabled = true;
            txtOrderFilter.Text = "";

            DGV_Dis_todo.DataSource = "";

            var dispatch = from id in trackdb.Tracks
                           join iddd in trackdb.ITEMs on new { id.OC_ID, id.Item_ID, id.Pos } equals new { iddd.OC_ID, iddd.Item_ID, iddd.Pos }
                           join depar in trackdb.Departments on id.Recived_From equals depar.Departmet_ID
                           where id.Departmet_ID == 7 && id.QTY_ToDo > 0 && (id.Recived_From == 6 || id.Recived_From == 61)

                           select new
                           {

                               WorkOrder = id.OC_ID,
                               Item = id.Item_ID,
                               Width = iddd.Width,
                               Height = iddd.Hieght,
                               GlassType = id.IGU,
                               QTY_TO_Work = id.QTY_ToDo,
                               Recieved_From = depar.Department_Name,
                               Trak_ID = id.Track_ID,
                               Date = id.Date


                           };

            DGV_Dis_todo.DataSource = dispatch;

            trackdb.SubmitChanges();

            DGV_Dis_todo.Columns[5].DefaultCellStyle.BackColor = Color.Pink;
            for (int i = 0; i < DGV_Dis_todo.Rows.Count; i++)
            {
                if (i % 2 == 0)
                {
                    DGV_Dis_todo.Rows[i].DefaultCellStyle.BackColor = Color.LightBlue;
                }
            }

            int sumqty_grind = 0;
            for (int i = 0; i < DGV_Dis_todo.Rows.Count; ++i)
            {
                sumqty_grind += Convert.ToInt32(DGV_Dis_todo.Rows[i].Cells[5].Value);

            }
            txt_work_Qty.Text = sumqty_grind.ToString();

            double sumArea_grind = 0;
            for (int i = 0; i < DGV_Dis_todo.Rows.Count; ++i)
            {
                int qty = Convert.ToInt32(DGV_Dis_todo.Rows[i].Cells[5].Value);
                double w = Convert.ToDouble(DGV_Dis_todo.Rows[i].Cells[2].Value);
                double h = Convert.ToDouble(DGV_Dis_todo.Rows[i].Cells[3].Value);
                double raw1 = (qty * w * h) / 1000000;
                sumArea_grind += Convert.ToDouble(raw1);

            }
            txt_tarea_Dis.Text = sumArea_grind.ToString();
        }

        private void toolStripSG_Click(object sender, EventArgs e)
        {
            toolStripIGu.Checked = false;
            toolStripLG.Checked = false;
            toolStripSG.Checked = true;

            txtOrderFilter.Enabled = true;
            txtOrderFilter.Text = "";

            DGV_Dis_todo.DataSource = "";

            var dispatch = from id in trackdb.Tracks
                           join idd in trackdb.GlassTypes on id.Glass_ID equals idd.Glass_ID
                           join iddd in trackdb.ITEMs on new { id.OC_ID, id.Item_ID, id.Pos } equals new { iddd.OC_ID, iddd.Item_ID, iddd.Pos }
                           join depar in trackdb.Departments on id.Recived_From equals depar.Departmet_ID
                           where id.Departmet_ID == 7 && id.QTY_ToDo > 0

                           select new
                           {

                               WorkOrder = id.OC_ID,
                               Item = id.Item_ID,
                               Width = iddd.Width,
                               Height = iddd.Hieght,
                               GlassType = idd.Glass_Type,
                               QTY_TO_Work = id.QTY_ToDo,
                               Recieved_From = depar.Department_Name,
                               Trak_ID = id.Track_ID,
                               Date = id.Date


                           };

            DGV_Dis_todo.DataSource = dispatch;

            trackdb.SubmitChanges();

            DGV_Dis_todo.Columns[5].DefaultCellStyle.BackColor = Color.Pink;
            for (int i = 0; i < DGV_Dis_todo.Rows.Count; i++)
            {
                if (i % 2 == 0)
                {
                    DGV_Dis_todo.Rows[i].DefaultCellStyle.BackColor = Color.LightBlue;
                }
            }

            int sumqty_grind = 0;
            for (int i = 0; i < DGV_Dis_todo.Rows.Count; ++i)
            {
                sumqty_grind += Convert.ToInt32(DGV_Dis_todo.Rows[i].Cells[5].Value);

            }
            txt_work_Qty.Text = sumqty_grind.ToString();

            double sumArea_grind = 0;
            for (int i = 0; i < DGV_Dis_todo.Rows.Count; ++i)
            {
                int qty = Convert.ToInt32(DGV_Dis_todo.Rows[i].Cells[5].Value);
                double w = Convert.ToDouble(DGV_Dis_todo.Rows[i].Cells[2].Value);
                double h = Convert.ToDouble(DGV_Dis_todo.Rows[i].Cells[3].Value);
                double raw1 = (qty * w * h) / 1000000;
                sumArea_grind += Convert.ToDouble(raw1);

            }
            txt_tarea_Dis.Text = sumArea_grind.ToString();

        }

        private void btnFinishDis_Click(object sender, EventArgs e)
        {
            var track = from id in trackdb.Tracks
                        join idd in trackdb.Orders on id.OC_ID equals idd.OC_ID
                        join iddd in trackdb.ITEMs on new { id.OC_ID, id.Item_ID, id.Pos } equals new { iddd.OC_ID, iddd.Item_ID, iddd.Pos }
                        join idDEP in trackdb.Departments on id.Departmet_ID equals idDEP.Departmet_ID
                        where (id.Date <= dateToDis.Value && id.Date >= datefromDis.Value) && id.Departmet_ID == 7 && id.Delivery_No!=null
                        select new
                        {

                            WorkOrder = id.OC_ID,
                            Item = id.Item_ID,
                            Width = iddd.Width,
                            Height = iddd.Hieght,
                            Description = idd.Descreption,
                            Qty_Delivery = id.QTY_Recive,
                            Delivery_No = id.Delivery_No,
                            Delivery_Date = id.Date,
                            Trak_ID = id.Track_ID
                        };
            DGV_Dis_Detail.DataSource = track;

            for (int i = 0; i < DGV_Dis_Detail.Rows.Count; i++)
            {
                if (i % 2 == 0)
                {
                    DGV_Dis_Detail.Rows[i].DefaultCellStyle.BackColor = Color.LightBlue;
                }
            }
        }

        private void copyAlltoClipboard()
        {

            DGV_Dis_Detail.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            DGV_Dis_Detail.MultiSelect = true;
            DGV_Dis_Detail.SelectAll();
            DataObject dataObj = DGV_Dis_Detail.GetClipboardContent();
            if (dataObj != null)
                Clipboard.SetDataObject(dataObj);
        }
        private void btnExcelDis_Click(object sender, EventArgs e)
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
            xlWorkSheet.PasteSpecial(CR, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, true);

        }

        private void DGV_Dis_todo_DoubleClick(object sender, EventArgs e)
        {
            txt_oc_Dis.Text = DGV_Dis_todo.SelectedRows[0].Cells[0].Value.ToString();
            txt_item_Dis.Text = DGV_Dis_todo.SelectedRows[0].Cells[1].Value.ToString();
            txt_w_Dis.Text = DGV_Dis_todo.SelectedRows[0].Cells[2].Value.ToString();
            txt_H_Dis.Text = DGV_Dis_todo.SelectedRows[0].Cells[3].Value.ToString();
            txt_glaa_Dis.Text = DGV_Dis_todo.SelectedRows[0].Cells[4].Value.ToString();
            txt_track_ID.Text = DGV_Dis_todo.SelectedRows[0].Cells[7].Value.ToString();
            txt_qty_deliv.Focus();
        }

        private void btn_send_Click(object sender, EventArgs e)
        {
            if (txt_qty_deliv.Text == ""||txt_Deliv_No.Text=="")
            { MessageBox.Show(@"Please insert the correct No."); }
            else
            {

                var work = from up in trackdb.Tracks
                           join iddd in trackdb.ITEMs on new { up.OC_ID, up.Item_ID, up.Pos } equals new { iddd.OC_ID, iddd.Item_ID, iddd.Pos }
                           join dep in trackdb.Departments on up.Departmet_ID equals dep.Departmet_ID
                           where up.Track_ID == int.Parse(txt_track_ID.Text)

                           select up;

                foreach (Track up in work)
                {
                    if (up.QTY_ToDo < int.Parse(txt_qty_deliv.Text))
                    { MessageBox.Show(@"الكمية المدخلة أكبر من المطلوب"); }
                    else
                    {
                        up.QTY_ToDo = up.QTY_ToDo - int.Parse(txt_qty_deliv.Text);
                        up.QTY_Send = int.Parse(txt_qty_deliv.Text);

                        //var DepName = trackdb.Departments.Single(Name => Name.Department_Name == combo_send_grind.Text);
                        up.Send_To = 12;

                        Track add = new Track();
                        add.OC_ID = up.OC_ID;
                        add.Item_ID = up.Item_ID;
                        add.Pos = up.Pos;
                        add.QTY_Recive = int.Parse(txt_qty_deliv.Text);
                        add.Recived_From = up.Departmet_ID;
                        add.Balance = up.Balance;
                        add.Delivery_Date = DateTime.Today;
                        add.Shape = up.Shape;
                        add.Step = up.Step;
                        add.Departmet_ID =7;
                        add.Delivery_No = int.Parse(txt_Deliv_No.Text);
                        add.Track_ID_Parent = up.Track_ID;

                        if (up.Glass_ID!=null)
                        { add.Glass_ID = up.Glass_ID; }
                        if(up.LG!=null)
                        { add.LG = up.LG; }
                        if(up.IGU!=null)
                        { add.IGU = up.IGU; }
                        
                        trackdb.Tracks.InsertOnSubmit(add);

                        trackdb.SubmitChanges();


                        txt_track_ID.Text = "";
                        txt_oc_Dis.Text = "";
                        txt_item_Dis.Text = "";
                        txt_w_Dis.Text = "";
                        txt_H_Dis.Text = "";
                        txt_glaa_Dis.Text = "";
                        txt_qty_deliv.Text = "";
                        // combo_send_grind.Text = "";

                        MessageBox.Show(@"The Items sent to the Customer");
                    }
                }
            }
        }

        private void txt_qty_deliv_KeyPress(object sender, KeyPressEventArgs e)
        {
            Utility.onlynumber(e);
        }

        private void txt_Deliv_No_KeyPress(object sender, KeyPressEventArgs e)
        {
            Utility.onlynumber(e);
        }

        private void txtOrderFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            Utility.onlynumber(e);
        }

        private void txtOrderFilter_KeyDown(object sender, KeyEventArgs e)
        {
            if (txtOrderFilter.Text == "" && e.KeyCode == Keys.Enter)
                MessageBox.Show(@"Please enter the Order No."); 
            else if (e.KeyCode == Keys.Enter && toolStripSG.Checked == true)
            {

                var filter = from fil in trackdb.Tracks
                             join idd in trackdb.GlassTypes on fil.Glass_ID equals idd.Glass_ID
                             join iddd in trackdb.ITEMs on new { fil.OC_ID, fil.Item_ID, fil.Pos } equals new { iddd.OC_ID, iddd.Item_ID, iddd.Pos }
                             join depar in trackdb.Departments on fil.Recived_From equals depar.Departmet_ID
                             where fil.Departmet_ID == 7 && fil.QTY_ToDo > 0 && fil.OC_ID == int.Parse(txtOrderFilter.Text)

                             select new
                             {

                                 WorkOrder = fil.OC_ID,
                                 Item = fil.Item_ID,
                                 Width = iddd.Width,
                                 Height = iddd.Hieght,
                                 GlassType = idd.Glass_Type,
                                 QTY_TO_DO = fil.QTY_ToDo,
                                 Recieved_From = depar.Department_Name,
                                 Trak_ID = fil.Track_ID,
                                 Date = fil.Date,


                             };

                DGV_Dis_todo.DataSource = filter;
            }
            else if (e.KeyCode == Keys.Enter && toolStripLG.Checked == true)
            {

                var filter = from fil in trackdb.Tracks
                             join iddd in trackdb.ITEMs on new { fil.OC_ID, fil.Item_ID, fil.Pos } equals new { iddd.OC_ID, iddd.Item_ID, iddd.Pos }
                             join depar in trackdb.Departments on fil.Recived_From equals depar.Departmet_ID
                             where fil.Departmet_ID == 7 && fil.QTY_ToDo > 0 && fil.Recived_From == 5 && fil.OC_ID == int.Parse(txtOrderFilter.Text)

                             select new
                             {

                                 WorkOrder = fil.OC_ID,
                                 Item = fil.Item_ID,
                                 Width = iddd.Width,
                                 Height = iddd.Hieght,
                                 GlassType = fil.LG,
                                 QTY_TO_Work = fil.QTY_ToDo,
                                 Recieved_From = depar.Department_Name,
                                 Trak_ID = fil.Track_ID,
                                 Date = fil.Date
                             };

                DGV_Dis_todo.DataSource = filter;

            }
            else if (e.KeyCode == Keys.Enter && toolStripIGu.Checked == true)
            {

                var filter = from fil in trackdb.Tracks
                             join iddd in trackdb.ITEMs on new { fil.OC_ID, fil.Item_ID, fil.Pos } equals new { iddd.OC_ID, iddd.Item_ID, iddd.Pos }
                             join depar in trackdb.Departments on fil.Recived_From equals depar.Departmet_ID
                             where fil.Departmet_ID == 7 && fil.QTY_ToDo > 0 && fil.Recived_From == 6 && fil.OC_ID == int.Parse(txtOrderFilter.Text)

                             select new
                             {

                                 WorkOrder = fil.OC_ID,
                                 Item = fil.Item_ID,
                                 Width = iddd.Width,
                                 Height = iddd.Hieght,
                                 GlassType = fil.IGU,
                                 QTY_TO_Work = fil.QTY_ToDo,
                                 Recieved_From = depar.Department_Name,
                                 Trak_ID = fil.Track_ID,
                                 Date = fil.Date
                             };

                DGV_Dis_todo.DataSource = filter;
            }

        }

        private void btnSelectItem_Click(object sender, EventArgs e)
        {
            if (txt_Deliv_No.Text == "")
            { MessageBox.Show(@"Please Enter The Delivery Number !!!"); }
            else
            {
                for (int i = 0; i < DGV_Dis_todo.SelectedRows.Count; i++)
                {
                    var FullOrder = from up in trackdb.Tracks
                                    join iddd in trackdb.ITEMs on new { up.OC_ID, up.Item_ID, up.Pos } equals new { iddd.OC_ID, iddd.Item_ID, iddd.Pos }
                                    join dep in trackdb.Departments on up.Departmet_ID equals dep.Departmet_ID
                                    where up.Track_ID == int.Parse(DGV_Dis_todo.SelectedRows[i].Cells[7].Value.ToString())

                                    select up;

                    foreach (Track up in FullOrder)
                    {


                        up.QTY_Send = up.QTY_ToDo;
                        up.QTY_ToDo = 0;

                       
                        up.Send_To = 12;

                        Track add = new Track();
                        add.OC_ID = up.OC_ID;
                        add.Item_ID = up.Item_ID;
                        add.Pos = up.Pos;
                        add.QTY_Recive = up.QTY_Send;
                        add.Recived_From = up.Departmet_ID;
                        add.Balance = up.Balance;
                        add.Date = DateTime.Today;
                        add.Delivery_Date = DateTime.Today;
                        add.Shape = up.Shape;
                        add.Step = up.Step;
                        add.Departmet_ID = 7;
                        add.Delivery_No = int.Parse(txt_Deliv_No.Text);
                        add.Track_ID_Parent = up.Track_ID;

                        if (up.Glass_ID != null)
                        { add.Glass_ID = up.Glass_ID; }
                        if (up.LG != null)
                        { add.LG = up.LG; }
                        if (up.IGU != null)
                        { add.IGU = up.IGU; }

                        trackdb.Tracks.InsertOnSubmit(add);

                        trackdb.SubmitChanges();


                        txt_track_ID.Text = "";
                        txt_oc_Dis.Text = "";
                        txt_item_Dis.Text = "";
                        txt_w_Dis.Text = "";
                        txt_H_Dis.Text = "";
                        txt_glaa_Dis.Text = "";
                        txt_qty_deliv.Text = "";
                        
                    }
                }
                MessageBox.Show(@"The Items sent to next department");
                txt_Deliv_No.Text = "";
            }
        }
    }
}

