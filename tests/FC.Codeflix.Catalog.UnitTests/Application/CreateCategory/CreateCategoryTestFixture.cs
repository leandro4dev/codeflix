
using FC.Codeflix.Catalog.Application.Interfaces;
using FC.Codeflix.Catalog.Application.UseCases.Category.CreateCategory;
using FC.Codeflix.Catalog.Domain.Repository;
using FC.Codeflix.Catalog.UnitTests.Common;
using Moq;

namespace FC.Codeflix.Catalog.UnitTests.Application.CreateCategory
{
    public class CreateCategoryTestFixture : BaseFixture
    {
        public CreateCategoryTestFixture() : base() { }

        public string GetValidCategoryName()
        {
            var categoryName = "";

            while (categoryName.Length < 3)
            {
                categoryName = Faker.Commerce.Categories(1)[0];
            }

            if (categoryName.Length > 255)
            {
                categoryName = categoryName[..255]; //primeiros 255 characters
            }

            return categoryName;
        }

        public string GetValidCategoryDescription()
        {
            var categoryDescription = Faker.Commerce.ProductDescription();
            if (categoryDescription.Length > 10000)
            {
                categoryDescription = categoryDescription[..10_000];
            }

            return categoryDescription;
        }

        public bool GetRandomBoolean()
        {
            return (new Random()).NextDouble() < 0.5;
        }

        public CreateCategoryInput GetValidInput()
        {
            return new(
                GetValidCategoryName(),
                GetValidCategoryDescription(),
                GetRandomBoolean()
            );
        }

        public Mock<ICategoryRepository> GetRepositoryMock()
        {
            return new Mock<ICategoryRepository>();
        }

        public Mock<IUnitOfWork> GetUnitOfWorkMock()
        {
            return new Mock<IUnitOfWork>();
        }
    }

    [CollectionDefinition(nameof(CreateCategoryTestFixture))]
    public class CreateCategoryTestFixtureCollection : ICollectionFixture<CreateCategoryTestFixture> { }
}