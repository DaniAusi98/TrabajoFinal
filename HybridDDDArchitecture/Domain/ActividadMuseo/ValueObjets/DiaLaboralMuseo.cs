using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ActividadMuseo.ValueObjets
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    namespace Domain.ActividadMuseo.ValueObjects
    {
        public record DiasLaboralesMuseo
        {
            public IReadOnlyCollection<DayOfWeek> Dias { get; init; }

            public DiasLaboralesMuseo(IEnumerable<DayOfWeek> dias)
            {
                if (dias == null)
                    throw new ArgumentNullException(nameof(dias));

                var diasLista = dias.Distinct().ToList();

                if (!diasLista.Any())
                    throw new ArgumentException(
                        "Debe existir al menos un día laboral configurado");

                Dias = new ReadOnlyCollection<DayOfWeek>(diasLista);
            }


            public bool EsDiaLaboral(DayOfWeek dia)
            {
                return Dias.Contains(dia);
            }
        }
    }
}
