using GestioTornejos.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestioTornejos.DB
{
    public class SociDB : DB
    {
        public static Soci GetById(int id)
        {
            Soci soci = null;

            using (MySqlConnection connexio = MySQL.GetConnexio())
            {
                connexio.Open();

                using (MySqlCommand consulta = connexio.CreateCommand())
                {
                    consulta.CommandText = @"select id, nom, cognom1, cognom2 from socis where id = @p_Id";

                    AddParameter(consulta, "p_Id", id, MySqlDbType.Int32);

                    MySqlDataReader reader = consulta.ExecuteReader();

                    if (reader.Read())
                    {
                        soci = new Soci(id, reader["nom"].ToString(), reader["cognom1"].ToString(), reader["cognom2"].ToString());
                    }
                }

                connexio.Close();
            }

            return soci;
        }
    }
}
