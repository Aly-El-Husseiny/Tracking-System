using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using System.Windows.Forms.DataVisualization.Charting;




namespace Cutting
{
    public partial class Frm_Track : Form
    {
        TrackingDataContext trackdb = new TrackingDataContext();
        SMARTDBDataContext Alfak = new SMARTDBDataContext();
        public Frm_Track()
        {
            InitializeComponent();
        }
        private void copyAlltoClipboard(DataGridView DGV)
        {

            DGV.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            DGV.MultiSelect = true;
            DGV.SelectAll();
            DataObject dataObj = DGV.GetClipboardContent();
            if (dataObj != null)
                Clipboard.SetDataObject(dataObj);

        }
        private void createExcel(DataGridView exDGV)
        {
            copyAlltoClipboard(exDGV);
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
        private void toolStripButton3_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void dateTo_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label17_Click(object sender, EventArgs e)
        {

        }

        private void label23_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            DGV_Inprod.DataSource = "";
            txtOrderNO.Text = "";

            var prodSer = from pro in trackdb.Tracks
                          from Glass in trackdb.GlassTypes.Where(Glass => Glass.Glass_ID == pro.Glass_ID).DefaultIfEmpty()
                          join iddd in trackdb.ITEMs on new { pro.OC_ID, pro.Item_ID, pro.Pos } equals new { iddd.OC_ID, iddd.Item_ID, iddd.Pos }
                          join ord in trackdb.Orders on pro.OC_ID equals ord.OC_ID
                          join dep in trackdb.Departments on pro.Departmet_ID equals dep.Departmet_ID
                          //join gtype in trackdb.GlassTypes on pro.Glass_ID equals gtype.Glass_ID

                          where
                          (pro.QTY_ToDo != 0) &&
                          (string.IsNullOrEmpty(txtcustomer.Text) || ord.Clinet_Name == txtcustomer.Text) &&
                          (string.IsNullOrEmpty(txtproject.Text) || ord.Project_Name == txtproject.Text) &&
                          (string.IsNullOrEmpty(comboDep.Text) || dep.Department_Name == comboDep.Text) &&
                          (string.IsNullOrEmpty(comboGlass.Text) || Glass.Glass_Type == comboGlass.Text)

                          select new
                          {
                              Customer = ord.Clinet_Name,
                              Project = ord.Project_Name,
                              Department = dep.Department_Name,
                              Order_No = pro.OC_ID,
                              Item = pro.Item_ID,
                              Width = iddd.Width,
                              Height = iddd.Hieght,
                              QTY = pro.QTY_ToDo,
                              Glass_Type = Glass.Glass_Type,
                              Tempering = pro.Temp,
                              LG = pro.LG,
                              IGU = pro.IGU,
                              Recive_Date = pro.Date,
                              Time = pro.Time,
                              ID = pro.Track_ID,
                          };
            DGV_Inprod.DataSource = prodSer;
            for (int i = 0; i < DGV_Inprod.Rows.Count; i++)
            {
                if (i % 2 == 0)
                {
                    DGV_Inprod.Rows[i].DefaultCellStyle.BackColor = Color.LightBlue;
                }
            }
            int sumqty = 0;
            for (int i = 0; i < DGV_Inprod.Rows.Count; ++i)
            {
                sumqty += Convert.ToInt32(DGV_Inprod.Rows[i].Cells[7].Value);

            }
            txtqty.Text = sumqty.ToString();

            double sumArea = 0;
            for (int i = 0; i < DGV_Inprod.Rows.Count; ++i)
            {
                int qty = Convert.ToInt32(DGV_Inprod.Rows[i].Cells[7].Value);
                double w = Convert.ToDouble(DGV_Inprod.Rows[i].Cells[5].Value);
                double h = Convert.ToDouble(DGV_Inprod.Rows[i].Cells[6].Value);
                double raw1 = (qty * w * h) / 1000000;
                sumArea += Convert.ToDouble(raw1);

            }
            sumArea = Math.Round(sumArea, 2);
            txtsqm.Text =  sumArea.ToString();

        }

        private void Frm_Track_Load(object sender, EventArgs e)
        {
            comboDep.DataSource = trackdb.Departments.ToList();
            comboDep.ValueMember = "Departmet_ID";
            comboDep.DisplayMember = "Department_Name";
            comboDep.Text = "";

            comboGlass.DataSource = trackdb.GlassTypes.ToList();
            comboGlass.ValueMember = "Glass_ID";
            comboGlass.DisplayMember = "Glass_Type";
            comboGlass.Text = "";

        }

