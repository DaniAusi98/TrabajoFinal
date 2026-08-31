using Core.Domain.Entities;
using Domain.Common.Exceptions;
namespace Domain.ActividadMuseo.Entities
{
    public class ActividadException:DomainEntity<string>
    {

        public string ActividadMuseoId { get; private set; }

        public DateOnly Date { get; private set; }

        private ActividadException() { }

        public ActividadException(
            string actividadMuseoId,
            DateOnly date)
        {
            if (string.IsNullOrWhiteSpace(actividadMuseoId))
                throw new DomainException(
                    "La excepción debe estar asociada a una actividad.");

            Id = Guid.NewGuid().ToString();

            ActividadMuseoId = actividadMuseoId;

            Date = date;
        }
    }
}