
using Application.ActividadMuseo.Repositories;

using Core.Infraestructure.Repositories.Sql;

using Domain.ActividadMuseo.Entities;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Sql.DisponibilidadActividades
{
    internal sealed class RepositorioActividadMuseo(MuseoDbContext context) : BaseRepository<Domain.ActividadMuseo.Entities.ActividadMuseo>(context), IRepositorioActividadMuseo
    {
        public async Task<List<Domain.ActividadMuseo.Entities.ActividadMuseo>> FindAllAsync(
    DateTime fechaDesde,
    DateTime fechaHasta)
        {
            Console.WriteLine("========================================");
            Console.WriteLine("[RepositorioActividadMuseo] CONSULTANDO");
            Console.WriteLine($"Desde: {fechaDesde:O}");
            Console.WriteLine($"Hasta: {fechaHasta:O}");

            var actividades = await Repository
                .Include(a => a.Salas)
                .Include(a => a.Recursos)
                    .ThenInclude(r => r.Recurso)
                .Include(a => a.Exceptions)
                .Where(a =>
                    a.Horario.Inicio < fechaHasta &&
                    a.Horario.Fin > fechaDesde)
                .ToListAsync();

            Console.WriteLine(
                $"[RepositorioActividadMuseo] Encontradas: {actividades.Count}");

            foreach (var actividad in actividades)
            {
                Console.WriteLine(
                    $"Actividad {actividad.Id} | " +
                    $"Inicio: {actividad.Horario.Inicio:O} | " +
                    $"Fin: {actividad.Horario.Fin:O} | " +
                    $"Estado: {actividad.Estado} | " +
                    $"Tiene recurrencia: {actividad.Recurrence != null}");
            }

            return actividades;
        }
    }
}
