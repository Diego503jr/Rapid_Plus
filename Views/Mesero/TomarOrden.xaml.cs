﻿using Rapid_Plus.Controllers;
using Rapid_Plus.Models;
using Rapid_Plus.Models.Mesero;
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
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Rapid_Plus.Views.Mesero
{
    /// <summary>
    /// Lógica de interacción para TomarOrden.xaml
    /// </summary>
    public partial class TomarOrden : Page
    {
        public TomarOrden()
        {
            InitializeComponent();

        }

        #region VARIABLES LOCALES
        int idMesa = -1;
        int idOrden = -1;
        int idplatillo = -1;
        int idPlatilloOrden = -1; //Para datagrid platillos por orden
        int idCategoria = -1;
        int idDetalleOrden = -1;
        bool agregando = false, editando = false;
        #endregion

        #region MÉTODOS PERSONALIZADOS
        //Muestra mesas asignadas (ocupadas)
        private void CargarNumeroMesa()
        {
            using (var conDb = new SqlConnection(Properties.Settings.Default.DbRapidPlus))
            {
                conDb.Open();
                using (var command = new SqlCommand("SELECT IdMesa, Mesa FROM Mesa WHERE IdEstado = 0", conDb))
                {
                    SqlDataReader dr = command.ExecuteReader();
                    var mesas = new List<dynamic>();
                    while (dr.Read())
                    {
                        mesas.Add(new { Id = dr.GetInt32(0), Mesa = dr.GetInt32(1) });
                    }

                    cmbMesa.ItemsSource = mesas;
                }
            }

            //Define que campos mostrar
            cmbMesa.DisplayMemberPath = "Mesa";
            cmbMesa.SelectedValuePath = "Id";
        }

        //Muestra categorías de platillos
        private void CargarCategorias()
        {
            using (var conDb = new SqlConnection(Properties.Settings.Default.DbRapidPlus))
            {
                conDb.Open();
                using (var command = new SqlCommand("SELECT IdCategoria, Categoria FROM Categoria", conDb))
                {
                    SqlDataReader dr = command.ExecuteReader();
                    var estados = new List<dynamic>();
                    while (dr.Read())
                    {
                        estados.Add(new { Id = dr.GetInt32(0), Categoria = dr.GetString(1) });
                    }

                    cmbPlatillo.ItemsSource = estados;
                }
            }
            cmbPlatillo.DisplayMemberPath = "Categoria";
            cmbPlatillo.SelectedValuePath = "Id";

        }

        //Limpia objetos 
        private void LimpiarObjetos()
        {
            cmbMesa.SelectedIndex = -1;
            cmbPlatillo.SelectedIndex = -1;
            txtCantidad.Clear();
            idPlatilloOrden = -1;
            txbEstado.Text = null;
            txbOrden.Text = null;
            txbPlatillo.Text = null;
            dgPlatillos.DataContext = null;
            dgOrdenes.DataContext = null;
            dgOrdenes.IsEnabled = false;

        }

        //Obtiene le número de mesa seleccionado en el combobox
        private int IdMesa()
        {

            if (cmbMesa.SelectedIndex != -1)
            {
                idMesa = (int)cmbMesa.SelectedValue;
            }
            else
            {
                idMesa = -1;
            }
            return idMesa;
        }

        //Valida campos completos del formulario
        private bool ValidarFomrulario()
        {
            bool estado = true;
            string mensaje = null;

            if (string.IsNullOrEmpty(txbOrden.Text))
            {
                estado = false;
                mensaje += "Número de orden\n";
            }
            if (idplatillo == -1 && idPlatilloOrden == -1)
            {
                estado = false;
                mensaje += "Platillo\n";
            }
            if (string.IsNullOrEmpty(txtCantidad.Text))
            {
                estado = false;
                mensaje += "Cantidad";
            }

            if (!estado)
            {
                MessageBox.Show("Debe completar los campos:\n" + mensaje, "Validación de formulario", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return estado;
        }

        //Activa o desactiva campos y botones
        private void ControlAcciones()
        {
            bool accion = agregando || editando;

            btnGuardar.IsEnabled = accion;
            btnCancelar.IsEnabled = accion;
            btnNuevo.IsEnabled = !accion;
            btnEditar.IsEnabled = !agregando && !editando;
            btnEliminar.IsEnabled = editando;

            txtCantidad.IsEnabled = accion;
            cmbMesa.IsEnabled = accion;
            cmbPlatillo.IsEnabled = agregando;
            dgPlatillos.IsEnabled = agregando;
            dgOrdenes.IsEnabled = !agregando;

        }

        #endregion

        #region EVENTOS

        //Cargas por defecto
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LimpiarObjetos();
            CargarNumeroMesa();
            CargarCategorias();
            agregando = false;
            editando = false;
            ControlAcciones();

        }

        //Muestra elementos según mesa seleccionada
        private void cmbMesa_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            idMesa = IdMesa();
            var detalle = DetalleOrdenController.ObtenerDetalleOrden(idMesa);
            var ordenes = OrdenController.MostrarOrdenesPorMesa(idMesa);

            if (detalle != null)
            {
                OrdenController.MostrarOrdenesPorMesa(idMesa);
                dgOrdenes.DataContext = ordenes;

                txbOrden.Text = detalle.IdOrden.ToString();
                txbEstado.Text = detalle.EstadoOrden;
            }
            else
            {
                txbOrden.Text = string.Empty;
            }
        }

        //Muestra elementos según categoria de platillo seleccionada
        private void cmbPlatillo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbPlatillo.SelectedIndex != -1)
            {
                idCategoria = (int)cmbPlatillo.SelectedValue;
                var platillos = PlatilloController.MostrarPlatillos(idCategoria);
                if (platillos != null)
                {
                    dgPlatillos.DataContext = platillos;

                }
                else
                {
                    MessageBox.Show("No hay Platillos disponibles.");
                }
            }
            
        }

        //Obtiene información de registros seleccionados
        private void dgPlatillos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            PlatilloModel platillo = (PlatilloModel)dgPlatillos.SelectedItem;
            if (platillo == null)
            {
                return;
            }
            txbPlatillo.Text = platillo.Platillo;
            idplatillo = platillo.PlatilloId;

        }
        private void dgOrdenes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            OrdenesModel ordenes = (OrdenesModel)dgOrdenes.SelectedItem;
            if (ordenes == null)
            {
                return;
            }
            txtCantidad.Text = ordenes.Cantidad.ToString();
            txbPlatillo.Text = ordenes.NombrePlatillo;

            idPlatilloOrden = ordenes.IdPlatilloOrden;
            idDetalleOrden = ordenes.IdDetalleOrden;
            idOrden = ordenes.IdOrden;
        }

        //BOTONES
        private void btnNuevo_Click(object sender, RoutedEventArgs e)
        {
            //Agregando
            agregando = true;
            editando = false;

            //Método para activar y desactivar campos y botones
            ControlAcciones();
        }
        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            string mensaje = null;
            if (ValidarFomrulario())
            {
                DetalleOrdenModel detalle = new DetalleOrdenModel();
                detalle.IdOrden = Convert.ToInt32(txbOrden.Text);
                detalle.Cantidad = Convert.ToInt32(txtCantidad.Text);
                detalle.IdEstado = 1;
                detalle.IdPlatillo = idplatillo;
                detalle.IdPlatilloOrden = idPlatilloOrden;

                if (agregando)
                {
                    idOrden = DetalleOrdenController.CrearDetalleOrden(detalle);
                    mensaje = "Orden creada con éxito";
                }
                else
                {
                    idOrden = DetalleOrdenController.ActualizarDetalleOrden(detalle, idDetalleOrden);
                    mensaje = "Orden actualizada";
                }

                if (idOrden > 0)
                {

                    MessageBox.Show(mensaje, "Validación de formulario", MessageBoxButton.OK, MessageBoxImage.Information);
                    agregando = false;
                    editando = false;
                    ControlAcciones();
                }
                agregando = false;
                editando = false;
                ControlAcciones();
                LimpiarObjetos();

            }
        }
        private void btnEditar_Click(object sender, RoutedEventArgs e)
        {
            //Editando
            agregando = false;
            editando = true;

            //Método para activar y desactivar campos y botones
            ControlAcciones();
        }
        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Desea cancelar la operación", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                LimpiarObjetos();
                agregando = false;
                editando = false;
                ControlAcciones();
                
            }
        }
        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (idDetalleOrden > 0 && idOrden > 0 && idPlatilloOrden != -1)
            {
                if (MessageBox.Show("¿Desea eliminar el detalle de la orden?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {

                    DetalleOrdenController.EliminarDetalleOrden(idDetalleOrden, idOrden);
                    MessageBox.Show("Detalle de orden eliminado con éxito.", "Confirmación", MessageBoxButton.OK, MessageBoxImage.Information);

                    LimpiarObjetos();
                    agregando = false;
                    editando = false;
                    ControlAcciones();
                }
                else
                {
                    LimpiarObjetos();
                }
            }
            else
            {
                MessageBox.Show("Seleccione un detalle de orden válido para eliminar.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                agregando = false;
                editando = true;
                ControlAcciones();
            }
        }
        #endregion


    }
}
