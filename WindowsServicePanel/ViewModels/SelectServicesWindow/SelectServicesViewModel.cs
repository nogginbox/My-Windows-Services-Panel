using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using WindowsServicePanel.Extensions;
using WindowsServicePanel.Sevices;
using WindowsServicePanel.ViewModels.Events;

namespace WindowsServicePanel.ViewModels.SelectServicesWindow
{
    public class SelectServicesViewModel : ObservableViewModelBase
    {
        private IList<ServiceViewModel> _allServiceViewModels;
        private readonly WindowsServicesService _servicesService;
        private readonly IEnumerable<string> _selectedServices;

        public ObservableCollection<ServiceViewModel> Services { get; }

        public SelectServicesViewModel(WindowsServicesService servicesService, IEnumerable<String> selectedServices)
        {
            _servicesService = servicesService;
            _selectedServices = selectedServices;
            Services = new ObservableCollection<ServiceViewModel>();
        }

        public async Task InitServiceList()
        {
            var allServices = await Task.Run(() => _servicesService.GetAllServices().ToList());
            _allServiceViewModels = CreateServiceViewModels(allServices, OnServiceChanged)
                .OrderByDescending(s => s.Selected)
                .ThenBy(a => a.Name)
                .ToList();

            Services.Add(_allServiceViewModels);
        }

        private IList<ServiceViewModel> CreateServiceViewModels(IEnumerable<ServiceInfo> allServices, PropertyChangedEventHandler onServiceSelectedChanged)
        {
            var allServicesViewModels = allServices
               .Select(s => new ServiceViewModel(s))
               .ToList();

            foreach (var curentlySelectedService in _selectedServices)
            {
                var serviceViewModel = allServicesViewModels.FirstOrDefault(s => s.Name == curentlySelectedService);
                if (serviceViewModel == null) continue;
                serviceViewModel.Selected = true;
            }

            foreach (var service in allServicesViewModels)
            {
                service.PropertyChanged += OnServiceChanged;
            }

            return allServicesViewModels;
        }

        public ICommand CloseWindowCommand => new DelegateCommand(CloseWindow, c => true);

        private static void CloseWindow(object context)
        {
            var window = context as Xaml.SelectServicesWindow.SelectServicesWindow;
            window?.Close();
        }

        public String SearchText
        {
            get
            {
                return _searchText;
            }
            set
            {
                if (_searchText == value) return;
                _searchText = value;
                RaisePropertyChanged("SearchText");
                SearchTextFilterServices(value);
            }
        }
        private String _searchText;

        /// <summary>
        /// Shows or hides services in the list of services based on whether their name matches the search text
        /// </summary>
        private void SearchTextFilterServices(string searchText)
        {
            var isVisible = string.IsNullOrEmpty(searchText)
                ? (Func<string, bool>)((s) => true)
                : (Func<string, bool>)((s) => s.IndexOf(searchText, StringComparison.CurrentCultureIgnoreCase) >= 0);

            var services = _allServiceViewModels.Where(s => isVisible(s.Name));

            Services.Clear();
            Services.Add(services);
        }
        
        public event SelectedServicesChangedEventHandler SelectedServicesChanged;
        public delegate void SelectedServicesChangedEventHandler(object sender, ServicesChangedEventArgs e);
        
        private void OnServiceChanged(object sender, PropertyChangedEventArgs eventArgs)
        {
            switch(eventArgs.PropertyName)
            {
                case "Selected":
                    var serviceViewModel = sender as ServiceViewModel;
                    if (serviceViewModel == null) return;

                    var args = new ServicesChangedEventArgs(serviceViewModel.Name, serviceViewModel.Selected);
                    SelectedServicesChanged?.Invoke(this, args);
                    break;
            }
        }
    }
}