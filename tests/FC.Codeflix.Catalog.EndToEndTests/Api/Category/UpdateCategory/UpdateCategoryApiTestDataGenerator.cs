using FC.Codeflix.Catalog.EndToEndTests.Api.Category.CreateCategory;

namespace FC.Codeflix.Catalog.EndToEndTests.Api.Category.UpdateCategory;

public class UpdateCategoryApiTestDataGenerator
{
    public static IEnumerable<object[]> GetInvalidInputs()
    {
        var fixture = new UpdateCategoryApiTestFixture();

        var invalidInputList = new List<object[]>();
        var totalInvalidCases = 3;

        for (int index = 0; index < totalInvalidCases; index++)
        {
            switch (index % totalInvalidCases)
            {
                case 0:
                    //nome não pode ser menor que 3 caracteres
                    var input1 = fixture.GetExampleInput();
                    input1.Name = fixture.GetInvalidShortName();
                    invalidInputList.Add(new object[] {
                        input1,
                        "Name should not be less than 3 characters long"
                    });
                    break;
                case 1:
                    //nome não pode ser maior do que 255 caracteres
                    var input2 = fixture.GetExampleInput();
                    input2.Name = fixture.GetInvalidNameTooLong();
                    invalidInputList.Add(new object[] {
                        input2,
                        "Name should not be greater than 255 characters long"
                    });
                    break;
                case 2:
                    //description ser maior do que 10.000 caracteres
                    var input4 = fixture.GetExampleInput();
                    input4.Description = fixture.GetInvalidInputDescriptionTooLong();
                    invalidInputList.Add(new object[] {
                        input4,
                        "Description should not be greater than 10000 characters long"
                    });
                    break;
                default: break;
            }
        }

        return invalidInputList;
    }
}
