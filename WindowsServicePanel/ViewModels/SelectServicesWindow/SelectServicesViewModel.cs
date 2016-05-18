using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using WindowsServicePanel.Sevices;
using WindowsServicePanel.ViewModels.Events;

namespace WindowsServicePanel.ViewModels.SelectServicesWindow
{
    public class SelectServicesViewModel : ObservableViewModelBase
    {
        private readonly WindowsServicesService _servicesService;
        private readonly IEnumerable<string> _selectedServices;

        public ObservableCollectionEx<ServiceViewModel> Services { get; }

        public SelectServicesViewModel(WindowsServicesService servicesService, IEnumerable<String> selectedServices)
        {
            _servicesService = servicesService;
            _selectedServices = selectedServices;
            Services = new ObservableCollectionEx<ServiceViewModel>();
        }

        public async Task InitServiceList()
        {
            var allServices = await Task.Run(() => _servicesService.GetAllServices());
            var allServiceViewModels = await Task.Run(() => CreateServiceViewModels(allServices));

            using (var delayedServiceCollection = Services.DelayNotifications())
            {
                foreach (var serviceViewMode in allServiceViewModels)
                {
                    delayedServiceCollection.Add(serviceViewMode);
                    serviceViewMode.PropertyChanged += OnServiceSelectedChanged;
                }
            }
        }

        private IEnumerable<ServiceViewModel> CreateServiceViewModels(IEnumerable<ServiceInfo> allServices)
        {
            var allServicesViewModels = allServices
               .Select(s => new ServiceViewModel(s))
               .ToList();

            foreach (var curentlySelectedService in _selectedServices)
            {
                var serviceViewModel = allServicesViewModels.FirstOrDefault(s => s.Name == curentlySelectedService);
                if (serviceViewModel == null) break;
                serviceViewModel.Selected = true;
            }

            return allServicesViewModels.OrderByDescending(s => s.Selected).ThenBy(a => a.Name);
        }

        public ICommand CloseWindowCommand => new DelegateCommand(CloseWindow, c => true);

        private static void CloseWindow(object context)
        {
            var window = context as Xaml.SelectServicesWindow.SelectServicesWindow;
            window?.Close();
        }

        public event SelectedServicesChangedEventHandler SelectedServicesChanged;
        public delegate void SelectedServicesChangedEventHandler(object sender, ServicesChangedEventArgs e);
        
        private void OnServiceSelectedChanged(object sender, PropertyChangedEventArgs eventArgs)
        {
            var serviceViewModel = sender as ServiceViewModel;
            if (serviceViewModel == null) return;

            var args = new ServicesChangedEventArgs(serviceViewModel.Name, serviceViewModel.Selected);
            SelectedServicesChanged?.Invoke(this, args);
        }
    }
}