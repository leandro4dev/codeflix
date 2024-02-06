using FC.Codeflix.Catalog.EndToEndTests.Common;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entity;

namespace FC.Codeflix.Catalog.EndToEndTests.Api.Category.Common;

public class CategoryBaseFixture : BaseFixture
{
    public CategoryPersistence Persistence;

    public CategoryBaseFixture() : base()
    {
        Persistence = new CategoryPersistence(CreateDbContext());
    }

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

    public string GetInvalidShortName()
    {
        var tooShortName = Faker.Commerce.ProductName();
        var invalidShortName = tooShortName.Substring(0, 2);
        return invalidShortName;
    }

    public string GetInvalidNameTooLong()
    {
        var tooLongNameForCategory = "";

        while (tooLongNameForCategory.Length < 255)
        {
            tooLongNameForCategory = $"{tooLongNameForCategory} {Faker.Commerce.ProductName}";
        }

        return tooLongNameForCategory;
    }

    public string GetInvalidInputDescriptionTooLong()
    {
        var tooLongDescription = Faker.Commerce.ProductName();

        while (tooLongDescription.Length <= 10000)
        {
            tooLongDescription = $"{tooLongDescription} {Faker.Commerce.ProductDescription()}";
        }

        return tooLongDescription;
    }

    public DomainEntity.Category GetExampleCategory()
    {
        return new DomainEntity.Category(
            GetValidCategoryName(),
            GetValidCategoryDescription(),
            GetRandomBoolean()
        );
    }

    public List<DomainEntity.Category> GetExampleCategoriesList(int length = 10)
    {
        return Enumerable.Range(1, length)
            .Select(_ => GetExampleCategory()).ToList();
    }
}
