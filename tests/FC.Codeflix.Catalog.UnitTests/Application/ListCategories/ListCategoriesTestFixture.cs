using FC.Codeflix.Catalog.UnitTests.Common;

namespace FC.Codeflix.Catalog.UnitTests.Application.ListCategories;

public class ListCategoriesTestFixture : BaseFixture
{

}

[CollectionDefinition(nameof(ListCategoriesTestFixture))]
public class ListCategoriesTestFixtureCollection :
    ICollectionFixture<ListCategoriesTestFixture> { }