using System;
using KillerDex.Core.Enums;

namespace KillerDex.Core.Models
{
    /// <summary>
    /// Contains dashboard statistics for the main form
    /// </summary>
    public class DashboardStats
    {
        /// <summary>
        /// Total number of matches played
        /// </summary>
        public int TotalMatches { get; set; }

        /// <summary>
        /// Number of matches where I survived
        /// </summary>
        public int Wins { get; set; }

        /// <summary>
        /// Number of matches where I did not survive
        /// </summary>
        public int Losses { get; set; }

        /// <summary>
        /// Win rate as a percentage (0-100)
        /// </summary>
        public double WinRate { get; set; }

        /// <summary>
        /// The ally with the highest win rate when playing together
        /// </summary>
        public AllyStats BestAlly { get; set; }

        /// <summary>
        /// The killer faced most frequently
        /// </summary>
        public KillerStats MostFacedKiller { get; set; }
    }

    /// <summary>
    /// Statistics for an ally
    /// </summary>
    public class AllyStats
    {
        /// <summary>
        /// Ally ID
        /// </summary>
        public Guid AllyId { get; set; }

        /// <summary>
        /// Ally name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Number of matches played together
        /// </summary>
        public int MatchesPlayed { get; set; }

        /// <summary>
        /// Number of wins when playing together
        /// </summary>
        public int Wins { get; set; }

        /// <summary>
        /// Win rate when playing together (0-100)
        /// </summary>
        public double WinRate { get; set; }
    }

    /// <summary>
    /// Statistics for a killer
    /// </summary>
    public class KillerStats
    {
        /// <summary>
        /// The killer type
        /// </summary>
        public KillerType Killer { get; set; }

        /// <summary>
        /// Number of times faced
        /// </summary>
        public int TimesFaced { get; set; }

        /// <summary>
        /// Number of wins against this killer
        /// </summary>
        public int Wins { get; set; }

        /// <summary>
        /// Win rate against this killer (0-100)
        /// </summary>
        public double WinRate { get; set; }
    }
}
