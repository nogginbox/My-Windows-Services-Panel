using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WindowsServicePanel.Sevices;


namespace WindowsServicePanel
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly WindowsServiceMonitor _windowsServiceMonitor;

        public MainWindow()
        {
            // MSSQLSERVER | SQL Server (MSSQLSERVER)
            // W3SVC | World Wide Web Publishing Service
            _windowsServiceMonitor = new WindowsServiceMonitor("W3SVC");
            InitializeComponent();
        }

        protected override void OnActivated(EventArgs e)
        {
            SetButtonState(IisButton, _windowsServiceMonitor.IsRunning);
            base.OnActivated(e);
        }

        private static void SetButtonState(Button button, bool isRunning)
        {
            if (isRunning)
            {
                button.Content = "Enabled";
                button.Background = Brushes.LightGreen;
            }
            else
            {
                button.Content = "Stopped";
                button.Background = Brushes.LightGray;
            }
        }

        private void OnClickChangeIisServiceStateButton(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_windowsServiceMonitor.IsRunning)
                {
                    _windowsServiceMonitor.Stop();
                }
                else
                {
                    _windowsServiceMonitor.Start();
                }
            }
            catch (Exception ex)
            {
                UserMessages.Text = ex.InnerException.Message;
            }
            SetButtonState(IisButton, _windowsServiceMonitor.IsRunning);
        }
    }
}