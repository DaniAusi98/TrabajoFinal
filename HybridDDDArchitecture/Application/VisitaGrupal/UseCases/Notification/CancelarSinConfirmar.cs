using Application.VisitaGrupal.DomainEvents;
using Application.VisitaGrupal.Repositories;
using MediatR;

namespace Application.VisitaGrupal.UseCases.Notification
{
    internal class CancelarSinConfirmar(IRepositorioVisitaGuiada repositorioVisitaGuiada) : INotificationHandler<VisitaGuiadaCreated>
    {
        private readonly IRepositorioVisitaGuiada _repositorioVisitaGuiada = repositorioVisitaGuiada ?? throw new ArgumentNullException(nameof(repositorioVisitaGuiada));
        public Task Handle(VisitaGuiadaCreated notification, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
