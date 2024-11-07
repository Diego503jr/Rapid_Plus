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

        //ORDENES
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
                            throw new Exception("No fue posible crear la orden.");
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
                                ordenes.Mesa =int.Parse(dr["MESA"].ToString()); //Número de mesa
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


        //PLATILLOS
        public static List<OrdenesModel> MostrarPlatillos(int? IdCategoria = null)
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
                                ordenes.IdPlatillo = int.Parse(dr["IDPLATILLO"].ToString());
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


        //CLIENTES
        public static int CrearCliente(OrdenesModel cliente)
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
                        command.CommandText = "SPCREARCLIENTE";

                        command.Parameters.AddWithValue("@NOMBRECLIENTE", cliente.NombreCliente);
                        command.Parameters.AddWithValue("@APELLIDOCLIENTE", cliente.ApellidoCliente);

                        res = command.ExecuteNonQuery();
                        if (res <= 0)
                        {
                            throw new Exception("Cliente ya existe.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Validación cliente",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return res;
        }
        public static List<OrdenesModel> MostrarClientes()
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
                MessageBox.Show("Ocurrio un error al intentar mostrar los cliente:" + ex.Message, "Mostrar clientes", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return lstOrdenes;

        }
        public static int ActualizarCliente(OrdenesModel cliente, int idCliente)
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
                        command.CommandText = "SPACTUALIZARCLIENTE";

                        command.Parameters.AddWithValue("@IDCLIENTE", idCliente);
                        command.Parameters.AddWithValue("@NOMBRECLIENTE", cliente.NombreCliente);
                        command.Parameters.AddWithValue("@APELLIDOCLIENTE", cliente.ApellidoCliente);
                        res = command.ExecuteNonQuery();
                        if (res <= 0)
                        {
                            throw new Exception("Cliente ya existe.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrio un error al intentar editar cliente: " + ex.Message, "Validación cliente",
                        MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return res;
        }
        

        //DETALLE ORDEN
        public static OrdenesModel ObtenerDetalleOrden(int idMesa)
        {
            OrdenesModel orden = null;

            using (var conDb = new SqlConnection(conexion))
            {
                conDb.Open();

                using (var command = conDb.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "SPOBTENERDETALLEORDEN";
                    command.Parameters.AddWithValue("@IDMESA", idMesa);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            orden = new OrdenesModel
                            {
                                IdOrden = reader.GetInt32(reader.GetOrdinal("IDORDEN")),
                                EstadoOrden = reader.GetString(reader.GetOrdinal("ESTADOORDEN"))
                            };
                        }
                    }
                }
            }

            return orden;
        }
        public static int CrearDetalleOrden(OrdenesModel orden)
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
                        command.CommandText = "SPCREARDETALLEORDEN";

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
                                throw new Exception("Platillo ya existente en la orden.");
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
        public static int ActualizarDetalleOrden(OrdenesModel orden, int idOrden)
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
                        command.CommandText = "SPACTUALIZARORDEN";
                        if (orden.Cantidad > 0)
                        {
                            command.Parameters.AddWithValue("@IDDETALLEORDEN", idOrden);
                            command.Parameters.AddWithValue("@IDORDEN", orden.IdOrden);
                            command.Parameters.AddWithValue("@CANTIDAD", orden.Cantidad);
                            command.Parameters.AddWithValue("@IDESTADO", orden.IdEstado);
                            command.Parameters.AddWithValue("@IDPLATILLO", orden.IdPlatilloOrden);
                            res = command.ExecuteNonQuery();

                            if (res <= 0)
                            {
                                throw new Exception("Platillo ya existente en la orden.");
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
                MessageBox.Show("Ocurrio un error al intentar editar la orden " + ex.Message, "Validacion",
                        MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return res;
        }
        public static int EliminarDetalleOrden(int idDetalleOrden, int idOrden)
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
                        command.CommandText = "SPELIMINARDETALLEORDEN";

                        command.Parameters.AddWithValue("@IDDETALLEORDEN", idDetalleOrden);
                        command.Parameters.AddWithValue("@IDORDEN", idOrden);
                        res = command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrio un error al intentar eliminar el detalle de la orden" + ex.Message, "Validacion",
                        MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return res;
        }


    }


}
