using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using KillerDex.Core.Interfaces;
using KillerDex.Core.Models;

namespace KillerDex.Infrastructure.Repositories
{
    public class JsonAllyRepository : IAllyRepository
    {
        private readonly string _filePath;
        private List<Ally> _allies;

        public JsonAllyRepository()
        {
            _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "allies.json");
            LoadAllies();
        }

        public JsonAllyRepository(string filePath)
        {
            _filePath = filePath;
            LoadAllies();
        }

        private void LoadAllies()
        {
            if (File.Exists(_filePath))
            {
                string json = File.ReadAllText(_filePath);
                _allies = JsonConvert.DeserializeObject<List<Ally>>(json) ?? new List<Ally>();
            }
            else
            {
                _allies = new List<Ally>();
            }
        }

        private void SaveAllies()
        {
            string json = JsonConvert.SerializeObject(_allies, Formatting.Indented);
            File.WriteAllText(_filePath, json);
        }

        public List<Ally> GetAll()
        {
            return _allies.OrderBy(a => a.Name).ToList();
        }

        public Ally GetById(Guid id)
        {
            return _allies.FirstOrDefault(a => a.Id == id);
        }

        public void Add(Ally ally)
        {
            _allies.Add(ally);
            SaveAllies();
        }

        public void Update(Ally ally)
        {
            var existing = GetById(ally.Id);
            if (existing != null)
            {
                existing.Name = ally.Name;
                SaveAllies();
            }
        }

        public void Delete(Guid id)
        {
            var ally = GetById(id);
            if (ally != null)
            {
                _allies.Remove(ally);
                SaveAllies();
            }
        }

        public bool ExistsByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return false;

            return _allies.Any(a =>
                string.Equals(a.Name?.Trim(), name.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        public bool ExistsByNameExcludingId(string name, Guid excludeId)
        {
            if (string.IsNullOrWhiteSpace(name))
                return false;

            return _allies.Any(a =>
                a.Id != excludeId &&
                string.Equals(a.Name?.Trim(), name.Trim(), StringComparison.OrdinalIgnoreCase));
        }
    }
}