using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Script.Serialization;
using KillerDex.Models;

namespace KillerDex.Services
{
    public class AllyService
    {
        private readonly string _filePath;
        private List<Ally> _allies;
        private readonly JavaScriptSerializer _serializer;

        public AllyService()
        {
            _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "allies.json");
            _serializer = new JavaScriptSerializer();
            LoadAllies();
        }

        private void LoadAllies()
        {
            if (File.Exists(_filePath))
            {
                string json = File.ReadAllText(_filePath);
                _allies = _serializer.Deserialize<List<Ally>>(json) ?? new List<Ally>();
            }
            else
            {
                _allies = new List<Ally>();
            }
        }

        private void SaveAllies()
        {
            string json = _serializer.Serialize(_allies);
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
    }
}