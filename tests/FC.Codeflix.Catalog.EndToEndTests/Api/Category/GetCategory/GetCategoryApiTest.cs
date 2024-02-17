using FC.Codeflix.Catalog.Application.UseCases.Category.Common;
using FC.Codeflix.Catalog.EndToEndTests.Extensions.DateTime;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FC.Codeflix.Catalog.EndToEndTests.Api.Category.GetCategory;

class GetCategoryResponse
{
    public GetCategoryResponse(CategoryModelOutput data)
    {
        Data = data;
    }

    public CategoryModelOutput Data { get; set; }
}


[Collection(nameof(GetCategoryApiTestFixture))]
public class GetCategoryApiTest : IDisposable
{
    private readonly GetCategoryApiTestFixture _fixture;

    public GetCategoryApiTest(GetCategoryApiTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(GetCategory))]
    [Trait("End2End/Api", "Category/Get - Endpoints")]
    public async Task GetCategory()
    {
        var exampleCategoryList = _fixture.GetExampleCategoriesList(20);
        await _fixture.Persistence.InsertList(exampleCategoryList);
        var exampleCategory = exampleCategoryList[10];

        var (response, output) = await _fixture.ApiClient
            .Get<GetCategoryResponse>(
                $"/categories/{exampleCategory.Id}"
            );

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
        output.Should().NotBeNull();
        output!.Data.Should().NotBeNull();
        output.Data.Id.Should().NotBeEmpty();
        output.Data.Name.Should().Be(exampleCategory.Name);
        output.Data.Description.Should().Be(exampleCategory.Description);
        output.Data.IsActive.Should().Be(exampleCategory.IsActive);
        output.Data.CreatedAt.TrimMillisSeconds().Should().Be(
            output.Data.CreatedAt.TrimMillisSeconds()
        );
    }

    [Fact(DisplayName = nameof(ErrorWhenNotFound))]
    [Trait("End2End/Api", "Category/Get - Endpoints")]
    public async Task ErrorWhenNotFound()
    {
        var exampleCategoryList = _fixture.GetExampleCategoriesList(20);
        await _fixture.Persistence.InsertList(exampleCategoryList);
        var randomGuid = Guid.NewGuid();

        var (response, output) = await _fixture.ApiClient
            .Get<ProblemDetails>(
                $"/categories/{randomGuid}"
            );

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status404NotFound);
        output.Should().NotBeNull();
        output!.Status.Should().Be(StatusCodes.Status404NotFound);
        output.Title.Should().Be("Not found");
        output.Type.Should().Be("NotFound");
        output.Detail.Should().Be($"Category '{randomGuid}' not found");
    }

    public void Dispose()
    {
        _fixture.CleanPersistence();
    }
}
