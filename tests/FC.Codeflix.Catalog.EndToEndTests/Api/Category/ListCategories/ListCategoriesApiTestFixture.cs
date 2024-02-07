using FC.Codeflix.Catalog.EndToEndTests.Api.Category.Common;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entity;

namespace FC.Codeflix.Catalog.EndToEndTests.Api.Category.ListCategories;

public class ListCategoriesApiTestFixture : CategoryBaseFixture
{
    public List<DomainEntity.Category> GetExampleCategoriesListWithName(List<string> name)
    {
        return name.Select(name =>
        {
            var category = GetExampleCategory();
            category.Update(name, category.Description);
            return category;
        }).ToList();
    }
}

[CollectionDefinition(nameof(ListCategoriesApiTestFixture))]
public class ListCategoriesApiTestFixtureColletion : ICollectionFixture<ListCategoriesApiTestFixture> { }