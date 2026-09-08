using Application.ActividadMuseo.Repositories;
using Core.Infraestructure.Repositories.Sql;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Sql.DisponibilidadActividades
{
    internal sealed class RepositorioActividadMuseo(MuseoDbContext context)
        : BaseRepository<Domain.ActividadMuseo.Entities.ActividadMuseo>(context), IRepositorioActividadMuseo
    {
        public async Task<List<Domain.ActividadMuseo.Entities.ActividadMuseo>> FindAllAsync(
            DateTime fechaDesde,
            DateTime fechaHasta)
        {
            if (fechaDesde == fechaHasta)
            {
                fechaHasta = fechaHasta.Date.AddDays(1).AddSeconds(-1); // Ajusta hasta el final del día
            }

            // ============================================================
            // CONSULTA ÚNICA OPTIMIZADA: Trae candidatos potenciales
            // ============================================================
            var actividadesEnRango = await Repository
                .Include(a => a.Salas)
                .Include(a => a.Recursos)
                    .ThenInclude(r => r.Recurso)
                .Include(a => a.Exceptions)
                .Where(a =>
                    // Caso A: Actividades fijas individuales que caen justo en esta ventana
                    (!string.IsNullOrEmpty(a.RRule) && a.Horario.Inicio < fechaHasta && a.Horario.Fin > fechaDesde) ||

                    // Caso B: Actividades recurrentes que YA empezaron en el pasado o empiezan ahora.
                    // Si su primera cita histórica empezó después de 'fechaHasta', es imposible que generen ocurrencias hoy.
                    (string.IsNullOrEmpty(a.RRule) && a.Horario.Inicio <= fechaHasta))
                .ToListAsync();

            // NOTA TÁCTICA: Si una actividad recurrente tiene una fecha límite de finalización (UNTIL) 
            // que quedó en el pasado (ej: terminó el año pasado), tu 'ActivityAvailabilityFactory' 
            // al ejecutar el Engine e invocar a 'ExpandRule' generará una lista vacía de slots (0 elementos),
            // descartándola automáticamente del cálculo en memoria de forma ultra rápida y segura.

            return actividadesEnRango;
        }
    }
}
