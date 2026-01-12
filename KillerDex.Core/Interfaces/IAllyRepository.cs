using KillerDex.Core.Models;
using System;
using System.Collections.Generic;

namespace KillerDex.Core.Interfaces
{
    public interface IAllyRepository
    {
        List<Ally> GetAll();
        Ally GetById(Guid id);
        void Add(Ally ally);
        void Update(Ally ally);
        void Delete(Guid id);
        bool ExistsByName(string name);
        bool ExistsByNameExcludingId(string name, Guid excludeId);
    }
}