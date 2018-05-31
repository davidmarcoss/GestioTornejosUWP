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
    public class PartidaDB : DB
    {
        public static bool Insert(Torneig torneig)
        {
            using (MySqlConnection connexio = MySQL.GetConnexio())
            {
                connexio.Open();

                MySqlTransaction trans = connexio.BeginTransaction();

                using (MySqlCommand consulta = connexio.CreateCommand())
                {
                    consulta.Transaction = trans;

                    consulta.CommandText = "insert into partides (id, sociA_id, sociB_id, torneig_id, grup_id, data_realitzacio) " +
                                             "values (@p_id, @p_sociA_id, @p_sociB_id, @p_torneig_id, @p_grup_id,  null)";

                    AddParameter(consulta, "p_id", 1, MySqlDbType.Int32);
                    AddParameter(consulta, "p_sociA_id", "", MySqlDbType.Int32);
                    AddParameter(consulta, "p_sociB_id", "", MySqlDbType.Int32);
                    AddParameter(consulta, "p_torneig_id", torneig.Id, MySqlDbType.Int32);
                    AddParameter(consulta, "p_grup_id", "", MySqlDbType.Int32);

                    try
                    {
                        bool isInserted = true;

                        MySqlParameterCollection mspc = consulta.Parameters;

                        foreach(Partida partida in torneig.Partides)
                        {
                            mspc[mspc.IndexOf("p_id")].Value = partida.Id;
                            mspc[mspc.IndexOf("p_sociA_id")].Value = partida.SociA.Id;
                            mspc[mspc.IndexOf("p_sociB_id")].Value = partida.SociB.Id;
                            mspc[mspc.IndexOf("p_grup_id")].Value = partida.Grup.Id;
                            if (consulta.ExecuteNonQuery() != 1)
                            {
                                trans.Rollback();
                                isInserted = false;
                                break;
                            }
                        }

                        if (isInserted)
                        {
                            trans.Commit();
                            SetLasId("partides", torneig.Partides.Last().Id + 1);
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

        public static ObservableCollection<Partida> GetByGrup(Grup grup)
        {
            ObservableCollection<Partida> partides = new ObservableCollection<Partida>();

            using (MySqlConnection connexio = MySQL.GetConnexio())
            {
                connexio.Open();
                using (MySqlCommand consulta = connexio.CreateCommand())
                {
                    consulta.CommandText = @"select * from partides
                                            where grup_id = @p_grup_id";

                    AddParameter(consulta, "p_grup_id", grup.Id, MySqlDbType.Int32);

                    MySqlDataReader reader = consulta.ExecuteReader();
                    while (reader.Read())
                    {
                        Dictionary<string, object> fila = getFila(reader);

                        Inscripcio inscripcioA = grup.Inscripcions.Single(Ins => Ins.Soci.Id == (int)fila["sociA_id"]);
                        Inscripcio inscripcioB = grup.Inscripcions.Single(Ins => Ins.Soci.Id == (int)fila["sociB_id"]);
                        EstatPartida estatPartida = (int)fila["estat_partida"] == 0 ? EstatPartida.PENDENT : EstatPartida.JUGAT;
                        DateTime? dt = (DateTime?)fila["data_realitzacio"];

                        int modeVictoriaBD = fila["mode_victoria"] != null ? (int)fila["mode_victoria"] : -1;
                        int guanyadorDB = fila["guanyador"] != null ? (int)fila["guanyador"] : -1;

                        Partida partida = null;

                        if (modeVictoriaBD >= 0 && guanyadorDB >= 0)
                        {
                            ModeVictoria modeVictoria = ModeVictoria.PER_CARAMBOLES;
                            if (modeVictoriaBD == 1)
                            {
                                modeVictoria = ModeVictoria.ENTRADES_ASSOLIDES;
                            }
                            else if (modeVictoriaBD == 2)
                            {
                                modeVictoria = ModeVictoria.ABANDONAMENT;
                            }


                            Guanyador guanyador = (Guanyador) guanyadorDB;

                            partida = new Partida((int)fila["id"], inscripcioA.Soci, inscripcioB.Soci, grup, grup.Torneig, (int)fila["carambolesA"], (int)fila["carambolesB"], dt, (int)fila["num_entradesA"], (int)fila["num_entradesB"], estatPartida, modeVictoria, guanyador);
                        }
                        else
                        {
                            partida = new Partida((int)fila["id"], inscripcioA.Soci, inscripcioB.Soci, grup, grup.Torneig, (int)fila["carambolesA"], (int)fila["carambolesB"], (int)fila["num_entradesA"], (int)fila["num_entradesB"], estatPartida);
                        }


                        partides.Add(partida);
                    }
                }

                connexio.Close();
            }

            return partides;
        }

        public static bool Update(Partida partida)
        {
            using (MySqlConnection connexio = MySQL.GetConnexio())
            {
                connexio.Open();

                MySqlTransaction trans = connexio.BeginTransaction();

                using (MySqlCommand consulta = connexio.CreateCommand())
                {
                    consulta.Transaction = trans;

                    consulta.CommandText = @"update partides set 
                                                sociA_id=@p_sociA_id, 
                                                sociB_id=@p_sociB_id, 
                                                carambolesA=@p_carambolesA,
                                                carambolesB=@p_carambolesB,
                                                data_realitzacio=@p_data_realitzacio,
                                                num_entradesA=@p_num_entradesA,
                                                num_entradesB=@p_num_entradesB,
                                                estat_partida=@p_estat_partida,
                                                mode_victoria=@p_mode_victoria,
                                                guanyador=@p_guanyador
                                             where id = @p_id";

                    AddParameter(consulta, "p_id", partida.Id, MySqlDbType.Int32);
                    AddParameter(consulta, "p_sociA_id", partida.SociA.Id, MySqlDbType.String);
                    AddParameter(consulta, "p_sociB_id", partida.SociB.Id, MySqlDbType.Int32);
                    AddParameter(consulta, "p_carambolesA", partida.CarambolesA, MySqlDbType.Int32);
                    AddParameter(consulta, "p_carambolesB", partida.CarambolesB, MySqlDbType.Int32);
                    AddParameter(consulta, "p_data_realitzacio", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), MySqlDbType.String);
                    AddParameter(consulta, "p_num_entradesA", partida.NumEntradesA, MySqlDbType.Int32);
                    AddParameter(consulta, "p_num_entradesB", partida.NumEntradesB, MySqlDbType.Int32);
                    AddParameter(consulta, "p_estat_partida", partida.EstatPartida, MySqlDbType.Int32);
                    AddParameter(consulta, "p_mode_victoria", partida.ModeVictoria, MySqlDbType.Int32);
                    AddParameter(consulta, "p_guanyador", partida.Guanyador, MySqlDbType.Int32);

                    try
                    {
                        if (consulta.ExecuteNonQuery() != 1)
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
