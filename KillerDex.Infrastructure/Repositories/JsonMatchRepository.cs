using KillerDex.Core.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace KillerDex.Infrastructure.Repositories
{
    public class JsonMatchRepository
    {
        private readonly string _filePath;
        private List<Match> _matches;

        public JsonMatchRepository()
        {
            _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "matches.json");
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
                existing.MapId = match.MapId;
                existing.KillerId = match.KillerId;
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
            return _matches.Count(m => m.Survivors > 0);
        }

        public int GetLossesCount()
        {
            return _matches.Count(m => m.Survivors == 0);
        }
    }
}
