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
        
        public static int GetLastId(String taula)
        {
            int id = 0;

            using (MySqlConnection connexio = MySQL.GetConnexio())
            {
                connexio.Open();
                using (MySqlCommand consulta = connexio.CreateCommand())
                {
                    consulta.CommandText = @"select next_val from comptadors where clau = @p_clau";

                    AddParameter(consulta, "p_clau", taula, MySqlDbType.String);

                    MySqlDataReader reader = consulta.ExecuteReader();

                    if (reader.Read())
                    {
                        id = Int32.Parse(((Int64)reader["next_val"]).ToString());
                    }
                }

                connexio.Close();
            }

            return id;
        }

        public static bool SetLasId(String taula, int id)
        {
            using (MySqlConnection connexio = MySQL.GetConnexio())
            {
                connexio.Open();

                MySqlTransaction trans = connexio.BeginTransaction();
                using (MySqlCommand consulta = connexio.CreateCommand())
                {
                    consulta.CommandText = @"update comptadors set next_val = @p_next_val where clau = @p_clau";

                    AddParameter(consulta, "p_clau", taula, MySqlDbType.String);
                    AddParameter(consulta, "p_next_val", id, MySqlDbType.Int32);

                    try
                    {
                        if (consulta.ExecuteNonQuery() != 1)
                        {
                            trans.Rollback();
                            return false;
                        }
                        else
                        {
                            trans.Commit();
                        }
                    }
                    catch (Exception e)
                    {
                        trans.Rollback();
                        return false;
                    }
                }

                connexio.Close();
            }

            return true;
        }
    }
}
