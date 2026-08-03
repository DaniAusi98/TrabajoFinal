using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ActividadMuseo.ValueObjets
{


    public record HorarioMuseo
    {
        public TimeOnly HoraApertura { get; init; }
        public TimeOnly HoraCierre { get; init; }

        public HorarioMuseo(TimeOnly apertura, TimeOnly cierre)
        {
            if (cierre <= apertura)
                throw new ArgumentException(
                    "El horario de cierre debe ser posterior a la apertura");

            HoraApertura = apertura;
            HoraCierre = cierre;
        }
    }

}
