
using Core.Application.Repositories;
using Domain.VisitasGrupales.Entities;

namespace Application.VisitaGrupal.Repositories
{
    public interface IRepositorioVisitaGrupalAutoguiada:IRepository<VisitaGrupalAutoguiada>
    {
        Task<List<VisitaGrupalAutoguiada>> GetAllGroupVisitAuAsync(DateTime fechaDesde, DateTime fechaHasta);

    }
}
