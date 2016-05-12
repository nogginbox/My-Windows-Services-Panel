using System;
using System.Windows;
using System.Windows.Controls;
using WindowsServicePanel.ViewModels.SelectServicesWindow;

namespace WindowsServicePanel.Xaml.SelectServicesWindow
{
    /// <summary>
    /// Interaction logic for ServicePane.xaml
    /// </summary>
    public partial class ServicePane : UserControl
    {
        public ServicePane()
        {
            InitializeComponent();
        }

        static ServicePane()
        {
            ServiceInfoProperty =
                DependencyProperty.Register("ServiceInfo",
                    typeof(ServiceViewModel),
                    typeof(ServicePane),
                    new PropertyMetadata(null));
        }

        public ServiceViewModel ServiceInfo
        {
            get { return (ServiceViewModel)GetValue(ServiceInfoProperty); }
            set { SetValue(ServiceInfoProperty, value); }
        }
        public static DependencyProperty ServiceInfoProperty { get; }
    }
}
