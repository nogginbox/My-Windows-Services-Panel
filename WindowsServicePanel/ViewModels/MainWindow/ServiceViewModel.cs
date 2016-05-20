using System;
using System.Windows.Input;
using WindowsServicePanel.Sevices;

namespace WindowsServicePanel.ViewModels.MainWindow
{
    public class ServiceViewModel : ServiceBaseViewModel
    {
        private readonly WindowsServiceMonitor _serviceMonitor;

        public ServiceViewModel(WindowsServiceMonitor serviceMonitor)
        {
            _serviceMonitor = serviceMonitor;
            UpdateService();
        }

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
                throw new Exception("Could not do that", ex);
                // UserMessages.Text = ex.InnerException.Message;
            }

            Running = _serviceMonitor.IsRunning;
        }

        public void UpdateService()
        {
            Name = _serviceMonitor.DisplayName;
            Running = _serviceMonitor.IsRunning;
        }
    }
}
