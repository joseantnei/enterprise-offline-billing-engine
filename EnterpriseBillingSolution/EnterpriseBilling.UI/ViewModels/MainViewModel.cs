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
        
        public ICommand NavigateDashboardCommand { get; }
        public ICommand NavigateCustomersCommand { get; }

        public MainViewModel() 
        {
            var dashboardVm = new DashboardViewModel();
            var costumersVm = new CustomersViewModel();

            NavigateDashboardCommand = new RelayCommand(_  => CurrentView = dashboardVm);
            NavigateCustomersCommand = new RelayCommand(_ => CurrentView = costumersVm);

            CurrentView = dashboardVm;
        }
    }
}
