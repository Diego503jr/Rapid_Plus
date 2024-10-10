using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Incluimos las librerias
using System.Data;
using System.Data.SqlClient;
using Rapid_Plus.Models;
using System.Windows;
using System.Data.Common;

namespace Rapid_Plus.Controllers
{
    class UsuarioControlador
    {
        public static List<UsuarioModelo> MostrarUsuarios() 
        {
            List<UsuarioModelo> lstUsuarios = new List<UsuarioModelo>();

            try
            {
                using (var conDb = new SqlConnection(Properties.Settings.Default.DbRapidPlus)) 
                {
                    conDb.Open();
                    using (var command = conDb.CreateCommand()) 
                    { 
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "SPMOSTRARUSUARIOS";

                        using (DbDataReader dr = command.ExecuteReader()) 
                        {
                            while (dr.Read()) 
                            {
                                UsuarioModelo usuario = new UsuarioModelo();
                                usuario.UsuarioId = int.Parse(dr["Id"].ToString());
                                usuario.Usuario = dr["Usuario"].ToString();
                                usuario.Clave = dr["Clave"].ToString();
                                usuario.Nombres = dr["Nombre_Usuario"].ToString();
                                usuario.Apellidos = dr["Apellido_Usuario"].ToString();
                                usuario.Rol = dr["Rol"].ToString();
                                usuario.DUI = int.Parse(dr["DUI"].ToString());
                                usuario.Sexo = dr["Sexo"].ToString();
                                usuario.FechaNacimiento = dr["Fecha_Nacimiento"].ToString();

                                //Agregar a la lista
                                lstUsuarios.Add(usuario);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrio un error al intentar mostrar los registros" + ex.Message, "Validacion",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return lstUsuarios;
        }
    }
}
