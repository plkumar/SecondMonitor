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
                TotalRaces = 20,
                CurrentRace = 1,
            });

            allChampionshipsDto.Championships.Add(new ChampionshipDto()
            {
                SimulatorName = "R3E",
                ChampionshipState = ChampionshipState.Started,
                ChampionshipName = "Not started Champ",
                TotalRaces = 20,
                CurrentRace = 15,
            });

            allChampionshipsDto.Championships.Add(new ChampionshipDto()
            {
                SimulatorName = "R3E",
                ChampionshipState = ChampionshipState.Finished,
                ChampionshipName = "Finished Camp",
                TotalRaces = 20,
                CurrentRace = 20,
            });

            return allChampionshipsDto;
        }

        public void Save(AllChampionshipsDto allChampionshipsDto)
        {
        }
    }
}