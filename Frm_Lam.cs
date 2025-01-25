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
    public partial class Frm_Lam : Form

    {
        TrackingDataContext trackdb = new TrackingDataContext();
        public Frm_Lam()
        {
            InitializeComponent();
        }

        private void RefreshLamin()
        {
            DGV_Lamin_todo.DataSource = "";

            var grind = from id in trackdb.Tracks
                        join idd in trackdb.GlassTypes on id.Glass_ID equals idd.Glass_ID
                        join iddd in trackdb.ITEMs on new { id.OC_ID, id.Item_ID, id.Pos } equals new { iddd.OC_ID, iddd.Item_ID, iddd.Pos }
                        join depar in trackdb.Departments on id.Recived_From equals depar.Departmet_ID
                        where id.Departmet_ID == 5 && id.QTY_ToDo > 0

                        select new
                        {

                            WorkOrder = id.OC_ID,
                            Item = id.Item_ID,
                            Width = iddd.Width,
                            Height = iddd.Hieght,
                            GlassType = idd.Glass_Type,
                            QTY_TO_Work = id.QTY_ToDo,
                            POS=id.Pos,
                            Recieved_From = depar.Department_Name,
                            Trak_ID = id.Track_ID,
                            Date = id.Date


                        };

            DGV_Lamin_todo.DataSource = grind;

            trackdb.SubmitChanges();

            DGV_Lamin_todo.Columns[5].DefaultCellStyle.BackColor = Color.Pink;
            for (int i = 0; i < DGV_Lamin_todo.Rows.Count; i++)
            {
                if (i % 2 == 0)
                {
                    DGV_Lamin_todo.Rows[i].DefaultCellStyle.BackColor = Color.LightBlue;
                }
            }

            int sumqty_grind = 0;
            for (int i = 0; i < DGV_Lamin_todo.Rows.Count; ++i)
            {
                sumqty_grind += Convert.ToInt32(DGV_Lamin_todo.Rows[i].Cells[5].Value);

            }
            txt_work_Qty.Text = sumqty_grind.ToString();

            double sumArea_grind = 0;
            for (int i = 0; i < DGV_Lamin_todo.Rows.Count; ++i)
            {
                int qty = Convert.ToInt32(DGV_Lamin_todo.Rows[i].Cells[5].Value);
                double w = Convert.ToDouble(DGV_Lamin_todo.Rows[i].Cells[2].Value);
                double h = Convert.ToDouble(DGV_Lamin_todo.Rows[i].Cells[3].Value);
                double raw1 = (qty * w * h) / 1000000;
                sumArea_grind += Convert.ToDouble(raw1);

            }
            txt_tarea_Lamin.Text = sumArea_grind.ToString();
        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (combo_layer.SelectedItem.ToString())
            {
                case "2":
                    tableGlass1.Visible = true;
                    tableGlass2.Visible = true;
                    tableGlass3.Visible = false;
                    tableGlass4.Visible = false;
                    tableGlass5.Visible = false;
                    tableGlass6.Visible = false;
                    break;

                case "3":
                    tableGlass1.Visible = true;
                    tableGlass2.Visible = true;
                    tableGlass3.Visible = true;
                    tableGlass4.Visible = false;
                    tableGlass5.Visible = false;
                    tableGlass6.Visible = false;
                    break;

                case "4":
                    tableGlass1.Visible = true;
                    tableGlass2.Visible = true;
                    tableGlass3.Visible = true;
                    tableGlass4.Visible = true;
                    tableGlass5.Visible = false;
                    tableGlass6.Visible = false;
                    break;

                case "5":
                    tableGlass1.Visible = true;
                    tableGlass2.Visible = true;
                    tableGlass3.Visible = true;
                    tableGlass4.Visible = true;
                    tableGlass5.Visible = true;
                    tableGlass6.Visible = false;
                    break;

                case "6":
                    tableGlass1.Visible = true;
                    tableGlass2.Visible = true;
                    tableGlass3.Visible = true;
                    tableGlass4.Visible = true;
                    tableGlass5.Visible = true;
                    tableGlass6.Visible = true;
                    break;
            }


        }

        private void Frm_Lam_Load(object sender, EventArgs e)
        {
            comboShift.DataSource = trackdb.Shfits.ToList();
            comboShift.ValueMember = "ID";
            comboShift.DisplayMember = "Shift";
            comboShift.Text = "";

            comboSV.DataSource = trackdb.Logins.Where(x => x.Departmet_ID == 5).ToList();
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





            if (txt_LG.Text=="")
            { btn_Send_Lamin.Enabled = false; }
            combo_layer.SelectedItem = "2";
            
            RefreshLamin();


            
            DGV_Lamin_todo.Columns[5].DefaultCellStyle.BackColor = Color.Pink;

            // Header Color
            DGV_Lamin_todo.ColumnHeadersDefaultCellStyle.BackColor = Color.LightSteelBlue;

            DGV_Lamin_todo.EnableHeadersVisualStyles = false;

            // odd row color
            for (int i = 0; i < DGV_Lamin_todo.Rows.Count; i++)
            {
                if (i % 2 == 0)
                {
                    DGV_Lamin_todo.Rows[i].DefaultCellStyle.BackColor = Color.LightBlue;
                }
            }
            DGV_Lamin_todo.Columns[5].DefaultCellStyle.BackColor = Color.Pink;





        }

        private void btn_data_ok_Click(object sender, EventArgs e)
        {
            

        }

        private void btn_Data_clear_Click(object sender, EventArgs e)
        {
           
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            txtOrderFilter.Text = "";
            RefreshLamin();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
           
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            Frm_Reject newMDIChild = new Frm_Reject();
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


        private void ntb_Send_Click(object sender, EventArgs e)
        {
            // ************update the work to do for this department



            if (txt_qtysend_Lamin.Text == "" )//|| txt_track_ID.Text == txt_track_ID2.Text)
            { MessageBox.Show(@"من فضلك تأكد من البيانات المدخلة"); }
            else
            {

                var work = from up in trackdb.Tracks
                           join iddd in trackdb.ITEMs on new { up.OC_ID, up.Item_ID, up.Pos } equals new { iddd.OC_ID, iddd.Item_ID, iddd.Pos }
                           join dep in trackdb.Departments on up.Departmet_ID equals dep.Departmet_ID
                           where up.Track_ID == int.Parse(txt_track_ID.Text)

                           select up;

                foreach (Track up in work)
                {
                    if (up.QTY_ToDo < int.Parse(txt_qtysend_Lamin.Text))
                    { MessageBox.Show(@"الكمية المدخلة أكبر من المطلوب"); }
                    else
                    {
                        up.QTY_ToDo = up.QTY_ToDo - int.Parse(txt_qtysend_Lamin.Text);
                        up.QTY_Send = int.Parse(txt_qtysend_Lamin.Text);

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
                        var LGType = (from ty in trackdb.Orders where ty.OC_ID == up.OC_ID select new { ty.LG_Type }).SingleOrDefault();
                        var LGcost = (from pco in trackdb.Processes where pco.ID == LGType.LG_Type select new { pco.Pocess_Cost }).SingleOrDefault();
                        /////////////////////////////////////////

                        Track add = new Track();
                        add.OC_ID = up.OC_ID;
                        add.Item_ID = up.Item_ID;
                        add.Pos = up.Pos;
                        add.Glass_ID = up.Glass_ID;
                        add.LG = txt_LG.Text;
                        add.QTY_Recive = int.Parse(txt_qtysend_Lamin.Text);
                        add.QTY_ToDo = int.Parse(txt_qtysend_Lamin.Text);
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
                        add.ProcessType = LGType.LG_Type;
                        add.Standard_Unit_Process_Cost = LGcost.Pocess_Cost;

                        add.Balance = up.Balance;
                        add.Shape = up.Shape;
                        add.Step = up.Step;
                        add.Departmet_ID = up.Send_To;
                        add.Track_ID_Parent = up.Track_ID;

                        trackdb.Tracks.InsertOnSubmit(add);
                        trackdb.SubmitChanges();

                        var Track2 = from up2 in trackdb.Tracks
                                     join iddd in trackdb.ITEMs on new { up2.OC_ID, up2.Item_ID, up2.Pos } equals new { iddd.OC_ID, iddd.Item_ID, iddd.Pos }
                                     join dep in trackdb.Departments on up2.Departmet_ID equals dep.Departmet_ID
                                     where up2.Track_ID == int.Parse(txt_track_ID2.Text)
                                     select up2;
                        foreach (Track up2 in Track2)
                        {
                            if (up2.QTY_ToDo < int.Parse(txt_qtysend_Lamin.Text))
                            { MessageBox.Show(@"الكمية المدخلة أكبر من المطلوب"); }
                            else
                            {
                                up2.QTY_ToDo = up2.QTY_ToDo - int.Parse(txt_qtysend_Lamin.Text);
                                up2.QTY_Send = int.Parse(txt_qtysend_Lamin.Text);


                                up2.Send_To = up.Send_To;
                                //trackdb.SubmitChanges();
                                //RefreshLamin();

                                //ClearTextBoxes();
                                //MessageBox.Show("The Items sent to next department");
                            }

                        }

                        if (txt_track_ID3.Text != "")
                        {
                            var Track3 = from up3 in trackdb.Tracks
                                         join iddd in trackdb.ITEMs on new { up3.OC_ID, up3.Item_ID, up3.Pos } equals new { iddd.OC_ID, iddd.Item_ID, iddd.Pos }
                                         join dep in trackdb.Departments on up3.Departmet_ID equals dep.Departmet_ID
                                         where up3.Track_ID == int.Parse(txt_track_ID3.Text)
                                         select up3;
                            foreach (Track up3 in Track3)
                            {
                                if (up3.QTY_ToDo < int.Parse(txt_qtysend_Lamin.Text))
                                { MessageBox.Show(@"الكمية المدخلة أكبر من المطلوب"); }
                                else
                                {

                                    up3.QTY_ToDo = up3.QTY_ToDo - int.Parse(txt_qtysend_Lamin.Text);
                                    up3.QTY_Send = int.Parse(txt_qtysend_Lamin.Text);


                                    up3.Send_To = up.Send_To;
                                    //trackdb.SubmitChanges();
                                    //RefreshLamin();

                                    //ClearTextBoxes();
                                    //MessageBox.Show("The Items sent to next department");
                                }
                            }
                        }


                        if (txt_track_ID4.Text != "")
                        {
                            var Track4 = from up4 in trackdb.Tracks
                                         join iddd in trackdb.ITEMs on new { up4.OC_ID, up4.Item_ID, up4.Pos } equals new { iddd.OC_ID, iddd.Item_ID, iddd.Pos }
                                         join dep in trackdb.Departments on up4.Departmet_ID equals dep.Departmet_ID
                                         where up4.Track_ID == int.Parse(txt_track_ID4.Text)
                                         select up4;
                            foreach (Track up4 in Track4)
                            {
                                if (up4.QTY_ToDo < int.Parse(txt_qtysend_Lamin.Text))
                                { MessageBox.Show(@"الكمية المدخلة أكبر من المطلوب"); }
                                else
                                {
                                    up4.QTY_ToDo = up4.QTY_ToDo - int.Parse(txt_qtysend_Lamin.Text);
                                    up4.QTY_Send = int.Parse(txt_qtysend_Lamin.Text);


                                    up4.Send_To = up.Send_To;
                                    trackdb.SubmitChanges();
                                    //RefreshLamin();

                                    //ClearTextBoxes();
                                    //MessageBox.Show("The Items sent to next department");
                                }
                            }
                        }

                        if (txt_track_ID5.Text != "")
                        {
                            var Track5 = from up5 in trackdb.Tracks
                                         join iddd in trackdb.ITEMs on new { up5.OC_ID, up5.Item_ID, up5.Pos } equals new { iddd.OC_ID, iddd.Item_ID, iddd.Pos }
                                         join dep in trackdb.Departments on up5.Departmet_ID equals dep.Departmet_ID
                                         where up5.Track_ID == int.Parse(txt_track_ID5.Text)
                                         select up5;
                            foreach (Track up5 in Track5)
                            {
                                if (up5.QTY_ToDo < int.Parse(txt_qtysend_Lamin.Text))
                                { MessageBox.Show(@"الكمية المدخلة أكبر من المطلوب"); }
                                {
                                    up5.QTY_ToDo = up5.QTY_ToDo - int.Parse(txt_qtysend_Lamin.Text);
                                    up5.QTY_Send = int.Parse(txt_qtysend_Lamin.Text);


                                    up5.Send_To = up.Send_To;
                                    //trackdb.SubmitChanges();
                                    //RefreshLamin();

                                    //ClearTextBoxes();
                                    //MessageBox.Show("The Items sent to next department");
                                }
                            }
                        }

                        if (txt_track_ID6.Text != "")
                        {
                            var Track6 = from up6 in trackdb.Tracks
                                         join iddd in trackdb.ITEMs on new { up6.OC_ID, up6.Item_ID, up6.Pos } equals new { iddd.OC_ID, iddd.Item_ID, iddd.Pos }
                                         join dep in trackdb.Departments on up6.Departmet_ID equals dep.Departmet_ID
                                         where up6.Track_ID == int.Parse(txt_track_ID6.Text)
                                         select up6;
                            foreach (Track up6 in Track6)
                            {
                                if (up6.QTY_ToDo < int.Parse(txt_qtysend_Lamin.Text))
                                { MessageBox.Show(@"الكمية المدخلة أكبر من المطلوب"); }
                                else
                                {
                                    up6.QTY_ToDo = up6.QTY_ToDo - int.Parse(txt_qtysend_Lamin.Text);
                                    up6.QTY_Send = int.Parse(txt_qtysend_Lamin.Text);


                                    up6.Send_To = up.Send_To;
                                    //trackdb.SubmitChanges();
                                    //RefreshLamin();

                                    //ClearTextBoxes();
                                    //MessageBox.Show("The Items sent to next department");
                                }
                            }
                        }



                    }
                }


                trackdb.SubmitChanges();
                RefreshLamin();

                ClearTextBoxes();
                MessageBox.Show(@"The Items sent to next department");
            }
        }
            

        private void DGV_lamin_todo_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void DGV_lamin_todo_MouseDoubleClick_1(object sender, MouseEventArgs e)
        {
            if (txt_oc_lamin1.Text == "")
            {
                txt_oc_lamin1.Text = DGV_Lamin_todo.SelectedRows[0].Cells[0].Value.ToString();
                txt_item_lamin1.Text = DGV_Lamin_todo.SelectedRows[0].Cells[1].Value.ToString();
                txt_w_lamin1.Text = DGV_Lamin_todo.SelectedRows[0].Cells[2].Value.ToString();
                txt_H_lamin1.Text = DGV_Lamin_todo.SelectedRows[0].Cells[3].Value.ToString();
                txt_glaa_lamin1.Text = DGV_Lamin_todo.SelectedRows[0].Cells[4].Value.ToString();
                txt_track_ID.Text = DGV_Lamin_todo.SelectedRows[0].Cells[7].Value.ToString();
            }
            else if (txt_oc_lamin1.Text != "" && txt_oc_lamin2.Text == "")
            {
                txt_oc_lamin2.Text = DGV_Lamin_todo.SelectedRows[0].Cells[0].Value.ToString();
                txt_item_lamin2.Text = DGV_Lamin_todo.SelectedRows[0].Cells[1].Value.ToString();
                txt_w_lamin2.Text = DGV_Lamin_todo.SelectedRows[0].Cells[2].Value.ToString();
                txt_H_lamin2.Text = DGV_Lamin_todo.SelectedRows[0].Cells[3].Value.ToString();
                txt_glaa_lamin2.Text = DGV_Lamin_todo.SelectedRows[0].Cells[4].Value.ToString();
                txt_track_ID2.Text = DGV_Lamin_todo.SelectedRows[0].Cells[7].Value.ToString();

            }
            else if (txt_oc_lamin2.Text != "" && txt_oc_lamin3.Text == "")
            {
                txt_oc_lamin3.Text = DGV_Lamin_todo.SelectedRows[0].Cells[0].Value.ToString();
                txt_item_lamin3.Text = DGV_Lamin_todo.SelectedRows[0].Cells[1].Value.ToString();
                txt_w_lamin3.Text = DGV_Lamin_todo.SelectedRows[0].Cells[2].Value.ToString();
                txt_H_lamin3.Text = DGV_Lamin_todo.SelectedRows[0].Cells[3].Value.ToString();
                txt_glaa_lamin3.Text = DGV_Lamin_todo.SelectedRows[0].Cells[4].Value.ToString();
                txt_track_ID3.Text = DGV_Lamin_todo.SelectedRows[0].Cells[7].Value.ToString();

            }
            else if (txt_oc_lamin3.Text != "" && txt_oc_lamin4.Text == "")
            {
                txt_oc_lamin4.Text = DGV_Lamin_todo.SelectedRows[0].Cells[0].Value.ToString();
                txt_item_lamin4.Text = DGV_Lamin_todo.SelectedRows[0].Cells[1].Value.ToString();
                txt_w_lamin4.Text = DGV_Lamin_todo.SelectedRows[0].Cells[2].Value.ToString();
                txt_H_lamin4.Text = DGV_Lamin_todo.SelectedRows[0].Cells[3].Value.ToString();
                txt_glaa_lamin4.Text = DGV_Lamin_todo.SelectedRows[0].Cells[4].Value.ToString();
                txt_track_ID4.Text = DGV_Lamin_todo.SelectedRows[0].Cells[7].Value.ToString();

            }
            else if (txt_oc_lamin4.Text != "" && txt_oc_lamin5.Text == "")
            {
                txt_oc_lamin5.Text = DGV_Lamin_todo.SelectedRows[0].Cells[0].Value.ToString();
                txt_item_lamin5.Text = DGV_Lamin_todo.SelectedRows[0].Cells[1].Value.ToString();
                txt_w_lamin5.Text = DGV_Lamin_todo.SelectedRows[0].Cells[2].Value.ToString();
                txt_H_lamin5.Text = DGV_Lamin_todo.SelectedRows[0].Cells[3].Value.ToString();
                txt_glaa_lamin5.Text = DGV_Lamin_todo.SelectedRows[0].Cells[4].Value.ToString();
                txt_track_ID5.Text = DGV_Lamin_todo.SelectedRows[0].Cells[7].Value.ToString();

            }
            else if (txt_oc_lamin5.Text != "" && txt_oc_lamin6.Text == "")
            {
                txt_oc_lamin6.Text = DGV_Lamin_todo.SelectedRows[0].Cells[0].Value.ToString();
                txt_item_lamin6.Text = DGV_Lamin_todo.SelectedRows[0].Cells[1].Value.ToString();
                txt_w_lamin6.Text = DGV_Lamin_todo.SelectedRows[0].Cells[2].Value.ToString();
                txt_H_lamin6.Text = DGV_Lamin_todo.SelectedRows[0].Cells[3].Value.ToString();
                txt_glaa_lamin6.Text = DGV_Lamin_todo.SelectedRows[0].Cells[4].Value.ToString();
                txt_track_ID6.Text = DGV_Lamin_todo.SelectedRows[0].Cells[7].Value.ToString();

            }
        }

        private void btn_Lamin_Click(object sender, EventArgs e)
        {

            if (txt_oc_lamin1.Text == "")
            {
                txt_LG.Text = "Can't Laminate";
            }
            else if (txt_oc_lamin1.Text != "" && txt_oc_lamin2.Text == "")
            {
                txt_LG.Text = "Can't Laminate";
            }
            else if (txt_oc_lamin2.Text != "" && txt_oc_lamin3.Text == "" && txt_item_lamin1.Text == txt_item_lamin2.Text)
            {
                txt_LG.Text = "LG( " + txt_glaa_lamin1.Text + " + " + txt_glaa_lamin2.Text + " )";
            }
            else if (txt_oc_lamin3.Text != "" && txt_oc_lamin4.Text == "" && txt_item_lamin1.Text == txt_item_lamin2.Text && txt_item_lamin2.Text == txt_item_lamin3.Text)
            {
                txt_LG.Text = "LG( " + txt_glaa_lamin1.Text + " + " + txt_glaa_lamin2.Text + " + " + txt_glaa_lamin3.Text + " )";
            }
            else if (txt_oc_lamin4.Text != "" && txt_oc_lamin5.Text == "" && txt_item_lamin1.Text == txt_item_lamin2.Text && txt_item_lamin2.Text == txt_item_lamin3.Text && txt_item_lamin3.Text == txt_item_lamin4.Text)
            {
                txt_LG.Text = "LG( " + txt_glaa_lamin1.Text + " + " + txt_glaa_lamin2.Text + " + " + txt_glaa_lamin3.Text + " + " + txt_glaa_lamin4.Text + " + " + " )";
            }
            else if (txt_oc_lamin5.Text != "" && txt_oc_lamin6.Text == "")
            {
                txt_LG.Text = "LG( " + txt_glaa_lamin1.Text + " + " + txt_glaa_lamin2.Text + " + " + txt_glaa_lamin3.Text + " + " + txt_glaa_lamin4.Text + " + " + txt_glaa_lamin5.Text + " + " + " )";
            }
            else if (txt_oc_lamin6.Text != "")
            { txt_LG.Text = "LG( " + txt_glaa_lamin1.Text + " + " + txt_glaa_lamin2.Text + " + " + txt_glaa_lamin3.Text + " + " + txt_glaa_lamin4.Text + " + " + txt_glaa_lamin5.Text + " + " + txt_glaa_lamin6.Text + " )"; }
            else
            { MessageBox.Show(@"Worng Selected Item"); }

            btn_Send_Lamin.Enabled = true;

        }

        private void ClearTextBoxes()
        {
            Action<Control.ControlCollection> func = null;

            func = (controls) =>
            {
                foreach (Control control in controls)
                    if (control is TextBox )
                        (control as TextBox).Clear();
                    else
                        func(control.Controls);
            };

            func(Controls);
        }

        private void btn_Clear_Click(object sender, EventArgs e)
        {
            ClearTextBoxes();
            btn_Send_Lamin.Enabled = false;
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            FrmBarCode newMDIChild = new FrmBarCode();
            newMDIChild.MdiParent = this.MdiParent;
            newMDIChild.Show();
        }

        private void btn_data_ok_Click_1(object sender, EventArgs e)
        {
           
        }

        private void btnclear_Click(object sender, EventArgs e)
        {
        }

        private void btnFinishLamin_Click(object sender, EventArgs e)
        {
            var track = from id in trackdb.Tracks
                        //join idd in trackdb.GlassTypes on id.Glass_ID equals idd.Glass_ID
                        join iddd in trackdb.ITEMs on new { id.OC_ID, id.Item_ID, id.Pos } equals new { iddd.OC_ID, iddd.Item_ID, iddd.Pos }
                        join idDEP in trackdb.Departments on id.Departmet_ID equals idDEP.Departmet_ID
                        where (id.Date <= dateToLamin.Value && id.Date >= datefromLamin.Value) && id.Recived_From == 5
                        select new
                        {

                            WorkOrder = id.OC_ID,
                            Item = id.Item_ID,
                            Width = iddd.Width,
                            Height = iddd.Hieght,
                            GlassType = id.LG,
                            Qty_Send = id.QTY_Recive,
                            Send_To = idDEP.Department_Name,
                            Balance = id.Balance,
                            Trak_ID = id.Track_ID
                        };
            DGV_Lamin_Detail.DataSource = track;

            for (int i = 0; i < DGV_Lamin_Detail.Rows.Count; i++)
            {
                if (i % 2 == 0)
                {
                    DGV_Lamin_Detail.Rows[i].DefaultCellStyle.BackColor = Color.LightBlue;
                }
            }
        }

        private void copyAlltoClipboard()
        {

            DGV_Lamin_Detail.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            DGV_Lamin_Detail.MultiSelect = true;
            DGV_Lamin_Detail.SelectAll();
            DataObject dataObj = DGV_Lamin_Detail.GetClipboardContent();
            if (dataObj != null)
                Clipboard.SetDataObject(dataObj);
        }
        private void btnExcelLamin_Click(object sender, EventArgs e)
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

        private void txt_qtysend_Lamin_KeyPress(object sender, KeyPressEventArgs e)
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
            { MessageBox.Show(@"Please enter the Order No."); }
            else
            {

                if (e.KeyCode == Keys.Enter)
                {
                    var filter = from fil in trackdb.Tracks
                                 join idd in trackdb.GlassTypes on fil.Glass_ID equals idd.Glass_ID
                                 join iddd in trackdb.ITEMs on new { fil.OC_ID, fil.Item_ID, fil.Pos } equals new { iddd.OC_ID, iddd.Item_ID, iddd.Pos }
                                 join depar in trackdb.Departments on fil.Recived_From equals depar.Departmet_ID
                                 where fil.Departmet_ID == 5 && fil.QTY_ToDo > 0 && fil.OC_ID == int.Parse(txtOrderFilter.Text)

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

                    DGV_Lamin_todo.DataSource = filter;
                }
            }
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            btncleardep.PerformClick();

            if (comboShift.Text == "")
            { MessageBox.Show(@"من فضلك ادخل البيانات المطلوبة "); }

            else
            {
                var Shift = trackdb.Shfits.Single(Name => Name.Shift == comboShift.Text);


                var check = (from ch in trackdb.DepDatas
                             where ch.Date == dateCut.Value && ch.Department_ID == 5 && ch.Shift_ID == Shift.ID
                             select ch).SingleOrDefault();
                if (check == null)
                {
                    MessageBox.Show(@"من فضلك ادخل البيانات المطلوبة ");
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

                var check = (from ch in trackdb.DepDatas
                             where ch.Date == dateCut.Value && ch.Department_ID ==5 && ch.Shift_ID == Shift.ID
                             select ch).SingleOrDefault();
                if (check == null)
                { // add.Grind_Type = comboGrindProcess.Text.Length == 0 ? 0 : Grindprocess.ID;
                    DepData add = new DepData();
                    add.Date = dateCut.Value;
                    add.Department_ID = 5;
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


                    trackdb.DepDatas.InsertOnSubmit(add);
                    trackdb.SubmitChanges();
                    btncleardep.PerformClick();
                }
                else
                {

                    check.Date = DateTime.Today;
                    check.Department_ID = 5;
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
    

}
