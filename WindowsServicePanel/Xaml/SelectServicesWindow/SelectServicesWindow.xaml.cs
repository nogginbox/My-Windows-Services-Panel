using System;
using System.Windows;
using WindowsServicePanel.ViewModels.SelectServicesWindow;

namespace WindowsServicePanel.Xaml.SelectServicesWindow
{
    /// <summary>
    /// Interaction logic for SelectServicesWindow.xaml
    /// </summary>
    public partial class SelectServicesWindow : Window
    {
        private bool _loaded;

        public SelectServicesWindow(SelectServicesViewModel viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();
        }

        protected override async void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);
            if (_loaded) return;

            _loaded = true;
            await (DataContext as SelectServicesViewModel)?.InitServiceList();
            ShowLoadingBar.Visibility = Visibility.Collapsed;
        }
    }
}