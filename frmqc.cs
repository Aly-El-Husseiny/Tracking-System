using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Excel = Microsoft.Office.Interop.Excel;






namespace Cutting
{

    public partial class frmqc : Form
    {

        // link to DB by linq
        TrackingDataContext qcdb = new TrackingDataContext();


        public frmqc()
        {
            InitializeComponent();
        }

        private void frmqc_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'trackingDataSet1.QC' table. You can move, or remove it, as needed.
            //this.qCTableAdapter1.Fill(this.trackingDataSet1.QC);
            // TODO: This line of code loads data into the 'trackingDataSet.QC' table. You can move, or remove it, as needed.
            //this.qCTableAdapter.Fill(this.trackingDataSet.QC);
            // TODO: This line of code loads data into the 'trackingDataSet.QC' table. You can move, or remove it, as needed.
            //this.qCTableAdapter.Fill(this.trackingDataSet.QC);
            // TODO: This line of code loads data into the 'trackingDataSet.Department' table. You can move, or remove it, as needed.
            // this.departmentTableAdapter.Fill(this.trackingDataSet.Department);


            //Cutting.TrackingDataContext comdep = new TrackingDataContext();
            comboDep.DataSource = qcdb.Departments.ToList();
            comboDep.ValueMember = "Departmet_ID";
            comboDep.DisplayMember = "Department_Name";
            comboDep.Text = "";


            //Cutting.TrackingDataContext comrej = new TrackingDataContext();
            comboRej.DataSource = qcdb.QC_rejs.ToList();
            comboRej.ValueMember = "Rej_ID";
            comboRej.DisplayMember = "Rej_Name";
            comboRej.Text = "";

            comboGlass.DataSource = qcdb.GlassTypes.ToList();
            comboGlass.ValueMember = "Glass_ID";
            comboGlass.DisplayMember = "Glass_Type";
            comboGlass.Text = "";

            txtOrderNO.Text = "";

            txtLG.Enabled = false;
            panel1.Enabled = false;
            panel2.Enabled = false;
            btnupdate.Enabled = false;
            btndelete.Enabled = false;
            btnSave.Enabled = false;
            btnclear.Enabled = false;
            dataGridView1.Enabled = false;
           // btnRemove.Enabled = false;
            comboDep.SelectedIndex = -1;
            comboGlass.SelectedIndex = -1;
            combo_correc.SelectedIndex = -1;
            comboRej.SelectedIndex = -1;

            QCRefresh();


        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            // add item to compobox
            var department = from dep in qcdb.Departments
                             select new { dep.Department_Name };


            emptytxtbox();



            comboGlass.Enabled = true;
            combo_correc.Enabled = true;
            comboDep.Enabled = true;
            txtLG.Enabled = false;
            btnNew.Enabled = false;
            panel1.Enabled = true;
            btnupdate.Enabled = false;
            //btndelete.Enabled = true;
            btnSave.Enabled = true;
            btnclear.Enabled = true;
            dataGridView1.Enabled = true;


        }

        private void btnSearchfrom_Click(object sender, EventArgs e)
        {

            panel1.Enabled = true;
            btnclear.Enabled = true;
            dataGridView1.Enabled = true;
            btnSave.Enabled = true;
            btnsearch.Enabled = true;

        }

