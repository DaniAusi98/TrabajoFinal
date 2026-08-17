using Application.Reportes.DataTransferObjets;
using AutoMapper;
using Domain.Reportes.Entities;

namespace Application.Reportes.Mappings
{
    public class Mapping:Profile
    {
        public Mapping () {

            CreateMap<ReporteGeneralVisitasGrupales, ReporteVisitasGrupalesDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ReservasTotales, opt => opt.MapFrom(src => src.ReservasTotales))
                .ForMember(dest => dest.VisitanteTotales, opt => opt.MapFrom(src => src.VisitanteTotales))
                .ForMember(dest => dest.VisitasConfirmadas, opt => opt.MapFrom(src => src.VisitasConfirmadas))
                .ForMember(dest => dest.VisitasCanceladas, opt => opt.MapFrom(src => src.VisitasCanceladas))
                .ForMember(dest => dest.Reprogramadas, opt => opt.MapFrom(src => src.Reprogramadas))
                .ForMember(dest => dest.Pendientes, opt => opt.MapFrom(src => src.Pendientes));

            CreateMap<ReporteVisitasGuiadas, ReporteVisitaGuiadaDto>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
               .ForMember(dest => dest.ReservasTotales, opt => opt.MapFrom(src => src.ReservasTotales))
               .ForMember(dest => dest.VisitanteTotales, opt => opt.MapFrom(src => src.VisitanteTotales))
               .ForMember(dest => dest.VisitasConfirmadas, opt => opt.MapFrom(src => src.VisitasConfirmadas))
               .ForMember(dest => dest.VisitasCanceladas, opt => opt.MapFrom(src => src.VisitasCanceladas))
               .ForMember(dest => dest.Reprogramadas, opt => opt.MapFrom(src => src.Reprogramadas))
               .ForMember(dest => dest.Pendientes, opt => opt.MapFrom(src => src.Pendientes))
               .ForMember(dest => dest.TasaOcupacion, opt => opt.MapFrom(src => src.TasaOcupacion));
            CreateMap<ReporteVisitasAutoguiadas, ReporteVisitaAutoguiadaDto>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
               .ForMember(dest => dest.ReservasTotales, opt => opt.MapFrom(src => src.ReservasTotales))
               .ForMember(dest => dest.VisitanteTotales, opt => opt.MapFrom(src => src.VisitanteTotales))
               .ForMember(dest => dest.VisitasConfirmadas, opt => opt.MapFrom(src => src.VisitasConfirmadas))
               .ForMember(dest => dest.VisitasCanceladas, opt => opt.MapFrom(src => src.VisitasCanceladas))
               .ForMember(dest => dest.Reprogramadas, opt => opt.MapFrom(src => src.Reprogramadas))
               .ForMember(dest => dest.Pendientes, opt => opt.MapFrom(src => src.Pendientes))
               .ForMember(dest => dest.TasaOcupacion, opt => opt.MapFrom(src => src.TasaOcupacion));









        }
    }
}
