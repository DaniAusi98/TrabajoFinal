using Core.Domain.Validators;

using Domain.Common.Constants;
using Domain.RecursoMuseo.Entities;

using FluentValidation;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Validators.RecursoMuseoValidators
{
    public class SalaValidator : EntityValidator<Sala>
    {
        public SalaValidator()
        {
            //Las reglas de negocio deben ir definidas aca
            RuleFor(x => x.Nombre).NotNull().NotEmpty().WithMessage(DomainConstants.NOTNULL_OR_EMPTY);
            RuleFor(x => x.Capacidad).GreaterThan(0).WithMessage(DomainConstants.GREATER_THAN_ZERO);
            RuleFor(x => x.Ubicacion).IsInEnum().WithMessage(DomainConstants.UBICACION_VALIDA);

        }
    }
}
