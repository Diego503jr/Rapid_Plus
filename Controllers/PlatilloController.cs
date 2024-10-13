using Rapid_Plus.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using System.Windows;

namespace Rapid_Plus.Controllers
{
    internal class PlatilloController
    {
        private static string conexion = Properties.Settings.Default.DbRapidPlus;

        //Leer Platillos
        public static List<PlatilloModel> MostrarMenu() 
        { 
            List<PlatilloModel> lstPlatillos = new List<PlatilloModel>();

            try 
            {
                using (var conDb = new SqlConnection(conexion))
                { 
                    conDb.Open();
                    using (var command = conDb.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "SPMOSTRARMENU";

                        using (DbDataReader dr = command.ExecuteReader()) 
                        {
                            while (dr.Read())
                            {
                                PlatilloModel platillo = new PlatilloModel();

                                platillo.PlatilloId = Convert.ToInt32(dr["Id"].ToString());
                                platillo.Platillo = dr["Nombre_Platillo"].ToString();
                                platillo.Descripcion = dr["Descripcion_Platillos"].ToString();
                                platillo.Categoria = dr["Categoria"].ToString();
                                platillo.Precio = Convert.ToDecimal(dr["Precio"].ToString());
                                platillo.Estado = dr["Estado"].ToString();

                                lstPlatillos.Add(platillo);
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

            return lstPlatillos;
        }

        //Crear Platillos
        public static int AgregarPlatillo(PlatilloModel platillo) 
        {
            int res = -1;

            try
            {
                using (var condDb = new SqlConnection(conexion))
                {
                    condDb.Open();
                    using (var command = condDb.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "SPCREARPLATILLO";

                        command.Parameters.AddWithValue("@Nombre", platillo.Platillo);
                        command.Parameters.AddWithValue("@Descripcion", platillo.Descripcion);
                        command.Parameters.AddWithValue("@Id_Categoria", platillo.CategoriaId);
                        command.Parameters.AddWithValue("@Precio", platillo.Precio);
                        command.Parameters.AddWithValue("@Id_Estado", platillo.EstadoId);

                        res = command.ExecuteNonQuery();

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrio un error al intentar registrar el platillo" + ex.Message, "Validacion",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return res;
        }

        //Editar Platillos
        public static int EditarPlatillo(PlatilloModel platillo, int idPlatillo)
        {
            int res = -1;

            try
            {
                using (var condDb = new SqlConnection(conexion))
                {
                    condDb.Open();
                    using (var command = condDb.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "SPACTUALIZARPLATILLO";

                        command.Parameters.AddWithValue("@Id", idPlatillo);
                        command.Parameters.AddWithValue("@Nombre", platillo.Platillo);
                        command.Parameters.AddWithValue("@Descripcion", platillo.Descripcion);
                        command.Parameters.AddWithValue("@Id_Categoria", platillo.CategoriaId);
                        command.Parameters.AddWithValue("@Precio", platillo.Precio);
                        command.Parameters.AddWithValue("@Id_Estado", platillo.EstadoId);

                        res = command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrio un error al intentar registrar el platillo" + ex.Message, "Validacion",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return res;
        }

        //Eliminar Platillos
        public static int EliminarPlatillo(int id, int estado) 
        { 
            int res = -1;

            try
            {
                using (var condB = new SqlConnection(conexion)) 
                { 
                    condB.Open();
                    using (var command = condB.CreateCommand()) 
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "SPELIMINARPLATILLO";

                        command.Parameters.AddWithValue("@Id", id);
                        command.Parameters.AddWithValue("@Id_Estado", estado);

                        res = command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrio un error al intentar eliminar el platillo" + ex.Message, "Validacion",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return res;
        }
    }
}
