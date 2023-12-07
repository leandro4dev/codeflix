using Bogus;
using FC.Codeflix.Catalog.Domain.Exceptions;
using FC.Codeflix.Catalog.Domain.Validation;
using FluentAssertions;

namespace FC.Codeflix.Catalog.UnitTests.Domain.Validation
{
    public class DomainValidationTest
    {
        private Faker Faker { get; set; } = new Faker();

        //dont be null
        [Fact(DisplayName = nameof(NotNullOk))]
        [Trait("Domain", "DomainValidation - Validation")]
        public void NotNullOk()
        {
            var value = Faker.Commerce.ProductName();
            Action action = () => DomainValidation.NotNull(value, "Value");

            action.Should().NotThrow();
        }

        [Fact(DisplayName = nameof(NotNullThrowWhenNull))]
        [Trait("Domain", "DomainValidation - Validation")]
        public void NotNullThrowWhenNull()
        {
            string? value = null;
            Action action = () => DomainValidation.NotNull(value, "FieldName");

            action.Should().Throw<EntityValidationException>().WithMessage("FieldName should not be null");
        }

        //dont be null or empty
        [Fact(DisplayName = nameof(NotNullOrEmptyOk))]
        [Trait("Domain", "DomainValidation - Validation")]
        public void NotNullOrEmptyOk()
        {
            var target = Faker.Commerce.ProductName();

            Action action = () => DomainValidation.NotNullOrEmpty(target, "FieldName");
            action.Should().NotThrow();
        }

        [Theory(DisplayName = nameof(NotNullOrEmptyThrowWhenNullOrEmpty))]
        [Trait("Domain", "DomainValidation - Validation")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public void NotNullOrEmptyThrowWhenNullOrEmpty(string value)
        {
            Action action = () => DomainValidation.NotNullOrEmpty(value, "FieldName");
            action.Should().Throw<EntityValidationException>()
                .WithMessage("FieldName should not be empty or null");
        }

        // max length
        [Theory(DisplayName = nameof(MaxLengthOk))]
        [Trait("Domain", "DomainValidation - Validation")]
        [MemberData(nameof(GetValueSmallerThanMax), parameters: 5)]
        public void MaxLengthOk(string target, int maxLength)
        {
            Action action = () => DomainValidation.MaxLength(target, maxLength, "FieldName");
            action.Should().NotThrow();
        }

        public static IEnumerable<object[]> GetValueSmallerThanMax(int numberOfTests)
        {
            yield return new object[] { "123456", 7 };

            var faker = new Faker();

            for (int i = 0; i < (numberOfTests - 1); i++)
            {
                var example = faker.Commerce.ProductName();
                var maxLength = example.Length + (new Random()).Next(1, 5);

                yield return new object[]{
                    example,
                    maxLength
                };
            }
        }

        [Theory(DisplayName = nameof(MaxLengthThrowWhenGreater))]
        [Trait("Domain", "DomainValidation - Validation")]
        [MemberData(nameof(GetValuesGreaterThenMax), parameters: 5)]
        public void MaxLengthThrowWhenGreater(string target, int maxLength)
        {
            Action action = () => DomainValidation.MaxLength(target, maxLength, "FieldName");
            action.Should().Throw<EntityValidationException>()
                .WithMessage($"FieldName should not be greater than {maxLength} characters long");
        }

        public static IEnumerable<object[]> GetValuesGreaterThenMax(int numberOfTests)
        {
            yield return new object[] { "123456", 5 };

            var faker = new Faker();

            for (int i = 0; i < (numberOfTests - 1); i++)
            {
                var example = faker.Commerce.ProductName();
                var maxLength = example.Length - (new Random()).Next(1, 5);

                yield return new object[]{
                    example,
                    maxLength
                };
            }
        }

        // min length 
        [Theory(DisplayName = nameof(MinLengthOk))]
        [Trait("Domain", "DomainValidation - Validation")]
        [MemberData(nameof(GetValuesGreaterThanMin), parameters: 5)]
        public void MinLengthOk(string target, int minLength)
        {
            Action action = () =>
                DomainValidation.MinLength(target, minLength, "FieldName");
            action.Should().NotThrow();
        }

        public static IEnumerable<object[]> GetValuesGreaterThanMin(int numberOfTests)
        {
            yield return new object[] { "123456", 6 };

            var faker = new Faker();

            for (int i = 0; i < (numberOfTests - 1); i++)
            {
                var example = faker.Commerce.ProductName();
                var minLength = example.Length - (new Random()).Next(1, 5);

                yield return new object[] {
                    example, minLength
                };
            }
        }

        [Theory(DisplayName = nameof(MinLengthThrowWhenLess))]
        [Trait("Domain", "DomainValidation - Validation")]
        [MemberData(nameof(GetValuesSmallerThanMin), parameters: 5)]
        public void MinLengthThrowWhenLess(string target, int minLength)
        {
            Action action = () =>
                DomainValidation.MinLength(target, minLength, "FieldName");
            action.Should().Throw<EntityValidationException>().WithMessage($"FieldName should not be less than {minLength} characters long");
        }

        public static IEnumerable<object[]> GetValuesSmallerThanMin(int numberOfTests)
        {
            yield return new object[] { "123456", 10 };

            var faker = new Faker();

            for (int i = 0; i < (numberOfTests - 1); i++)
            {
                var example = faker.Commerce.ProductName();
                var minLength = example.Length + (new Random()).Next(1, 20);

                yield return new object[] {
                    example, minLength
                };
            }
        }
    }
}