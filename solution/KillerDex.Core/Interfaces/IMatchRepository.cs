using KillerDex.Core.Models;
using System;
using System.Collections.Generic;

namespace KillerDex.Core.Interfaces
{
    public interface IMatchRepository
    {
        List<Match> GetAll();
        List<Match> GetRecent(int count);
        Match GetById(Guid id);
        void Add(Match match);
        void Update(Match match);
        void Delete(Guid id);
        int GetTotalCount();
        int GetWinsCount();
        int GetLossesCount();
    }
}