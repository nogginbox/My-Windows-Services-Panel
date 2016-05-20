using System;


namespace WindowsServicePanel.ViewModels
{
    public abstract class ServiceBaseViewModel : ObservableViewModelBase
    {
        public String Name
        {
            get
            {
                return _name;
            }
            protected set
            {
                if (_name == value) return;
                _name = value;
                RaisePropertyChanged("Name");
            }
        }
        private String _name;

        public bool Running 
        {
            get
            {
                return _running;
            }
            set
            {
                if (_running == value) return;
                _running = value;
                RaisePropertyChanged("Running");
            }
        }
        private bool _running;
    }
}