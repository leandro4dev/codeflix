﻿using FC.Codeflix.Catalog.Api.ApiModels.Category;
using FC.Codeflix.Catalog.Api.ApiModels.Response;
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
    public async Task UpdateCategory()
    {
        var exampleCategory = _fixture.GetExampleCategory();
        await _fixture.Persistence.Insert(exampleCategory);

        var input = _fixture.GetExampleInput();

        var (response, output) = await _fixture.ApiClient
            .Put<ApiResponse<CategoryModelOutput>>(
                $"/categories/{exampleCategory.Id}",
                input
            );

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
        output!.Data.Should().NotBeNull();
        output.Data.Id.Should().Be(exampleCategory.Id);
        output.Data.Name.Should().Be(input.Name);
        output.Data.Description.Should().Be(input.Description);
        output.Data.IsActive.Should().Be((bool)input.IsActive!);

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
            .Put<ApiResponse<CategoryModelOutput>>(
                $"/categories/{exampleCategory.Id}",
                input
            );

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
        output!.Data.Id.Should().Be(exampleCategory.Id);
        output.Data.Name.Should().Be(input.Name);
        output.Data.Description.Should().Be(exampleCategory.Description);
        output.Data.IsActive.Should().Be((bool)exampleCategory.IsActive!);

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
            .Put<ApiResponse<CategoryModelOutput>>(
                $"/categories/{exampleCategory.Id}",
                input
            );

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
        output!.Data.Should().NotBeNull();
        output.Data.Id.Should().Be(exampleCategory.Id);
        output.Data.Name.Should().Be(input.Name);
        output.Data.Description.Should().Be(input.Description);
        output.Data.IsActive.Should().Be((bool)exampleCategory.IsActive!);

        var dbCategory = await _fixture.Persistence.GetById(exampleCategory.Id);

        dbCategory.Should().NotBeNull();
        dbCategory!.Name.Should().Be(input.Name);
        dbCategory.Description.Should().Be(input.Description);
        dbCategory.IsActive.Should().Be((bool)exampleCategory.IsActive!);
        dbCategory.Id.Should().Be(exampleCategory.Id);
    }

    [Fact(DisplayName = nameof(ErrorWhenNotFound))]
    [Trait("End2End/Api", "Category/Update - Endpoints")]
    public async Task ErrorWhenNotFound()
    {
        var examplecategoryList = _fixture.GetExampleCategoriesList();
        await _fixture.Persistence.InsertList(examplecategoryList);

        var randomGuid = Guid.NewGuid();

        var input = _fixture.GetExampleInput();

        var (response, output) = await _fixture.ApiClient.Put<ProblemDetails>(
            $"/categories/{randomGuid}",
            input
        );

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status404NotFound);
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
    public async Task ErrorWhenCantInstantiateAggregate(
        UpdateCategoryApiInput input,
        string expectedDetail
    )
    {
        var exampleCategoryList = _fixture.GetExampleCategoriesList(10);
        await _fixture.Persistence.InsertList(exampleCategoryList);

        var exampleCategory = exampleCategoryList[5];

        var (response, output) = await _fixture.ApiClient.Put<ProblemDetails>(
            $"/categories/{exampleCategory.Id}",
            input
        );

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status422UnprocessableEntity);
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
