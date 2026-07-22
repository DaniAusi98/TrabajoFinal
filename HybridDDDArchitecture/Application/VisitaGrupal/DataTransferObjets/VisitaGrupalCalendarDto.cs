using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.VisitaGrupal.DataTransferObjets
{
    public class VisitaGrupalCalendarDto
    {
        public int Id { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public string IdProvincia { get; set; }
        public string IdLocalidad { get; set; }
        public int CantidadPersonas { get; set; }
        public string Institucion { get; set;}
        public string DiversidadFuncional { get; set; }
        public DateTime HoraInicio { get; set; }
        public DateTime HoraFin { get; set; }


    }
}
/*Tipo
              *provincia
              *localidad
             CantidadPersonas
             Institucion
             diversidadFuncional
             TimeSlot*/
