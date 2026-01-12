using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using KillerDex.Core.Interfaces;
using KillerDex.Core.Models;

namespace KillerDex.Infrastructure.Repositories
{
    public class JsonMapRepository : IMapRepository
    {
        private readonly string _filePath;
        private List<Map> _maps;

        public JsonMapRepository()
        {
            _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "maps.json");
            LoadMaps();
        }

        public JsonMapRepository(string filePath)
        {
            _filePath = filePath;
            LoadMaps();
        }

        private void LoadMaps()
        {
            if (File.Exists(_filePath))
            {
                string json = File.ReadAllText(_filePath);
                _maps = JsonConvert.DeserializeObject<List<Map>>(json) ?? new List<Map>();
            }
            else
            {
                _maps = new List<Map>();
            }
        }

        private void SaveMaps()
        {
            string json = JsonConvert.SerializeObject(_maps, Formatting.Indented);
            File.WriteAllText(_filePath, json);
        }

        public List<Map> GetAll()
        {
            return _maps.OrderBy(m => m.Name).ToList();
        }

        public Map GetById(Guid id)
        {
            return _maps.FirstOrDefault(m => m.Id == id);
        }

        public void Add(Map map)
        {
            _maps.Add(map);
            SaveMaps();
        }

        public void Update(Map map)
        {
            var existing = GetById(map.Id);
            if (existing != null)
            {
                existing.Name = map.Name;
                SaveMaps();
            }
        }

        public void Delete(Guid id)
        {
            var map = GetById(id);
            if (map != null)
            {
                _maps.Remove(map);
                SaveMaps();
            }
        }

        public bool ExistsByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return false;

            return _maps.Any(m =>
                string.Equals(m.Name?.Trim(), name.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        public bool ExistsByNameExcludingId(string name, Guid excludeId)
        {
            if (string.IsNullOrWhiteSpace(name))
                return false;

            return _maps.Any(m =>
                m.Id != excludeId &&
                string.Equals(m.Name?.Trim(), name.Trim(), StringComparison.OrdinalIgnoreCase));
        }
    }
}