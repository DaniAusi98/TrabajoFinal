using Domain.Constants.DisponibilidadMuseo;
using Domain.DomainServices.DisponibilidadMuseo.EspecificacionesSolapamiento;
using Domain.Entities.DisponibilidadMuseo;

using static Domain.Enums.DisponibilidadMuseo.Enums;

namespace Domain.DomainServices.DisponibilidadMuseo.EspecificacionesSolapamiento.Espec.Evento 
{     

    public class ConflictoEventoConVisitaGrupalSpecification : IEspecificacionConflictoActividad
    {
        public bool AplicaA(ActividadMuseo existente)
            => existente.TipoActividad == TipoActividad.VisitaGrupalGuiada ||
               existente.TipoActividad == TipoActividad.VisitaGrupalAutoguiada;

        public bool HayConflicto(ActividadMuseo nueva, ActividadMuseo existente)
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