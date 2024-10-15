using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Rapid_Plus.Models;
using System.Data.Common;

namespace Rapid_Plus.Controllers
{
    class MesaController
    {

        private static string conexion = Properties.Settings.Default.DbRapidPlus;

        //Leer Mesa
        public static List<MesasModel> MostrarMesa() 
        {
            List<MesasModel> lstMesas = new List<MesasModel>();

            try
            {
                using (var conDb = new SqlConnection(conexion))
                { 
                    conDb.Open();
                    using (var command = conDb.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "SPMOSTRARMESAS";

                        using (DbDataReader dr = command.ExecuteReader())
                        { 
                            while (dr.Read())
                            {
                                MesasModel mesa = new MesasModel();

                                mesa.MesaId = Convert.ToInt32(dr["Id"].ToString());
                                mesa.Mesa = Convert.ToInt32(dr["Mesa"].ToString());
                                mesa.Estado = dr["Estado"].ToString();

                                lstMesas.Add(mesa);
                            }
                        }
                    }
                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show("Ocurrio un error al intentar mostrar los registros" + ex.Message, "Validacion",
                        MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return lstMesas;
        }

        //Crear Mesa
        public static int CrearMesa(MesasModel mesa) 
        {
            int res = -1;

            try
            {
                using (var conDb = new SqlConnection(conexion))
                {
                    conDb.Open();
                    using (var command = conDb.CreateCommand())
                    { 
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "SPCREARMESA";

                        command.Parameters.AddWithValue("@Mesa", mesa.Mesa);
                        command.Parameters.AddWithValue("@Id_Estado", mesa.EstadoId);

                        res = command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrio un error al intentar registrar las mesas" + ex.Message, "Validacion",
                        MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return res;
        }

        //Editar Mesa
        public static int EditarMesa(MesasModel mesa, int idMesa)
        {
            int res = -1;

            try 
            {
                using (var conDb = new SqlConnection(conexion))
                {
                    conDb.Open();
                    using (var command = conDb.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "SPACTUALIZARMESA";

                        command.Parameters.AddWithValue("@Id", idMesa);
                        command.Parameters.AddWithValue("@Mesa", mesa.Mesa);
                        command.Parameters.AddWithValue("@Id_Estado", mesa.EstadoId);

                        res = command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrio un error al intentar editar las mesas" + ex.Message, "Validacion",
                        MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return res;
        }

        //Eliminar Mesa
        public static int EliminarMesa(int mesa, int estado)
        {
            int res = -1;

            try
            {
                using (var conDb = new SqlConnection(conexion))
                {
                    conDb.Open();
                    using (var command = conDb.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "SPELIMINARMESA";

                        command.Parameters.AddWithValue("@Id", mesa);
                        command.Parameters.AddWithValue("@Id_Estado", estado);

                        res = command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrio un error al intentar eliminar la mesa" + ex.Message, "Validacion",
                        MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return res;
        }
    }
}
