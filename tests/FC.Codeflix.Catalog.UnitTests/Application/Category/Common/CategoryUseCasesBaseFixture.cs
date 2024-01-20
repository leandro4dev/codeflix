using FC.Codeflix.Catalog.Application.Interfaces;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entity;
using FC.Codeflix.Catalog.Domain.Repository;
using FC.Codeflix.Catalog.UnitTests.Common;
using Moq;

namespace FC.Codeflix.Catalog.UnitTests.Application.Category.Common;

public abstract class CategoryUseCasesBaseFixture : BaseFixture
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

    public bool GetRandomBoolean()
    {
        return new Random().NextDouble() < 0.5;
    }

    public DomainEntity.Category GetValidCategory()
    {
        return new DomainEntity.Category(
            GetValidCategoryName(),
            GetValidCategoryDescription(),
            GetRandomBoolean()
        );
    }

    public Mock<ICategoryRepository> GetRepositoryMock()
    {
        return new Mock<ICategoryRepository>();
    }

    public Mock<IUnitOfWork> GetUnitOfWorkMock()
    {
        return new Mock<IUnitOfWork>();
    }
}
