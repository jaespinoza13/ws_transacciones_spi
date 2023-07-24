using Application.Common.ISO20022.Models;
using Domain.Entities.Consultas;

namespace Application.Features.Consultas.Queries.Cuentas;

public class ResBuscarCuentas: ResComun
{
    public List<Cuenta> lst_cuentas { get; set; } = new();
}