﻿namespace SecondMonitor.Rating.Application.Championship.Pool
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common.DataModel.Championship;
    using Repository;

    public class ChampionshipsPool : IChampionshipsPool
    {
        private readonly IChampionshipsRepository _championshipsRepository;

        private readonly Lazy<AllChampionshipsDto> _allChampionshipDtoLazy;

        public ChampionshipsPool(IChampionshipsRepository championshipsRepository)
        {
            _championshipsRepository = championshipsRepository;
            _allChampionshipDtoLazy = new Lazy<AllChampionshipsDto>(LoadAllChampionshipsDto);
        }

        protected AllChampionshipsDto AllChampionshipsDto => _allChampionshipDtoLazy.Value;

        public event EventHandler<ChampionshipEventArgs> ChampionshipAdded;
        public event EventHandler<ChampionshipEventArgs> ChampionshipRemoved;
        public event EventHandler<ChampionshipEventArgs> ChampionshipUpdated;

        public IReadOnlyCollection<ChampionshipDto> GetAllChampionshipDtos()
        {
            return AllChampionshipsDto.Championships.AsReadOnly();
        }

        public IReadOnlyCollection<ChampionshipDto> GetAllChampionshipDtos(string simulatorName)
        {
            return AllChampionshipsDto.Championships.Where(x => x.SimulatorName == simulatorName).ToList();
        }

        public void AddNewChampionship(ChampionshipDto championshipDto)
        {
            AllChampionshipsDto.Championships.Add(championshipDto);
            _championshipsRepository.Save(AllChampionshipsDto);
            ChampionshipAdded?.Invoke(this, new ChampionshipEventArgs(championshipDto));
        }

        public void RemoveChampionship(ChampionshipDto championshipDto)
        {
            AllChampionshipsDto.Championships.RemoveAll(x => x.ChampionshipGlobalId == championshipDto.ChampionshipGlobalId);
            _championshipsRepository.Save(AllChampionshipsDto);
            ChampionshipRemoved?.Invoke(this, new ChampionshipEventArgs(championshipDto));
        }

        public void UpdateChampionship(ChampionshipDto championshipDto)
        {
            AllChampionshipsDto.Championships.RemoveAll(x => x.ChampionshipGlobalId == championshipDto.ChampionshipGlobalId);
            AllChampionshipsDto.Championships.Add(championshipDto);
            _championshipsRepository.Save(AllChampionshipsDto);
            ChampionshipUpdated?.Invoke(this, new ChampionshipEventArgs(championshipDto));
        }

        private AllChampionshipsDto LoadAllChampionshipsDto()
        {
            return _championshipsRepository.LoadRatingsOrCreateNew();
        }
    }
}