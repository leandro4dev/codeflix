using FC.Codeflix.Catalog.Domain.Entity;
using FC.Codeflix.Catalog.Domain.SeedWork.SearchableRepository;
using Moq;
using System.Text;

namespace FC.Codeflix.Catalog.UnitTests.Application.ListCategories;

[Collection(nameof(ListCategoriesTestFixture))]
public class ListCategoriesTest
{
    private readonly ListCategoriesTestFixture _fixture;

    public ListCategoriesTest(ListCategoriesTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(ListCategories))]
    [Trait("Application", "ListCategories - Use Case")]
    public async Task ListCategories()
    {
        var categoriesList = _fixture.GetExampleCategoriesList();
        var repositoryMock = _fixture.GetRepositoryMock();

        var input = new ListCategoriesInput(
            page: 2,
            perPage: 15,
            search: "search-example",
            sort: "name",
            dir: SearchOrder.Asc
        );

        var outputRepositorySearch = new SearchOutput<Category>(
            currentPage: input.Page,
            perPage: input.Perpage,
            items: (IReadOnlyList<Category>) categoriesList,
            total: 70
        );

        repositoryMock.Setup(x => x.Search(
            It.Is<SearchInput>(
                searchInput.Page == input.Page &&
                searchInput.PerPage == input.PerPage &&
                searchInput.Search == input.Search &&
                searchInput.OrderBy == input.OrderBy &&
                searchInput.Order == input.Order
            ),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(outputRepositorySearch);

        var useCase = new ListCategories(repositoryMock.Object);

        var output = await useCase.Handle(input, CancellationToken.None);

        //Asserts
        output.Should().NotBeNull();
        output.Page.Should().Be(outputRepositorySearch.currentPage);
        output.PerPage.Should().Be(outputRepositorySearch.PerPage);
        output.Total.Should().Be(outputRepositorySearch.Total);
        output.Items.Should().HaveCount(outputRepositorySearch.Items.Count);

        output.Items.Foreach(outputItem => {
            var respositoryCategory = outputRepositorySearch.Items
                .Find(x => x.Id == outputItem.Id);

            outputItem.Should().NotBeNull();
            outputItem.Id.Should().Be(respositoryCategory.Id);
            outputItem.Name.Should().Be(respositoryCategory.Name);
            outputItem.Description.Should().Be(respositoryCategory.Description);
            outputItem.IsActive.Should().Be(respositoryCategory.IsActive);
            outputItem.CreatedAt.Should().Be(respositoryCategory.CreatedAt);
        });

        repositoryMock.Verify(x => x.Search(
            It.Is<SearchInput>(
                searchInput.Page == input.Page &&
                searchInput.PerPage == input.PerPage &&
                searchInput.Search == input.Search &&
                searchInput.OrderBy == input.OrderBy &&
                searchInput.Order == input.Order
            ),
            It.IsAny<CancellationToken>()
        ), Times.Once);
    }
}
