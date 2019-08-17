namespace SecondMonitor.ViewModels.Repository
{
    using System.IO;
    using System.Xml.Serialization;

    public abstract class AbstractXmlRepository<T> where T : class, new()
    {
        private readonly object _lockObject = new  object();
        private readonly XmlSerializer _xmlSerializer;

        protected AbstractXmlRepository()
        {
            _xmlSerializer = new XmlSerializer(typeof(T));
        }

        protected abstract string RepositoryDirectory { get; }
        protected abstract string FileName { get; }

        private void CheckDirectory()
        {
            if (!Directory.Exists(RepositoryDirectory))
            {
                Directory.CreateDirectory(RepositoryDirectory);
            }
        }

        public T LoadRatingsOrCreateNew()
        {
            string fileName = Path.Combine(RepositoryDirectory, FileName);
            lock (_lockObject)
            {
                CheckDirectory();
                if (!File.Exists(fileName))
                {
                    return new T();
                }

                using (FileStream file = File.Open(fileName, FileMode.Open, FileAccess.Read))
                {
                    T deserialized = _xmlSerializer.Deserialize(file) as T;
                    return deserialized ?? new T();
                }
            }
        }

        public void Save(T toSave)
        {
            string fileName = Path.Combine(RepositoryDirectory, FileName);
            lock (_lockObject)
            {
                CheckDirectory();
                using (FileStream file = File.Exists(fileName) ? File.Open(fileName, FileMode.Truncate) : File.Create(fileName))
                {
                    _xmlSerializer.Serialize(file, toSave);
                }
            }
        }
    }
}