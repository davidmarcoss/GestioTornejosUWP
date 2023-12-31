﻿using GestioTornejos.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestioTornejos.DB
{
    public class GrupDB : DB
    {
        public static ObservableCollection<Grup> GetByTorneig(int torneigId = 0)
        {
            ObservableCollection<Grup> grups = new ObservableCollection<Grup>();

            using (MySqlConnection connexio = MySQL.GetConnexio())
            {
                connexio.Open();
                using (MySqlCommand consulta = connexio.CreateCommand())
                {
                    consulta.CommandText = @"select * from grups
                                            where torneig_id = @p_torneigId
                                            and actiu = 1";

                    AddParameter(consulta, "p_torneigId", torneigId, MySqlDbType.Int32);

                    MySqlDataReader reader = consulta.ExecuteReader();
                    while (reader.Read())
                    {
                        Dictionary<string, object> fila = getFila(reader);

                        Grup grup = new Grup((int)fila["id"], (string)fila["descripcio"], (int)fila["caramboles_victoria"], (int)fila["limit_entrades"]);

                        grups.Add(grup);
                    }
                }

                connexio.Close();
            }

            return grups;
        }

        public static Grup GetById(int id = 0)
        {
            Grup grup = null;

            using (MySqlConnection connexio = MySQL.GetConnexio())
            {
                connexio.Open();
                using (MySqlCommand consulta = connexio.CreateCommand())
                {
                    consulta.CommandText = @"select * from grups
                                            where id = @p_id
                                            and actiu = 1";

                    AddParameter(consulta, "p_id", id, MySqlDbType.Int32);

                    MySqlDataReader reader = consulta.ExecuteReader();
                    while (reader.Read())
                    {
                        Dictionary<string, object> fila = getFila(reader);

                        grup = new Grup((int)fila["id"], (string)fila["descripcio"], (int)fila["caramboles_victoria"], (int)fila["limit_entrades"]);
                    }
                }

                connexio.Close();
            }

            return grup;
        }

        public static bool Insert(Torneig torneig, Grup grup)
        {
            using (MySqlConnection connexio = MySQL.GetConnexio())
            {
                connexio.Open();

                MySqlTransaction trans = connexio.BeginTransaction();

                using (MySqlCommand consulta = connexio.CreateCommand())
                {
                    consulta.Transaction = trans;

                    consulta.CommandText = "insert into grups (id, torneig_id, descripcio, caramboles_victoria, limit_entrades) " +
                                             "values (@p_id, @p_torneigId, @p_descripcio, @p_carambolesVictoria, @p_limitEntrades)";

                    AddParameter(consulta, "p_id", grup.Id, MySqlDbType.Int32);
                    AddParameter(consulta, "p_torneigId", torneig.Id, MySqlDbType.Int32);
                    AddParameter(consulta, "p_descripcio", grup.Descripcio, MySqlDbType.String);
                    AddParameter(consulta, "p_carambolesVictoria", grup.CarambolesVictoria, MySqlDbType.Int32);
                    AddParameter(consulta, "p_limitEntrades", grup.LimitEntrades, MySqlDbType.Int32);

                    try
                    {
                        int numRows = consulta.ExecuteNonQuery();
                        if (numRows != 1)
                        {
                            trans.Rollback();
                        }
                        else
                        {
                            SetLasId("grups", grup.Id + 1);

                            consulta.CommandText = @"update inscripcions set grup_id=@p_grupId where id = @p_id";
                            AddParameter(consulta, "p_grupId", "", MySqlDbType.Int32);
                            foreach (Inscripcio inscripcio in grup.Inscripcions)
                            {
                                MySqlParameterCollection mspc = consulta.Parameters;
                                mspc[mspc.IndexOf("p_grupId")].Value = grup.Id;
                                mspc[mspc.IndexOf("p_id")].Value = inscripcio.Id;
                                consulta.ExecuteNonQuery();
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

        public static bool Update(Torneig torneig, Grup grup)
        {
            using (MySqlConnection connexio = MySQL.GetConnexio())
            {
                connexio.Open();

                MySqlTransaction trans = connexio.BeginTransaction();

                using (MySqlCommand consulta = connexio.CreateCommand())
                {
                    consulta.Transaction = trans;

                    consulta.CommandText = @"update grups set 
                                                descripcio=@p_descripcio, 
                                                caramboles_victoria=@p_carambolesVictoria, 
                                                limit_entrades=@p_limitEntrades  
                                             where id = @p_id";

                    AddParameter(consulta, "p_id", grup.Id, MySqlDbType.Int32);
                    AddParameter(consulta, "p_descripcio", grup.Descripcio, MySqlDbType.String);
                    AddParameter(consulta, "p_carambolesVictoria", grup.CarambolesVictoria, MySqlDbType.Int32);
                    AddParameter(consulta, "p_limitEntrades", grup.LimitEntrades, MySqlDbType.Int32);

                    try
                    {
                        if (consulta.ExecuteNonQuery() != 1)
                        {
                            trans.Rollback();
                        }
                        else
                        {
                            MySqlParameterCollection mspc = null;

                            consulta.CommandText = @"update inscripcions set grup_id=NULL where grup_id = @p_grupId";
                            AddParameter(consulta, "p_grupId", "", MySqlDbType.Int32);
                            mspc = consulta.Parameters;
                            mspc[mspc.IndexOf("p_grupId")].Value = grup.Id;
                            consulta.ExecuteNonQuery();

                            consulta.CommandText = @"update inscripcions set grup_id=@p_grupId where id = @p_id";
                            mspc = consulta.Parameters;
                            mspc[mspc.IndexOf("p_grupId")].Value = grup.Id;
                            foreach (Inscripcio inscripcio in grup.Inscripcions)
                            {
                                mspc[mspc.IndexOf("p_id")].Value = inscripcio.Id;
                                consulta.ExecuteNonQuery();
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

        public static bool Delete(Grup grup)
        {
            using (MySqlConnection connexio = MySQL.GetConnexio())
            {
                connexio.Open();
                MySqlTransaction trans = connexio.BeginTransaction();
                using (MySqlCommand consulta = connexio.CreateCommand())
                {
                    consulta.Transaction = trans;

                    consulta.CommandText = @"update grups set actiu=0 where id = @p_id";

                    AddParameter(consulta, "p_id", grup.Id, MySqlDbType.Int32);

                    try
                    {
                        if (consulta.ExecuteNonQuery() != 1)
                        {
                            trans.Rollback();
                        }
                        else
                        {
                            consulta.CommandText = @"update inscripcions set grup_id=NULL where grup_id = @p_id";
                            consulta.ExecuteNonQuery();

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
