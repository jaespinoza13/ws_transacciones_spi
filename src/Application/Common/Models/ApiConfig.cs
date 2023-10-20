namespace Application.Common.Models;

public class ApiConfig
{
    public string db_meg_sistemas { get; set; } = null!;
    public string db_meg_servicios { get; set; } = null!;
    public string db_meg_bce { get; set; } = null!;

    public string client_grpc_sybase { get; set; } = null!;
    public string client_grpc_mongo { get; set; } = null!;
    public int timeoutGrpcSybase { get; set; }
    public int delayOutGrpcSybase { get; set; }
    public int timeoutGrpcMongo { get; set; }
    public int delayOutGrpcMongo { get; set; }

    public string nombre_base_mongo { get; set; } = null!;
    public string coll_peticiones { get; set; } = null!;
    public string coll_errores { get; set; } = null!;
    public string coll_amenazas { get; set; } = null!;
    public string coll_respuesta { get; set; } = null!;
    public string coll_peticiones_diarias { get; set; } = null!;
    public string coll_promedio_peticiones_diarias { get; set; } = null!;
    public string coll_errores_db { get; set; } = null!;
    public string coll_errores_http { get; set; } = null!;

    public string auth_ws { get; set; } = null!;

    public string servicio_ws_alfresco { get; set; } = null!;


    public int timeOutHttp { get; set; }
    public int timeOutHttpBanRed { get; set; }

    public string logs_path_peticiones { get; set; } = null!;
    public string logs_path_errores { get; set; } = null!;
    public string logs_path_errores_db { get; set; } = null!;
    public string logs_path_amenazas { get; set; } = null!;
    public string logs_path_errores_http { get; set; } = null!;

    public List<string> lst_canales_encriptar { get; set; } = new();

    public int mostrar_descripcion_badrequest { get; set; }
    public string server_interno_email { get; set; } = null!;
    public string puerto_interno_email { get; set; } = null!;

    public string path_logo_png { get; set; } = null!;

    public string secretKey { get; set; } = null!;
    public string issuer { get; set; } = null!;
    public string Key_token_pub { get; set; } = null!;
    public string Key_encrypt_token { get; set; } = null!;
}