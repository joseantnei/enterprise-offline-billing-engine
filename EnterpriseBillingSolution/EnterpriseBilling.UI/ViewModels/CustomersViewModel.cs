using EnterpriseBilling.UI.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace EnterpriseBilling.UI.ViewModels
{
    public class CustomersViewModel : ObservableObject
    {
        private readonly AppDbContext _context;
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public ObservableCollection<Customer> Customers { get; set; }
        public ICommand AddCustomerCommand {  get; }

        public CustomersViewModel() 
        { 
            _context = new AppDbContext();
            Customers = new ObservableCollection<Customer>(_context.Customers.ToList());

            AddCustomerCommand = new RelayCommand(_ => AddCustomer());
        }

        private void AddCustomer()
        {
            var newCustomer = new Customer { Name = Name, Email = Email, Phone = Phone};
            _context.Customers.Add(newCustomer);
            _context.SaveChanges();

            Customers.Add(newCustomer);
        }
    }
}
