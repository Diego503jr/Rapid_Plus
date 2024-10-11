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
            LimpiarFormulario();
        }

        #region VARIABLES LOCALES
        //Conexion a la DB
        SqlConnection conDB = new SqlConnection(Properties.Settings.Default.DbRapidPlus);

        //Variables de estado
        private bool agregar = false, editar = false;

        //Variable para almacenar el id
        private int idUsuario = 0;
        //Variable para almaccenar el idEstado
        private int idEstado = 0;
        #endregion

        #region METODOS PERSONALIZADOS

        //Validar el formulario
        bool ValidarFormulario()
        {
            bool estado = true;
            string msj = null;

            if (string.IsNullOrEmpty(txtUsuario.Text))
            {
                estado = false;
                msj = "Nombre Usuario\n";
            }

            if (string.IsNullOrEmpty(txtClave.Password))
            {
                estado = false;
                msj = "Clave Usuario\n";
            }

            if (string.IsNullOrEmpty(txtNombre.Text))
            {
                estado = false;
                msj = "Nombres Usuario\n";
            }

            if (string.IsNullOrEmpty(txtApellido.Text))
            {
                estado = false;
                msj = "Apellidos Usuario\n";
            }

            if (string.IsNullOrEmpty(cmbRol.Text))
            {
                estado = false;
                msj = "Rol Usuario\n";
            }

            if (string.IsNullOrEmpty(txtDUI.Text))
            {
                estado = false;
                msj = "DUI Usuario\n";
            }

            if (string.IsNullOrEmpty(cmbSexo.Text))
            {
                estado = false;
                msj = "Sexo Usuario\n";
            }

            if (string.IsNullOrEmpty(dtpFechaNacimiento.Text))
            {
                estado = false;
                msj = "Fecha de Nacimiento Usuario\n";
            }

            if (string.IsNullOrEmpty(cmbEstado.Text))
            {
                estado = false;
                msj = "Estado Usuario\n";
            }

            if (!estado)
            {
                MessageBox.Show("Debe cumplir estos campos:\n" + msj,
               "Validacion de formulario", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return estado;
        }

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
            lbFecha.IsEnabled = accion;
            cmbEstado.IsEnabled = accion;
        }

        //Metodo para manejar botones del CRUD
        void ControlFormulario() 
        {
            if (dgUsuarios.Items.Count < 0)
            {
                btnNuevo.IsEnabled = true;
                btnEditar.IsEnabled = false;
                btnEliminar.IsEnabled = false;

                btnGuardar.IsEnabled = false;
                btnCancelar.IsEnabled = false;
            }
            else
            {
                btnNuevo.IsEnabled = true;
                btnEditar.IsEnabled = true;
                btnEliminar.IsEnabled = true;

                btnGuardar.IsEnabled = false;
                btnCancelar.IsEnabled = false;
            }

            if (agregar || editar)
            {
                btnNuevo.IsEnabled = false;
                btnEditar.IsEnabled = false;
                btnEliminar.IsEnabled = false;

                btnGuardar.IsEnabled = true;
                btnCancelar.IsEnabled = true;
            }
        }

        //Metodo para limpiar campos
        void LimpiarFormulario()
        {
            txtUsuario.Clear();
            txtClave.Clear();
            txtNombre.Clear();
            txtApellido.Clear();
            cmbRol.SelectedIndex = -1;
            txtDUI.Clear();
            cmbSexo.SelectedIndex = -1;
            dtpFechaNacimiento.SelectedDate = null;
            cmbEstado.SelectedIndex = -1;
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

        private void CargarEstados()
        {
            using (var conDB = new SqlConnection(Properties.Settings.Default.DbRapidPlus))
            { 
                conDB.Open();
                using (var command = new SqlCommand("SELECT Id, Estado FROM Estados", conDB))
                {
                    SqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        cmbEstado.Items.Add(new { Id = dr.GetInt32(0), Nombre = dr.GetString(1) });
                    }
                }
            }

            //Definir en que campos mostrar
            cmbEstado.DisplayMemberPath = "Nombre";
            cmbEstado.SelectedValuePath = "Id";
        }

        //Metodo para cargar la pagina
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LimpiarFormulario();
            MostrarUsuarios();
            CargarSexos();
            CargarRoles();
            CargarEstados();
            HabilitarFormulario(false);
            ControlFormulario();
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
            dtpFechaNacimiento.Text = Convert.ToString(usuario.FechaNacimiento);
            cmbEstado.Text = usuario.Estado;
        }

        //Boton para manejar el CRUD
        private void btnNuevo_Click(object sender, RoutedEventArgs e)
        {
            HabilitarFormulario(true);

            LimpiarFormulario();

            agregar = true;
            editar = false;

            ControlFormulario();

            txtUsuario.Focus();
        }

        private void btnEditar_Click(object sender, RoutedEventArgs e)
        {
            HabilitarFormulario(true);

            agregar = false;
            editar = true;

            ControlFormulario();

            txtUsuario.Focus();
        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            //Verificar si hay registro
            if (dgUsuarios.Items.Count > 0 && !String.IsNullOrEmpty(txtUsuario.Text))
            {
                //Confirmar
                if (
                MessageBox.Show("¿ Desear eliminar al usuario '"+idUsuario+"' ?",
                "Accion",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question) == MessageBoxResult.Yes
                )
                {
                    if (UsuarioControlador.EliminarUsuario(idUsuario, idEstado) > -1)
                    {
                        MessageBox.Show("Registro eliminado correctamente", "Validacion",
                           MessageBoxButton.OK, MessageBoxImage.Information);

                        LimpiarFormulario();

                        MostrarUsuarios();

                        agregar = false;
                        editar = false;

                        ControlFormulario();
                    }
                }
            }
            else 
            {
                MessageBox.Show("Selecciona un usuario para eliminar",
                "Accion",
                MessageBoxButton.OK, MessageBoxImage.Error );
            }
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            string msj = null;

            //Validamos el formulario
            if (ValidarFormulario())
            {
                //Recuperamos los datos del formulario
                UsuarioModelo usuario = new UsuarioModelo();
                usuario.Usuario = txtUsuario.Text;
                usuario.Clave = txtClave.Password;
                usuario.Nombres = txtNombre.Text;
                usuario.Apellidos = txtApellido.Text;
                usuario.RolId = (int)cmbRol.SelectedValue;
                usuario.DUI = Convert.ToInt32(txtDUI.Text);
                usuario.SexoId = (int)cmbSexo.SelectedValue;
                usuario.FechaNacimiento = DateTime.Parse(dtpFechaNacimiento.Text);
                usuario.EstadoId = (int)cmbEstado.SelectedValue;

                //Evaluar si se esta agregando o editando
                if (agregar)
                {
                    idUsuario = UsuarioControlador.CrearUsuario(usuario);
                    msj = "Insercion correctamente";
                }
                else
                {
                    idUsuario = UsuarioControlador.EditarUsuario(usuario, idUsuario);
                    msj = "Actualizacion correctamente";
                }

                //Evaluar si se ingreso todo correctamente
                if (idUsuario > 0)
                {
                    //Limpiar los campos
                    LimpiarFormulario();

                    MessageBox.Show(msj, "Validacion del formulario",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);

                    //Actualizar variables
                    agregar = false;
                    editar = false;

                    HabilitarFormulario(false);

                    ControlFormulario();
                }
                //Actualizar el dt
                MostrarUsuarios();
            }
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            //Validamos si quiere confirmar la cancelacion
            if (
                MessageBox.Show("Desear cancelar la operacion",
                "Accion",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question) == MessageBoxResult.Yes
                )
            {
                LimpiarFormulario();

                HabilitarFormulario(false);

                agregar = false;
                editar = false;

                ControlFormulario();
            }
        }

        #endregion
    }
}
