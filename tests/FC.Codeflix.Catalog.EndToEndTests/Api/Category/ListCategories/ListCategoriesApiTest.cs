using FC.Codeflix.Catalog.Api.ApiModels.Response;
using FC.Codeflix.Catalog.Application.UseCases.Category.Common;
using FC.Codeflix.Catalog.Application.UseCases.Category.ListCategories;
using FC.Codeflix.Catalog.Domain.SeedWork.SearchableRepository;
using FC.Codeflix.Catalog.EndToEndTests.Extensions.DateTime;
using FC.Codeflix.Catalog.EndToEndTests.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net;
using Xunit.Abstractions;

namespace FC.Codeflix.Catalog.EndToEndTests.Api.Category.ListCategories;


[Collection(nameof(ListCategoriesApiTestFixture))]
public class ListCategoriesApiTest : IDisposable
{
    private readonly ListCategoriesApiTestFixture _fixture;
    private readonly ITestOutputHelper _output;

    public ListCategoriesApiTest(ListCategoriesApiTestFixture fixture, ITestOutputHelper output)
    {
        _fixture = fixture;
        _output = output;
    }

    [Fact(DisplayName = nameof(ListCategoriesAndTotalByDefault))]
    [Trait("End2End/Api", "Category/List - Endpoints")]
    public async Task ListCategoriesAndTotalByDefault()
    {
        var defaultPage = 1;
        var defaultPerPage = 15;
        var exampleCategoryList = _fixture.GetExampleCategoriesList(20);
        await _fixture.Persistence.InsertList(exampleCategoryList);

        var (response, output) = await _fixture.ApiClient.Get<TestApiResponseList<CategoryModelOutput>>(
            "/categories"
        );

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
        output.Should().NotBeNull();
        output!.Data.Should().NotBeNull();
        output.Meta.Should().NotBeNull();
        output.Meta!.Total.Should().Be(exampleCategoryList.Count);
        output.Meta.PerPage.Should().Be(defaultPerPage);
        output.Meta.CurrentPage.Should().Be(defaultPage);

        foreach (CategoryModelOutput outputItem in output.Data)
        {
            var exampleItem = exampleCategoryList.FirstOrDefault(x => x.Id == outputItem.Id);

            exampleItem.Should().NotBeNull();

            outputItem.Id.Should().Be(exampleItem!.Id);
            outputItem.Name.Should().Be(exampleItem.Name);
            outputItem.Description.Should().Be(exampleItem.Description);
            outputItem.IsActive.Should().Be(exampleItem.IsActive);
            outputItem.CreatedAt.TrimMillisSeconds().Should().Be(
                exampleItem.CreatedAt.TrimMillisSeconds()
            );
        }
    }

    [Fact(DisplayName = nameof(ItemsEmptyWhenPersistenceEmpty))]
    [Trait("End2End/Api", "Category/List - Endpoints")]
    public async Task ItemsEmptyWhenPersistenceEmpty()
    {
        var defaultPage = 1;
        var defaultPerPage = 15;

        var (response, output) = await _fixture.ApiClient.Get<TestApiResponseList<CategoryModelOutput>>(
            "/categories"
        );

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
        output.Should().NotBeNull();
        output.Data.Should().NotBeNull();
        output.Meta.Should().NotBeNull();
        output.Meta.Total.Should().Be(0);
        output.Data.Should().HaveCount(0);
        output.Meta.PerPage.Should().Be(defaultPerPage);
        output.Meta.CurrentPage.Should().Be(defaultPage);
    }

    [Fact(DisplayName = nameof(ListCategoriesAndTotal))]
    [Trait("End2End/Api", "Category/List - Endpoints")]
    public async Task ListCategoriesAndTotal()
    {
        var exampleCategoryList = _fixture.GetExampleCategoriesList();
        await _fixture.Persistence.InsertList(exampleCategoryList);

        var input = new ListCategoriesInput(page: 1, perPage: 5);

        var (response, output) = await _fixture.ApiClient.Get<TestApiResponseList<CategoryModelOutput>>(
            "/categories",
            input
        );

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
        output.Should().NotBeNull();
        output!.Meta.Should().NotBeNull();
        output.Data.Should().NotBeNull();
        output.Meta.Total.Should().Be(exampleCategoryList.Count);
        output.Data.Should().HaveCount(input.PerPage);
        output.Meta.CurrentPage.Should().Be(input.Page);
        output.Meta.PerPage.Should().Be(input.PerPage);

        foreach (var outputItem in output.Data)
        {
            var exampleCategory = exampleCategoryList.Find(x => x.Id == outputItem.Id);

            exampleCategory.Should().NotBeNull();
            outputItem.Should().NotBeNull();
            outputItem.Id.Should().Be(exampleCategory!.Id);
            outputItem.Name.Should().Be(exampleCategory.Name);
            outputItem.Description.Should().Be(exampleCategory.Description);
            outputItem.IsActive.Should().Be(exampleCategory.IsActive);
            outputItem.CreatedAt.TrimMillisSeconds().Should().Be(
                exampleCategory.CreatedAt.TrimMillisSeconds()
            );
        }
    }

