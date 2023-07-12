namespace Domain.Services.Alfresco;

public class DocumentoAlfresco
{
    public int int_ente { get; set; }
    public int int_tipo_identifica { get; set; }
    public string str_num_identifica { get; set; } = string.Empty;
    public string str_documento_id { get; set; } = string.Empty;
    public string str_referencia { get; set; } = string.Empty;
    public string str_path_repositorio { get; set; } = string.Empty;
    public string str_modelo_doc_alfresco { get; set; } = string.Empty;
    public string str_nombre { get; set; } = string.Empty;
    public string str_observacion { get; set; } = string.Empty;
    public string str_doc_extencion { get; set; } = string.Empty;
    public string file_bytes { get; set; } = string.Empty;
    public int int_sistema { get; set; }
    public int int_oficina { get; set; }
    public int int_perfil { get; set; }
    public string str_usuario { get; set; } = string.Empty;
    public string str_terminal { get; set; } = string.Empty;
}