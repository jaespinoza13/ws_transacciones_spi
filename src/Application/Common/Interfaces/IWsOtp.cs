using Application.Common.ISO20022.Models;
using Application.Common.Models;

namespace Application.Common.Interfaces;

public interface IWsOtp
{
    Task<bool> ValidaRequiereOtp(Header header, string strOperacion);
    Task<RespuestaTransaccion> ValidaOtp(dynamic reqValidaOtp);
}