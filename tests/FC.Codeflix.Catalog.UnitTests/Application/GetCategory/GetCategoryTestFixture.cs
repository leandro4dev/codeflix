using FC.Codeflix.Catalog.Domain.Entity;
using FC.Codeflix.Catalog.Domain.Repository;
using FC.Codeflix.Catalog.UnitTests.Common;
using Moq;

namespace FC.Codeflix.Catalog.UnitTests.Application.GetCategory;

public class GetCategoryTestFixture : BaseFixture
{
    public string GetValidCategoryName()
    {
        var categoryName = "";

        while (categoryName.Length < 3)
        {
            categoryName = Faker.Commerce.Categories(1)[0];
        }

        if (categoryName.Length > 255)
        {
            categoryName = categoryName[..255]; //primeiros 255 characters
        }

        return categoryName;
    }

    public string GetValidCategoryDescription()
    {
        var categoryDescription = Faker.Commerce.ProductDescription();
        if (categoryDescription.Length > 10000)
        {
            categoryDescription = categoryDescription[..10_000];
        }

        return categoryDescription;
    }

    public Category GetValidCategory()
    {
        return new Category(
            GetValidCategoryName(),
            GetValidCategoryDescription()
        );
    }

    public Mock<ICategoryRepository> GetRepositoryMock()
    {
        return new Mock<ICategoryRepository>();
    }
}

[CollectionDefinition(nameof(GetCategoryTestFixture))]
public class GetCategoryTestFixtureCollection : ICollectionFixture<GetCategoryTestFixture>{ }