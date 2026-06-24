using Core.Domain.Validators;

using Domain.Entities.DisponibilidadMuseo;

using FluentValidation;

namespace Domain.Validators.DisponibilidadMuseo
{
    public class ActividadMuseoValidator : EntityValidator<ActividadMuseo>
    {
        public ActividadMuseoValidator()
        {
            RuleFor(x => x.TipoActividad)
                .IsInEnum();

            RuleFor(x => x.Estado)
                .IsInEnum();

            RuleFor(x => x.CantidadPersonas)
                .GreaterThan(0)
                .When(x => x.CantidadPersonas is not null);

            RuleFor(x => x.TimeSlots)
                .NotEmpty()
                .WithMessage("Debe haber al menos un horario asignado.");

            RuleForEach(x => x.TimeSlots)
                .NotNull()
                .WithMessage("El TimeSlot no puede ser nulo.");

            RuleForEach(x => x.Salas)
                .NotNull()
                .WithMessage("La sala no puede ser nula.");

            RuleFor(x => x.Salas)
                .Must(salas => salas.DistinctBy(s => s.Id).Count() == salas.Count)
                .WithMessage("No puede haber salas repetidas.");

            RuleForEach(x => x.Recursos)
                .NotNull()
                .WithMessage("El recurso asignado no puede ser nulo.");

            RuleFor(x => x.Recursos)
                .Must(recursos => recursos.DistinctBy(r => r.Id).Count() == recursos.Count)
                .WithMessage("No puede haber recursos repetidos.");

            RuleForEach(x => x.Recursos)
                .SetValidator(new RecursoAsignadoValidator());
        }
    }
}