        private void btnSearchfrom_Click_1(object sender, EventArgs e)
        {
            panel1.Enabled = true;

            panel2.Enabled = true;
            btnSave.Enabled = false;


        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var DepName = qcdb.Departments.Single(Name => Name.Department_Name == comboDep.Text);
            var RejectID = qcdb.QC_rejs.Single(Name => Name.Rej_Name == comboRej.Text);
            var Glass = qcdb.GlassTypes.Single(Name => Name.Glass_Type == comboGlass.Text);
           var TrackId = qcdb.QCs.Single(TrkID => TrkID.ID == int.Parse(txtid.Text));
            QC add = new QC();

            if (txtproject.Text == "" || txtHeight.Text == "" || txtItem.Text == "" || txtOrderNO.Text == "" || txtproject.Text == "" ||
            txtQTY.Text == "" || txtwidth.Text == "" || comboDep.Text == "" || comboRej.Text == "")
            {
                MessageBox.Show(@"Please complete the missing data");

            }
            else
            {
                add.Project = txtproject.Text;
                add.OC_ID = txtOrderNO.Text;
                add.Glass_ID = Glass.Glass_ID;
                add.Item = int.Parse(txtItem.Text);
                add.Width = int.Parse(txtwidth.Text);
                add.Height = int.Parse(txtHeight.Text);
                add.Remarks = txtremark.Text;
                add.QTY = int.Parse(txtQTY.Text);
                add.Rej_Reason = txtreason.Text;
                add.Action = combo_correc.Text;
                add.Reject_Date = DateTime.Today;
                add.Reject_Time = DateTime.Today.TimeOfDay;
                add.Area = decimal.Parse(txtArea.Text);
                add.Departmet_ID = DepName.Departmet_ID;
                add.Rej_ID = RejectID.Rej_ID;
               add.Track_ID = TrackId.Track_ID;


                qcdb.QCs.InsertOnSubmit(add);
                qcdb.SubmitChanges();
                ////////////////////////////////////////////////
                var glassid = qcdb.GlassTypes.Single(id => id.Glass_Type == comboGlass.Text);

                Track balance = new Track();

                if (combo_correc.Text == "Cut again")
                {
                    DialogResult result = MessageBox.Show("Do you want to send this item to Cutting department ?", "Cut Again Item", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        balance.Departmet_ID = 1;
                        balance.OC_ID = int.Parse(txtOrderNO.Text);
                        balance.Item_ID = int.Parse(txtItem.Text);
                        balance.Glass_ID = glassid.Glass_ID;
                        balance.QTY_Recive = int.Parse(txtQTY.Text);
                        balance.QTY_ToDo = int.Parse(txtQTY.Text);
                        balance.Recived_From = 9;
                        balance.Balance = true;
                        balance.Pos = int.Parse(txt_Pos.Text);
                        balance.Date = DateTime.Today;
                        qcdb.Tracks.InsertOnSubmit(balance);
                        qcdb.SubmitChanges();

                        /////// add waste to item table
                        var waste = from id in qcdb.ITEMs
                                    where id.OC_ID == int.Parse(txtOrderNO.Text) && id.Item_ID == int.Parse(txtItem.Text)
                                 && id.Glass_ID == glassid.Glass_ID && id.Pos == int.Parse(txt_Pos.Text)
                                    select id;
                        foreach (ITEM it in waste)
                        {
                            it.Waste_QTY = it.Waste_QTY + 1;
                            qcdb.SubmitChanges();
                        }
                        ////////////
                        MessageBox.Show(@"This Item will be cut again");
                        emptytxtbox();

                    }
                }


                if (combo_correc.Text == "Reprocess")
                {
                    DialogResult result = MessageBox.Show("Do you want to send this item to Same department again ?", "Reprocess Item", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        var DepNo = qcdb.Departments.Single(id => id.Department_Name == comboDep.Text);
                        balance.Departmet_ID = DepNo.Departmet_ID;
                        balance.OC_ID = int.Parse(txtOrderNO.Text);
                        balance.Item_ID = int.Parse(txtItem.Text);
                        balance.Glass_ID = glassid.Glass_ID;
                        balance.QTY_Recive = int.Parse(txtQTY.Text);
                        balance.QTY_ToDo = int.Parse(txtQTY.Text);
                        balance.Recived_From = 9;
                        balance.Pos = int.Parse(txt_Pos.Text);
                        balance.Date = DateTime.Today;
                        qcdb.Tracks.InsertOnSubmit(balance);
                        qcdb.SubmitChanges();
                        MessageBox.Show(@"This Item will be Reprocess");
                        emptytxtbox();
                    }
                }

           

            ////////////////////////////////////////////////
           // MessageBox.Show("The Inspection case was saved");

                                     
                emptytxtbox();


                panel1.Enabled = false;
                panel2.Enabled = false;
                btnupdate.Enabled = false;
                btndelete.Enabled = false;
                btnSave.Enabled = false;
                btnclear.Enabled = false;
                dataGridView1.Enabled = false;
                btnNew.Enabled = true;
            }




        }

