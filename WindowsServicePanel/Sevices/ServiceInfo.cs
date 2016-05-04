using System;

namespace WindowsServicePanel.Sevices
{
    public struct ServiceInfo
    {
        public ServiceInfo(String name, String displayName, String description, String startMode, bool isRunning)
        {
            Name = name;
            DisplayName = displayName;
            Description = description;
            StartMode = startMode;
            IsRunning = isRunning;
        }

        public String Description { get; private set; }
        public String DisplayName { get; private set; }
        public String Name { get; private set; }
        public String StartMode { get; private set; }
        public bool IsRunning { get; set; }
    }
}