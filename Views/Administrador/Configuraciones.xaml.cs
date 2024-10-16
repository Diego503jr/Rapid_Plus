﻿using Rapid_Plus.Controllers;
using Rapid_Plus.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

namespace Rapid_Plus.Views.Administrador
{
    /// <summary>
    /// Lógica de interacción para Configuraciones.xaml
    /// </summary>
    public partial class Configuraciones : Page
    {
        public Configuraciones()
        {
            InitializeComponent();
            CargarEstados();
        }

        #region VARIABLES LOCALES

        private static string conexion = Properties.Settings.Default.DbRapidPlus;
        //Conexion a la DB
        SqlConnection conDB = new SqlConnection(conexion);

        //Variables de estado
        private bool agregar = false, editar = false;

        //Variable para almacenar el id
        private int idMesa = 0;
        //Variable para almaccenar el idEstado
        private int idEstado = 0;

        #endregion

        #region METODOS PERSONALIZADOS

        //Validar formulario 
        bool ValidarFormulario()
        {
            bool estado = true;
            string msj = null;

            if (string.IsNullOrEmpty(txtMesa.Text))
            {
                estado = false;
                msj = "Numero Mesa\n";

            }

            if (string.IsNullOrEmpty(cmbEstado.Text))
            {
                estado = false;
                msj = "Estado Mesa\n";

            }

            if (!estado)
            {
                MessageBox.Show("Debe cumplir estos campos:\n" + msj,
               "Validacion de formulario", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return estado;
        }

        //Habilitar formulario
        void HabilitarFormulario(bool accion)
        {
            txtMesa.IsEnabled = accion;
            cmbEstado.IsEnabled = accion;
        }

        //Controlar el formulario
        void ControlFormulario()
        {
            if (dgMesas.Items.Count < 0)
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

        //Limpiar el formulario
        void LimpiarFormulario()
        {
            txtMesa.Clear();
            cmbEstado.SelectedIndex = -1;
        }

        void MostrarMesas() 
        {
            dgMesas.DataContext = MesaController.MostrarMesa();
        }

        private void CargarEstados()
        {
            using (var conDB = new SqlConnection(conexion))
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

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            MostrarMesas();
            HabilitarFormulario(false);
            ControlFormulario();
            LimpiarFormulario();
        }

        #endregion

        #region METODOS FORMULARIO

        private void dgMesas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MesasModel mesa = (MesasModel)dgMesas.SelectedItem;

            if (mesa == null)
            {
                return;
            }

            idMesa = mesa.MesaId;
            txtMesa.Text = Convert.ToString(mesa.Mesa);
            cmbEstado.Text = mesa.Estado;
        }

        private void btnNuevo_Click(object sender, RoutedEventArgs e)
        {
            HabilitarFormulario(true);

            LimpiarFormulario();

            agregar = true;
            editar = false;

            ControlFormulario();

            txtMesa.Focus();
        }

        private void btnEditar_Click(object sender, RoutedEventArgs e)
        {
            HabilitarFormulario(true);

            agregar = false;
            editar = true;

            ControlFormulario();

            txtMesa.Focus();
        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (dgMesas.Items.Count > 0 && !string.IsNullOrEmpty(txtMesa.Text))
            {
                if (
                    MessageBox.Show("¿ Desear eliminar al usuario '" + idMesa + "' ?",
                "Accion",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question) == MessageBoxResult.Yes
                    )
                {
                    if (MesaController.EliminarMesa(idMesa, idEstado) > -1)
                    {
                        MessageBox.Show("Registro eliminado correctamente", "Validacion",
                          MessageBoxButton.OK, MessageBoxImage.Information);

                        LimpiarFormulario();

                        MostrarMesas();

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
                MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            string msj = null;

            if (ValidarFormulario())
            {
                //Recuperamos los datos
                MesasModel mesa = new MesasModel();
                mesa.Mesa= Convert.ToInt32(txtMesa.Text);
                mesa.EstadoId = (int)cmbEstado.SelectedValue;

                if (agregar)
                {
                    idMesa = MesaController.CrearMesa(mesa);
                    msj = "Insercion correctamente";
                }
                else
                {
                    idMesa = MesaController.EditarMesa(mesa, idMesa);
                    msj = "Actualizacion correctamente";
                }

                if (idMesa > 0)
                {
                    //Limpiar formulario
                    LimpiarFormulario();

                    MessageBox.Show(msj, "Validacion del formulario",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);

                    agregar = false;
                    editar = false;

                    HabilitarFormulario(false);

                    ControlFormulario();
                }

                MostrarMesas();
            }
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
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
