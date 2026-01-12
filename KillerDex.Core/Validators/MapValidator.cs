using KillerDex.Core.Interfaces;
using KillerDex.Core.Models;
using KillerDex.Core.Resources;

namespace KillerDex.Core.Validators
{
    public class MapValidator
    {
        private readonly IMapRepository _repository;

        public MapValidator(IMapRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Validates a map for creation
        /// </summary>
        public ValidationResult ValidateForCreate(Map map)
        {
            var result = new ValidationResult();

            ValidateName(map.Name, result);

            if (result.IsValid && _repository.ExistsByName(map.Name.Trim()))
            {
                result.AddError(ValidationMessages.Map_NameDuplicate);
            }

            return result;
        }

        /// <summary>
        /// Validates a map for update
        /// </summary>
        public ValidationResult ValidateForUpdate(Map map)
        {
            var result = new ValidationResult();

            ValidateName(map.Name, result);

            if (result.IsValid && _repository.ExistsByNameExcludingId(map.Name.Trim(), map.Id))
            {
                result.AddError(ValidationMessages.Map_NameDuplicate);
            }

            return result;
        }

        private void ValidateName(string name, ValidationResult result)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                result.AddError(ValidationMessages.Map_NameRequired);
            }
        }
    }
}