using System.Reflection;
using System.Text;
using System.Text.Json;
using Application.Common.Interfaces;
using Application.Common.Models;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services;

public class HttpService : IHttpService
    {
        private readonly ILogs _logs;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ApiConfig _settings;
        private readonly string _strClase;

        public HttpService(IOptionsMonitor<ApiConfig> options, ILogs logs, IHttpClientFactory httpClientFactory)
        {
            _logs = logs;
            _httpClientFactory = httpClientFactory;
            _settings = options.CurrentValue;
            _strClase = GetType().FullName!;
        }

        public async Task<T> GetRestServiceDataAsync<T>(string serviceAddress)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                client.BaseAddress = new Uri( serviceAddress );

                var response = await client.GetAsync( client.BaseAddress );
                response.EnsureSuccessStatusCode();

                var jsonResult = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<T>( jsonResult )!;

                return result;
            }
            catch (Exception ex)
            {
                await _logs.SaveHttpErrorLogs( null, MethodBase.GetCurrentMethod()!.Name, _strClase, ex, null );
                throw;
            }
        }

        public async Task<T> PostRestServiceDataAsync<T>(string serializedData, string serviceAddress, string parameters, string auth, string authorizationType, string idTransaccion, int timeout = 0)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                client.BaseAddress = new Uri( serviceAddress );
                client.DefaultRequestHeaders.Add( "Authorization", authorizationType + " " + auth );
                client.Timeout = timeout == 0
                    ? TimeSpan.FromSeconds( _settings.timeOutHttp )
                    : TimeSpan.FromSeconds( _settings.timeOutHttpBanRed );

                var httpContent = new StringContent( serializedData, Encoding.UTF8, "application/json" );

                var response = await client.PostAsync( parameters, httpContent );
                var resultadoJson = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var respuesta = JsonSerializer.Deserialize<T>( resultadoJson )!;
                    return respuesta;
                }

                var responseService = new
                {
                    codigo = response.StatusCode,
                    cabecera = response.Headers,
                    cuerpo = resultadoJson
                };

                _logs.SaveHttpErrorLogs( JsonSerializer.Deserialize<dynamic>( serializedData ),MethodBase.GetCurrentMethod()!.Name, "HttpService", responseService, idTransaccion );

                return default!;
            }
            catch (Exception ex)
            {
                _logs.SaveHttpErrorLogs( JsonSerializer.Deserialize<dynamic>( serializedData ),MethodBase.GetCurrentMethod()!.Name, _strClase, ex, idTransaccion );
                throw;
            }
        }
    }

