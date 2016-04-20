using System;
using System.Linq;
using System.Management;
using System.ServiceProcess;
using Microsoft.Win32;

namespace WindowsServicePanel.Sevices
{


    /// <summary>
    /// This class can be used to monitor a windows service
    /// http://www.codeproject.com/Tips/703289/How-to-Control-a-Windows-Service-from-Code
    /// </summary>
    public class WindowsServiceMonitor
    {
        #region Fields
        /// <summary>
        /// The Windows service that is controlled through the .NET ServiceController
        /// </summary>
        private readonly ServiceController _service;
        #endregion

        #region Properties
        /// <summary>
        /// Name of the Windows service
        /// </summary>
        public string ServiceName { get; }

        /// <summary>
        /// Tells us if the Windows service is running
        /// </summary>
        public bool IsRunning => _service.Status == ServiceControllerStatus.Running;

        /// <summary>
        /// Tells us if the Windows service is stopped
        /// </summary>
        public bool IsStopped => _service.Status == ServiceControllerStatus.Stopped;

        /// <summary>
        /// Tells us if the Windows Service is disabled
        /// </summary>
        public bool IsDisabled
        {
            get
            {
                try
                {
                    var query = $"SELECT * FROM Win32_Service WHERE Name = '{_service.ServiceName}'";
                    var querySearch = new ManagementObjectSearcher(query);
                    var services = querySearch.Get();

                    // Since we have set the servicename in the cons tructor we asume the first result is always
                    // the service we are looking for
                    foreach (var service in services.Cast<ManagementObject>())
                        return Convert.ToString(service.GetPropertyValue
                        ("StartMode")) == "Disabled";
                }
                catch
                {
                    return false;
                }

                return false;
            }
        }

        /// <summary>
        /// Can be called to enable the Windows Service
        /// </summary>
        public void Enable()
        {
            try
            {
                var key = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Services\" + ServiceName, true);
                key?.SetValue("Start", 2);
            }
            catch (Exception e)
            {
                throw new Exception("Could not enable the service, error: " + e.Message);
            }
        }

        /// <summary>
        /// Disables the Windows service
        /// </summary>
        public void Disable()
        {
            try
            {
                var key = Registry.LocalMachine.OpenSubKey
                (@"SYSTEM\CurrentControlSet\Services\" + ServiceName, true);
                key?.SetValue("Start", 4);
            }
            catch (Exception e)
            {
                throw new Exception("Could not disable the service, error: " + e.Message);
            }
        }

        /// <summary>
        /// This will give us the displayname for the 
        /// Windows Service that we have set in the constructor
        /// </summary>
        public string DisplayName => _service.DisplayName;

        #endregion

        #region Constructor
        /// <summary>
        /// Create this class for the given service.
        /// </summary>
        /// <param name="serviceName" />
        /// The name of the service (don't use the display name!!)
        public WindowsServiceMonitor(string serviceName)
        {
            _service = new ServiceController(serviceName);
            ServiceName = _service.ServiceName;
        }
        #endregion

        #region Start
        /// <summary>
        /// Start the Windows service, a timeout exception will be thrown when the service
        /// does not start in one minute.
        /// </summary>
        public void Start()
        {
            if (_service.Status != ServiceControllerStatus.Running ||
            _service.Status != ServiceControllerStatus.StartPending)
                _service.Start();

            _service.WaitForStatus(ServiceControllerStatus.Running, new TimeSpan(0, 0, 1, 0));
        }
        #endregion

        #region Stop
        /// <summary>
        /// Stop the Windows service, a timeout exception will be thrown when the service
        /// does not start in one minute.
        /// </summary>
        public void Stop()
        {
            _service.Stop();
            _service.WaitForStatus(ServiceControllerStatus.Stopped, new TimeSpan(0, 0, 1, 0));
        }
        #endregion

        #region Restart
        /// <summary>
        /// Restart the Windows service, a timeout exception will be thrown when the service
        /// does not stop or start in one minute.
        /// </summary>
        public void Restart()
        {
            Stop();
            Start();
        }
        #endregion
    }
}

