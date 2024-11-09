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
