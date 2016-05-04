using System.Collections.Generic;

namespace WindowsServicePanel.ViewModels.SelectServicesWindow
{
    public class SelectServicesViewModel : ObservableViewModelBase
    {
        public IList<ServiceViewModel> Services { get; set; }
    }
}