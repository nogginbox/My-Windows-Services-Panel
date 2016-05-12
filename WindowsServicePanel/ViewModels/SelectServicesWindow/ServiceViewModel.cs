using System;
using WindowsServicePanel.Sevices;

namespace WindowsServicePanel.ViewModels.SelectServicesWindow
{
    public class ServiceViewModel : ServiceBaseViewModel
    {
        public ServiceViewModel(ServiceInfo serviceInfo)
        {
            Name = serviceInfo.DisplayName;
            Running = serviceInfo.IsRunning;
            StartMode = serviceInfo.StartMode;
            State = serviceInfo.State;

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

        public String StartMode
        {
            get
            {
                return _startMode;
            }
            set
            {
                _startMode = value;
                RaisePropertyChanged("StartMode");
            }
        }
        private String _startMode;

        public String State
        {
            get
            {
                return _state;
            }
            set
            {
                _state = value;
                RaisePropertyChanged("State");
            }
        }
        private String _state;

    }
}
