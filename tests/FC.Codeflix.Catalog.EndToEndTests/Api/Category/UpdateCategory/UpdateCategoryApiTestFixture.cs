using FC.Codeflix.Catalog.Api.ApiModels.Category;
using FC.Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;
using FC.Codeflix.Catalog.EndToEndTests.Api.Category.Common;

namespace FC.Codeflix.Catalog.EndToEndTests.Api.Category.UpdateCategory;

public class UpdateCategoryApiTestFixture : CategoryBaseFixture
{
    public UpdateCategoryApiInput GetExampleInput()
    {
        return new UpdateCategoryApiInput(
            GetValidCategoryName(), 
            GetValidCategoryDescription(), 
            GetRandomBoolean()
        );
    }
}

[CollectionDefinition(nameof(UpdateCategoryApiTestFixture))]
public class UpdateCategoryApiTestFixtureColletion : ICollectionFixture<UpdateCategoryApiTestFixture> { }