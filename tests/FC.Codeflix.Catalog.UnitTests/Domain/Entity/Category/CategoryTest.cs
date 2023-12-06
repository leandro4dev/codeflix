using Bogus;
using FC.Codeflix.Catalog.Domain.Exceptions;
using FC.Codeflix.Catalog.UnitTests.Domain.Entity.Category;
using FluentAssertions;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entity;

namespace FC.Codeflix.Catalog.UnitTests
{
    [Collection(nameof(CategoryTestFixture))]
    public class CategoryTest
    {
        private readonly CategoryTestFixture _categoryTestFixture;

        public CategoryTest(CategoryTestFixture categoryTestFixture)
        {
            _categoryTestFixture = categoryTestFixture;
        }

        [Fact(DisplayName = nameof(Instantiate))]
        [Trait("Domain", "Category - Aggregates")]
        public void Instantiate()
        {
            var validCategory = _categoryTestFixture.GetValidCategory();

            var datetimeBefore = DateTime.Now;
            var category = new DomainEntity.Category(
                validCategory.Name,
                validCategory.Description
            );
            var datetimeAfter = DateTime.Now.AddSeconds(1);

            category.Should().NotBeNull();
            category.Name.Should().Be(validCategory.Name);
            category.Description.Should().Be(validCategory.Description);
            category.Id.Should().NotBeEmpty();
            category.CreatedAt.Should().NotBeSameDateAs(default(DateTime));

            (category.CreatedAt >= datetimeBefore).Should().BeTrue();
            (category.CreatedAt <= datetimeAfter).Should().BeTrue();
            category.IsActive.Should().BeTrue();
        }

        [Theory(DisplayName = nameof(InstantiateWithIsActiveStatus))]
        [Trait("Domain", "Category - Aggregates")]
        [InlineData(true)]
        [InlineData(false)]
        public void InstantiateWithIsActiveStatus(bool isActive)
        {
            var validCategory = _categoryTestFixture.GetValidCategory();

            var datetimeBefore = DateTime.Now;
            var category = new DomainEntity.Category(validCategory.Name, validCategory.Description, isActive);
            var datetimeAfter = DateTime.Now.AddSeconds(1);

            category.Should().NotBeNull();
            category.Name.Should().Be(validCategory.Name);
            category.Description.Should().Be(validCategory.Description);
            category.Id.Should().NotBeEmpty();
            category.CreatedAt.Should().NotBeSameDateAs(default(DateTime));

            (category.CreatedAt >= datetimeBefore).Should().BeTrue();
            (category.CreatedAt <= datetimeAfter).Should().BeTrue();
            category.IsActive.Should().Be(isActive);
        }

