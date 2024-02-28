using FC.Codeflix.Catalog.UnitTests.Common;

namespace FC.Codeflix.Catalog.UnitTests.Domain.Entity.Genre;

public class GenreTestFixture : BaseFixture
{
    public GenreTestFixture() : base() { }



}


[CollectionDefinition(nameof(GenreTestFixture))]
public class GenreTestFixtureColletion : ICollectionFixture<GenreTestFixture> { }