using Application.VisitaGrupal.ApplicationServices;
using Core.Application;
using Domain.VisitasGrupales.Options;

namespace Application.VisitaGrupal.UseCases.Commands.CreateConfiguracionVisitasGrupalesGuiadas
{
    internal sealed class CreateConfiguracionVisitasGrupalesGuiadasHandler(IConfiguracionVisitasGrupalesGuiadasService service) : IRequestCommandHandler<CreateConfiguracionVisitasGrupalesGuiadasCommand, int>
    {
        private readonly IConfiguracionVisitasGrupalesGuiadasService _service = service ?? throw new ArgumentNullException(nameof(service));

        public async Task<int> Handle(CreateConfiguracionVisitasGrupalesGuiadasCommand request, CancellationToken cancellationToken)
        {
            var options = new TurnosVisitasOptions
            {
                MinGuiasParaCapacidadCompleta = request.MinGuiasParaCapacidadCompleta,
                CapacidadPorGuia = request.CapacidadPorGuia,
                CapacidadMaximaPorTurno = request.CapacidadMaximaPorTurno
            };

            await _service.UpdateAsync(options);

            return 0;
        }
    }
}
