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
                    consulta.CommandText = @"select 
                                                i.*,
                                                s.id as 'soci_id', s.nom, s.cognom1, s.cognom2,
                                                g.id as 'grup_id', g.descripcio, g.caramboles_victoria, g.limit_entrades
                                            from inscripcions i
                                            left join socis s on i.soci_id = s.id
                                            left join grups g on i.grup_id = g.id and g.actiu = 1
                                            where i.torneig_id = @p_torneigId";

                    AddParameter(consulta, "p_torneigId", torneigId, MySqlDbType.Int32);

                    MySqlDataReader reader = consulta.ExecuteReader();
                    while (reader.Read())
                    {
                        Dictionary<string, object> fila = getFila(reader);

                        Soci soci = soci = new Soci((int)reader["soci_id"], reader["nom"].ToString(), reader["cognom1"].ToString(), reader["cognom2"].ToString());
                        soci.Estadistiques = ModalitatDB.GetEstadistiquesBySoci(soci);
                        Inscripcio inscripcio = new Inscripcio((int)reader["id"], (DateTime)reader["data_creacio"], soci);
                        try
                        {
                            inscripcio.Grup = new Grup((int)reader["grup_id"], (string)reader["descripcio"], (int)reader["caramboles_victoria"], (int)reader["limit_entrades"]);
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
