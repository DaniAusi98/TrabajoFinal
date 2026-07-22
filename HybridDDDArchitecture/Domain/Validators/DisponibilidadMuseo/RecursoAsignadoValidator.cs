using Core.Domain.Validators;

using Domain.ActividadMuseo.Constants;
using Domain.ActividadMuseo.Entities;

using FluentValidation;


namespace Domain.Validators.DisponibilidadMuseo
{
    public class RecursoAsignadoValidator: EntityValidator<RecursoAsignado>
    {
        // Reglas de validación para ActividadRecurso
        public RecursoAsignadoValidator()
        {
            // Reglas de validación para ActividadRecurso
            RuleFor(ar => ar.CantidadAsignada)
                .GreaterThan(0)
                .WithMessage(DomainConstants.GREATER_THAN_ZERO);
            RuleFor(ar => ar.RecursoId)
                .GreaterThan(0)
                .WithMessage(DomainConstants.GREATER_THAN_ZERO);
           
        }
    }
}
