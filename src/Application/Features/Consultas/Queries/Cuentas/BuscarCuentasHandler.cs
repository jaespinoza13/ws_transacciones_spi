using System.Reflection;
using Application.Common.Converting;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Persistence;
using Domain.Entities.Consultas;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Consultas.Queries.Cuentas;

public class BuscarCuentasHandler: IRequestHandler<ReqBuscarCuentas, ResBuscarCuentas>
{
    private readonly IConsultaDat _consultaDat;
    private readonly ILogs _logs;
    private readonly string _clase;
    private readonly ILogger<BuscarCuentasHandler> _logger;

    public BuscarCuentasHandler(IConsultaDat consultaDat, ILogs logs, ILogger<BuscarCuentasHandler> logger)
    {
        _consultaDat = consultaDat;
        _logs = logs;
        _clase = GetType().Name;
        _logger = logger;
    }

    public async Task<ResBuscarCuentas> Handle(ReqBuscarCuentas request, CancellationToken cancellationToken)
    {
        var respuesta = new ResBuscarCuentas();

        const string strOperacion = "GET_CUENTAS_SOCIO";

        try
        {
            respuesta.LlenarResHeader( request );

            _ = _logs.SaveHeaderLogs( request, strOperacion, MethodBase.GetCurrentMethod()!.Name, _clase );

            var respuestaTransaccion = await _consultaDat.BuscarCuentasSocios( request );

            if (respuestaTransaccion.codigo.Equals( "000" ))
            {
                var ienumCuentas = Conversions.ConvertToListClassDynamic<Cuenta>( (ConjuntoDatos)respuestaTransaccion.cuerpo );
                var ienumFirmantes = Conversions.ConvertToListClassDynamic<Firmante>( (ConjuntoDatos)respuestaTransaccion.cuerpo, 1 );

                var lstFirmantes = (List<Firmante>)ienumFirmantes;
                var lstCuentas = (List<Cuenta>)ienumCuentas;

                var lstCuentasAhorroProposito = lstCuentas.Where( x => x is { int_tipo_producto: 8, bit_cta_estado: true } ).ToList();
                var lstCuentasAhorroDigital = lstCuentas.Where( x => x is { int_tipo_producto: 9, bit_cta_estado: true } ).ToList();
                var lstCuentasAhorro = lstCuentas.Where( x => x.int_tipo_producto == 1 ).ToList();

                foreach (var cuenta in lstCuentasAhorro)
                {
                    cuenta.lst_firmantes = lstFirmantes.Where( f => f.str_num_cta == cuenta.str_num_cuenta ).ToList();
                    cuenta.str_condicion_cuenta = cuenta.lst_firmantes.Count > 0 ? cuenta.lst_firmantes[0].str_observacion : null;
                }

                var lstCuentasSocios = lstCuentasAhorro.Concat( lstCuentasAhorroProposito ).Concat( lstCuentasAhorroDigital ).ToList();

                respuesta.lst_cuentas = lstCuentasSocios;
            }

            respuesta.str_res_codigo = respuestaTransaccion.codigo;
            respuesta.str_res_info_adicional = respuestaTransaccion.diccionario["Error"];
            _ = _logs.SaveResponseLogs( respuesta, strOperacion, MethodBase.GetCurrentMethod()!.Name, _clase );
        }
        catch (Exception e)
        {
            _ = _logs.SaveExceptionLogs( respuesta, strOperacion, MethodBase.GetCurrentMethod()!.Name, _clase, e );
            _logger.LogError( e, "Error en BuscarCuentasHandler" );
            throw new ArgumentException( respuesta.str_id_transaccion );
        }

        return respuesta;
    }
}