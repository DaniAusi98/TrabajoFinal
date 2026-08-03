using Domain.VisitasGrupales.Options;
using System.Threading.Tasks;

namespace Application.VisitaGrupal.ApplicationServices
{
    internal sealed class ConfiguracionVisitasOptionsProvider : Domain.VisitasGrupales.Options.IConfiguracionVisitasOptionsProvider
    {
        private readonly IConfiguracionVisitasGrupalesGuiadasService _service;

        public ConfiguracionVisitasOptionsProvider(IConfiguracionVisitasGrupalesGuiadasService service)
        {
            _service = service;
        }

        public async Task<TurnosVisitasOptions> GetOptionsAsync()
        {
            return await _service.GetAsync();
        }
    }
}
