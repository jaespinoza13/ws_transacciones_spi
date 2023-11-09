using Application.Features.Opis.Vm;
using AutoMapper;
using Domain.Entities.Opis;

namespace Application.Mappings;

public class MappingProfile: Profile
{
    public MappingProfile()
    {
        CreateMap<BuscarOpis, TransferenciaReporteVm>().ReverseMap();
        CreateMap<BuscarOpis, ProveedorReporteVm>().ReverseMap();
    }
}