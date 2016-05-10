using WindowsServicePanel.Sevices;

namespace WindowsServicePanel.ViewModels.SelectServicesWindow
{
    public class ServiceViewModel : ServiceBaseViewModel
    {
        public ServiceViewModel(ServiceInfo serviceInfo)
        {
            UpdateService(serviceInfo);
        }

        public bool Selected
        {
            get
            {
                return _selected;
            }
            set
            {
                _selected = value;
                RaisePropertyChanged("Selected");
            }
        }
        private bool _selected;

        public void UpdateService(ServiceInfo serviceInfo)
        {
            Name = serviceInfo.DisplayName;
            Running = serviceInfo.IsRunning;
        }
    }
}
