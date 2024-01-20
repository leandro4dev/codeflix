using FC.Codeflix.Catalog.Domain.Entity;
using FC.Codeflix.Catalog.Domain.Repository;
using FC.Codeflix.Catalog.UnitTests.Application.Common;


namespace FC.Codeflix.Catalog.UnitTests.Application.GetCategory;

public class GetCategoryTestFixture : CategoryUseCasesBaseFixture
{

}

[CollectionDefinition(nameof(GetCategoryTestFixture))]
public class GetCategoryTestFixtureCollection : ICollectionFixture<GetCategoryTestFixture>{ }