using EnterpriseBilling.UI.Models;
using EnterpriseBilling.UI.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace EnterpriseBilling.UI.ViewModels
{
    public class InvoicesViewModel : INotifyPropertyChanged
    {
        private string _customerName = "Final Consumer";
        private int _id = 1;
        private string _barcodeSearch;
        private decimal _subtotal;
        private decimal _totalTaxes;
        private decimal _totalPay;

        public ObservableCollection<InvoiceItemRow> InvoiceItems { get; set; }

        public ICommand SearchProductCommand { get; set; }
        public ICommand SaveInvoiceCommand { get; set; }
        public ICommand OpenCustomerSearchCommand { get; set; }

        public InvoicesViewModel()
        {
            InvoiceItems = new ObservableCollection<InvoiceItemRow>();

            InvoiceItems.CollectionChanged += (s, e) =>
            {
                if (e.NewItems != null)
                    foreach (InvoiceItemRow item in e.NewItems) item.PropertyChanged += Item_PropertyChanged;
            };

            // Dos maneras de usar el relaycommand agregando object? parameter o una funcion lambda
            SearchProductCommand = new RelayCommand(ExecuteSearchProduct);
            SaveInvoiceCommand = new RelayCommand(p => ExecuteSaveInvoice());
            OpenCustomerSearchCommand = new RelayCommand(ExecuteOpenCustomerSearch);
        }

        public string CustomerName { get { return _customerName; } set { _customerName = value; OnPropertyChanged(); } }
        public string BarcodeSearch { get { return _barcodeSearch; } set { _barcodeSearch = value; OnPropertyChanged(); } }
        public decimal SubTotal { get { return _subtotal; } set { _subtotal = value; OnPropertyChanged(); } }
        public decimal TotalTaxes { get { return _totalTaxes; } set { _totalTaxes = value; OnPropertyChanged(); } }
        public decimal TotalPay { get { return _totalPay; } set { _totalPay = value; OnPropertyChanged(); } }

        private void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(InvoiceItemRow.Quantity))
            {
                CalculateTotals();
            }
        }
        //  BUSQUEDA DE PRODUCTOS SQLITE
        private void ExecuteSearchProduct(object? parameter)
        {
            if (string.IsNullOrWhiteSpace(BarcodeSearch)) return;

            using (var db = new AppDbContext())
            {
                //Buscando Taxes
                var defaultTax = db.TaxesTypes.FirstOrDefault(t => t.IdTaxesType == 3);

                //Buscando Producto
                var product = db.Products
                    .FirstOrDefault(p => p.Barcode == BarcodeSearch);

                if (product != null)
                {
                    var existingItem = InvoiceItems.FirstOrDefault(i => i.ProductId == product.IdProduct);
                    if (existingItem != null) {
                        existingItem.Quantity += 1;
                    }
                    else
                    {
                        InvoiceItems.Add(new InvoiceItemRow
                        {
                            ProductId = product.IdProduct,
                            Barcode = product.Barcode,
                            ProductName = product.NameProduct,
                            UnitPrice = product.SalePrice,
                            Quantity = 1,
                            TaxPercent = defaultTax?.Percent ?? 0m
                        });
                    }
                    BarcodeSearch = string.Empty;
                    CalculateTotals();
                }
                else
                {
                    MessageBox.Show("Producto no encontrado", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void ExecuteOpenCustomerSearch(object? parameter)
        {
            // 1. Instanciamos la ventana que creamos en el Paso 1
            var searchWindow = new CustomerSearchWindow();

            // 2. Le decimos a Windows quién es su "madre" visual para que no se pierda en el fondo
            searchWindow.Owner = Application.Current.MainWindow;

            // 3. .ShowDialog() abre la ventana de forma "Bloqueante". 
            // El código de abajo se detiene y espera a que el cajero termine de elegir.
            if (searchWindow.ShowDialog() == true)
            {
                // 4. Si el cajero seleccionó un cliente con éxito, extraemos el objeto completo
                var customer = searchWindow.SelectedCustomer;

                if (customer != null)
                {
                    // 5. Actualizamos las variables del ViewModel. 
                    // El cambio se reflejará solo en el TextBox del XAML gracias al OnPropertyChanged()
                    this._id = customer.Id;
                    this.CustomerName = customer.Name;
                }
            }
        }

        private void CalculateTotals()
        {
            SubTotal = InvoiceItems.Sum(item => item.Quantity * item.UnitPrice);
            TotalTaxes = InvoiceItems.Sum(item => (item.Quantity * item.UnitPrice) * (item.TaxPercent / 100m));

            TotalPay = SubTotal + TotalTaxes;
        }
        private void ResetInvoice()
        {
            InvoiceItems.Clear();
            SubTotal = 0;
            TotalTaxes = 0;
            TotalPay = 0;

            // Notificamos todo de un solo golpe
            OnPropertyChanged(nameof(SubTotal));
            OnPropertyChanged(nameof(TotalTaxes));
            OnPropertyChanged(nameof(TotalPay));
            OnPropertyChanged(nameof(InvoiceItems));
        }

        //Guarda Factura
        private void ExecuteSaveInvoice()
        {
            if (!InvoiceItems.Any())
            {
                MessageBox.Show("Carrito Empty", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            try
            {
                using (var db = new AppDbContext())
                {
                    var clienteExistente = db.Customers.Find(this._id);
                    var usuarioExistente = db.Users.Find(1);
                    if (clienteExistente == null || usuarioExistente == null)
                    {
                        MessageBox.Show("El cliente o el usuario no existen en la base de datos.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    var newBill = new Bill
                    {
                        BillNumber = "FAC-" + DateTime.Now.Ticks.ToString().Substring(10),
                        Date = DateTime.Now,
                        Subtotal = this.SubTotal,
                        TotalTaxes = this.TotalTaxes,
                        TotalPay = this.TotalPay,
                        //Id = this._id,
                        //IdUser = 1,
                        Customer = clienteExistente,
                        User = usuarioExistente
                    };

                    db.Bills.Add(newBill);
                    db.SaveChanges();

                    // Guardar Detalles y Descontar Stock
                    foreach (var item in InvoiceItems)
                    {
                        var detail = new BillDetail
                        {
                            IdBill = newBill.IdBill,
                            IdProduct = item.ProductId,
                            Quantity = item.Quantity,
                            UnitPrice = item.UnitPrice,
                            PercentTaxes = item.TaxPercent
                        };
                        db.BillDetails.Add(detail);

                        // Descontar inventario localmente
                        var dbProduct = db.Products.Find(item.ProductId);
                        if (dbProduct != null)
                        {
                            dbProduct.Stock -= item.Quantity;
                        }
                    }

                    db.SaveChanges();
                    MessageBox.Show($"Factura {newBill.BillNumber} emitida con éxito en SQLite y Stock actualizado.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                    ResetInvoice();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar la factura: {ex.Message}", "Fatal", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        //Refresh
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }

    public class InvoiceItemRow : INotifyPropertyChanged
    {
        private int _quantity;
        public int ProductId { get; set; }
        public string Barcode { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TaxPercent { get; set; }

        public int Quantity
        {
            get => _quantity;
            set { _quantity = value; OnPropertyChanged(); OnPropertyChanged(nameof(Subtotal)); }
        }

        public decimal Subtotal => Quantity * UnitPrice;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }

    
}
