
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Application.VisitaGrupal.DataTransferObjets;

using Core.Application;

namespace Application.VisitaGrupal.UseCases.Queries.GetReservationsById
{
    public class GetReservationsByUserIdQuery : QueryRequest<QueryResult<GuidedTourReservationDto>>
    {
        public string UsuarioVisitanteId { get; set; }
        public GetReservationsByUserIdQuery()
        {
                
        }
    }
}
