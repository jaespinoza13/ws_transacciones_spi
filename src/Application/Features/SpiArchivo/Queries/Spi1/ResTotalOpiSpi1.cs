using Application.Common.ISO20022.Models;
using Domain.Entities.SpiArchivo;

namespace Application.Features.SpiArchivo.Queries.Spi1;

public class ResTotalOpiSpi1: ResComun
{
    public IReadOnlyList<TotalOpiCorte> lst_total_opi_corte { get; set; } = new List<TotalOpiCorte>();
    public string dec_total_monto { get; set; } = string.Empty;
    public int int_total_opis { get; set; }
}