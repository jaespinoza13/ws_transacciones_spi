using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiVersion("1")]
[Route("api/v{version:apiVersion}/ws_[controller]")]
[Produces("application/json")]
[ApiController]
public class BaseController : ControllerBase
{
}