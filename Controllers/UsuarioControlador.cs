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
        private static string conexion = Properties.Settings.Default.DbRapidPlus;

        public static List<UsuarioModelo> MostrarUsuarios() 
        {
            List<UsuarioModelo> lstUsuarios = new List<UsuarioModelo>();

            try
            {
                using (var conDb = new SqlConnection(conexion)) 
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
                                usuario.FechaNacimiento = DateTime.Parse(dr["Fecha_Nacimiento"].ToString());
                                usuario.Estado = dr["Estado"].ToString();

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

        public static int CrearUsuario(UsuarioModelo user) 
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
                        command.CommandText = "SPCREARUSUARIO";

                        command.Parameters.AddWithValue("@Usuario", user.Usuario);
                        command.Parameters.AddWithValue("@Clave", user.Clave);
                        command.Parameters.AddWithValue("@Nombres", user.Nombres);
                        command.Parameters.AddWithValue("@Apellidos", user.Apellidos);
                        command.Parameters.AddWithValue("@Id_Rol", user.RolId);
                        command.Parameters.AddWithValue("@DUI", user.DUI);
                        command.Parameters.AddWithValue("@Id_Sexo", user.SexoId);
                        command.Parameters.AddWithValue("@Fecha_Nacimiento", user.FechaNacimiento);
                        command.Parameters.AddWithValue("@Id_Estado", user.EstadoId);

                        res = command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrio un error al intentar crear los registros" + ex.Message, "Validacion",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return res;
        }

        public static int EditarUsuario(UsuarioModelo user, int idUsuario) 
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
                        command.CommandText = "SPACTUALIZARUSUARIO";

                        command.Parameters.AddWithValue("@Id", idUsuario);
                        command.Parameters.AddWithValue("@Usuario", user.Usuario);
                        command.Parameters.AddWithValue("@Clave", user.Clave);
                        command.Parameters.AddWithValue("@Nombres", user.Nombres);
                        command.Parameters.AddWithValue("@Apellidos", user.Apellidos);
                        command.Parameters.AddWithValue("@Id_Rol", user.RolId);
                        command.Parameters.AddWithValue("@DUI", user.DUI);
                        command.Parameters.AddWithValue("@Id_Sexo", user.SexoId);
                        command.Parameters.AddWithValue("@Fecha_Nacimiento", user.FechaNacimiento);
                        command.Parameters.AddWithValue("@Id_Estado", user.EstadoId);

                        res = command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show("Ocurrio un error al intentar editar los registros" + ex.Message, "Validacion",
                        MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return res;
        }

        public static int EliminarUsuario(int idUsuario, int idEstado) 
        { 
            int res = -1;

            try
            {
                using (var conDB = new SqlConnection(conexion)) 
                {
                    conDB.Open();

                    using (var command = conDB.CreateCommand()) 
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "SPELIMINARUSUARIO";

                        command.Parameters.AddWithValue("@Id", idUsuario);
                        command.Parameters.AddWithValue("@Id_Estado", idEstado);

                        res = command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show("Ocurrio un error al intentar editar los registros" + ex.Message, "Validacion",
                           MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return res;
        }
    }
}
