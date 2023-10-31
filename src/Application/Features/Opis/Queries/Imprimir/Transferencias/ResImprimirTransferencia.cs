using Application.Common.ISO20022.Models;
using Application.Common.Models;

namespace Application.Features.Opis.Queries.Imprimir.Transferencias;

public class ResImprimirTransferencia : ResComun
{
    public string? file_bytes { get; set; } 
    public string? str_doc_extencion { get; set; }
}