        [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsEmpty))]
        [Trait("Domain", "Category - Aggregates")]
        [InlineData(" ")]
        [InlineData(null)]
        [InlineData("     ")]
        public void InstantiateErrorWhenNameIsEmpty(string? name)
        {
            var validCategory = _categoryTestFixture.GetValidCategory();

            Action action = () => new DomainEntity.Category(name!, validCategory.Description);
            action.Should().Throw<EntityValidationException>().WithMessage("Name should not be empty or null");
        }

        [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsNull))]
        [Trait("Domain", "Category - Aggregates")]
        public void InstantiateErrorWhenDescriptionIsNull()
        {
            var validCategory = _categoryTestFixture.GetValidCategory();

            Action action = () => new DomainEntity.Category(validCategory.Name, null!);
            action.Should().Throw<EntityValidationException>().WithMessage("Description should not be null");
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
            var validCategory = _categoryTestFixture.GetValidCategory();

            Action action = () => new DomainEntity.Category(invalidName, validCategory.Description);
            action.Should().Throw<EntityValidationException>().WithMessage("Name should be at leats 3 characters long");
        }

        //name deve ter no maximo 255 caracteres
        [Fact(DisplayName = nameof(InstantiateErrorWhenNameIsGreaterThen255Characters))]
        [Trait("Domain", "Category - Aggregates")]
        public void InstantiateErrorWhenNameIsGreaterThen255Characters()
        {
            var validCategory = _categoryTestFixture.GetValidCategory();

            var invalidName = String.Join(
                null,
                Enumerable.Range(1, 256).Select(_ => "a").ToArray()
            );

            Action action = () => new DomainEntity.Category(invalidName, validCategory.Description);
            action.Should().Throw<EntityValidationException>().WithMessage("Name should be less or equal 255 characters long");
        }

        //descrição deve ter no maximo 10000 caracteres
        [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsGreaterThen10_000Characters))]
        [Trait("Domain", "Category - Aggregates")]
        public void InstantiateErrorWhenDescriptionIsGreaterThen10_000Characters()
        {
            var validCategory = _categoryTestFixture.GetValidCategory();

            var invalidDescription = String.Join(
                null,
                Enumerable.Range(1, 10001).Select(_ => "a").ToArray()
            );

            Action action = () => new DomainEntity.Category(validCategory.Name, invalidDescription);
            action.Should().Throw<EntityValidationException>().WithMessage("Description should be less or equal 10.000 characters long");
        }

        [Fact(DisplayName = nameof(Activate))]
        [Trait("Domain", "Category - Aggregates")]
        public void Activate()
        {
            var validCategory = _categoryTestFixture.GetValidCategory();

            var category = new DomainEntity.Category(validCategory.Name, validCategory.Description, false);
            category.Activate();

            category.IsActive.Should().BeTrue();
        }

        [Fact(DisplayName = nameof(Deactivate))]
        [Trait("Domain", "Category - Aggregates")]
        public void Deactivate()
        {
            var validCategory = _categoryTestFixture.GetValidCategory();

            var category = new DomainEntity.Category(validCategory.Name, validCategory.Description, true);
            category.Deactivate();

            category.IsActive.Should().BeFalse();
        }

        [Fact(DisplayName = nameof(Update))]
        [Trait("Domain", "Category - Aggregates")]
        public void Update()
        {
            var validCategory = _categoryTestFixture.GetValidCategory();

            var category = new DomainEntity.Category(validCategory.Name, validCategory.Description);

            var categoryWithNewValues = _categoryTestFixture.GetValidCategory();

            category.Update(categoryWithNewValues.Name, categoryWithNewValues.Description);

            categoryWithNewValues.Name.Should().Be(category.Name);
            categoryWithNewValues.Description.Should().Be(category.Description);
        }

        [Fact(DisplayName = nameof(UpdateOnlyName))]
        [Trait("Domain", "Category - Aggregates")]
        public void UpdateOnlyName()
        {
            var validCategory = _categoryTestFixture.GetValidCategory();

            var category = new DomainEntity.Category(validCategory.Name, validCategory.Description);
            var newName = _categoryTestFixture.GetValidCategoryName();
            var currentDescription = category.Description;

            category.Update(newName);

            category.Name.Should().Be(newName);
            category.Description.Should().Be(currentDescription);
        }

        [Theory(DisplayName = nameof(UpdateErrorWhenNameIsEmpty))]
        [Trait("Domain", "Category - Aggregates")]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData(null)]
        public void UpdateErrorWhenNameIsEmpty(string? name)
        {
            var validCategory = _categoryTestFixture.GetValidCategory();

            var category = new DomainEntity.Category(validCategory.Name, validCategory.Description);
            Action action = () => category.Update(name!);
            action.Should().Throw<EntityValidationException>().WithMessage("Name should not be empty or null");
        }

        [Theory(DisplayName = nameof(UpdateErrorWhenNameIsLessThan3Characters))]
        [Trait("Domain", "Category - Aggregates")]
        [InlineData("1")]
        [InlineData("12")]
        [InlineData("a")]
        [InlineData("ab")]
        public void UpdateErrorWhenNameIsLessThan3Characters(string invalidName)
        {
            var validCategory = _categoryTestFixture.GetValidCategory();

            var category = new DomainEntity.Category(validCategory.Name, validCategory.Description);
            Action action = () => category.Update(invalidName);
            action.Should().Throw<EntityValidationException>().WithMessage("Name should be at leats 3 characters long");
        }

        [Fact(DisplayName = nameof(UpdateErrorWhenNameIsGreaterThan255Characters))]
        [Trait("Domain", "Category - Aggregates")]
        public void UpdateErrorWhenNameIsGreaterThan255Characters()
        {
            var validCategory = _categoryTestFixture.GetValidCategory();

            var category = new DomainEntity.Category(validCategory.Name, validCategory.Description);
            var invalidName = _categoryTestFixture.Faker.Lorem.Letter(256);
            Action action = () => category.Update(invalidName);

            action.Should().Throw<EntityValidationException>().WithMessage("Name should be less or equal 255 characters long");
        }

        [Fact(DisplayName = nameof(UpdateErrorWhenDescriptionIsGreaterThen10_000Characters))]
        [Trait("Domain", "Category - Aggregates")]
        public void UpdateErrorWhenDescriptionIsGreaterThen10_000Characters()
        {
            var validCategory = _categoryTestFixture.GetValidCategory();

            var category = new DomainEntity.Category(validCategory.Name, validCategory.Description);

            var invalidDescription = _categoryTestFixture.Faker.Commerce.ProductDescription();

            while (invalidDescription.Length <= 10000)
            {
                invalidDescription = $"{invalidDescription} {_categoryTestFixture.Faker.Commerce.ProductDescription()}";
            }

            Action action = () => category.Update("Category new name", invalidDescription);

            action.Should().Throw<EntityValidationException>().WithMessage("Description should be less or equal 10.000 characters long");
        }
    }
}

