﻿using Rapid_Plus.Controllers;
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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Rapid_Plus.Views.Mesero
{
    /// <summary>
    /// Lógica de interacción para VerOrden.xaml
    /// </summary>
    public partial class VerOrden : Page
    {
        public VerOrden()
        {
            InitializeComponent();
           
        }

        #region DECLARACION DE VARIABLES LOCALES
        int IdEstadoOrden = -1;
        #endregion


        #region MÉTODOS PERSONALIZADOS
        //Lista las ordenes en un datagrid
        void MostrarOrdenes()
        {
            dgOrdenes.DataContext = OrdenController.MostrarOrdenes();
        }
        #endregion

        #region EVENTOS

        //Carga por defecto
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            MostrarOrdenes();
           
        }

    }
    #endregion


}
