using FC.Codeflix.Catalog.Application.UseCases.Category.CreateCategory;
using FC.Codeflix.Catalog.EndToEndTests.Api.Common;

namespace FC.Codeflix.Catalog.EndToEndTests.Api.Category;

public class CreateCategoryApiTestFixture : CategoryBaseFixture
{
    public CreateCategoryInput GetExampleInput()
    {
        return new CreateCategoryInput(
            GetValidCategoryName(),
            GetValidCategoryDescription(),
            GetRandomBoolean()
        );
    }
}

[CollectionDefinition(nameof(CreateCategoryApiTestFixtureCollection))]
public class CreateCategoryApiTestFixtureCollection 
    : ICollectionFixture<CreateCategoryApiTestFixture> { }