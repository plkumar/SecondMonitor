namespace SecondMonitor.PluginsConfiguration.Common.DataModel
{
    using System;
    using System.Xml.Serialization;

    [Serializable]
    public class F12019Configuration
    {
        public F12019Configuration()
        {
            Port = 20777;
        }

        [XmlAttribute]
        public int Port { get; set; }
    }
}