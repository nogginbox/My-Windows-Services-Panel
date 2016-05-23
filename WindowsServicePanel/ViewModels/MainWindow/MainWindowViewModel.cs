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
                if (_statusMessage == value) return;
                _statusMessage = value;
                RaisePropertyChanged("StatusMessage");
            }
        }
        private String _statusMessage;


        public ObservableCollection<ServiceViewModel> Services { get; }

        public ICommand OpenServicesSelectionWindowCommand => new DelegateCommand(OpenServicesSelectionWindow, c => true);

        private void OpenServicesSelectionWindow(object context)
        {
            var selectServicesViewModel = new SelectServicesViewModel(ServicesService, Services.Select(s => s.Name));
            selectServicesViewModel.SelectedServicesChanged += OnSelectedServicesChanged;
            var window = new Xaml.SelectServicesWindow.SelectServicesWindow(selectServicesViewModel);
            window.ShowDialog();
        }

        public ICommand OpenAboutWindowCommand => new DelegateCommand(OpenAboutWindow, c => true);

        private static void OpenAboutWindow(object context)
        {
            var window = new Xaml.AboutWindow.AboutWindow();
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