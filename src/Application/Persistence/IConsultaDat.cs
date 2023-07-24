using Application.Common.Models;
using Application.Features.Consultas.Queries.Cuentas;
using Application.Features.Consultas.Queries.Entidades;

namespace Application.Persistence;

public interface IConsultaDat
{
    Task<RespuestaTransaccion> BuscarCuentasSocios(ReqBuscarCuentas request);
    Task<RespuestaTransaccion> BuscarEntidades(ReqEntidades request);
}