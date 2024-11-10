using Rapid_Plus.Models.Mesero;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Rapid_Plus.Controllers
{
    internal class OrdenController
    {
        private static string conexion = Properties.Settings.Default.DbRapidPlus;

        //CREAR ORDEN
        public static int CrearOrden(OrdenesModel orden)
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
                        command.CommandText = "SPCREARORDEN";

                        command.Parameters.AddWithValue("@IdCliente", orden.IdCliente);
                        command.Parameters.AddWithValue("@IdUsuario", orden.IdUsuario);
                        command.Parameters.AddWithValue("@FechaOrden", orden.FechaOrden);
                        command.Parameters.AddWithValue("@Total", orden.Total);
                        command.Parameters.AddWithValue("@IdMesa", orden.Mesa);
                        command.Parameters.AddWithValue("@IdEstadoOrden", orden.IdEstadoOrden);

                        res = command.ExecuteNonQuery();
                        if (res <= 0)
                        {
                            throw new Exception("El cliente ha sido asignado a otra mesa.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Validación orden",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return res;
        }

        //MOSTRAR LISTADO DE ORDENES FILTRANDO POR ESTADO (LISTO/ PENDIENTE/ CANCELADO)
        public static List<OrdenesModel> MostrarOrdenes(int? IdEstado = null)
        {
            List<OrdenesModel> lstOrdenes = new List<OrdenesModel>();

            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    con.Open();
                    using (var command = con.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "SPMOSTRARORDENES";
                        if (IdEstado.HasValue)
                        {
                            command.Parameters.AddWithValue("@ESTADO", IdEstado.Value);
                        }
                        else
                        {
                            // Si no se pasa un valor, asegurarse de que el parámetro sea NULL
                            command.Parameters.AddWithValue("@ESTADO", DBNull.Value);
                        }
                        using (DbDataReader dr = command.ExecuteReader())
                        {
                            //Recorrer el dataReader
                            while (dr.Read())
                            {
                                OrdenesModel ordenes = new OrdenesModel();
                                ordenes.IdOrden = int.Parse(dr["IDORDEN"].ToString());
                                ordenes.Cantidad = int.Parse(dr["CANTIDAD"].ToString());
                                ordenes.Orden = dr["PLATILLO"].ToString();
                                ordenes.Mesa = int.Parse(dr["MESA"].ToString()); //Número de mesa
                                ordenes.EstadoOrden = dr["ESTADOORDEN"].ToString();

                                //Agregar a la lista inicial
                                lstOrdenes.Add(ordenes);
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrio un error al intentar mostrar las ordenes:" + ex.Message, "Validaccion ordenes", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return lstOrdenes;

        }

       
        //MOSTRAR LISTADO DE ORDENES FILTRANDO POR MESA
        public static List<OrdenesModel> MostrarOrdenesPorMesa(int idMesa)
        {
            List<OrdenesModel> lstOrdenes = new List<OrdenesModel>();

            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    con.Open();
                    using (var command = con.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "SPMOSTRARORDENESPORMESA";
                        command.Parameters.AddWithValue("@IDMESA", idMesa);
                        using (DbDataReader dr = command.ExecuteReader())
                        {
                            //Recorrer el dataReader
                            while (dr.Read())
                            {
                                OrdenesModel ordenes = new OrdenesModel();
                                ordenes.IdOrden = int.Parse(dr["IDORDEN"].ToString());
                                ordenes.IdPlatilloOrden = int.Parse(dr["IDPLATILLOORDEN"].ToString());
                                ordenes.IdDetalleOrden = int.Parse(dr["IDDETALLEORDEN"].ToString());
                                ordenes.NombrePlatillo = dr["PLATILLO"].ToString();
                                ordenes.DescripcionPlatillo = dr["DESCRIPCION"].ToString();
                                ordenes.IdMesa = int.Parse(dr["MESA"].ToString());
                                ordenes.Cantidad = int.Parse(dr["CANTIDAD"].ToString());
                                ordenes.EstadoOrden = dr["ESTADOORDEN"].ToString();

                                //Agregar a la lista inicial
                                lstOrdenes.Add(ordenes);
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrio un error al intentar mostrar las ordenes:" + ex.Message, "Validaccion ordenes", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return lstOrdenes;

        }
    }
}
