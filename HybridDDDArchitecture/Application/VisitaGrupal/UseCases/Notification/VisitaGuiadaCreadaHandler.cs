using Application.ApplicationMuseo.DomainEvents;
using MediatR;

namespace Application.VisitaGrupal.UseCases.Notification
{
    internal class VisitaGuiadaCreadaHandler : INotificationHandler<DummyEntityCreated>
    {
        public Task Handle(DummyEntityCreated notification, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
