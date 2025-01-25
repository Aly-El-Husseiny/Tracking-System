using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace Cutting.DataBase
{
    class Connection
    {
        public static SqlConnection sqlcon()
        {
            SqlConnection con = new SqlConnection("SERVER=AELHUSSIENY-IT;Database=Tracking;Integrated Security=True ");

            con.Open();
            return con;

        }
    }
}
