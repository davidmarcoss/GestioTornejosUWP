using GestioTornejos.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestioTornejos.DB
{
    public class ModalitatDB : DB
    {
        public static Modalitat GetById(int id)
        {
            Modalitat modalitat = null;

            using (MySqlConnection connexio = MySQL.GetConnexio())
            {
                connexio.Open();

                using (MySqlCommand consulta = connexio.CreateCommand())
                {
                    consulta.CommandText = @"select * from modalitats where id = @p_Id";

                    AddParameter(consulta, "p_Id", id, MySqlDbType.Int32);

                    MySqlDataReader reader = consulta.ExecuteReader();

                    if (reader.Read())
                    {
                        modalitat = new Modalitat(id , reader["descripcio"].ToString());
                    }
                }

                connexio.Close();
            }

            return modalitat;
        }

        public static ObservableCollection<Modalitat> Get()
        {
            ObservableCollection<Modalitat> modalitats = new ObservableCollection<Modalitat>();

            using (MySqlConnection connexio = MySQL.GetConnexio())
            {
                connexio.Open();
                using (MySqlCommand consulta = connexio.CreateCommand())
                {
                    consulta.CommandText = @"select * from modalitats;";

                    MySqlDataReader reader = consulta.ExecuteReader();
                    while (reader.Read())
                    {
                        Dictionary<string, object> fila = getFila(reader);

                        Modalitat modalitat = new Modalitat((int)fila["id"], (string)fila["descripcio"]);

                        modalitats.Add(modalitat);
                    }
                }

                connexio.Close();
            }

            return modalitats;
        }
    }
}