        private void button3_Click(object sender, EventArgs e)
        {
            var prodution = from pro in trackdb.Tracks
                            from Glass in trackdb.GlassTypes.Where(Glass => Glass.Glass_ID == pro.Glass_ID).DefaultIfEmpty()
                            join iddd in trackdb.ITEMs on new { pro.OC_ID, pro.Item_ID, pro.Pos } equals new { iddd.OC_ID, iddd.Item_ID, iddd.Pos }
                            join ord in trackdb.Orders on pro.OC_ID equals ord.OC_ID
                            join dep in trackdb.Departments on pro.Departmet_ID equals dep.Departmet_ID

                            where
                            (pro.QTY_ToDo != 0)
                            select new
                            {
                                Department = dep.Department_Name,
                                Customer=ord.Clinet_Name,
                                Project=ord.Project_Name,
                                Order_No = pro.OC_ID,
                                Item = pro.Item_ID,
                                Width = iddd.Width,
                                Height = iddd.Hieght,
                                QTY = pro.QTY_ToDo,
                                Glass_Type = Glass.Glass_Type,
                                Recive_Date = pro.Date,
                                Time = pro.Time,
                                ID = pro.Track_ID,
                                Tempering = pro.Temp,
                                LG = pro.LG,
                                IGU = pro.IGU,

                            };
            DGV_Inprod.DataSource = prodution;


            for (int i = 0; i < DGV_Inprod.Rows.Count; i++)
            {
                if (i % 2 == 0)
                {
                    DGV_Inprod.Rows[i].DefaultCellStyle.BackColor = Color.LightBlue;
                }
            }

            int sumqty = 0;
            for (int i = 0; i < DGV_Inprod.Rows.Count; ++i)
            {
                sumqty += Convert.ToInt32(DGV_Inprod.Rows[i].Cells[7].Value);

            }
            txtqty.Text = sumqty.ToString();

            double sumArea = 0;
            for (int i = 0; i < DGV_Inprod.Rows.Count; ++i)
            {
                int qty = Convert.ToInt32(DGV_Inprod.Rows[i].Cells[7].Value);
                double w = Convert.ToDouble(DGV_Inprod.Rows[i].Cells[5].Value);
                double h = Convert.ToDouble(DGV_Inprod.Rows[i].Cells[6].Value);
                double raw1 = (qty * w * h) / 1000000;
                sumArea += Convert.ToDouble(raw1);

            }
            sumArea = Math.Round(sumArea, 2);
            txtsqm.Text = sumArea.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            txtcustomer.Text = ""; txtOrderNO.Text = ""; txtproject.Text = ""; comboDep.Text = ""; comboGlass.Text = "";
            DGV_Inprod.DataSource = "";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var FinGood = from Orders in trackdb.Dispatches
                        join oc in trackdb.Orders on Orders.OC_ID equals oc.OC_ID
                        join iddd in trackdb.ITEMs on new { Orders.OC_ID, Orders.Item_ID, Orders.Pos } equals new { iddd.OC_ID, iddd.Item_ID, iddd.Pos }
                        where oc.Clinet_Name.Contains(txtcustomer.Text) && oc.Project_Name.Contains(txtproject.Text)
                        group Orders by new

                        {
                            Customer = oc.Clinet_Name,
                            Project = oc.Project_Name,
                            Order_No = Orders.OC_ID,

                            Item_No = Orders.Item_ID,
                            Description = oc.Descreption,
                            Width = iddd.Width,
                            Height = iddd.Hieght,


                        } into total

                        select new

                        {
                            Customer = total.Key.Customer,
                            Project = total.Key.Project,
                            OrderNo = total.Key.Order_No,
                            itemNo = total.Key.Item_No,
                            Description = total.Key.Description,
                            Width = total.Key.Width,
                            Height = total.Key.Height,
                            Total_Recive = total.Sum(p => p.QTY_Recive),

                            Total_Sent = total.Sum(p => p.QTY_Send),

                            Total_Balance = (total.Sum(p => p.QTY_Recive) - total.Sum(p => p.QTY_Send).GetValueOrDefault(0))


                        };
            var filtered = FinGood.Where(t => t.Total_Balance > 0);
            DGV_Inprod.DataSource = filtered;
            DGV_Inprod.Columns[9].DefaultCellStyle.BackColor = Color.LightBlue;


            //var Good = from pr in trackdb.Tracks

            //           join iddd in trackdb.ITEMs on new { pr.OC_ID, pr.Item_ID, pr.Pos } equals new { iddd.OC_ID, iddd.Item_ID, iddd.Pos }
            //           join ord in trackdb.Orders on pr.OC_ID equals ord.OC_ID
            //           where (pr.Departmet_ID == 7 && pr.QTY_ToDo != 0)
            //           select new
            //           {

            //               Customer=ord.Clinet_Name,
            //               Project=ord.Project_Name,
            //               WorkOrder = pr.OC_ID,
            //               Item = pr.Item_ID,
            //               Width = iddd.Width,
            //               Height = iddd.Hieght,
            //               QTY = pr.QTY_ToDo,
            //               Glass_Type = ord.Descreption,
            //               Recive_Date = pr.Date,
            //               ID = pr.Track_ID,

            //           };
           // DGV_Inprod.DataSource = FinGood;
            for (int i = 0; i < DGV_Inprod.Rows.Count; i++)
            {
                if (i % 2 == 0)
                {
                    DGV_Inprod.Rows[i].DefaultCellStyle.BackColor = Color.LightBlue;
                }
            }
            int sumqty = 0;
            for (int i = 0; i < DGV_Inprod.Rows.Count; ++i)
            {
                sumqty += Convert.ToInt32(DGV_Inprod.Rows[i].Cells[9].Value);

            }
            txtqty.Text = sumqty.ToString();

            double sumArea = 0;
            for (int i = 0; i < DGV_Inprod.Rows.Count; ++i)
            {
                int qty = Convert.ToInt32(DGV_Inprod.Rows[i].Cells[9].Value);
                double w = Convert.ToDouble(DGV_Inprod.Rows[i].Cells[5].Value);
                double h = Convert.ToDouble(DGV_Inprod.Rows[i].Cells[6].Value);
                double raw1 = (qty * w * h) / 1000000;
                sumArea += Convert.ToDouble(raw1);

            }
            sumArea = Math.Round(sumArea, 2);
            txtsqm.Text = sumArea.ToString();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            if (txtOrderNO.Text == "")
            { MessageBox.Show(@"Please enter the Order No."); }
            else
            {
                var prodution = from pro in trackdb.Tracks
                                from Glass in trackdb.GlassTypes.Where(Glass => Glass.Glass_ID == pro.Glass_ID).DefaultIfEmpty()
                                join iddd in trackdb.ITEMs on new { pro.OC_ID, pro.Item_ID ,pro.Pos} equals new { iddd.OC_ID, iddd.Item_ID,iddd.Pos }
                                join ord in trackdb.Orders on pro.OC_ID equals ord.OC_ID
                                join dep in trackdb.Departments on pro.Departmet_ID equals dep.Departmet_ID

                                where
                                (pro.QTY_ToDo != 0) && pro.OC_ID == int.Parse(txtOrderNO.Text)
                                select new
                                {
                                    Department = dep.Department_Name,
                                    Item = pro.Item_ID,
                                    Width = iddd.Width,
                                    Height = iddd.Hieght,
                                    QTY = pro.QTY_ToDo,
                                    Glass_Type = Glass.Glass_Type,
                                    Tempering = pro.Temp,
                                    LG = pro.LG,
                                    IGU = pro.IGU,
                                    Recive_Date = pro.Date,
                                    ID = pro.Track_ID,

                                };
                DGV_Inprod.DataSource = prodution;
                int sumqty = 0;
                for (int i = 0; i < DGV_Inprod.Rows.Count; ++i)
                {
                    sumqty += Convert.ToInt32(DGV_Inprod.Rows[i].Cells[4].Value);

                }
                txtqty.Text = sumqty.ToString();

                double sumArea = 0;
                for (int i = 0; i < DGV_Inprod.Rows.Count; ++i)
                {
                    int qty = Convert.ToInt32(DGV_Inprod.Rows[i].Cells[4].Value);
                    double w = Convert.ToDouble(DGV_Inprod.Rows[i].Cells[2].Value);
                    double h = Convert.ToDouble(DGV_Inprod.Rows[i].Cells[3].Value);
                    double raw1 = (qty * w * h) / 1000000;  
                    sumArea += Convert.ToDouble(raw1);

                }
                sumArea = Math.Round(sumArea, 2);
                txtsqm.Text = sumArea.ToString();
            }

        }

       
        private void btnExcelProd_Click(object sender, EventArgs e)
        {
            createExcel(DGV_Inprod);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (txtOrderBalance.Text == "")
            { MessageBox.Show(@"Please enter the Order No."); }
            else
            {

              
                var dispatched = from itemID in Alfak.BW_AUFTR_POs
                                 join pro in Alfak.BW_AUFTR_KOPFs on itemID.ID equals pro.ID
                                 where itemID.ID == int.Parse(txtOrderBalance.Text) && (itemID.PP_ORIG_MENGE - itemID.PP_MENGE) > 0
                                 select new
                                 {
                                    
                                     Order_No = itemID.ID,
                                     Item_No = itemID.POS_NR,
                                     Width_mm = Convert.ToInt32(itemID.PP_BREITE),
                                     Hight_mm = Convert.ToInt32(itemID.PP_HOEHE),
                                     Item_qty = (Convert.ToInt32(itemID.PP_ORIG_MENGE) - Convert.ToInt32(itemID.PP_MENGE)),
                                     GLass_Name = itemID.PROD_BEZ1 + itemID.PROD_BEZ2 + itemID.PROD_BEZ3

                                 };
                DGV_Dispatched.DataSource = dispatched;
                DGV_Dispatched.Columns[0].DefaultCellStyle.BackColor = Color.LightGreen;
                DGV_Dispatched.Columns[4].DefaultCellStyle.BackColor = Color.LightGreen;



                var balance = from itemID in Alfak.BW_AUFTR_POs
                              join pro in Alfak.BW_AUFTR_KOPFs on itemID.ID equals pro.ID
                              where itemID.ID == int.Parse(txtOrderBalance.Text) && itemID.PP_MENGE > 0
                              select new
                              {
                                 
                                  Order_No = itemID.ID,
                                  Item_No = itemID.POS_NR,
                                  Width_mm = Convert.ToInt32(itemID.PP_BREITE),
                                  Hight_mm = Convert.ToInt32(itemID.PP_HOEHE),
                                  Item_qty = Convert.ToInt32(itemID.PP_MENGE),
                                  GLass_Name = itemID.PROD_BEZ1 + itemID.PROD_BEZ2 + itemID.PROD_BEZ3

                              };

                DGVBalance.DataSource = balance;
                DGVBalance.Columns[1].DefaultCellStyle.BackColor = Color.LightPink;
                DGVBalance.Columns[4].DefaultCellStyle.BackColor = Color.LightPink;


                var project = (from pro in Alfak.BW_AUFTR_KOPFs
                               where pro.ID == int.Parse(txtOrderBalance.Text)
                               select new { cust = pro.AH_MCODE, proj = pro.AH_NAME2, }).SingleOrDefault();

                txtcustbalance.Text = project.cust;
                txtprojbalance.Text = project.proj;

            };

            int sumqty_Dis = 0;
            for (int i = 0; i < DGV_Dispatched.Rows.Count; ++i)
            {
                sumqty_Dis += Convert.ToInt32(DGV_Dispatched.Rows[i].Cells[4].Value);

            }
            txtqtydis.Text = sumqty_Dis.ToString();

            double sumArea_Dis = 0;
            for (int i = 0; i < DGV_Dispatched.Rows.Count; ++i)
            {
                int qty = Convert.ToInt32(DGV_Dispatched.Rows[i].Cells[4].Value);
                double w = Convert.ToDouble(DGV_Dispatched.Rows[i].Cells[2].Value);
                double h = Convert.ToDouble(DGV_Dispatched.Rows[i].Cells[3].Value);
                double raw1 = (qty * w * h) / 1000000;
                sumArea_Dis += Convert.ToDouble(raw1);

            }
            sumArea_Dis = Math.Round(sumArea_Dis, 2);
            txtsqmdis.Text = sumArea_Dis.ToString();


            int sumqty_Balance = 0;
            for (int i = 0; i < DGVBalance.Rows.Count; ++i)
            {
                sumqty_Balance += Convert.ToInt32(DGVBalance.Rows[i].Cells[4].Value);

            }
            txtqtybalance.Text = sumqty_Balance.ToString();

            double sumArea_Balance = 0;
            for (int i = 0; i < DGVBalance.Rows.Count; ++i)
            {
                int qty = Convert.ToInt32(DGVBalance.Rows[i].Cells[4].Value);
                double w = Convert.ToDouble(DGVBalance.Rows[i].Cells[2].Value);
                double h = Convert.ToDouble(DGVBalance.Rows[i].Cells[3].Value);
                double raw1 = (qty * w * h) / 1000000;
                sumArea_Balance += Convert.ToDouble(raw1);

            }
            txtsqmbalance.Text = sumArea_Balance.ToString();

        }





