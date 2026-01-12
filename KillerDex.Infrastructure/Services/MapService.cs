using System;
using System.Collections.Generic;
using KillerDex.Core.Interfaces;
using KillerDex.Core.Models;
using KillerDex.Core.Validators;
using KillerDex.Infrastructure.Repositories;

namespace KillerDex.Infrastructure.Services
{
    public class MapService
    {
        private readonly IMapRepository _repository;
        private readonly MapValidator _validator;

        public MapService()
        {
            _repository = new JsonMapRepository();
            _validator = new MapValidator(_repository);
        }

        public MapService(IMapRepository repository)
        {
            _repository = repository;
            _validator = new MapValidator(_repository);
        }

        /// <summary>
        /// Gets all maps ordered by name
        /// </summary>
        public List<Map> GetAll()
        {
            return _repository.GetAll();
        }

        /// <summary>
        /// Gets a map by its ID
        /// </summary>
        public Map GetById(Guid id)
        {
            return _repository.GetById(id);
        }

        /// <summary>
        /// Adds a new map after validation
        /// </summary>
        /// <returns>ValidationResult with any errors</returns>
        public ValidationResult Add(Map map)
        {
            var result = _validator.ValidateForCreate(map);

            if (result.IsValid)
            {
                _repository.Add(map);
            }

            return result;
        }

        /// <summary>
        /// Updates an existing map after validation
        /// </summary>
        /// <returns>ValidationResult with any errors</returns>
        public ValidationResult Update(Map map)
        {
            var result = _validator.ValidateForUpdate(map);

            if (result.IsValid)
            {
                _repository.Update(map);
            }

            return result;
        }

        /// <summary>
        /// Deletes a map by ID
        /// </summary>
        public void Delete(Guid id)
        {
            _repository.Delete(id);
        }

        /// <summary>
        /// Checks if a map with the given name exists
        /// </summary>
        public bool ExistsByName(string name)
        {
            return _repository.ExistsByName(name);
        }
    }
}