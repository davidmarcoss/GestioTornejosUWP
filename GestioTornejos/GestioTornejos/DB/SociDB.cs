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
                    consulta.CommandText = @"select id, nom, cognom1, cognom2 from socis s";

                    AddParameter(consulta, "p_Id", id, MySqlDbType.Int32);

                    MySqlDataReader reader = consulta.ExecuteReader();

                    if (reader.Read())
                    {
                        soci = new Soci(id, reader["nom"].ToString(), reader["cognom1"].ToString(), reader["cognom2"].ToString());
                        soci.Estadistiques = ModalitatDB.GetEstadistiquesBySoci(soci);
                    }
                }

                connexio.Close();
            }

            return soci;
        }

        public static bool UpdateEstadistiques(Soci soci, EstadisticaModalitat estadistica)
        {
            using (MySqlConnection connexio = MySQL.GetConnexio())
            {
                connexio.Open();

                MySqlTransaction trans = connexio.BeginTransaction();

                using (MySqlCommand consulta = connexio.CreateCommand())
                {
                    consulta.Transaction = trans;

                    consulta.CommandText = @"update estadistiques_modalitat set 
                                                coeficient_base=@p_coeficient_base, 
                                                caramboles_temporada_actual=@p_caramboles_temporada_actual, 
                                                entrades_temporada_actual=@p_entrades_temporada_actual
                                             where soci_id = @p_soci_id and modalitat_id = @p_modalitat_id";

                    AddParameter(consulta, "p_soci_id", soci.Id, MySqlDbType.Int32);
                    AddParameter(consulta, "p_modalitat_id", estadistica.Modalitat.Id, MySqlDbType.Int32);
                    AddParameter(consulta, "p_coeficient_base", estadistica.CoeficientBase, MySqlDbType.Double);
                    AddParameter(consulta, "p_caramboles_temporada_actual", estadistica.CarambolesTemporadaActual, MySqlDbType.Int32);
                    AddParameter(consulta, "p_entrades_temporada_actual", estadistica.EntradesTemporadaActual, MySqlDbType.Int32);

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

        public static bool InsertEstadistiques(Soci soci, EstadisticaModalitat estadistica)
        {
            using (MySqlConnection connexio = MySQL.GetConnexio())
            {
                connexio.Open();

                MySqlTransaction trans = connexio.BeginTransaction();

                using (MySqlCommand consulta = connexio.CreateCommand())
                {
                    consulta.Transaction = trans;

                    consulta.CommandText = @"insert into estadistiques_modalitat values(@p_soci_id, @p_modalitat_id, @p_coeficient_base, @p_caramboles_temporada_actual, @p_entrades_temporada_actual)";

                    AddParameter(consulta, "p_soci_id", soci.Id, MySqlDbType.Int32);
                    AddParameter(consulta, "p_modalitat_id", estadistica.Modalitat.Id, MySqlDbType.Int32);
                    AddParameter(consulta, "p_coeficient_base", estadistica.CarambolesTemporadaActual, MySqlDbType.Double);
                    AddParameter(consulta, "p_caramboles_temporada_actual", estadistica.CarambolesTemporadaActual, MySqlDbType.Int32);
                    AddParameter(consulta, "p_entrades_temporada_actual", estadistica.EntradesTemporadaActual, MySqlDbType.Int32);

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
