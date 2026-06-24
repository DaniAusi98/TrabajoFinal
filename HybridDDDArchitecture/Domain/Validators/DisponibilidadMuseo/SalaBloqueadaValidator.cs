
using Core.Domain.Validators;

using Domain.Entities.DisponibilidadMuseo;

using FluentValidation;

namespace Domain.Validators.DisponibilidadMuseo
{
    public class SalaBloqueadaValidator:EntityValidator<SalaBloqueadaMuseo>
    {
        public SalaBloqueadaValidator()
        {
            RuleFor(x => x.FechaDesde)
                .NotEmpty().NotNull().WithMessage("La fecha de inicio es obligatoria.");
            RuleFor(x => x.FechaHasta)
                .GreaterThanOrEqualTo(x => x.FechaDesde)
                .WithMessage("La fecha de fin debe ser mayor o igual a la fecha de inicio.");
            RuleFor(x => x.SalasBloqueadas)
                .NotEmpty().WithMessage("Debe especificar al menos una sala bloqueada.");
            RuleFor(x => x.Motivo)
                .IsInEnum().WithMessage("El motivo de bloqueo debe ser un valor válido del enum TipoBloqueoSala.");
            RuleFor(x => x.Observaciones)
                .MaximumLength(500).WithMessage("Las observaciones no pueden exceder los 500 caracteres.");
        }
    }
}
