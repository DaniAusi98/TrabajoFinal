namespace Domain.Enums.DisponibilidadMuseo
{
    /// <summary>
    /// Las enumeraciones deben ir definidas aqui
    /// </summary>

    public static class Enums
    {
        /// <summary>
        /// Ejemplo de enumeracion Dummy
        /// </summary>
        public enum DummyValues
        {
            value1,
            value2,
            value3,
        }

        public enum DatabaseType
        {
            MYSQL,
            MARIADB,
            SQLSERVER,
            MONGODB
        }

        public enum TipoActividad
        {
            VisitaGrupalGuiada,
            VisitaGrupalAutoguiada,
            Evento,
            ActividadExterna,
            ActividadEducativa,     
            MuestraExposicionTemporal,
            ActividadEspecial
        }

        public enum NivelBloqueo
        {
            BloqueoParcialMuseo,
            BloqueoTotalMuseo

        }

        public enum BloqueoTipo
        {
            DiaCompleto,
            IntervaloFechas,
            SlotHorario
        }

        public enum TipoEtapa
        {
            MontajeOArmado,
            Presentacion,
            DesmontajeODesarmado,
        }

        public enum EstadoActividad
        {
            
            Activa,
            Reprogramada,
            Cancelada
        }

        public enum AlcanceBloqueo
        {
            Total,
            Parcial,

        }

        public enum TipoBloqueoSala
        {
            Mantenimiento,
            EventoInstitucional,
            MontajeMuestraTemporal,
            DesmontajeMuestraTemporal,
            Otro
        }


        public enum MotivoCierreMuseo
        {
            MantenimientoyRefacciones,
            Feriado,
            DiasFestivos,
            EventosInstitucionales,
            Otro
        }
    }
}
