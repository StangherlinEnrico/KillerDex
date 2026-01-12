using KillerDex.Core.Models;
using System;
using System.Collections.Generic;

namespace KillerDex.Core.Interfaces
{
    public interface IKillerRepository
    {
        List<Killer> GetAll();
        Killer GetById(Guid id);
        void Add(Killer killer);
        void Update(Killer killer);
        void Delete(Guid id);
        bool ExistsByAlias(string alias);
        bool ExistsByAliasExcludingId(string alias, Guid excludeId);
    }
}