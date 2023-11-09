using Application.Common.ISO20022.Models;

namespace Application.Features.Opis.Queries.Cuadre;

public class ResCuadreOpis : ResComun
{
    public string? file_bytes { get; set; }
    public string? str_doc_extencion { get; set; }
}