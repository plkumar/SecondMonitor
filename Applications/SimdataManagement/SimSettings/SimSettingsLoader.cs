using System;
using System.Xml;

namespace SecondMonitor.SimdataManagement.SimSettings
{
    using DataModel.OperationalRange;
    using System.IO;
    using System.Linq;
    using System.Xml.Serialization;

    public class SimSettingsLoader
    {
        private const string FileSuffix = ".xml";

        private readonly XmlSerializer _xmlSerializer;

        public SimSettingsLoader(string path)
        {
            Path = path;
            _xmlSerializer = new XmlSerializer(typeof(DataSourceProperties));
        }

        public string Path { get;  }

        public DataSourceProperties GetDataSourceProperties(string sourceName)
        {
            string primaryFilePath = System.IO.Path.Combine(Path, sourceName + FileSuffix);
            DataSourceProperties baseProperties = File.Exists(primaryFilePath) ? LoadDataSourceProperties(primaryFilePath) : new DataSourceProperties() { SourceName = sourceName, ContainsRearTyres = true};
            return baseProperties;
        }

        public void SaveDataSourceProperties(DataSourceProperties properties)
        {
            Directory.CreateDirectory(Path);
            string path = System.IO.Path.Combine(Path, properties.SourceName + FileSuffix);

            using (FileStream file = File.Exists(path) ? File.Open(path, FileMode.Truncate) : File.Create(path))
            {
                _xmlSerializer.Serialize(file, properties);
            }
        }

        private DataSourceProperties LoadDataSourceProperties(string filePath)
        {
            try
            {
                using (TextReader file = File.OpenText(filePath))
                {

                    XmlReader reader = XmlReader.Create(file, new XmlReaderSettings() { CheckCharacters = false });
                    var foo = (DataSourceProperties)_xmlSerializer.Deserialize(reader);
                    foo.MigrateUp();
                    return foo;
                }

            }
            catch (Exception ex)
            {
                string copiedFilePath = ResetInvalidFile(filePath);
                throw new SimSettingsException($"Error in configuration file : {filePath}. File was recreated, but all car settings for sim were lost. Corrupted file was copied to {copiedFilePath}. ", ex);
            }
        }

        private static string ResetInvalidFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return string.Empty;
            }

            string newFileName = filePath + ".error";
            File.Move(filePath, newFileName);
            return newFileName;
        }
    }
}