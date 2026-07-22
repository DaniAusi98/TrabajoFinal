/*using static Domain.DisponibilidadMuseo.Enums.Enums;

namespace Domain.DisponibilidadMuseo.DomainServices
{
    public class ValidadorDisponibilidadFactory
    {
        public IValidadorDisponibilidad Crear(TipoActividad tipoActividad)
        {
            return tipoActividad switch
            {
                TipoActividad.VisitaGrupal => new ValidadorDisponibilidadVisitaGrupal(
                    new List<IEspecificacionConflictoActividad>
                    {
                    new ConflictoVisitaGrupalConEventoSpecification(),
                    new ConflictoVisitaGrupalConVisitaGrupalSpecification()
                    }),

                TipoActividad.Evento => new ValidadorDisponibilidadEvento(
                    new List<IEspecificacionConflictoActividad>
                    {
                    new ConflictoEventoConVisitaGrupalSpecification(),
                    new ConflictoEventoConEventoSpecification()
                    }),

                _ => throw new NotSupportedException()
            };
        }
    } 
}
*/
