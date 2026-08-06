using Domain.ActividadMuseo.Entities;
using Domain.ActividadMuseo.ValueObjets;
using Domain.VisitasGrupales.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.VisitasGrupales.Factories
{
    public static class ConfiguracionVisitasAutoguiadasFactory
    {
        public static ConfiguracionHorarioAutoguiada CrearConfiguracionPorDefecto(CalendarioMuseo calendario)
        {
            var horario = new Horario(
                inicio: new TimeOnly(9, 0),
                fin: new TimeOnly(18, 0)
            );
            var diasLaborales = new DiasLaboralesMuseo(
               new[]
               {
                    DayOfWeek.Monday,
                    DayOfWeek.Tuesday,
                    DayOfWeek.Wednesday,
                    DayOfWeek.Thursday,
                    DayOfWeek.Friday,

               });

            if (calendario == null)
                throw new ArgumentNullException(nameof(calendario));
            var diasDisponibles = new DiasLaboralesMuseo(calendario.DiasApertura.Dias);
            var configuracion = new ConfiguracionHorarioAutoguiada(
                capacidadMaximaPorGrupo: 25,
                diasDisponibles: diasLaborales,
                horarioDisponibleVisitaAutoguiadas:horario, // Asumiendo un horario por defecto
                calendario: calendario
            );
            // Validar explícitamente contra el calendario
            configuracion.ActualizarDiasDisponibles(diasDisponibles, calendario);
            return configuracion;
        }
    }
}
