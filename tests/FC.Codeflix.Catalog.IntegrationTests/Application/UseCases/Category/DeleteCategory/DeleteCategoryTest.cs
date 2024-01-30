using FC.Codeflix.Catalog.Application.UseCases.Category.DeleteCategory;
using FC.Codeflix.Catalog.Infra.Data.EF;
using FC.Codeflix.Catalog.Infra.Data.EF.Repositories;
using FC.Codeflix.Catalog.Infra.Data.EF.UnitOfWork;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using UseCase = FC.Codeflix.Catalog.Application.UseCases.Category.DeleteCategory;

namespace FC.Codeflix.Catalog.IntegrationTests.Application.UseCases.Category.DeleteCategory;

[Collection(nameof(DeleteCategoryTestFixture))]
public class DeleteCategoryTest
{
    private readonly DeleteCategoryTestFixture _fixture;

    public DeleteCategoryTest(DeleteCategoryTestFixture fixture)
    {
        _fixture = fixture;
    }


    [Fact(DisplayName = nameof(DeleteCategory))]
    [Trait("Integration/Application", "DeleteCategory - Use Cases")]
    public async Task DeleteCategory()
    {
        CodeflixCatalogDbContext dbContext = _fixture.CreateDbContext();

        var exampleCategory = _fixture.GetExampleCategory();

        var trackingInfo = await dbContext.Categories.AddAsync(exampleCategory);
        await dbContext.SaveChangesAsync();
        trackingInfo.State = EntityState.Detached;

        var input = new DeleteCategoryInput(
            exampleCategory.Id
        );

        var repository = new CategoryRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);

        var useCase = new UseCase.DeleteCategory(repository, unitOfWork);

        await useCase.Handle(input, CancellationToken.None);

        var dbCategoriesList = (_fixture.CreateDbContext(true)).Categories.AsNoTracking().ToList() ;
        dbCategoriesList.Should().HaveCount(0);
    }
}
