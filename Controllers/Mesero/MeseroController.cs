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


namespace Rapid_Plus.Controllers.Mesero
{
    class MeseroController
    {
        public static List<OrdenesModel> ListarOrdenes()
        {
            List<OrdenesModel> lstOrdenes = new List<OrdenesModel>();

            try
            {
                using (var con = new SqlConnection(Properties.Settings.Default.DbRapidPlus))
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
                                ordenes.Mesa = dr["MESA"].ToString();
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

    }


}
