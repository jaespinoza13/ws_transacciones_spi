using Application.Common.Interfaces;
using Application.Common.Models;
using Infrastructure.GrpcClients.Sybase;


namespace Infrastructure.SessionControl;

public class SessionControl : ISessionControl
{
    private readonly ISessionDat _sessionDat;
    public SessionControl(ISessionDat sessionDat) => _sessionDat = sessionDat;

    public RespuestaTransaccion SessionControlFilter(ValidaSesion bodyRequest)
    {
        return _sessionDat.ControlSession( bodyRequest );
    }
}