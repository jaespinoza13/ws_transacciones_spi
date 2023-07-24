namespace Domain.Entities.Consultas;

public class Cuenta
{
    public int int_ente { get; set; }
    public string str_identificaion { get; set; } = string.Empty;
    public string str_apellidos { get; set; } = string.Empty;
    public string str_nombres { get; set; } = string.Empty;
    public string str_nombre_titular { get; set; } = string.Empty;
    public string str_nombre_cuenta { get; set; } = string.Empty;
    public string int_id_cuenta { get; set; } = string.Empty;
    public string str_id_cuenta { get; set; } = string.Empty;
    public string str_num_cuenta { get; set; } = string.Empty;
    public string str_num_cuenta_enmascarada { get; set; } = string.Empty;
    public string str_tipo_persona { get; set; } = string.Empty;
    public string dec_monto { get; set; } = string.Empty;
    public string srt_cta_estado { get; set; } = string.Empty;
    public bool bit_cta_estado { get; set; }
    public string str_telefono { get; set; } = string.Empty;
    public string str_email { get; set; } = string.Empty;
    public string str_telefono_enmascarado { get; set; } = string.Empty;
    public string str_email_enmascarado { get; set; } = string.Empty;
    public string str_tipo_bloqueo { get; set; } = string.Empty;
    public int int_tipo_producto { get; set; }
    public string str_nombre_producto { get; set; } = string.Empty;
    public string str_tipo_cuenta { get; set; } = string.Empty;
    public string str_nombre_tipo_cuenta { get; set; } = string.Empty;
    public bool bit_requiere_act_medios { get; set; }
    public string str_lista_bloqueo { get; set; } = string.Empty;
    public string? str_condicion_cuenta { get; set; }
    public List<Firmante> lst_firmantes { get; set; } = new();
}
