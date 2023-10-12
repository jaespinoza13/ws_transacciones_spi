using AccesoDatosGrpcAse.Neg;
using Application.Common.ISO20022.Models;
using Application.Common.Models;

namespace Infrastructure.Common.Functions;

public static class Funciones
{
    public static ConjuntoDatos ObtenerDatos(DatosRespuesta resultado)
    {
        var conjuntoDatos = new ConjuntoDatos
        {
            LstTablas = resultado.ListaTablas.Select(t =>
                new Tabla
                {
                    LstFilas = t.ListaFilas.Select(t1 =>
                        new Application.Common.Models.Fila
                        {
                            NombreValor = t1.ListaColumnas
                                .ToDictionary(t2 => t2.NombreCampo, t2 => (object)t2.Valor) // Conversión a object
                        }
                    ).ToList()
                }
            ).ToList()
        };

        return conjuntoDatos;
    }



    public static void LlenarDatosAuditoriaSalida(DatosSolicitud ds, Header header)
    {
        ds.ListaPEntrada.Add( new ParametroEntrada{ StrNameParameter = "@str_id_transaccion", TipoDato = TipoDato.VarChar, ObjValue = header.str_id_transaccion} );
        ds.ListaPEntrada.Add( new ParametroEntrada{ StrNameParameter = "@int_id_sistema", TipoDato = TipoDato.Integer, ObjValue = header.str_id_sistema } );
        ds.ListaPEntrada.Add( new ParametroEntrada{ StrNameParameter = "@str_login", TipoDato = TipoDato.VarChar, ObjValue = header.str_login } );
        ds.ListaPEntrada.Add( new ParametroEntrada{ StrNameParameter = "@int_id_perfil", TipoDato = TipoDato.Integer, ObjValue = header.str_id_perfil } );
        ds.ListaPEntrada.Add( new ParametroEntrada{ StrNameParameter = "@int_id_oficina", TipoDato = TipoDato.Integer, ObjValue = header.str_id_oficina } );
        ds.ListaPEntrada.Add( new ParametroEntrada{ StrNameParameter = "@str_nemonico_canal", TipoDato = TipoDato.VarChar, ObjValue = header.str_nemonico_canal} );
        ds.ListaPEntrada.Add( new ParametroEntrada{ StrNameParameter = "@str_ip_dispositivo", TipoDato = TipoDato.VarChar, ObjValue = header.str_ip_dispositivo} );
        ds.ListaPEntrada.Add( new ParametroEntrada{ StrNameParameter = "@str_sesion", TipoDato = TipoDato.VarChar, ObjValue = header.str_sesion } );
        ds.ListaPEntrada.Add( new ParametroEntrada{ StrNameParameter = "@str_mac_dispositivo", TipoDato = TipoDato.VarChar,ObjValue = header.str_mac_dispositivo } );
        ds.ListaPEntrada.Add( new ParametroEntrada{ StrNameParameter = "@str_pais", TipoDato = TipoDato.VarChar, ObjValue = header.str_pais } );
        ds.ListaPSalida.Add( new ParametroSalida { StrNameParameter = "@str_o_error", TipoDato = TipoDato.VarChar } );
        ds.ListaPSalida.Add( new ParametroSalida { StrNameParameter = "@int_o_error_cod", TipoDato = TipoDato.Integer } );
    }
}