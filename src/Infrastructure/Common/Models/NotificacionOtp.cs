namespace Infrastructure.Common.Models;

public class NotificacionOtp
{
    public string str_nombre_socio { get; set; } = string.Empty;
    public string str_configuracion_email { get; set; } = string.Empty;
    public string str_variables_email { get; set; } = string.Empty;
    public string str_plantilla_email { get; set; } = string.Empty;
    public string str_configuracion_sms { get; set; } = string.Empty;
    public string str_variables_sms { get; set; } = string.Empty;
    public string str_plantilla_sms { get; set; } = string.Empty;
}