﻿using FC.Codeflix.Catalog.Application.UseCases.Category.CreateCategory;
using FC.Codeflix.Catalog.Domain.Exceptions;
using FC.Codeflix.Catalog.Infra.Data.EF;
using FC.Codeflix.Catalog.Infra.Data.EF.Repositories;
using FC.Codeflix.Catalog.Infra.Data.EF.UnitOfWork;
using FluentAssertions;
using UseCase = FC.Codeflix.Catalog.Application.UseCases.Category.CreateCategory;

namespace FC.Codeflix.Catalog.IntegrationTests.Application.UseCases.Category.CreateCategory;

[Collection(nameof(CreateCategoryTestFixture))]
public class CreateCategoryTest
{
    private readonly CreateCategoryTestFixture _fixture;

    public CreateCategoryTest(CreateCategoryTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(CreateCategory))]
    [Trait("Integration/Application", "CreateCategory - Use Cases")]
    public async Task CreateCategory()
    {   
        CodeflixCatalogDbContext dbContext = _fixture.CreateDbContext();

        var categoryRepository = new CategoryRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);

        var useCase = new UseCase.CreateCategory(categoryRepository, unitOfWork);

        var input = _fixture.GetInput();
        var output = await useCase.Handle(input, CancellationToken.None);

        var dbCategory = await (_fixture.CreateDbContext(true)).Categories.FindAsync(output.Id);

        dbCategory.Should().NotBeNull();
        dbCategory!.Name.Should().Be(input.Name);
        dbCategory.Description.Should().Be(input.Description);
        dbCategory.IsActive.Should().Be(input.IsActive);
        dbCategory.Id.Should().NotBeEmpty();
        dbCategory.CreatedAt.Should().Be(output.CreatedAt);

        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().Be(input.IsActive);
        output.Id.Should().NotBeEmpty();
        output.CreatedAt.Should().NotBeSameDateAs(default);
    }

    [Fact(DisplayName = nameof(CreateCategoryWithOnlyName))]
    [Trait("Integration/Application", "CreateCategory - Use Cases")]
    public async Task CreateCategoryWithOnlyName()
    {
        CodeflixCatalogDbContext dbContext = _fixture.CreateDbContext();

        var categoryRepository = new CategoryRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);

        var useCase = new UseCase.CreateCategory(categoryRepository, unitOfWork);
        var input = new CreateCategoryInput(_fixture.GetInput().Name);

        var output = await useCase.Handle(input, CancellationToken.None);

        var dbCategory = await (_fixture.CreateDbContext(true)).Categories.FindAsync(output.Id);

        dbCategory.Should().NotBeNull();
        dbCategory!.Name.Should().Be(input.Name);
        dbCategory.Description.Should().Be("");
        dbCategory.IsActive.Should().Be(true);
        dbCategory.Id.Should().NotBeEmpty();
        dbCategory.CreatedAt.Should().Be(output.CreatedAt);

        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be("");
        output.IsActive.Should().Be(true);
        output.Id.Should().NotBeEmpty();
        output.CreatedAt.Should().NotBeSameDateAs(default);
    }


    [Fact(DisplayName = nameof(CreateCategoryWithOnlyNameAndDescription))]
    [Trait("Integration/Application", "CreateCategory - Use Cases")]
    public async void CreateCategoryWithOnlyNameAndDescription()
    {
        CodeflixCatalogDbContext dbContext = _fixture.CreateDbContext();

        var categoryRepository = new CategoryRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);

        var useCase = new UseCase.CreateCategory(categoryRepository, unitOfWork);
        var input = new CreateCategoryInput(
            _fixture.GetValidCategoryName(),
            _fixture.GetValidCategoryDescription()
        );

        var output = await useCase.Handle(input, CancellationToken.None);
        var dbCategory = await (_fixture.CreateDbContext(true)).Categories.FindAsync(output.Id);

        dbCategory.Should().NotBeNull();
        dbCategory!.Name.Should().Be(input.Name);
        dbCategory.Description.Should().Be(input.Description);
        dbCategory.IsActive.Should().Be(true);
        dbCategory.Id.Should().NotBeEmpty();
        dbCategory.CreatedAt.Should().Be(output.CreatedAt);

        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().Be(true);
        output.Id.Should().NotBeEmpty();
        output.CreatedAt.Should().NotBeSameDateAs(default);
    }


    [Theory(DisplayName = nameof(ThrowWhenCantInstantiateCategory))]
    [Trait("Integration/Application", "CreateCategory - Use Cases")]
    [MemberData(
        nameof(CreateCategoryTestDataGenerator.GetInvalidInputs),
        parameters: 6,
        MemberType = typeof(CreateCategoryTestDataGenerator)
    )]
    public async Task ThrowWhenCantInstantiateCategory(CreateCategoryInput input, string exceptionMessage)
    {
        CodeflixCatalogDbContext dbContext = _fixture.CreateDbContext();
        
        var categoryRepository = new CategoryRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);

        var useCase = new UseCase.CreateCategory(
            categoryRepository,
            unitOfWork
        );

        Func<Task> task = async () => await useCase.Handle(input, CancellationToken.None);

        await task.Should()
            .ThrowAsync<EntityValidationException>()
            .WithMessage(exceptionMessage);
    }
}
