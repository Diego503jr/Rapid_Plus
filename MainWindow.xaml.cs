using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using Rapid_Plus.Models;
using Rapid_Plus.Views.Administrador;
using Rapid_Plus.Views.Mesero;
using Rapid_Plus.Views.Cajero;
using Rapid_Plus.Views.JefeDeCocina;
using System;

namespace Rapid_Plus
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        SqlConnection conDb = new SqlConnection(Properties.Settings.Default.DbRapidPlus);

        string consultaSQL = null;
        #region METODO
        void EncontrarUsuario()
        {
            int resultado = 0;

            if (conDb.State == ConnectionState.Closed)
            {
                conDb.Open();

                consultaSQL = "SELECT dbo.FNENCONTRARUSUARIO(@User, @Password)";
                SqlCommand sqlCmd = new SqlCommand(consultaSQL, conDb);
                sqlCmd.CommandType = CommandType.Text;

                sqlCmd.Parameters.AddWithValue("@User", txtCorreo.Text.Trim());
                sqlCmd.Parameters.AddWithValue("@Password", txtPassword.Password.Trim());

                try
                {
                    // Ejecutar la consulta y obtener el resultado
                    resultado = Convert.ToInt32(sqlCmd.ExecuteScalar());

                    // Manejar el resultado según el valor devuelto
                    if (resultado == -1)
                    {
                        MessageBox.Show("Usuario no encontrado", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else if (resultado == 0)
                    {
                        DashboardAdmin dashboardAdmin = new DashboardAdmin();
                        dashboardAdmin.Show();
                        this.Close();
                    }
                    else if (resultado == 1)
                    {
                        DashboardMesero dashboardMesero = new DashboardMesero();
                        dashboardMesero.Show();
                        this.Close();
                    }
                    else if (resultado == 2)
                    {
                        DashboardCajero dashboardCajero = new DashboardCajero();
                        dashboardCajero.Show();
                        this.Close();
                    }
                    else if (resultado == 3)
                    {
                        DashboardJefeCocina dashboardJefeCocina = new DashboardJefeCocina();
                        dashboardJefeCocina.Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Error inesperado", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex) 
                {
                    MessageBox.Show("Ocurrió un error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    // Asegurarse de cerrar la conexión
                    if (conDb.State == ConnectionState.Open)
                    {
                        conDb.Close();
                    }
                }
            }
            
        }
        #endregion

        #region EVENTOS BOTONES
        private void BtnCerrarLogin_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void BotonIngresar_MouseEnter(object sender, MouseEventArgs e)
        {
            Button boton = sender as Button;

            if (boton != null)
            {
                boton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF008592"));
            }
        }

        private void BotonIngresar_MouseLeave(object sender, MouseEventArgs e)
        {
            Button boton = sender as Button;

            if (boton != null)
            {
                boton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFD63C3C"));
            }
        }

        private void btnIngresar_Click(object sender, RoutedEventArgs e)
        {
            EncontrarUsuario();
        }
        #endregion

    }
}