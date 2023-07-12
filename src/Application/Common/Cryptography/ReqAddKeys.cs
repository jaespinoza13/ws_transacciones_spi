using MediatR;
using Application.Common.ISO20022.Models;

namespace Application.Common.Cryptography;

public class ReqAddKeys : Header, IRequest<ResAddKeys>
{

    public string str_modulo { get; set; } = string.Empty;
    public string str_exponente { get; set; } = string.Empty;
    public string str_llave_privada { get; set; } = string.Empty;
    public string str_llave_simetrica { get; set; } = string.Empty;

}