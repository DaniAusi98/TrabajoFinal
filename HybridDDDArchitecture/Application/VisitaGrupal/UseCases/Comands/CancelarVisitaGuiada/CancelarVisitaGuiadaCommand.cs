using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.Application;

namespace Application.VisitaGrupal.UseCases.Comands.CancelarVisitaGuiada
{
    public class CancelarVisitaGuiadaCommand: IRequestCommand
    {
        public int ReservationId { get; set; }

        public CancelarVisitaGuiadaCommand()
        {
                
        }
    }
}
