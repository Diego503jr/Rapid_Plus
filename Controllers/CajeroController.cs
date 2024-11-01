﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using Rapid_Plus.Models;
using Rapid_Plus.Models.Mesero;
using System.Data.Common;
using System.Windows;
using System.Data.SqlClient;
using System.Data;
using Rapid_Plus.Models;


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



    }
}
