using Application.Common.ISO20022.Models;
using Domain.Entities.Opis;

namespace Application.Features.Opis.Queries.Buscar;

public class ResBuscarOpis : ResComun
{
    public List<BuscarOpis> lst_opis { get; set; } = new();
    public string? str_reporte_base64 { get; set; } 
    
    public string str_nombre_reporte { get; set; } = string.Empty;
}