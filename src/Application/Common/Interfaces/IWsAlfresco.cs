
using Domain.Services.Alfresco;

namespace Application.Common.Interfaces;

public interface IWsAlfresco
{
    ResAddDocAlfresco AddDocumentoAlfresco(DocumentoAlfresco documentoAlfresco, string strOperacion, string strIdTransaccion);
    ResGetDocAlfresco GetDocumentoAlfresco(string idDocumentoAlfresco, string strOperacion, string strIdTransaccion);
}