namespace Application.Common.Models;

public class ConjuntoDatos
{
    public List<Tabla> LstTablas { get; set; } = new();
}

public class Tabla
{
    public List<Fila> LstFilas { get; set; } = new();
}

public class Fila
{
    public Dictionary<string, object> NombreValor { get; set; }

    public Fila()
    {
        NombreValor = new Dictionary<string, object>();
    }
}