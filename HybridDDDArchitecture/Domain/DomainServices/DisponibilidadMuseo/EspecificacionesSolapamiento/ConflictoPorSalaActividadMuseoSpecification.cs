using Domain.Entities.DisponibilidadMuseo;

namespace Domain.DomainServices.DisponibilidadMuseo.EspecificacionesSolapamiento
{
    public class ConflictoPorSalaActividadMuseoSpecification : IEspecificacionConflictoActividad
    {
        public bool AplicaA(ActividadMuseo existente) => true;

        public  bool HayConflicto(ActividadMuseo nueva, ActividadMuseo existente)
        {
            return nueva.Salas.Any(ns => existente.Salas.Any(es => es.Id == ns.Id));
        }

        
    }
}
