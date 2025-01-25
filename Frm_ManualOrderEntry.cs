using DevExpress.XtraExport;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.Charts.Native;
using DevExpress.DXCore.Controls.XtraLayout.Customization.Controls;
using DevExpress.XtraPrinting.Native;
using Microsoft.Office.Interop.Excel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;
using DataTable = System.Data.DataTable;
using TextBox = Microsoft.Office.Interop.Excel.TextBox;
using System.Web.Services.Description;

namespace Cutting
{
    public partial class FrmManualOrderEntry : Form
    {
        readonly TrackingDataContext _orderEnter = new TrackingDataContext();
        readonly DataTable _table = new DataTable();
        private int _itemNo = 1;
        public FrmManualOrderEntry()
        {
            InitializeComponent();
        }

        private void Frm_ManualOrderEntry_Load(object sender, EventArgs e)
        {
            if (comb_GlassType.ComboBox != null)
            {
                comb_GlassType.ComboBox.DataSource = _orderEnter.GlassTypes.ToList();
                comb_GlassType.ComboBox.ValueMember = "Glass_ID";
                comb_GlassType.ComboBox.DisplayMember = "Glass_Type";
                comb_GlassType.ComboBox.Text = string.Empty;
                comb_GlassType.ComboBox.BindingContext = this.BindingContext;
            }
            _table.Columns.Add("No", typeof(int));
            _table.Columns.Add("Qty", typeof(int));
            _table.Columns.Add("Width", typeof(decimal));
            _table.Columns.Add("Hight", typeof(decimal));
            _table.Columns.Add("SQM", typeof(decimal));
            _table.Columns.Add("LM", typeof(decimal));
            DGV_Item.DataSource = _table;
        }

        private void tv_Description_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void btn_Build_Click(object sender, EventArgs e)
        {
            if (txt_FullDesc.Text != "")
            {
                TreeNode newNode = new TreeNode(txt_FullDesc.Text);
                tv_Description.Nodes.Add(newNode);
            }
            else
                MessageBox.Show(@"please enter description first");

        }

        private void mnu_Remove_Click(object sender, EventArgs e)
        {
            tv_Description.SelectedNode?.Remove();

        }

