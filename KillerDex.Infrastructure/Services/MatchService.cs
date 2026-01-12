using System;
using System.Collections.Generic;
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
    }
}