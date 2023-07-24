using Application.Common.ISO20022.Models;
using Domain.Entities.Catalogos;

namespace Application.Features.Catalogos.Queries.Consultar;

public class ResGetCatalogos: ResComun
{
    public List<Catalogo> lst_catalogos { get; set; } = new();
}