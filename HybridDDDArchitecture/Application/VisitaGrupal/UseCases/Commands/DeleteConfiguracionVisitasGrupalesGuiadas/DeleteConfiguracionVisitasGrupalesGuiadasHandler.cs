using Application.VisitaGrupal.ApplicationServices;
using Core.Application;
using Domain.VisitasGrupales.Options;

using MediatR;

namespace Application.VisitaGrupal.UseCases.Commands.DeleteConfiguracionVisitasGrupalesGuiadas
{
    internal sealed class DeleteConfiguracionVisitasGrupalesGuiadasHandler(IConfiguracionVisitasGrupalesGuiadasService service) : IRequestCommandHandler<DeleteConfiguracionVisitasGrupalesGuiadasCommand,Unit>
    {
        private readonly IConfiguracionVisitasGrupalesGuiadasService _service = service ?? throw new ArgumentNullException(nameof(service));

        public async Task<Unit> Handle(DeleteConfiguracionVisitasGrupalesGuiadasCommand request, CancellationToken cancellationToken)
        {
            var defaultOptions = new TurnosVisitasOptions();
            await _service.UpdateAsync(defaultOptions);
            return Unit.Value;
        }
    }
}
