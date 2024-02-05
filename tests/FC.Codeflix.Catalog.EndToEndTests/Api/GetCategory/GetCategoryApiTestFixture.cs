using FC.Codeflix.Catalog.EndToEndTests.Api.Common;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entity;

namespace FC.Codeflix.Catalog.EndToEndTests.Api.GetCategory;

public class GetCategoryApiTestFixture : CategoryBaseFixture
{
    
}

[CollectionDefinition(nameof(GetCategoryApiTestFixture))]
public class GetCategoryApiTestFixtureColletion : ICollectionFixture<GetCategoryApiTestFixture> { }