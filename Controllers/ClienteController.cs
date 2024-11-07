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

namespace Rapid_Plus.Controllers
{
    internal class ClienteController
    {

        private static string conexion = Properties.Settings.Default.DbRapidPlus;
        
        //CREAR CLIENTE
        public static int CrearCliente(ClienteModel cliente)
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

        //MOSTRAR LISTADO DE CLIENTES
        public static List<ClienteModel> MostrarClientes()
        {
            List<ClienteModel> lstCliente = new List<ClienteModel>();

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
                                ClienteModel cliente= new ClienteModel();
                                cliente.IdCliente = int.Parse(dr["IDCLIENTE"].ToString());
                                cliente.NombreCliente = dr["NOMBRE"].ToString();
                                cliente.ApellidoCliente = dr["APELLIDO"].ToString();

                                //Agregar a la lista inicial
                                lstCliente.Add(cliente);
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrio un error al intentar mostrar los cliente:" + ex.Message, "Mostrar clientes", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return lstCliente;

        }

        //ACTUALIZAR DATOS DE CLIENTE
        public static int ActualizarCliente(ClienteModel cliente, int idCliente)
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
    }
}
