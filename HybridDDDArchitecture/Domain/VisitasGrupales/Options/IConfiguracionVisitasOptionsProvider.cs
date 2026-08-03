using System.Threading.Tasks;
using Domain.VisitasGrupales.Options;

namespace Domain.VisitasGrupales.Options
{
    public interface IConfiguracionVisitasOptionsProvider
    {
        Task<TurnosVisitasOptions> GetOptionsAsync();
    }
}
