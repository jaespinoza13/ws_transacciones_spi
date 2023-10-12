namespace Application.Common.ISO20022.Models;

public class ResComun : Header
{
    public string str_res_codigo { get; set; } = string.Empty;
    public string str_res_info_adicional { get; set; } = string.Empty;

    public void LlenarResHeader(Header header)
    {
        str_id_transaccion = Guid.NewGuid().ToString();
        str_nemonico_canal = header.str_nemonico_canal;
        str_app = header.str_app;
        str_id_servicio = header.str_id_servicio.Replace( "REQ", "RES" );
        str_version_servicio = header.str_version_servicio;
        str_mac_dispositivo = header.str_mac_dispositivo;
        str_ip_dispositivo = header.str_ip_dispositivo;
        str_remitente = header.str_receptor;
        str_receptor = header.str_remitente;
        str_id_sistema = header.str_id_sistema;
        str_id_usuario = header.str_id_usuario;
        str_tipo_peticion = "REQ";
        str_id_msj = header.str_id_msj;
        dt_fecha_operacion = DateTime.ParseExact( DateTime.Now.ToString( "yyyy-MM-dd HH:mm:ss" ), "yyyy-MM-dd HH:mm:ss", null );
        bl_posible_duplicado = header.bl_posible_duplicado;
        str_prioridad = header.str_prioridad;
        str_login = header.str_login;
        str_latitud = header.str_latitud;
        str_longitud = header.str_longitud;
        str_firma_digital = header.str_firma_digital;
        str_num_sim = header.str_num_sim;
        str_clave_secreta = header.str_clave_secreta;
        str_pais = header.str_pais;
        str_sesion = header.str_sesion;
        str_id_perfil = header.str_id_perfil;
        str_id_oficina = header.str_id_oficina;
        str_ente = header.str_ente;
        header.str_id_transaccion = str_id_transaccion;
    }
}