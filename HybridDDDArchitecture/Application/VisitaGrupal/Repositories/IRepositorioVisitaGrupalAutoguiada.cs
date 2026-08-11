using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Application.Repositories;
using Domain.VisitasGrupales.Entities;

namespace Application.VisitaGrupal.Repositories
{
    public interface IRepositorioVisitaGrupalAutoguiada:IRepository<VisitaGrupalAutoguiada>
    {
        Task<List<VisitaGrupalAutoguiada>> GetAllGroupVisitAuAsync(DateTime fechaDesde, DateTime fechaHasta);

    }
}
