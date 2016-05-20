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
                if (_selected == value) return;
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
                if (_startMode == value) return;
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
                if (_state == value) return;
                _state = value;
                RaisePropertyChanged("State");
            }
        }
        private String _state;

    }
}
