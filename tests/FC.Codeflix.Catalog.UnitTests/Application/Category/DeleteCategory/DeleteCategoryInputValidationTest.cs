using FC.Codeflix.Catalog.Application.UseCases.Category.DeleteCategory;
using FluentAssertions;

namespace FC.Codeflix.Catalog.UnitTests.Application.Category.DeleteCategory;

[Collection(nameof(DeleteCategoryTestFixture))]
public class DeleteCategoryInputValidationTest
{
    private readonly DeleteCategoryTestFixture _fixture;

    public DeleteCategoryInputValidationTest(DeleteCategoryTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(ValidationOk))]
    [Trait("Application", "DeleteCategoryInputValidationTest - UseCases")]
    public void ValidationOk()
    {
        var validInput = new DeleteCategoryInput(Guid.NewGuid());
        var validator = new DeleteCategoryInputValidator();

        var validationResult = validator.Validate(validInput);

        validationResult.Should().NotBeNull();
        validationResult.IsValid.Should().BeTrue();
        validationResult.Errors.Should().HaveCount(0);
    }

    [Fact(DisplayName = nameof(InvalidWhenEmptyGuidId))]
    [Trait("Application", "DeleteCategoryInputValidationTest - UseCases")]
    public void InvalidWhenEmptyGuidId()
    {
        var validInput = new DeleteCategoryInput(Guid.Empty);
        var validator = new DeleteCategoryInputValidator();

        var validationResult = validator.Validate(validInput);

        validationResult.Should().NotBeNull();
        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Should().HaveCount(1);
        validationResult.Errors[0].ErrorMessage.Should().Be("'Id' must not be empty.");
    }
}
