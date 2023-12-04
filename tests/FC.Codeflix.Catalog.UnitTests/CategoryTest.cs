using FC.Codeflix.Catalog.Domain.Exceptions;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entity;


namespace FC.Codeflix.Catalog.UnitTests
{
    public class CategoryTest
    {
        [Fact(DisplayName = nameof(Instantiate))]
        [Trait("Domain", "Category - Aggregates")]
        public void Instantiate()
        {
            //Arrange
            var validData = new
            {
                Name = "category name",
                Description = "category description"
            };

            var datetimeBefore = DateTime.Now;

            //Act
            var category = new DomainEntity.Category(validData.Name, validData.Description);

            var datetimeAfter = DateTime.Now;

            //Assert
            Assert.NotNull(category);
            Assert.Equal(validData.Name, category.Name);
            Assert.Equal(validData.Description, category.Description);
            Assert.NotEqual(default(Guid), category.Id);
            Assert.NotEqual(default(DateTime), category.CreatedAt);
            Assert.True(category.CreatedAt > datetimeBefore);
            Assert.True(category.CreatedAt < datetimeAfter);
            Assert.True(category.IsActive);
        }

        [Theory(DisplayName = nameof(InstantiateWithIsActiveStatus))]
        [Trait("Domain", "Category - Aggregates")]
        [InlineData(true)]
        [InlineData(false)]
        public void InstantiateWithIsActiveStatus(bool isActive)
        {
            //Arrange
            var validData = new
            {
                Name = "category name",
                Description = "category description",
            };

            //Act
            var datetimeBefore = DateTime.Now;
            var category = new DomainEntity.Category(validData.Name, validData.Description, isActive);
            var datetimeAfter = DateTime.Now;

            //Assert
            Assert.NotNull(category);
            Assert.Equal(validData.Name, category.Name);
            Assert.Equal(validData.Description, category.Description);
            Assert.NotEqual(default(Guid), category.Id);
            Assert.NotEqual(default(DateTime), category.CreatedAt);
            Assert.True(category.CreatedAt > datetimeBefore);
            Assert.True(category.CreatedAt < datetimeAfter);
            Assert.Equal(category.IsActive, isActive);
        }

        [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsEmpty))]
        [Trait("Domain", "Category - Aggregates")]
        [InlineData(" ")]
        [InlineData(null)]
        [InlineData("     ")]
        public void InstantiateErrorWhenNameIsEmpty(string? name)
        {
            Action action = () => new DomainEntity.Category(name!, "Category Description");
            var exception = Assert.Throws<EntityValidationException>(action);
            Assert.Equal("Name should not be empty or null", exception.Message);
        }

        [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsNull))]
        [Trait("Domain", "Category - Aggregates")]
        public void InstantiateErrorWhenDescriptionIsNull()
        {
            Action action = () => new DomainEntity.Category("Category name", null!);
            var exception = Assert.Throws<EntityValidationException>(action);
            Assert.Equal("Description should not be null", exception.Message);
        }

        //nome deve ter no minimo 3 caracteres
        [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsLessThan3Characters))]
        [Trait("Domain", "Category - Aggregates")]
        [InlineData("1")]
        [InlineData("12")]
        [InlineData("a")]
        [InlineData("ab")]
        public void InstantiateErrorWhenNameIsLessThan3Characters(string invalidName)
        {
            Action action = () => new DomainEntity.Category(invalidName, "Category description");
            var exception = Assert.Throws<EntityValidationException>(action);
            Assert.Equal("Name should be at leats 3 characters long", exception.Message);
        }

        //name deve ter no maximo 255 caracteres
        [Fact(DisplayName = nameof(InstantiateErrorWhenNameIsGreaterThen255Characters))]
        [Trait("Domain", "Category - Aggregates")]
        public void InstantiateErrorWhenNameIsGreaterThen255Characters()
        {
            var invalidName = String.Join(
                null,
                Enumerable.Range(1, 256).Select(_ => "a").ToArray()
            );

            Action action = () => new DomainEntity.Category(invalidName, "Category description");
            var exception = Assert.Throws<EntityValidationException>(action);
            Assert.Equal("Name should be less or equal 255 characters long", exception.Message);
        }

        //descrição deve ter no maximo 10000 caracteres
        [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsGreaterThen10_000Characters))]
        [Trait("Domain", "Category - Aggregates")]
        public void InstantiateErrorWhenDescriptionIsGreaterThen10_000Characters()
        {
            var invalidDescription = String.Join(
                null,
                Enumerable.Range(1, 10001).Select(_ => "a").ToArray()
            );

            Action action = () => new DomainEntity.Category("Category name", invalidDescription);
            var exception = Assert.Throws<EntityValidationException>(action);
            Assert.Equal("Description should be less or equal 10.000 characters long", exception.Message);
        }
    }
}

