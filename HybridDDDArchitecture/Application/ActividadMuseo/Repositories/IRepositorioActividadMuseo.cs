using Core.Application.Repositories;

namespace Application.ActividadMuseo.Repositories
{
    public interface IRepositorioActividadMuseo:IRepository<Domain.ActividadMuseo.Entities.Actividad>
    {
        public Task<List<Domain.ActividadMuseo.Entities.Actividad>> FindAllAsync(DateTime fechaDesde, DateTime fechaHasta);

    }
}
