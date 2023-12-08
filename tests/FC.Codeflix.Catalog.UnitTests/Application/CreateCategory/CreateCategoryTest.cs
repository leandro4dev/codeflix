using FC.Codeflix.Catalog.Domain.Entity;
using Moq;
using UseCases = FC.Codeflix.Application.UseCases.CreateCategory;

namespace FC.Codeflix.Catalog.UnitTests.Application.CreateCategory;

public class CreateCategoryTest
{
    [Fact(DisplayName = nameof(CreateCategory))]
    [Trait("Application", "CreateCategory - Use Cases")]
    public async void CreateCategory()
    {
        //arrange
        var repositoryMock = new Mock<ICategoryRepository>();
        var unitOfWorkMock = new Mock<IunitOfWork>();

        var useCase = new UseCases.CreateCategory(
            repositoryMock.Object, 
            unitOfWorkMock.Object
        );

        var input = new CreatecategoryInput(
            "Category Name",
            "Category Description",
            true
        );
        
        //act
        var output = await useCase.Handle(input, CancellationToken.None);


        //assert
        repositoryMock.Verify(
            repository => repository.Create(
                It.IsAny<Category>(),
                It.IsAny<CancellationToken>()
            ), 
            Times.Once
        );

        unitOfWorkMock.Verify(
            uow => uow.Commit(
                It.IsAny<CancellationToken>()
            ), 
            Times.Once
        );
        
        output.Should().NotBeNull();
        output.Name.Should().Be("Category Name");
        output.Description.Should().Be("Category Description");
        (output.Id != null && output.Id != Guid.Empty).Should().BeTrue();
        (output.CreatedAt != null && output.CreatedAt != default(DateTime)).Should().BeTrue();
    }
}
