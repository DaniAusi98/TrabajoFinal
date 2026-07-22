namespace Domain.VisitasGrupales.Enums
{
    public class Enums
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



        public enum NivelEducativo
        {
            Inicial,
            Primario,
            Secundario,
            Superior


        }

        public enum EstadoConfirmacionVisita
        {
            PendienteConfirmar,
            Confirmada,
            

        }
        public enum EstadoTurno
        {
            Disponible,
            NoDisponible,
            Completo,
            Cancelado
        }






    }
}