        private void emptytxtbox()
        {
            txtArea.Text = "";
            combo_correc.Text = "";
            comboGlass.Text = "";
            txtHeight.Text = "";
            txtItem.Text = "";
            txtOrderNO.Text = "";
            txtproject.Text = "";
            txtQTY.Text = "";
            txtreason.Text = "";
            txtremark.Text = "";
            txtwidth.Text = "";
            comboDep.Text = "";
            comboRej.Text = "";
            comboDep.SelectedIndex = -1;
            comboGlass.SelectedIndex = -1;
            combo_correc.SelectedIndex = -1;
            comboRej.SelectedIndex = -1;

        }

        private void btnsearch_Click(object sender, EventArgs e)
        {
            var qcid = from id in qcdb.QCs
                       join dep in qcdb.Departments on id.Departmet_ID equals dep.Departmet_ID
                       join glass in qcdb.GlassTypes on id.Glass_ID equals glass.Glass_ID
                       join rej in qcdb.QC_rejs on id.Rej_ID equals rej.Rej_ID
                       where
              (id.Reject_Date <= dateTo.Value && id.Reject_Date >= datefrom.Value) &&
                (string.IsNullOrEmpty(txtproject.Text) || id.Project == txtproject.Text) &&
               (string.IsNullOrEmpty(txtOrderNO.Text) || id.OC_ID == (txtOrderNO.Text)) &&
                (string.IsNullOrEmpty(txtreason.Text) || id.Rej_Reason == txtreason.Text) &&
                (string.IsNullOrEmpty(comboDep.Text) || dep.Department_Name == comboDep.Text) &&
                (string.IsNullOrEmpty(comboGlass.Text) || glass.Glass_Type == comboGlass.Text)&&
                       (string.IsNullOrEmpty(comboRej.Text) || rej.Rej_Name == comboRej.Text)

                       select new
                       {
                           Rej_Date=id.Reject_Date,
                           Rej_time=id.Reject_Time,
                           Order = id.OC_ID,
                           Project = id.Project,
                           Glass_Type=glass.Glass_Type,
                           Item=id.Item,
                           Widht=id.Width,
                           Height=id.Height,
                           QTy=id.QTY,
                           Area=id.Area,
                           Department=dep.Department_Name,
                           Rejection=rej.Rej_Name,
                           Reason=id.Rej_Reason,
                           Remark=id.Remarks,
                           Action =id.Action,
                           Action_Date=id.Action_Date,
                           Action_Time=id.Action_time,
                           ID=id.ID

                       };
            dataGridView1.DataSource = qcid;


            dataGridView1.Enabled = true;
            btnNew.Enabled = true;

            double sumarea = 0;
            for (int i = 0; i < dataGridView1.Rows.Count; ++i)
            {
                sumarea += Convert.ToDouble(dataGridView1.Rows[i].Cells[9].Value);

            }

            txttarea.Text = sumarea.ToString();


            int sumqty = 0;
            for (int i = 0; i < dataGridView1.Rows.Count; ++i)
            {
                sumqty += Convert.ToInt32(dataGridView1.Rows[i].Cells[8].Value);

            }
            txttqty.Text = sumqty.ToString();







        }

