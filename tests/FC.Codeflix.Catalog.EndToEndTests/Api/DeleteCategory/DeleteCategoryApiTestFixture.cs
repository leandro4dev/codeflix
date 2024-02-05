using FC.Codeflix.Catalog.EndToEndTests.Api.Common;

namespace FC.Codeflix.Catalog.EndToEndTests.Api.DeleteCategory;

public class DeleteCategoryApiTestFixture : CategoryBaseFixture
{

}

[CollectionDefinition(nameof(DeleteCategoryApiTestFixture))]
public class DeleteCategoryApiTestFixtureColletion : ICollectionFixture<DeleteCategoryApiTestFixture> { }