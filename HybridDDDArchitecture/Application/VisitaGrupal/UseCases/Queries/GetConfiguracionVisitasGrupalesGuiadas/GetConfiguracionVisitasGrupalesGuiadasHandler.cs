using Application.VisitaGrupal.ApplicationServices;
using Application.VisitaGrupal.DataTransferObjets;
using Core.Application;

namespace Application.VisitaGrupal.UseCases.Queries.GetConfiguracionVisitasGrupalesGuiadas
{
    internal sealed class GetConfiguracionVisitasGrupalesGuiadasHandler(IConfiguracionVisitasGrupalesGuiadasService service) : IRequestQueryHandler<GetConfiguracionVisitasGrupalesGuiadasQuery, ConfiguracionVisitasGrupalesGuiadasDto>
    {
        private readonly IConfiguracionVisitasGrupalesGuiadasService _service = service ?? throw new ArgumentNullException(nameof(service));

        public async Task<ConfiguracionVisitasGrupalesGuiadasDto> Handle(GetConfiguracionVisitasGrupalesGuiadasQuery request, CancellationToken cancellationToken)
        {
            var opts = await _service.GetAsync();
            return new ConfiguracionVisitasGrupalesGuiadasDto
            {
                MinGuiasParaCapacidadCompleta = opts.MinGuiasParaCapacidadCompleta,
                CapacidadPorGuia = opts.CapacidadPorGuia,
                CapacidadMaximaPorTurno = opts.CapacidadMaximaPorTurno
            };
        }
    }
}
