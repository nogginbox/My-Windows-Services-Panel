using System;

namespace WindowsServicePanel.Sevices
{
    public struct ServiceInfo
    {
        public ServiceInfo(String name, String displayName, String description, String startMode, String status)
        {
            Name = name;
            DisplayName = displayName;
            Description = description;
            StartMode = startMode;
            State = status;
            IsRunning = status == "Running";
        }

        public String Description { get; private set; }
        public String DisplayName { get; private set; }
        public String Name { get; private set; }
        public String StartMode { get; private set; }
        public string State { get; private set; }
        public bool IsRunning { get; set; }
    }
}