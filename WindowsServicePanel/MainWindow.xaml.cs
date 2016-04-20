using System;
using System.Windows;
using System.Windows.Controls.Primitives;
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

        private void SetButtonState(ToggleButton button, bool isRunning)
        {
            button.IsChecked = isRunning;
            IisStatus.Text = isRunning ? "Started" : "Stopped";
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