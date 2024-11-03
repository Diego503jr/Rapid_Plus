using Rapid_Plus.Models;
using Rapid_Plus.Models.Mesero;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Windows;


namespace Rapid_Plus.Controllers
{
    public class CajeroController
    {
        private static string conexion = Properties.Settings.Default.DbRapidPlus;

        //MOSTRAR ORDENES DE ACUERDO A LA MESA
        internal static List<OrdenesModel> MostrarOrdenPorMesa(int numeroMesa)
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
                        command.CommandText = "SPFACTURAPORMESA";
                        // Cambiar aquí para asegurarte de que el nombre es correcto
                        command.Parameters.AddWithValue("@NumeroMesa", numeroMesa);

                        using (DbDataReader dr = command.ExecuteReader())
                        {
                            // Recorrer el dataReader
                            while (dr.Read())
                            {
                                OrdenesModel ordenes = new OrdenesModel();
                                ordenes.IdOrden = int.Parse(dr["NumeroOrden"].ToString());
                                ordenes.NombreCliente = dr["Cliente"].ToString();
                                ordenes.NombreUsuario = dr["Usuario"].ToString();
                                ordenes.FechaOrden = Convert.ToDateTime(dr["FechaOrden"]);
                                ordenes.Orden = dr["Platillo"].ToString();
                                ordenes.PrecioPlatillo = decimal.Parse(dr["Precio"].ToString());
                                ordenes.Cantidad = int.Parse(dr["Cantidad"].ToString());
                                ordenes.Subtotal = decimal.Parse(dr["Subtotal"].ToString());

                                // Agregar a la lista inicial
                                lstOrdenes.Add(ordenes);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error al intentar mostrar los registros: " + ex.Message, "Validación", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return lstOrdenes;
        }

        //MOSTRAR UNICAMENTE LAS MESAS LISTAS
        internal static List<MesasModel> ObtenerMesas()
        {
            List<MesasModel> mesas = new List<MesasModel>();

            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    con.Open();
                    using (var command = con.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "SPOBTENERMESASLISTAS"; // Asegúrate de que el nombre es correcto

                        using (DbDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                mesas.Add(new MesasModel
                                {
                                    Mesa = reader.GetInt32(0) // Suponiendo que la mesa es el primer campo en la consulta
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error al intentar mostrar los registros de mesas: " + ex.Message, "Validación", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return mesas;
        }

        //ACTUALIZAR/FACTURAR LA ORDEN QUE ESTE LISTA
        public static void CambiarEstadoOrden(int idOrden)
        {
            using (var con = new SqlConnection(conexion))
            {
                con.Open();
                using (var command = con.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "SPCAMBIARESTADOORDEN"; // Asegúrate de que el nombre es correcto
                    command.Parameters.AddWithValue("@IdOrden", idOrden);

                    try
                    {
                        command.ExecuteNonQuery();
                        MessageBox.Show("El estado de la orden ha sido actualizado.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ocurrió un error al intentar cambiar el estado de la orden: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }



    }
}
