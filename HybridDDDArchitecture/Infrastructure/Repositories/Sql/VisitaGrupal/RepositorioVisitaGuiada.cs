using System.ComponentModel;
using System.Linq.Expressions;

using Application.VisitaGrupal.Repositories;

using Core.Infraestructure.Repositories.Sql;

using Domain.Entities.VisitasGrupalesMuseo;

using Microsoft.EntityFrameworkCore;

using static Domain.Enums.VisitasGrupalesEnums.Enums;

namespace Infrastructure.Repositories.Sql.VisitaGrupal
{
    internal sealed class RepositorioVisitaGuiada(MuseoDbContext context) : BaseRepository<VisitaGrupalGuiada>(context), IRepositorioVisitaGuiada
    {
        public Task<VisitaGrupalGuiada> FindByIdWithActividadAsync(int id)
        {
            return Repository
                .Include(v => v.Tematicas)
                .Include(v => v.ActividadMuseo)
                    .ThenInclude(a => a.TimeSlots)
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<List<VisitaGrupalGuiada>> ObtenerConActividadAsync(
            DateTime fechaDesde,
            DateTime fechaHasta)
        {
            try
            {
                return await Repository
                    .Include(v => v.Tematicas)
                    .Include(v => v.ActividadMuseo)
                        .ThenInclude(a => a.TimeSlots)

                    .Where(v =>
                        v.ActividadMuseo.TimeSlots.Any(ts =>
                            ts.Inicio >= fechaDesde &&
                            ts.Fin <= fechaHasta))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

                throw;
            }
        }
        public async Task<List<VisitaGrupalGuiada>>ObtenerPorUsuarioIdAsync(
             string usuarioId,
             DateTime fechaActual
        )
        {
            return await Repository
                .Include(v => v.Tematicas)
                .Include(v => v.ActividadMuseo)
                    .ThenInclude(a => a.TimeSlots)
                .Where(v =>
                    v.UsuarioVisitanteId == usuarioId &&
                    v.ActividadMuseo.TimeSlots.Any(
                        ts => ts.Inicio >= fechaActual))
                .ToListAsync();
        }
    }
}
