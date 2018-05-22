using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestioTornejos.DB
{
    public class MySQL
    {
        public static MySqlConnection GetConnexio()
        {
            System.Text.EncodingProvider ppp;
            ppp = System.Text.CodePagesEncodingProvider.Instance;
            Encoding.RegisterProvider(ppp);

            string conString = "server = 127.0.0.1; uid = root; pwd = ;charset=utf8; SslMode=None ; database = billars";
            //string conString = "server = 92.222.27.83; uid = m2-dmarcos; pwd = 23844512K;charset=utf8; SslMode=None ; database = m2_dmarcos";
            return new MySqlConnection(conString);
        }
    }
}
