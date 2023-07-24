namespace Domain.Entities.Consultas;

public class Firmante
{
    public int int_ente {get; set;}
    public string str_identificacion {get; set;} = string.Empty;
    public string str_nombres {get; set;} = string.Empty;
    public string? str_observacion {get; set;} = string.Empty;
    public string str_num_cta {get; set;} = string.Empty;
}