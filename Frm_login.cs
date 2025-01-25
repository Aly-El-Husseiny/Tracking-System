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
    public partial class Frm_login : Form
    {
        public Frm_login()
        {
            InitializeComponent();
        }

        private void txtLogin_Enter(object sender, EventArgs e)
        {

        }

        private void txtLogin_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void txtLogin_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void txtLogin_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            this.Hide();
            MDItrack MDI = new MDItrack();

            MDI.Show();


            MDI.LabelTextDepartment = "smart Glass - Cutting Department - ";



        }
    }
}
