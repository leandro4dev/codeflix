using FC.Codeflix.Catalog.Application.UseCases.Category.Common;
using FC.Codeflix.Catalog.Application.UseCases.Category.ListCategories;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace FC.Codeflix.Catalog.EndToEndTests.Api.Category.ListCategories;

[Collection(nameof(ListCategoriesApiTestFixture))]
public class ListCategoriesApiTest : IDisposable
{
    private readonly ListCategoriesApiTestFixture _fixture;

    public ListCategoriesApiTest(ListCategoriesApiTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(ListCategoriesAndTotalByDefault))]
    [Trait("End2End/Api", "Category/List - Endpoints")]
    public async void ListCategoriesAndTotalByDefault() 
    {
        var defaultPerPage = 15;
        var exampleCategoryList = _fixture.GetExampleCategoriesList(20);
        await _fixture.Persistence.InsertList(exampleCategoryList);

        var (response, output) = await _fixture.ApiClient.Get<ListCategoriesOutput>(
            "/categories"
        );

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
        output.Should().NotBeNull();
        output!.Total.Should().Be(exampleCategoryList.Count);
        output.Items.Should().HaveCount(defaultPerPage);

        foreach (CategoryModelOutput outputItem in output.Items) {
            var exampleItem = exampleCategoryList.FirstOrDefault(x => x.Id == outputItem.Id);

            exampleItem.Should().NotBeNull();

            outputItem.Id.Should().Be(exampleItem!.Id);
            outputItem.Name.Should().Be(exampleItem.Name);
            outputItem.Description.Should().Be(exampleItem.Description);
            outputItem.IsActive.Should().Be(exampleItem.IsActive);
            outputItem.CreatedAt.Should().Be(exampleItem.CreatedAt);
        }
    }

    public void Dispose()
    {
        _fixture.CleanPersistence();
    }
}
