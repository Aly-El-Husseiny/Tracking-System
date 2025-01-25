using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Cutting
{
    public partial class FrmReworkReject : Form
    {
        TrackingDataContext trackdb = new TrackingDataContext();
        public FrmReworkReject()
        {
            InitializeComponent();
        }

        private void FrmReworkReject_Load(object sender, EventArgs e)
        {


            comboQCAction.DataSource = trackdb.QC_Actions.ToList();
            comboQCAction.ValueMember = "id";
            comboQCAction.DisplayMember = "QC_Action1";
            comboQCAction.Text = "";


            comboReject.DataSource = trackdb.QC_rejs.ToList();
            comboReject.ValueMember = "Rej_ID";
            comboReject.DisplayMember = "Rej_Name";
            comboReject.Text = "";

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






        }

        private void toolStripRefresh_Click(object sender, EventArgs e)
        {

        }

        private void DGV_cut_todo_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void DGV_cut_todo_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void DGV_cut_todo_DoubleClick(object sender, EventArgs e)
        {
            txt_oc_cut.Text = DGV_cut_todo.SelectedRows[0].Cells[0].Value.ToString();
            txt_item_cut.Text = DGV_cut_todo.SelectedRows[0].Cells[1].Value.ToString();
            txt_w_cut.Text = DGV_cut_todo.SelectedRows[0].Cells[2].Value.ToString();
            txt_H_cut.Text = DGV_cut_todo.SelectedRows[0].Cells[3].Value.ToString();
            if (DGV_cut_todo.SelectedRows[0].Cells[11].Value != null) { txt_glaa_cut.Text = DGV_cut_todo.SelectedRows[0].Cells[11].Value.ToString(); }
            else if (DGV_cut_todo.SelectedRows[0].Cells[10].Value != null) { txt_glaa_cut.Text = DGV_cut_todo.SelectedRows[0].Cells[10].Value.ToString(); }
            else
            { txt_glaa_cut.Text = DGV_cut_todo.SelectedRows[0].Cells[4].Value.ToString(); }
            txt_track_ID.Text = DGV_cut_todo.SelectedRows[0].Cells[7].Value.ToString();

            var IGURej = from id in trackdb.ITEMs
                            
                         where id.OC_ID == int.Parse(txt_oc_cut.Text) && id.Item_ID == int.Parse(txt_item_cut.Text)
                         select new
                         {

                             WorkOrder = id.OC_ID,
                             Item = id.Item_ID,
                             POS = id.Pos,
                             GlassID = id.Glass_ID,
                             Width = id.Width,
                             Height = id.Hieght,
                            

                             

                            
                         };
            DGV_Grind_Detail.DataSource = IGURej;

            txt_qtysend_cut.Focus();

           

        }

        private void CompoDep_SelectedIndexChanged(object sender, EventArgs e)
        {
          
            var DEPID = trackdb.Departments.Single(Name => Name.Department_Name == ComboDepartment.Text);

            comboSV.DataSource = trackdb.Logins.Where(x => x.Departmet_ID == DEPID.Departmet_ID).ToList();
            comboSV.ValueMember = "ID";
            comboSV.DisplayMember = "Name";
            comboSV.Text = "";

            DGV_cut_todo.DataSource = "";
            comboRejdepa.Text = ComboDepartment.Text;

            
            // add.Shift = Shift.ID;
            var cut = from id in trackdb.Tracks
                      join idd in trackdb.GlassTypes on id.Glass_ID equals idd.Glass_ID
                      join iddd in trackdb.ITEMs on new { id.OC_ID, id.Item_ID, id.Pos } equals new { iddd.OC_ID, iddd.Item_ID, iddd.Pos }
                      join depar in trackdb.Departments on id.Recived_From equals depar.Departmet_ID

                      where id.Departmet_ID == DEPID.Departmet_ID && id.QTY_ToDo > 0

                      select new
                      {

                          WorkOrder = id.OC_ID,
                          Item = id.Item_ID,
                          Width = iddd.Width,
                          Height = iddd.Hieght,
                          GlassType = idd.Glass_Type,

                          QTY_TO_DO = id.QTY_ToDo,
                          Recieved_From = depar.Department_Name,
                          Trak_ID = id.Track_ID,

                          Date = id.Date,
                          POS = id.Pos,

                          LG = id.LG,
                          IGU =id.IGU,
                         

                      };

            DGV_cut_todo.DataSource = cut;

            trackdb.SubmitChanges();

            DGV_cut_todo.Columns[5].DefaultCellStyle.BackColor = Color.Pink;
            for (int i = 0; i < DGV_cut_todo.Rows.Count; i++)
            {
                if (i % 2 == 0)
                {
                    DGV_cut_todo.Rows[i].DefaultCellStyle.BackColor = Color.LightBlue;
                }
            }

        }

        private void txt_qtysend_cut_KeyPress(object sender, KeyPressEventArgs e)
        {
            Utility.onlynumber(e);
        }

        private void txtOrderFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            Utility.onlynumber(e);
        }

        private void btn_Send_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("سيتم اعادة العمل على الزجاج المحدد ?", "Rework Items", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {

                if (comboReject.Text == "" || comboQCAction.Text == "" || ComboDepartment.Text == "" || txt_qtysend_cut.Text == "" || comboSV.Text == "" || comboQC.Text == "" || comboQA.Text == "")
                { MessageBox.Show(@"من فضلك ادخل البيانات المطلوبة "); }

                else
                {
                    var ShiftID = trackdb.Shfits.Single(Name => Name.Shift == comboShift.Text);
                    var SVID = trackdb.Logins.Single(Name => Name.Name == comboSV.Text);
                    var QCID = trackdb.Logins.Single(Name => Name.Name == comboQC.Text);
                    var QAID = trackdb.Logins.Single(Name => Name.Name == comboQA.Text);

                    var rejectID = trackdb.QC_rejs.Single(Name => Name.Rej_Name == comboReject.Text);
                    var ActionID = trackdb.QC_Actions.Single(Name => Name.QC_Action1 == comboQCAction.Text);
                    var Department_reject = trackdb.Departments.Single(Name => Name.Department_Name == comboRejdepa.Text);

                    var up = (from rej in trackdb.Tracks
                              join iddd in trackdb.ITEMs on new { rej.OC_ID, rej.Item_ID, rej.Pos } equals new { iddd.OC_ID, iddd.Item_ID, iddd.Pos }
                              join dep in trackdb.Departments on rej.Departmet_ID equals dep.Departmet_ID
                              where rej.Track_ID == int.Parse(txt_track_ID.Text)
                              select rej).SingleOrDefault();

                    if (up.QTY_ToDo < int.Parse(txt_qtysend_cut.Text))
                    { MessageBox.Show(@"الكمية المدخلة أكبر من المطلوب"); }
                    else
                    {
                        //var Department_reject = trackdb.Departments.Single(Name => Name.Department_Name == comboRejdepa.Text);

                        up.QTY_ToDo = up.QTY_ToDo - int.Parse(txt_qtysend_cut.Text);
                        up.QTY_Send = int.Parse(txt_qtysend_cut.Text);
                        ////// send to
                        if (comboQCAction.Text == "Cut Again")
                        {
                            if (up.QTY_Reject == null) { up.QTY_Reject = 0; }
                            up.Send_To = 1;
                            up.QTY_Reject = up.QTY_Reject + int.Parse(txt_qtysend_cut.Text); }

                        else if (comboQCAction.Text == "REWORK")
                        {
                            if (up.QTY_Rework == null) { up.QTY_Rework = 0; }
                            up.Send_To = Department_reject.Departmet_ID;
                            up.QTY_Rework = up.QTY_Rework + int.Parse(txt_qtysend_cut.Text); }

                        ////
                        if (up.IGU != null || up.LG != null)
                        {

                            for (int i = 0; i < DGV_Grind_Detail.Rows.Count; i++)
                            {
                               
                                Track addIGU = new Track();
                                addIGU.OC_ID = up.OC_ID;
                                addIGU.Item_ID = up.Item_ID;
                                addIGU.Pos = int.Parse(DGV_Grind_Detail.Rows[i].Cells[2].Value.ToString());
                                addIGU.Glass_ID = int.Parse(DGV_Grind_Detail.Rows[i].Cells[3].Value.ToString());
                                addIGU.QTY_Recive = up.QTY_Send;
                                addIGU.QTY_ToDo = up.QTY_Send;
                                addIGU.Recived_From = Department_reject.Departmet_ID;

                                /////////

                                addIGU.Shift = ShiftID.ID;
                                addIGU.Recived_From_SV = SVID.ID;
                                addIGU.Recived_From_QC = QCID.ID;
                                addIGU.Recived_From_QA = QAID.ID;
                                addIGU.QC_Reject = rejectID.Rej_ID;
                                addIGU.QC_Action = ActionID.id;

                                ///////////
                                if (comboQCAction.Text == "Cut Again") { addIGU.Balance = true; }
                                addIGU.Shape = up.Shape;
                                addIGU.Step = up.Step;

                                if (comboQCAction.Text == "Cut Again") { addIGU.Departmet_ID = 1; }
                                else if (comboQCAction.Text == "REWORK")
                                {
                                    addIGU.Departmet_ID = Department_reject.Departmet_ID;
                                  //  up.QTY_Recive = up.QTY_Recive - int.Parse(txt_qtysend_cut.Text);
                                }

                                addIGU.Track_ID_Parent = up.Track_ID;


                                trackdb.Tracks.InsertOnSubmit(addIGU);

                                trackdb.SubmitChanges();


                                txt_track_ID.Text = "";
                                txt_oc_cut.Text = "";
                                txt_item_cut.Text = "";
                                txt_w_cut.Text = "";
                                txt_H_cut.Text = "";
                                txt_glaa_cut.Text = "";
                                txt_qtysend_cut.Text = "";

                            }

                        }
                        else
                        {
                            Track add = new Track();
                            add.OC_ID = up.OC_ID;
                            add.Item_ID = up.Item_ID;
                            add.Pos = up.Pos;
                            add.Glass_ID = up.Glass_ID;
                            add.QTY_Recive = up.QTY_Send;
                            add.QTY_ToDo = up.QTY_Send;
                            add.Recived_From = Department_reject.Departmet_ID;

                            /////////
                            var Shift = trackdb.Shfits.Single(Name => Name.Shift == comboShift.Text);
                            var SV = trackdb.Logins.Single(Name => Name.Name == comboSV.Text);
                            var QC = trackdb.Logins.Single(Name => Name.Name == comboQC.Text);
                            var QA = trackdb.Logins.Single(Name => Name.Name == comboQA.Text);

                            var reject = trackdb.QC_rejs.Single(Name => Name.Rej_Name == comboReject.Text);
                            var Action = trackdb.QC_Actions.Single(Name => Name.QC_Action1 == comboQCAction.Text);
                            add.Shift = Shift.ID;
                            add.Recived_From_SV = SV.ID;
                            add.Recived_From_QC = QC.ID;
                            add.Recived_From_QA = QA.ID;
                            add.QC_Reject = reject.Rej_ID;
                            add.QC_Action = Action.id;

                            ///////////
                            if (comboQCAction.Text == "Cut Again") { add.Balance = true; }
                            add.Shape = up.Shape;
                            add.Step = up.Step;

                            if (comboQCAction.Text == "Cut Again") { add.Departmet_ID = 1; }
                            else if (comboQCAction.Text == "REWORK") { add.Departmet_ID = Department_reject.Departmet_ID; }

                            add.Track_ID_Parent = up.Track_ID;


                            trackdb.Tracks.InsertOnSubmit(add);

                            trackdb.SubmitChanges();


                            txt_track_ID.Text = "";
                            txt_oc_cut.Text = "";
                            txt_item_cut.Text = "";
                            txt_w_cut.Text = "";
                            txt_H_cut.Text = "";
                            txt_glaa_cut.Text = "";
                            txt_qtysend_cut.Text = "";
                           
                        }
                    }
                }
                DGV_Grind_Detail.DataSource = "";
                MessageBox.Show(@"The Items will be Rework");
            }




           
        }


            
      
       

        private void btnSelectItem_Click(object sender, EventArgs e)
        {
            string str = txt_glaa_cut.Text;
            if
           (!String.IsNullOrEmpty(str) && Char.IsLetter(str[0]))
            { MessageBox.Show(@"غير مسموح بارسال أكثر من بند"); }

            else
            { 

            DialogResult result = MessageBox.Show("سيتم اعادة العمل على الزجاج المحدد ?", "Rework Items", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {

                    if (comboReject.Text == "" || comboQCAction.Text == "" || ComboDepartment.Text == "" || txt_qtysend_cut.Text == "" || comboSV.Text == "" || comboQC.Text == "" || comboQA.Text == "")
                    { MessageBox.Show(@"من فضلك ادخل البيانات المطلوبة "); }

                    else
                    {
                        var Department_reject = trackdb.Departments.Single(Name => Name.Department_Name == comboRejdepa.Text);
                        for (int i = 0; i < DGV_cut_todo.SelectedRows.Count; i++)
                        {
                            var FullOrder = from up in trackdb.Tracks
                                            join iddd in trackdb.ITEMs on new { up.OC_ID, up.Item_ID, up.Pos } equals new { iddd.OC_ID, iddd.Item_ID, iddd.Pos }
                                            join dep in trackdb.Departments on up.Departmet_ID equals dep.Departmet_ID
                                            where up.Track_ID == int.Parse(DGV_cut_todo.SelectedRows[i].Cells[7].Value.ToString())

                                            select up;

                            foreach (Track up in FullOrder)
                            {


                                up.QTY_Send = up.QTY_ToDo;
                                up.QTY_ToDo = 0;
                                ////// send to
                                if (comboQCAction.Text == "Cut Again") { up.Send_To = 1; }
                                if (comboQCAction.Text == "REWORK") { up.Send_To = Department_reject.Departmet_ID; }

                                ////

                                Track add = new Track();
                                add.OC_ID = up.OC_ID;
                                add.Item_ID = up.Item_ID;
                                add.Pos = up.Pos;
                                add.Glass_ID = up.Glass_ID;
                                add.QTY_Recive = up.QTY_Send;
                                add.QTY_ToDo = up.QTY_Send;
                                add.Recived_From = Department_reject.Departmet_ID;

                                /////////
                                var Shift = trackdb.Shfits.Single(Name => Name.Shift == comboShift.Text);
                                var SV = trackdb.Logins.Single(Name => Name.Name == comboSV.Text);
                                var QC = trackdb.Logins.Single(Name => Name.Name == comboQC.Text);
                                var QA = trackdb.Logins.Single(Name => Name.Name == comboQA.Text);

                                var reject = trackdb.QC_rejs.Single(Name => Name.Rej_Name == comboReject.Text);
                                var Action = trackdb.QC_Actions.Single(Name => Name.QC_Action1 == comboQCAction.Text);
                                add.Shift = Shift.ID;
                                add.Recived_From_SV = SV.ID;
                                add.Recived_From_QC = QC.ID;
                                add.Recived_From_QA = QA.ID;
                                add.QC_Reject = reject.Rej_ID;
                                add.QC_Action = Action.id;

                                ///////////
                                if (comboQCAction.Text == "Cut Again") { add.Balance = true; }
                                add.Shape = up.Shape;
                                add.Step = up.Step;
                                if (comboQCAction.Text == "Cut Again") { add.Departmet_ID = 1; }
                                else if (comboQCAction.Text == "REWORK") { add.Departmet_ID = Department_reject.Departmet_ID; }
                                add.Track_ID_Parent = up.Track_ID;


                                trackdb.Tracks.InsertOnSubmit(add);

                                trackdb.SubmitChanges();


                                txt_track_ID.Text = "";
                                txt_oc_cut.Text = "";
                                txt_item_cut.Text = "";
                                txt_w_cut.Text = "";
                                txt_H_cut.Text = "";
                                txt_glaa_cut.Text = "";
                                txt_qtysend_cut.Text = "";



                            }
                        }

                        MessageBox.Show(@"The Items will be Rework");
                    }
                }
            }
        }

        private void comboRejdepa_SelectedIndexChanged(object sender, EventArgs e)
        {
            var DEPID = trackdb.Departments.Single(Name => Name.Department_Name == comboRejdepa.Text);

            comboSV.DataSource = trackdb.Logins.Where(x => x.Departmet_ID == DEPID.Departmet_ID).ToList();
        }
    }
}
