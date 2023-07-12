namespace Application.Common.Models;

public class SolicitudTransaccion
{
    public Cabecera cabecera { get; set; }
    public object cuerpo { get; set; }

    public SolicitudTransaccion(Cabecera cabecera, object cuerpo)
    {
        this.cabecera = cabecera;
        this.cuerpo = cuerpo;
    }
}