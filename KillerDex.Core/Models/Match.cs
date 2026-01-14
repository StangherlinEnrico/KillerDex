using System;
using System.Collections.Generic;
using System.Linq;
using KillerDex.Core.Enums;

namespace KillerDex.Core.Models
{
    public class Match
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public List<Guid> AllyIds { get; set; }
        public MapType Map { get; set; }
        public KillerType Killer { get; set; }
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
        /// Known values for "Myself" across all supported languages
        /// </summary>
        private static readonly string[] MyselfValues = { "Myself", "Me stesso" };

        /// <summary>
        /// Returns true if I survived (my name is in the survivors list)
        /// This is the actual win condition for the player
        /// </summary>
        public bool DidISurvive => Survivors != null && Survivors.Any(s => MyselfValues.Contains(s, StringComparer.OrdinalIgnoreCase));

        /// <summary>
        /// Returns true if at least one survivor escaped
        /// </summary>
        public bool IsWin => DidISurvive;

        /// <summary>
        /// Returns the number of survivors
        /// </summary>
        public int SurvivorsCount => Survivors?.Count ?? 0;
    }
}