using FC.Codeflix.Catalog.Application.Validation;
using FluentValidation;

namespace FC.Codeflix.Catalog.Application.UseCases.Category.DeleteCategory;

public class DeleteCategoryInputValidator : FluentValidationService<DeleteCategoryInput>
{
    public DeleteCategoryInputValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
