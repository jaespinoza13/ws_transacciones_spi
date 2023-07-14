namespace Application.Features.Spi1.Command.GenerarSpi1.Common;

public static class GenerarArchivo
{
    public static async Task<string> GenerarArchivoAsync(string nombreArchivo, string contenido)
    {
        var path = Path.Combine( Directory.GetCurrentDirectory(), "wwwroot", "temp", nombreArchivo );
        await File.WriteAllTextAsync( path, contenido );
        return path;
    }
}