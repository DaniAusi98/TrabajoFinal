namespace Domain.Common.Others
{
    public static class SeSolapaEnTiempo
    {
        public static bool SolapaEnTiempo(Domain.ActividadMuseo.Entities.ActividadMuseo nuevaActividad, Domain.ActividadMuseo.Entities.ActividadMuseo actividadExistente)
        {
            if (nuevaActividad.Horario.SeSolapaCon(actividadExistente.Horario)){return true;}
            return false;
        }
    }
}
