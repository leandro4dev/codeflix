using FC.Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;

namespace FC.Codeflix.Catalog.UnitTests.Application.UpdateCategory;

public class UpdateCategoryTestDataGenerator
{
    public static IEnumerable<object[]> GetCategoriesToUpdate(int times = 5)
    {
        var fixture = new UpdateCategoryTestFixture();
        for(int index = 0; index < times; index++)
        {
            var exampleCategory = fixture.GetExampleCategory();
            var exampleInput = new UpdateCategoryInput(
                exampleCategory.Id,
                fixture.GetValidCategoryName(),
                fixture.GetValidCategoryDescription(),
                fixture.GetRandomBoolean()
            );

            yield return new object[]
            {
                exampleCategory,
                exampleInput
            };
        }
    }
}
