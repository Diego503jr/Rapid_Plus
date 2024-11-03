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
using Rapid_Plus.Models;


namespace Rapid_Plus.Controllers.Mesero
{
    class MeseroController
    {
        private static string conexion = Properties.Settings.Default.DbRapidPlus;

        //MOSTRAR DATOS
        public static List<OrdenesModel> ListarOrdenes(int? IdEstado = null)
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

        public static List<OrdenesModel> ListaPlatillos(int? IdCategoria = null)
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
                        command.Parameters.AddWithValue("@ID", numeroMesa);
                        using (DbDataReader dr = command.ExecuteReader())
                        {
                            //Recorrer el dataReader
                            while (dr.Read())
                            {
                                OrdenesModel ordenes = new OrdenesModel();
                                ordenes.IdOrden = int.Parse(dr["ORDEN"].ToString());
                                ordenes.Orden = dr["PLATILLO"].ToString();
                                ordenes.Mesa = int.Parse(dr["MESA"].ToString());
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
        public static List<OrdenesModel> ListarClientes()
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
                        command.CommandText = "SPMOSTRARCLIENTES";
                        using (DbDataReader dr = command.ExecuteReader())
                        {
                            //Recorrer el dataReader
                            while (dr.Read())
                            {
                                OrdenesModel ordenes = new OrdenesModel();
                                ordenes.IdCliente = int.Parse(dr["IDCLIENTE"].ToString());
                                ordenes.NombreCliente = dr["NOMBRE"].ToString();
                                ordenes.ApellidoCliente = dr["APELLIDO"].ToString();

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
        public static OrdenesModel ObtenerOrdenPorMesa(int idMesa)
        {
            OrdenesModel orden = null;

            using (var conDb = new SqlConnection(conexion))
            {
                conDb.Open();

                string query = "SELECT TOP 1 Orden.IdOrden AS OrdenId, EstadoOrden.EstadoOrden AS Estado FROM Orden INNER JOIN EstadoOrden ON Orden.IdEstadoOrden = EstadoOrden.IdEstadoOrden WHERE Orden.IdMesa = @IdMesa ORDER BY Orden.IdOrden DESC";
                using (var command = new SqlCommand(query, conDb))
                {
                    command.Parameters.AddWithValue("@IdMesa", idMesa);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            orden = new OrdenesModel
                            {
                                IdOrden = reader.GetInt32(reader.GetOrdinal("OrdenId")),
                                EstadoOrden = reader.GetString(reader.GetOrdinal("Estado"))
                            };
                        }
                    }
                }
            }

            return orden;
        }

        //CREAR ORDEN

        public static int CrearOrden(OrdenesModel orden)
        {
            int idOrden = 0;
            try
            {
                using (var conDb = new SqlConnection(conexion))
                {
                    conDb.Open();

                    using (var command = conDb.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "SPCREARORDEN";

                        command.Parameters.AddWithValue("@NombreCliente", orden.NombreCliente);
                        command.Parameters.AddWithValue("@ApellidoCliente", orden.ApellidoCliente);
                        command.Parameters.AddWithValue("@FechaOrden", orden.FechaOrden);
                        command.Parameters.AddWithValue("@Total", orden.Total);
                        command.Parameters.AddWithValue("@IdMesa", orden.Mesa);
                        command.Parameters.AddWithValue("@IdUsuario", orden.UsuarioId);
                        command.Parameters.AddWithValue("@IdEstadoOrden", orden.IdEstadoOrden);

                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int parsedId))
                        {
                            idOrden = parsedId;
                        }
                        else
                        {
                            throw new Exception("No se pudo obtener el ID de la orden creada.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error al intentar crear los registros: " + ex.Message, "Validación",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return idOrden;
        }

        // AGREGAR ORDEN
        public static int InsertarOrden(OrdenesModel orden)
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
                        command.CommandText = "SPINSERTARORDEN";

                        // Validar que los datos sean válidos
                        if (orden.Cantidad > 0)
                        {
                            command.Parameters.AddWithValue("@IDORDEN", orden.IdOrden);
                            command.Parameters.AddWithValue("@CANTIDAD", orden.Cantidad);
                            command.Parameters.AddWithValue("@IDESTADO", orden.IdEstado);
                            command.Parameters.AddWithValue("@IDPLATILLO", orden.IdPlatillo);
                            

                            // Ejecutar la inserción
                            res = command.ExecuteNonQuery();
                            if (res <= 0)
                            {
                                throw new Exception("No se pudo insertar el detalle de la orden.");
                            }
                        }
                        else
                        {
                            throw new ArgumentException("Valores de la orden no válidos para la inserción.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error al intentar insertar los detalles de la orden: " + ex.Message, "Validación",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return res;
        }

        //EDITAR ORDEN
        public static int EditarOrden(OrdenesModel orden, int idOrden)
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
                        command.CommandText = "ACTUALIZARORDEN";

                        command.Parameters.AddWithValue("@ID", idOrden);
                        command.Parameters.AddWithValue("@IDORDEN", orden.IdOrden);
                        command.Parameters.AddWithValue("@CANTIDAD", orden.Cantidad);
                        command.Parameters.AddWithValue("@IDPLATILLO", orden.IdPlatillo);
                        res = command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrio un error al intentar editar la orden" + ex.Message, "Validacion",
                        MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return res;
        }


    }


}
