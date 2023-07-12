using Application.Common.Models;

namespace Infrastructure.Common.Interfaces;

public interface IOtpDat
{
    Task<RespuestaTransaccion> GetDatosOtpDat(dynamic request);
}