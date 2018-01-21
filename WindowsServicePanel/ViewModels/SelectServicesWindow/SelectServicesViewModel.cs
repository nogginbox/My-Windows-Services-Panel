using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using WindowsServicePanel.Extensions;
using WindowsServicePanel.Sevices;
using WindowsServicePanel.ViewModels.Events;

namespace WindowsServicePanel.ViewModels.SelectServicesWindow
{
    public class SelectServicesViewModel : ObservableViewModelBase
    {
        private const int ItemsToShowFirst = 30;
        private IEnumerable<ServiceViewModel> _allServiceViewModels;
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
            var allServices = await Task.Run(() => _servicesService.GetAllServices().ToList());
            _allServiceViewModels = CreateServiceViewModels(allServices, OnServiceChanged);

            // Show screen worth of services first to make this seem faster (not sure if delayedServiceCollection makes any/much difference)
            using (var delayedServiceCollection = Services.DelayNotifications())
            {
                delayedServiceCollection.Add(_allServiceViewModels.Take(ItemsToShowFirst));
            }

            // Show the rest after small timed pause to give UI chance to draw the first screen's worth
            var dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(InitRestOfServivesAfterPause);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 50);
            dispatcherTimer.Start();
        }

        private void InitRestOfServivesAfterPause(object sender, EventArgs e)
        {
            (sender as DispatcherTimer).Stop();
            Services.Add(_allServiceViewModels.Skip(ItemsToShowFirst));
        }

        private IEnumerable<ServiceViewModel> CreateServiceViewModels(IEnumerable<ServiceInfo> allServices, PropertyChangedEventHandler onServiceSelectedChanged)
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

            return allServicesViewModels.OrderByDescending(s => s.Selected).ThenBy(a => a.Name);
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

            foreach (var service in Services)
            {
                service.Show = isVisible(service.Name);
            }
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