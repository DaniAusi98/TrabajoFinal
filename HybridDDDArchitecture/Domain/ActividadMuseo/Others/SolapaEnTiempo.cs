namespace Domain.ActividadMuseo.Others
{
    public static class SeSolapaEnTiempo
    {
        public static bool SolapaEnTiempo(Entities.ActividadMuseo nuevaActividad, Entities.ActividadMuseo actividadExistente)
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
