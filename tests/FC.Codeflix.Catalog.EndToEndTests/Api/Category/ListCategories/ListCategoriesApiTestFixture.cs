using FC.Codeflix.Catalog.EndToEndTests.Api.Category.Common;

namespace FC.Codeflix.Catalog.EndToEndTests.Api.Category.ListCategories;

public class ListCategoriesApiTestFixture : CategoryBaseFixture
{

}

[CollectionDefinition(nameof(ListCategoriesApiTestFixture))]
public class ListCategoriesApiTestFixtureColletion : ICollectionFixture<ListCategoriesApiTestFixture> { }