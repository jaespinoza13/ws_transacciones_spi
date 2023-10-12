using Application.Common.ISO20022.Models;
using Application.Common.Models;

namespace Application.Common.Functions;

internal static class Functions
{
    public static Cabecera LlenarCabeceraSolicitud(Header header)
    {
        Cabecera cabecera = new()
        {
            int_sistema = Convert.ToInt32( header.str_id_sistema ),
            str_usuario = header.str_login,
            int_perfil = Convert.ToInt32( header.str_id_oficina ),
            int_oficina = Convert.ToInt32( header.str_id_oficina ),
            str_nemonico_canal = header.str_nemonico_canal,
            str_ip = header.str_ip_dispositivo,
            str_session = header.str_sesion,
            str_mac = header.str_mac_dispositivo,
            str_nombre_canal = header.str_app
        };

        return cabecera;
    }
}