using Domain.ActividadMuseo.DomainServices.EspecificacionesSolapamiento;

using static Domain.ActividadMuseo.Enums.Enums;

namespace Domain.ActividadMuseo.DomainServices.EspecificacionesSolapamiento.Espec.VisitaGrupal
{
    public class VisitaGrupalAutoguiadaVisitaGrupalGuiadaSpecification : IEspecificacionConflictoActividad
    {
        public bool AplicaA(Entities.ActividadMuseo existente)
        => existente.TipoActividad == TipoActividad.VisitaGrupalGuiada;

     
        public bool HayConflicto(Entities.ActividadMuseo nueva, Entities.ActividadMuseo existente)
        {
            
                return  nueva.TipoActividad == TipoActividad.VisitaGrupalAutoguiada
                     && existente.TipoActividad == TipoActividad.VisitaGrupalGuiada;
        }

      
    }
}
