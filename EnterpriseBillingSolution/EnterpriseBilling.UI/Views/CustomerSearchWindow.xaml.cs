using EnterpriseBilling.UI.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EnterpriseBilling.UI.Views
{
    /// <summary>
    /// Interaction logic for CustomerSearchWindow.xaml
    /// </summary>
    public partial class CustomerSearchWindow : Window
    {
        // Propiedad pública que guardará el cliente que el cajero elija
        public Customer SelectedCustomer { get; private set; }

        public CustomerSearchWindow()
        {
            InitializeComponent();
            CargarClientes(""); // Carga todos al abrir la ventana
        }

        // Método encargado de leer de SQLite y filtrar
        private void CargarClientes(string filtro)
        {
            using (var db = new AppDbContext())
            {
                // Si no hay filtro, trae los primeros 20 clientes para no saturar.
                // Si hay filtro, busca coincidencias en Name 
                var query = db.Customers.AsQueryable();

                if (!string.IsNullOrWhiteSpace(filtro))
                {
                    filtro = filtro.ToLower();
                    query = query.Where(c => c.Name.ToLower().Contains(filtro) ||
                                             c.Id.ToString().Contains(filtro));
                }

                dgCustomers.ItemsSource = query.Take(20).ToList();
            }
        }

        // Evento que se ejecuta cada vez que el cajero escribe una letra
        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            CargarClientes(txtSearch.Text);
        }

        // Método centralizado para confirmar la selección y cerrar
        private void ConfirmarSeleccion()
        {
            if (dgCustomers.SelectedItem is Customer cliente)
            {
                SelectedCustomer = cliente;
                this.DialogResult = true; // Indica que se cerró con éxito aceptando un dato
                this.Close();
            }
            else
            {
                MessageBox.Show("Por favor, seleccione un cliente de la lista.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtnSelect_Click(object sender, RoutedEventArgs e)
        {
            ConfirmarSeleccion();
        }

        // Doble clic sobre la fila de la tabla para seleccionar al instante sin presionar el botón abajo
        private void DgCustomers_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ConfirmarSeleccion();
        }
    }
}
