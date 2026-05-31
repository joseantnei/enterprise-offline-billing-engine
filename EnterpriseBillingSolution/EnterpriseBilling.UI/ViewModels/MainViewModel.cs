using EnterpriseBilling.UI.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace EnterpriseBilling.UI.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        private object _currentView;

        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; OnPropertyChanged(); }
        }
        
        //Declara propiedad
        public ICommand NavigateDashboardCommand { get; }
        public ICommand NavigateCustomersCommand { get; }
        public ICommand NavigateProductsCommand { get; }
        public ICommand NavigateInvoicesCommand { get; }

        //Constructor
        public MainViewModel() 
        {
            //Crea instancia
            var dashboardVm = new DashboardViewModel();
            var costumersVm = new CustomersViewModel();
            var productsVm = new ProductsViewModel();
            var invoicesVm = new InvoicesViewModel();

            //Emlazarlo
            NavigateDashboardCommand = new RelayCommand(_  => CurrentView = dashboardVm);
            NavigateCustomersCommand = new RelayCommand(_ => CurrentView = costumersVm);
            NavigateProductsCommand = new RelayCommand(_ => CurrentView = productsVm);
            NavigateInvoicesCommand = new RelayCommand(_ => CurrentView = invoicesVm);

            CurrentView = dashboardVm;
        }
    }
}
