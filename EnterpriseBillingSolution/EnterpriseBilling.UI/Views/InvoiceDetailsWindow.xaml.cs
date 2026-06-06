using System;
using System.Linq;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using EnterpriseBilling.UI.Models;

namespace EnterpriseBilling.UI.Views
{
    public partial class InvoiceDetailsWindow : Window
    {
        private readonly int _invoiceId;

        // El constructor recibe el ID de la factura que queremos inspeccionar
        public InvoiceDetailsWindow(int invoiceId)
        {
            InitializeComponent();
            this._invoiceId = invoiceId;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadInvoiceDetails();
        }

        private void LoadInvoiceDetails()
        {
            try
            {
                using (var db = new AppDbContext())
                {
                    // EXPLICACIÓN DEL COMODÍN .Include() y .ThenInclude():
                    // Buscamos la factura por su ID. 
                    // .Include(b => b.Customer) une la tabla de Clientes para sacar el Nombre.
                    // .Include(b => b.BillDetails) trae todas las líneas vendidas.
                    // .ThenInclude(d => d.Product) "viaja" desde el detalle hacia la tabla Productos 
                    // para obtener el nombre comercial del artículo vendido.
                    var invoice = db.Bills
                        .Include(b => b.Customer)
                        .Include(b => b.BillDetails)
                            .ThenInclude(d => d.Product)
                        .FirstOrDefault(b => b.IdBill == _invoiceId);

                    if (invoice != null)
                    {
                        // Asignamos los textos a la interfaz
                        lblInvoiceNumber.Text = invoice.BillNumber;
                        lblDate.Text = invoice.Date.ToString("yyyy-MM-dd HH:mm:ss");
                        lblCustomer.Text = invoice.Customer?.Name ?? "N/A";
                        lblId.Text = invoice.Customer?.Id.ToString() ?? "N/A";

                        lblSubtotal.Text = invoice.Subtotal.ToString("C2");
                        lblTaxes.Text = invoice.TotalTaxes.ToString("C2");
                        lblTotal.Text = invoice.TotalPay.ToString("C2");

                        // Para mostrar el Subtotal por línea en el DataGrid (Cantidad * Precio), 
                        // transformamos la lista usando LINQ para añadir el campo calculado 'Subtotal' sobre la marcha.
                        var itemsSource = invoice.BillDetails.Select(d => new {
                            d.Product,
                            d.Quantity,
                            d.UnitPrice,
                            d.PercentTaxes,
                            Subtotal = d.Quantity * d.UnitPrice
                        }).ToList();

                        dgInvoiceDetails.ItemsSource = itemsSource;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading details: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}