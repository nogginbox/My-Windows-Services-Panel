using System;
using System.Windows;
using System.Windows.Controls;

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
            RunningProperty =
                DependencyProperty.Register("Running",
                    typeof(bool),
                    typeof(ServicePane),
                    new PropertyMetadata(false));

            ServiceNameProperty =
                DependencyProperty.Register("ServiceName",
                    typeof(String),
                    typeof(ServicePane),
                    new PropertyMetadata("Unknown"));
        }

        public String ServiceName
        {
            get { return (String)GetValue(ServiceNameProperty); }
            set { SetValue(ServiceNameProperty, value); }
        }
        public static DependencyProperty ServiceNameProperty { get; }

        public bool Running
        {
            get { return (bool)GetValue(RunningProperty); }
            set { SetValue(RunningProperty, value); }
        }
        public static DependencyProperty RunningProperty { get; }
    }
}
