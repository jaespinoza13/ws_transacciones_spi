using Application.Common.ISO20022.Models;
using Domain.Entities.Opis;

namespace Application.Features.Opis.Queries.Detalle;

public class ResDetalleOpi : ResComun
{
    public DetalleOpi? detalle_opi { get; set; }
    public List<FirmanteCuenta>? lst_condiciones { get; set; } = new();
}