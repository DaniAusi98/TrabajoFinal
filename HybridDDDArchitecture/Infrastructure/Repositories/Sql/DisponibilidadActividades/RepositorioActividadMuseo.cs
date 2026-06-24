
using Application.ActividadMuseo.Repositories;

using Core.Infraestructure.Repositories.Sql;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Sql.DisponibilidadActividades
{
    internal sealed class RepositorioActividadMuseo(MuseoDbContext context) : BaseRepository<Domain.Entities.DisponibilidadMuseo.ActividadMuseo>(context), IRepositorioActividadMuseo
    {
        public async Task<List<Domain.Entities.DisponibilidadMuseo.ActividadMuseo>> FindAllAsync(DateTime fechaDesde, DateTime fechaHasta)
        {
            
                return await Repository
                    .Include(a => a.Salas)
                    .Include(a => a.Recursos)
                     .ThenInclude(r => r.Recurso)
                    .Include(a => a.TimeSlots)

                    .Where(a =>
                        a.TimeSlots.Any(ts =>
                            ts.Inicio >= fechaDesde &&
                            ts.Fin <= fechaHasta))
                    .ToListAsync();
            
          
        }
    }
}
