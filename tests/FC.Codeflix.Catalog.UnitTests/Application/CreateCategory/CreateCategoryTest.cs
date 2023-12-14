using FC.Codeflix.Catalog.Application.UseCases.Category.CreateCategory;
using FC.Codeflix.Catalog.Domain.Entity;
using FC.Codeflix.Catalog.Domain.Exceptions;
using FluentAssertions;
using Moq;
using UseCases = FC.Codeflix.Catalog.Application.UseCases.Category.CreateCategory;

namespace FC.Codeflix.Catalog.UnitTests.Application.CreateCategory;

[Collection(nameof(CreateCategoryTestFixture))]
public class CreateCategoryTest
{
    private readonly CreateCategoryTestFixture _createCategoryTestFixture;
    public CreateCategoryTest(CreateCategoryTestFixture createCategoryTestFixture)
    {
        _createCategoryTestFixture = createCategoryTestFixture;
    }

    [Fact(DisplayName = nameof(CreateCategory))]
    [Trait("Application", "CreateCategory - Use Cases")]
    public async void CreateCategory()
    {
        //arrange
        var repositoryMock = _createCategoryTestFixture.GetRepositoryMock();
        var unitOfWorkMock = _createCategoryTestFixture.GetUnitOfWorkMock();

        var useCase = new UseCases.CreateCategory(
            repositoryMock.Object,
            unitOfWorkMock.Object
        );

        var input = _createCategoryTestFixture.GetValidInput();

        //act
        var output = await useCase.Handle(input, CancellationToken.None);

        //assert
        repositoryMock.Verify(
            repository => repository.Insert(
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
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().Be(input.IsActive);
        output.Id.Should().NotBeEmpty();
        output.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
    }

    [Theory(DisplayName = nameof(ThrowWhenCantInstantiateAggregate))]
    [Trait("Application", "CreateCategory - Use Cases")]
    [MemberData(nameof(GetInvalidInputs))]
    public void ThrowWhenCantInstantiateAggregate(CreateCategoryInput input, string exceptionMessage)
    {
        var useCase = new UseCases.CreateCategory(
            _createCategoryTestFixture.GetRepositoryMock().Object,
            _createCategoryTestFixture.GetUnitOfWorkMock().Object
        );

        Action action = async () => await useCase.Handle(input, CancellationToken.None);

        action.Should().Throw<EntityValidationException>().WithMessage(exceptionMessage);
    }

    public static IEnumerable<object[]> GetInvalidInputs()
    {
        var invalidInputList = new List<object[]>();

        //nome não pode ser menor que 4 caracteres
        //nome não pode ser maior do que 255 caracteres
        //description não pode ser nula

        return invalidInputList;
    }
}
