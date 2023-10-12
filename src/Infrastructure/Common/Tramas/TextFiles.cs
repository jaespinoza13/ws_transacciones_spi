using System.Text.Json;

namespace Infrastructure.Common.Tramas;

public static class TextFiles
{
    private static readonly object ObjetoBloqueoJson = new();
    private static readonly object ObjetoBloqueo = new();

    public static void RegistrarTramas(string strTramaRequest, string rutaArchivo)
    {
        try
        {
            lock (ObjetoBloqueo)
            {
                if (!File.Exists( rutaArchivo ))
                {
                    Directory.CreateDirectory( rutaArchivo );
                }

                var fileName = Path.Combine( rutaArchivo, DateTime.Now.ToString( "yyyyMMdd" ) + ".txt" );

                using var fs = File.Open( fileName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite );
                using (var writer = new StreamWriter( fs ))
                {

                    writer.WriteLine($"{DateTime.Now.ToString( "HHmmssff" ) } { strTramaRequest }");
                }

                fs.Close();
            }
        }
        catch (Exception exception)
        {
            throw new ArgumentException( exception.Message );
        }
    }

    public static void RegistrarTramas(string strTipo, dynamic obj, string ruta)
    {
        var rutaArchivo = Directory.GetCurrentDirectory() + ruta;
        try
        {
            lock (ObjetoBloqueoJson)
            {
                if (!File.Exists( rutaArchivo ))
                {
                    Directory.CreateDirectory( rutaArchivo );
                }

                var fileName = Path.Combine( rutaArchivo, DateTime.Now.ToString( "yyyyMMdd" ) + ".txt" );

                using var fs = File.Open( fileName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite );
                using (var writer = new StreamWriter( fs ))
                {
                    writer.WriteLine( DateTime.Now.ToString( "HHmmssff" ) + " " + strTipo +
                                      JsonSerializer.Serialize( obj ) + " " );
                }

                fs.Close();
            }
        }
        catch (Exception exception)
        {
            throw new ArgumentException( exception.Message );
        }
    }
}