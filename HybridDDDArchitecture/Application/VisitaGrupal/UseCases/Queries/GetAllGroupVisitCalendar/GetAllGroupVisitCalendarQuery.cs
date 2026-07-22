using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Application.ActividadMuseo.DataTransferObjets;
using Application.VisitaGrupal.DataTransferObjets;

using Core.Application;

namespace Application.VisitaGrupal.UseCases.Queries.GetAllGroupVisitCalendar
{
    public class GetAllGroupVisitCalendarQuery(DateTime fechaConsultaDesde, DateTime fechaConsultaHasta) : QueryRequest<QueryResult<VisitaGrupalCalendarDto>>
    {
        public DateTime FechaConsultaDesde { get; } = fechaConsultaDesde;
        public DateTime FechaConsultaHasta { get; } = fechaConsultaHasta;
    }
}
