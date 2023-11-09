using Application.Common.ISO20022.Models;
using MediatR;

namespace Application.Features.Catalogos.Queries.GetCatalogos;

public class ReqGetCatalogo : Header, IRequest<ResGetCatalogo>
{
    public string str_catalogo { get; set; } = string.Empty;
    public string str_filtro { get; set; } = "-1";
}