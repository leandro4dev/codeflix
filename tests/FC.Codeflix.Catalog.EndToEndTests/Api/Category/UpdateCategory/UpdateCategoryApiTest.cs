using FC.Codeflix.Catalog.Application.UseCases.Category.Common;
using FC.Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FC.Codeflix.Catalog.EndToEndTests.Api.Category.UpdateCategory;

[Collection(nameof(UpdateCategoryApiTestFixture))]
public class UpdateCategoryApiTest : IDisposable
{
    private readonly UpdateCategoryApiTestFixture _fixture;

    public UpdateCategoryApiTest(UpdateCategoryApiTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(UpdateCategory))]
    [Trait("End2End/Api", "Category/Update - Endpoints")]
    public async void UpdateCategory()
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
    public async void UpdateCategoryOnlyName()
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
    public async void UpdateCategoryNameAndDescription()
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

    [Fact(DisplayName = nameof(ErrorWhenNotFound))]
    [Trait("End2End/Api", "Category/Update - Endpoints")]
    public async void ErrorWhenNotFound()
    {
        var examplecategoryList = _fixture.GetExampleCategoriesList();
        await _fixture.Persistence.InsertList(examplecategoryList);

        var randomGuid = Guid.NewGuid();

        var input = _fixture.GetExampleInput(randomGuid);

        var (response, output) = await _fixture.ApiClient.Put<ProblemDetails>(
            $"/categories/{randomGuid}",
            input
        );

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode) StatusCodes.Status404NotFound);
        output.Should().NotBeNull();
        output!.Status.Should().Be(StatusCodes.Status404NotFound);
        output.Title.Should().Be("Not found");
        output.Type.Should().Be("NotFound");
        output.Detail.Should().Be($"Category '{randomGuid}' not found");
    }

    [Theory(DisplayName = nameof(ErrorWhenCantInstantiateAggregate))]
    [Trait("End2End/Api", "Category/Update - Endpoints")]
    [MemberData(
        nameof(UpdateCategoryApiTestDataGenerator.GetInvalidInputs),
        MemberType = typeof(UpdateCategoryApiTestDataGenerator)
    )]
    public async void ErrorWhenCantInstantiateAggregate(
        UpdateCategoryInput input, 
        string expectedDetail
    )
    {
        var exampleCategoryList = _fixture.GetExampleCategoriesList(10);
        await _fixture.Persistence.InsertList(exampleCategoryList);

        var exampleCategory = exampleCategoryList[5];
        input.Id = exampleCategory.Id;

        var (response, output) = await _fixture.ApiClient.Put<ProblemDetails>(
            $"/categories/{input.Id}",
            input
        );

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode) StatusCodes.Status422UnprocessableEntity);
        output.Should().NotBeNull();
        output!.Title.Should().Be("One or more validation errors ocurred");
        output.Type.Should().Be("UnprocessableEntity");
        output.Status.Should().Be(StatusCodes.Status422UnprocessableEntity);
        output.Detail.Should().Be(expectedDetail);
    }

    public void Dispose()
    {
        _fixture.CleanPersistence();
    }
}
