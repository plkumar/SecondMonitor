namespace SecondMonitor.Telemetry.TelemetryApplication.Settings.DTO.ChartProperties
{
    using System.Xml.Serialization;
    using DataModel.BasicProperties;

    public class CamberHistogram
    {
        public CamberHistogram()
        {
            FromCamber = Angle.GetFromDegrees(-10);
            ToCamber = Angle.GetFromDegrees(1);
            FromG = 0.5;
            ToG = 5;
            IsLoadedSelected = true;
            IsUnloadedSelected = false;
            BandSize = Angle.GetFromDegrees(0.1);
        }

        public Angle FromCamber { get; set; }
        public Angle ToCamber { get; set; }

        [XmlAttribute]
        public double FromG { get; set; }

        [XmlAttribute]
        public double ToG { get; set; }

        [XmlAttribute]
        public bool IsLoadedSelected { get; set; }

        [XmlAttribute]
        public bool IsUnloadedSelected { get; set; }

        public Angle BandSize { get; set; }
    }
}