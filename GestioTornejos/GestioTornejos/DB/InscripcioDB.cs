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
    public class InscripcioDB : DB
    {
        public static ObservableCollection<Inscripcio> GetByTorneig(int torneigId = 0)
        {
            ObservableCollection<Inscripcio> inscripcions = new ObservableCollection<Inscripcio>();

            using (MySqlConnection connexio = MySQL.GetConnexio())
            {
                connexio.Open();
                using (MySqlCommand consulta = connexio.CreateCommand())
                {
                    consulta.CommandText = @"select * from inscripcions
                                            where torneig_id = @p_torneigId";

                    AddParameter(consulta, "p_torneigId", torneigId, MySqlDbType.Int32);

                    MySqlDataReader reader = consulta.ExecuteReader();
                    while (reader.Read())
                    {
                        Dictionary<string, object> fila = getFila(reader);

                        Soci soci = SociDB.GetById((int)reader["soci_id"]);
                        Inscripcio inscripcio = new Inscripcio((int)reader["id"], (DateTime)reader["data_creacio"], soci);
                        try
                        {
                            inscripcio.Grup = GrupDB.GetById((int)reader["grup_id"]);
                        } 
                        catch(NullReferenceException ex)
                        {

                        }
                        
                        inscripcions.Add(inscripcio);
                    }
                }

                connexio.Close();
            }

            return inscripcions;
        }
    }
}
