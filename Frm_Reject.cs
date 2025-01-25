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
    public partial class Frm_Reject : Form
    {
       
        TrackingDataContext trackdb = new TrackingDataContext();
       

        public Frm_Reject()
        {
            InitializeComponent();
        }

        private void comboRej_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void Frm_Reject_Load(object sender, EventArgs e)
        {
            tablerej.Enabled = false;
            tabletrackID.Enabled = false;
            toolStripBtnclear.Enabled = false;
            toolStripBtnsend.Enabled = false;

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            tablerej.Enabled = true;
            tabletrackID.Enabled = true;
            toolStripBtnclear.Enabled = true;
            toolStripBtnsend.Enabled = true;

            ////txt_dep_rej.Text = "Grinding";
            //Cutting.TrackingDataContext comrej = new TrackingDataContext();
            //combo_Rej_rej.DataSource = comrej.QC_rejs.ToList();
            //combo_Rej_rej.ValueMember = "Rej_ID";
            //combo_Rej_rej.DisplayMember = "Rej_Name";
            //combo_Rej_rej.Text = "";

            comboShift.DataSource = trackdb.Shfits.ToList();
            comboShift.ValueMember = "ID";
            comboShift.DisplayMember = "Shift";
            comboShift.Text = "";

            comboSV.DataSource = trackdb.Logins.Where(x => x.Departmet_ID != 9 && x.Departmet_ID != 10).ToList();
            comboSV.ValueMember = "ID";
            comboSV.DisplayMember = "Name";
            comboSV.Text = "";

            comboQC.DataSource = trackdb.Logins.Where(x=>x.Departmet_ID==9).ToList();
            comboQC.ValueMember = "ID";
            comboQC.DisplayMember = "Name";
            comboQC.Text = "";

            comboQA.DataSource = trackdb.Logins.Where(x => x.Departmet_ID == 10).ToList();
            comboQA.ValueMember = "ID";
            comboQA.DisplayMember = "Name";
            comboQA.Text = "";

        }

        private void toolStripBtnsend_Click(object sender, EventArgs e)
        {

            
            //update track table***************
            var reject = from up in trackdb.Tracks
                         join iddd in trackdb.ITEMs on new { up.OC_ID, up.Item_ID,up.Pos } equals new { iddd.OC_ID, iddd.Item_ID,iddd.Pos }
                         join dep in trackdb.Departments on up.Departmet_ID equals dep.Departmet_ID
                         where up.Track_ID == int.Parse(txt_rej_trakID.Text)

                         select up;

            foreach (Track up in reject)
            {
                if (txt_QTY_rej.Text == "" || txt_reason_rej.Text == "" || combo_Rej_rej.Text == "" 
                    || txt_QTY_rej.Text == "" || int.Parse(txt_QTY_rej.Text)> up.QTY_ToDo || comboQC.Text=="" || comboSV.Text == "" || comboShift.Text == "")
                {
                    MessageBox.Show(@"Please complete the missing data or Check the QTY");

                }
                else
                {
                    up.QTY_ToDo = up.QTY_ToDo - int.Parse(txt_QTY_rej.Text);
                    trackdb.SubmitChanges();

                    var DepName = trackdb.Departments.Single(Name => Name.Department_Name == txt_dep_rej.Text);
                    var RejectID = trackdb.QC_rejs.Single(Name => Name.Rej_Name == combo_Rej_rej.Text);

                    var Shift = trackdb.Shfits.Single(Name => Name.Shift == comboShift.Text);
                    var SV = trackdb.Logins.Single(Name => Name.Name == comboSV.Text);
                    var QC = trackdb.Logins.Single(Name => Name.Name == comboQC.Text);
                    var QA = trackdb.Logins.Single(Name => Name.Name == comboQA.Text);


                    QC add = new QC();

                    if (up.LG != null)
                    {
                       add.Glass_Desc = up.LG.ToString();
                    }
                    else if (up.IGU != null)
                    {
                        add.Glass_Desc = up.IGU.ToString();
                    }
                    else  // for Single Glass Only
                    {
                        var Glass = trackdb.GlassTypes.Single(Name => Name.Glass_Type == txt_glass_rej.Text);
                        add.Glass_ID = Glass.Glass_ID;
                    }
                   // TimeSpan timeOfDay = dt.TimeOfDay;
                    add.OC_ID = (txt_Order_rej.Text);
                    add.Item = int.Parse(txt_Item_rej.Text);
                    add.Pos = up.Pos;
                    add.Width = int.Parse(txt_width_rej.Text);
                    add.Height = int.Parse(txt_Height_rej.Text);
                    add.QTY = int.Parse(txt_QTY_rej.Text);
                    add.Rej_Reason = txt_reason_rej.Text;
                    add.Reject_Date = DateTime.Today;
                    add.Reject_Time =DateTime.Now.TimeOfDay;//TimeSpan timeOfDay = dt.TimeOfDay;
                    add.Rej_ID= RejectID.Rej_ID;
                    add.Departmet_ID = DepName.Departmet_ID;
                    add.Project = txt_project.Text;
                    add.Track_ID = up.Track_ID;
                    add.Shift = Shift.ID;
                    add.Recived_From_SV = SV.ID;
                    add.Recived_From_QC = QC.ID;
                    add.Recived_From_QA = QA.ID;



                    trackdb.QCs.InsertOnSubmit(add);
                    trackdb.SubmitChanges();
                    MessageBox.Show(@"The Reject case was sent");

                    txt_rej_trakID.Text = "";
                    txt_dep_rej.Text = "";
                    txt_Order_rej.Text = "";
                    txt_Item_rej.Text = "";
                    txt_Height_rej.Text = "";
                    txt_width_rej.Text = "";
                    txt_QTY_rej.Text = "";
                    txt_reason_rej.Text = "";
                    txt_glass_rej.Text = "";
                    combo_Rej_rej.Text = "";
                    txt_project.Text = "";
                    comboQA.Text = "";
                    comboQC.Text = "";
                    comboSV.Text = "";
                    comboShift.Text = "";
                }
            }
        }

                        
                    
       

        private void button1_Click(object sender, EventArgs e)
        {
            var fill = (from up in trackdb.Tracks
                       // join idd in trackdb.GlassTypes on up.Glass_ID equals idd.Glass_ID
                        join iddd in trackdb.ITEMs on new { up.OC_ID, up.Item_ID ,up.Pos} equals new { iddd.OC_ID, iddd.Item_ID,iddd.Pos }
                        join depar in trackdb.Departments on up.Departmet_ID equals depar.Departmet_ID
                        join orderProj in trackdb.Orders on up.OC_ID equals orderProj.OC_ID
                        where up.Track_ID == int.Parse(txt_rej_trakID.Text)
                        select new
                        {
                            depar.Department_Name,
                            up.OC_ID,
                            up.Item_ID,
                            //idd.Glass_Type,
                            up.LG,
                            up.IGU,
                            iddd.Width,
                            iddd.Hieght,
                            orderProj.Project_Name,
                            up.Shift,
                            up.Recived_From_SV,
                            up.Recived_From_QC,
                            up.Recived_From_QA,
                        }).SingleOrDefault();
            
            {
               txt_dep_rej.Text = fill.Department_Name.ToString();
                txt_Order_rej.Text = fill.OC_ID.ToString();
                txt_Item_rej.Text = fill.Item_ID.ToString();
                txt_width_rej.Text = fill.Width.ToString();
                txt_Height_rej.Text = fill.Hieght.ToString();
                txt_project.Text = fill.Project_Name.ToString();
                
                if (fill.Shift != null) { var Shift = trackdb.Shfits.Single(Name => Name.ID == fill.Shift); comboShift.Text = Shift.Shift.ToString(); }
                if (fill.Recived_From_SV != null) { var SV = trackdb.Logins.Single(Name => Name.ID == fill.Recived_From_SV); comboSV.Text = SV.Name.ToString(); }
                if (fill.Recived_From_QC != null) { var SVQC = trackdb.Logins.Single(Name => Name.ID == fill.Recived_From_QC); comboQC.Text = SVQC.Name.ToString(); }
                if (fill.Recived_From_QA != null) { var SVQA = trackdb.Logins.Single(Name => Name.ID == fill.Recived_From_QA); comboQA.Text = SVQA.Name.ToString(); }


                if (fill.LG != null)
                {
                    txt_glass_rej.Text = fill.LG.ToString();
                }
                else if (fill.IGU != null)
                {
                    txt_glass_rej.Text = fill.IGU.ToString();
                }
                else  // for Single Glass Only
                {
                    var fillSG = (from up in trackdb.Tracks
                                     join idd in trackdb.GlassTypes on up.Glass_ID equals idd.Glass_ID
                                //join iddd in trackdb.ITEMs on new { up.OC_ID, up.Item_ID, up.Pos } equals new { iddd.OC_ID, iddd.Item_ID, iddd.Pos }
                               // join depar in trackdb.Departments on up.Departmet_ID equals depar.Departmet_ID
                                //join orderProj in trackdb.Orders on up.OC_ID equals orderProj.OC_ID
                                where up.Track_ID == int.Parse(txt_rej_trakID.Text)
                                select new
                                {
                                    //depar.Department_Name,
                                    //up.OC_ID,
                                    //up.Item_ID,
                                    idd.Glass_Type,
                                    //up.LG,
                                    //up.IGU,
                                    //iddd.Width,
                                    //iddd.Hieght,
                                    //orderProj.Project_Name
                                }).SingleOrDefault();

                    {
                        //txt_dep_rej.Text = fillSG.Department_Name.ToString();
                        //txt_Order_rej.Text = fillSG.OC_ID.ToString();
                        //txt_Item_rej.Text = fillSG.Item_ID.ToString();
                        //txt_width_rej.Text = fillSG.Width.ToString();
                        //txt_Height_rej.Text = fillSG.Hieght.ToString();
                        //txt_project.Text = fillSG.Project_Name.ToString();
                        txt_glass_rej.Text = fillSG.Glass_Type.ToString();
                    };


                    }
                  

               



            };


        }

        private void toolStripBtnclear_Click(object sender, EventArgs e)
        {
            txt_Order_rej.Text = "";
            txt_Item_rej.Text = "";
            txt_Height_rej.Text = "";
            txt_width_rej.Text = "";
            txt_QTY_rej.Text = "";
            txt_reason_rej.Text = "";
            //txt_glass_rej.Text = "";
            combo_Rej_rej.Text = "";
            comboQA.Text = "";
            comboQC.Text = "";
            comboSV.Text = "";
            comboShift.Text = "";

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void txt_rej_trakID_KeyPress(object sender, KeyPressEventArgs e)
        {
            Utility.onlynumber(e);
        }

        private void txt_QTY_rej_KeyPress(object sender, KeyPressEventArgs e)
        {
            Utility.onlynumber(e);
        }
    }
}