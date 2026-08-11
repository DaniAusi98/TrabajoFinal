using Application.VisitaGrupal.DataTransferObjets;
using AutoMapper;
using Domain.VisitasGrupales.Entities;
using Domain.VisitasGrupales.ValueObjects;

namespace Application.VisitaGrupal.Mappings
{
    public class MappingVisitasGuiadas:Profile
    {
        public MappingVisitasGuiadas()
        {
            CreateMap<TurnoDisponible, TurnoDisponibleDto>()
                .ForMember(dest => dest.HorarioTurno, opt => opt.MapFrom(src => src.HorarioTurno))
                .ForMember(dest => dest.CapacidadMaxima, opt => opt.MapFrom(src => src.CapacidadMaxima))
                .ForMember(dest => dest.EstadoTurno, opt => opt.MapFrom(src => src.EstadoTurno.ToString()));
            CreateMap<SlotDisponibleVisitaAutoguiada, SlotDisponibleDto>()
                .ForMember(dest => dest.HorarioSlot, opt => opt.MapFrom(src => src.HorarioSlot))
                .ForMember(dest => dest.CapacidadMaxima, opt => opt.MapFrom(src => src.CapacidadMaxima))
                .ForMember(dest => dest.CuposDisponibles, opt => opt.MapFrom(src => src.CuposDisponibles))
                .ForMember(dest => dest.EstadoSlot, opt => opt.MapFrom(src => src.EstadoSlot.ToString()));
            CreateMap<BloqueoVisitasGuiadas, BloqueoVisitaGuiadaDto>()
                .ForMember(dest => dest.FechaDesde, opt => opt.MapFrom(src => src.FechaDesde))
                .ForMember(dest => dest.FechaHasta, opt => opt.MapFrom(src => src.FechaHasta))
                .ForMember(dest => dest.Motivo, opt => opt.MapFrom(src => src.Motivo));


            CreateMap<TematicaVisita, TematicaVisitaDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre))
                .ForMember(dest => dest.Descripcion, opt => opt.MapFrom(src => src.Descripcion))
                .ForMember(dest => dest.Disponible, opt => opt.MapFrom(src => src.Disponible))
                .ForMember(dest => dest.SalaIds, opt => opt.MapFrom(src => src.Salas.Select(s => s.Id)));

            CreateMap<VisitaGrupalGuiada, GuidedTourReservationDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UsuarioVisitanteId, opt => opt.MapFrom(src => src.UsuarioVisitanteId))
                .ForMember(dest => dest.Institucion, opt => opt.MapFrom(src => src.Institucion))
                .ForMember(dest => dest.EmailInstitucion, opt => opt.MapFrom(src => src.EmailInstitucion.Valor))
                .ForMember(dest => dest.ProvinciaInstitucion, opt => opt.MapFrom(src => src.ProvinciaInstitucion))
                .ForMember(dest => dest.DepartamentoInstitucion, opt => opt.MapFrom(src => src.DepartamentoInstitucion))
                .ForMember(dest => dest.CiudadInstitucion, opt => opt.MapFrom(src => src.LocalidadInstitucion))
                .ForMember(dest => dest.NivelCurso, opt => opt.MapFrom(src => src.NivelEducativo))
                .ForMember(dest => dest.AnioCurso, opt => opt.MapFrom(src => src.AnioGrado))
                .ForMember(dest => dest.DescripcionDiscapacidad, opt => opt.MapFrom(src => src.DiversidadFuncionalDescripcion))
                .ForMember(dest => dest.MotivoVisita, opt => opt.MapFrom(src => src.MotivoRelacionVisita))
                .ForMember(dest => dest.Observaciones, opt => opt.MapFrom(src => src.Observaciones))
                .ForMember(dest => dest.TematicasDto, opt => opt.MapFrom(src => src.Tematicas))
                .ForMember(dest => dest.FechaInicio, opt => opt.MapFrom(src => src.TimeSlots.Select(t => t.Inicio).FirstOrDefault()))
                .ForMember(dest => dest.FechaFin, opt => opt.MapFrom(src => src.TimeSlots.Select(t => t.Fin).FirstOrDefault()))
                .ForMember(dest => dest.CantidadPersonas, opt => opt.MapFrom(src => src.CantidadPersonas))
                .ForMember(dest => dest.EstadoConfirmacion, opt => opt.MapFrom(src => src.EstadoConfirmacion));


            CreateMap<VisitaGrupalAutoguiada, VisitaAutoguiadaDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UsuarioVisitanteId, opt => opt.MapFrom(src => src.UsuarioVisitanteId))
                .ForMember(dest => dest.Institucion, opt => opt.MapFrom(src => src.Institucion))
                .ForMember(dest => dest.EmailInstitucion, opt => opt.MapFrom(src => src.EmailInstitucion.Valor))
                .ForMember(dest => dest.ProvinciaInstitucion, opt => opt.MapFrom(src => src.ProvinciaInstitucion))
                .ForMember(dest => dest.DepartamentoInstitucion, opt => opt.MapFrom(src => src.DepartamentoInstitucion))
                .ForMember(dest => dest.CiudadInstitucion, opt => opt.MapFrom(src => src.LocalidadInstitucion))
                .ForMember(dest => dest.DescripcionDiscapacidad, opt => opt.MapFrom(src => src.DiversidadFuncional))
                .ForMember(dest => dest.Observaciones, opt => opt.MapFrom(src => src.Observaciones))
                .ForMember(dest => dest.TematicasDto, opt => opt.MapFrom(src => src.Tematicas))
                .ForMember(dest => dest.FechaInicio, opt => opt.MapFrom(src => src.TimeSlots.Select(t => t.Inicio).FirstOrDefault()))
                .ForMember(dest => dest.FechaFin, opt => opt.MapFrom(src => src.TimeSlots.Select(t => t.Fin).FirstOrDefault()))
                .ForMember(dest => dest.CantidadPersonas, opt => opt.MapFrom(src => src.CantidadPersonas))
                .ForMember(dest => dest.EstadoConfirmacion, opt => opt.MapFrom(src => src.EstadoConfirmacion));

        }   
    }
}
