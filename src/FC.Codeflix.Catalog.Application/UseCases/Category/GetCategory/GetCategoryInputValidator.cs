using FluentValidation;

namespace FC.Codeflix.Catalog.Application.UseCases.Category.GetCategory;

public class GetCategoryInputValidator : AbstractValidator<GetCategoryInput>
{
    public GetCategoryInputValidator()
    {
        ValidatorOptions.Global.LanguageManager.Enabled = false;
        RuleFor(x => x.Id).NotEmpty();
    }
}
