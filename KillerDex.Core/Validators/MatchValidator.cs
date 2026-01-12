using KillerDex.Core.Models;
using KillerDex.Core.Resources;
using System;

namespace KillerDex.Core.Validators
{
    public class MatchValidator
    {
        /// <summary>
        /// Validates a match for creation
        /// </summary>
        public ValidationResult ValidateForCreate(Match match)
        {
            var result = new ValidationResult();

            ValidateMatch(match, result);

            return result;
        }

        /// <summary>
        /// Validates a match for update
        /// </summary>
        public ValidationResult ValidateForUpdate(Match match)
        {
            var result = new ValidationResult();

            ValidateMatch(match, result);

            return result;
        }

        private void ValidateMatch(Match match, ValidationResult result)
        {
            // Map is required
            if (match.MapId == Guid.Empty)
            {
                result.AddError(ValidationMessages.Match_MapRequired);
            }

            // Killer is required
            if (match.KillerId == Guid.Empty)
            {
                result.AddError(ValidationMessages.Match_KillerRequired);
            }

            // First hook is required
            if (string.IsNullOrWhiteSpace(match.FirstHook))
            {
                result.AddError(ValidationMessages.Match_FirstHookRequired);
            }

            // Generators must be between 0 and 5
            if (match.GeneratorsCompleted < 0 || match.GeneratorsCompleted > 5)
            {
                result.AddError(ValidationMessages.Match_GeneratorsInvalid);
            }

            // Survivors list cannot exceed 4 names
            if (match.Survivors != null && match.Survivors.Count > 4)
            {
                result.AddError(ValidationMessages.Match_SurvivorsMaxExceeded);
            }

            // Max 3 allies
            if (match.AllyIds != null && match.AllyIds.Count > 3)
            {
                result.AddError(ValidationMessages.Match_AlliesMaxExceeded);
            }
        }
    }
}