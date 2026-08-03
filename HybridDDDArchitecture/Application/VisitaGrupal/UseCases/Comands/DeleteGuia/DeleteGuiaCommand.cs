using Core.Application;

using MediatR;

namespace Application.VisitaGrupal.UseCases.Comands.DeleteGuia
{
    public class DeleteGuiaCommand : IRequestCommand<Unit>
    {
        public int GuiaId { get; set; }
    }
}
