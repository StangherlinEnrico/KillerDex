using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using KillerDex.Core.Interfaces;
using KillerDex.Core.Models;

namespace KillerDex.Infrastructure.Repositories
{
    public class JsonMatchRepository : IMatchRepository
    {
        private readonly string _filePath;
        private List<Match> _matches;

        public JsonMatchRepository()
        {
            _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "matches.json");
            LoadMatches();
        }

        public JsonMatchRepository(string filePath)
        {
            _filePath = filePath;
            LoadMatches();
        }

        private void LoadMatches()
        {
            if (File.Exists(_filePath))
            {
                string json = File.ReadAllText(_filePath);
                _matches = JsonConvert.DeserializeObject<List<Match>>(json) ?? new List<Match>();
            }
            else
            {
                _matches = new List<Match>();
            }
        }

        private void SaveMatches()
        {
            string json = JsonConvert.SerializeObject(_matches, Formatting.Indented);
            File.WriteAllText(_filePath, json);
        }

        public List<Match> GetAll()
        {
            return _matches.OrderByDescending(m => m.Date).ToList();
        }

        public List<Match> GetRecent(int count)
        {
            return _matches.OrderByDescending(m => m.Date).Take(count).ToList();
        }

        public Match GetById(Guid id)
        {
            return _matches.FirstOrDefault(m => m.Id == id);
        }

        public void Add(Match match)
        {
            _matches.Add(match);
            SaveMatches();
        }

        public void Update(Match match)
        {
            var existing = GetById(match.Id);
            if (existing != null)
            {
                existing.Date = match.Date;
                existing.AllyIds = match.AllyIds;
                existing.Map = match.Map;
                existing.Killer = match.Killer;
                existing.FirstHook = match.FirstHook;
                existing.GeneratorsCompleted = match.GeneratorsCompleted;
                existing.Survivors = match.Survivors;
                existing.Notes = match.Notes;
                SaveMatches();
            }
        }

        public void Delete(Guid id)
        {
            var match = GetById(id);
            if (match != null)
            {
                _matches.Remove(match);
                SaveMatches();
            }
        }

        public int GetTotalCount()
        {
            return _matches.Count;
        }

        public int GetWinsCount()
        {
            // IsWin è true se Survivors ha almeno un elemento
            return _matches.Count(m => m.IsWin);
        }

        public int GetLossesCount()
        {
            // Loss è quando nessuno è sopravvissuto
            return _matches.Count(m => !m.IsWin);
        }
    }
}