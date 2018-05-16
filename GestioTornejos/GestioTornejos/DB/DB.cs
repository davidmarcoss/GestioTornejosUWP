using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestioTornejos.DB
{
    public class DB
    {
        public static void AddParameter(MySqlCommand consulta, string nom, object valor, MySqlDbType tipus)
        {
            MySqlParameter param = consulta.CreateParameter();
            param.MySqlDbType = tipus;
            param.Value = valor;
            param.ParameterName = nom;
            consulta.Parameters.Add(param);
        }


        internal static Dictionary<string, object> getFila(MySqlDataReader reader)
        {
            Dictionary<string, object> fila = new Dictionary<string, object>();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                object valor = reader.GetValue(i);
                string nomColumna = reader.GetName(i);
                fila[nomColumna] = valor;
            }

            return fila;
        }
        
    }
}
