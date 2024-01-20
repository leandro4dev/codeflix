using FC.Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;
using FC.Codeflix.Catalog.Domain.Entity;
using FC.Codeflix.Catalog.UnitTests.Application.Common;

namespace FC.Codeflix.Catalog.UnitTests.Application.UpdateCategory;

public class UpdateCategoryTestFixture : CategoryUseCasesBaseFixture
{
    public Category GetExampleCategory()
    {
        return new Category(
            GetValidCategoryName(),
            GetValidCategoryDescription(),
            GetRandomBoolean()
        );
    }

    public UpdateCategoryInput GetValidInput(Guid? id = null)
    {
        return new UpdateCategoryInput(
            id ?? Guid.NewGuid(),
            GetValidCategoryName(),
            GetValidCategoryDescription(),
            GetRandomBoolean()
        );
    }

    public UpdateCategoryInput GetInvalidInputShortName()
    {
        var invalidInputShortName = GetValidInput();
        invalidInputShortName.Name = invalidInputShortName.Name.Substring(0, 2);

        return invalidInputShortName;
    }

    public UpdateCategoryInput GetInvalidInputTooLongName()
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

    public UpdateCategoryInput GetInvalidInputDescriptionTooLong()
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

[CollectionDefinition(nameof(UpdateCategoryTestFixture))]
public class UpdateCategoryTestFixtureCollection : ICollectionFixture<UpdateCategoryTestFixture> { }
