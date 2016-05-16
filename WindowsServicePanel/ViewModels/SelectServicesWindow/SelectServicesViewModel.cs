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

        public ICollection<ServiceViewModel> Services { get; }

        public SelectServicesViewModel(WindowsServicesService servicesService)
        {
            _servicesService = servicesService;
            Services = new ObservableCollection<ServiceViewModel>();
        }

        private void AddService(ServiceViewModel service)
        {
            Services.Add(service);
            service.PropertyChanged += OnServiceSelectedChanged;
        }

        public async Task InitServiceList()
        {
            var allServices = _servicesService.GetAllServices();
            var allServicesViewModels = allServices
                .Select(s => new ServiceViewModel(s))
                .ToList();

            foreach (var curentlySelectedService in allServicesViewModels)
            {
                var serviceViewModel = allServicesViewModels.FirstOrDefault(s => s.Name == curentlySelectedService.Name);
                if (serviceViewModel == null) break;
                serviceViewModel.Selected = true;
            }

            foreach (var serviceViewMode in allServicesViewModels.OrderByDescending(s => s.Selected).ThenBy(a => a.Name))
            {
                AddService(serviceViewMode);
            }
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