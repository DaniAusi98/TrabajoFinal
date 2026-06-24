using Domain.Entities.DisponibilidadMuseo;

namespace Domain.Others.Helpers.DisponibilidadMuseoHelpers
{
    public static class SeSolapaEnTiempo
    {
        public static bool SolapaEnTiempo(ActividadMuseo nuevaActividad, ActividadMuseo actividadExistente)
        {
            
                    
            foreach (var slotNuevo in nuevaActividad.TimeSlots)
            {
                foreach (var slotExistente in actividadExistente.TimeSlots)
                {
                    if (slotNuevo.Inicio.Date == slotExistente.Inicio.Date &&
                        slotNuevo.SeSolapaCon(slotExistente))
                        // Hay solapamiento
                        return false;
                }
            }
                    
           //No hay solapamiento
            return true;
        }
    }
}
