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

        //MOSTRAR DATOS
        public static List<EstadoModel> VerOrdenes(int? IdEstado = null)
        {
            List<EstadoModel> lstEstados = new List<EstadoModel>();

            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    con.Open();
                    using (var command = con.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "SPMOSTRARORDENESPENDIENTES";

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
                                EstadoModel estado = new EstadoModel();
                                estado.IdOrden = int.Parse(dr["ORDEN"].ToString());
                                estado.Orden = dr["PLATILLO"].ToString();
                                estado.EstadoOrden = dr["ESTADO"].ToString();

                                //Agregar a la lista inicial
                                lstEstados.Add(estado);
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrio un error al intentar mostrar los registros:" + ex.Message, "Validaccion", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return lstEstados;

        }
        //EDITAR ESTADO DE LA ORDEN
        public static int EditarEstadoOrden(EstadoModel estado, int idOrden)
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
