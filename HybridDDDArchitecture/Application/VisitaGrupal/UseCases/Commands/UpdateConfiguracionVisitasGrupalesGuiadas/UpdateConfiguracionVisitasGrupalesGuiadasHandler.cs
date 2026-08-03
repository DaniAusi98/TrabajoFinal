using Application.VisitaGrupal.ApplicationServices;
using Core.Application;
using Domain.VisitasGrupales.Options;

using MediatR;

namespace Application.VisitaGrupal.UseCases.Commands.UpdateConfiguracionVisitasGrupalesGuiadas
{
    internal sealed class UpdateConfiguracionVisitasGrupalesGuiadasHandler(IConfiguracionVisitasGrupalesGuiadasService service) : IRequestCommandHandler<UpdateConfiguracionVisitasGrupalesGuiadasCommand>
    {
        private readonly IConfiguracionVisitasGrupalesGuiadasService _service = service ?? throw new ArgumentNullException(nameof(service));

        public async Task Handle(UpdateConfiguracionVisitasGrupalesGuiadasCommand request, CancellationToken cancellationToken)
        {
            var options = new TurnosVisitasOptions
            {
                MinGuiasParaCapacidadCompleta = request.MinGuiasParaCapacidadCompleta,
                CapacidadPorGuia = request.CapacidadPorGuia,
                CapacidadMaximaPorTurno = request.CapacidadMaximaPorTurno
            };

            await _service.UpdateAsync(options);

           
        }

        
    }
}