    [Theory(DisplayName = nameof(ListCategoriesPaginated))]
    [Trait("End2End/Api", "Category/List - Endpoints")]
    [InlineData(10, 1, 5, 5)]
    [InlineData(10, 2, 5, 5)]
    [InlineData(7, 2, 5, 2)]
    [InlineData(7, 3, 5, 0)]
    public async Task ListCategoriesPaginated(
        int quantityCategoriesToGenerate,
        int page,
        int perPage,
        int expectedQuantityItems
    )
    {
        var exampleCategoryList = _fixture.GetExampleCategoriesList(quantityCategoriesToGenerate);
        await _fixture.Persistence.InsertList(exampleCategoryList);

        var input = new ListCategoriesInput(
            page,
            perPage
        );

        var (response, output) = await _fixture.ApiClient.Get<TestApiResponseList<CategoryModelOutput>>(
            "/categories",
            input
        );

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
        output.Should().NotBeNull();
        output!.Meta.Should().NotBeNull();
        output.Data.Should().NotBeNull();
        output.Meta.Total.Should().Be(quantityCategoriesToGenerate);
        output.Data.Should().HaveCount(expectedQuantityItems);
        output.Meta.CurrentPage.Should().Be(input.Page);
        output.Meta.PerPage.Should().Be(input.PerPage);

        foreach (var outputItem in output.Data)
        {
            var exampleCategory = exampleCategoryList.Find(x => x.Id == outputItem.Id);
            exampleCategory.Should().NotBeNull();
            outputItem.Should().NotBeNull();
            outputItem.Id.Should().Be(exampleCategory!.Id);
            outputItem.Name.Should().Be(exampleCategory.Name);
            outputItem.Description.Should().Be(exampleCategory.Description);
            outputItem.IsActive.Should().Be(exampleCategory.IsActive);
            outputItem.CreatedAt.TrimMillisSeconds().Should().Be(
                exampleCategory.CreatedAt.TrimMillisSeconds()
            );
        }
    }

    [Theory(DisplayName = nameof(ListCategorySearchByText))]
    [Trait("End2End/Api", "Category/List - Endpoints")]
    [InlineData("Action", 1, 5, 1, 1)]
    [InlineData("Horror", 1, 5, 3, 3)]
    [InlineData("Horror", 2, 5, 0, 3)]
    [InlineData("Sci-fi", 1, 5, 4, 4)]
    [InlineData("Sci-fi", 1, 2, 2, 4)]
    [InlineData("Sci-fi", 2, 3, 1, 4)]
    [InlineData("Sci-fi Other", 1, 3, 0, 0)]
    [InlineData("Robots", 1, 5, 2, 2)]
    public async Task ListCategorySearchByText(
        string search,
        int page,
        int perPage,
        int expectedQuantityItemsReturned,
        int expectedQuantityTotalItems
    )
    {
        var exampleCategoriesList = _fixture.GetExampleCategoriesListWithName(
            new List<string>
            {
                "Action",
                "Horror",
                "Horror - Based on Real Facts",
                "Horror = Robots",
                "Drama",
                "Sci-fi IA",
                "Sci-fi Space",
                "Sci-fi Robots",
                "Sci-fi Future"
            }
        );

        await _fixture.Persistence.InsertList(exampleCategoriesList);

        var input = new ListCategoriesInput(
            page,
            perPage,
            search
        );

        var (response, output) = await _fixture.ApiClient.Get<TestApiResponseList<CategoryModelOutput>>(
            "/categories",
            input
        );

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
        output.Should().NotBeNull();
        output!.Meta.Should().NotBeNull();
        output.Data.Should().NotBeNull();
        output.Meta.Total.Should().Be(expectedQuantityTotalItems);
        output.Data.Should().HaveCount(expectedQuantityItemsReturned);
        output.Meta.CurrentPage.Should().Be(input.Page);
        output.Meta.PerPage.Should().Be(input.PerPage);

        foreach (var outputItem in output.Data)
        {
            var exampleCategory = exampleCategoriesList.Find(x => x.Id == outputItem.Id);
            exampleCategory.Should().NotBeNull();
            outputItem.Should().NotBeNull();
            outputItem.Id.Should().Be(exampleCategory!.Id);
            outputItem.Name.Should().Be(exampleCategory.Name);
            outputItem.Description.Should().Be(exampleCategory.Description);
            outputItem.IsActive.Should().Be(exampleCategory.IsActive);
        }
    }

