using FC.Codeflix.Catalog.Domain.Exceptions;

namespace FC.Codeflix.Catalog.Domain.Validation
{
    public class DomainValidation
    {
        public static void NotNull(object? target, string fieldName)
        {
            if (target == null)
            {
                throw new EntityValidationException(
                    $"{fieldName} should not be null"
                );
            }
        }

        public static void NotNullOrEmpty(string? target, string fieldName)
        {
            if (String.IsNullOrWhiteSpace(target))
            {
                throw new EntityValidationException(
                    $"{fieldName} should not be empty or null"
                );
            }
        }

        public static void MinLength(string target, int minLength, string fieldName)
        {
            if (target.Length < minLength)
            {
                throw new EntityValidationException(
                    $"{fieldName} should not be than {minLength} characters long"
                );
            }
        }
    }
}