using Core.Domain.Validators;

using Domain.Constants;
using Domain.Entities.RecursoMuseo;

using FluentValidation;

namespace Domain.Validators.RecursoMuseoValidators
{
    public class RecursoValidator : EntityValidator<Recurso>
    {
        public RecursoValidator()
        {

            RuleFor(x => x.NombreRecurso).NotNull().NotEmpty().WithMessage(DomainConstants.NOTNULL_OR_EMPTY);
            RuleFor(x => x.Descripcion).NotNull().NotEmpty().WithMessage(DomainConstants.NOTNULL_OR_EMPTY_OR_WHITESPACE);
            RuleFor(x => x.CantidadTotal).GreaterThanOrEqualTo(0).WithMessage(DomainConstants.GREATER_THAN_ZERO);
        }


    }
}
