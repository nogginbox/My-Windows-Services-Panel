using System;
using System.Collections.ObjectModel;


namespace WindowsServicePanel.ViewModels
{
    public class MainWindowViewModel : ObservableViewModelBase
    {
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
    }
}