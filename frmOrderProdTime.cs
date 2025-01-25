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
    public partial class frmOrderProdTime : Form
    {
        TrackingDataContext trackdb = new TrackingDataContext();
        public frmOrderProdTime()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void comboDep_SelectedIndexChanged(object sender, EventArgs e)
        {
            DGV_Sear_details.DataSource = ""; txtSQM.Text = "";
            comboType.DataSource =null;
            comboOrder.DataSource = null;


            // var DepID = (from dep in trackdb.Departments where dep.Department_Name == comboDep.Text select new { dep.Departmet_ID }).SingleOrDefault();
            if (checkBox1.Checked)
            {
                var orderno = from pro in trackdb.Tracks
                              join idd in trackdb.Orders on pro.OC_ID equals idd.OC_ID
                              join DepID in trackdb.Departments on pro.Recived_From equals DepID.Departmet_ID
                              where pro.Date == dateTimeProd.Value && DepID.Department_Name == comboDep.Text//pro.Recived_From == DepID.Departmet_ID
                                && (pro.Run_Time_Min == 0 )
                              select new { pro.OC_ID, };
                var distinctItems = orderno.GroupBy(x => x.OC_ID).Select(y => y.First());
                comboOrder.DataSource = distinctItems;

                comboOrder.DisplayMember = "OC_ID";
            }
            else
            {
                var GlassNO = from pro in trackdb.Tracks
                              join idd in trackdb.GlassTypes on pro.Glass_ID equals idd.Glass_ID
                              join DepID in trackdb.Departments on pro.Recived_From equals DepID.Departmet_ID
                              where pro.Date == dateTimeProd.Value && DepID.Department_Name == comboDep.Text//pro.Recived_From == DepID.Departmet_ID
                                && (pro.Run_Time_Min == 0 )
                              select new { pro.OC_ID, };
                var distinctItems = GlassNO.GroupBy(x => x.OC_ID).Select(y => y.First());
                comboType.DataSource = distinctItems;

                comboType.DisplayMember = "Glass_Name";
            }

        }

        private void comboOrder_SelectedIndexChanged(object sender, EventArgs e)
        {

            var DepID = (from dep in trackdb.Departments where dep.Department_Name == comboDep.Text select new { dep.Departmet_ID }).SingleOrDefault();

            if (DepID.Departmet_ID == 6)
            {
                var GlassType = from pro in trackdb.Tracks
                                join idd in trackdb.GlassTypes on pro.Glass_ID equals idd.Glass_ID
                                where pro.Date == dateTimeProd.Value && pro.Recived_From == DepID.Departmet_ID && pro.OC_ID == int.Parse(comboOrder.Text)
                                && (pro.Run_Time_Min == 0 )
                                select new { pro.IGU, };
                var GassItems = GlassType.GroupBy(x => x.IGU).Select(y => y.First());
                comboType.DataSource = GassItems;
                comboType.DisplayMember = "IGU";
                // comboType.ValueMember = "Glass_ID";
            }

            else if (DepID.Departmet_ID == 5)
            {
                var GlassType = from pro in trackdb.Tracks
                                join idd in trackdb.GlassTypes on pro.Glass_ID equals idd.Glass_ID
                                where pro.Date == dateTimeProd.Value && pro.Recived_From == DepID.Departmet_ID && pro.OC_ID == int.Parse(comboOrder.Text)
                                && (pro.Run_Time_Min == 0 )
                                select new { pro.LG, };
                var GassItems = GlassType.GroupBy(x => x.LG).Select(y => y.First());
                comboType.DataSource = GassItems;
                comboType.DisplayMember = "LG";
            }
            else
            {
                var GlassType = from pro in trackdb.Tracks
                                join idd in trackdb.GlassTypes on pro.Glass_ID equals idd.Glass_ID
                                where pro.Date == dateTimeProd.Value && pro.Recived_From == DepID.Departmet_ID && pro.OC_ID == int.Parse(comboOrder.Text)
                                && (pro.Run_Time_Min == 0 )
                                select new { idd.Glass_Type, };
                var GassItems = GlassType.GroupBy(x => x.Glass_Type).Select(y => y.First());
                comboType.DataSource = GassItems;
                comboType.DisplayMember = "Glass_Type";
            }

        }

        private void comboType_SelectedIndexChanged(object sender, EventArgs e)
        {
            DGV_Sear_details.DataSource = ""; txtSQM.Text = "";


            var DepID = (from dep in trackdb.Departments where dep.Department_Name == comboDep.Text select new { dep.Departmet_ID }).SingleOrDefault();
            var GlassID = (from g in trackdb.GlassTypes where g.Glass_Type == comboType.Text select new { g.Glass_ID }).SingleOrDefault();
            if (checkBox1.Checked)
            {
                //  txtGlassID.Text = GlassID.Glass_ID.ToString();


                var Total_SQM = from Trk in trackdb.Tracks
                                join idd in trackdb.GlassTypes on Trk.Glass_ID equals idd.Glass_ID
                                join iddd in trackdb.ITEMs on new { Trk.OC_ID, Trk.Item_ID, Trk.Pos } equals new { iddd.OC_ID, iddd.Item_ID, iddd.Pos }

                                where Trk.Date == dateTimeProd.Value && Trk.Recived_From == DepID.Departmet_ID
                              && Trk.Glass_ID == GlassID.Glass_ID && Trk.OC_ID == int.Parse(comboOrder.Text)
                                select new
                                {
                                    WorkOrder = Trk.OC_ID,
                                    Item = Trk.Item_ID,
                                    Width = iddd.Width,
                                    Height = iddd.Hieght,
                                    Qty_Send = Trk.QTY_Recive,
                                    Total_Item_SQM = (iddd.Width * iddd.Hieght * Trk.QTY_Recive) / 1000000,
                                    Total_Item_LM = ((iddd.Width + iddd.Hieght) * 2 * Trk.QTY_Recive) / 1000,
                                    Type = idd.Glass_Type,
                                    LG = Trk.LG,
                                    IGU = Trk.IGU,
                                    ID = Trk.Track_ID,   //8

                                };

                DGV_Sear_details.DataSource = Total_SQM;
            }
            else   ///////// without order no.
            {
                var Total_SQM = from Trk in trackdb.Tracks
                                join idd in trackdb.GlassTypes on Trk.Glass_ID equals idd.Glass_ID
                                join iddd in trackdb.ITEMs on new { Trk.OC_ID, Trk.Item_ID, Trk.Pos } equals new { iddd.OC_ID, iddd.Item_ID, iddd.Pos }

                                where Trk.Date == dateTimeProd.Value && Trk.Recived_From == DepID.Departmet_ID
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
                                    Type = idd.Glass_Type,
                                    LG = Trk.LG,
                                    IGU = Trk.IGU,
                                    ID = Trk.Track_ID,   //8

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

        private void frmOrderProdTime_Load(object sender, EventArgs e)
        {
            comboOrder.Enabled = false;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked) { comboOrder.Enabled = true; }
            else { comboOrder.Enabled = false; }
        }
    }
    }
