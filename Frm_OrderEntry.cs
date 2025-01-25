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
    public partial class Frm_OrderEntry : Form

    {

        SMARTDBDataContext Alfak = new SMARTDBDataContext();
        TrackingDataContext orderenter = new TrackingDataContext();
        AlcimDBDataContext Alcim = new AlcimDBDataContext();

        public Frm_OrderEntry()
        {
            InitializeComponent();
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }


        private void button2_Click(object sender, EventArgs e)
        {

            //var ord = from oc in orderenter.Orders select oc;
            //foreach (var od in ord)
            //{ 
            // if (od.LG_Type == 0 && od.IGU_type == 0 && od.Bonding_Type != 0) { od.Category = 4; }
            //else if (od.LG_Type != 0 && od.IGU_type == 0 && od.Bonding_Type != 0) { od.Category = 5; }
            //    else if (od.LG_Type == 0 && od.IGU_type != 0 && od.Bonding_Type != 0) { od.Category = 6; }
            //    else if (od.LG_Type == 0 && od.IGU_type != 0 && od.Bonding_Type == 0) { od.Category = 3; }
            //    else if (od.LG_Type != 0 && od.IGU_type != 0 && od.Bonding_Type == 0) { od.Category = 7; }
            //    else if (od.LG_Type != 0 && od.IGU_type == 0 && od.Bonding_Type == 0) { od.Category = 2; }
            //    else if (od.LG_Type == 0 && od.IGU_type == 0 && od.Bonding_Type == 0) { od.Category = 1; }
            //}
            //orderenter.SubmitChanges();

            txt_Customer.Text = "";
            txt_FullDesc.Text = "";
            txt_OD_ID.Text = "";
            txt_Project.Text = "";
            txt_TQTY.Text = "";
            txt_TSQM.Text = "";
            txt_TLM.Text = "";
            txt_TWeight.Text = "";
            txtBtach.Text = "";
            DGV_Item.DataSource = "";

        }

        private void btn_AW_Click(object sender, EventArgs e)
        {


            var recive = (from rec in Alfak.BW_AUFTR_KOPFs
                          join itemdata in Alfak.BW_AUFTR_POs on rec.ID equals itemdata.ID
                          // join txt in Alfak.BW_AUFTR_KTXTs on rec.ID equals txt.ID
                          where rec.ID == int.Parse(txt_OD_ID.Text) //&& itemdata.POS_NR == 1       // for item NO 1
                          select new
                          {
                              rec.AH_MCODE,      // client
                              TQTY = Convert.ToInt32(rec.SU_STUECK),     //TQTY
                              rec.SU_LFM_FAKT,    //TLM
                              rec.SU_QM_FAKT,     //TSQM
                              rec.AH_NAME2,       // Project
                              rec.SU_GEWICHT,
                              alfakdate = rec.DATUM_ERF,     //Date
                                                             // txt.BEZ,
                              itemdata.PROD_BEZ1,
                              itemdata.PROD_BEZ2,
                              itemdata.PROD_BEZ3,
                          }).FirstOrDefault();
            {
                if (recive == null)
                { MessageBox.Show(@"Try again"); }
                else
                {
                    txt_Customer.Text = recive.AH_MCODE.ToString();
                    txt_Project.Text = recive.AH_NAME2.ToString();
                    txt_TLM.Text = recive.SU_LFM_FAKT.ToString();
                    txt_TQTY.Text = recive.TQTY.ToString();
                    txt_TSQM.Text = recive.SU_QM_FAKT.ToString();
                    txt_TWeight.Text = recive.SU_GEWICHT.ToString();

                    dateALFAK.Value = recive.alfakdate;
                    // txtTXT.Text = recive.BEZ.;

                    txt_FullDesc.Text = recive.PROD_BEZ1.ToString() + recive.PROD_BEZ2.ToString() + recive.PROD_BEZ3.ToString();
                }
            };


            var ItemDGV = from itemID in Alfak.BW_AUFTR_STKLs
                          join ItNo in Alfak.BW_AUFTR_POs on new { itemID.ID, itemID.POS_NR } equals new { ItNo.ID, ItNo.POS_NR }
                          where itemID.ID == int.Parse(txt_OD_ID.Text) && itemID.STL_WGR[0] == '1' //&& itemID.PRODUCTION_DATE !=null
                          select new
                          {
                              Order_No = itemID.ID,
                              Item_No = itemID.POS_NR,
                              Width_mm = Convert.ToInt32(itemID.STL_BREITE),
                              Hight_mm = Convert.ToInt32(itemID.STL_HOEHE),
                              //Item_qty = Convert.ToInt32(itemID.STL_MENGE),
                              Item_qty = Convert.ToInt32(ItNo.PP_ORIG_MENGE),
                              Glass_ID = itemID.BOM_PRODUKT,
                              Pos = itemID.BOM_PUID,
                              GLass_Name = itemID.STL_BEZ,



                          };
            DGV_Item.DataSource = ItemDGV;
            if (DGV_Item.Rows.Count == 0)
            {
                DGV_Item.DataSource = "";
                var ItemAnn = from ann in Alfak.BW_AUFTR_POs
                              where ann.ID == int.Parse(txt_OD_ID.Text) && ann.PROD_WGR[0] == '1'
                              select new
                              {
                                  Order_No = ann.ID,
                                  Item_No = ann.POS_NR,
                                  Width_mm = Convert.ToInt32(ann.PP_BREITE),
                                  Hight_mm = Convert.ToInt32(ann.PP_HOEHE),
                                  Item_qty = Convert.ToInt32(ann.PP_ORIG_MENGE),
                                  Glass_ID = ann.PROD_ID,
                                  Pos = 1,
                                  GLass_Name = ann.PROD_BEZ1,
                              };
                DGV_Item.DataSource = ItemAnn;
            }
            btn_Save.Enabled = true;

            btn_Process.Enabled = true;


        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            if (txtBtach.Text == "") { MessageBox.Show(@"PLease enter Batch No."); }
            else
            {
                var Grindprocess = (from gr in orderenter.Processes where gr.Process_Name == comboGrindProcess.Text select new { gr.ID }).SingleOrDefault();
                var Printprocess = (from pr in orderenter.Processes where pr.Process_Name == comboPrintProcess.Text select new { pr.ID }).SingleOrDefault();
                var LGtprocess = (from lg in orderenter.Processes where lg.Process_Name == comboLGProcess.Text select new { lg.ID }).SingleOrDefault();
                var IGUtprocess = (from ig in orderenter.Processes where ig.Process_Name == comboIGUProcess.Text select new { ig.ID }).SingleOrDefault();
                var Bondingtprocess = (from bo in orderenter.Processes where bo.Process_Name == comboBondingProcess.Text select new { bo.ID }).SingleOrDefault();

                var OrderCheck = (from ser in orderenter.Orders where (ser.OC_ID == int.Parse(txt_OD_ID.Text)) select ser).SingleOrDefault();
                if (OrderCheck != null)
                {
                    OrderCheck.Grind_Type = comboGrindProcess.Text.Length == 0 ? 0 : Grindprocess.ID;
                    OrderCheck.Print_type = comboPrintProcess.Text.Length == 0 ? 0 : Printprocess.ID;
                    OrderCheck.LG_Type = comboLGProcess.Text.Length == 0 ? 0 : LGtprocess.ID;
                    OrderCheck.IGU_type = comboIGUProcess.Text.Length == 0 ? 0 : IGUtprocess.ID;
                    OrderCheck.Bonding_Type = comboBondingProcess.Text.Length == 0 ? 0 : Bondingtprocess.ID;
                    OrderCheck.Batch = int.Parse(txtBtach.Text);
                    orderenter.SubmitChanges();
                    MessageBox.Show(@"The Order Process saved");
                }


                else
                {
                    Order add = new Order();
                    add.OC_ID = int.Parse(txt_OD_ID.Text);
                    add.Clinet_Name = txt_Customer.Text;
                    add.Project_Name = txt_Project.Text;
                    add.Descreption = txt_FullDesc.Text;
                    add.TQTY = int.Parse(txt_TQTY.Text);
                    add.TSQM = decimal.Parse(txt_TSQM.Text);
                    add.TLM = decimal.Parse(txt_TLM.Text);

                    add.TQTY_Delivered = 0;
                    add.TSQM_Delivered = 0;
                    add.TLM_Delivered = 0;
                  //  add.Total_Invoice = 0;

                    add.Batch = int.Parse(txtBtach.Text);
                    add.ALFAK_Date = dateALFAK.Value;
                    add.Status = "Waiting";

                    add.Grind_Type = comboGrindProcess.Text.Length == 0 ? 0 : Grindprocess.ID;
                    add.Print_type = comboPrintProcess.Text.Length == 0 ? 0 : Printprocess.ID;
                    add.LG_Type = comboLGProcess.Text.Length == 0 ? 0 : LGtprocess.ID;
                    add.IGU_type = comboIGUProcess.Text.Length == 0 ? 0 : IGUtprocess.ID;
                    add.Bonding_Type = comboBondingProcess.Text.Length == 0 ? 0 : Bondingtprocess.ID;
                    add.Category = 8;
                    if (checkBalance.Checked)
                    { add.Balance = true; }
                    orderenter.Orders.InsertOnSubmit(add);
                    //orderenter.SubmitChanges();


                    for (int i = 0; i < DGV_Item.Rows.Count; i++)
                    {
                        ITEM additem = new ITEM();
                        additem.OC_ID = int.Parse(DGV_Item.Rows[i].Cells[0].Value.ToString());
                        additem.Item_ID = int.Parse(DGV_Item.Rows[i].Cells[1].Value.ToString());
                        additem.Width = float.Parse(DGV_Item.Rows[i].Cells[2].Value.ToString());
                        additem.Hieght = float.Parse(DGV_Item.Rows[i].Cells[3].Value.ToString());
                        additem.QTY_Item = int.Parse(DGV_Item.Rows[i].Cells[4].Value.ToString());
                        additem.Glass_ID = int.Parse(DGV_Item.Rows[i].Cells[5].Value.ToString());
                        additem.Pos = int.Parse(DGV_Item.Rows[i].Cells[6].Value.ToString());


                        orderenter.ITEMs.InsertOnSubmit(additem);

                    }
                    orderenter.SubmitChanges();
                    var ord = from oc in orderenter.Orders where oc.OC_ID==int.Parse(txt_OD_ID.Text)
                              select oc;
                    foreach (var od in ord)
                    {
                        if (od.LG_Type == 0 && od.IGU_type == 0 && od.Bonding_Type != 0) { od.Category = 4; }
                        else if (od.LG_Type != 0 && od.IGU_type == 0 && od.Bonding_Type != 0) { od.Category = 5; }
                        else if (od.LG_Type == 0 && od.IGU_type != 0 && od.Bonding_Type != 0) { od.Category = 6; }
                        else if (od.LG_Type == 0 && od.IGU_type != 0 && od.Bonding_Type == 0) { od.Category = 3; }
                        else if (od.LG_Type != 0 && od.IGU_type != 0 && od.Bonding_Type == 0) { od.Category = 7; }
                        else if (od.LG_Type != 0 && od.IGU_type == 0 && od.Bonding_Type == 0) { od.Category = 2; }
                        else if (od.LG_Type == 0 && od.IGU_type == 0 && od.Bonding_Type == 0) { od.Category = 1; }
                    }
                    orderenter.SubmitChanges();


                    MessageBox.Show(@"The Order saved");

                    btn_Save.Enabled = false;

                    btn_AW.Enabled = false;
                    button3.PerformClick();
                }
            }
        }

        private void OrderEntry_Load(object sender, EventArgs e)
        {
            panel1.Enabled = false;
            panel2.Enabled = false;


            btn_AW.Enabled = false;
            btn_Save.Enabled = false;
            btn_Process.Enabled = false;
            button3.Enabled = false;


            comboStep2.Enabled = false; comboStep3.Enabled = false;
            comboStep3.Enabled = false; comboStep4.Enabled = false;
            comboStep5.Enabled = false; comboStep6.Enabled = false;
            comboStep7.Enabled = false; comboStep8.Enabled = false;

            comboStep1.Text = ""; comboStep2.Text = ""; comboStep3.Text = ""; comboStep4.Text = "";
            comboStep5.Text = ""; comboStep6.Text = ""; comboStep7.Text = ""; comboStep8.Text = "";


        }

        public static void onlynumber(KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!char.IsDigit(ch) && ch != 8 && ch != 46)
            {
                e.Handled = true;

            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (txt_OD_ID.Text == "")
            { MessageBox.Show(@"Please enter the Order No."); }
            else
            {
                txtBtach.Text = "";
                txt_Customer.Text = "";
                txt_Project.Text = "";
                txt_FullDesc.Text = "";
                txt_TLM.Text = "";
                txt_TQTY.Text = "";
                txt_TSQM.Text = "";
               
                comboGrindProcess.Text = ""; comboPrintProcess.Text = ""; comboLGProcess.Text = ""; comboIGUProcess.Text = ""; comboBondingProcess.Text = "";
                btn_Process.Enabled = true;

                var search = (from ser in orderenter.Orders where (ser.OC_ID == int.Parse(txt_OD_ID.Text)) select ser).SingleOrDefault();
                if (search == null) { MessageBox.Show(@"This Order not exist"); }
                else

                {

                    txt_Customer.Text = search.Clinet_Name;
                    txt_Project.Text = search.Project_Name;
                    txt_FullDesc.Text = search.Descreption;
                    txt_TLM.Text = search.TLM.ToString();
                    txt_TQTY.Text = search.TQTY.ToString();
                    txt_TSQM.Text = search.TSQM.ToString();
                   txtBtach.Text = search.Batch.ToString();

                    var Grind = (from did in orderenter.Processes where search.Grind_Type == did.ID select new { did.Process_Name }).SingleOrDefault();
                    var Priint = (from did in orderenter.Processes where search.Print_type == did.ID select new { did.Process_Name }).SingleOrDefault();
                    var LG = (from did in orderenter.Processes where search.LG_Type == did.ID select new { did.Process_Name }).SingleOrDefault();
                    var IGU = (from did in orderenter.Processes where search.IGU_type == did.ID select new { did.Process_Name }).SingleOrDefault();
                    var Bonding = (from did in orderenter.Processes where search.Bonding_Type == did.ID select new { did.Process_Name }).SingleOrDefault();
                    if (Grind != null || Convert.ToInt32(Grind) != 0) { comboGrindProcess.Text = Grind.Process_Name; }
                    if (Priint != null || Convert.ToInt32(Priint) != 0) { comboPrintProcess.Text = Priint.Process_Name; }
                    if (LG != null || Convert.ToInt32(LG) != 0) { comboLGProcess.Text = LG.Process_Name; }
                    if (IGU != null || Convert.ToInt32(IGU) != 0) { comboIGUProcess.Text = IGU.Process_Name; }
                    if (Bonding != null || Convert.ToInt32(Bonding) != 0) { comboBondingProcess.Text = Bonding.Process_Name; }

                    btn_Process.Enabled = true;



                }
                var itemSer = from it in orderenter.ITEMs
                              where it.OC_ID == int.Parse(txt_OD_ID.Text) 
                              select new
                              {

                                  Order_No = it.OC_ID,
                                  Item_No = it.Item_ID,
                                  Width_mm = it.Width,
                                  Hight_mm = it.Hieght,
                                  Item_qty = it.QTY_Item,
                                  Glass_ID = it.Glass_ID,
                                  Pos = it.Pos,

                              };
                DGV_Item.DataSource = itemSer;
                DGV_Item.Columns[5].DefaultCellStyle.BackColor = Color.SpringGreen;
                DGV_Item.Columns[6].DefaultCellStyle.BackColor = Color.SpringGreen;
                orderenter.SubmitChanges();

                // fill compo glass   
                var ItemNo = (from it in orderenter.ITEMs where it.OC_ID == int.Parse(txt_OD_ID.Text) select new { itemNoFirest = it.Item_ID }).FirstOrDefault();
                var glstype = from gl in orderenter.ITEMs
                              join gt in orderenter.GlassTypes on gl.Glass_ID equals gt.Glass_ID
                              where gl.OC_ID == int.Parse(txt_OD_ID.Text) & gl.Item_ID == ItemNo.itemNoFirest & gl.Step1 == null               // for item NO 1
                              select new
                              {
                                  glassType = gt.Glass_Type,
                                  Pos = gl.Pos,

                              };


                comboglasstype.DataSource = glstype.Select(x => x.glassType);
                comboPos.DataSource = glstype.Select(y => y.Pos);



                var SearchTra = from tr in orderenter.Tracks
                                where tr.OC_ID == int.Parse(txt_OD_ID.Text)
                                select tr;
                foreach (Track tr in SearchTra)
                {
                    if (tr.OC_ID == int.Parse(txt_OD_ID.Text))
                    { btn_Process.Enabled = false; btn_Release.Enabled = false; }
                }

                if (txt_Customer.Text == "")
                    btn_AW.Enabled = true;
                else
                    btn_AW.Enabled = false;
                btn_Save.Enabled = true;


            }

        }



        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            panel1.Enabled = true;
            panel2.Enabled = true;
            txt_OD_ID.Focus();

        }

        private void btn_Process_Click(object sender, EventArgs e)
        {
            if (comboStep1.Text == "")
            {
                MessageBox.Show(@"Select Atleast one Process");
            }
            else
            {


                var Glass_ID = (from up in orderenter.GlassTypes
                                where up.Glass_Type == comboglasstype.Text
                                select new { GlssID = up.Glass_ID, }).SingleOrDefault();

                //add item Track
                var dep1 = (from did in orderenter.Departments where did.Department_Name == comboStep1.Text select new { dep1ID = did.Departmet_ID }).SingleOrDefault();
                var dep2 = (from did in orderenter.Departments where did.Department_Name == comboStep2.Text select new { dep2ID = did.Departmet_ID }).SingleOrDefault();
                var dep3 = (from did in orderenter.Departments where did.Department_Name == comboStep3.Text select new { dep3ID = did.Departmet_ID }).SingleOrDefault();
                var dep4 = (from did in orderenter.Departments where did.Department_Name == comboStep4.Text select new { dep4ID = did.Departmet_ID }).SingleOrDefault();
                var dep5 = (from did in orderenter.Departments where did.Department_Name == comboStep5.Text select new { dep5ID = did.Departmet_ID }).SingleOrDefault();
                var dep6 = (from did in orderenter.Departments where did.Department_Name == comboStep6.Text select new { dep6ID = did.Departmet_ID }).SingleOrDefault();
                var dep7 = (from did in orderenter.Departments where did.Department_Name == comboStep7.Text select new { dep7ID = did.Departmet_ID }).SingleOrDefault();
                var dep8 = (from did in orderenter.Departments where did.Department_Name == comboStep8.Text select new { dep8ID = did.Departmet_ID }).SingleOrDefault();

                for (int i = 0; i < DGV_Item.Rows.Count; i++)
                {
                    var step = from st in orderenter.ITEMs
                               join gt in orderenter.GlassTypes on st.Glass_ID equals gt.Glass_ID
                               where st.OC_ID == int.Parse(txt_OD_ID.Text) & gt.Glass_Type == comboglasstype.Text & st.Pos == int.Parse(comboPos.Text) //int.Parse(DGV_Item.Rows[i].Cells[0].Value.ToString()) & st.Item_ID == int.Parse(DGV_Item.Rows[i].Cells[1].Value.ToString())
                                                                                                                                                       // & st.Glass_ID == int.Parse(Glass_ID.GlssID.ToString()) & st.Pos == int.Parse(comboPos.Text.ToString())
                               select st;

                    foreach (ITEM st in step)
                    {
                        if (comboStep1.Text != "")
                        {
                            st.Step1 = dep1.dep1ID;

                            orderenter.SubmitChanges();

                        }
                        else { st.Step1 = null; }
                        if (comboStep2.Text != "")
                        {
                            st.Step2 = dep2.dep2ID;


                            orderenter.SubmitChanges();

                        }
                        else { st.Step2 = null; }

                        if (comboStep3.Text != "")
                        {
                            st.Step3 = dep3.dep3ID;

                            orderenter.SubmitChanges();

                        }
                        else { st.Step3 = null; }

                        if (comboStep4.Text != "")
                        {
                            st.Step4 = dep4.dep4ID;

                            orderenter.SubmitChanges();
                        }
                        else { st.Step4 = null; }

                        if (comboStep5.Text != "")
                        {
                            st.Step5 = dep5.dep5ID;

                            orderenter.SubmitChanges();
                        }
                        else { st.Step5 = null; }

                        if (comboStep6.Text != "")
                        {
                            st.Step6 = dep6.dep6ID;

                            orderenter.SubmitChanges();
                        }
                        else { st.Step6 = null; }

                        if (comboStep7.Text != "")
                        {
                            st.Step7 = dep7.dep7ID;

                            orderenter.SubmitChanges();
                        }
                        else { st.Step7 = null; }
                        if (comboStep8.Text != "")
                        {
                            st.Step8 = dep8.dep8ID;

                            orderenter.SubmitChanges();
                        }
                        else { st.Step8 = null; }

                        orderenter.SubmitChanges();


                    }

                }
                //// Add to Original QTY
                // if (comboStep1.Text != "")
                // {

                //     for (int i = 0; i < DGV_Item.Rows.Count; i++)
                //     {
                //         QtyOriginal add = new QtyOriginal();

                //         add.Departmet_ID = dep1.dep1ID;
                //         add.OC_ID = int.Parse(DGV_Item.Rows[i].Cells[0].Value.ToString());
                //         add.Item_ID = int.Parse(DGV_Item.Rows[i].Cells[1].Value.ToString());
                //         add.QTYOrigi = int.Parse(DGV_Item.Rows[i].Cells[4].Value.ToString());
                //         add.Glass_ID = int.Parse(DGV_Item.Rows[i].Cells[5].Value.ToString());
                //         add.Pos = int.Parse(DGV_Item.Rows[i].Cells[6].Value.ToString());
                //         add.QTYFinish = 0;
                //         orderenter.QtyOriginals.InsertOnSubmit(add);
                //         orderenter.SubmitChanges();
                //     }
                // }

                // if (comboStep2.Text != "")
                // {

                //     for (int i = 0; i < DGV_Item.Rows.Count; i++)
                //     {
                //         QtyOriginal add = new QtyOriginal();

                //         add.Departmet_ID = dep2.dep2ID;
                //         add.OC_ID = int.Parse(DGV_Item.Rows[i].Cells[0].Value.ToString());
                //         add.Item_ID = int.Parse(DGV_Item.Rows[i].Cells[1].Value.ToString());
                //         add.QTYOrigi = int.Parse(DGV_Item.Rows[i].Cells[4].Value.ToString());
                //         add.Glass_ID = int.Parse(DGV_Item.Rows[i].Cells[5].Value.ToString());
                //         add.Pos = int.Parse(DGV_Item.Rows[i].Cells[6].Value.ToString());
                //         orderenter.QtyOriginals.InsertOnSubmit(add);
                //         orderenter.SubmitChanges();
                //     }
                // }

                // if (comboStep3.Text != "")
                // {

                //     for (int i = 0; i < DGV_Item.Rows.Count; i++)
                //     {
                //         QtyOriginal add = new QtyOriginal();

                //         add.Departmet_ID = dep3.dep3ID;
                //         add.OC_ID = int.Parse(DGV_Item.Rows[i].Cells[0].Value.ToString());
                //         add.Item_ID = int.Parse(DGV_Item.Rows[i].Cells[1].Value.ToString());
                //         add.QTYOrigi = int.Parse(DGV_Item.Rows[i].Cells[4].Value.ToString());
                //         add.Glass_ID = int.Parse(DGV_Item.Rows[i].Cells[5].Value.ToString());
                //         add.Pos = int.Parse(DGV_Item.Rows[i].Cells[6].Value.ToString());
                //         orderenter.QtyOriginals.InsertOnSubmit(add);
                //         orderenter.SubmitChanges();
                //     }
                // }

                // if (comboStep4.Text != "")
                // {

                //     for (int i = 0; i < DGV_Item.Rows.Count; i++)
                //     {
                //         QtyOriginal add = new QtyOriginal();

                //         add.Departmet_ID = dep4.dep4ID;
                //         add.OC_ID = int.Parse(DGV_Item.Rows[i].Cells[0].Value.ToString());
                //         add.Item_ID = int.Parse(DGV_Item.Rows[i].Cells[1].Value.ToString());
                //         add.QTYOrigi = int.Parse(DGV_Item.Rows[i].Cells[4].Value.ToString());
                //         add.Glass_ID = int.Parse(DGV_Item.Rows[i].Cells[5].Value.ToString());
                //         add.Pos = int.Parse(DGV_Item.Rows[i].Cells[6].Value.ToString());
                //         orderenter.QtyOriginals.InsertOnSubmit(add);
                //         orderenter.SubmitChanges();
                //     }
                // }

                //if (comboStep5.Text != "")
                // {

                //     for (int i = 0; i < DGV_Item.Rows.Count; i++)
                //     {
                //         QtyOriginal add = new QtyOriginal();

                //         add.Departmet_ID = dep5.dep5ID;
                //         add.OC_ID = int.Parse(DGV_Item.Rows[i].Cells[0].Value.ToString());
                //         add.Item_ID = int.Parse(DGV_Item.Rows[i].Cells[1].Value.ToString());
                //         add.QTYOrigi = int.Parse(DGV_Item.Rows[i].Cells[4].Value.ToString());
                //         add.Glass_ID = int.Parse(DGV_Item.Rows[i].Cells[5].Value.ToString());
                //         add.Pos = int.Parse(DGV_Item.Rows[i].Cells[6].Value.ToString());
                //         orderenter.QtyOriginals.InsertOnSubmit(add);
                //         orderenter.SubmitChanges();
                //     }
                // }

                //  if (comboStep6.Text != "")
                // {

                //     for (int i = 0; i < DGV_Item.Rows.Count; i++)
                //     {
                //         QtyOriginal add = new QtyOriginal();

                //         add.Departmet_ID = dep6.dep6ID;
                //         add.OC_ID = int.Parse(DGV_Item.Rows[i].Cells[0].Value.ToString());
                //         add.Item_ID = int.Parse(DGV_Item.Rows[i].Cells[1].Value.ToString());
                //         add.QTYOrigi = int.Parse(DGV_Item.Rows[i].Cells[4].Value.ToString());
                //         add.Glass_ID = int.Parse(DGV_Item.Rows[i].Cells[5].Value.ToString());
                //         add.Pos = int.Parse(DGV_Item.Rows[i].Cells[6].Value.ToString());
                //         orderenter.QtyOriginals.InsertOnSubmit(add);
                //         orderenter.SubmitChanges();
                //     }
                // }

                //  if (comboStep7.Text != "")
                // {

                //     for (int i = 0; i < DGV_Item.Rows.Count; i++)
                //     {
                //         QtyOriginal add = new QtyOriginal();

                //         add.Departmet_ID = dep7.dep7ID;
                //         add.OC_ID = int.Parse(DGV_Item.Rows[i].Cells[0].Value.ToString());
                //         add.Item_ID = int.Parse(DGV_Item.Rows[i].Cells[1].Value.ToString());
                //         add.QTYOrigi = int.Parse(DGV_Item.Rows[i].Cells[4].Value.ToString());
                //         add.Glass_ID = int.Parse(DGV_Item.Rows[i].Cells[5].Value.ToString());
                //         add.Pos = int.Parse(DGV_Item.Rows[i].Cells[6].Value.ToString());
                //         orderenter.QtyOriginals.InsertOnSubmit(add);
                //         orderenter.SubmitChanges();
                //     }
                // }

                //  if (comboStep8.Text != "")
                // {

                //     for (int i = 0; i < DGV_Item.Rows.Count; i++)
                //     {
                //         QtyOriginal add = new QtyOriginal();

                //         add.Departmet_ID = dep8.dep8ID;
                //         add.OC_ID = int.Parse(DGV_Item.Rows[i].Cells[0].Value.ToString());
                //         add.Item_ID = int.Parse(DGV_Item.Rows[i].Cells[1].Value.ToString());
                //         add.QTYOrigi = int.Parse(DGV_Item.Rows[i].Cells[4].Value.ToString());
                //         add.Glass_ID = int.Parse(DGV_Item.Rows[i].Cells[5].Value.ToString());
                //         add.Pos = int.Parse(DGV_Item.Rows[i].Cells[6].Value.ToString());
                //         orderenter.QtyOriginals.InsertOnSubmit(add);
                //         orderenter.SubmitChanges();
                //     }
                // }


                //  MessageBox.Show("One process added");
                comboglasstype.Text = "";
                comboPos.Text = "";
                txtglsID.Text = "";
                var ItemNo = (from it in orderenter.ITEMs where it.OC_ID == int.Parse(txt_OD_ID.Text) select new { itemNoFirest = it.Item_ID }).FirstOrDefault();
                var glstype = from gl in orderenter.ITEMs
                              join gt in orderenter.GlassTypes on gl.Glass_ID equals gt.Glass_ID
                              where gl.OC_ID == int.Parse(txt_OD_ID.Text) & gl.Item_ID == ItemNo.itemNoFirest & gl.Step1 == null
                              select new
                              {
                                  glassType = gt.Glass_Type,
                                  Pos = gl.Pos,

                              };


                comboglasstype.DataSource = glstype.Select(x => x.glassType);
                comboPos.DataSource = glstype.Select(y => y.Pos);



                //};
            }
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void txt_OD_ID_TextChanged(object sender, EventArgs e)
        {

            button3.Enabled = true;
        }

        private void txt_OD_ID_KeyPress(object sender, KeyPressEventArgs e)
        {
            onlynumber(e);
        }

        private void txt_OD_ID_DragEnter(object sender, DragEventArgs e)
        {

        }

        private void txt_OD_ID_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button3.PerformClick();
            }

        }

        private void comboStep1_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboStep1.Text != "")
            { comboStep2.Enabled = true; }
            else { }

            if (comboStep1.Text == "Dispatch")
            {
                comboStep2.Text = "";
                comboStep2.Enabled = false;
                comboStep3.Text = "";
                comboStep3.Enabled = false;
                comboStep4.Text = "";
                comboStep4.Enabled = false;
                comboStep5.Text = "";
                comboStep5.Enabled = false;
                comboStep6.Text = "";
                comboStep6.Enabled = false;
                comboStep7.Text = "";
                comboStep7.Enabled = false;
                comboStep8.Text = "";
                comboStep8.Enabled = false;
            }

        }

        private void comboStep2_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboStep2.Text != "")
            { comboStep3.Enabled = true; }
            else { }

            if (comboStep2.Text == "Dispatch")
            {
                comboStep3.Text = "";
                comboStep3.Enabled = false;
                comboStep4.Text = "";
                comboStep4.Enabled = false;
                comboStep5.Text = "";
                comboStep5.Enabled = false;
                comboStep6.Text = "";
                comboStep6.Enabled = false;
                comboStep7.Text = "";
                comboStep7.Enabled = false;
                comboStep8.Text = "";
                comboStep8.Enabled = false;
            }
        }

        private void comboStep3_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboStep3.Text != "")
            { comboStep4.Enabled = true; }
            else { }

            if (comboStep3.Text == "Dispatch")
            {

                comboStep4.Text = "";
                comboStep4.Enabled = false;
                comboStep5.Text = "";
                comboStep5.Enabled = false;
                comboStep6.Text = "";
                comboStep6.Enabled = false;
                comboStep7.Text = "";
                comboStep7.Enabled = false;
                comboStep8.Text = "";
                comboStep8.Enabled = false;
            }
        }

        private void comboStep4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboStep4.Text != "")
            { comboStep5.Enabled = true; }
            else { }

            if (comboStep4.Text == "Dispatch")
            {
                comboStep5.Text = "";
                comboStep5.Enabled = false;
                comboStep6.Enabled = false;
                comboStep7.Text = "";
                comboStep7.Enabled = false;
                comboStep8.Text = "";
                comboStep8.Enabled = false;
            }
        }

        private void comboStep5_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboStep5.Text != "")
            { comboStep6.Enabled = true; }
            else { }

            if (comboStep5.Text == "Dispatch")
            {
                comboStep6.Text = "";
                comboStep6.Enabled = false;
                comboStep7.Text = "";
                comboStep7.Enabled = false;
                comboStep8.Text = "";
                comboStep8.Enabled = false;
            }
        }

        private void comboStep6_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboStep6.Text != "")
            { comboStep7.Enabled = true; }
            else { }

            if (comboStep6.Text == "Dispatch")
            {
                comboStep7.Text = "";
                comboStep7.Enabled = false;
                comboStep8.Text = "";
                comboStep8.Enabled = false;
            }
        }

        private void comboStep7_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboStep7.Text != "")
            { comboStep8.Enabled = true; }
            else { }

            if (comboStep7.Text == "Dispatch")
            {
                comboStep8.Text = "";
                comboStep8.Enabled = false;
            }
        }

        private void comboStep8_SelectedValueChanged(object sender, EventArgs e)
        {

        }


        private void comboglasstype_SelectedValueChanged(object sender, EventArgs e)
        {

            //var glsPOS= from it in orderenter.ITEMs where 

            var glsid = from gid in orderenter.GlassTypes
                        join it in orderenter.ITEMs on gid.Glass_ID equals it.Glass_ID
                        where gid.Glass_Type == comboglasstype.Text
                        select gid;
            foreach (GlassType up in glsid)

            { txtglsID.Text = (up.Glass_ID.ToString()); }
        }

        private void btn_Release_Click_1(object sender, EventArgs e)
        {



            //// add to track


            for (int i = 0; i < DGV_Item.Rows.Count; i++)
            {

                Track add = new Track();

                add.Track_ID_Parent = 8;
                add.OC_ID = int.Parse(DGV_Item.Rows[i].Cells[0].Value.ToString());
                add.Item_ID = int.Parse(DGV_Item.Rows[i].Cells[1].Value.ToString());
                add.QTY_Recive = int.Parse(DGV_Item.Rows[i].Cells[4].Value.ToString());
                add.QTY_ToDo = int.Parse(DGV_Item.Rows[i].Cells[4].Value.ToString());
                add.Glass_ID = int.Parse(DGV_Item.Rows[i].Cells[5].Value.ToString());
                add.Pos = int.Parse(DGV_Item.Rows[i].Cells[6].Value.ToString());
                add.Recived_From = 8;



                var stepTrack = from st in orderenter.ITEMs
                                where st.OC_ID == int.Parse(DGV_Item.Rows[i].Cells[0].Value.ToString()) & st.Item_ID == int.Parse(DGV_Item.Rows[i].Cells[1].Value.ToString())
                                     & st.Glass_ID == int.Parse(DGV_Item.Rows[i].Cells[5].Value.ToString()) & st.Pos == int.Parse(DGV_Item.Rows[i].Cells[6].Value.ToString())
                                select st;
                foreach (ITEM st in stepTrack)
                { add.Departmet_ID = st.Step1; }


                if (checkBalance.Checked == true)
                { add.Balance = true; }


                orderenter.Tracks.InsertOnSubmit(add);

            }



            DialogResult result = MessageBox.Show("Are you sure you want to send the order to this department  ?", "Relese Order", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                orderenter.SubmitChanges();
                var OrderStatus = (from ser in orderenter.Orders where (ser.OC_ID == int.Parse(txt_OD_ID.Text)) select ser).SingleOrDefault();
                OrderStatus.Status = "In Production";
                OrderStatus.Release_Date = DateTime.Today;
                

                orderenter.SubmitChanges();

                txt_Customer.Text = "";
                txt_FullDesc.Text = "";
                txt_OD_ID.Text = "";
                txt_Project.Text = "";
                txt_TQTY.Text = "";
                txt_TSQM.Text = "";
                txt_TLM.Text = "";

                DGV_Item.DataSource = "";



                MessageBox.Show(@"The Items sent to next department");
            }
            else { }

            ///// add track No to barcode table
            ////////// Fill BarCode Table /////////////////DbFunctions.Right("00" + RN.Number, 3) Text.Remove(txtBarCode.Text.Length - 1);
            ///////////l.Select(s => new string(s.Skip(4).ToArray())).ToList();

            //for (int i = 0; i < DGV_Item.Rows.Count; i++)
            //{
            //    int W = int.Parse(DGV_Item.Rows[i].Cells[2].Value + "000");
            //    int H = int.Parse(DGV_Item.Rows[i].Cells[3].Value + "000");
            //    var getTelNo = from gt in Alcim.pool_teiles
            //                       // join pr in Alcim.fein_units on new { gt.auftnr, gt.pos, gt.teile_nr } equals new { pr.auftnr, pr.pos, pr.teile_nr }
            //                   where gt.auftnr == int.Parse(DGV_Item.Rows[i].Cells[0].Value.ToString())
            //                   & gt.glasart == int.Parse(DGV_Item.Rows[i].Cells[5].Value.ToString())
            //                  & ((gt.pos == int.Parse(DGV_Item.Rows[i].Cells[1].Value.ToString())) || (gt.breite == W) & (gt.hoehe == H))
            //                  & (gt.ttyp == 1)

            //                   select gt;
            //    foreach (pool_teile gt in getTelNo)
            //    {
            //        var getBarCode = from pr in Alcim.fein_units
            //                         where
            //                           pr.auftnr == gt.auftnr & pr.pos == gt.pos
            //                           & pr.teile_nr == gt.teile_nr & pr.typ == 100

            //                         select pr;
            //        foreach (fein_unit pr in getBarCode)
            //        {
            //            BarCode addbar = new BarCode();
            //            addbar.OC_ID = pr.auftnr;
            //            addbar.Item_ID = int.Parse(DGV_Item.Rows[i].Cells[1].Value.ToString());
            //            addbar.Pos = int.Parse(DGV_Item.Rows[i].Cells[6].Value.ToString());
            //            addbar.Part_nr = pr.teile_nr;
            //            addbar.BarCodeItem = pr.etikettnr;
            //            addbar.Glass_ID = gt.glasart;
            //            addbar.AlcimPos = pr.pos;
            //            addbar.Type = pr.typ;
            //            addbar.No = pr.lfd_nummer;
            //            addbar.Width = gt.breite;
            //            addbar.Hieght = gt.hoehe;


            //            orderenter.BarCodes.InsertOnSubmit(addbar);
            //            //orderenter.SubmitChanges();
            //        }
            //        orderenter.SubmitChanges();
            //    }







            // remove duplicate barcode
            //var duplicates = (from r in orderenter.BarCodes
            //                  where r.OC_ID == int.Parse(txt_OD_ID.Text)
            //                  group r by new { r.BarCodeItem } into results
            //                  select results.Skip(1)
            //  ).SelectMany(a => a);

            //orderenter.BarCodes.DeleteAllOnSubmit(duplicates);


            //// change the order status
            //orderenter.SubmitChanges();






        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var search = (from ser in orderenter.Orders where (ser.OC_ID == int.Parse(txt_OD_ID.Text)) select ser).SingleOrDefault();
            if (search == null) { MessageBox.Show(@"This Order not exist"); }
            else

            {
                    var searchTrack = (from sert in orderenter.Tracks where (sert.OC_ID == int.Parse(txt_OD_ID.Text)) select sert).FirstOrDefault();
                if (searchTrack != null) { MessageBox.Show(@"This Order is released. can't delete it"); }
                else
                {

                    DialogResult result = MessageBox.Show("Are you sure you want to delete this order ?", "Delete Order", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        var delitem = from delIT in orderenter.ITEMs where (delIT.OC_ID == int.Parse(txt_OD_ID.Text)) select delIT;
                        orderenter.ITEMs.DeleteAllOnSubmit(delitem);
                        orderenter.SubmitChanges();

                        var delorder = from del in orderenter.Orders where (del.OC_ID == int.Parse(txt_OD_ID.Text)) select del;
                        orderenter.Orders.DeleteAllOnSubmit(delorder);
                        orderenter.SubmitChanges();
                        button2.PerformClick();

                    }
                    else { }
                }
             
            }
        }

        private void comboLGProcess_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}




