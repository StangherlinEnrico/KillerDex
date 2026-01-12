using System;
using System.Collections.Generic;
using System.Linq;
using KillerDex.Core.Interfaces;
using KillerDex.Core.Models;

namespace KillerDex.Tests.Mocks
{
    public class MockAllyRepository : IAllyRepository
    {
        private readonly List<Ally> _allies = new List<Ally>();

        public void SeedData(params Ally[] allies)
        {
            _allies.Clear();
            _allies.AddRange(allies);
        }

        public List<Ally> GetAll()
        {
            return _allies.ToList();
        }

        public Ally GetById(Guid id)
        {
            return _allies.FirstOrDefault(a => a.Id == id);
        }

        public void Add(Ally ally)
        {
            _allies.Add(ally);
        }

        public void Update(Ally ally)
        {
            var existing = GetById(ally.Id);
            if (existing != null)
            {
                existing.Name = ally.Name;
            }
        }

        public void Delete(Guid id)
        {
            var ally = GetById(id);
            if (ally != null)
            {
                _allies.Remove(ally);
            }
        }

        public bool ExistsByName(string name)
        {
            return _allies.Any(a =>
                string.Equals(a.Name?.Trim(), name?.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        public bool ExistsByNameExcludingId(string name, Guid excludeId)
        {
            return _allies.Any(a =>
                a.Id != excludeId &&
                string.Equals(a.Name?.Trim(), name?.Trim(), StringComparison.OrdinalIgnoreCase));
        }
    }
}