        private void comb_GlassType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tv_Description.SelectedNode != null)
            {
                TreeNode newSubNode = new TreeNode(comb_GlassType.Text);
                tv_Description.SelectedNode.Nodes.Add(newSubNode);
            }
            else
            {
                TreeNode newNode = new TreeNode(txt_FullDesc.Text);
                tv_Description.Nodes.Add(newNode);
            }
            descMenu.Close();
        }

        private void comb_Process_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tv_Description.SelectedNode != null)
            {
                TreeNode newSubNode = new TreeNode(comb_Process.Text);
                tv_Description.SelectedNode.Nodes.Add(newSubNode);
            }
            else
            {
                TreeNode newNode = new TreeNode(txt_FullDesc.Text);
                tv_Description.Nodes.Add(newNode);
            }
            descMenu.Close();
        }

        private void btn_addItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (txt_qty.Text.IsEmpty() || txt_width.Text.IsEmpty() || txt_height.Text.IsEmpty())
                    throw new ArgumentException("fields cannot be empty");

                _table.Rows.Add(_itemNo, txt_qty.Text, txt_width.Text, txt_height.Text, txt_sqm.Text, txt_lm.Text);
                DGV_Item.DataSource = _table;
                ++_itemNo;

                int tqty = 0;
                decimal tsqm = 0;
                decimal tlm = 0;

                foreach (DataGridViewRow row in DGV_Item.Rows)
                {
                    tqty += int.Parse(row.Cells[1].Value.ToString());
                    tsqm += decimal.Parse(row.Cells[4].Value.ToString());
                    tlm += decimal.Parse(row.Cells[5].Value.ToString());
                }

                txt_TQTY.Text = tqty.ToString();
                txt_TSQM.Text = (tsqm * tqty).ToString();
                txt_TLM.Text = (tlm * tqty).ToString();
                txt_TWeight.Text = "";

            }
            catch (ArgumentException ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Input Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            var grindProcess = _orderEnter.Processes.SingleOrDefault(x => x.Process_Name == comboGrindProcess.Text);
            var printProcess = _orderEnter.Processes.SingleOrDefault(x => x.Process_Name == comboPrintProcess.Text);
            var lGProcess = _orderEnter.Processes.SingleOrDefault(x => x.Process_Name == comboLGProcess.Text);
            var iguProcess = _orderEnter.Processes.SingleOrDefault(x => x.Process_Name == comboIGUProcess.Text);
            var bondingProcess = _orderEnter.Processes.SingleOrDefault(x => x.Process_Name == comboBondingProcess.Text);

            int orderNumber = 0;
            var lastOrder = _orderEnter.Orders.AsEnumerable().IsEmpty() ? null : _orderEnter.Orders.AsEnumerable().Last();
            if (lastOrder == null)
                orderNumber = 10000;
            else
                orderNumber += lastOrder.OC_ID;

            Order order = new Order();

            order.OC_ID = orderNumber;
            order.Clinet_Name = txt_Customer.Text;
            order.Project_Name = txt_Project.Text;
            order.Descreption = txt_FullDesc.Text;
            order.TQTY = int.Parse(txt_TQTY.Text);
            order.TSQM = decimal.Parse(txt_TSQM.Text);
            order.TLM = decimal.Parse(txt_TLM.Text);
            //ToDo: Save tree of description with pos node# 

            order.TQTY_Delivered = 0;
            order.TSQM_Delivered = 0;
            order.TLM_Delivered = 0;
            //  add.Total_Invoice = 0;

            order.Status = "Waiting";

            if (grindProcess != null) order.Grind_Type = comboGrindProcess.Text.Length == 0 ? 0 : grindProcess.ID;
            if (printProcess != null) order.Print_type = comboPrintProcess.Text.Length == 0 ? 0 : printProcess.ID;
            if (lGProcess != null) order.LG_Type = comboLGProcess.Text.Length == 0 ? 0 : lGProcess.ID;
            if (iguProcess != null) order.IGU_type = comboIGUProcess.Text.Length == 0 ? 0 : iguProcess.ID;
            if (bondingProcess != null) order.Bonding_Type = comboBondingProcess.Text.Length == 0 ? 0 : bondingProcess.ID;

            order.Category = 8;

            if (checkBalance.Checked)
                order.Balance = true;

            _orderEnter.Orders.InsertOnSubmit(order);

            //ToDo: foreach class ID add this items

            for (int i = 0; i < DGV_Item.Rows.Count; i++)
            {
                ITEM addItem = new ITEM();
                addItem.OC_ID = orderNumber;
                addItem.Item_ID = int.Parse(DGV_Item.Rows[i].Cells[0].Value.ToString());
                addItem.Width = float.Parse(DGV_Item.Rows[i].Cells[2].Value.ToString());
                addItem.Hieght = float.Parse(DGV_Item.Rows[i].Cells[3].Value.ToString());
                addItem.QTY_Item = int.Parse(DGV_Item.Rows[i].Cells[1].Value.ToString());
                //addItem.Glass_ID = int.Parse(DGV_Item.Rows[i].Cells[5].Value.ToString());
                //addItem.Pos = int.Parse(DGV_Item.Rows[i].Cells[6].Value.ToString());

                _orderEnter.ITEMs.InsertOnSubmit(addItem);
            }
            _orderEnter.SubmitChanges();
            //var ord = from oc in _orderEnter.Orders
            //          where oc.OC_ID == int.Parse(txt_OD_ID.Text)
            //          select oc;
            //foreach (var od in ord)
            //{
            //    if (od.LG_Type == 0 && od.IGU_type == 0 && od.Bonding_Type != 0) { od.Category = 4; }
            //    else if (od.LG_Type != 0 && od.IGU_type == 0 && od.Bonding_Type != 0) { od.Category = 5; }
            //    else if (od.LG_Type == 0 && od.IGU_type != 0 && od.Bonding_Type != 0) { od.Category = 6; }
            //    else if (od.LG_Type == 0 && od.IGU_type != 0 && od.Bonding_Type == 0) { od.Category = 3; }
            //    else if (od.LG_Type != 0 && od.IGU_type != 0 && od.Bonding_Type == 0) { od.Category = 7; }
            //    else if (od.LG_Type != 0 && od.IGU_type == 0 && od.Bonding_Type == 0) { od.Category = 2; }
            //    else if (od.LG_Type == 0 && od.IGU_type == 0 && od.Bonding_Type == 0) { od.Category = 1; }
            //}
            //_orderEnter.SubmitChanges();

            MessageBox.Show(@"The Order saved");

            btn_Save.Enabled = false;

        }

        private void txt_height_TextChanged(object sender, EventArgs e)
        {
            CalculateSqmAndLm(txt_width.Text, txt_height.Text);
        }

        private void txt_width_TextChanged(object sender, EventArgs e)
        {
            CalculateSqmAndLm(txt_width.Text, txt_height.Text);
        }

        private void CalculateSqmAndLm(string width, string height)
        {
            if (!(width.IsEmpty() || height.IsEmpty()))
            {
                decimal sqm = decimal.Parse(width) * decimal.Parse(height) / 1000000;
                txt_sqm.Text = sqm.ToString(CultureInfo.InvariantCulture);

                var lm = (2 * (double.Parse(width) + double.Parse(height))) / 1000;
                txt_lm.Text = lm.ToString(CultureInfo.InvariantCulture);
            }
        }

        private void txt_qty_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow control characters like Backspace
            if (char.IsControl(e.KeyChar))
            {
                return;
            }

            // Allow digits and the decimal point
            if (!char.IsDigit(e.KeyChar))
            {
                e.Handled = true;  // Ignore the character if it's not a digit or decimal point
            }
        }

        private void txt_width_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow control characters like Backspace
            if (char.IsControl(e.KeyChar))
            {
                return;
            }

            // Allow digits and the decimal point
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;  // Ignore the character if it's not a digit or decimal point
            }

            // Ensure only one decimal point is allowed
            if (e.KeyChar == '.' && txt_width.Text.Contains("."))
            {
                e.Handled = true;  // Ignore the second decimal point
            }
        }

        private void txt_height_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow control characters like Backspace
            if (char.IsControl(e.KeyChar))
            {
                return;
            }

            // Allow digits and the decimal point
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;  // Ignore the character if it's not a digit or decimal point
            }

            // Ensure only one decimal point is allowed
            if (e.KeyChar == '.' && txt_height.Text.Contains("."))
            {
                e.Handled = true;  // Ignore the second decimal point
            }
        }

        private void btn_Search_Click(object sender, EventArgs e)
        {
            var order = _orderEnter.Orders.FirstOrDefault(o => o.OC_ID == int.Parse(txt_OD_ID.Text));
            if (order != null)
            {
                txt_Customer.Text = order.Clinet_Name;
                txt_Project.Text = order.Project_Name;
                txt_Note.Text = "";
                txt_FullDesc.Text = order.Descreption;
                txt_TQTY.Text = order.TQTY.ToString();
                txt_TSQM.Text = order.TSQM.ToString();
                txt_TLM.Text = order.TLM.ToString();

                //comboGrindProcess.SelectedItem = order.Grind_Type;
                var grindProcess = _orderEnter.Processes.FirstOrDefault(o => o.ID == order.Grind_Type).Process_Name;
                var printProcess = _orderEnter.Processes.FirstOrDefault(o => o.ID == order.Print_type).Process_Name;
                var lGProcess = _orderEnter.Processes.FirstOrDefault(o => o.ID == order.LG_Type).Process_Name;
                var iguProcess = _orderEnter.Processes.FirstOrDefault(o => o.ID == order.IGU_type).Process_Name;
                var bondingProcess = _orderEnter.Processes.FirstOrDefault(o => o.ID == order.Bonding_Type).Process_Name;

                if (grindProcess != null)
                    comboGrindProcess.Text = grindProcess;
                if (printProcess != null)
                    comboPrintProcess.Text = printProcess;
                if (lGProcess != null)
                    comboLGProcess.Text = lGProcess; 
                if (iguProcess != null)
                    comboIGUProcess.Text = iguProcess; 
                if (bondingProcess != null)
                    comboBondingProcess.Text = bondingProcess; 
            }
            else
                MessageBox.Show("this order not found", "search error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
