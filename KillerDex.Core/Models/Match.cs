using System;
using System.Collections.Generic;

namespace KillerDex.Core.Models
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
        public List<string> Survivors { get; set; }
        public string Notes { get; set; }

        public Match()
        {
            Id = Guid.NewGuid();
            Date = DateTime.Now;
            AllyIds = new List<Guid>();
            Survivors = new List<string>();
        }

        /// <summary>
        /// Returns true if at least one survivor escaped
        /// </summary>
        public bool IsWin => Survivors != null && Survivors.Count > 0;

        /// <summary>
        /// Returns the number of survivors
        /// </summary>
        public int SurvivorsCount => Survivors?.Count ?? 0;
    }
}