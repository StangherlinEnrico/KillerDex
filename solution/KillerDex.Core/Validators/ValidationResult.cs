using System.Collections.Generic;

namespace KillerDex.Core.Validators
{
    public class ValidationResult
    {
        public bool IsValid => Errors.Count == 0;
        public List<string> Errors { get; }

        public ValidationResult()
        {
            Errors = new List<string>();
        }

        public void AddError(string error)
        {
            Errors.Add(error);
        }

        /// <summary>
        /// Returns all errors as a single string separated by newlines
        /// </summary>
        public string GetErrorsAsString()
        {
            return string.Join("\n", Errors);
        }

        /// <summary>
        /// Returns all errors as a single string with custom separator
        /// </summary>
        public string GetErrorsAsString(string separator)
        {
            return string.Join(separator, Errors);
        }
    }
}