using System;
using System.Windows.Input;
using WindowsServicePanel.Sevices;

namespace WindowsServicePanel.ViewModels
{
    public class ServiceViewModel : ObservableViewModelBase
    {
        private readonly WindowsServiceMonitor _serviceMonitor;

        public ServiceViewModel(WindowsServiceMonitor serviceMonitor)
        {
            _serviceMonitor = serviceMonitor;
            UpdateService();
        }

        public String Name
        {
            get
            {
                return _name;
            }
            private set
            {
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
                _running = value;
                RaisePropertyChanged("Running");
            }
        }
        private bool _running;


        public ICommand ChangeServiceStatusCommand => new DelegateCommand(ChangeServiceStatus, c => true);

        private void ChangeServiceStatus(object context)
        {
            try
            {
                if (_serviceMonitor.IsRunning)
                {
                    _serviceMonitor.Stop();
                }
                else
                {
                    _serviceMonitor.Start();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Could not do that");
                // UserMessages.Text = ex.InnerException.Message;
            }
            //SetButtonState(IisButton, _windowsServiceMonitor.IsRunning); */
        }


        public void UpdateService()
        {
            Name = _serviceMonitor.DisplayName;
            Running = _serviceMonitor.IsRunning;
        }
    }
}