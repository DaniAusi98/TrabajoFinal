using Application.VisitaGrupal.Repositories;
using Core.Infraestructure.Repositories.Sql;
using Domain.VisitasGrupales.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Sql.VisitaGrupal
{
    internal sealed class RepositorioConfiguracionVisitasGrupalesGuiadas(MuseoDbContext context) : BaseRepository<ConfiguracionVisitasGrupalesGuiadas>(context), IRepositorioConfiguracionVisitasGrupalesGuiadas
    {
        public async Task<ConfiguracionVisitasGrupalesGuiadas> ObtenerConfiguracionAsync()
        {
            return await Repository.FirstOrDefaultAsync();
        }

        public async Task GuardarConfiguracionAsync(ConfiguracionVisitasGrupalesGuiadas configuracion)
        {
            var existing = await Repository.FirstOrDefaultAsync();
            if (existing == null)
            {
                await AddAsync(configuracion);
                return;
            }

            existing.Update(configuracion.MinGuiasParaCapacidadCompleta, configuracion.CapacidadPorGuia, configuracion.CapacidadMaximaPorTurno);
            Context.SaveChanges();
        }
    }
}
