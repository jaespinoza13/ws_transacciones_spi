using MediatR;
using Application.Common.ISO20022.Models;

namespace Application.Common.Cryptography;

public class ReqGetKeys : Header, IRequest<ResGetKeys>
{

}