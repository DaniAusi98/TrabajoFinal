using Core.Application.Repositories;
using Domain.Common.Entities;

namespace Application.Common.Repositories
{
    public interface ILocalidadRepository : IRepository<Localidad>
    {
        Task<List<Localidad>> GetByDepartamentoIdAsync(string departamentoId,CancellationToken cancellationToken = default);
        Task<List<Localidad>> GetByIdsAsync(IEnumerable<string> ids);

    }
}
