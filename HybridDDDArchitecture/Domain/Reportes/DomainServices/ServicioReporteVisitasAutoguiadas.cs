using Domain.Reportes.Entities;
using Domain.VisitasGrupales.Entities;

namespace Domain.Reportes.DomainServices
{
    public class ServicioReporteVisitasAutoguiadas
    {
        public static ReporteVisitasAutoguiadas GenerarReporteVisitasAutoguiadas(
          IReadOnlyCollection<VisitaGrupalAutoguiada> visitasGrupalesAutoguiadas,int capacidadTotalDisponible)
        {
            int reservasTotales = visitasGrupalesAutoguiadas.Count;

            int cantidadVisitantesAutoguiadas = 0;

            int visitasAutoguiadasConfirmadas = 0;
            int visitasAutoguiadasCanceladas = 0;
            int visitasAutoguiadasPendientes = 0;
            int visitasAutoguiadasReprogramadas = 0;

            foreach (var visita in visitasGrupalesAutoguiadas)
            {
                cantidadVisitantesAutoguiadas += (int)visita.CantidadPersonas;
                if (visita.Estado == ActividadMuseo.Enums.Enums.EstadoActividad.Activa)
                    visitasAutoguiadasConfirmadas++;

                if (visita.Estado == ActividadMuseo.Enums.Enums.EstadoActividad.Cancelada)
                    visitasAutoguiadasCanceladas++;

                if (visita.EstadoConfirmacion == VisitasGrupales.Enums.Enums.EstadoConfirmacionVisita.PendienteConfirmar)
                    visitasAutoguiadasPendientes++;

                if (visita.Estado == ActividadMuseo.Enums.Enums.EstadoActividad.Reprogramada)
                    visitasAutoguiadasReprogramadas++;
            }



            int cantidadVisitantesTotales = cantidadVisitantesAutoguiadas;
            decimal tasaOcupacion = (capacidadTotalDisponible == 0) ? 0 : (decimal)cantidadVisitantesTotales / capacidadTotalDisponible * 100;


            return new ReporteVisitasAutoguiadas(
                reservasTotales,
                cantidadVisitantesTotales,
                visitasAutoguiadasConfirmadas,
                visitasAutoguiadasCanceladas,
                visitasAutoguiadasReprogramadas,
                visitasAutoguiadasPendientes,
                tasaOcupacion
            );
        }
    }
}
