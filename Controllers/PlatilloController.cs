using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using Rapid_Plus.Models;
using System.Threading.Tasks;
using System.Data.Common;
using System.Windows;
using Rapid_Plus.Models.Mesero;

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

                                platillo.PlatilloId = Convert.ToInt32(dr["IdPlatillo"].ToString());
                                platillo.Platillo = dr["NombrePlatillo"].ToString();
                                platillo.Descripcion = dr["DescripcionPlatillo"].ToString();
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
        public static int AgregarPlatillo(PlatilloModel platillo, int idEstado) 
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
                        command.CommandText = "SPCREARMENU";

                        command.Parameters.AddWithValue("@Nombre", platillo.Platillo);
                        command.Parameters.AddWithValue("@Descripcion", platillo.Descripcion);
                        command.Parameters.AddWithValue("@IdCategoria", platillo.CategoriaId);
                        command.Parameters.AddWithValue("@Precio", platillo.Precio);
                        command.Parameters.AddWithValue("@IdEstado", idEstado);

                        res = command.ExecuteNonQuery();

                        if (res < 0)
                        {
                            throw new Exception(" Ya existe este platillo");
                        }
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
                        command.CommandText = "SPACTUALIZARMENU";

                        command.Parameters.AddWithValue("@IdPlatillo", idPlatillo);
                        command.Parameters.AddWithValue("@Nombre", platillo.Platillo);
                        command.Parameters.AddWithValue("@Descripcion", platillo.Descripcion);
                        command.Parameters.AddWithValue("@IdCategoria", platillo.CategoriaId);
                        command.Parameters.AddWithValue("@Precio", platillo.Precio);
                        command.Parameters.AddWithValue("@IdEstado", platillo.EstadoId);

                        res = command.ExecuteNonQuery();

                        if (res < 0)
                        {
                            throw new Exception(" Ya existe este platillo");
                        }

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
                        command.CommandText = "SPELIMINARMENU";

                        command.Parameters.AddWithValue("@IdPlatillo", id);
                        command.Parameters.AddWithValue("@IdEstado", estado);

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

        //PLATILLOS MESERO
        public static List<PlatilloModel> MostrarPlatillos(int? IdCategoria = null)
        {
            List<PlatilloModel> lstPlatillos = new List<PlatilloModel>();

            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    con.Open();
                    using (var command = con.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "SPMOSTRARPLATILLOS";
                        if (IdCategoria.HasValue)
                        {
                            command.Parameters.AddWithValue("@CATEGORIA", IdCategoria.Value);
                        }
                        else
                        {
                            // Si no se pasa un valor, asegurarse de que el parámetro sea NULL
                            command.Parameters.AddWithValue("@CATEGORIA", DBNull.Value);
                        }
                        using (DbDataReader dr = command.ExecuteReader())
                        {
                            //Recorrer el dataReader
                            while (dr.Read())
                            {
                                PlatilloModel platillo = new PlatilloModel();
                                platillo.PlatilloId = int.Parse(dr["IDPLATILLO"].ToString());
                                platillo.Platillo = dr["PLATILLO"].ToString();
                                platillo.Descripcion = dr["DESCRIPCION"].ToString();

                                //Agregar a la lista inicial
                                lstPlatillos.Add(platillo);
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrio un error al intentar mostrar los registros:" + ex.Message, "Validaccion", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return lstPlatillos;

        }

    }
}
