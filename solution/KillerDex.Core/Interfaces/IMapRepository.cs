using KillerDex.Core.Models;
using System;
using System.Collections.Generic;

namespace KillerDex.Core.Interfaces
{
    public interface IMapRepository
    {
        List<Map> GetAll();
        Map GetById(Guid id);
        void Add(Map map);
        void Update(Map map);
        void Delete(Guid id);
        bool ExistsByName(string name);
        bool ExistsByNameExcludingId(string name, Guid excludeId);
    }
}