using System;
using System.Collections.Generic;

namespace WindowsServicePanel.ViewModels.Events
{
    public class ServicesChangedEventArgs : EventArgs
    {
        public ServicesChangedEventArgs(String serviceName, bool selected)
        {
            if (selected)
            {
                ServicesSelected = new [] {serviceName};
                ServicesUnSelected = new String[] {};
            }
            else
            {
                ServicesSelected = new String[] { };
                ServicesUnSelected = new [] { serviceName };
            }
        }

        public IList<String> ServicesSelected { get; }
        public IList<String> ServicesUnSelected { get; }
    }
}