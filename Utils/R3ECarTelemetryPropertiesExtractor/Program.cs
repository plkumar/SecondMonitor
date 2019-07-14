namespace R3ECarTelemetryPropertiesExtractor
{
    using System.IO;
    using System.Xml.Serialization;
    using SecondMonitor.Telemetry.TelemetryApplication.Settings.DTO.DefaultCarProperties;

    class Program
    {
        static void Main(string[] args)
        {
            string[] bumpData = File.ReadAllLines(@"c:\Users\Darkman\Documents\Visual Studio 2015\Projects\SecondMonitor\Artifacts\Data\R3E\Bumpstage.txt");
            string[] reboundData = File.ReadAllLines(@"c:\Users\Darkman\Documents\Visual Studio 2015\Projects\SecondMonitor\Artifacts\Data\R3E\ReboundStage.txt");
            SuspensionDataFiller suspensionDataFiller = new SuspensionDataFiller();
            var properties = suspensionDataFiller.FillData(bumpData, reboundData);


            XmlSerializer xmlSerializer = new XmlSerializer(typeof(DefaultR3ECarsProperties));
            string fileName = @"c:\Users\Darkman\Documents\Visual Studio 2015\Projects\SecondMonitor\Artifacts\Data\R3E\R3EDefaultProperties.xml";
            using (var stream = File.Exists(fileName) ? File.Open(fileName, FileMode.Truncate) : File.OpenWrite(fileName))
            {
                xmlSerializer.Serialize(stream, properties);
            }
        }
    }
}
