using Domain.Reportes.Entities;
using Domain.VisitasGrupales.Entities;
using Domain.VisitasGrupales.Entities.GrupalGuiada;


namespace Domain.Reportes.DomainServices
{
    public class ServicioReporteVisitasGrupales
    {
        public static ReporteGeneralVisitasGrupales GenerarReporteGeneralVisitasGrupales(
            IReadOnlyCollection<VisitaGrupalAutoguiada> visitaGrupalAutoguiadas,
            IReadOnlyCollection<VisitaGrupalGuiada> visitaGrupalGuiadas)
        {
            int reservasTotales =
                visitaGrupalAutoguiadas.Count + visitaGrupalGuiadas.Count;

            int cantidadVisitantesGuiadas = 0;
            int cantidadVisitantesAutoguiadas = 0;

            int visitasGrupalesConfirmadas = 0;
            int visitasGrupalesCanceladas = 0;
            int visitasGrupalesPendientes = 0;
            int visitasGrupalesReprogramadas = 0;

            foreach (var visita in visitaGrupalAutoguiadas)
            {
                cantidadVisitantesAutoguiadas += (int)visita.CantidadPersonas;

                if (visita.Estado == ActividadMuseo.Enums.Enums.EstadoActividad.Activa)
                    visitasGrupalesConfirmadas++;

                if (visita.Estado == ActividadMuseo.Enums.Enums.EstadoActividad.Cancelada)
                    visitasGrupalesCanceladas++;

                if (visita.EstadoConfirmacion == VisitasGrupales.Enums.Enums.EstadoConfirmacionVisita.PendienteConfirmar)
                    visitasGrupalesPendientes++;

                if (visita.Estado == ActividadMuseo.Enums.Enums.EstadoActividad.Reprogramada)
                    visitasGrupalesReprogramadas++;
            }

            foreach (var visita in visitaGrupalGuiadas)
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

            int cantidadVisitantesTotales =
                cantidadVisitantesAutoguiadas + cantidadVisitantesGuiadas;

            return new ReporteGeneralVisitasGrupales(
                reservasTotales,
                cantidadVisitantesTotales,
                visitasGrupalesConfirmadas,
                visitasGrupalesCanceladas,
                visitasGrupalesReprogramadas,
                visitasGrupalesPendientes
            );
        }
    }
}
