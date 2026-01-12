using KillerDex.Core.Interfaces;
using KillerDex.Core.Models;
using KillerDex.Core.Resources;

namespace KillerDex.Core.Validators
{
    public class KillerValidator
    {
        private readonly IKillerRepository _repository;

        public KillerValidator(IKillerRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Validates a killer for creation
        /// </summary>
        public ValidationResult ValidateForCreate(Killer killer)
        {
            var result = new ValidationResult();

            ValidateAlias(killer.Alias, result);

            if (result.IsValid && _repository.ExistsByAlias(killer.Alias.Trim()))
            {
                result.AddError(ValidationMessages.Killer_AliasDuplicate);
            }

            return result;
        }

        /// <summary>
        /// Validates a killer for update
        /// </summary>
        public ValidationResult ValidateForUpdate(Killer killer)
        {
            var result = new ValidationResult();

            ValidateAlias(killer.Alias, result);

            if (result.IsValid && _repository.ExistsByAliasExcludingId(killer.Alias.Trim(), killer.Id))
            {
                result.AddError(ValidationMessages.Killer_AliasDuplicate);
            }

            return result;
        }

        private void ValidateAlias(string alias, ValidationResult result)
        {
            if (string.IsNullOrWhiteSpace(alias))
            {
                result.AddError(ValidationMessages.Killer_AliasRequired);
            }
        }
    }
}