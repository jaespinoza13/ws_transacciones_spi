using System.Reflection;
using System.Text.Json;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Services;
using Domain.Types;
using Microsoft.Extensions.Options;

namespace Infrastructure.ExternalApis;

public class Alfresco : IAlfresco
{
    private readonly IHttpService _httpService;
    private readonly ILogs _logService;
    private readonly ApiConfig _settings;
    private readonly string _strClase;

    public Alfresco(IHttpService httpService, ILogs logService, IOptionsMonitor<ApiConfig> options)
    {
        _httpService = httpService;
        _logService = logService;
        _settings = options.CurrentValue;
        _strClase = GetType().FullName!;
    }

    public async Task<ResAddDocAlfresco> AddDocumento(DocumentoAlfresco documentoAlfresco, string operacion, string idTransaccion)
    {
        try
        {
            var endpointAlfresco = documentoAlfresco.str_documento_id.Equals( "-1" )
                ? "add_documento"
                : "set_documento_version";
            var url = _settings.servicio_ws_alfresco + endpointAlfresco;
            var data = JsonSerializer.Serialize( documentoAlfresco );


            return await _httpService.PostRestServiceDataAsync<ResAddDocAlfresco>
            ( data,
                url,
                string.Empty,
                string.Empty,
                AuthorizationType.Basic,
                idTransaccion
            );
        }
        catch (Exception ex)
        {
            var data = new { cuerpo = documentoAlfresco, str_operacion = operacion };
            await _logService.SaveHttpErrorLogs( data, MethodBase.GetCurrentMethod()!.Name, _strClase, ex, idTransaccion );
            throw new ArgumentException( idTransaccion );
        }
    }

    public async Task<ResGetDocAlfresco> GetDocumento(string idDocumentoAlfresco, string operacion, string idTransaccion)
    {
        try
        {
            var payload = new { str_id_documento = idDocumentoAlfresco };
            var data = JsonSerializer.Serialize( payload );
            var url = _settings.servicio_ws_alfresco + "get_documento";

            return await _httpService.PostRestServiceDataAsync<ResGetDocAlfresco>
            ( data,
                url,
                string.Empty,
                string.Empty,
                AuthorizationType.Basic,
                idTransaccion
            );
        }
        catch (Exception ex)
        {
            var data = new { cuerpo = idDocumentoAlfresco, str_operacion = operacion };
            await _logService.SaveHttpErrorLogs( data, MethodBase.GetCurrentMethod()!.Name, _strClase, ex, idTransaccion );
            throw new ArgumentException( idTransaccion );
        }
    }
}