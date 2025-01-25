using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;

namespace Cutting
{
    public partial class MDItrack : Form
    {
        public string LabelTextDepartment
        {
            get
            {
                return this.toolStripLabel4.Text;
            }
            set
            {
                this.toolStripLabel4.Text = value;
            }
        }


        TrackingDataContext trackdb = new TrackingDataContext();

        private int childFormNumber = 0;

        public MDItrack()
        {
            InitializeComponent();
        }

        private void ShowNewForm(object sender, EventArgs e)
        {

            // link to DB by linq 16-3-2017
            TrackingDataContext qcdb = new TrackingDataContext();





            Form childForm = new Form();
            childForm.MdiParent = this;
            childForm.Text = "Window " + childFormNumber++;
            childForm.Show();
        }

        private void OpenFile(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = openFileDialog.FileName;
            }
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = saveFileDialog.FileName;
            }
        }

        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CutToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void ToolBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void StatusBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }

        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            frmqc newMDIChild = new frmqc();
            // Set the Parent Form of the Child window.
            newMDIChild.MdiParent = this;
            // Display the new form.
            newMDIChild.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            frmcut newMDIChild = new frmcut();
            // Set the Parent Form of the Child window.
            newMDIChild.MdiParent = this;
            // Display the new form.
            newMDIChild.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            frmAris newMDIChild = new frmAris();
            // Set the Parent Form of the Child window.
            newMDIChild.MdiParent = this;
            // Display the new form.
            newMDIChild.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            FRM_dispatch newF = new FRM_dispatch();
            // Set the Parent Form of the Child window.
            newF.MdiParent = this;
            // Display the new form.
            newF.Show();
        }

        private void MDItrack_Load(object sender, EventArgs e)
        {
           // pictureBox1.Visible = false;
            enableButton(false);
            if (this.txtPassword.Control is TextBox)
            {//important
                TextBox tb = this.txtPassword.Control as TextBox;
                tb.PasswordChar ='*';
            }
        


    }

    private void enableButton(bool enable)
        {
            foreach (Button button in this.Controls.OfType<Button>())
                button.Enabled = enable;

        }
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (txtLogin.Text=="")
            { MessageBox.Show(@"Please enter your ID and Password"); }
            else
            {
                var loginIn = from log in trackdb.Logins
                              join dep in trackdb.Departments on log.Departmet_ID equals dep.Departmet_ID
                              where log.ID == int.Parse(txtLogin.Text) && log.PW == txtPassword.Text
                              select log;



                foreach (Login log in loginIn)
                {
                    switch (log.Departmet_ID)
                    {
                        case 1:
                            enableButton(false);
                            button10.Enabled = true;

                            break;
                        case 2:
                            enableButton(false);
                            button2.Enabled = true;

                            break;
                        case 3:
                            enableButton(false);
                            button9.Enabled = true;

                            break;

                        case 31:
                            enableButton(false);
                            button13.Enabled = true;

                            break;

                        case 4:
                            enableButton(false);
                            button3.Enabled = true;
                            button12.Enabled = true;

                            break;
                        case 5:
                            enableButton(false);
                            button4.Enabled = true;

                            break;
                        case 6:
                            enableButton(false);
                            button5.Enabled = true;
                            button7.Enabled = true;

                            break;
                        case 7:
                            enableButton(false);
                            button6.Enabled = true;

                            break;
                        case 8:
                            enableButton(false);
                            button11.Enabled = true;


                            break;
                        case 9:
                            enableButton(false);
                            button8.Enabled = true;
                            button10.Enabled = true;

                            break;
                        case 10:
                            enableButton(false);
                            button10.Enabled = true;
                            button7.Enabled = true;
                            break;

                        case 11:
                            enableButton(false);
                            button10.Enabled = true;
                            break;
                        case 99:
                            enableButton(true);
                            break;
                        default:
                            MessageBox.Show(@"Try Again");
                            break;

                    }
                }

            }

        }

        private void button10_Click(object sender, EventArgs e)
        {
            Frm_Track newTr = new Frm_Track();
            // Set the Parent Form of the Child window.
            newTr.MdiParent = this;
            // Display the new form.
            newTr.Show();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            FrmPrint newprint = new FrmPrint();
            // Set the Parent Form of the Child window.
            newprint.MdiParent = this;
            // Display the new form.
            newprint.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FrmTemp newtemp = new FrmTemp();
            // Set the Parent Form of the Child window.
            newtemp.MdiParent = this;
            // Display the new form.
            newtemp.Show();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            Frm_OrderEntry Entry = new Frm_OrderEntry();
            // Set the Parent Form of the Child window.
            Entry.MdiParent = this;
            // Display the new form.
            Entry.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Frm_Lam newF= new Frm_Lam();
            // Set the Parent Form of the Child window.
            newF.MdiParent = this;
            // Display the new form.
            newF.Show();

        }

        private void button5_Click(object sender, EventArgs e)
        {
            Frm_IGU newF = new Frm_IGU();
            // Set the Parent Form of the Child window.
            newF.MdiParent = this;
            // Display the new form.
            newF.Show();
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                toolStripButton1.PerformClick();
            }
        }

        private void txtLogin_KeyPress(object sender, KeyPressEventArgs e)
        {
            Utility.onlynumber(e);
        }

        private void txtLogin_Enter(object sender, EventArgs e)
        {
           
        }

        private void txtLogin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            { txtPassword.Focus(); }
        }

        private void txtLogin_KeyUp(object sender, KeyEventArgs e)
        {
          
        }

        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            Utility.onlynumber(e);

        }

        private void button12_Click(object sender, EventArgs e)
        {
            frmgrind newMDIChild = new frmgrind();
            // Set the Parent Form of the Child window.
            newMDIChild.MdiParent = this;
            // Display the new form.
            newMDIChild.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            FrmBonding newMDIChild = new FrmBonding();
           
            newMDIChild.MdiParent = this;
            
            newMDIChild.Show();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            FrmSand newMDIChild = new FrmSand();

            newMDIChild.MdiParent = this;

            newMDIChild.Show();
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void button14_Click(object sender, EventArgs e)
        {
            frmTime newMDIChild = new frmTime();

            newMDIChild.MdiParent = this;

            newMDIChild.Show();
        }
    }
}
