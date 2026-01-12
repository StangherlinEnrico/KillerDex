using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using KillerDex.Core.Interfaces;
using KillerDex.Core.Models;

namespace KillerDex.Infrastructure.Repositories
{
    public class JsonKillerRepository : IKillerRepository
    {
        private readonly string _filePath;
        private List<Killer> _killers;

        public JsonKillerRepository()
        {
            _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "killers.json");
            LoadKillers();
        }

        public JsonKillerRepository(string filePath)
        {
            _filePath = filePath;
            LoadKillers();
        }

        private void LoadKillers()
        {
            if (File.Exists(_filePath))
            {
                string json = File.ReadAllText(_filePath);
                _killers = JsonConvert.DeserializeObject<List<Killer>>(json) ?? new List<Killer>();
            }
            else
            {
                _killers = new List<Killer>();
            }
        }

        private void SaveKillers()
        {
            string json = JsonConvert.SerializeObject(_killers, Formatting.Indented);
            File.WriteAllText(_filePath, json);
        }

        public List<Killer> GetAll()
        {
            return _killers.OrderBy(k => k.Alias).ToList();
        }

        public Killer GetById(Guid id)
        {
            return _killers.FirstOrDefault(k => k.Id == id);
        }

        public void Add(Killer killer)
        {
            _killers.Add(killer);
            SaveKillers();
        }

        public void Update(Killer killer)
        {
            var existing = GetById(killer.Id);
            if (existing != null)
            {
                existing.Alias = killer.Alias;
                SaveKillers();
            }
        }

        public void Delete(Guid id)
        {
            var killer = GetById(id);
            if (killer != null)
            {
                _killers.Remove(killer);
                SaveKillers();
            }
        }

        public bool ExistsByAlias(string alias)
        {
            if (string.IsNullOrWhiteSpace(alias))
                return false;

            return _killers.Any(k =>
                string.Equals(k.Alias?.Trim(), alias.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        public bool ExistsByAliasExcludingId(string alias, Guid excludeId)
        {
            if (string.IsNullOrWhiteSpace(alias))
                return false;

            return _killers.Any(k =>
                k.Id != excludeId &&
                string.Equals(k.Alias?.Trim(), alias.Trim(), StringComparison.OrdinalIgnoreCase));
        }
    }
}