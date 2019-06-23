namespace AcTyresExtractor.Extractor
{
    using System.Collections.Generic;
    using System.IO;
    using System.Xml.Serialization;
    using SecondMonitor.DataModel.OperationalRange;

    public class FoldersExtractor
    {
        private readonly TyreOperationalRangeExtractor _tyreOperationalRangeExtractor;

        public FoldersExtractor()
        {
            _tyreOperationalRangeExtractor = new TyreOperationalRangeExtractor();
        }
        public void ExtractDataFromAllSubFolders(string folder)
        {
            List<CarModelProperties> carModelsProperties = new List<CarModelProperties>();
            DirectoryInfo parentDirectoryInfo = new DirectoryInfo(folder);
            foreach (DirectoryInfo directory in parentDirectoryInfo.GetDirectories())
            {
                var tyreCompounds = _tyreOperationalRangeExtractor.ExtractFromFolder(directory.FullName);
                if (tyreCompounds.Count == 0)
                {
                    continue;
                }

                carModelsProperties.Add(new CarModelProperties()
                {
                    Name = FormatACName(directory.Name),
                    TyreCompoundsProperties = tyreCompounds
                });
            }

            DataSourceProperties acProperties = new DataSourceProperties()
            {
                CarModelsProperties = carModelsProperties,
                SourceName = "AC_DEFAULT",
                ContainsRearTyres = true
            };
            var xmlSerializer = new XmlSerializer(typeof(DataSourceProperties));
            var fileName = Path.Combine(folder, "AcTyreProperties.xml");
            using (FileStream file = File.Exists(fileName) ? File.Open(fileName, FileMode.Truncate) : File.Create(fileName))
            {
                xmlSerializer.Serialize(file, acProperties);
            }
        }

        private static string FormatACName(string name)
        {
            return name.Replace("ks_", string.Empty).Replace("_", " ");
        }
    }
}