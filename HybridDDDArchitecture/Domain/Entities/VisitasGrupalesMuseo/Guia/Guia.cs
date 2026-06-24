using Core.Domain.Entities;

using Domain.Exceptions;
using Domain.Validators.VisitasGrupalesValidators;

namespace Domain.Entities.VisitasGrupalesMuseo.Guia
{
    public class Guia : DomainEntity<int, GuiaValidator>
    {
        public List<HorarioGuia> HorariosGuia { get; private set; } = new();

        public List<AusenciaGuia> AusenciasProgramadas { get; private set; } = new();

        public int PersonalInternoId { get; private set; }

        public bool Activo { get; private set; } = true;


        protected Guia()
        {
            // EF Core
        }

        public Guia(
            int personalInternoId,
            IEnumerable<HorarioGuia> horariosGuia)
        {
            PersonalInternoId = personalInternoId;

            HorariosGuia = horariosGuia?.ToList() ?? new List<HorarioGuia>();
        }

        public void AsignarHorarios(
            IEnumerable<HorarioGuia> nuevosHorarios)
        {
            ArgumentNullException.ThrowIfNull(nuevosHorarios);

            HorariosGuia.Clear();

            HorariosGuia.AddRange(nuevosHorarios);
        }

        public void AgregarAusencia(AusenciaGuia ausencia)
        {
            ArgumentNullException.ThrowIfNull(ausencia);

            AusenciasProgramadas.Add(ausencia);
        }

        public void EliminarAusencia(int ausenciaId)
        {
            var ausencia = AusenciasProgramadas
                .FirstOrDefault(a => a.Id == ausenciaId)
                ?? throw new DomainException(
                    "La ausencia indicada no existe.");

            AusenciasProgramadas.Remove(ausencia);
        }

        public void GuiaNoActivo()
        {
            if (!Activo)
                throw new DomainException(
                    "El guía ya está desactivado.");

            Activo = false;
        }
    }
}
