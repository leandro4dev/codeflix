using DomainEntity = FC.Codeflix.Catalog.Domain.Entity;

namespace FC.Codeflix.Catalog.UnitTests.Domain.Entity.Category
{
    public class CategoryTestFixture
    {
        public DomainEntity.Category GetValidCategory()
        {
            return new DomainEntity.Category("Category name", "Category Description");
        }
    }

    [CollectionDefinition(nameof(CategoryTestFixture))]
    public class CategoryTestFixtureColletion : ICollectionFixture<CategoryTestFixture>
    { }
}