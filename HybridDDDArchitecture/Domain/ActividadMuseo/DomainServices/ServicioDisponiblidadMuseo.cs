/*using Domain.DisponibilidadMuseo.Entities;
using Domain.DisponibilidadMuseo.Others.Helpers;

namespace Domain.DisponibilidadMuseo.DomainServices
{
    public class ServicioDisponibilidadMuseo
    {
        public bool EstaDisponible(ActividadMuseo nueva, IEnumerable<ActividadMuseo> existentes)
        {
            var solapadas = existentes
                .Where(e => SeSolapaEnTiempo.SolapaEnTiempo(nueva, e))
                .ToList();

            if (solapadas.Count == 0)
                return true;

            var validador = _factory.CrearPara(nueva.TipoActividad);

            return validador.EstaDisponible(nueva, solapadas);
        }
    }
}
*/
