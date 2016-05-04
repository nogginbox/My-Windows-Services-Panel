using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using WindowsServicePanel.Sevices;
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


        public ObservableCollection<ServiceViewModel> Services { get; private set; }

        public ICommand OpenServicesSelectionWindowCommand => new DelegateCommand(OpenServicesSelectionWindow, c => true);

        private void OpenServicesSelectionWindow(object context)
        {
            var allServices = ServicesService.GetAllServices();

            var window = new Xaml.SelectServicesWindow.SelectServicesWindow
            {
                DataContext = new SelectServicesViewModel
                {
                    Services = allServices.Select(s => new SelectServicesWindow.ServiceViewModel(s)).ToList()
                }
            };
            window.ShowDialog();
        }
    }
}