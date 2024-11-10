using Rapid_Plus.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Rapid_Plus.Controllers
{
    internal class DetalleOrdenController
    {
        private static string conexion = Properties.Settings.Default.DbRapidPlus;


        //AGREGAR PLATILLOS A LA ORDEN
        public static int CrearDetalleOrden(DetalleOrdenModel detalle)
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
                        if (detalle.Cantidad > 0)
                        {
                            command.Parameters.AddWithValue("@IDORDEN", detalle.IdOrden);
                            command.Parameters.AddWithValue("@CANTIDAD", detalle.Cantidad);
                            command.Parameters.AddWithValue("@IDESTADO", detalle.IdEstado);
                            command.Parameters.AddWithValue("@IDPLATILLO", detalle.IdPlatillo);


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
        //MOSTRAR ESTADO Y NÚMERO DE ORDEN
        public static DetalleOrdenModel ObtenerDetalleOrden(int idMesa)
        {
            DetalleOrdenModel detalle = null;

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
                            detalle = new DetalleOrdenModel
                            {
                                IdOrden = reader.GetInt32(reader.GetOrdinal("IDORDEN")),
                                EstadoOrden = reader.GetString(reader.GetOrdinal("ESTADOORDEN"))
                            };
                        }
                    }
                }
            }

            return detalle;
        }

        //ACTUALIZAR CANTIDAD DE PLATILLO
        public static int ActualizarDetalleOrden(DetalleOrdenModel detalle, int idDetalleOrden)
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
                        if (detalle.Cantidad > 0)
                        {
                            command.Parameters.AddWithValue("@IDDETALLEORDEN", idDetalleOrden);
                            command.Parameters.AddWithValue("@IDORDEN", detalle.IdOrden);
                            command.Parameters.AddWithValue("@CANTIDAD", detalle.Cantidad);
                            command.Parameters.AddWithValue("@IDESTADO", detalle.IdEstado);
                            command.Parameters.AddWithValue("@IDPLATILLO", detalle.IdPlatilloOrden);
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
                MessageBox.Show("Ocurrio un error al intentar editar la orden: " + ex.Message, "Validacion",
                        MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return res;
        }

        //ELIMINAR UN PLATILLO DEL DETALLE
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
                MessageBox.Show("Ocurrio un error al intentar eliminar el detalle de la orden: " + ex.Message, "Validacion",
                        MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return res;
        }
    }
}
