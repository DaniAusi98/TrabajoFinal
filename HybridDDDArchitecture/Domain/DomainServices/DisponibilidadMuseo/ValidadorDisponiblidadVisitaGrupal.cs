/*using Domain.DisponibilidadMuseo.DomainServices.EspecificacionesSolapamiento;
using Domain.DisponibilidadMuseo.Entities;

namespace Domain.DisponibilidadMuseo.DomainServices
{
    public class ValidadorDisponibilidadVisitaGrupal : IValidadorDisponibilidad
    {
        private readonly IReadOnlyCollection<IEspecificacionConflictoActividad> _specs;

        public ValidadorDisponibilidadVisitaGrupal(
            IReadOnlyCollection<IEspecificacionConflictoActividad> specs)
        {
            _specs = specs;
        }

        public bool EstaDisponible(ActividadMuseo nueva, IReadOnlyCollection<ActividadMuseo> solapadas)
        {
            foreach (var existente in solapadas)
            {
                foreach (var spec in _specs)
                {
                    if (!spec.AplicaA(existente))
                        continue;

                    if (spec.HayConflicto(nueva, existente))
                        return false;
                }
            }

            return true;
        }
    }
}
*/
