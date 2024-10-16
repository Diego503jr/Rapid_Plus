﻿using System;
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
using System.Windows.Shapes;

namespace Rapid_Plus.Views.Administrador
{
    /// <summary>
    /// Lógica de interacción para DashboardAdmin.xaml
    /// </summary>
    public partial class DashboardAdmin : Window
    {
        public DashboardAdmin()
        {
            InitializeComponent();
        }

        // INSTANCIAS DE LAS PAGINAS
        Contactos contacto = new Contactos();
        Menu menu = new Menu();
        Configuraciones configuracion = new Configuraciones();


        #region METODOS ESTILOS
        private void btnMenu_MouseEnter(object sender, MouseEventArgs e)
        {
            btnMenu.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFD63C3C"));
        }

        private void btnMenu_MouseLeave(object sender, MouseEventArgs e)
        {
            btnMenu.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF008592"));
        }

        private void btnUsuario_MouseEnter(object sender, MouseEventArgs e)
        {
            btnUsuario.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFD63C3C"));
        }

        private void btnUsuario_MouseLeave(object sender, MouseEventArgs e)
        {
            btnUsuario.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF008592"));
        }

        private void btnReportes_MouseEnter(object sender, MouseEventArgs e)
        {
            btnReportes.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFD63C3C"));
        }

        private void btnReportes_MouseLeave(object sender, MouseEventArgs e)
        {
            btnReportes.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF008592"));
        }

        private void btnCerrarSesion_MouseEnter(object sender, MouseEventArgs e)
        {
            btnCerrarSesion.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFD63C3C"));
        }

        private void btnCerrarSesion_MouseLeave(object sender, MouseEventArgs e)
        {
            btnCerrarSesion.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF008592"));
        }

        private void btnMas_MouseEnter(object sender, MouseEventArgs e)
        {
            btnMas.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFD63C3C"));
        }

        private void btnMas_MouseLeave(object sender, MouseEventArgs e)
        {
            btnMas.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF008592"));
        }

        #endregion

        #region METODOS FORMULARIO

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            frContent.NavigationService.Navigate(contacto);
        }
        private void btnMenu_Click(object sender, RoutedEventArgs e)
        {
            frContent.NavigationService.Navigate(menu);
        }

        private void btnUsuario_Click(object sender, RoutedEventArgs e)
        {
            frContent.NavigationService.Navigate(contacto);
        }

        private void btnMas_Click(object sender, RoutedEventArgs e)
        {
            frContent.NavigationService.Navigate(configuracion);
        }

        private void btnCerrarVentana_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnCerrarSesion_Click(object sender, RoutedEventArgs e)
        {
            if (
                MessageBox.Show("Desea Cerrar Sesión?",
                "Cerrar sesión",
                MessageBoxButton.YesNo,
                MessageBoxImage.Information) == MessageBoxResult.Yes)
            {
                MainWindow login = new MainWindow();
                login.Show();
                this.Close();
            }
        }

        #endregion
    }
}
