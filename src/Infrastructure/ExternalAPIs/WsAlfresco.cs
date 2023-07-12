using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Services.Alfresco;
using Domain.Types;
using Microsoft.Extensions.Options;
using System.Reflection;
using System.Text.Json;

namespace Infrastructure.ExternalAPIs;

public class WsAlfresco : IWsAlfresco
{
    private readonly IHttpService _httpService;
    private readonly ILogs _logService;
    private readonly ApiSettings _settings;
    private readonly string _strClase;

    public WsAlfresco(IHttpService httpService, ILogs logs, IOptionsMonitor<ApiSettings> option)
    {
        _httpService = httpService;
        _logService = logs;
        _settings = option.CurrentValue;
        _strClase = GetType().Name;
    }


    public ResAddDocAlfresco AddDocumentoAlfresco(DocumentoAlfresco documentoAlfresco, string strOperacion, string strIdTransaccion)
    {
        try
        {
            var endpointAlfresco = documentoAlfresco.str_documento_id.Equals( "-1" )
                ? "add_documento"
                : "set_documento_version";

            var strData = JsonSerializer.Serialize( documentoAlfresco );

            return _httpService.PostRestServiceDataAsync<ResAddDocAlfresco>
            ( strData,
                _settings.servicio_ws_alfresco + endpointAlfresco,
                string.Empty,
                string.Empty,
                AuthorizationType.Basic,
                strIdTransaccion
            ).Result;
        }
        catch (Exception ex)
        {
            var data = new { cuerpo = documentoAlfresco, str_operacion = strOperacion };

            _logService.SaveHttpErrorLogs(
                data, MethodBase.GetCurrentMethod()!.Name,
                _strClase, ex,
                strIdTransaccion
                );

            throw new ArgumentException( strIdTransaccion );
        }
    }

    public ResGetDocAlfresco GetDocumentoAlfresco(string idDocumentoAlfresco, string strOperacion, string strIdTransaccion)
    {
        try
        {
            var data = new { str_id_documento = idDocumentoAlfresco };

            var strData = JsonSerializer.Serialize( data );

            return _httpService.PostRestServiceDataAsync<ResGetDocAlfresco>
            ( strData,
                _settings.servicio_ws_alfresco + "get_documento",
                string.Empty,
                string.Empty,
                AuthorizationType.Basic,
                strIdTransaccion
            ).Result;
            
        }
        catch (Exception ex)
        {
            var data = new { cuerpo = idDocumentoAlfresco, strOperacion };

            _logService.SaveHttpErrorLogs(
                data,
                MethodBase.GetCurrentMethod()!.Name,
                _strClase, ex,
                strIdTransaccion
            );

            throw new ArgumentException( strIdTransaccion );
        }
    }
}