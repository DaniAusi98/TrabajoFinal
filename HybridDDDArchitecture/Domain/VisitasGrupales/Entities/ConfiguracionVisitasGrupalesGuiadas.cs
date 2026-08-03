using Core.Domain.Entities;

namespace Domain.VisitasGrupales.Entities
{
    public class ConfiguracionVisitasGrupalesGuiadas : DomainEntity<int>
    {
        public int MinGuiasParaCapacidadCompleta { get; private set; }
        public int CapacidadPorGuia { get; private set; }
        public int CapacidadMaximaPorTurno { get; private set; }

        protected ConfiguracionVisitasGrupalesGuiadas() { }

        public ConfiguracionVisitasGrupalesGuiadas(int minGuias, int capacidadPorGuia, int capacidadMaxima)
        {
            MinGuiasParaCapacidadCompleta = minGuias;
            CapacidadPorGuia = capacidadPorGuia;
            CapacidadMaximaPorTurno = capacidadMaxima;
        }

        public void Update(int minGuias, int capacidadPorGuia, int capacidadMaxima)
        {
            MinGuiasParaCapacidadCompleta = minGuias;
            CapacidadPorGuia = capacidadPorGuia;
            CapacidadMaximaPorTurno = capacidadMaxima;
        }
    }
}
