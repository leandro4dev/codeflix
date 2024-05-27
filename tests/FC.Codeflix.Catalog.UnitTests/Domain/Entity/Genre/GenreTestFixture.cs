using FC.Codeflix.Catalog.UnitTests.Common;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entity;

namespace FC.Codeflix.Catalog.UnitTests.Domain.Entity.Genre;

public class GenreTestFixture : BaseFixture
{
    public GenreTestFixture() : base() { }

    public string GetValidName()
    {
        return Faker.Commerce.Categories(1)[0];
    }

    public DomainEntity.Genre GetExampleGenre(bool isActive = true)
    {
        return new DomainEntity.Genre(GetValidName(), isActive);
    }
}


[CollectionDefinition(nameof(GenreTestFixture))]
public class GenreTestFixtureColletion : ICollectionFixture<GenreTestFixture> { }