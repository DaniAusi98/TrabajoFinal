using Core.Application.Repositories;
using Domain.Entities.DisponibilidadMuseo;

namespace Application.ActividadMuseo.Repositories
{
    public interface IRepositorioActividadMuseo:IRepository<Domain.Entities.DisponibilidadMuseo.ActividadMuseo>
    {
        public Task<List<Domain.Entities.DisponibilidadMuseo.ActividadMuseo>> FindAllAsync(DateTime fechaDesde, DateTime fechaHasta);

    }
}
