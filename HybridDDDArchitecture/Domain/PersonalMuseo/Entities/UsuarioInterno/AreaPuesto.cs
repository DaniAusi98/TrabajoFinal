using Core.Domain.Entities;

using System;
using Domain.Common.Exceptions;

namespace Domain.PersonalMuseo.Entities.UsuarioInterno
{
    public class AreaPuesto : DomainEntity<int>
    {
        public int AreaId { get; private set; }
        public Area Area { get; private set; }

        public int PuestoId { get; private set; }
        public Puesto Puesto { get; private set; }

        public AreaPuesto()
        {
        }

        public AreaPuesto(int areaId, int puestoId)
        {
            SetAreaId(areaId);
            SetPuestoId(puestoId);
        }

        public void SetAreaId(int areaId)
        {
            if (areaId <= 0) throw new DomainException("AreaId debe ser mayor a 0.");
            AreaId = areaId;
        }

        public void SetPuestoId(int puestoId)
        {
            if (puestoId <= 0) throw new DomainException("PuestoId debe ser mayor a 0.");
            PuestoId = puestoId;
        }
    }
}
