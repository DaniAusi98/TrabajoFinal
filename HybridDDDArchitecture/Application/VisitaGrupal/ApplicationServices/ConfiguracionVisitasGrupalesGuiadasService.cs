using Application.VisitaGrupal.Repositories;
using Domain.VisitasGrupales.Entities;
using Domain.VisitasGrupales.Options;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Application.VisitaGrupal.ApplicationServices
{
    internal sealed class ConfiguracionVisitasGrupalesGuiadasService(IRepositorioConfiguracionVisitasGrupalesGuiadas repo, IOptions<TurnosVisitasOptions> options) : IConfiguracionVisitasGrupalesGuiadasService
    {
        private readonly IRepositorioConfiguracionVisitasGrupalesGuiadas _repo = repo;
        private readonly TurnosVisitasOptions _fallback = options?.Value ?? new TurnosVisitasOptions();

        public async Task<TurnosVisitasOptions> GetAsync()
        {
            var config = await _repo.ObtenerConfiguracionAsync();
            if (config == null)
                return _fallback;

            return new TurnosVisitasOptions
            {
                MinGuiasParaCapacidadCompleta = config.MinGuiasParaCapacidadCompleta,
                CapacidadPorGuia = config.CapacidadPorGuia,
                CapacidadMaximaPorTurno = config.CapacidadMaximaPorTurno
            };
        }

        public async Task UpdateAsync(TurnosVisitasOptions options)
        {
            var config = new ConfiguracionVisitasGrupalesGuiadas(options.MinGuiasParaCapacidadCompleta, options.CapacidadPorGuia, options.CapacidadMaximaPorTurno);
            await _repo.GuardarConfiguracionAsync(config);
        }
    }
}
