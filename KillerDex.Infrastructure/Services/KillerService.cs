using System;
using System.Collections.Generic;
using KillerDex.Core.Interfaces;
using KillerDex.Core.Models;
using KillerDex.Core.Validators;
using KillerDex.Infrastructure.Repositories;

namespace KillerDex.Infrastructure.Services
{
    public class KillerService
    {
        private readonly IKillerRepository _repository;
        private readonly KillerValidator _validator;

        public KillerService()
        {
            _repository = new JsonKillerRepository();
            _validator = new KillerValidator(_repository);
        }

        public KillerService(IKillerRepository repository)
        {
            _repository = repository;
            _validator = new KillerValidator(_repository);
        }

        /// <summary>
        /// Gets all killers ordered by alias
        /// </summary>
        public List<Killer> GetAll()
        {
            return _repository.GetAll();
        }

        /// <summary>
        /// Gets a killer by its ID
        /// </summary>
        public Killer GetById(Guid id)
        {
            return _repository.GetById(id);
        }

        /// <summary>
        /// Adds a new killer after validation
        /// </summary>
        /// <returns>ValidationResult with any errors</returns>
        public ValidationResult Add(Killer killer)
        {
            var result = _validator.ValidateForCreate(killer);

            if (result.IsValid)
            {
                _repository.Add(killer);
            }

            return result;
        }

        /// <summary>
        /// Updates an existing killer after validation
        /// </summary>
        /// <returns>ValidationResult with any errors</returns>
        public ValidationResult Update(Killer killer)
        {
            var result = _validator.ValidateForUpdate(killer);

            if (result.IsValid)
            {
                _repository.Update(killer);
            }

            return result;
        }

        /// <summary>
        /// Deletes a killer by ID
        /// </summary>
        public void Delete(Guid id)
        {
            _repository.Delete(id);
        }

        /// <summary>
        /// Checks if a killer with the given alias exists
        /// </summary>
        public bool ExistsByAlias(string alias)
        {
            return _repository.ExistsByAlias(alias);
        }
    }
}