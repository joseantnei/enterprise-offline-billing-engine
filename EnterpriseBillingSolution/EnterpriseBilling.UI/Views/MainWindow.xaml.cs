using EnterpriseBilling.UI.ViewModels;
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

namespace EnterpriseBilling.UI.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainViewModel();
        }

        // AGREGADO: Se ejecuta justo cuando la ventana aparece en pantalla
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ProbarConexionBaseDeDatos();
        }

        // AGREGADO: Lógica de prueba
        private void ProbarConexionBaseDeDatos()
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    int userCount = context.Users.Count();

                    if (userCount > 0)
                    {
                        txtDbStatus.Text = $"Conectado a SQLite. Usuarios en BD: {userCount}";
                        txtDbStatus.Foreground = System.Windows.Media.Brushes.DarkGreen;
                    }
                    else
                    {
                        txtDbStatus.Text = "Conectado, pero la base de datos está vacía.";
                        txtDbStatus.Foreground = System.Windows.Media.Brushes.DarkOrange;
                    }
                }
            }
            catch (Exception ex)
            {
                txtDbStatus.Text = "Error de conexión";
                txtDbStatus.Foreground = System.Windows.Media.Brushes.Red;

                MessageBox.Show($"Ocurrió un error al conectar con SQLite:\n\n{ex.Message}",
                                "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}