using Domain.VisitasGrupales.Entities;
using Core.Application.Repositories;

namespace Application.VisitaGrupal.Repositories
{
    public interface IRepositorioConfiguracionVisitasGrupalesGuiadas : IRepository<ConfiguracionVisitasGrupalesGuiadas>
    {
        Task<ConfiguracionVisitasGrupalesGuiadas?> ObtenerConfiguracionActivaAsync();
    }
}
