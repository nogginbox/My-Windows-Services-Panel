using System.Windows.Input;
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
        private ServiceInfo s;

        public ICommand ChangeServiceSelectedCommand => new DelegateCommand(ChangeServiceSelected, c => true);

        private void ChangeServiceSelected(object context)
        {
            this.Selected = !_selected;
        }

        public void UpdateService(ServiceInfo serviceInfo)
        {
            Name = serviceInfo.DisplayName;
            Running = serviceInfo.IsRunning;
        }
    }
}
