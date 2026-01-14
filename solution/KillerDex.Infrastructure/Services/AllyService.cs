using System;
using System.Collections.Generic;
using KillerDex.Core.Interfaces;
using KillerDex.Core.Models;
using KillerDex.Core.Validators;
using KillerDex.Infrastructure.Repositories;

namespace KillerDex.Infrastructure.Services
{
    public class AllyService
    {
        private readonly IAllyRepository _repository;
        private readonly AllyValidator _validator;

        public AllyService()
        {
            _repository = new JsonAllyRepository();
            _validator = new AllyValidator(_repository);
        }

        public AllyService(IAllyRepository repository)
        {
            _repository = repository;
            _validator = new AllyValidator(_repository);
        }

        /// <summary>
        /// Gets all allies ordered by name
        /// </summary>
        public List<Ally> GetAll()
        {
            return _repository.GetAll();
        }

        /// <summary>
        /// Gets an ally by its ID
        /// </summary>
        public Ally GetById(Guid id)
        {
            return _repository.GetById(id);
        }

        /// <summary>
        /// Adds a new ally after validation
        /// </summary>
        /// <returns>ValidationResult with any errors</returns>
        public ValidationResult Add(Ally ally)
        {
            var result = _validator.ValidateForCreate(ally);

            if (result.IsValid)
            {
                _repository.Add(ally);
            }

            return result;
        }

        /// <summary>
        /// Updates an existing ally after validation
        /// </summary>
        /// <returns>ValidationResult with any errors</returns>
        public ValidationResult Update(Ally ally)
        {
            var result = _validator.ValidateForUpdate(ally);

            if (result.IsValid)
            {
                _repository.Update(ally);
            }

            return result;
        }

        /// <summary>
        /// Deletes an ally by ID
        /// </summary>
        public void Delete(Guid id)
        {
            _repository.Delete(id);
        }

        /// <summary>
        /// Checks if an ally with the given name exists
        /// </summary>
        public bool ExistsByName(string name)
        {
            return _repository.ExistsByName(name);
        }
    }
}