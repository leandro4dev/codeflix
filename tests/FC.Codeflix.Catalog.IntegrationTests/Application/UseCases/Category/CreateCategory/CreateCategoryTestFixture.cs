using FC.Codeflix.Catalog.IntegrationTests.Application.UseCases.Category.Common;

namespace FC.Codeflix.Catalog.IntegrationTests.Application.UseCases.Category.CreateCategory;

public class CreateCategoryTestFixture : CategoryUseCasesBaseFixture
{

}

[CollectionDefinition(nameof(CreateCategoryTestFixture))]
public class CreateCategoryTestFixtureColletion : ICollectionFixture<CreateCategoryTestFixture> { }
