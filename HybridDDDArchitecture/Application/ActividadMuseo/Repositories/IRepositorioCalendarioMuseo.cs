using Core.Application.Repositories;
using Domain.ActividadMuseo.Entities;

namespace Application.ActividadMuseo.Repositories
{
    public interface IRepositorioCalendarioMuseo : IRepository<CalendarioMuseo>
    {
        Task<CalendarioMuseo?> ObtenerCalendarioActivoAsync();
    }
}
