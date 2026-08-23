using Application.VisitaGrupal.Repositories;

using Core.Infraestructure.Repositories.Sql;
using Domain.VisitasGrupales.Entities.GrupalGuiada;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Sql.VisitaGrupal
{
    internal sealed class RepositorioVisitaGuiada(MuseoDbContext context)
        : BaseRepository<VisitaGrupalGuiada>(context), IRepositorioVisitaGuiada
    {
        public Task<VisitaGrupalGuiada?> FindByIdWithActividadAsync(string id)
        {
            return Repository
                .Include(v => v.Tematicas)
                .Include(v => v.TimeSlots)
                .Include(v => v.Salas)
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<List<VisitaGrupalGuiada>> GetAllGroupVisitAsync(
            DateTime fechaDesde,
            DateTime fechaHasta)
        {
            try
            {
                return await Repository
                    .Include(v => v.Tematicas)
                    .Include(v => v.TimeSlots)
                    .Include(v => v.Salas)
                    .Where(v =>
                        v.TimeSlots.Any(ts =>
                            ts.Inicio >= fechaDesde &&
                            ts.Fin <= fechaHasta))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public async Task<List<VisitaGrupalGuiada>> ObtenerPorUsuarioIdAsync(
            string usuarioId,
            DateTime fechaActual)
        {
            return await Repository
                .Include(v => v.Tematicas)
                .Include(v => v.TimeSlots)
                .Include(v => v.Salas)
                .Where(v =>
                    v.UsuarioVisitanteId == usuarioId &&
                    v.TimeSlots.Any(ts => ts.Inicio >= fechaActual))
                .ToListAsync();
        }
       
    }
}
