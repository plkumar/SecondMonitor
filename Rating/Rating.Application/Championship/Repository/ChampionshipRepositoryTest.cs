namespace SecondMonitor.Rating.Application.Championship.Repository
{
    using Common.DataModel.Championship;

    public class ChampionshipRepositoryTest : IChampionshipsRepository
    {
        public AllChampionshipsDto LoadRatingsOrCreateNew()
        {
            var allChampionshipsDto = new AllChampionshipsDto();
            allChampionshipsDto.Championships.Add(new ChampionshipDto()
            {
                SimulatorName = "R3E",
                ChampionshipState = ChampionshipState.NotStarted,
                ChampionshipName = "Not Started Champ",
                TotalEvents = 20,
                CurrentEventIndex = 1,
                NextTrack = "Monza",
                Position = 0,
                TotalDrivers = 0,
            });

            allChampionshipsDto.Championships.Add(new ChampionshipDto()
            {
                SimulatorName = "R3E",
                ChampionshipState = ChampionshipState.Started,
                ChampionshipName = "Started Champ",
                TotalEvents = 20,
                CurrentEventIndex = 15,
                NextTrack = "Slovakia Ring",
                Position = 1,
                TotalDrivers = 23,
                ClassName = "Porsche GT 3",
            });

            allChampionshipsDto.Championships.Add(new ChampionshipDto()
            {
                SimulatorName = "R3E",
                ChampionshipState = ChampionshipState.Finished,
                ChampionshipName = "Finished Camp",
                TotalEvents = 20,
                CurrentEventIndex = 20,
                NextTrack = "",
                Position = 12,
                TotalDrivers = 23,
                ClassName = "Porsche GT 3",
            });

            return allChampionshipsDto;
        }

        public void Save(AllChampionshipsDto allChampionshipsDto)
        {
        }
    }
}