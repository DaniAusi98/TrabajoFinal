using Domain.Reportes.Entities;
using Domain.VisitasGrupales.Entities.GrupalGuiada;

namespace Domain.Reportes.DomainServices
{
    public class ServicioReporteVisitasGuiadas
    {
        public static ReporteVisitasGuiadas ReporteVisitasGuiadas(
           IReadOnlyCollection<VisitaGrupalGuiada> visitasGrupalesGuiadas,int capacidadTotalDisponible)
        {
            int reservasTotales = visitasGrupalesGuiadas.Count;

            int cantidadVisitantesGuiadas = 0;

            int visitasGrupalesConfirmadas = 0;
            int visitasGrupalesCanceladas = 0;
            int visitasGrupalesPendientes = 0;
            int visitasGrupalesReprogramadas = 0;

            foreach (var visita in visitasGrupalesGuiadas)
            {
                cantidadVisitantesGuiadas += (int)visita.CantidadPersonas;

                if (visita.Estado == ActividadMuseo.Enums.Enums.EstadoActividad.Activa)
                    visitasGrupalesConfirmadas++;

                if (visita.Estado == ActividadMuseo.Enums.Enums.EstadoActividad.Cancelada)
                    visitasGrupalesCanceladas++;

                if (visita.EstadoConfirmacion == VisitasGrupales.Enums.Enums.EstadoConfirmacionVisita.PendienteConfirmar)
                    visitasGrupalesPendientes++;

                if (visita.Estado == ActividadMuseo.Enums.Enums.EstadoActividad.Reprogramada)
                    visitasGrupalesReprogramadas++;
            }

           

            int cantidadVisitantesTotales = cantidadVisitantesGuiadas;
            decimal tasaOcupacion = (capacidadTotalDisponible == 0) ? 0 : (decimal)cantidadVisitantesTotales / capacidadTotalDisponible * 100;

            return new ReporteVisitasGuiadas(
                reservasTotales,
                cantidadVisitantesTotales,
                visitasGrupalesConfirmadas,
                visitasGrupalesCanceladas,
                visitasGrupalesReprogramadas,
                visitasGrupalesPendientes,
                tasaOcupacion

            );
        }
    }
}
