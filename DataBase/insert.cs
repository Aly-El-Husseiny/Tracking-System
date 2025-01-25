using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Windows.Forms;
namespace Cutting.DataBase
{
    class insert
    {
        public static void insertdb (string sqlcmd,string msgbox)
        {
            SqlCommand cmd = new SqlCommand(sqlcmd, DataBase.Connection.sqlcon());
            cmd.ExecuteNonQuery();
            MessageBox.Show(msgbox);


        }
        
           
           

    }
 }
