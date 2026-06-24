using Core.Application.Repositories;

using Domain.Entities.VisitasGrupalesMuseo;

using System.Linq.Expressions;

namespace Application.VisitaGrupal.Repositories
{
    public interface IRepositorioVisitaGuiada : IRepository<VisitaGrupalGuiada>
    {
        //Task<List<VisitaGrupalGuiada>> FindByUserIdAsync(Expression<Func<VisitaGrupalGuiada, bool>> filter);
        
        Task<List<VisitaGrupalGuiada>> ObtenerConActividadAsync(DateTime fechaDesde, DateTime fechaHasta);
        Task<List<VisitaGrupalGuiada>> ObtenerPorUsuarioIdAsync(string usuarioId,DateTime fechaActual);
        Task<VisitaGrupalGuiada> FindByIdWithActividadAsync(int id);



    }
}