    [Theory(DisplayName = nameof(ListCategoryOrdered))]
    [Trait("End2End/Api", "Category/List - Endpoints")]
    [InlineData("name", "asc")]
    [InlineData("name", "desc")]
    [InlineData("id", "asc")]
    [InlineData("id", "desc")]
    [InlineData("createdAt", "asc")]
    [InlineData("createdAt", "desc")]
    public async Task ListCategoryOrdered(
        string orderBy,
        string order
    )
    {
        var exampleCategoriesList = _fixture.GetExampleCategoriesList(10);
        await _fixture.Persistence.InsertList(exampleCategoriesList);

        var searchOrder = order.ToLower() == "asc" ? SearchOrder.Asc : SearchOrder.Desc;

        var input = new ListCategoriesInput(
            page: 1,
            perPage: 20,
            dir: searchOrder,
            sort: orderBy
        );

        var (response, output) = await _fixture.ApiClient.Get<TestApiResponseList<CategoryModelOutput>>(
            "/categories",
            input
        );

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
        output.Should().NotBeNull();
        output!.Meta.Should().NotBeNull();
        output.Data.Should().NotBeNull();
        output.Meta.Total.Should().Be(exampleCategoriesList.Count);
        output.Data.Should().HaveCount(exampleCategoriesList.Count);
        output.Meta.CurrentPage.Should().Be(input.Page);
        output.Meta.PerPage.Should().Be(input.PerPage);

        var expectedOrderedList = _fixture.CloneCategoriesListOrdered(
            exampleCategoriesList,
            orderBy,
            searchOrder
        );

        var count = 0;
        var expectedArr = expectedOrderedList.Select(x => $"{++count}, {x.Id}, {x.Name}, {x.CreatedAt} {JsonConvert.SerializeObject(x)}");
        count = 0;
        var outputArr = output.Data.Select(x => $"{++count}, {x.Id}, {x.Name}, {x.CreatedAt} {JsonConvert.SerializeObject(x)}");

        _output.WriteLine("Expecteds...");
        _output.WriteLine(String.Join('\n', expectedArr));
        _output.WriteLine("Outputs...");
        _output.WriteLine(String.Join('\n', outputArr));

        for (int indice = 0; indice < expectedOrderedList.Count; indice++)
        {
            var outputItem = output.Data[indice];
            var expectedItem = expectedOrderedList[indice];

            outputItem.Should().NotBeNull();
            expectedItem.Should().NotBeNull();
            outputItem.Name.Should().Be(expectedItem.Name);
            outputItem.Description.Should().Be(expectedItem.Description);
            outputItem.IsActive.Should().Be(expectedItem.IsActive);
            outputItem.CreatedAt.TrimMillisSeconds().Should().Be(
                expectedItem.CreatedAt.TrimMillisSeconds()
            );
        }
    }


    [Theory(DisplayName = nameof(ListOrderedDate))]
    [Trait("EndToEnd/API", "Category/List Endpoints")]
    [InlineData("createdAt", "asc")]
    [InlineData("createdAt", "desc")]
    public async Task ListOrderedDate(string orderBy, string order)
    {
        var exampleCategoriesList = _fixture.GetExampleCategoriesList(10);
        await _fixture.Persistence.InsertList(exampleCategoriesList);

        var searchOrder = order.ToLower() == "asc" ? SearchOrder.Asc : SearchOrder.Desc;

        var input = new ListCategoriesInput(
            page: 1,
            perPage: 20,
            dir: searchOrder,
            sort: orderBy
        );

        var (response, output) = await _fixture.ApiClient.Get<TestApiResponseList<CategoryModelOutput>>(
            "/categories",
            input
        );

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
        output.Should().NotBeNull();
        output!.Meta.Should().NotBeNull();
        output.Data.Should().NotBeNull();
        output.Meta.Total.Should().Be(exampleCategoriesList.Count);
        output.Data.Should().HaveCount(exampleCategoriesList.Count);
        output.Meta.CurrentPage.Should().Be(input.Page);
        output.Meta.PerPage.Should().Be(input.PerPage);
        DateTime? lastItemDate = null;

        foreach (var outputItem in output.Data)
        {
            var exampleCategory = exampleCategoriesList.Find(x => x.Id == outputItem.Id);
            exampleCategory.Should().NotBeNull();
            outputItem.Should().NotBeNull();
            outputItem.Id.Should().Be(exampleCategory!.Id);
            outputItem.Name.Should().Be(exampleCategory.Name);
            outputItem.Description.Should().Be(exampleCategory.Description);
            outputItem.IsActive.Should().Be(exampleCategory.IsActive);
            outputItem.CreatedAt.TrimMillisSeconds().Should().Be(
                exampleCategory.CreatedAt.TrimMillisSeconds()
            );

            if (lastItemDate != null)
            {
                if (order == "asc")
                {
                    Assert.True(outputItem.CreatedAt >= lastItemDate);
                }
                else
                {
                    Assert.True(outputItem.CreatedAt <= lastItemDate);
                }
            }

            lastItemDate = outputItem.CreatedAt;
        }
    }

    public void Dispose()
    {
        _fixture.CleanPersistence();
    }
}
