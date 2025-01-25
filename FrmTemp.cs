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
    public partial class FrmTemp : Form
    {
        TrackingDataContext trackdb = new TrackingDataContext();
        public FrmTemp()
        {
            InitializeComponent();
        }
        private void RefreshTemp()
        {
            DGV_Temp_todo.DataSource = "";
            
            var grind = from id in trackdb.Tracks
                        join idd in trackdb.GlassTypes on id.Glass_ID equals idd.Glass_ID
                        join iddd in trackdb.ITEMs on new { id.OC_ID, id.Item_ID, id.Pos } equals new { iddd.OC_ID, iddd.Item_ID, iddd.Pos }
                        join depar in trackdb.Departments on id.Recived_From equals depar.Departmet_ID
                        where id.Departmet_ID == 4 && id.QTY_ToDo > 0

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
                            Date = id.Date,
                            POS = id.Pos,


                        };

            DGV_Temp_todo.DataSource = grind;

            trackdb.SubmitChanges();

            DGV_Temp_todo.Columns[5].DefaultCellStyle.BackColor = Color.Pink;
            for (int i = 0; i < DGV_Temp_todo.Rows.Count; i++)
            {
                if (i % 2 == 0)
                {
                    DGV_Temp_todo.Rows[i].DefaultCellStyle.BackColor = Color.LightBlue;
                }
            }

            int sumqty_grind = 0;
            for (int i = 0; i < DGV_Temp_todo.Rows.Count; ++i)
            {
                sumqty_grind += Convert.ToInt32(DGV_Temp_todo.Rows[i].Cells[5].Value);

            }
            txt_work_Qty.Text = sumqty_grind.ToString();

            double sumArea_grind = 0;
            for (int i = 0; i < DGV_Temp_todo.Rows.Count; ++i)
            {
                int qty = Convert.ToInt32(DGV_Temp_todo.Rows[i].Cells[5].Value);
                double w = Convert.ToDouble(DGV_Temp_todo.Rows[i].Cells[2].Value);
                double h = Convert.ToDouble(DGV_Temp_todo.Rows[i].Cells[3].Value);
                double raw1 = (qty * w * h) / 1000000;
                sumArea_grind += Convert.ToDouble(raw1);

            }
            txt_tarea_Temp.Text = sumArea_grind.ToString();
        }

        private void FrmTemp_Load(object sender, EventArgs e)
        {
            comboShift.DataSource = trackdb.Shfits.ToList();
            comboShift.ValueMember = "ID";
            comboShift.DisplayMember = "Shift";
            comboShift.Text = "";

            comboSV.DataSource = trackdb.Logins.Where(x => x.Departmet_ID == 4).ToList();
            comboSV.ValueMember = "ID";
            comboSV.DisplayMember = "Name";
            comboSV.Text = "";

            comboQC.DataSource = trackdb.Logins.Where(x => x.Departmet_ID == 9).ToList();
            comboQC.ValueMember = "ID";
            comboQC.DisplayMember = "Name";
            comboQC.Text = "";

            comboQA.DataSource = trackdb.Logins.Where(x => x.Departmet_ID == 10).ToList();
            comboQA.ValueMember = "ID";
            comboQA.DisplayMember = "Name";
            comboQA.Text = "";




            RefreshTemp();


        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void toolStripRefreshTemp_Click(object sender, EventArgs e)
        {
            txtOrderFilter.Text = "";
            RefreshTemp();
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
          
        }

        private void btnclear_Click(object sender, EventArgs e)
        {
          
        }

        private void btn_Temp_send_Click(object sender, EventArgs e)
        {
            // ************update the work to do for this department



            if (txt_qtysend_Temp.Text == "" || comboShift.Text == "" || comboSV.Text == "" || comboQC.Text == "" || comboQA.Text == "")
            { MessageBox.Show(@"من فضلك ادخل البيانات المطلوبة "); }

            else
            {

                var work = from up in trackdb.Tracks
                           join iddd in trackdb.ITEMs on new { up.OC_ID, up.Item_ID, up.Pos } equals new { iddd.OC_ID, iddd.Item_ID, iddd.Pos }
                           join dep in trackdb.Departments on up.Departmet_ID equals dep.Departmet_ID
                           where up.Track_ID == int.Parse(txt_track_ID.Text)

                           select up;

                foreach (Track up in work)
                {
                    if (up.QTY_ToDo < int.Parse(txt_qtysend_Temp.Text))
                    { MessageBox.Show(@"الكمية المدخلة أكبر من المطلوب"); }
                    else
                    {
                        up.QTY_ToDo = up.QTY_ToDo - int.Parse(txt_qtysend_Temp.Text);
                        up.QTY_Send = int.Parse(txt_qtysend_Temp.Text);

                        ////////// Send to Next ///////////////////
                        var Next = (from Ne in trackdb.Tracks
                                    join iddd in trackdb.ITEMs on new { Ne.OC_ID, Ne.Item_ID, Ne.Pos, Ne.Glass_ID } equals new { iddd.OC_ID, iddd.Item_ID, iddd.Pos, iddd.Glass_ID }
                                    where Ne.Track_ID == int.Parse(txt_track_ID.Text)
                                    select new { DepID = Ne.Departmet_ID, step1 = iddd.Step1, step2 = iddd.Step2, step3 = iddd.Step3, step4 = iddd.Step4, step5 = iddd.Step5, step6 = iddd.Step6, step7 = iddd.Step7, step8 = iddd.Step8, }).SingleOrDefault();

                        if (Next.DepID == Next.step1 && Next.step2 != null) { up.Send_To = Next.step2; }
                        else if (Next.DepID == Next.step2 && Next.step3 != null) { up.Send_To = Next.step3; }
                        else if (Next.DepID == Next.step3 && Next.step4 != null) { up.Send_To = Next.step4; }
                        else if (Next.DepID == Next.step4 && Next.step5 != null) { up.Send_To = Next.step5; }
                        else if (Next.DepID == Next.step5 && Next.step6 != null) { up.Send_To = Next.step6; }
                        else if (Next.DepID == Next.step6 && Next.step7 != null) { up.Send_To = Next.step7; }
                        else if (Next.DepID == Next.step7 && Next.step8 != null) { up.Send_To = Next.step8; }
                        else { up.Send_To = 7; }
                        /////////////////////////////////////////
                        var glassTemp = (from fr in trackdb.GlassTypes where fr.Glass_ID == up.Glass_ID select new { fr.Temp_Cost }).SingleOrDefault();
                        ///////////

                        Track add = new Track();
                        add.OC_ID = up.OC_ID;
                        add.Item_ID = up.Item_ID;
                        add.Pos = up.Pos;
                        add.Glass_ID = up.Glass_ID;
                        add.QTY_Recive = int.Parse(txt_qtysend_Temp.Text);
                        add.QTY_ToDo = int.Parse(txt_qtysend_Temp.Text);
                        add.Recived_From = up.Departmet_ID;
                        /////////
                        var Shift = trackdb.Shfits.Single(Name => Name.Shift == comboShift.Text);
                        var SV = trackdb.Logins.Single(Name => Name.Name == comboSV.Text);
                        var QC = trackdb.Logins.Single(Name => Name.Name == comboQC.Text);
                        var QA = trackdb.Logins.Single(Name => Name.Name == comboQA.Text);
                        add.Shift = Shift.ID;
                        add.Recived_From_SV = SV.ID;
                        add.Recived_From_QC = QC.ID;
                        add.Recived_From_QA = QA.ID;
                        ///////////
                        add.Standard_Unit_Process_Cost = glassTemp.Temp_Cost;
                        //add.ProcessType = 18;

                        add.Balance = up.Balance;
                        add.Shape = up.Shape;
                        add.Step = up.Step;
                        add.Temp = true;
                        add.Departmet_ID = up.Send_To;
                        add.Track_ID_Parent = up.Track_ID;
                        
                        
                        if (radioSouth.Checked)
                        { add.Furnace = "South Tech"; }
                        else { add.Furnace = "Glass Robort"; }

                        if (radioHS.Checked)
                        { add.ProcessType = 24; }
                        else { add.ProcessType = 18; }



                        trackdb.Tracks.InsertOnSubmit(add);

                        trackdb.SubmitChanges();


                        txt_track_ID.Text = "";
                        txt_oc_Temp.Text = "";
                        txt_item_Temp.Text = "";
                        txt_w_Temp.Text = "";
                        txt_H_Temp.Text = "";
                        txt_glaa_Temp.Text = "";
                        txt_qtysend_Temp.Text = "";
                       
                        MessageBox.Show(@"The Items sent to next department");
                    }
                }
                RefreshTemp();
            
        }
    }

        private void btnFinishTemp_Click(object sender, EventArgs e)
        {

            var track = from id in trackdb.Tracks
                        join idd in trackdb.GlassTypes on id.Glass_ID equals idd.Glass_ID
                        join iddd in trackdb.ITEMs on new { id.OC_ID, id.Item_ID, id.Pos } equals new { iddd.OC_ID, iddd.Item_ID, iddd.Pos }
                        join idDEP in trackdb.Departments on id.Departmet_ID equals idDEP.Departmet_ID
                        where (id.Date <= dateToTemp.Value && id.Date >= datefromTemp.Value) && id.Recived_From == 4
                        select new
                        {

                            WorkOrder = id.OC_ID,
                            Item = id.Item_ID,
                            Width = iddd.Width,
                            Height = iddd.Hieght,
                            GlassType = idd.Glass_Type,
                            Qty_Send = id.QTY_Recive,
                            Send_To = idDEP.Department_Name,
                            Furnace =id.Furnace,
                            Balance = id.Balance,
                            Trak_ID = id.Track_ID
                        };
            DGV_Temp_Detail.DataSource = track;

            for (int i = 0; i < DGV_Temp_Detail.Rows.Count; i++)
            {
                if (i % 2 == 0)
                {
                    DGV_Temp_Detail.Rows[i].DefaultCellStyle.BackColor = Color.LightBlue;
                }
            }




        }

        private void copyAlltoClipboard()
        {

            DGV_Temp_Detail.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            DGV_Temp_Detail.MultiSelect = true;
            DGV_Temp_Detail.SelectAll();
            DataObject dataObj = DGV_Temp_Detail.GetClipboardContent();
            if (dataObj != null)
                Clipboard.SetDataObject(dataObj);

        }
        private void btnExcelTemp_Click(object sender, EventArgs e)
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

        private void DGV_Temp_todo_DoubleClick(object sender, EventArgs e)
        {
            txt_oc_Temp.Text = DGV_Temp_todo.SelectedRows[0].Cells[0].Value.ToString();
            txt_item_Temp.Text = DGV_Temp_todo.SelectedRows[0].Cells[1].Value.ToString();
            txt_w_Temp.Text = DGV_Temp_todo.SelectedRows[0].Cells[2].Value.ToString();
            txt_H_Temp.Text = DGV_Temp_todo.SelectedRows[0].Cells[3].Value.ToString();
            txt_glaa_Temp.Text = DGV_Temp_todo.SelectedRows[0].Cells[4].Value.ToString();
            txt_track_ID.Text = DGV_Temp_todo.SelectedRows[0].Cells[7].Value.ToString();
            txt_qtysend_Temp.Focus();
        }

        private void txt_qtysend_Temp_KeyPress(object sender, KeyPressEventArgs e)
        {
            Utility.onlynumber(e);
        }

        private void txtOrderFilter_KeyDown(object sender, KeyEventArgs e)
        {
            if (txtOrderFilter.Text == "" && e.KeyCode == Keys.Enter)
            { MessageBox.Show(@"Please enter the Order No."); }
            else
            {

                if (e.KeyCode == Keys.Enter)
                {
                    var filter = from fil in trackdb.Tracks
                                 join idd in trackdb.GlassTypes on fil.Glass_ID equals idd.Glass_ID
                                 join iddd in trackdb.ITEMs on new { fil.OC_ID, fil.Item_ID, fil.Pos } equals new { iddd.OC_ID, iddd.Item_ID, iddd.Pos }
                                 join depar in trackdb.Departments on fil.Recived_From equals depar.Departmet_ID
                                 where fil.Departmet_ID == 4 && fil.QTY_ToDo > 0 && fil.OC_ID == int.Parse(txtOrderFilter.Text)

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
                                     POS = fil.Pos,

                                 };

                    DGV_Temp_todo.DataSource = filter;
                }
            }
        }

        private void txtOrderFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            Utility.onlynumber(e);
        }

        private void btnSelectItem_Click(object sender, EventArgs e)
        {
           
                for (int i = 0; i < DGV_Temp_todo.SelectedRows.Count; i++)
                {
                    var FullOrder = from up in trackdb.Tracks
                                    join iddd in trackdb.ITEMs on new { up.OC_ID, up.Item_ID, up.Pos } equals new { iddd.OC_ID, iddd.Item_ID, iddd.Pos }
                                    join dep in trackdb.Departments on up.Departmet_ID equals dep.Departmet_ID
                                    where up.Track_ID == int.Parse(DGV_Temp_todo.SelectedRows[i].Cells[7].Value.ToString())

                                    select up;

                    foreach (Track up in FullOrder)
                    {


                        up.QTY_Send = up.QTY_ToDo;
                        up.QTY_ToDo = 0;

                    ////////// Send to Next ///////////////////
                    var Next = (from Ne in trackdb.Tracks
                                join iddd in trackdb.ITEMs on new { Ne.OC_ID, Ne.Item_ID, Ne.Pos, Ne.Glass_ID } equals new { iddd.OC_ID, iddd.Item_ID, iddd.Pos, iddd.Glass_ID }
                                where Ne.Track_ID == int.Parse(DGV_Temp_todo.SelectedRows[i].Cells[7].Value.ToString())
                                select new { DepID = Ne.Departmet_ID, step1 = iddd.Step1, step2 = iddd.Step2, step3 = iddd.Step3, step4 = iddd.Step4, step5 = iddd.Step5, step6 = iddd.Step6, step7 = iddd.Step7, step8 = iddd.Step8, }).SingleOrDefault();

                    if (Next.DepID == Next.step1 && Next.step2 != null) { up.Send_To = Next.step2; }
                    else if (Next.DepID == Next.step2 && Next.step3 != null) { up.Send_To = Next.step3; }
                    else if (Next.DepID == Next.step3 && Next.step4 != null) { up.Send_To = Next.step4; }
                    else if (Next.DepID == Next.step4 && Next.step5 != null) { up.Send_To = Next.step5; }
                    else if (Next.DepID == Next.step5 && Next.step6 != null) { up.Send_To = Next.step6; }
                    else if (Next.DepID == Next.step6 && Next.step7 != null) { up.Send_To = Next.step7; }
                    else if (Next.DepID == Next.step7 && Next.step8 != null) { up.Send_To = Next.step8; }
                    else { up.Send_To = 7; }
                    /////////////////////////////////////////
                    var glassTemp = (from fr in trackdb.GlassTypes where fr.Glass_ID == up.Glass_ID select new { fr.Temp_Cost }).SingleOrDefault();
                    ///////////
                    Track add = new Track();
                        add.OC_ID = up.OC_ID;
                        add.Item_ID = up.Item_ID;
                        add.Pos = up.Pos;
                        add.Glass_ID = up.Glass_ID;
                        add.QTY_Recive = up.QTY_Send;
                        add.QTY_ToDo = up.QTY_Send;
                        add.Recived_From = up.Departmet_ID;
                    /////////
                    var Shift = trackdb.Shfits.Single(Name => Name.Shift == comboShift.Text);
                    var SV = trackdb.Logins.Single(Name => Name.Name == comboSV.Text);
                    var QC = trackdb.Logins.Single(Name => Name.Name == comboQC.Text);
                    var QA = trackdb.Logins.Single(Name => Name.Name == comboQA.Text);
                    add.Shift = Shift.ID;
                    add.Recived_From_SV = SV.ID;
                    add.Recived_From_QC = QC.ID;
                    add.Recived_From_QA = QA.ID;
                    ///////////
                    add.Standard_Unit_Process_Cost = glassTemp.Temp_Cost;
                   // add.ProcessType = 18;

                    add.Balance = up.Balance;
                        add.Shape = up.Shape;
                        add.Step = up.Step;
                        add.Departmet_ID = up.Send_To;
                        add.Track_ID_Parent = up.Track_ID;

                    if (radioSouth.Checked)
                    { add.Furnace = "South Tech"; }
                    else { add.Furnace = "Glass Robort"; }

                    if (radioHS.Checked)
                    { add.ProcessType = 24; }
                    else { add.ProcessType = 18; }


                    trackdb.Tracks.InsertOnSubmit(add);

                        trackdb.SubmitChanges();


                        txt_track_ID.Text = "";
                        txt_oc_Temp.Text = "";
                        txt_item_Temp.Text = "";
                        txt_w_Temp.Text = "";
                        txt_H_Temp.Text = "";
                        txt_glaa_Temp.Text = "";
                        txt_qtysend_Temp.Text = "";



                    }
                }
                RefreshTemp();
                MessageBox.Show(@"The Items sent to next department");
          
        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {

        }

        private void btncleardep_Click(object sender, EventArgs e)
        {
            txt_cut_Man.Text = "";
            txt_cut_downRes.Text = "";
            txt_cut_downT.Text = "";
            txt_cut_prodT.Text = "";
            txt_cut_remark.Text = "";

            txtPlanCap.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboShift.Text == "" || comboSV.Text == "" || comboQC.Text == "" || comboQA.Text == "")
            { MessageBox.Show(@"من فضلك ادخل البيانات المطلوبة "); }

            else
            {
                var Shift = trackdb.Shfits.Single(Name => Name.Shift == comboShift.Text);
                var SV = trackdb.Logins.Single(Name => Name.Name == comboSV.Text);
                var QC = trackdb.Logins.Single(Name => Name.Name == comboQC.Text);
                var QA = trackdb.Logins.Single(Name => Name.Name == comboQA.Text);

                if (radioSouth.Checked)
                {
                    var check = (from ch in trackdb.DepDatas
                                 where ch.Date == dateCut.Value && ch.Department_ID == 4 && ch.Shift_ID == Shift.ID && ch.Furnace== "South Tech"
                                 select ch).SingleOrDefault();
                    if (check == null)
                    {

                        DepData add = new DepData();
                        add.Date = dateCut.Value;
                        add.Department_ID = 4;
                        add.Shift_ID = Shift.ID;
                        add.SV = SV.ID;
                        add.SV_QC = QC.ID;
                        add.SV_QA = QA.ID;
                        add.FullCapacity = txtFullCap.Text.Length == 0 ? 0 : int.Parse(txtFullCap.Text);
                        add.PlanCapacity = txtPlanCap.Text.Length == 0 ? 0 : int.Parse(txtPlanCap.Text);
                        add.Man_power = txt_cut_Man.Text.Length == 0 ? 0 : int.Parse(txt_cut_Man.Text);
                        add.ProdTime = txt_cut_prodT.Text.Length == 0 ? 0 : int.Parse(txt_cut_prodT.Text);
                        add.ProdDown = txt_cut_downT.Text.Length == 0 ? 0 : int.Parse(txt_cut_downT.Text);
                        add.ReasonDwn = txt_cut_downRes.Text;
                        add.Remark = txt_cut_remark.Text;
                        add.Furnace = "South Tech";

                        trackdb.DepDatas.InsertOnSubmit(add);
                        trackdb.SubmitChanges();
                        btncleardep.PerformClick();
                    }
                    else
                    {

                        check.Date = DateTime.Today;
                        check.Department_ID = 4;
                        check.Shift_ID = Shift.ID;
                        check.SV = SV.ID;
                        check.SV_QC = QC.ID;
                        check.SV_QA = QA.ID;
                        check.FullCapacity = txtFullCap.Text.Length == 0 ? 0 : int.Parse(txtFullCap.Text);
                        check.PlanCapacity = txtPlanCap.Text.Length == 0 ? 0 : int.Parse(txtPlanCap.Text);
                        check.Man_power = txt_cut_Man.Text.Length == 0 ? 0 : int.Parse(txt_cut_Man.Text);
                        check.ProdTime = txt_cut_prodT.Text.Length == 0 ? 0 : int.Parse(txt_cut_prodT.Text);
                        check.ProdDown = txt_cut_downT.Text.Length == 0 ? 0 : int.Parse(txt_cut_downT.Text);
                        check.ReasonDwn = txt_cut_downRes.Text;
                        check.Remark = txt_cut_remark.Text;

                        trackdb.SubmitChanges();
                        btncleardep.PerformClick();
                    }
                }
                if (radioRobot.Checked)
                {
                    var check = (from ch in trackdb.DepDatas
                                 where ch.Date == dateCut.Value && ch.Department_ID == 4 && ch.Shift_ID == Shift.ID && ch.Furnace == "Glass Robort"
                                 select ch).SingleOrDefault();
                    if (check == null)
                    {
                        DepData add = new DepData();
                        add.Date = dateCut.Value;
                        add.Department_ID = 4;
                        add.Shift_ID = Shift.ID;
                        add.SV = SV.ID;
                        add.SV_QC = QC.ID;
                        add.SV_QA = QA.ID;
                        add.FullCapacity = txtFullCap.Text.Length == 0 ? 0 : int.Parse(txtFullCap.Text);
                        add.PlanCapacity = txtPlanCap.Text.Length == 0 ? 0 : int.Parse(txtPlanCap.Text);
                        add.Man_power = txt_cut_Man.Text.Length == 0 ? 0 : int.Parse(txt_cut_Man.Text);
                        add.ProdTime = txt_cut_prodT.Text.Length == 0 ? 0 : int.Parse(txt_cut_prodT.Text);
                        add.ProdDown = txt_cut_downT.Text.Length == 0 ? 0 : int.Parse(txt_cut_downT.Text);
                        add.ReasonDwn = txt_cut_downRes.Text;
                        add.Remark = txt_cut_remark.Text;
                        add.Furnace = "Glass Robort";


                        trackdb.DepDatas.InsertOnSubmit(add);
                        trackdb.SubmitChanges();
                        btncleardep.PerformClick();
                    }

                    else
                    {

                        check.Date = DateTime.Today;
                        check.Department_ID = 4;
                        check.Shift_ID = Shift.ID;
                        check.SV = SV.ID;
                        check.SV_QC = QC.ID;
                        check.SV_QA = QA.ID;
                        check.FullCapacity = txtFullCap.Text.Length == 0 ? 0 : int.Parse(txtFullCap.Text);
                        check.PlanCapacity = txtPlanCap.Text.Length == 0 ? 0 : int.Parse(txtPlanCap.Text);
                        check.Man_power = txt_cut_Man.Text.Length == 0 ? 0 : int.Parse(txt_cut_Man.Text);
                        check.ProdTime = txt_cut_prodT.Text.Length == 0 ? 0 : int.Parse(txt_cut_prodT.Text);
                        check.ProdDown = txt_cut_downT.Text.Length == 0 ? 0 : int.Parse(txt_cut_downT.Text);
                        check.ReasonDwn = txt_cut_downRes.Text;
                        check.Remark = txt_cut_remark.Text;

                        trackdb.SubmitChanges();
                        btncleardep.PerformClick();
                    }
                }
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            btncleardep.PerformClick();

            if (comboShift.Text == "")
            { MessageBox.Show(@"من فضلك ادخل البيانات المطلوبة "); }

            else
            {
                var Shift = trackdb.Shfits.Single(Name => Name.Shift == comboShift.Text);

                if (radioRobot.Checked)
                {
                    var check = (from ch in trackdb.DepDatas
                                 where ch.Date == dateCut.Value && ch.Department_ID == 4 && ch.Shift_ID == Shift.ID && ch.Furnace == "Glass Robort"
                                 select ch).SingleOrDefault();
                    if (check == null)
                    {
                        MessageBox.Show(@"لا توجد بيانات محفوظة ");
                    }
                    else
                    {
                        var DEPSV = (from did in trackdb.Logins where check.SV == did.ID select new { did.Name }).SingleOrDefault();
                        if (DEPSV != null || Convert.ToInt32(DEPSV) != 0) { comboSV.Text = DEPSV.Name; }

                        var SVQC = (from did in trackdb.Logins where check.SV_QC == did.ID select new { did.Name }).SingleOrDefault();
                        if (SVQC != null || Convert.ToInt32(SVQC) != 0) { comboQC.Text = SVQC.Name; }

                        var DEPQA = (from did in trackdb.Logins where check.SV_QA == did.ID select new { did.Name }).SingleOrDefault();
                        if (DEPQA != null || Convert.ToInt32(DEPQA) != 0) { comboQA.Text = DEPQA.Name; }

                        txtFullCap.Text = check.FullCapacity.ToString();
                        txtPlanCap.Text = check.PlanCapacity.ToString();
                        txt_cut_Man.Text = check.Man_power.ToString();
                        txt_cut_prodT.Text = check.ProdTime.ToString();

                        txt_cut_downT.Text = check.ProdDown.ToString();
                        txt_cut_downRes.Text = check.ReasonDwn.ToString();
                        txt_cut_remark.Text = check.Remark;

                    }
                }

                else if (radioSouth.Checked)
                {
                    var check = (from ch in trackdb.DepDatas
                                 where ch.Date == dateCut.Value && ch.Department_ID == 4 && ch.Shift_ID == Shift.ID && ch.Furnace == "South Tech"
                                 select ch).SingleOrDefault();
                    if (check == null)
                    {
                        MessageBox.Show(@"لا توجد بيانات محفوظة ");
                    }
                    else
                    {
                        var DEPSV = (from did in trackdb.Logins where check.SV == did.ID select new { did.Name }).SingleOrDefault();
                        if (DEPSV != null || Convert.ToInt32(DEPSV) != 0) { comboSV.Text = DEPSV.Name; }

                        var SVQC = (from did in trackdb.Logins where check.SV_QC == did.ID select new { did.Name }).SingleOrDefault();
                        if (SVQC != null || Convert.ToInt32(SVQC) != 0) { comboQC.Text = SVQC.Name; }

                        var DEPQA = (from did in trackdb.Logins where check.SV_QA == did.ID select new { did.Name }).SingleOrDefault();
                        if (DEPQA != null || Convert.ToInt32(DEPQA) != 0) { comboQA.Text = DEPQA.Name; }

                        txtFullCap.Text = check.FullCapacity.ToString();
                        txtPlanCap.Text = check.PlanCapacity.ToString();
                        txt_cut_Man.Text = check.Man_power.ToString();
                        txt_cut_prodT.Text = check.ProdTime.ToString();

                        txt_cut_downT.Text = check.ProdDown.ToString();
                        txt_cut_downRes.Text = check.ReasonDwn.ToString();
                        txt_cut_remark.Text = check.Remark;

                    }
                }
            }
        }
    }
}
