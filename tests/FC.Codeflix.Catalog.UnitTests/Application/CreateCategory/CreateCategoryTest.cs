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

    [Fact(DisplayName = nameof(CreateCategoryWithOnlyName))]
    [Trait("Application", "CreateCategory - Use Cases")]
    public async void CreateCategoryWithOnlyName()
    {
        var repositoryMock = _createCategoryTestFixture.GetRepositoryMock();
        var unitOfWorkMock = _createCategoryTestFixture.GetUnitOfWorkMock();

        var useCase = new UseCases.CreateCategory(
            repositoryMock.Object,
            unitOfWorkMock.Object
        );

        var input = new CreateCategoryInput(_createCategoryTestFixture.GetValidCategoryName());

        var output = await useCase.Handle(input, CancellationToken.None);

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
        output.Description.Should().BeEmpty();
        output.IsActive.Should().BeTrue();
        output.Id.Should().NotBeEmpty();
        output.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
    }

    [Fact(DisplayName = nameof(CreateCategoryWithOnlyNameAndDescription))]
    [Trait("Application", "CreateCategory - Use Cases")]
    public async void CreateCategoryWithOnlyNameAndDescription()
    {
        var repositoryMock = _createCategoryTestFixture.GetRepositoryMock();
        var unitOfWorkMock = _createCategoryTestFixture.GetUnitOfWorkMock();

        var useCase = new UseCases.CreateCategory(
            repositoryMock.Object,
            unitOfWorkMock.Object
        );

        var input = new CreateCategoryInput(
            _createCategoryTestFixture.GetValidCategoryName(),
            _createCategoryTestFixture.GetValidCategoryDescription()
        );

        var output = await useCase.Handle(input, CancellationToken.None);

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
        output.IsActive.Should().BeTrue();
        output.Id.Should().NotBeEmpty();
        output.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
    }

    [Theory(DisplayName = nameof(ThrowWhenCantInstantiateAggregate))]
    [Trait("Application", "CreateCategory - Use Cases")]
    [MemberData(nameof(GetInvalidInputs))]
    public async void ThrowWhenCantInstantiateAggregate(CreateCategoryInput input, string exceptionMessage)
    {
        var useCase = new UseCases.CreateCategory(
            _createCategoryTestFixture.GetRepositoryMock().Object,
            _createCategoryTestFixture.GetUnitOfWorkMock().Object
        );

        Func<Task> task = async () => await useCase.Handle(input, CancellationToken.None);

        await task.Should()
            .ThrowAsync<EntityValidationException>()
            .WithMessage(exceptionMessage);
    }

    public static IEnumerable<object[]> GetInvalidInputs()
    {
        var fixture = new CreateCategoryTestFixture();
        var invalidInputList = new List<object[]>();

        //nome não pode ser menor que 4 caracteres
        var invalidInputShortName = fixture.GetValidInput();
        invalidInputShortName.Name = invalidInputShortName.Name.Substring(0, 2);

        invalidInputList.Add(new object[] {
            invalidInputShortName,
            "Name should not be less than 3 characters long"
        });

        //nome não pode ser maior do que 255 caracteres
        var invalidInputTooLongName = fixture.GetValidInput();
        var tooLongNameForCategory = "";

        while (tooLongNameForCategory.Length < 255)
        {
            tooLongNameForCategory = $"{tooLongNameForCategory} {fixture.Faker.Commerce.ProductName}";
        }

        invalidInputTooLongName.Name = tooLongNameForCategory;

        invalidInputList.Add(new object[] {
            invalidInputTooLongName,
            "Name should not be greater than 255 characters long"
        });

        //description não pode ser nula
        var invalidInputDescriptionNull = fixture.GetValidInput();
        invalidInputDescriptionNull.Description = null!;
        invalidInputList.Add(new object[] {
            invalidInputDescriptionNull,
            "Description should not be null"
        });


        //description ser maior do que 10.000 caracteres
        var invalidInputTooLongDescription = fixture.GetValidInput();
        var tooLongDescription = fixture.Faker.Commerce.ProductName();

        while (tooLongDescription.Length <= 10000)
        {
            tooLongDescription = $"{tooLongDescription} {fixture.Faker.Commerce.ProductDescription()}";
        }

        invalidInputTooLongDescription.Description = tooLongDescription;
        invalidInputList.Add(new object[] {
            invalidInputTooLongDescription,
            "Description should not be greater than 10000 characters long"
        });

        return invalidInputList;
    }
}
