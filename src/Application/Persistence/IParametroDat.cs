using Application.Common.Models;
using Application.Features.Parametros.Queries.GetParametros;

namespace Application.Persistence;

public interface IParametroDat
{
    Task<RespuestaTransaccion> GetParametros(ReqGetParametros request);
}