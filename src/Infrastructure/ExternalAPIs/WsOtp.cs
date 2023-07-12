using Domain.Types;
using Application.Common.Converting;
using Application.Common.Interfaces;
using Application.Common.ISO20022.Models;
using Application.Common.Models;
using Infrastructure.Common.Interfaces;
using Infrastructure.Common.Models;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Infrastructure.ExternalAPIs;

public class WsOtp : IWsOtp
{
    private readonly ApiSettings _settings;
    private readonly IHttpService _httpservice;
    private readonly IOtpDat _otpDat;

    public WsOtp(IOptionsMonitor<ApiSettings> options, IHttpService httpService, IOtpDat otpDat)
    {
        _settings = options.CurrentValue;
        _httpservice = httpService;
        _otpDat = otpDat;
    }

    /// <summary>
    /// Valida si la operacion requiere OTP
    /// </summary>
    /// <returns></returns>
    public async Task<bool> ValidaRequiereOtp(Header header, string strOperacion)
    {
        var cabecera = new
        {
            int_id_sistema = Convert.ToInt32( header.str_id_sistema ),
            int_id_usuario = Convert.ToInt32( header.str_id_usuario ),
            str_usuario = header.str_login,
            int_id_perfil = header.str_id_perfil,
            int_id_oficina = header.str_id_oficina,
            str_nombre_canal = header.str_app,
            header.str_nemonico_canal,
            str_ip = header.str_ip_dispositivo,
            str_session = header.str_sesion,
            str_mac = header.str_mac_dispositivo
        };

        var cuerpo = new
        {
            strOperacion,
        };

        var raw = new
        {
            cabecera, cuerpo
        };

        var strData = JsonSerializer.Serialize( raw );
        
        var respuesta = await _httpservice.PostRestServiceDataAsync<RespuestaTransaccion>(
            strData,
            _settings.servicio_ws_otp + "VALIDA_REQUIERE_OTP",
            string.Empty,
            _settings.auth_ws_otp,
            AuthorizationType.Basic,
            header.str_id_transaccion );

        var requiereOtp = respuesta.codigo switch
        {
            "1009" => true,
            "1006" => false,
            _ => throw new ArgumentException( respuesta.diccionario["str_error"] )
        };

        return requiereOtp;
    }


    /// <summary>
    /// Valida OTP
    /// </summary>
    /// <returns></returns>
    public async Task<RespuestaTransaccion> ValidaOtp(dynamic reqValidaOtp)
    {
        var resultOtpDat = await _otpDat.GetDatosOtpDat( reqValidaOtp );
        var datosOtp = Conversions.ConvertToClassDynamic<ConfiguracionOtp>( (ConjuntoDatos)resultOtpDat.cuerpo );

        var cabecera = new
        {
            int_id_sistema = Convert.ToInt32( reqValidaOtp.str_id_sistema ),
            int_id_usuario = Convert.ToInt32( reqValidaOtp.str_id_usuario ),
            str_usuario = reqValidaOtp.str_login,
            int_id_perfil = reqValidaOtp.str_id_perfil,
            int_id_oficina = reqValidaOtp.str_id_oficina,
            str_nombre_canal = reqValidaOtp.str_app,
            reqValidaOtp.str_nemonico_canal,
            str_ip = reqValidaOtp.str_ip_dispositivo,
            str_session = reqValidaOtp.str_sesion,
            str_mac = reqValidaOtp.str_mac_dispositivo
        };

        var configOtp = new
        {
            int_ente_socio = Convert.ToInt32( reqValidaOtp.str_ente ),
            datosOtp.str_celular,
            str_canal = reqValidaOtp.str_nemonico_canal,
            str_proceso = reqValidaOtp.str_app,
            str_servicio = reqValidaOtp.str_id_servicio,
            str_clave = reqValidaOtp.str_otp
        };

        var raw = new
        {
            cabecera,
            cuerpo = configOtp
        };


        var strData = JsonSerializer.Serialize( raw );

        RespuestaTransaccion respuesta = await _httpservice.PostRestServiceDataAsync<RespuestaTransaccion>
            ( strData,
            _settings.servicio_ws_otp + "VALIDA_OTP",
            string.Empty,
            _settings.auth_ws_otp,
            AuthorizationType.Basic,
            reqValidaOtp.str_id_transaccion
            );

        return respuesta;
    }
}