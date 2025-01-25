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
    public partial class FrmBarCodeSend : Form
    {
        TrackingDataContext trackdb = new TrackingDataContext();
        public FrmBarCodeSend()
        {
            InitializeComponent();
        }

        private void txt_BarCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            //var BareCode = from id in trackdb.Tracks join bar in trackdb.BarCodes on new { id.OC_ID, id.Item_ID, id.Pos } equals new { bar.OC_ID, bar.Item_ID, bar.Pos }
            //               join idd in trackdb.GlassTypes on id.Glass_ID equals idd.Glass_ID 
            //           join iddd in trackdb.ITEMs on new { id.OC_ID, id.Item_ID, id.Pos } equals new { iddd.OC_ID, iddd.Item_ID, iddd.Pos }
            //           join depar in trackdb.Departments on id.Recived_From equals depar.Departmet_ID
            //           where id.Departmet_ID == 13 && id.QTY_ToDo > 0

            //           select new
            //           {

            //               WorkOrder = id.OC_ID,
            //               Item = id.Item_ID,
            //               Width = iddd.Width,
            //               Height = iddd.Hieght,
            //               GlassType = idd.Glass_Type,
            //               QTY_TO_Work = id.QTY_ToDo,
            //               Recieved_From = depar.Department_Name,
            //               Trak_ID = id.Track_ID,
            //               Date = id.Date


            //           };

            //DGV_Aris_todo.DataSource = aris;
        }

        private void FrmBarCodeSend_Load(object sender, EventArgs e)
        {
            
        }
    }
}
