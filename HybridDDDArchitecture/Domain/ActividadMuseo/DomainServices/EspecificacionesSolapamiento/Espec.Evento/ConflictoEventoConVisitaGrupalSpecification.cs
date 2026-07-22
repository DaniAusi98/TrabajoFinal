using Domain.ActividadMuseo.Constants;
using Domain.ActividadMuseo.DomainServices.EspecificacionesSolapamiento;

using static Domain.ActividadMuseo.Enums.Enums;

namespace Domain.ActividadMuseo.DomainServices.EspecificacionesSolapamiento.Espec.Evento 
{     

    public class ConflictoEventoConVisitaGrupalSpecification : IEspecificacionConflictoActividad
    {
        public bool AplicaA(Entities.Actividad existente)
            => existente.TipoActividad == TipoActividad.VisitaGrupalGuiada ||
               existente.TipoActividad == TipoActividad.VisitaGrupalAutoguiada;

        public bool HayConflicto(Entities.Actividad nueva, Entities.Actividad existente)
        {

            // 1. la nueva tiene que ser una actividad "tipo evento"
            var nuevaEsActividadConflictiva =
               
                nueva.TipoActividad != TipoActividad.VisitaGrupalGuiada &&
                nueva.TipoActividad != TipoActividad.VisitaGrupalAutoguiada;

            if (!nuevaEsActividadConflictiva)
                return false;

            var nuevaEstaEnHall = nueva.Salas.Any(s =>
                s.CodigoSala == DomainConstants.CODIGO_HALLINGRESO_PRINCIPAL && s.Id==DomainConstants.ID_HALLINGRESO_PRINCIPAL);

            if (!nuevaEstaEnHall)
                return false;

            return true;
        }
    }
}
