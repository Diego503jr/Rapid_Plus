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
    class UsuarioController
    {
        private static string conexion = Properties.Settings.Default.DbRapidPlus;

        //Leer Usuario
        public static List<UsuarioModel> MostrarUsuarios() 
        {
            List<UsuarioModel> lstUsuarios = new List<UsuarioModel>();

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
                                UsuarioModel usuario = new UsuarioModel();
                                usuario.UsuarioId = int.Parse(dr["IdUsuario"].ToString());
                                usuario.Usuario = dr["Usuario"].ToString();
                                usuario.Clave = dr["Clave"].ToString();
                                usuario.Nombres = dr["NombreUsuario"].ToString();
                                usuario.Apellidos = dr["ApellidoUsuario"].ToString();
                                usuario.Rol = dr["Rol"].ToString();
                                usuario.DUI = int.Parse(dr["DUI"].ToString());
                                usuario.Sexo = dr["Sexo"].ToString();
                                usuario.FechaNacimiento = DateTime.Parse(dr["FechaNacimiento"].ToString());
                                usuario.Estado = dr["Estado"].ToString();

                                //Agregar a la lista
                                lstUsuarios.Add(usuario);
                            }

                            // Mover al siguiente conjunto de resultados (teléfonos)
                            if (dr.NextResult())
                            {
                                // Segunda consulta: obtener los teléfonos de los usuarios
                                while (dr.Read())
                                {
                                    int usuarioId = int.Parse(dr["IdUsuario"].ToString());
                                    string telefono1 = dr["Telefono1"].ToString();
                                    string telefono2 = dr["Telefono2"].ToString();

                                    // Encontrar el usuario y asignarle el teléfono
                                    var usuario = lstUsuarios.FirstOrDefault(u => u.UsuarioId == usuarioId);

                                    if (usuario != null)
                                    {
                                        usuario.Telefono1 = telefono1;
                                        usuario.Telefono2 = telefono2;
                                    }
                                }
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

        //Crear Usuario
        public static int CrearUsuario(UsuarioModel user, int idEstado) 
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
                        command.Parameters.AddWithValue("@IdRol", user.RolId);
                        command.Parameters.AddWithValue("@DUI", user.DUI);
                        command.Parameters.AddWithValue("@IdSexo", user.SexoId);
                        command.Parameters.AddWithValue("@FechaNacimiento", user.FechaNacimiento);
                        command.Parameters.AddWithValue("@IdEstado", idEstado);
                        command.Parameters.AddWithValue("@Telefono1", user.Telefono1);
                        command.Parameters.AddWithValue("@Telefono2", user.Telefono2);

                        res = command.ExecuteNonQuery();

                        if (res < 0) 
                        {
                            throw new Exception(" Ya existe este usuario");
                        }
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

        //Editar Usuario
        public static int EditarUsuario(UsuarioModel user, int idUsuario) 
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

                        command.Parameters.AddWithValue("@IdUsuario", idUsuario);
                        command.Parameters.AddWithValue("@Usuario", user.Usuario);
                        command.Parameters.AddWithValue("@Clave", user.Clave);
                        command.Parameters.AddWithValue("@Nombres", user.Nombres);
                        command.Parameters.AddWithValue("@Apellidos", user.Apellidos);
                        command.Parameters.AddWithValue("@IdRol", user.RolId);
                        command.Parameters.AddWithValue("@DUI", user.DUI);
                        command.Parameters.AddWithValue("@IdSexo", user.SexoId);
                        command.Parameters.AddWithValue("@FechaNacimiento", user.FechaNacimiento);
                        command.Parameters.AddWithValue("@IdEstado", user.EstadoId);
                        command.Parameters.AddWithValue("@Telefono1", user.Telefono1);
                        command.Parameters.AddWithValue("@Telefono2", user.Telefono2);

                        res = command.ExecuteNonQuery();

                        if (res < 0)
                        {
                            throw new Exception(" Ya existe este usuario");
                        }
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

        //Eliminar Usuario
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

                        command.Parameters.AddWithValue("@IdUsuario", idUsuario);
                        command.Parameters.AddWithValue("@IdEstado", idEstado);

                        res = command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show("Ocurrio un error al intentar eliminar los registros" + ex.Message, "Validacion",
                           MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return res;
        }
    }
}
