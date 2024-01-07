
namespace FC.Codeflix.Catalog.UnitTests.Application.CreateCategory
{
    public class CreateCategoryTestDataGenerator
    {
        public static IEnumerable<object[]> GetInvalidInputs(int times = 12)
        {
            var fixture = new CreateCategoryTestFixture();
            var invalidInputList = new List<object[]>();
            var totalInvalidCases = 4;    

            for(int index = 0; index < times; index++)
            {
                switch(index % totalInvalidCases)
                {
                    case 0:
                        //nome não pode ser menor que 3 caracteres
                        invalidInputList.Add(new object[] {
                            fixture.GetInvalidInputShortName(),
                            "Name should not be less than 3 characters long"
                        });
                        break;
                    case 1:
                        //nome não pode ser maior do que 255 caracteres
                        invalidInputList.Add(new object[] {
                            fixture.GetInvalidInputTooLongName(),
                            "Name should not be greater than 255 characters long"
                        });
                        break;
                    case 2:
                        //description não pode ser nula
                        invalidInputList.Add(new object[] {
                            fixture.GetInvalidInputDescriptionNull(),
                            "Description should not be null"
                        });
                        break;
                    case 3:
                        //description ser maior do que 10.000 caracteres
                        invalidInputList.Add(new object[] {
                            fixture.GetInvalidInputDescriptionTooLong(),
                            "Description should not be greater than 10000 characters long"
                        });
                        break;
                    default: break;
                }
            }

            return invalidInputList;
        }
    }
}