using Core.Domain.Entities;
using Domain.ActividadMuseo.Entities;
using Domain.ActividadMuseo.ValueObjets;

namespace Domain.VisitasGrupales.Entities
{
    public class ConfiguracionHorarioAutoguiada : DomainEntity<int>
    {
        public DiasLaboralesMuseo DiasDisponibles { get; private set; }

        public Horario HorarioDisponibleVisitaAutoguiadas { get; private set; }

        public int CapacidadMaximaPorGrupo { get; private set; }

        protected ConfiguracionHorarioAutoguiada()
        {
        }
        public ConfiguracionHorarioAutoguiada(
            int capacidadMaximaPorGrupo,
            DiasLaboralesMuseo diasDisponibles,
            Horario horarioDisponibleVisitaAutoguiadas,
            CalendarioMuseo calendario)
        {
            if (calendario == null)
                throw new ArgumentNullException(nameof(calendario));

            if (capacidadMaximaPorGrupo <= 0)
                throw new ArgumentException("La capacidad máxima por grupo debe ser mayor a 0.");

            ValidarHorarioDentroDelCalendario(
                horarioDisponibleVisitaAutoguiadas,
                calendario);

            CapacidadMaximaPorGrupo = capacidadMaximaPorGrupo;
            DiasDisponibles = diasDisponibles;
            HorarioDisponibleVisitaAutoguiadas = horarioDisponibleVisitaAutoguiadas;
        }
        public void ActualizarHorarioDisponible(Horario horario,CalendarioMuseo calendario)
        {
            if (horario == null)
                throw new ArgumentNullException(nameof(horario));

            if (calendario == null)
                throw new ArgumentNullException(nameof(calendario));

            ValidarHorarioDentroDelCalendario(horario, calendario);

            HorarioDisponibleVisitaAutoguiadas = horario;
        }
        public void ActualizarCapacidadMaxima(int capacidadMaximaPorGrupo)
        {
            if (capacidadMaximaPorGrupo <= 0)
                throw new ArgumentException("La capacidad máxima por grupo debe ser mayor a 0.", nameof(capacidadMaximaPorGrupo));
            CapacidadMaximaPorGrupo = capacidadMaximaPorGrupo;
        }
        public void ActualizarDiasDisponibles(DiasLaboralesMuseo diasDisponibles, CalendarioMuseo calendario)
        {
            if (diasDisponibles == null)
                throw new ArgumentNullException(nameof(diasDisponibles));
            if (calendario == null)
                throw new ArgumentNullException(nameof(calendario));
            // Validar que los días disponibles estén dentro de los días de apertura del museo
            foreach (var dia in diasDisponibles.Dias)
            {
                if (!calendario.DiasApertura.Dias.Contains(dia))
                {
                    throw new InvalidOperationException($"El día {dia} no está dentro de los días de apertura del museo.");
                }
            }
            DiasDisponibles = diasDisponibles;
        }

        private static void ValidarHorarioDentroDelCalendario(
            Horario horario,
            CalendarioMuseo calendario)
        {
            Console.WriteLine($"Horario visita: {horario.HoraInicio} - {horario.HoraFin}");
            Console.WriteLine($"Horario museo: {calendario.HorarioApertura.HoraInicio} - {calendario.HorarioApertura.HoraFin}");

            if (horario.HoraInicio < calendario.HorarioApertura.HoraInicio ||
                horario.HoraFin > calendario.HorarioApertura.HoraFin)
            {
                throw new InvalidOperationException(
                    "El horario de visitas debe estar dentro del horario de apertura del museo.");
            }
        }
    }
}
