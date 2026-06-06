using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using EnterpriseBilling.UI.Models;
using EnterpriseBilling.UI.Views;

namespace EnterpriseBilling.UI.ViewModels
{
    public class InvoiceHistoryViewModel : INotifyPropertyChanged
    {
        private string _searchQuery = string.Empty;
        private Bill _selectedInvoice;
        private ObservableCollection<Bill> _allInvoices;
        private ObservableCollection<Bill> _filteredInvoices;

        public ICommand OpenDetailsCommand { get; set; }

        public InvoiceHistoryViewModel()
        {
            _allInvoices = new ObservableCollection<Bill>();
            _filteredInvoices = new ObservableCollection<Bill>();

            OpenDetailsCommand = new RelayCommand(ExecuteOpenDetails);

            LoadInvoicesFromDb();
        }

        // Propiedades enlazadas al XAML
        public string SearchQuery
        {
            get => _searchQuery;
            set
            {
                _searchQuery = value;
                OnPropertyChanged();
                ApplyFilter(); // Filtra automáticamente en memoria mientras el usuario escribe
            }
        }

        public Bill SelectedInvoice
        {
            get => _selectedInvoice;
            set { _selectedInvoice = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Bill> FilteredInvoices
        {
            get => _filteredInvoices;
            set { _filteredInvoices = value; OnPropertyChanged(); }
        }

        // Método que extrae el historial inicial desde SQLite
        private void LoadInvoicesFromDb()
        {
            try
            {
                using (var db = new AppDbContext())
                {
                    var data = db.Bills
                        .Include(b => b.Customer) // Traemos los clientes vinculados
                        .OrderByDescending(b => b.Date) // Mostrar las más recientes arriba
                        .ToList();

                    _allInvoices = new ObservableCollection<Bill>(data);
                    FilteredInvoices = new ObservableCollection<Bill>(data);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading database history: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Lógica del filtro reactivo en memoria (Súper veloz)
        private void ApplyFilter()
        {
            if (string.IsNullOrWhiteSpace(SearchQuery))
            {
                FilteredInvoices = new ObservableCollection<Bill>(_allInvoices);
            }
            else
            {
                string searchLower = SearchQuery.ToLower();
                var results = _allInvoices.Where(b =>
                    b.BillNumber.ToLower().Contains(searchLower) ||
                    (b.Customer != null && b.Customer.Name.ToLower().Contains(searchLower))
                ).ToList();

                FilteredInvoices = new ObservableCollection<Bill>(results);
            }
        }

        // Método que levanta el modal detallado
        private void ExecuteOpenDetails(object? parameter)
        {
            if (SelectedInvoice == null) return;

            // Instanciamos el pop-up pasándole el ID de la factura seleccionada
            var detailsWindow = new InvoiceDetailsWindow(SelectedInvoice.IdBill);

            // Lo enlazamos a la ventana principal para mantener orden jerárquico visual
            detailsWindow.Owner = Application.Current.MainWindow;

            detailsWindow.ShowDialog();
        }

        // Implementación Estándar de MVVM para refrescar UI
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }
}