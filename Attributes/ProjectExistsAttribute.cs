using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using TaskFlowAPI.Data;

namespace TaskFlowAPI.Attributes
{
    public class ProjectExistsAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if ( value == null)
            {
                return ValidationResult.Success;
            }

            var dbContext = (AppDbContext?)validationContext.GetService(typeof(AppDbContext));

            if (dbContext == null)
            {
                throw new InvalidOperationException("DbContext not found। Please added AddDbContext in Program.cs");
            }

            if (int.TryParse(value.ToString(), out int projectId))
            {
                bool projectExists = dbContext.Projects.Any(p => p.Id == projectId);

                if (projectExists)
                {
                    return ValidationResult.Success;
                }
            }
         
            string errorMessage = ErrorMessage ?? $"No project was found with the provided {validationContext.DisplayName} (ID: {value}).";

            return new ValidationResult(errorMessage);
        }

    }
}
