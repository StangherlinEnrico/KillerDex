using System;
using System.Collections.Generic;
using System.Linq;
using KillerDex.Core.Enums;
using KillerDex.Core.Interfaces;
using KillerDex.Core.Models;
using KillerDex.Core.Validators;
using KillerDex.Infrastructure.Repositories;

namespace KillerDex.Infrastructure.Services
{
    public class MatchService
    {
        private readonly IMatchRepository _repository;
        private readonly MatchValidator _validator;

        public MatchService()
        {
            _repository = new JsonMatchRepository();
            _validator = new MatchValidator();
        }

        public MatchService(IMatchRepository repository)
        {
            _repository = repository;
            _validator = new MatchValidator();
        }

        /// <summary>
        /// Gets all matches ordered by date (newest first)
        /// </summary>
        public List<Match> GetAll()
        {
            return _repository.GetAll();
        }

        /// <summary>
        /// Gets the most recent matches
        /// </summary>
        /// <param name="count">Number of matches to retrieve</param>
        public List<Match> GetRecent(int count)
        {
            return _repository.GetRecent(count);
        }

        /// <summary>
        /// Gets a match by its ID
        /// </summary>
        public Match GetById(Guid id)
        {
            return _repository.GetById(id);
        }

        /// <summary>
        /// Adds a new match after validation
        /// </summary>
        /// <returns>ValidationResult with any errors</returns>
        public ValidationResult Add(Match match)
        {
            var result = _validator.ValidateForCreate(match);

            if (result.IsValid)
            {
                _repository.Add(match);
            }

            return result;
        }

        /// <summary>
        /// Updates an existing match after validation
        /// </summary>
        /// <returns>ValidationResult with any errors</returns>
        public ValidationResult Update(Match match)
        {
            var result = _validator.ValidateForUpdate(match);

            if (result.IsValid)
            {
                _repository.Update(match);
            }

            return result;
        }

        /// <summary>
        /// Deletes a match by ID
        /// </summary>
        public void Delete(Guid id)
        {
            _repository.Delete(id);
        }

        /// <summary>
        /// Gets total number of matches
        /// </summary>
        public int GetTotalCount()
        {
            return _repository.GetTotalCount();
        }

        /// <summary>
        /// Gets number of wins (at least one survivor escaped)
        /// </summary>
        public int GetWinsCount()
        {
            return _repository.GetWinsCount();
        }

        /// <summary>
        /// Gets number of losses (no survivors)
        /// </summary>
        public int GetLossesCount()
        {
            return _repository.GetLossesCount();
        }

        /// <summary>
        /// Calculates win rate as a percentage
        /// </summary>
        public double GetWinRate()
        {
            int total = GetTotalCount();
            if (total == 0) return 0;

            return (double)GetWinsCount() / total * 100;
        }

        /// <summary>
        /// Gets complete dashboard statistics
        /// </summary>
        /// <param name="allyService">The ally service to resolve ally names</param>
        public DashboardStats GetDashboardStats(AllyService allyService)
        {
            var matches = GetAll();
            int total = matches.Count;
            int wins = matches.Count(m => m.IsWin);
            int losses = total - wins;
            double winRate = total > 0 ? (double)wins / total * 100 : 0;

            return new DashboardStats
            {
                TotalMatches = total,
                Wins = wins,
                Losses = losses,
                WinRate = winRate,
                BestAlly = GetBestAlly(matches, allyService),
                MostFacedKiller = GetMostFacedKiller(matches)
            };
        }

        /// <summary>
        /// Gets the ally with the highest win rate
        /// </summary>
        private AllyStats GetBestAlly(List<Match> matches, AllyService allyService)
        {
            if (matches == null || matches.Count == 0)
                return null;

            var allies = allyService.GetAll();
            if (allies == null || allies.Count == 0)
                return null;

            AllyStats best = null;

            foreach (var ally in allies)
            {
                var allyMatches = matches.Where(m => m.AllyIds != null && m.AllyIds.Contains(ally.Id)).ToList();

                if (allyMatches.Count == 0)
                    continue;

                int allyWins = allyMatches.Count(m => m.IsWin);
                double allyWinRate = (double)allyWins / allyMatches.Count * 100;

                if (best == null || allyWinRate > best.WinRate ||
                    (allyWinRate == best.WinRate && allyMatches.Count > best.MatchesPlayed))
                {
                    best = new AllyStats
                    {
                        AllyId = ally.Id,
                        Name = ally.Name,
                        MatchesPlayed = allyMatches.Count,
                        Wins = allyWins,
                        WinRate = allyWinRate
                    };
                }
            }

            return best;
        }

        /// <summary>
        /// Gets the killer faced most frequently
        /// </summary>
        private KillerStats GetMostFacedKiller(List<Match> matches)
        {
            if (matches == null || matches.Count == 0)
                return null;

            var killerGroups = matches
                .Where(m => m.Killer != KillerType.Unknown)
                .GroupBy(m => m.Killer)
                .Select(g => new KillerStats
                {
                    Killer = g.Key,
                    TimesFaced = g.Count(),
                    Wins = g.Count(m => m.IsWin),
                    WinRate = g.Count() > 0 ? (double)g.Count(m => m.IsWin) / g.Count() * 100 : 0
                })
                .OrderByDescending(k => k.TimesFaced)
                .ThenByDescending(k => k.WinRate)
                .FirstOrDefault();

            return killerGroups;
        }
    }
}