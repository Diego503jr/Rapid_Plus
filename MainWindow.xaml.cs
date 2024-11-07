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

            if (string.IsNullOrWhiteSpace(txtCorreo.Text))
            {
                MessageBox.Show("Por favor ingrese su nombre de usuario.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (conDb.State == ConnectionState.Closed)
            {
                conDb.Open();

                consultaSQL = "EXEC SPENCONTRARUSUARIO @User, @Password, @ID_USUARIO OUTPUT, @ID_ROL OUTPUT";
                SqlCommand sqlCmd = new SqlCommand(consultaSQL, conDb);
                sqlCmd.CommandType = CommandType.Text;

                sqlCmd.Parameters.AddWithValue("@User", txtCorreo.Text.Trim());
                sqlCmd.Parameters.AddWithValue("@Password", txtPassword.Password.Trim());

                SqlParameter paramIDUsuario = new SqlParameter("@ID_USUARIO", SqlDbType.Int) { Direction = ParameterDirection.Output };
                SqlParameter paramIDRol = new SqlParameter("@ID_ROL", SqlDbType.Int) { Direction = ParameterDirection.Output };
                sqlCmd.Parameters.Add(paramIDUsuario);
                sqlCmd.Parameters.Add(paramIDRol);

                try
                {
                    sqlCmd.ExecuteNonQuery();

                    // Obtiene los valores de salida
                    int usuarioID = (int)paramIDUsuario.Value;
                    int tipoUsuario = (int)paramIDRol.Value;

                    // Verifica si el usuario fue encontrado (si userID y tipoUsuario no son -1)
                    if (usuarioID == -1)
                    {
                        MessageBox.Show("Usuario no encontrado", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        // Según el tipo de usuario, abre el dashboard correspondiente y pasa el ID de usuario
                        switch (tipoUsuario)
                        {
                            case 0:
                                DashboardAdmin dashboardAdmin = new DashboardAdmin();
                                dashboardAdmin.Show();
                                break;
                            case 1:
                                DashboardMesero dashboardMesero = new DashboardMesero(usuarioID);
                                dashboardMesero.Show();
                                break;
                            case 2:
                                DashboardCajero dashboardCajero = new DashboardCajero(usuarioID);
                                dashboardCajero.Show();
                                break;
                            case 3:
                                DashboardJefeCocina dashboardJefeCocina = new DashboardJefeCocina();
                                dashboardJefeCocina.Show();
                                break;
                            default:
                                MessageBox.Show("Error inesperado", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                break;
                        }

                        this.Close(); // Cierra la ventana actual de login
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
                boton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A2BCE0"));
            }
        }

        private void btnIngresar_Click(object sender, RoutedEventArgs e)
        {
            EncontrarUsuario();
        }
        #endregion

    }
}