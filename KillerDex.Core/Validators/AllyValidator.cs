using KillerDex.Core.Interfaces;
using KillerDex.Core.Models;
using KillerDex.Core.Resources;

namespace KillerDex.Core.Validators
{
    public class AllyValidator
    {
        private readonly IAllyRepository _repository;

        public AllyValidator(IAllyRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Validates an ally for creation
        /// </summary>
        public ValidationResult ValidateForCreate(Ally ally)
        {
            var result = new ValidationResult();

            ValidateName(ally.Name, result);

            if (result.IsValid && _repository.ExistsByName(ally.Name.Trim()))
            {
                result.AddError(ValidationMessages.Ally_NameDuplicate);
            }

            return result;
        }

        /// <summary>
        /// Validates an ally for update
        /// </summary>
        public ValidationResult ValidateForUpdate(Ally ally)
        {
            var result = new ValidationResult();

            ValidateName(ally.Name, result);

            if (result.IsValid && _repository.ExistsByNameExcludingId(ally.Name.Trim(), ally.Id))
            {
                result.AddError(ValidationMessages.Ally_NameDuplicate);
            }

            return result;
        }

        private void ValidateName(string name, ValidationResult result)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                result.AddError(ValidationMessages.Ally_NameRequired);
            }
        }
    }
}