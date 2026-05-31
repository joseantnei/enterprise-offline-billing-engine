using EnterpriseBilling.UI.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace EnterpriseBilling.UI.ViewModels
{
    public class ProductsViewModel : ObservableObject
    {
        private readonly AppDbContext _context;
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public ObservableCollection<Product> Products { get; set; }
        public ICommand AddProductCommand {  get; }

        public ProductsViewModel() 
        { 
            _context = new AppDbContext();
            Products = new ObservableCollection<Product>(_context.Products.ToList());

            AddProductCommand = new RelayCommand(_ => AddProduct());
        }

        private void AddProduct()
        {
            //var newProduct = new Product { Name = Name, Email = Email, Phone = Phone};
            var newProduct = new Product { };
            _context.Products.Add(newProduct);
            _context.SaveChanges();

            Products.Add(newProduct);
        }
    }
}
