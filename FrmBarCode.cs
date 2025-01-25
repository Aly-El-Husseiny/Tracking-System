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
    public partial class FrmBarCode : Form
    {
        TrackingDataContext trackdb = new TrackingDataContext();

        public FrmBarCode()
        {
            InitializeComponent();
        }

        private void FrmBarCode_Load(object sender, EventArgs e)
        {
            var addclient = from add in trackdb.Orders
                           select add;

            foreach (Order add in addclient)
            {
                if (!listBox1.Items.Contains(add.Clinet_Name))
                {
                    listBox1.Items.Add(add.Clinet_Name);
                }
            }


        }

        private void listBox1_MouseClick(object sender, MouseEventArgs e)
        {
            var project = from pro in trackdb.Orders
                          where pro.Clinet_Name == listBox1.SelectedItem.ToString()
                          select pro;
            foreach (Order pro in project)
            {
                listBox2.Items.Clear();
                if (!listBox2.Items.Contains(pro.Project_Name))
                {
                    listBox2.Items.Add(pro.Project_Name);
                }
            }

           
        }

        private void listBox2_MouseClick(object sender, MouseEventArgs e)
        {
            var orderno = from ord in trackdb.Orders
                          where ord.Project_Name == listBox2.SelectedItem.ToString()
                          select ord;
            foreach (Order ord in orderno)
            {
                listBox3.Items.Clear();
                listBox3.Items.Add(ord.OC_ID);
            }
        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            var details = from det in trackdb.Orders
                          where det.OC_ID== int.Parse(listBox3.SelectedItem.ToString())
                          select det;
            foreach(Order det in details)
            {
                txt_FullDesc.Text = det.Descreption;
                txt_TLM.Text = det.TLM.ToString();
                txt_TQTY.Text = det.TQTY.ToString();
                txt_TSQM.Text = det.TSQM.ToString();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var details = from det in trackdb.Orders
                          where det.OC_ID == int.Parse(txt_wo_bar.Text)
                          select det;
            foreach (Order det in details)
            {
                txt_FullDesc.Text = det.Descreption;
                txt_TLM.Text = det.TLM.ToString();
                txt_TQTY.Text = det.TQTY.ToString();
                txt_TSQM.Text = det.TSQM.ToString();
                listBox1.Items.Clear();
                listBox1.Items.Add(det.Clinet_Name.ToString());
                listBox2.Items.Clear();
                listBox2.Items.Add(det.Project_Name.ToString());
                listBox3.Items.Clear();
                listBox3.Items.Add(det.OC_ID.ToString()); ;

            }

        }

        private void txt_wo_bar_KeyPress(object sender, KeyPressEventArgs e)
        {
            Utility.onlynumber(e);
        }
    }
}
