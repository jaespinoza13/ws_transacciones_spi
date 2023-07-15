using Application.Common.ISO20022.Models;
using Application.Common.Models;

namespace Application.Features.Opis.Queries.Imprimir.Transferencias;

public class ResImprimirTransferencia : ResComun
{
    public Archivo autorizacion { get; set; } = new();
}