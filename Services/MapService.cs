using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Script.Serialization;
using KillerDex.Models;

namespace KillerDex.Services
{
    public class MapService
    {
        private readonly string _filePath;
        private List<Map> _maps;
        private readonly JavaScriptSerializer _serializer;

        public MapService()
        {
            _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "maps.json");
            _serializer = new JavaScriptSerializer();
            LoadMaps();
        }

        private void LoadMaps()
        {
            if (File.Exists(_filePath))
            {
                string json = File.ReadAllText(_filePath);
                _maps = _serializer.Deserialize<List<Map>>(json) ?? new List<Map>();
            }
            else
            {
                _maps = new List<Map>();
            }
        }

        private void SaveMaps()
        {
            string json = _serializer.Serialize(_maps);
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
    }
}