using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using WindowsServicePanel.ViewModels.Events;

namespace WindowsServicePanel.ViewModels.SelectServicesWindow
{
    public class SelectServicesViewModel : ObservableViewModelBase
    {
        public SelectServicesViewModel(IEnumerable<ServiceViewModel> services)
        {
            Services = new List<ServiceViewModel>(services
                .OrderByDescending(s => s.Selected)
                .ThenBy(a => a.Name)
            );
            foreach (var serviceViewModel in Services)
            {
                serviceViewModel.PropertyChanged += OnServiceSelectedChanged;
            }
        }

        public IList<ServiceViewModel> Services { get; }

        public event SelectedServicesChangedEventHandler SelectedServicesChanged;
        public delegate void SelectedServicesChangedEventHandler(object sender, ServicesChangedEventArgs e);
        
        private void OnServiceSelectedChanged(object sender, PropertyChangedEventArgs eventArgs)
        {
            var serviceViewModel = sender as ServiceViewModel;
            if (serviceViewModel == null) return;

            var args = new ServicesChangedEventArgs(serviceViewModel.Name, serviceViewModel.Selected);
            SelectedServicesChanged?.Invoke(this, args);
        }
    }
}