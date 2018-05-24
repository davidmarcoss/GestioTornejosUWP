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
    public class TorneigDB : DB
    {
        public static ObservableCollection<Torneig> Get(int preinscripcioOberta, String dataFrom = null, String dataTo = null)
        {
            ObservableCollection<Torneig> tornejos = new ObservableCollection<Torneig>();

            using (MySqlConnection connexio = MySQL.GetConnexio())
            {
                connexio.Open();
                using (MySqlCommand consulta = connexio.CreateCommand())
                {
                    consulta.CommandText = @"select m.descripcio, t.* from tornejos t
                                            left join modalitats m on t.modalitat_id = m.id
                                            where (@p_preinscripcioOberta = -1 or t.preinscripcio_oberta = @p_preinscripcioOberta)
                                            and (@p_dataFrom = '' or @p_dataTo = '') or (t.data_inici >= @p_dataFrom and t.data_inici <= @p_dataTo)
                                            and t.actiu = 1";

                    AddParameter(consulta, "p_preinscripcioOberta", preinscripcioOberta, MySqlDbType.Int32);
                    AddParameter(consulta, "p_dataFrom", dataFrom, MySqlDbType.String);
                    AddParameter(consulta, "p_dataTo", dataTo, MySqlDbType.String);

                    MySqlDataReader reader = consulta.ExecuteReader();
                    while (reader.Read())
                    {
                        Dictionary<string, object> fila = getFila(reader);

                        int torneigId = (int)fila["id"];
                        int modalitatId = (int)fila["modalitat_id"];

                        Modalitat modalitat = new Modalitat((int)fila["modalitat_id"], (string)fila["descripcio"]);
                        Torneig torneig = new Torneig(torneigId, modalitat, (string)fila["nom"], (DateTime)fila["data_inici"], (DateTime)fila["data_fi"], (bool)reader["preinscripcio_oberta"]);
                        torneig.Grups = GrupDB.GetByTorneig(torneigId);
                        torneig.Inscripcions = InscripcioDB.GetByTorneig(torneigId);
                        
                        // Passem les inscripcions a dins dels grups
                        foreach(Grup grup in torneig.Grups)
                        {
                            for (int i = torneig.Inscripcions.Count - 1; i >= 0; i--)
                            {
                                if (torneig.Inscripcions[i].Grup != null && torneig.Inscripcions[i].Grup.Equals(grup))
                                {
                                    grup.Inscripcions.Add(torneig.Inscripcions[i]);
                                    torneig.Inscripcions.RemoveAt(i);
                                }
                            }

                            // Li assignem el Torneig al Grup
                            grup.Torneig = torneig;

                            // Seleccionem les partides de cada grup
                            grup.Partides = PartidaDB.GetByGrup(grup);

                            // Passem les partides al Torneig
                            foreach(Partida partida in grup.Partides)
                            {
                                torneig.Partides.Add(partida);
                            }
                        }

                        tornejos.Add(torneig);
                    }
                }
                connexio.Close();
            }

            return tornejos;
        }

        public static bool InsertOrUdate(Torneig torneig, bool esInsert)
        {
            using (MySqlConnection connexio = MySQL.GetConnexio())
            {
                connexio.Open();

                MySqlTransaction trans = connexio.BeginTransaction();

                using (MySqlCommand consulta = connexio.CreateCommand())
                {
                    if (esInsert)
                    {
                        consulta.CommandText = "insert into tornejos (id, modalitat_id, nom, data_inici, data_fi, preinscripcio_oberta, actiu) values (@p_id, @p_modalitatId, @p_nom, @p_dataInici, @p_dataFi, 1, 1)";
                    }
                    else
                    {
                        consulta.CommandText = @"update tornejos set modalitat_id=@p_modalitatId, nom=@p_nom, data_inici=@p_dataInici, data_fi=@p_dataFi, preinscripcio_oberta=@p_preinscripcioOberta, actiu=1  
                                                 where id = @p_id";
                    }

                    AddParameter(consulta, "p_id", torneig.Id, MySqlDbType.Int32);
                    AddParameter(consulta, "p_modalitatId", torneig.Modalitat.Id, MySqlDbType.Int32);
                    AddParameter(consulta, "p_nom", torneig.Nom, MySqlDbType.String);
                    AddParameter(consulta, "p_dataInici", torneig.DataInici.ToString("yyyy-MM-dd"), MySqlDbType.String);
                    AddParameter(consulta, "p_dataFi", torneig.DataFi.ToString("yyyy-MM-dd"), MySqlDbType.String);
                    AddParameter(consulta, "p_preinscripcioOberta", torneig.PreinscripcioOberta ? 1 : 0, MySqlDbType.Int32);

                    try
                    {
                        int numRows = consulta.ExecuteNonQuery();
                        if (numRows != 1)
                        {
                            trans.Rollback();
                        }
                        else
                        {
                            if (esInsert)
                            {
                                SetLasId("tornejos", torneig.Id + 1);
                            }

                            trans.Commit();
                        }

                    }
                    catch (Exception e)
                    {
                        trans.Rollback();
                        return false;
                    }

                    return true;
                }
            }
        }

        public static bool Delete(int Id)
        {
            using (MySqlConnection connexio = MySQL.GetConnexio())
            {
                connexio.Open();
                MySqlTransaction trans = connexio.BeginTransaction();
                using (MySqlCommand consulta = connexio.CreateCommand())
                {
                    consulta.Transaction = trans;

                    consulta.CommandText = @"update tornejos set actiu=0 where id = @p_id";

                    AddParameter(consulta, "p_id", Id, MySqlDbType.Int32);

                    try
                    {                
                        int numRows = consulta.ExecuteNonQuery();
                        if (numRows != 1)
                        {
                            trans.Rollback();
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

                    return true;
                }
            }
        }

        public static bool TancarPreinscripcions(Torneig torneig)
        {
            using (MySqlConnection connexio = MySQL.GetConnexio())
            {
                connexio.Open();

                MySqlTransaction trans = connexio.BeginTransaction();

                using (MySqlCommand consulta = connexio.CreateCommand())
                {
                    consulta.Transaction = trans;

                    consulta.CommandText = @"update tornejos set preinscripcio_oberta = @p_preinscripcioOberta  
                                                where id = @p_id
                                                and actiu = 1";
        

                    AddParameter(consulta, "p_id", torneig.Id, MySqlDbType.Int32);
                    AddParameter(consulta, "p_preinscripcioOberta", 0, MySqlDbType.Int32);

                    try
                    {
                        int numRows = consulta.ExecuteNonQuery();
                        if (numRows != 1)
                        {
                            trans.Rollback();
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

                    return true;
                }
            }
        }
    }
}
