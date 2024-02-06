using FC.Codeflix.Catalog.Application.UseCases.Category.Common;
using FC.Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace FC.Codeflix.Catalog.EndToEndTests.Api.Category.UpdateCategory;

[Collection(nameof(UpdateCategoryApiTestFixture))]
public class UpdateCategoryApiTest
{
    private readonly UpdateCategoryApiTestFixture _fixture;

    public UpdateCategoryApiTest(UpdateCategoryApiTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(UpdateCategory))]
    [Trait("End2End/Api", "Category/Update - Endpoints")]
    public async Task UpdateCategory()
    {
        var exampleCategory = _fixture.GetExampleCategory();
        await _fixture.Persistence.Insert(exampleCategory);

        var input = _fixture.GetExampleInput(exampleCategory.Id);

        var (response, output) = await _fixture.ApiClient
            .Put<CategoryModelOutput>(
                $"/categories/{exampleCategory.Id}",
                input
            );

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode) StatusCodes.Status200OK);
        output!.Id.Should().Be(exampleCategory.Id);
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().Be((bool) input.IsActive!);

        var dbCategory = await _fixture.Persistence.GetById(exampleCategory.Id);

        dbCategory.Should().NotBeNull();
        dbCategory!.Name.Should().Be(input.Name);
        dbCategory.Description.Should().Be(input.Description);
        dbCategory.IsActive.Should().Be((bool)input.IsActive!);
        dbCategory.Id.Should().Be(exampleCategory.Id);
    }

    [Fact(DisplayName = nameof(UpdateCategoryOnlyName))]
    [Trait("End2End/Api", "Category/Update - Endpoints")]
    public async Task UpdateCategoryOnlyName()
    {
        var exampleCategory = _fixture.GetExampleCategory();
        await _fixture.Persistence.Insert(exampleCategory);

        var input = new UpdateCategoryInput(
            exampleCategory.Id, 
            _fixture.GetValidCategoryName()
        );

        var (response, output) = await _fixture.ApiClient
            .Put<CategoryModelOutput>(
                $"/categories/{exampleCategory.Id}",
                input
            );

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
        output!.Id.Should().Be(exampleCategory.Id);
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(exampleCategory.Description);
        output.IsActive.Should().Be((bool)exampleCategory.IsActive!);

        var dbCategory = await _fixture.Persistence.GetById(exampleCategory.Id);

        dbCategory.Should().NotBeNull();
        dbCategory!.Name.Should().Be(input.Name);
        dbCategory.Description.Should().Be(exampleCategory.Description);
        dbCategory.IsActive.Should().Be((bool)exampleCategory.IsActive!);
        dbCategory.Id.Should().Be(exampleCategory.Id);
    }

    [Fact(DisplayName = nameof(UpdateCategoryNameAndDescription))]
    [Trait("End2End/Api", "Category/Update - Endpoints")]
    public async Task UpdateCategoryNameAndDescription()
    {
        var exampleCategory = _fixture.GetExampleCategory();
        await _fixture.Persistence.Insert(exampleCategory);

        var input = new UpdateCategoryInput(
            exampleCategory.Id,
            _fixture.GetValidCategoryName(),
            _fixture.GetValidCategoryDescription()
        );

        var (response, output) = await _fixture.ApiClient
            .Put<CategoryModelOutput>(
                $"/categories/{exampleCategory.Id}",
                input
            );

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
        output!.Id.Should().Be(exampleCategory.Id);
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().Be((bool)exampleCategory.IsActive!);

        var dbCategory = await _fixture.Persistence.GetById(exampleCategory.Id);

        dbCategory.Should().NotBeNull();
        dbCategory!.Name.Should().Be(input.Name);
        dbCategory.Description.Should().Be(input.Description);
        dbCategory.IsActive.Should().Be((bool)exampleCategory.IsActive!);
        dbCategory.Id.Should().Be(exampleCategory.Id);
    }
}
