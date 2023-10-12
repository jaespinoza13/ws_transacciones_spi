using Application.Common.ISO20022.Models;
using Application.Common.Models;

namespace Application.Features.Opis.Queries.Imprimir.OrdenPago;

public class ResImprimirOrdenPago : ResComun
{
    public Archivo autorizacion { get; set; } = new();
}