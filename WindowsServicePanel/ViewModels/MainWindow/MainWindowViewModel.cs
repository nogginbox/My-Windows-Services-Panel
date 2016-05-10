using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using WindowsServicePanel.Sevices;
using WindowsServicePanel.ViewModels.Events;
using WindowsServicePanel.ViewModels.SelectServicesWindow;


namespace WindowsServicePanel.ViewModels.MainWindow
{
    public class MainWindowViewModel : ObservableViewModelBase
    {
        private static readonly WindowsServicesService ServicesService;

        static MainWindowViewModel()
        {
            ServicesService = new WindowsServicesService();
        }

        public MainWindowViewModel()
        {
            Services = new ObservableCollection<ServiceViewModel>();
        }

        public String StatusMessage
        {
            get
            {
                return _statusMessage;
            }
            set
            {
                _statusMessage = value;
                RaisePropertyChanged("StatusMessage");
            }
        }
        private String _statusMessage;


        public ObservableCollection<ServiceViewModel> Services { get; }

        public ICommand OpenServicesSelectionWindowCommand => new DelegateCommand(OpenServicesSelectionWindow, c => true);

        private void OpenServicesSelectionWindow(object context)
        {
            var allServices = ServicesService.GetAllServices();
            var allServicesViewModels = allServices
                .Select(s => new SelectServicesWindow.ServiceViewModel(s))
                .ToList();

            foreach (var curentlySelectedService in Services)
            {
                var serviceViewModel = allServicesViewModels.FirstOrDefault(s => s.Name == curentlySelectedService.Name);
                if (serviceViewModel == null) break;
                serviceViewModel.Selected = true;
            }

            var selectServicesViewModel = new SelectServicesViewModel(allServicesViewModels);
            selectServicesViewModel.SelectedServicesChanged += OnSelectedServicesChanged;

            var window = new Xaml.SelectServicesWindow.SelectServicesWindow
            {
                DataContext = selectServicesViewModel
            };
            window.ShowDialog();
        }

        private void OnSelectedServicesChanged(object sender, ServicesChangedEventArgs servicesChangedEventArgs)
        {
            foreach (var unselectedServiceName in servicesChangedEventArgs.ServicesUnSelected)
            {
                var service = Services.FirstOrDefault(s => s.Name == unselectedServiceName);
                if (service == null) break;

                Services.Remove(service);
            }

            foreach (var selectedServiceName in servicesChangedEventArgs.ServicesSelected)
            {
                var service = new ServiceViewModel(new WindowsServiceMonitor(selectedServiceName));
                Services.Add(service);
            }
        }
    }
}