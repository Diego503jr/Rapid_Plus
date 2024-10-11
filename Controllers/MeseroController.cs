using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Rapid_Plus.Models.Mesero;
using System.Data.SqlClient;
using System.Data;
using Rapid_Plus.Models;


namespace Rapid_Plus.Controllers.Mesero
{
    class MeseroController
    {
        private static string conexion = Properties.Settings.Default.DbRapidPlus;

        //MOSTRAR
        public static List<OrdenesModel> ListarOrdenes()
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
                        using (DbDataReader dr = command.ExecuteReader())
                        {
                            //Recorrer el dataReader
                            while (dr.Read())
                            {
                                OrdenesModel ordenes = new OrdenesModel();
                                ordenes.IdOrden = int.Parse(dr["ORDEN"].ToString());
                                ordenes.Orden = dr["PLATILLO"].ToString();
                                ordenes.Cantidad = int.Parse(dr["CANTIDAD"].ToString());
                                ordenes.Mesa =int.Parse(dr["MESA"].ToString());
                                ordenes.EstadoOrden = dr["ESTADO"].ToString();

                                //Agregar a la lista inicial
                                lstOrdenes.Add(ordenes);
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrio un error al intentar mostrar los registros:" + ex.Message, "Validaccion", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return lstOrdenes;

        }

        public static List<OrdenesModel> ListaPlatillos()
        {
            List<OrdenesModel> lstPlatillos = new List<OrdenesModel>();

            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    con.Open();
                    using (var command = con.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "SPMOSTRARPLATILLOS";
                        using (DbDataReader dr = command.ExecuteReader())
                        {
                            //Recorrer el dataReader
                            while (dr.Read())
                            {
                                OrdenesModel ordenes = new OrdenesModel();
                                ordenes.IdPlatillo = int.Parse(dr["ID"].ToString());
                                ordenes.NombrePlatillo = dr["PLATILLO"].ToString();
                                ordenes.DescripcionPlatillo = dr["DESCRIPCION"].ToString();

                                //Agregar a la lista inicial
                                lstPlatillos.Add(ordenes);
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

        public static List<OrdenesModel> ListarOrdenesPorMesa(int numeroMesa)
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
                        command.Parameters.AddWithValue("@MESA", numeroMesa);
                        using (DbDataReader dr = command.ExecuteReader())
                        {
                            //Recorrer el dataReader
                            while (dr.Read())
                            {
                                OrdenesModel ordenes = new OrdenesModel();
                                ordenes.IdOrden = int.Parse(dr["ORDEN"].ToString());
                                ordenes.Orden = dr["PLATILLO"].ToString();
                                ordenes.DescripcionPlatillo = dr["DESCRIPCION"].ToString();
                                ordenes.Cantidad = int.Parse(dr["CANTIDAD"].ToString());
                                ordenes.EstadoOrden = dr["ESTADO"].ToString();

                                //Agregar a la lista inicial
                                lstOrdenes.Add(ordenes);
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrio un error al intentar mostrar los registros:" + ex.Message, "Validaccion", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return lstOrdenes;

        }

        //CREAR

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

                        command.Parameters.AddWithValue("@Nombre_Cliente", orden.NombreCliente);
                        command.Parameters.AddWithValue("@Apellido_Cliente", orden.ApellidoCliente);
                        command.Parameters.AddWithValue("@Fecha_Orden", orden.FechaOrden);
                        command.Parameters.AddWithValue("@Total", orden.Total);
                        command.Parameters.AddWithValue("@Id_Mesa", orden.Mesa);
                        command.Parameters.AddWithValue("@Id_Usuario", orden.UsuarioId);
                        command.Parameters.AddWithValue("@Id_Estado_Orden",orden.IdEstadoOrden);
                        MessageBox.Show("ID ESTADOORDEN" + orden.IdEstadoOrden);
                        res = command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrio un error al intentar crear los registros" + ex.Message, "Validacion",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return res;
        }

        //ACTUALIZAR / AGREGAR ORDEN

    }


}
