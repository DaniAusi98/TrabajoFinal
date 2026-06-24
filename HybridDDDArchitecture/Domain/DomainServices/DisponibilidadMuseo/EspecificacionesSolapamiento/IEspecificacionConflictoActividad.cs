using Domain.Entities.DisponibilidadMuseo;
using Domain.Entities.DisponibilidadMuseo;

namespace Domain.DomainServices.DisponibilidadMuseo.EspecificacionesSolapamiento
{
    public interface IEspecificacionConflictoActividad
    {
        bool AplicaA(ActividadMuseo existente);
        public bool HayConflicto(ActividadMuseo nueva, ActividadMuseo existente);
    }
}
