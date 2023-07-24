using Application.Common.ISO20022.Models;
using MediatR;

namespace Application.Features.Consultas.Queries.Cuentas;

public class ReqBuscarCuentas : Header, IRequest<ResBuscarCuentas>
{
    public string str_num_cuenta { get; set; } = string.Empty;
    public string str_identificacion { get; set; } = "-1";
    public int int_ente { get; set; } = -1;
    public int int_tipo_ahorro { get; set; } = 1;
    public int int_tipo_ahorro_proposito { get; set; } = -1;
    public int int_tipo_ahorro_digital { get; set; } = -1;
    public string str_firmantes { get; set; } = "N";
}