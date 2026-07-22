using Application.VisitaGrupal.DataTransferObjets;

using AutoMapper;

using Domain.VisitasGrupales.Entities;

namespace Application.VisitaGrupal.Mappings
{
    public class MappingVisitasGuiadas:Profile
    {
        public MappingVisitasGuiadas()
        {
            CreateMap<TurnoDisponible, TurnoDisponibleDto>()
                .ForMember(dest => dest.HorarioTurno, opt => opt.MapFrom(src => src.HorarioTurno))
                .ForMember(dest => dest.CapacidadMaxima, opt => opt.MapFrom(src => src.CapacidadMaxima))
                .ForMember(dest => dest.CapacidadDisponible, opt => opt.MapFrom(src => src.CapacidadDisponible))
                .ForMember(dest => dest.EstadoTurno, opt => opt.MapFrom(src => src.EstadoTurno.ToString()));

            CreateMap<TematicaVisita, TematicaVisitaGrupalDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre))
                .ForMember(dest => dest.Descripcion, opt => opt.MapFrom(src => src.Descripcion));

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
                .ForMember(dest => dest.FechaInicio, opt => opt.MapFrom(src => src.ActividadMuseo.TimeSlots.Select(t => t.Inicio).FirstOrDefault()))
                .ForMember(dest => dest.FechaFin, opt => opt.MapFrom(src => src.ActividadMuseo.TimeSlots.Select(t => t.Fin).FirstOrDefault()))
                .ForMember(dest => dest.CantidadPersonas, opt => opt.MapFrom(src => src.ActividadMuseo.CantidadPersonas))
                .ForMember(dest => dest.EstadoConfirmacion, opt => opt.MapFrom(src => src.EstadoConfirmacion));

        }   
    }
}
/*using Application.ApplicationMuseo.DataTransferObjects;
using Application.ApplicationMuseo.DomainEvents;

using AutoMapper;

using Domain.Entities;

namespace Application.ApplicationMuseo.Mappings
{
    /// <summary>
    /// El mapeo entre objetos debe ir definido aqui
    /// </summary>
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<DummyEntity, DummyEntityCreated>().ReverseMap();
            CreateMap<DummyEntity, DummyEntityUpdated>().ReverseMap();
            CreateMap<DummyEntity, DummyEntityDto>().ReverseMap();
        }
    }
}
*/
