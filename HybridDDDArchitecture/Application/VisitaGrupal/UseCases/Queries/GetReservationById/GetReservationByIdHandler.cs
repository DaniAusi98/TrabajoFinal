using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Application.VisitaGrupal.DataTransferObjets;
using Application.VisitaGrupal.Repositories;

using Core.Application;
using Domain.VisitasGrupales.Entities.GrupalGuiada;

namespace Application.VisitaGrupal.UseCases.Queries.GetReservationById
{
    internal sealed class GetReservationByIdHandler(IRepositorioVisitaGuiada repositorioVisitaGuiada) : IRequestQueryHandler<GetReservationByIdQuery, GuidedTourReservationDto>
    {
        private readonly IRepositorioVisitaGuiada _repositorioVisitaGuiada = repositorioVisitaGuiada ?? throw new ArgumentNullException(nameof(repositorioVisitaGuiada));
        public async Task<GuidedTourReservationDto> Handle(GetReservationByIdQuery request, CancellationToken cancellationToken)
        {
            VisitaGrupalGuiada visitaGrupal = await _repositorioVisitaGuiada.FindByIdWithActividadAsync(request.ReservationId);

            return visitaGrupal.To<GuidedTourReservationDto>();
        }
    }
}
