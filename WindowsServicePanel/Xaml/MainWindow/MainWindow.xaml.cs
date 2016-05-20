using System;
using System.Linq;
using System.Windows;
using WindowsServicePanel.Sevices;
using WindowsServicePanel.ViewModels.MainWindow;

namespace WindowsServicePanel.Xaml.MainWindow
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

            try
            {
                foreach (var service in Properties.Settings.Default.ServicesToShow)
                {
                    _viewModel.Services.Add(new ServiceViewModel(new WindowsServiceMonitor(service)));
                }
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

        protected override void OnClosed(EventArgs e)
        {
            try
            {
                var servicesToSave = _viewModel.Services.Select(s => s.Name);
                Properties.Settings.Default.ServicesToShow.Clear();
                Properties.Settings.Default.ServicesToShow.AddRange(servicesToSave.ToArray());
                Properties.Settings.Default.Save();
            }
            catch (Exception)
            {
                // Don't get too upset if we can't save on exit.   
            }

            base.OnClosed(e);
        }
    }
}