using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;

namespace WindowsServicePanel.Sevices
{
    public class WindowsServicesService
    {
        private static ServiceInfo ConvertToServiceInfo(ManagementBaseObject service)
        {
            //var text = service.GetText(TextFormat.CimDtd20);

            return new ServiceInfo(
                Convert.ToString(service.GetPropertyValue("Name")),
                Convert.ToString(service.GetPropertyValue("DisplayName")),
                Convert.ToString(service.GetPropertyValue("Description")),
                Convert.ToString(service.GetPropertyValue("StartMode")),
                Convert.ToString(service.GetPropertyValue("State"))
                );
        }


        public IEnumerable<ServiceInfo> GetAllServices()
        {
            ManagementObjectCollection services;
            try
            {
                var querySearch = new ManagementObjectSearcher("SELECT * FROM Win32_Service");    
                services = querySearch.Get();
            }
            catch
            {
                yield break;
            }

            foreach (var service in services.Cast<ManagementObject>())
            { 
                yield return ConvertToServiceInfo(service);        
            }
            
        }

        public ServiceInfo? GetServiceByName(String name)
        {
            try
            {
                var query = $"SELECT * FROM Win32_Service WHERE Name = '{name}'";
                var querySearch = new ManagementObjectSearcher(query);
                var service = querySearch.Get().Cast<ManagementObject>().First();
                return ConvertToServiceInfo(service);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}