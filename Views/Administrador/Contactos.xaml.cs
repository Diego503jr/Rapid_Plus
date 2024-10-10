using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
// Incluimos librerias SQL
using System.Data.SqlClient;
using System.Data;
using Rapid_Plus.Controllers;
using Rapid_Plus.Models;

namespace Rapid_Plus.Views.Administrador
{
    /// <summary>
    /// Lógica de interacción para Contactos.xaml
    /// </summary>
    public partial class Contactos : Page
    {
        public Contactos()
        {
            InitializeComponent();
            MostrarUsuarios();
            CargarSexos();
            CargarRoles();
        }

        #region VARIABLES LOCALES
        //Conexion a la DB
        SqlConnection conDB = new SqlConnection(Properties.Settings.Default.DbRapidPlus);

        //Variables de estado
        private bool agregar = false, editar = false;

        //Variable para almacenar el id
        private int idUsuario = 0;
        #endregion

        #region METODOS PERSONALIZADOS
        //Metodo para habilitar campos
        void HabilitarFormulario(bool accion) 
        { 
            txtUsuario.IsEnabled = accion;
            txtClave.IsEnabled = accion;
            txtNombre.IsEnabled = accion;
            txtApellido.IsEnabled = accion;
            cmbRol.IsEnabled = accion;
            txtDUI.IsEnabled = accion;
            cmbSexo.IsEnabled = accion;
            dtpFechaNacimiento.IsEnabled = accion;
        }

        //Metodo para limpiar campos
        void LimpiarFormulario()
        {
            txtUsuario.Clear();
            txtClave.Clear();
            txtNombre.Clear();
            txtApellido.Clear();
            cmbRol.SelectedItem = null;
            txtDUI.Clear();
            cmbSexo.SelectedItem = null;
            dtpFechaNacimiento.SelectedDate = null;
        }

        //Mostrar Usuarios
        void MostrarUsuarios() 
        {
            dgUsuarios.DataContext = UsuarioControlador.MostrarUsuarios();
        }

        //Metodos para cargar en los comboboxes
        private void CargarRoles()
        {
            using (var conDb = new SqlConnection(Properties.Settings.Default.DbRapidPlus))
            {
                conDb.Open();
                using (var command = new SqlCommand("SELECT Id, Rol FROM Roles", conDb))
                {
                    SqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        cmbRol.Items.Add(new { Id = dr.GetInt32(0), Nombre = dr.GetString(1) });
                    }
                }
            }

            // Define qué campo mostrar
            cmbRol.DisplayMemberPath = "Nombre";
            cmbRol.SelectedValuePath = "Id";
        }
        private void CargarSexos()
        {
            using (var conDb = new SqlConnection(Properties.Settings.Default.DbRapidPlus))
            {
                conDb.Open();
                using (var command = new SqlCommand("SELECT Id, Sexo FROM Sexo", conDb))
                {
                    SqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        cmbSexo.Items.Add(new { Id = dr.GetInt32(0), Nombre = dr.GetString(1) });
                    }
                }
            }

            // Define qué campo mostrar
            cmbSexo.DisplayMemberPath = "Nombre";
            cmbSexo.SelectedValuePath = "Id";
        }

        #endregion

        #region METODOS FORMULARIO

        //Para llenar el formulario
        private void dgUsuarios_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UsuarioModelo usuario = (UsuarioModelo)dgUsuarios.SelectedItem;

            if (usuario == null)
            {
                return;
            }

            //Llenamos los campos
            idUsuario = usuario.UsuarioId;
            txtUsuario.Text = usuario.Usuario;
            txtClave.Password = usuario.Clave;
            txtNombre.Text = usuario.Nombres;
            txtApellido.Text = usuario.Apellidos;
            cmbRol.Text = usuario.Rol;
            txtDUI.Text = Convert.ToString(usuario.DUI);
            cmbSexo.Text = usuario.Sexo;
            dtpFechaNacimiento.Text = usuario.FechaNacimiento;
        }
        #endregion
    }
}
