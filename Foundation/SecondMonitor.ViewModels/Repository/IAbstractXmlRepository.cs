namespace SecondMonitor.ViewModels.Repository
{
    public interface IAbstractXmlRepository<T> where T : class, new()
    {
        T LoadRatingsOrCreateNew();
        void Save(T toSave);
    }
}