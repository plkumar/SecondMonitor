namespace SecondMonitor.Timing.TrackRecords.Controller
{
    using DataModel.TrackRecords;
    using ViewModels.Repository;

    public class TrackRecordsRepository : AbstractXmlRepository<SimulatorsRecords>
    {
        public TrackRecordsRepository(string directory)
        {
            RepositoryDirectory = directory;
        }
        protected override string RepositoryDirectory { get; }
        protected override string FileName => "TrackRecords.xml";
    }
}