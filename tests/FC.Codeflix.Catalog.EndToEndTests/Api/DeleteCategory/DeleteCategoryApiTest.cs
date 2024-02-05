using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FC.Codeflix.Catalog.EndToEndTests.Api.DeleteCategory;

[Collection(nameof(DeleteCategoryApiTestFixture))]
public class DeleteCategoryApiTest
{
    private readonly DeleteCategoryApiTestFixture _fixture;

    public DeleteCategoryApiTest(DeleteCategoryApiTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(DeleteCategory))]
    [Trait("End2End/Api", "Category/Delete - Endpoints")]
    public async Task DeleteCategory()
    {
        var exampleCategory = _fixture.GetExampleCategory();
        await _fixture.Persistence.Insert(exampleCategory);

        var (response, output) = await _fixture.ApiClient.Delete<object>(
            $"/categories/{exampleCategory.Id}"
        );

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status204NoContent);
        output.Should().BeNull();

        var persistenceCategory = await _fixture.Persistence.GetById(exampleCategory.Id);
        persistenceCategory.Should().BeNull();
    }

    [Fact(DisplayName = nameof(ErrorWhenNotFound))]
    [Trait("End2End/Api", "Category/Delete - Endpoints")]
    public async Task ErrorWhenNotFound()
    {
        var exampleCategoryList = _fixture.GetExampleCategoriesList();
        await _fixture.Persistence.InsertList(exampleCategoryList);

        var randomGuid = Guid.NewGuid();

        var (response, output) = await _fixture.ApiClient.Delete<ProblemDetails>(
            $"/categories/{randomGuid}"
        );         

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode) StatusCodes.Status404NotFound);
        output!.Status.Should().Be((int)StatusCodes.Status404NotFound);
        output.Title.Should().Be("Not found");
        output.Type.Should().Be("NotFound");
        output.Detail.Should().Be($"Category '{randomGuid}' not found");
    }
}