        private void txtOrderBalance_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button6.PerformClick();
            }
        }

       
        private void button2_Click_2(object sender, EventArgs e)
        {
            createExcel(DGV_Dispatched);
            
        }

        
        private void button3_Click_1(object sender, EventArgs e)
        {
            createExcel(DGVBalance);
        }

        private void button7_Click(object sender, EventArgs e)
        {
           
            if (combo_Calc_dep.Text == "")
            { MessageBox.Show(@"PLease Select the Department first"); }
            else if (combo_Calc_dep.Text == "All")
            {
                Lap_result.Text = "All Departments Production in details";
                var trackfinish = from Trk in trackdb.Tracks
                                  from Glass in trackdb.GlassTypes.Where(Glass => Glass.Glass_ID == Trk.Glass_ID ).DefaultIfEmpty()
                                  join iddd in trackdb.ITEMs on new { Trk.OC_ID, Trk.Item_ID, Trk.Pos } equals new { iddd.OC_ID, iddd.Item_ID, iddd.Pos }
                                  join idDEP in trackdb.Departments on Trk.Departmet_ID equals idDEP.Departmet_ID
                                  join RecivedDep in trackdb.Departments on Trk.Recived_From equals RecivedDep.Departmet_ID
                                  join ocid in trackdb.Orders on Trk.OC_ID equals ocid.OC_ID
                                  where (Trk.Date <= date_Dep_to.Value && Trk.Date >= date_Dep_from.Value) && (Trk.Recived_From != 8 && Trk.Recived_From != 9)
                                  select new
                                  {
                                      
                                      WorkOrder = Trk.OC_ID,
                                      Item = Trk.Item_ID,
                                      Width = iddd.Width,
                                      Height = iddd.Hieght,
                                      Qty_Send = Trk.QTY_Recive,
                                      Total_Item_SQM= (iddd.Width* iddd.Hieght* Trk.QTY_Recive)/1000000,
                                      Total_Item_LM = ((iddd.Width + iddd.Hieght) *2 * Trk.QTY_Recive) /1000,
                                      GlassType = Glass.Glass_Type,
                                      Customer = ocid.Clinet_Name,
                                      Project = ocid.Project_Name,
                                      LG = Trk.LG,
                                      IGU = Trk.IGU,
                                      Send_To = idDEP.Department_Name,
                                   //   Recived_From=
                                      Date = Trk.Date,
                                      Time = Trk.Time,
                                      Department_Name = RecivedDep.Department_Name,
                                      Furnace = Trk.Furnace,
                                      Balance = Trk.Balance,
                                      Trak_ID = Trk.Track_ID

                                  };
                DGV_Sear_details.DataSource = trackfinish;
            }
            else
            {
                Lap_result.Text = "Departments Production in details";
                var trackfinish = from Trk in trackdb.Tracks
                                  from Glass in trackdb.GlassTypes.Where(Glass => Glass.Glass_ID == Trk.Glass_ID).DefaultIfEmpty()
                                  from DepName in trackdb.Departments.Where(DepName => DepName.Department_Name == combo_Calc_dep.Text)
                                  join iddd in trackdb.ITEMs on new { Trk.OC_ID, Trk.Item_ID, Trk.Pos } equals new { iddd.OC_ID, iddd.Item_ID, iddd.Pos }
                                  join idDEP in trackdb.Departments on Trk.Departmet_ID equals idDEP.Departmet_ID
                                  join RecivedDep in trackdb.Departments on Trk.Recived_From equals RecivedDep.Departmet_ID
                                  join ocid in trackdb.Orders on Trk.OC_ID equals ocid.OC_ID
                                  where (Trk.Date <= date_Dep_to.Value && Trk.Date >= date_Dep_from.Value) && Trk.Recived_From == DepName.Departmet_ID
                                  select new
                                  {

                                      WorkOrder = Trk.OC_ID,
                                      Item = Trk.Item_ID,
                                      Width = iddd.Width,
                                      Height = iddd.Hieght,
                                      Qty_Send = Trk.QTY_Recive,
                                     Total_Item_SQM = (iddd.Width * iddd.Hieght * Trk.QTY_Recive) / 1000000,
                                      Total_Item_LM = ((iddd.Width + iddd.Hieght) * 2 * Trk.QTY_Recive) / 1000,
                                      GlassType = Glass.Glass_Type,
                                      Customer = ocid.Clinet_Name,
                                      Project = ocid.Project_Name,
                                      LG = Trk.LG,
                                      IGU = Trk.IGU,
                                      Send_To = idDEP.Department_Name,
                                      Date = Trk.Date,
                                      Time = Trk.Time,
                                      Balance = Trk.Balance,
                                      Department_Name = RecivedDep.Department_Name,
                                      Furnace = Trk.Furnace,
                                      Trak_ID = Trk.Track_ID
                                  };
                DGV_Sear_details.DataSource = trackfinish;
            }
                int sumqty_Dep = 0;
                for (int i = 0; i < DGV_Sear_details.Rows.Count; ++i)
                {
                    sumqty_Dep += Convert.ToInt32(DGV_Sear_details.Rows[i].Cells[4].Value);

                }
                txt_Dep_QTY.Text = sumqty_Dep.ToString();

                double sumArea_Dep = 0;
                for (int i = 0; i < DGV_Sear_details.Rows.Count; ++i)
                {
                    int qty = Convert.ToInt32(DGV_Sear_details.Rows[i].Cells[4].Value);
                    double w = Convert.ToDouble(DGV_Sear_details.Rows[i].Cells[2].Value);
                    double h = Convert.ToDouble(DGV_Sear_details.Rows[i].Cells[3].Value);
                    double raw1 = (qty * w * h) / 1000000;
                    sumArea_Dep += Convert.ToDouble(raw1);

                }
                sumArea_Dep = Math.Round(sumArea_Dep, 2);
                txt_Dep_SQM.Text = sumArea_Dep.ToString();


                for (int i = 0; i < DGV_Sear_details.Rows.Count; i++)
                {
                    if (i % 2 == 0)
                    {
                        DGV_Sear_details.Rows[i].DefaultCellStyle.BackColor = Color.LightBlue;
                    }
                }
                var proTime = (from pr in trackdb.DepDatas
                               join depName in trackdb.Departments on pr.Department_ID equals depName.Departmet_ID
                               where pr.Date <= date_Dep_to.Value && pr.Date >= date_Dep_from.Value && depName.Department_Name == combo_Calc_dep.Text
                               select pr.ProdTime).Sum();

                txtprodT.Text = proTime.ToString();

                var downTime = (from pr in trackdb.DepDatas
                                join depName in trackdb.Departments on pr.Department_ID equals depName.Departmet_ID
                                where pr.Date <= date_Dep_to.Value && pr.Date >= date_Dep_from.Value && depName.Department_Name == combo_Calc_dep.Text
                                select pr.ProdDown).Sum();

                txtdownT.Text = downTime.ToString();
            
            
           




        }

        private void button5_Click(object sender, EventArgs e)
        {
         
            if (radioSingle.Checked == true)
            {
                Lap_result.Text = "Factory Production in details for Single Glass";
                var FinGood = from Orders in trackdb.Dispatches
                              join oc in trackdb.Orders on Orders.OC_ID equals oc.OC_ID
                              join iddd in trackdb.ITEMs on new { Orders.OC_ID, Orders.Item_ID, Orders.Pos } equals new { iddd.OC_ID, iddd.Item_ID, iddd.Pos }
                              where (Orders.DateIN <= date_Factory_to.Value && Orders.DateIN >= date_Factory_from.Value)
                              && (Orders.FullDesc[0]!='D' && Orders.FullDesc[0] != 'L')


                group Orders by new

                              {
                                  Customer = oc.Clinet_Name,
                                  Project = oc.Project_Name,
                                  Order_No = Orders.OC_ID,

                                  Item_No = Orders.Item_ID,
                                  Description = oc.Descreption,
                                  Width = iddd.Width,
                                  Height = iddd.Hieght,


                              } into total

                              select new

                              {
                                  Customer = total.Key.Customer,
                                  Project = total.Key.Project,
                                  OrderNo = total.Key.Order_No,
                                  itemNo = total.Key.Item_No,
                                  Description = total.Key.Description,
                                  Width = total.Key.Width,
                                  Height = total.Key.Height,
                                  Total_Recive = total.Sum(p => p.QTY_Recive),

                                  //Total_Sent = total.Sum(p => p.QTY_Send),

                                  //Total_Balance = (total.Sum(p => p.QTY_Recive) - total.Sum(p => p.QTY_Send).GetValueOrDefault(0))


                              };
                var filtered = FinGood.Where(t => t.Total_Recive > 0);
                DGV_Sear_details.DataSource = filtered;
                //DGV_Sear_details.Columns[9].DefaultCellStyle.BackColor = Color.LightBlue;
                //var FactoryProd = from id in trackdb.Tracks
                //                  join idd in trackdb.GlassTypes on id.Glass_ID equals idd.Glass_ID
                //                  join ord in trackdb.Orders on id.OC_ID equals ord.OC_ID
                //                  join iddd in trackdb.ITEMs on new { id.OC_ID, id.Item_ID, id.Pos } equals new { iddd.OC_ID, iddd.Item_ID, iddd.Pos }
                //                  //join idDEP in trackdb.Departments on id.Departmet_ID equals idDEP.Departmet_ID
                //                  where (id.Date <= date_Factory_to.Value && id.Date >= date_Factory_from.Value) && id.Departmet_ID == 7 && id.LG == null && id.IGU == null && id.Delivery_No != null
                //                  select new
                //                  {


                //                      Customer = ord.Clinet_Name,
                //                      Project = ord.Project_Name,
                //                      WorkOrder = id.OC_ID,
                //                      Item = id.Item_ID,
                //                      Width = iddd.Width,
                //                      Height = iddd.Hieght,
                //                      GlassType = idd.Glass_Type,
                //                      Qty_Production = id.QTY_Recive,
                //                      Date = id.Date,
                //                      Balance = id.Balance,
                //                      Delivary_No = id.Delivery_No,
                //                      Trak_ID = id.Track_ID
                //                  };

                //DGV_Sear_details.DataSource = FactoryProd;

                txt_prodType.Text = radioSingle.Text;

                int sumqty_Prod = 0;
                for (int i = 0; i < DGV_Sear_details.Rows.Count; ++i)
                {
                    sumqty_Prod += Convert.ToInt32(DGV_Sear_details.Rows[i].Cells[7].Value);

                }
                txt_Prod_qty.Text = sumqty_Prod.ToString();

                double sumArea_Prod = 0;
                for (int i = 0; i < DGV_Sear_details.Rows.Count; ++i)
                {
                    int qty = Convert.ToInt32(DGV_Sear_details.Rows[i].Cells[7].Value);
                    double w = Convert.ToDouble(DGV_Sear_details.Rows[i].Cells[5].Value);
                    double h = Convert.ToDouble(DGV_Sear_details.Rows[i].Cells[6].Value);
                    double raw1 = (qty * w * h) / 1000000;
                    sumArea_Prod += Convert.ToDouble(raw1);

                }
                sumArea_Prod = Math.Round(sumArea_Prod, 2);
                txt_Pro_sqm.Text = sumArea_Prod.ToString();


                for (int i = 0; i < DGV_Sear_details.Rows.Count; i++)
                {
                    if (i % 2 == 0)
                    {
                        DGV_Sear_details.Rows[i].DefaultCellStyle.BackColor = Color.LightBlue;
                    }
                }
            }else if(radioLamin.Checked==true)
            {
                Lap_result.Text = "Factory Production in details for Lamination Glass";
                var FinGoodLG = from Orders in trackdb.Dispatches
                              join oc in trackdb.Orders on Orders.OC_ID equals oc.OC_ID
                              join iddd in trackdb.ITEMs on new { Orders.OC_ID, Orders.Item_ID, Orders.Pos } equals new { iddd.OC_ID, iddd.Item_ID, iddd.Pos }
                              where (Orders.DateIN <= date_Factory_to.Value && Orders.DateIN >= date_Factory_from.Value)
                              && (Orders.FullDesc[0] == 'L')


                              group Orders by new

                              {
                                  Customer = oc.Clinet_Name,
                                  Project = oc.Project_Name,
                                  Order_No = Orders.OC_ID,

                                  Item_No = Orders.Item_ID,
                                  Description = oc.Descreption,
                                  Width = iddd.Width,
                                  Height = iddd.Hieght,


                              } into total

                              select new

                              {
                                  Customer = total.Key.Customer,
                                  Project = total.Key.Project,
                                  OrderNo = total.Key.Order_No,
                                  itemNo = total.Key.Item_No,
                                  Description = total.Key.Description,
                                  Width = total.Key.Width,
                                  Height = total.Key.Height,
                                  Total_Recive = total.Sum(p => p.QTY_Recive),

                                  //Total_Sent = total.Sum(p => p.QTY_Send),

                                  //Total_Balance = (total.Sum(p => p.QTY_Recive) - total.Sum(p => p.QTY_Send).GetValueOrDefault(0))


                              };
                var filtered = FinGoodLG.Where(t => t.Total_Recive > 0);
                DGV_Sear_details.DataSource = filtered;

                txt_prodType.Text = radioLamin.Text;

                int sumqty_Prod = 0;
                for (int i = 0; i < DGV_Sear_details.Rows.Count; ++i)
                {
                    sumqty_Prod += Convert.ToInt32(DGV_Sear_details.Rows[i].Cells[7].Value);

                }
                txt_Prod_qty.Text = sumqty_Prod.ToString();

                double sumArea_Prod = 0;
                for (int i = 0; i < DGV_Sear_details.Rows.Count; ++i)
                {
                    int qty = Convert.ToInt32(DGV_Sear_details.Rows[i].Cells[7].Value);
                    double w = Convert.ToDouble(DGV_Sear_details.Rows[i].Cells[5].Value);
                    double h = Convert.ToDouble(DGV_Sear_details.Rows[i].Cells[6].Value);
                    double raw1 = (qty * w * h) / 1000000;
                    sumArea_Prod += Convert.ToDouble(raw1);

                }
                txt_Pro_sqm.Text = sumArea_Prod.ToString();


                for (int i = 0; i < DGV_Sear_details.Rows.Count; i++)
                {
                    if (i % 2 == 0)
                    {
                        DGV_Sear_details.Rows[i].DefaultCellStyle.BackColor = Color.LightBlue;
                    }
                }
            }
            else if (radioIGU.Checked==true)
            {
                Lap_result.Text = "Factory Production in details for IGU";
                var FinGoodIGU = from Orders in trackdb.Dispatches
                              join oc in trackdb.Orders on Orders.OC_ID equals oc.OC_ID
                              join iddd in trackdb.ITEMs on new { Orders.OC_ID, Orders.Item_ID, Orders.Pos } equals new { iddd.OC_ID, iddd.Item_ID, iddd.Pos }
                              where (Orders.DateIN <= date_Factory_to.Value && Orders.DateIN >= date_Factory_from.Value)
                              && (Orders.FullDesc[0] == 'D')


                              group Orders by new

                              {
                                  Customer = oc.Clinet_Name,
                                  Project = oc.Project_Name,
                                  Order_No = Orders.OC_ID,

                                  Item_No = Orders.Item_ID,
                                  Description = oc.Descreption,
                                  Width = iddd.Width,
                                  Height = iddd.Hieght,


                              } into total

                              select new

                              {
                                  Customer = total.Key.Customer,
                                  Project = total.Key.Project,
                                  OrderNo = total.Key.Order_No,
                                  itemNo = total.Key.Item_No,
                                  Description = total.Key.Description,
                                  Width = total.Key.Width,
                                  Height = total.Key.Height,
                                  Total_Recive = total.Sum(p => p.QTY_Recive),

                                  //Total_Sent = total.Sum(p => p.QTY_Send),

                                  //Total_Balance = (total.Sum(p => p.QTY_Recive) - total.Sum(p => p.QTY_Send).GetValueOrDefault(0))


                              };
                var filtered = FinGoodIGU.Where(t => t.Total_Recive > 0);
                DGV_Sear_details.DataSource = filtered;

                txt_prodType.Text = radioIGU.Text;

                    int sumqty_Prod = 0;
                    for (int i = 0; i < DGV_Sear_details.Rows.Count; ++i)
                    {
                        sumqty_Prod += Convert.ToInt32(DGV_Sear_details.Rows[i].Cells[7].Value);

                    }
                    txt_Prod_qty.Text = sumqty_Prod.ToString();

                    double sumArea_Prod = 0;
                    for (int i = 0; i < DGV_Sear_details.Rows.Count; ++i)
                    {
                        int qty = Convert.ToInt32(DGV_Sear_details.Rows[i].Cells[7].Value);
                        double w = Convert.ToDouble(DGV_Sear_details.Rows[i].Cells[5].Value);
                        double h = Convert.ToDouble(DGV_Sear_details.Rows[i].Cells[6].Value);
                        double raw1 = (qty * w * h) / 1000000;
                        sumArea_Prod += Convert.ToDouble(raw1);

                    }
                    txt_Pro_sqm.Text = sumArea_Prod.ToString();


                    for (int i = 0; i < DGV_Sear_details.Rows.Count; i++)
                    {
                        if (i % 2 == 0)
                        {
                            DGV_Sear_details.Rows[i].DefaultCellStyle.BackColor = Color.LightBlue;
                        }
                    }
                }
            
        }

        private void txtOrderNO_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnOK.PerformClick();
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (txt_calc_rej_Or.Text == "")
            { MessageBox.Show(@"PLease enter the Order No."); }
            else
            {
                Lap_result.Text = "Order Waste in details ";
                var Original = from id in trackdb.Orders
                               where id.OC_ID == int.Parse(txt_calc_rej_Or.Text)
                               select id;
                foreach (Order id in Original)
                {

                    txt_calc_rej_pro.Text = id.Project_Name;
                    txt_calc_full.Text = id.Descreption;
                    txt_calc_org_qty.Text = id.TQTY.ToString();
                    txt_calc_org_sqm.Text = string.Format("{0:0.00}", id.TSQM).ToString();

                    var Waste = from idw in trackdb.QCs
                                join idd in trackdb.GlassTypes on idw.Glass_ID equals idd.Glass_ID
                                join iddd in trackdb.QC_rejs on idw.Rej_ID equals iddd.Rej_ID
                                join idDEP in trackdb.Departments on idw.Departmet_ID equals idDEP.Departmet_ID
                                where idw.OC_ID == txt_calc_rej_Or.Text && idw.Action == "Cut again"

                                select new
                                {

                                    Item = idw.Item,
                                    Width = idw.Width,
                                    Height = idw.Height,
                                    Glass_Type = idd.Glass_Type,
                                    Qty = idw.QTY,
                                    Area = idw.Area,
                                    Reject = iddd.Rej_Name,
                                    Department = idDEP.Department_Name,
                                    Rej_Date = idw.Reject_Date,

                                };

                    DGV_Sear_details.DataSource = Waste;

                    int sumqty_waste = 0;
                    for (int i = 0; i < DGV_Sear_details.Rows.Count; ++i)
                    {
                        sumqty_waste += Convert.ToInt32(DGV_Sear_details.Rows[i].Cells[4].Value);

                    }
                    txt_calc_rej_qty.Text = sumqty_waste.ToString();

                    double sumArea_waste = 0;
                    for (int i = 0; i < DGV_Sear_details.Rows.Count; ++i)
                    {
                        int qty = Convert.ToInt32(DGV_Sear_details.Rows[i].Cells[4].Value);
                        double w = Convert.ToDouble(DGV_Sear_details.Rows[i].Cells[1].Value);
                        double h = Convert.ToDouble(DGV_Sear_details.Rows[i].Cells[2].Value);
                        double raw1 = (qty * w * h) / 1000000;
                        sumArea_waste += Convert.ToDouble(raw1);

                    }
                    sumArea_waste = Math.Round(sumArea_waste, 2);
                    txt_calc_rej_sqm.Text = sumArea_waste.ToString();

                }
            }
            
        }

        private void txt_calc_rej_Or_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button10.PerformClick();
            }
        }

        private void ClearTextBoxes()
        {
            Action<Control.ControlCollection> func = null;
          
            func = (controls) =>
            {
                foreach (Control control in controls)
                    if (control is TextBox)
                        (control as TextBox).Clear();
                    else
                        func(control.Controls);
            };

            func(Controls);
        }
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            ClearTextBoxes();
            DGV_Sear_details.DataSource = "";
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


       

       
        private void button11_Click(object sender, EventArgs e)
        {
            createExcel(DGV_Sear_details);
           
        }

        private void txtOrderNO_KeyPress(object sender, KeyPressEventArgs e)
        {
            Utility.onlynumber(e);
        }

        private void txtOrderBalance_KeyPress(object sender, KeyPressEventArgs e)
        {
            Utility.onlynumber(e);
        }

        private void txt_calc_rej_Or_KeyPress(object sender, KeyPressEventArgs e)
        {
            Utility.onlynumber(e);
        }

        private void tabtrack_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txtOrderNO_TextChanged(object sender, EventArgs e)
        {

        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void tabClac_Click(object sender, EventArgs e)
        {

        }

        private void combo_Calc_dep_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            
            var balanceSG = from pro in Alfak.BW_AUFTR_KOPFs
                            join itemID in Alfak.BW_AUFTR_POs on pro.ID equals itemID.ID

                            where (pro.DATUM_AB <= dateBalaneTo.Value && pro.DATUM_AB >= dateBalaneFrom.Value) && pro.AH_IDENT == 3 && itemID.POS_NR == 1
                            select new
                            {
                                Order_No = pro.ID,
                                Original_Order = pro.AH_NAME2,
                                Project = pro.AH_NAME3,
                                TQTY = Convert.ToInt32(pro.SU_STUECK),     //TQTY3
                                TLM = Convert.ToDouble(String.Format("{0:0.00}", pro.SU_LFM_FAKT)),    //TLM4
                                TSQM = Convert.ToDouble(String.Format("{0:0.00}", pro.SU_QM_FAKT)),     //TSQM5
                                GLass_Name = itemID.PROD_BEZ1 + itemID.PROD_BEZ2 + itemID.PROD_BEZ3,
                                Date=pro.DATUM_AB,

                          };

            DGV_Balanc_SG.DataSource = balanceSG;
            DGV_Balanc_SG.Columns[1].DefaultCellStyle.BackColor = Color.LightPink;
            DGV_Balanc_SG.Columns[4].DefaultCellStyle.BackColor = Color.LightPink;
            int sumqty = 0;
            for (int i = 0; i < DGV_Balanc_SG.Rows.Count; ++i)
            {
                sumqty += Convert.ToInt32(DGV_Balanc_SG.Rows[i].Cells[3].Value);

            }
            txtBalanceQty.Text = sumqty.ToString();

            decimal sumArea = 0;
            for (int i = 0; i < DGV_Balanc_SG.Rows.Count; ++i)
            {
                sumArea += Convert.ToDecimal(DGV_Balanc_SG.Rows[i].Cells[5 ].Value);

            }
            
            txtBalanceSQM.Text = sumArea.ToString();

            decimal sumLM = 0;
            for (int i = 0; i < DGV_Balanc_SG.Rows.Count; ++i)
            {
                sumLM += Convert.ToDecimal(DGV_Balanc_SG.Rows[i].Cells[4].Value);

            }

            txt_LM.Text = sumLM.ToString();

            //chart1.Series.Clear();

            for (int i = 0; i < DGV_Balanc_SG.Rows.Count; ++i)
            {
                this.chart1.Series[0].Points.AddXY((DGV_Balanc_SG.Rows[i].Cells[1].Value.ToString()), (DGV_Balanc_SG.Rows[i].Cells[4].Value));
              
            }




        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            createExcel(DGV_Balanc_SG);
        }

        private void tabOrder_Click(object sender, EventArgs e)
        {

        }
    }
}