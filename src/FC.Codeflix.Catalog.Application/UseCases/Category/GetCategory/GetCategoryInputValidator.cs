using FC.Codeflix.Catalog.Application.Validation;
using FluentValidation;

namespace FC.Codeflix.Catalog.Application.UseCases.Category.GetCategory;

public class GetCategoryInputValidator : FluentValidationService<GetCategoryInput>
{
    public GetCategoryInputValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
