using Domain.DomainServices.DisponibilidadMuseo.EspecificacionesSolapamiento;
using Domain.Entities.DisponibilidadMuseo;

using static Domain.Enums.DisponibilidadMuseo.Enums;

namespace Domain.DomainServices.DisponibilidadMuseo.EspecificacionesSolapamiento.Espec.VisitaGrupal
{
    public class VisitaGrupalAutoguiadaVisitaGrupalGuiadaSpecification : IEspecificacionConflictoActividad
    {
        public bool AplicaA(ActividadMuseo existente)
        => existente.TipoActividad == TipoActividad.VisitaGrupalGuiada;

     
        public bool HayConflicto(ActividadMuseo nueva, ActividadMuseo existente)
        {
            
                return  nueva.TipoActividad == TipoActividad.VisitaGrupalAutoguiada
                     && existente.TipoActividad == TipoActividad.VisitaGrupalGuiada;
        }

      
    }
}