        private void fillGrid()
        {

            var qcid = from id in qcdb.QCs
                       where id.Reject_Date <= dateTo.Value && id.Reject_Date >= datefrom.Value


                       //  where id.OC_ID == Convert.ToInt32(txtOrderNO.Text)

                       select id;


            dataGridView1.DataSource = qcid;


        }



        private void txtOrderNO_KeyPress(object sender, KeyPressEventArgs e)
        {
            onlynumber(e);
        }

        public static void onlynumber(KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!char.IsDigit(ch) && ch != 8 && ch != 46)
            {
                e.Handled = true;

            }
        }

        private void txtItem_TextChanged(object sender, EventArgs e)
        {


        }

        private void txtItem_KeyPress(object sender, KeyPressEventArgs e)
        {
            onlynumber(e);

        }

        private void txtQTY_KeyPress(object sender, KeyPressEventArgs e)
        {
            onlynumber(e);



        }

        private void txtwidth_TextChanged(object sender, EventArgs e)
        {


        }

        private void txtwidth_KeyPress(object sender, KeyPressEventArgs e)
        {
            onlynumber(e);

        }

        private void txtHeight_KeyPress(object sender, KeyPressEventArgs e)
        {
            onlynumber(e);

        }

        private void txtArea_TextChanged(object sender, EventArgs e)
        {



        }

        private void txtQTY_TextChanged(object sender, EventArgs e)
        {



        }

        private void txtQTY_MouseEnter(object sender, EventArgs e)
        {

        }

        private void txtQTY_DragEnter(object sender, DragEventArgs e)
        {

        }

        private void txtQTY_MouseLeave(object sender, EventArgs e)
        {
            //txtArea.Text = "";
            //Int32 val1 = Convert.ToInt32(txtHeight.Text);
            //Int32 val2 = Convert.ToInt32(txtwidth.Text);
            //Int32 val3 = Convert.ToInt32(txtQTY.Text);
            //Int32 val4 = val1 * val2 * val3;
            //decimal valtotal = Convert.ToDecimal(val4) / 1000000;
            //txtArea.Text = valtotal.ToString();




        }

        private void btnclear_Click(object sender, EventArgs e)
        {
            emptytxtbox();
            comboDep.SelectedIndex = -1;
            comboGlass.SelectedIndex = -1;
            combo_correc.SelectedIndex = -1;
            comboRej.SelectedIndex = -1;
        }

        private void txtQTY_Leave(object sender, EventArgs e)
        {
            if (txtQTY.Text == "" || txtHeight.Text == "" || txtwidth.Text == "")
            {
                txtArea.Text = "0";
            }
            else
            {
                txtArea.Text = "";
                Int32 val1 = Convert.ToInt32(txtHeight.Text);
                Int32 val2 = Convert.ToInt32(txtwidth.Text);
                Int32 val3 = Convert.ToInt32(txtQTY.Text);
                Int32 val4 = val1 * val2 * val3;
                decimal valtotal = Convert.ToDecimal(val4) / 1000000;
                txtArea.Text = valtotal.ToString();
            }
        }



        private void txtQTY_KeyDown(object sender, KeyEventArgs e)
        {


        }

        private void txtQTY_Enter(object sender, EventArgs e)
        {



        }

        private void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {

            btnclear.Enabled = true;
            btndelete.Enabled = true;
            btnupdate.Enabled = true;
            panel1.Enabled = true;
            btnSave.Enabled = false;
            btnNew.Enabled = true;
            
            txtid.Text = dataGridView1.SelectedRows[0].Cells[17].Value.ToString();
            txtproject.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
            txtOrderNO.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            comboGlass.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
            txtItem.Text = dataGridView1.SelectedRows[0].Cells[5].Value.ToString();
            txtwidth.Text = dataGridView1.SelectedRows[0].Cells[6].Value.ToString();
            txtHeight.Text = dataGridView1.SelectedRows[0].Cells[7].Value.ToString();
            txtQTY.Text = dataGridView1.SelectedRows[0].Cells[8].Value.ToString();
            txtArea.Text = dataGridView1.SelectedRows[0].Cells[9].Value.ToString();
            comboDep.Text = dataGridView1.SelectedRows[0].Cells[10].Value.ToString();
            comboRej.Text = dataGridView1.SelectedRows[0].Cells[11].Value.ToString();
            combo_correc.Text = dataGridView1.SelectedRows[0].Cells[14].Value.ToString();
            txtreason.Text= dataGridView1.SelectedRows[0].Cells[12].Value.ToString();
            txtremark.Text = dataGridView1.SelectedRows[0].Cells[13].Value.ToString();



        }




        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnupdate_Click(object sender, EventArgs e)
        {
            
            //var Glass = qcdb.GlassTypes.Single(Name => Name.Glass_Type == comboGlass.Text);
            var query = from up in qcdb.QCs
                        where up.ID == int.Parse(txtid.Text)
                        select up;
            
            if (txtArea.Text == "")
                MessageBox.Show(@"Please insert the Area");
            else if (txtLG.Enabled==true)
                    {
                Track balanceLG = new Track();
                balanceLG.Departmet_ID = 6;
                balanceLG.OC_ID = int.Parse(txtOrderNO.Text);
                balanceLG.Item_ID = int.Parse(txtItem.Text);
                balanceLG.LG = txtLG.Text;
                balanceLG.QTY_Recive = int.Parse(txtQTY.Text);
                balanceLG.QTY_ToDo = int.Parse(txtQTY.Text);
                balanceLG.Recived_From = 9;
                balanceLG.Balance = true;
                balanceLG.Pos = int.Parse(txt_Pos.Text);
               // balanceLG.Date = DateTime.Today;
                qcdb.Tracks.InsertOnSubmit(balanceLG);
                qcdb.SubmitChanges();
                MessageBox.Show(@"This Item will be send to IGU");
                emptytxtbox();
                QCRefresh();
                txtLG.Text = "";
            }
            else
            {
                var Glass = qcdb.GlassTypes.Single(Name => Name.Glass_Type == comboGlass.Text);
                var DepID=qcdb.Departments.Single(Name => Name.Department_Name == comboDep.Text);
                foreach (QC up in query)
                {
                    //DateTime.Now.Date.ToString("yyyy-MM-dd HH:mm:ss"));
                    up.Project = txtproject.Text;
                    up.OC_ID = (txtOrderNO.Text);
                    up.Departmet_ID = DepID.Departmet_ID;
                    up.Glass_ID = Glass.Glass_ID;
                    up.Item = int.Parse(txtItem.Text);
                    up.Width = int.Parse(txtwidth.Text);
                    up.Height = int.Parse(txtHeight.Text);
                    up.Remarks = txtremark.Text;
                    up.QTY = int.Parse(txtQTY.Text);
                    up.Rej_Reason = txtreason.Text;
                    up.Action = combo_correc.Text;
                    up.Action_Date = DateTime.Today;
                    up.Action_time = DateTime.Now.TimeOfDay;
                    up.Area = decimal.Parse(txtArea.Text);
                    qcdb.SubmitChanges();
                }
                

                qcdb.SubmitChanges();
                MessageBox.Show(@"The Inspection case was Updated");
                var glassid = qcdb.GlassTypes.Single(id => id.Glass_Type == comboGlass.Text);

                Track balance = new Track();
                
                if (combo_correc.Text == "Cut again")
                {
                    DialogResult result = MessageBox.Show("Do you want to send this item to Cutting department ?", "Cut Again Item", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        balance.Departmet_ID = 1;
                        balance.OC_ID = int.Parse(txtOrderNO.Text);
                        balance.Item_ID = int.Parse(txtItem.Text);
                        balance.Glass_ID = glassid.Glass_ID;
                        balance.QTY_Recive = int.Parse(txtQTY.Text);
                        balance.QTY_ToDo = int.Parse(txtQTY.Text);
                        balance.Recived_From = 9;
                        balance.Balance = true;
                        balance.Pos = int.Parse(txt_Pos.Text);
                        balance.Date = DateTime.Today;
                        qcdb.Tracks.InsertOnSubmit(balance);
                        qcdb.SubmitChanges();


                        /////// add waste to item table
                        var waste = from id in qcdb.ITEMs
                                    where id.OC_ID == int.Parse(txtOrderNO.Text) && id.Item_ID == int.Parse(txtItem.Text)
                                 && id.Glass_ID == glassid.Glass_ID && id.Pos == int.Parse(txt_Pos.Text)
                                    select id;
                        foreach (ITEM it in waste)
                        {
                            it.Waste_QTY = it.Waste_QTY + 1;
                            qcdb.SubmitChanges();
                        }
                                ////////////

                                MessageBox.Show(@"This Item will be cut again");
                        emptytxtbox();
                        QCRefresh();
                    }
                   
                }
               

                if (combo_correc.Text == "Reprocess" || combo_correc.Text == "Acceptable")
                {
                    DialogResult result = MessageBox.Show("Do you want to send this item to Same department again ?", "Reprocess Item", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        var DepNo = qcdb.Departments.Single(id => id.Department_Name == comboDep.Text);
                        balance.Departmet_ID = DepNo.Departmet_ID;
                        balance.OC_ID = int.Parse(txtOrderNO.Text);
                        balance.Item_ID = int.Parse(txtItem.Text);
                        balance.Glass_ID = glassid.Glass_ID;
                        balance.QTY_Recive = int.Parse(txtQTY.Text);
                        balance.QTY_ToDo = int.Parse(txtQTY.Text);
                        balance.Recived_From = 9;
                        balance.Pos = int.Parse(txt_Pos.Text);
                        balance.Date = DateTime.Today;
                        qcdb.Tracks.InsertOnSubmit(balance);
                        qcdb.SubmitChanges();
                        MessageBox.Show(@"This Item will be Reprocess");
                        emptytxtbox();
                        QCRefresh();
                    }
                    
                }
                
            }
            var inspection = from ins in qcdb.QCs
                             join dep in qcdb.Departments on ins.Departmet_ID equals dep.Departmet_ID
                             where ins.Action == "" || ins.Action == null
                             select new

                             {
                                 inspiction_ID = ins.ID,

                                 Department = dep.Department_Name,

                             };

            DGV_QC_ins.DataSource = inspection;
            QCRefresh();
        }

        private void txtid_TextChanged(object sender, EventArgs e)
        {

        }

        private void btndelete_Click(object sender, EventArgs e)
        {
            var query = from del in qcdb.QCs
                        where del.ID == int.Parse(txtid.Text)
                        select del;

            foreach (QC del in query)
            {
                qcdb.QCs.DeleteOnSubmit(del);
            }


            DialogResult result = MessageBox.Show("Are you sure you want to delete this ?", "Delete Inspection", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                qcdb.SubmitChanges();
            }
            emptytxtbox();
            fillGrid();



        }

        private void btnprint_Click(object sender, EventArgs e)
        {
           
        }

        private void comboDep_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void chart1_Click(object sender, EventArgs e)
        {
            
        }

        private void btncalc_Click(object sender, EventArgs e)
        {
            /////// add waste to item table join iddd in trackdb.ITEMs on new { pro.OC_ID, pro.Item_ID, pro.Pos } equals new { iddd.OC_ID, iddd.Item_ID, iddd.Pos }
            //var waste = from id in qcdb.Tracks
            //          //  join qc in qcdb.ITEMs on new { id.OC_ID, id.Item_ID, id.Glass_ID, id.Pos } equals new { qc.OC_ID, qc.Item_ID, qc.Glass_ID, qc.Pos }
            //        where id.Recived_From==9 && id.Departmet_ID==1
            //            select id;
            //foreach (var it in waste)
            //{
            //    var QCWaste = from qc in qcdb.ITEMs
            //                  where qc.OC_ID == it.OC_ID && qc.Item_ID == it.Item_ID && qc.Glass_ID == it.Glass_ID && qc.Pos == it.Pos
            //                  select qc;

            //    foreach (var up in QCWaste)
            //    {
            //        up.Waste_QTY = Convert.ToInt32( up.Waste_QTY + it.QTY_Recive);
            //        qcdb.SubmitChanges();
            //    }
            //}
            ////////////
            //DepWasteReport report = new DepWasteReport();
            //report.Show();


        }

        private void btnRejWaste_Click(object sender, EventArgs e)
        {

            RejWasteReport report = new RejWasteReport();
            report.Show();
            //var AreaSum = from total in qcdb.QCs
            //              where (total.Date <= dateTo.Value && total.Date >= datefrom.Value)
            //              group total by total.Rej_ID into g

            //              select new
            //              {
            //                  Rejection = g.First().Rej_ID,
            //                  TotalArea = g.Sum(x => x.Area)

            //              };


            //dataGridView1.DataSource = AreaSum;
            //dataGridView1.Enabled = true;

        }

        private void btnchart_Click(object sender, EventArgs e)
        {
            
        }

        private void txtOrderNO_TextChanged(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void DGV_QC_ins_DoubleClick(object sender, EventArgs e)
        {
           
            var DepNo = qcdb.Departments.Single(id => id.Department_Name == DGV_QC_ins.SelectedRows[0].Cells[2].Value.ToString());

            txtid.Text = DGV_QC_ins.SelectedRows[0].Cells[0].Value.ToString();
            comboDep.Text = DepNo.Department_Name;
            var fill = (from fl in qcdb.QCs
                        join glassID in qcdb.GlassTypes on fl.Glass_ID equals glassID.Glass_ID
                        where fl.ID == int.Parse(txtid.Text)
                        select new
                        {
                            Project = fl.Project,
                            Order = fl.OC_ID,
                            glass = glassID.Glass_Type,
                            item = fl.Item,
                            wid = fl.Width,
                            hi = fl.Height,
                            qty = fl.QTY,
                            rej = fl.Rej_ID,
                            rejres = fl.Rej_Reason,
                            pos=fl.Pos,
                            // notes = fl.Rej_Reason

                        }).SingleOrDefault();
            txtproject.Text = fill.Project.ToString();
            txtOrderNO.Text = fill.Order.ToString();
            txtItem.Text = fill.item.ToString();
            txtwidth.Text = fill.wid.ToString();
            comboGlass.Text = fill.glass.ToString();
            txtHeight.Text = fill.hi.ToString();
            txtQTY.Text = fill.qty.ToString();
            txtreason.Text = fill.rejres.ToString();
            txt_Pos.Text = fill.pos.ToString();


            
            Int32 val1 = Convert.ToInt32(txtHeight.Text);
            Int32 val2 = Convert.ToInt32(txtwidth.Text);
            
            Int32 val3 = Convert.ToInt32(txtQTY.Text);
            Int32 val4 = val1 * val2 * val3;
            decimal valtotal = Convert.ToDecimal(val4) / 1000000;
            txtArea.Text = valtotal.ToString();

            btnSave.Enabled = false;
            btnupdate.Enabled = true;
        }

        private void DGV_QC_ins_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {


        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            QCRefresh();

        }

        private void QCRefresh()
        {
            var inspection = from ins in qcdb.QCs
                             join dep in qcdb.Departments on ins.Departmet_ID equals dep.Departmet_ID
                             join glassID in qcdb.GlassTypes on ins.Glass_ID equals glassID.Glass_ID
                             where (ins.Action == "" || ins.Action == null) && ins.Glass_ID != null
                             select new

                             {
                                 ID = ins.ID,
                                 Glass=glassID.Glass_Type,
                                 Department = dep.Department_Name,

                             };

            DGV_QC_ins.DataSource = inspection;

            var MultiLayerInspection = from ins in qcdb.QCs
                                       join dep in qcdb.Departments on ins.Departmet_ID equals dep.Departmet_ID
                                       where (ins.Action == "" || ins.Action == null) && ins.Glass_Desc != null
                                       select new

                                       {
                                           ID = ins.ID,
                                           Desc=ins.Glass_Desc,
                                           Department = dep.Department_Name,

                                       };


            DGV_QC_insItem.DataSource = MultiLayerInspection;
        }

        private void copyAlltoClipboardBalance()
        {

            dataGridView1.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            dataGridView1.MultiSelect = true;
            dataGridView1.SelectAll();
            DataObject dataObj = dataGridView1.GetClipboardContent();
            if (dataObj != null)
                Clipboard.SetDataObject(dataObj);

        }
        private void button1_Click(object sender, EventArgs e)
        {
            copyAlltoClipboardBalance();
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

        private void DGV_QC_insItem_DoubleClick(object sender, EventArgs e)
        {
            var DepNo = qcdb.Departments.Single(id => id.Department_Name == DGV_QC_insItem.SelectedRows[0].Cells[2].Value.ToString());

            txtid.Text = DGV_QC_insItem.SelectedRows[0].Cells[0].Value.ToString();
            comboDep.Text = DepNo.Department_Name;
            var fill = (from fl in qcdb.QCs
                        
                        where fl.ID == int.Parse(txtid.Text)
                        select new
                        {
                            Project = fl.Project,
                            Order = fl.OC_ID,
                            item = fl.Item,
                            wid = fl.Width,
                            hi = fl.Height,
                            qty = fl.QTY,
                            rej = fl.Rej_ID,
                            rejres = fl.Rej_Reason,
                            pos = fl.Pos,
                            // notes = fl.Rej_Reason

                        }).SingleOrDefault();
            txtproject.Text = fill.Project.ToString();
            txtOrderNO.Text = fill.Order.ToString();
            txtItem.Text = fill.item.ToString();
            txtwidth.Text = fill.wid.ToString(); 
            txtHeight.Text = fill.hi.ToString();
            txtQTY.Text = fill.qty.ToString();
            txtreason.Text = fill.rejres.ToString();
            txt_Pos.Text = fill.pos.ToString();
            
            Int32 val1 = Convert.ToInt32(txtHeight.Text);
            Int32 val2 = Convert.ToInt32(txtwidth.Text);
            
            Int32 val3 = Convert.ToInt32(txtQTY.Text);
            Int32 val4 = val1 * val2 * val3;
            decimal valtotal = Convert.ToDecimal(val4) / 1000000;
            txtArea.Text = valtotal.ToString();


            btnSave.Enabled = true;
            btnupdate.Enabled = false;
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (DGV_QC_insItem.SelectedRows.Count == 1)
            {

                // var DoneID= qcdb.QCs.Single(remove=>remove.ID==int.Parse( DGV_QC_ins.SelectedRows[0].Cells[0].Value.ToString()));
                var query = from up in qcdb.QCs
                            where up.ID == int.Parse(DGV_QC_insItem.SelectedRows[0].Cells[0].Value.ToString())
                            select up;
                foreach (QC up in query)
                {

                    up.Action = "Done";
                }
                qcdb.SubmitChanges();
                MessageBox.Show(@"The Inspection has been done");
                QCRefresh();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            txtLG.Enabled = true;
            comboGlass.Enabled = false;
            combo_correc.Enabled = false;
            comboDep.Enabled = false;
            btnNew.Enabled = true;
            btnupdate.Enabled = true;
            btnSave.Enabled = false;
        }

    }
}
  
    



