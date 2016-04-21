using System;
using System.Windows;
using WindowsServicePanel.Sevices;
using WindowsServicePanel.ViewModels;


namespace WindowsServicePanel.Xaml
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel _viewModel = new MainWindowViewModel();

        public MainWindow()
        {
            DataContext = _viewModel;
            InitializeComponent();

            // MSSQLSERVER | SQL Server (MSSQLSERVER)
            // W3SVC | World Wide Web Publishing Service
            try
            {
                _viewModel.Services.Add(new ServiceViewModel(new WindowsServiceMonitor("MSSQLSERVER")));
                _viewModel.Services.Add(new ServiceViewModel(new WindowsServiceMonitor("MSMQ")));
            }
            catch (Exception e)
            {
                _viewModel.StatusMessage = e.Message;
            }
        }

        protected override void OnActivated(EventArgs e)
        {
            foreach (var service in _viewModel.Services)
            {
                service.UpdateService();
            }
            base.OnActivated(e);
        }
    }
}