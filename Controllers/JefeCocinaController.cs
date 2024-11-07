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
using Rapid_Plus.Models;
using Rapid_Plus.Views.JefeDeCocina;

namespace Rapid_Plus.Controllers
{
    internal class JefeCocinaController
    {
        private static string conexion = Properties.Settings.Default.DbRapidPlus;
       
        //EDITAR ESTADO DE LA ORDEN
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
    

public static int EditarEstadoOrden(OrdenesModel estado, int idOrden)
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
                        command.CommandText = "SPEDITARESTADO";

                        command.Parameters.AddWithValue("@IDORDEN", idOrden); 
                        command.Parameters.AddWithValue("@IDESTADOORDEN", estado.IdEstadoOrden); 

                        res = command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrio un error al intentar editar el estado de la orden" + ex.Message, "Validacion",
                        MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return res;
        }
    }
}
