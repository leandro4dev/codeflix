using FC.Codeflix.Catalog.Application.Validation;
using FluentValidation;

namespace FC.Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;

public class UpdateCategoryInputValidator : FluentValidationService<UpdateCategoryInput>
{
    public UpdateCategoryInputValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
