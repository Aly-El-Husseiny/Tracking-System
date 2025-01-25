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
    public partial class frmTime : Form
    {
        TrackingDataContext trackdb = new TrackingDataContext();
        public frmTime()
        {
            InitializeComponent();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void comboDep_SelectedIndexChanged(object sender, EventArgs e)
        {


            DGV_Sear_details.DataSource = ""; txtSQM.Text = "";
            comboType.SelectedItem = "";
            comboProcess.SelectedItem = "";
            comboMachine.SelectedItem = "";

            var DepID = (from dep in trackdb.Departments where dep.Department_Name == comboDep.Text select new { dep.Departmet_ID }).SingleOrDefault();



            if (DepID.Departmet_ID == 5)
            {
                DGV_Sear_details.DataSource = ""; txtSQM.Text = "";
                comboProcess.SelectedItem = "";

                var Lamin = from pro in trackdb.Tracks
                            join idd in trackdb.Orders on pro.OC_ID equals idd.OC_ID
                            join lg in trackdb.Processes on idd.LG_Type equals lg.ID
                            where pro.Date == dateTimeProd.Value && pro.Recived_From == 5 && (pro.Run_Time_Min == 0 )
                            select lg;
                var LGfilter = Lamin.GroupBy(x => x.Process_Name).Select(y => y.First()).ToList();

                comboProcess.DataSource = LGfilter;
                comboProcess.DisplayMember = "Process_Name";
                comboProcess.ValueMember = "ID";
                txtDEPID.Text = DepID.Departmet_ID.ToString();
                comboProcess.Enabled = true;

            }
            else if (DepID.Departmet_ID == 6)
            {
                DGV_Sear_details.DataSource = ""; txtSQM.Text = "";
                comboProcess.SelectedItem = "";

                var Lamin = from pro in trackdb.Tracks
                            join idd in trackdb.Orders on pro.OC_ID equals idd.OC_ID
                            join lg in trackdb.Processes on idd.IGU_type equals lg.ID
                            where pro.Date == dateTimeProd.Value && pro.Recived_From == 6 && (pro.Run_Time_Min == 0 )
                            select lg;
                var LGfilter = Lamin.GroupBy(x => x.Process_Name).Select(y => y.First()).ToList();

                comboProcess.DataSource = LGfilter;
                comboProcess.DisplayMember = "Process_Name";
                comboProcess.ValueMember = "ID";
                txtDEPID.Text = DepID.Departmet_ID.ToString();
                comboProcess.Enabled = true;

            }

            else if (DepID.Departmet_ID == 61)
            {
                DGV_Sear_details.DataSource = ""; txtSQM.Text = "";
                comboProcess.SelectedItem = "";

                var Lamin = from pro in trackdb.Tracks
                            join idd in trackdb.Orders on pro.OC_ID equals idd.OC_ID
                            join lg in trackdb.Processes on idd.Bonding_Type equals lg.ID
                            where pro.Date == dateTimeProd.Value && pro.Recived_From == 61 && (pro.Run_Time_Min == 0)
                            select lg;
                var LGfilter = Lamin.GroupBy(x => x.Process_Name).Select(y => y.First()).ToList();

                comboProcess.DataSource = LGfilter;
                comboProcess.DisplayMember = "Process_Name";
                comboProcess.ValueMember = "ID";
                txtDEPID.Text = DepID.Departmet_ID.ToString();
                comboProcess.Enabled = true;

            }

            else if (DepID.Departmet_ID == 3)
            {
                DGV_Sear_details.DataSource = ""; txtSQM.Text = "";
                comboType.SelectedItem = "";

                var Lamin = from pro in trackdb.Tracks
                            join idd in trackdb.Orders on pro.OC_ID equals idd.OC_ID
                            join lg in trackdb.Processes on idd.Print_type equals lg.ID
                            where pro.Date == dateTimeProd.Value && pro.Recived_From == 3 && (pro.Run_Time_Min == 0 )
                            select lg;
                var LGfilter = Lamin.GroupBy(x => x.Process_Name).Select(y => y.First()).ToList();

                comboType.DataSource = LGfilter;
                comboType.DisplayMember = "Process_Name";
                comboType.ValueMember = "ID";
                txtDEPID.Text = DepID.Departmet_ID.ToString();
                comboProcess.Enabled = true;

            }
            else
            {
                if (DepID.Departmet_ID == 2 || DepID.Departmet_ID == 4)
                {
                    comboMachine.Enabled = true;
                }
                var glasstype = from pro in trackdb.Tracks
                                join idd in trackdb.GlassTypes on pro.Glass_ID equals idd.Glass_ID
                                where pro.Date == dateTimeProd.Value && pro.Recived_From == DepID.Departmet_ID && (pro.Run_Time_Min == 0 )


                                select idd;
                var filter = glasstype.GroupBy(x => x.Glass_Type).Select(y => y.First()).ToList();

                comboType.DataSource = filter;
                comboType.DisplayMember = "Glass_Type";
                comboType.ValueMember = "Glass_ID";
                txtDEPID.Text = DepID.Departmet_ID.ToString();
                comboType.Enabled = true;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var DepID = (from dep in trackdb.Departments where dep.Department_Name == comboDep.Text select new { dep.Departmet_ID }).SingleOrDefault();
            var GlassID = (from g in trackdb.GlassTypes where g.Glass_Type == comboType.Text select new { g.Glass_ID }).SingleOrDefault();
            decimal timeforsqm = 0;
          
            timeforsqm = Convert.ToDecimal(txtRunTime.Text) / Convert.ToDecimal(txtSQM.Text);
          
            timeforsqm = Math.Round(timeforsqm, 2);
            txtTimeSQM.Text = timeforsqm.ToString();
            for (int i = 0; i < DGV_Sear_details.Rows.Count; i++)


            {
                var UpdateSQM = (from Trk in trackdb.Tracks
                                 join idd in trackdb.GlassTypes on Trk.Glass_ID equals idd.Glass_ID
                                 join iddd in trackdb.ITEMs on new { Trk.OC_ID, Trk.Item_ID, Trk.Pos } equals new { iddd.OC_ID, iddd.Item_ID, iddd.Pos }
                                 where Trk.Track_ID == int.Parse(DGV_Sear_details.Rows[i].Cells[8].Value.ToString())
                                 select new { TotalTime = (Convert.ToDecimal(iddd.Width * iddd.Hieght * Trk.QTY_Recive) / 1000000) * timeforsqm, }).SingleOrDefault();

                var UpdateRunTime = (from Trk in trackdb.Tracks
                                     where Trk.Track_ID == int.Parse(DGV_Sear_details.Rows[i].Cells[8].Value.ToString())
                                     select Trk).SingleOrDefault();
               // if (UpdateSQM.TotalTime < 1) { UpdateRunTime.Run_Time_Min = 1; }
            UpdateRunTime.Run_Time_Min = Convert.ToDecimal(UpdateSQM.TotalTime);

                trackdb.SubmitChanges();
            }
                DGV_Sear_details.DataSource = ""; txtSQM.Text = ""; txtDEPID.Text = ""; txtGlassID.Text = ""; txtRunTime.Text = "";
                comboType.SelectedItem = "";
                comboProcess.SelectedItem = "";
                comboMachine.SelectedItem = "";
                comboDep.SelectedItem = "";
                MessageBox.Show(@"Done");


          

        }

        private void txtRunTime_KeyPress(object sender, KeyPressEventArgs e)
        {
            Utility.onlynumber(e);
        }

        private void comboType_SelectedIndexChanged(object sender, EventArgs e)
        {
            DGV_Sear_details.DataSource = ""; txtSQM.Text = "";


            var DepID = (from dep in trackdb.Departments where dep.Department_Name == comboDep.Text select new { dep.Departmet_ID }).SingleOrDefault();
            var GlassID = (from g in trackdb.GlassTypes where g.Glass_Type == comboType.Text select new { g.Glass_ID }).SingleOrDefault();
            if (GlassID != null)
            {
                txtGlassID.Text = GlassID.Glass_ID.ToString();

                if (DepID.Departmet_ID == 2 || DepID.Departmet_ID == 4)
                {
                    var Total_SQM = from Trk in trackdb.Tracks
                                    join idd in trackdb.GlassTypes on Trk.Glass_ID equals idd.Glass_ID
                                    join iddd in trackdb.ITEMs on new { Trk.OC_ID, Trk.Item_ID, Trk.Pos } equals new { iddd.OC_ID, iddd.Item_ID, iddd.Pos }

                                    where Trk.Date == dateTimeProd.Value && Trk.Run_Time_Min == 0 && Trk.Recived_From == DepID.Departmet_ID
                                  && Trk.Glass_ID == GlassID.Glass_ID && Trk.Furnace == comboType.Text
                                    select new
                                    {
                                        WorkOrder = Trk.OC_ID,
                                        Item = Trk.Item_ID,
                                        Width = iddd.Width,
                                        Height = iddd.Hieght,
                                        Qty_Send = Trk.QTY_Recive,
                                        Total_Item_SQM = (iddd.Width * iddd.Hieght * Trk.QTY_Recive) / 1000000,
                                        Total_Item_LM = ((iddd.Width + iddd.Hieght) * 2 * Trk.QTY_Recive) / 1000,
                                        Machine_TYpe = Trk.Furnace,
                                        ID=Trk.Track_ID,   //8

                                    };

                    DGV_Sear_details.DataSource = Total_SQM;
                }

                else if (DepID.Departmet_ID == 1)
                {
                    var Total_SQM = from Trk in trackdb.Tracks
                                    join idd in trackdb.GlassTypes on Trk.Glass_ID equals idd.Glass_ID
                                    join iddd in trackdb.ITEMs on new { Trk.OC_ID, Trk.Item_ID, Trk.Pos } equals new { iddd.OC_ID, iddd.Item_ID, iddd.Pos }

                                    where Trk.Date == dateTimeProd.Value && Trk.Run_Time_Min == 0 && Trk.Recived_From == DepID.Departmet_ID
                                  && Trk.Glass_ID == GlassID.Glass_ID
                                    select new
                                    {
                                        WorkOrder = Trk.OC_ID,
                                        Item = Trk.Item_ID,
                                        Width = iddd.Width,
                                        Height = iddd.Hieght,
                                        Qty_Send = Trk.QTY_Recive,
                                        Total_Item_SQM = (iddd.Width * iddd.Hieght * Trk.QTY_Recive) / 1000000,
                                        Total_Item_LM = ((iddd.Width + iddd.Hieght) * 2 * Trk.QTY_Recive) / 1000,
                                        Machine_TYpe = Trk.Furnace,
                                        ID = Trk.Track_ID,
                                    };

                    DGV_Sear_details.DataSource = Total_SQM;
                }
                int sumArea_Dep = 1;
                for (int i = 0; i < DGV_Sear_details.Rows.Count; ++i)
                {
                    int qty = Convert.ToInt32(DGV_Sear_details.Rows[i].Cells[4].Value);
                    double w = Convert.ToInt32(DGV_Sear_details.Rows[i].Cells[2].Value);
                    double h = Convert.ToInt32(DGV_Sear_details.Rows[i].Cells[3].Value);
                    double raw1 = (qty * w * h) / 1000000;
                    sumArea_Dep += Convert.ToInt32(raw1);

                }
               // sumArea_Dep = Math.Round(sumArea_Dep, 2);
                txtSQM.Text = sumArea_Dep.ToString();
            }
            else { }
        }

        private void comboMachine_SelectedIndexChanged(object sender, EventArgs e)
        {
            DGV_Sear_details.DataSource = ""; txtSQM.Text = "";


            var DepID = (from dep in trackdb.Departments where dep.Department_Name == comboDep.Text select new { dep.Departmet_ID }).SingleOrDefault();
            var GlassID = (from g in trackdb.GlassTypes where g.Glass_Type == comboType.Text select new { g.Glass_ID }).SingleOrDefault();
            if (GlassID != null)
            {
                txtGlassID.Text = GlassID.Glass_ID.ToString();

                if (DepID.Departmet_ID == 2 || DepID.Departmet_ID == 4)
                {
                    var Total_SQM = from Trk in trackdb.Tracks
                                    join idd in trackdb.GlassTypes on Trk.Glass_ID equals idd.Glass_ID
                                    join iddd in trackdb.ITEMs on new { Trk.OC_ID, Trk.Item_ID, Trk.Pos } equals new { iddd.OC_ID, iddd.Item_ID, iddd.Pos }

                                    where Trk.Date == dateTimeProd.Value && Trk.Run_Time_Min == 0 && Trk.Recived_From == DepID.Departmet_ID
                                  && Trk.Glass_ID == GlassID.Glass_ID && Trk.Furnace== comboMachine.Text
                                    select new
                                    {
                                        WorkOrder = Trk.OC_ID,
                                        Item = Trk.Item_ID,
                                        Width = iddd.Width,
                                        Height = iddd.Hieght,
                                        Qty_Send = Trk.QTY_Recive,
                                        Total_Item_SQM = (iddd.Width * iddd.Hieght * Trk.QTY_Recive) / 1000000,
                                        Total_Item_LM = ((iddd.Width + iddd.Hieght) * 2 * Trk.QTY_Recive) / 1000,
                                        Machine_TYpe = Trk.Furnace,
                                        ID = Trk.Track_ID,

                                    };

                    DGV_Sear_details.DataSource = Total_SQM;
                }

                else
                {
                   
                }
                int sumArea_Dep = 0;
                for (int i = 0; i < DGV_Sear_details.Rows.Count; ++i)
                {
                    int qty = Convert.ToInt32(DGV_Sear_details.Rows[i].Cells[4].Value);
                    double w = Convert.ToDouble(DGV_Sear_details.Rows[i].Cells[2].Value);
                    double h = Convert.ToDouble(DGV_Sear_details.Rows[i].Cells[3].Value);
                    double raw1 = (qty * w * h) / 1000000;
                    sumArea_Dep += Convert.ToInt32(raw1);

                }
               // sumArea_Dep = Math.Round(sumArea_Dep, 2);
                txtSQM.Text = sumArea_Dep.ToString();
            }
            else { }
        }

        private void comboProcess_SelectedIndexChanged(object sender, EventArgs e)
        {
            DGV_Sear_details.DataSource = ""; txtSQM.Text = "";


            var DepID = (from dep in trackdb.Departments where dep.Department_Name == comboDep.Text select new { dep.Departmet_ID }).SingleOrDefault();
            var ProcessID = (from g in trackdb.Processes where g.Process_Name == comboProcess.Text select new { g.ID }).SingleOrDefault();
            if (ProcessID != null)
            {
                txtGlassID.Text = ProcessID.ID.ToString();

                if (DepID.Departmet_ID == 3 )
                {
                    var Total_SQM = from Trk in trackdb.Tracks
                                    join idd in trackdb.Orders on Trk.OC_ID equals idd.OC_ID
                                    join iddd in trackdb.ITEMs on new { Trk.OC_ID, Trk.Item_ID, Trk.Pos } equals new { iddd.OC_ID, iddd.Item_ID, iddd.Pos }

                                    where Trk.Date == dateTimeProd.Value && Trk.Run_Time_Min == 0 && Trk.Recived_From == DepID.Departmet_ID
                                  && idd.Print_type==ProcessID.ID
                                    select new
                                    {
                                        WorkOrder = Trk.OC_ID,
                                        Item = Trk.Item_ID,
                                        Width = iddd.Width,
                                        Height = iddd.Hieght,
                                        Qty_Send = Trk.QTY_Recive,
                                        Total_Item_SQM = (iddd.Width * iddd.Hieght * Trk.QTY_Recive) / 1000000,
                                        Total_Item_LM = ((iddd.Width + iddd.Hieght) * 2 * Trk.QTY_Recive) / 1000,
                                        Machine_TYpe = Trk.Furnace,
                                        ID = Trk.Track_ID,

                                    };

                    DGV_Sear_details.DataSource = Total_SQM;
                }

                else if (DepID.Departmet_ID == 5)
                {
                    var Total_SQM = from Trk in trackdb.Tracks
                                    join idd in trackdb.Orders on Trk.OC_ID equals idd.OC_ID
                                    join iddd in trackdb.ITEMs on new { Trk.OC_ID, Trk.Item_ID, Trk.Pos } equals new { iddd.OC_ID, iddd.Item_ID, iddd.Pos }

                                    where Trk.Date == dateTimeProd.Value && Trk.Run_Time_Min == 0 && Trk.Recived_From == DepID.Departmet_ID
                                  && idd.LG_Type == ProcessID.ID
                                    select new
                                    {
                                        WorkOrder = Trk.OC_ID,
                                        Item = Trk.Item_ID,
                                        Width = iddd.Width,
                                        Height = iddd.Hieght,
                                        Qty_Send = Trk.QTY_Recive,
                                        Total_Item_SQM = (iddd.Width * iddd.Hieght * Trk.QTY_Recive) / 1000000,
                                        Total_Item_LM = ((iddd.Width + iddd.Hieght) * 2 * Trk.QTY_Recive) / 1000,
                                        Machine_TYpe = Trk.Furnace,
                                        ID = Trk.Track_ID,

                                    };

                    DGV_Sear_details.DataSource = Total_SQM;

                }

                else if (DepID.Departmet_ID == 6)
                {
                    var Total_SQM = from Trk in trackdb.Tracks
                                    join idd in trackdb.Orders on Trk.OC_ID equals idd.OC_ID
                                    join iddd in trackdb.ITEMs on new { Trk.OC_ID, Trk.Item_ID, Trk.Pos } equals new { iddd.OC_ID, iddd.Item_ID, iddd.Pos }

                                    where Trk.Date == dateTimeProd.Value && Trk.Run_Time_Min == 0 && Trk.Recived_From == DepID.Departmet_ID
                                  && idd.IGU_type == ProcessID.ID
                                    select new
                                    {
                                        WorkOrder = Trk.OC_ID,
                                        Item = Trk.Item_ID,
                                        Width = iddd.Width,
                                        Height = iddd.Hieght,
                                        Qty_Send = Trk.QTY_Recive,
                                        Total_Item_SQM = (iddd.Width * iddd.Hieght * Trk.QTY_Recive) / 1000000,
                                        Total_Item_LM = ((iddd.Width + iddd.Hieght) * 2 * Trk.QTY_Recive) / 1000,
                                        Machine_TYpe = Trk.Furnace,
                                        ID = Trk.Track_ID,

                                    };

                    DGV_Sear_details.DataSource = Total_SQM;

                }
                else if (DepID.Departmet_ID ==61)
                {
                    var Total_SQM = from Trk in trackdb.Tracks
                                    join idd in trackdb.Orders on Trk.OC_ID equals idd.OC_ID
                                    join iddd in trackdb.ITEMs on new { Trk.OC_ID, Trk.Item_ID, Trk.Pos } equals new { iddd.OC_ID, iddd.Item_ID, iddd.Pos }

                                    where Trk.Date == dateTimeProd.Value && Trk.Run_Time_Min == 0 && Trk.Recived_From == DepID.Departmet_ID
                                  && idd.Bonding_Type == ProcessID.ID
                                    select new
                                    {
                                        WorkOrder = Trk.OC_ID,
                                        Item = Trk.Item_ID,
                                        Width = iddd.Width,
                                        Height = iddd.Hieght,
                                        Qty_Send = Trk.QTY_Recive,
                                        Total_Item_SQM = (iddd.Width * iddd.Hieght * Trk.QTY_Recive) / 1000000,
                                        Total_Item_LM = ((iddd.Width + iddd.Hieght) * 2 * Trk.QTY_Recive) / 1000,
                                        Machine_TYpe = Trk.Furnace,
                                        ID = Trk.Track_ID,

                                    };

                    DGV_Sear_details.DataSource = Total_SQM;

                }

                int sumArea_Dep = 0;
                for (int i = 0; i < DGV_Sear_details.Rows.Count; ++i)
                {
                    int qty = Convert.ToInt32(DGV_Sear_details.Rows[i].Cells[4].Value);
                    double w = Convert.ToDouble(DGV_Sear_details.Rows[i].Cells[2].Value);
                    double h = Convert.ToDouble(DGV_Sear_details.Rows[i].Cells[3].Value);
                    double raw1 = (qty * w * h) / 1000000;
                    sumArea_Dep += Convert.ToInt32(raw1);

                }
               // sumArea_Dep = Math.Round(sumArea_Dep, 2);
                txtSQM.Text = sumArea_Dep.ToString();
            }
            else { }
        }

        private void txtRunTime_DoubleClick(object sender, EventArgs e)
        {
           // double timeforsqm = 0;

           // timeforsqm = (Convert.ToDouble(txtRunTime.Text) / Convert.ToDouble(txtSQM.Text));

           //timeforsqm = Math.Round(timeforsqm, 2);
           // // txtTimeSQM.Text = timeforsqm.ToString();
           // //var decimal = 62.25;
           // var timeSpan = TimeSpan.FromMinutes(timeforsqm);
           // int hh = timeSpan.Hours;
           // int mm = timeSpan.Minutes;
           // txtTimeSQM.Text = hh.ToString();
           // txtMin.Text = mm.ToString();

        }

        private void txtRunTime_TextChanged(object sender, EventArgs e)
        {

        }

        private void frmTime_Load(object sender, EventArgs e)
        {

        }
    }
}

