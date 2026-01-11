using System;
using System.Collections.Generic;

namespace KillerDex.Models
{
    public class Match
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public List<Guid> AllyIds { get; set; }
        public Guid MapId { get; set; }
        public Guid KillerId { get; set; }
        public string FirstHook { get; set; }
        public int GeneratorsCompleted { get; set; }
        public int Survivors { get; set; }
        public string Notes { get; set; }

        public Match()
        {
            Id = Guid.NewGuid();
            Date = DateTime.Now;
            AllyIds = new List<Guid>();
        }
    }
}