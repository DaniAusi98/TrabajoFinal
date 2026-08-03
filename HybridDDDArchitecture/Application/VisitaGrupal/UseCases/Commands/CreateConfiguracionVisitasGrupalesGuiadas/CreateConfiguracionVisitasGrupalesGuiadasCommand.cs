using MediatR;

namespace Application.VisitaGrupal.UseCases.Commands.CreateConfiguracionVisitasGrupalesGuiadas
{
    public class CreateConfiguracionVisitasGrupalesGuiadasCommand : IRequest<int>
    {
        public int MinGuiasParaCapacidadCompleta { get; set; }
        public int CapacidadPorGuia { get; set; }
        public int CapacidadMaximaPorTurno { get; set; }
    }
}
