using FC.Codeflix.Catalog.IntegrationTests.Application.UseCases.Category.Common;

namespace FC.Codeflix.Catalog.IntegrationTests.Application.UseCases.Category.DeleteCategory;

public class DeleteCategoryTestFixture : CategoryUseCasesBaseFixture
{

}

[CollectionDefinition(nameof(DeleteCategoryTestFixture))]
public class DeleteCategoryTestFixtureColletion : ICollectionFixture<DeleteCategoryTestFixture> { } 
