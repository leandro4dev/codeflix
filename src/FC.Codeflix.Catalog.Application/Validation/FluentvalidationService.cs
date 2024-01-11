using FluentValidation;

namespace FC.Codeflix.Catalog.Application.Validation;

public class FluentValidationService<TValidationInput> : AbstractValidator<TValidationInput>
{
    public FluentValidationService()
    {
        ValidatorOptions.Global.LanguageManager.Enabled = false;
    }
}
