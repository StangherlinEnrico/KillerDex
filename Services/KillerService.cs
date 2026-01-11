using KillerDex.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace KillerDex.Services
{
    public class KillerService
    {
        private readonly string _filePath;
        private List<Killer> _killers;

        public KillerService()
        {
            _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "killers.json");
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
            return _killers.ToList();
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
    }
}