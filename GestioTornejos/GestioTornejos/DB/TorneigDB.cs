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
        public static ObservableCollection<Torneig> Get(int preinscripcioOberta = 0)
        {
            ObservableCollection<Torneig> tornejos = new ObservableCollection<Torneig>();

            using (MySqlConnection connexio = MySQL.GetConnexio())
            {
                connexio.Open();
                using (MySqlCommand consulta = connexio.CreateCommand())
                {
                    consulta.CommandText = @"select * from tornejos
                                            where (@p_preinscripcioOberta = 0 or preinscripcio_oberta = @p_preinscripcioOberta)
                                            and actiu = 1";

                    AddParameter(consulta, "p_preinscripcioOberta", preinscripcioOberta, MySqlDbType.Int32);

                    MySqlDataReader reader = consulta.ExecuteReader();
                    while (reader.Read())
                    {
                        Dictionary<string, object> fila = getFila(reader);

                        int torneigId = (int)fila["id"];
                        int modalitatId = (int)fila["modalitat_id"];

                        Modalitat modalitat = ModalitatDB.GetById(modalitatId);
                        Torneig torneig = new Torneig(torneigId, modalitat, (string)fila["nom"], (DateTime)fila["data_inici"], (bool)reader["preinscripcio_oberta"]);
                        torneig.Grups = GrupDB.GetByTorneig(torneigId);
                        torneig.Inscripcions = InscripcioDB.GetByTorneig(torneigId);

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
                        }

                        tornejos.Add(torneig);
                    }
                }
                connexio.Close();
            }

            return tornejos;
        }

        public static bool InsertOrUdate(Torneig torneig)
        {
            using (MySqlConnection connexio = MySQL.GetConnexio())
            {
                connexio.Open();

                MySqlTransaction trans = connexio.BeginTransaction();

                using (MySqlCommand consulta = connexio.CreateCommand())
                {
                    consulta.Transaction = trans;

                    bool esInsert = (torneig.Id <= 0);
                    if (esInsert)
                    {
                        consulta.CommandText = "insert into tornejos (modalitat_id, nom, data_inici, preinscripcio_oberta, actiu) values (@p_modalitatId, @p_nom, @p_dataInici, 1, 1)";
                    }
                    else
                    {
                        consulta.CommandText = @"update tornejos set modalitat_id=@p_modalitatId, nom=@p_nom, data_inici=@p_dataInici, preinscripcio_oberta=@p_preinscripcioOberta, actiu=1  
                                                 where id = @p_id";
                    }

                    AddParameter(consulta, "p_id", torneig.Id, MySqlDbType.Int32);
                    AddParameter(consulta, "p_modalitatId", torneig.Modalitat.Id, MySqlDbType.Int32);
                    AddParameter(consulta, "p_nom", torneig.Nom, MySqlDbType.String);
                    AddParameter(consulta, "p_dataInici", torneig.DataInici.ToString("yyyy-MM-dd"), MySqlDbType.String);
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
                                torneig.Id = (int)consulta.LastInsertedId;
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
                                                where id = @p_id";
        

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
