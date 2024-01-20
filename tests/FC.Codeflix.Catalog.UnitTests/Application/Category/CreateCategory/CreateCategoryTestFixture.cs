using FC.Codeflix.Catalog.Application.UseCases.Category.CreateCategory;
using FC.Codeflix.Catalog.UnitTests.Application.Category.Common;

namespace FC.Codeflix.Catalog.UnitTests.Application.Category.CreateCategory
{
    public class CreateCategoryTestFixture : CategoryUseCasesBaseFixture
    {
        public CreateCategoryTestFixture() : base() { }

        public CreateCategoryInput GetValidInput()
        {
            return new(
                GetValidCategoryName(),
                GetValidCategoryDescription(),
                GetRandomBoolean()
            );
        }

        public CreateCategoryInput GetInvalidInputShortName()
        {
            var invalidInputShortName = GetValidInput();
            invalidInputShortName.Name = invalidInputShortName.Name.Substring(0, 2);

            return invalidInputShortName;
        }

        public CreateCategoryInput GetInvalidInputTooLongName()
        {
            var invalidInputTooLongName = GetValidInput();
            var tooLongNameForCategory = "";

            while (tooLongNameForCategory.Length < 255)
            {
                tooLongNameForCategory = $"{tooLongNameForCategory} {Faker.Commerce.ProductName}";
            }

            invalidInputTooLongName.Name = tooLongNameForCategory;

            return invalidInputTooLongName;
        }

        public CreateCategoryInput GetInvalidInputDescriptionNull()
        {
            var invalidInputDescriptionNull = GetValidInput();
            invalidInputDescriptionNull.Description = null!;

            return invalidInputDescriptionNull;
        }

        public CreateCategoryInput GetInvalidInputDescriptionTooLong()
        {
            var invalidInputTooLongDescription = GetValidInput();
            var tooLongDescription = Faker.Commerce.ProductName();

            while (tooLongDescription.Length <= 10000)
            {
                tooLongDescription = $"{tooLongDescription} {Faker.Commerce.ProductDescription()}";
            }

            invalidInputTooLongDescription.Description = tooLongDescription;

            return invalidInputTooLongDescription;
        }
    }

    [CollectionDefinition(nameof(CreateCategoryTestFixture))]
    public class CreateCategoryTestFixtureCollection : ICollectionFixture<CreateCategoryTestFixture> { }
}