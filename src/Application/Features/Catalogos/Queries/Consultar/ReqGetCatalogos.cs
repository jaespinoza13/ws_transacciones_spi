using Application.Common.ISO20022.Models;
using MediatR;

namespace Application.Features.Catalogos.Queries.Consultar;

public class ReqGetCatalogos: Header, IRequest<ResGetCatalogos>
{
    public string str_nombre_catalogo { get; set; } = string.Empty;
    public string str_filtro { get; set; } = "-1";
}