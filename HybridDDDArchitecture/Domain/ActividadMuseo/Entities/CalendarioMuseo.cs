using Domain.ActividadMuseo.ValueObjets;
using Domain.ActividadMuseo.ValueObjets.Domain.ActividadMuseo.ValueObjects;
using Domain.Common.ValueObjets;

namespace Domain.ActividadMuseo.Entities
{
    public class CalendarioMuseo : ICalendarioMuseo
    {
        private readonly IReadOnlyCollection<DiaCierreMuseo> _diasCierre;
        private readonly HorarioMuseo _horario;
        private readonly DiasLaboralesMuseo _diasLaborales;


        public CalendarioMuseo(
            HorarioMuseo horario,
            DiasLaboralesMuseo diasLaborales,
            IReadOnlyCollection<DiaCierreMuseo> diasCierre)
        {
            _horario = horario
                ?? throw new ArgumentNullException(nameof(horario));

            _diasLaborales = diasLaborales
                ?? throw new ArgumentNullException(nameof(diasLaborales));

            _diasCierre = diasCierre
                ?? throw new ArgumentNullException(nameof(diasCierre));
        }


        public bool EsDiaOperativo(DateTime fecha)
        {
            if (!_diasLaborales.EsDiaLaboral(fecha.DayOfWeek))
                return false;


            var inicioDia = fecha.Date;
            var finDia = fecha.Date.AddDays(1);


            return !_diasCierre.Any(d =>
                d.SolapaConFechas(inicioDia, finDia));
        }


        public bool EstaAbierto(DateTime inicio, DateTime fin)
        {
            if (!_diasLaborales.EsDiaLaboral(inicio.DayOfWeek))
                return false;
            // 2. Verifico si el turno cae dentro de un cierre
            if (_diasCierre.Any(d => d.SolapaConFechas(inicio, fin)))
                return false;



            return inicio.TimeOfDay >= _horario.HoraApertura.ToTimeSpan()
                && fin.TimeOfDay <= _horario.HoraCierre.ToTimeSpan();
        }


        public IEnumerable<TimeSlot> ObtenerFranjasOperativas(DateOnly fecha)
        {
            var fechaDateTime = fecha.ToDateTime(TimeOnly.MinValue);


            if (!EsDiaOperativo(fechaDateTime))
                return [];


            return
            [
                new TimeSlot(
                    fecha.ToDateTime(_horario.HoraApertura),
                    fecha.ToDateTime(_horario.HoraCierre))
            ];
        }
    }
}