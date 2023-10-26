using Domain.Services;

namespace Application.Common.Interfaces;

public interface IAlfresco
{
    Task<ResAddDocAlfresco> AddDocumento(DocumentoAlfresco documentoAlfresco, string operacion, string idTransaccion);

    Task<ResGetDocAlfresco> GetDocumento(string idDocumentoAlfresco, string operacion, string idTransaccion);
}