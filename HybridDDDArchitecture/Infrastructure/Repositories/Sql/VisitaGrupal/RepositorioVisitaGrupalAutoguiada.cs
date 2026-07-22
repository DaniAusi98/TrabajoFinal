using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Application.VisitaGrupal.Repositories;

using Core.Infraestructure.Repositories.Sql;

using Domain.VisitasGrupales.Entities;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Sql.VisitaGrupal
{
    internal  sealed class RepositorioVisitaGrupalAutoguiada(MuseoDbContext context):BaseRepository<VisitaGrupalAutoguiada>(context),IRepositorioVisitaGrupalAutoguiada
    {
        public async Task<List<VisitaGrupalAutoguiada>> GetAllGroupVisitAuAsync(
            DateTime fechaDesde,
            DateTime fechaHasta)
        {
            try
            {
                return await Repository
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
    }
}
