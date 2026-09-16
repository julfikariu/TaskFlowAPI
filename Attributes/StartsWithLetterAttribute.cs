using System.ComponentModel.DataAnnotations;

namespace TaskFlowAPI.Attributes
{
    public class StartsWithLetterAttribute : ValidationAttribute
    {
        private readonly string _allowedLetter;

        public StartsWithLetterAttribute(string allowedLetter)
        {
            _allowedLetter = allowedLetter;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            // If empty then handle it by [Required] attribute
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return ValidationResult.Success;
            }

            string stringValue = value.ToString()!;

            // Case-insensitive A or a accepted)
            if (stringValue.StartsWith(_allowedLetter, StringComparison.OrdinalIgnoreCase))
            {
                return ValidationResult.Success;
            }

            // Custom error message
            string errorMessage = ErrorMessage ?? $"{validationContext.DisplayName} of course start with '{_allowedLetter}' letter";

            return new ValidationResult(errorMessage);
        }

    }
}
