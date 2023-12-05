using FC.Codeflix.Catalog.Domain.Exceptions;
using FluentAssertions;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entity;

namespace FC.Codeflix.Catalog.UnitTests
{
    public class CategoryTest
    {
        [Fact(DisplayName = nameof(Instantiate))]
        [Trait("Domain", "Category - Aggregates")]
        public void Instantiate()
        {
            var validData = new
            {
                Name = "category name",
                Description = "category description"
            };

            var datetimeBefore = DateTime.Now;
            var category = new DomainEntity.Category(validData.Name, validData.Description);
            var datetimeAfter = DateTime.Now;

            category.Should().NotBeNull();
            category.Name.Should().Be(validData.Name);
            category.Description.Should().Be(validData.Description);
            category.Id.Should().NotBeEmpty();
            category.CreatedAt.Should().NotBeSameDateAs(default(DateTime));

            (category.CreatedAt > datetimeBefore).Should().BeTrue();
            (category.CreatedAt < datetimeAfter).Should().BeTrue();
            category.IsActive.Should().BeTrue();
        }

        [Theory(DisplayName = nameof(InstantiateWithIsActiveStatus))]
        [Trait("Domain", "Category - Aggregates")]
        [InlineData(true)]
        [InlineData(false)]
        public void InstantiateWithIsActiveStatus(bool isActive)
        {
            var validData = new
            {
                Name = "category name",
                Description = "category description",
            };

            var datetimeBefore = DateTime.Now;
            var category = new DomainEntity.Category(validData.Name, validData.Description, isActive);
            var datetimeAfter = DateTime.Now;

            category.Should().NotBeNull();
            category.Name.Should().Be(validData.Name);
            category.Description.Should().Be(validData.Description);
            category.Id.Should().NotBeEmpty();
            category.CreatedAt.Should().NotBeSameDateAs(default(DateTime));

            (category.CreatedAt > datetimeBefore).Should().BeTrue();
            (category.CreatedAt < datetimeAfter).Should().BeTrue();
            category.IsActive.Should().Be(isActive);
        }

        [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsEmpty))]
        [Trait("Domain", "Category - Aggregates")]
        [InlineData(" ")]
        [InlineData(null)]
        [InlineData("     ")]
        public void InstantiateErrorWhenNameIsEmpty(string? name)
        {
            Action action = () => new DomainEntity.Category(name!, "Category Description");
            action.Should().Throw<EntityValidationException>().WithMessage("Name should not be empty or null");
        }

        [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsNull))]
        [Trait("Domain", "Category - Aggregates")]
        public void InstantiateErrorWhenDescriptionIsNull()
        {
            Action action = () => new DomainEntity.Category("Category name", null!);
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
            Action action = () => new DomainEntity.Category(invalidName, "Category description");
            action.Should().Throw<EntityValidationException>().WithMessage("Name should be at leats 3 characters long");
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
            action.Should().Throw<EntityValidationException>().WithMessage("Name should be less or equal 255 characters long");
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
            action.Should().Throw<EntityValidationException>().WithMessage("Description should be less or equal 10.000 characters long");
        }

        [Fact(DisplayName = nameof(Activate))]
        [Trait("Domain", "Category - Aggregates")]
        public void Activate()
        {
            var validData = new
            {
                Name = "Category name",
                Description = "Category description"
            };

            var category = new DomainEntity.Category(validData.Name, validData.Description, false);
            category.Activate();

            category.IsActive.Should().BeTrue();
        }

        [Fact(DisplayName = nameof(Deactivate))]
        [Trait("Domain", "Category - Aggregates")]
        public void Deactivate()
        {
            var validData = new
            {
                Name = "Category name",
                Description = "Category description"
            };

            var category = new DomainEntity.Category(validData.Name, validData.Description, true);
            category.Deactivate();

            category.IsActive.Should().BeFalse();
        }

        [Fact(DisplayName = nameof(Update))]
        [Trait("Domain", "Category - Aggregates")]
        public void Update()
        {

            var category = new DomainEntity.Category("Category name", "Category Description");

            var newValues = new
            {
                Name = "New Name",
                Description = "New Description"
            };

            category.Update(newValues.Name, newValues.Description);

            newValues.Name.Should().Be(category.Name);
            newValues.Description.Should().Be(category.Description);
        }

        [Fact(DisplayName = nameof(UpdateOnlyName))]
        [Trait("Domain", "Category - Aggregates")]
        public void UpdateOnlyName()
        {
            var category = new DomainEntity.Category("Category name", "Category Description");
            var newValues = new { Name = "New Name" };
            var currentDescription = category.Description;

            category.Update(newValues.Name);

            newValues.Name.Should().Be(category.Name);
            currentDescription.Should().Be(category.Description);
        }

        [Theory(DisplayName = nameof(UpdateErrorWhenNameIsEmpty))]
        [Trait("Domain", "Category - Aggregates")]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData(null)]
        public void UpdateErrorWhenNameIsEmpty(string? name)
        {
            var category = new DomainEntity.Category("Category name", "Category Description");
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
            var category = new DomainEntity.Category("Category name", "Category Description");
            Action action = () => category.Update(invalidName);
            action.Should().Throw<EntityValidationException>().WithMessage("Name should be at leats 3 characters long");
        }

        [Fact(DisplayName = nameof(UpdateErrorWhenNameIsGreaterThan255Characters))]
        [Trait("Domain", "Category - Aggregates")]
        public void UpdateErrorWhenNameIsGreaterThan255Characters()
        {
            var category = new DomainEntity.Category("Category name", "Category Description");
            var invalidName = String.Join(null, Enumerable.Range(1, 256).Select(_ => "a").ToArray());
            Action action = () => category.Update(invalidName);

            action.Should().Throw<EntityValidationException>().WithMessage("Name should be less or equal 255 characters long");
        }

        [Fact(DisplayName = nameof(UpdateErrorWhenDescriptionIsGreaterThen10_000Characters))]
        [Trait("Domain", "Category - Aggregates")]
        public void UpdateErrorWhenDescriptionIsGreaterThen10_000Characters()
        {
            var category = new DomainEntity.Category("Category name", "Category Description");

            var invalidDescription = String.Join(null, Enumerable.Range(1, 10001).Select(_ => "a").ToArray());
            Action action = () => category.Update("Category new name", invalidDescription);

            action.Should().Throw<EntityValidationException>().WithMessage("Description should be less or equal 10.000 characters long");
        }
    }
}

