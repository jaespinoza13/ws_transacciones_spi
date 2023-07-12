using Application.Common.Exceptions;
using Application.Common.ISO20022.Interfaces;
using Application.Common.ISO20022.Models;
using Application.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace WebUI.Filters;

public  class ApiExceptionFilterAttribute : ExceptionFilterAttribute
{
    private readonly IDictionary<Type, Action<ExceptionContext>> _exceptionHandlers;
    private readonly ILogger<ApiExceptionFilterAttribute> _logger;
    private readonly ApiSettings _settings;

    private static readonly DateTime DateTimeFilter = DateTime.ParseExact( DateTime.Now.ToString( "yyyy-MM-dd HH:mm:ss" ), "yyyy-MM-dd HH:mm:ss", null );

    public ApiExceptionFilterAttribute(ILogger<ApiExceptionFilterAttribute> logger, IOptionsMonitor<ApiSettings> option)
    {
        _logger = logger;
        _settings = option.CurrentValue;
        _exceptionHandlers = new Dictionary<Type, Action<ExceptionContext>>
        {
            { typeof(NullReferenceException), HandleNullReferenceException },
            { typeof(ArgumentException), HandleArgumentException },
            { typeof(ValidationException), HandleValidationException },
            { typeof(NotFoundException), HandleNotFoundException },
            { typeof(UnauthorizedAccessException), HandleUnauthorizedAccessException },
            { typeof(ForbiddenAccessException), HandleForbiddenAccessException },
        };
    }

    public override void OnException(ExceptionContext context)
    {
        _logger.LogError( "An error occurred: {Message}", context.Exception.Message );
        HandleException( context );

        base.OnException( context );
    }

    private void HandleException(ExceptionContext context)
    {
        var type = context.Exception.GetType();

        if (_exceptionHandlers.TryGetValue( type, out var handler ))
        {
            handler.Invoke( context );
        }

        if (!context.ModelState.IsValid)
        {
            HandleInvalidModelStateException( context );
        }
    }

    #region ArgumentException

    private static void HandleArgumentException(ExceptionContext context)
    {
        var exception = (ArgumentException)context.Exception;

        ResArgumentException resArgumentException = new()
        {
            str_id_transaccion = exception.Message
        };
        ResArgumentExceptionHandler( resArgumentException );

        context.HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
        context.Result = new ObjectResult( resArgumentException );
        context.ExceptionHandled = true;
    }

    #endregion

    #region NullReferenceException

    private static void HandleNullReferenceException(ExceptionContext context)
    {
        ResException resException = new();
        ResExceptionHandler( resException, System.Net.HttpStatusCode.InternalServerError.ToString() );

        context.HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
        context.Result = new ObjectResult( resException );
        context.ExceptionHandled = true;
    }

    #endregion

    #region ValidationException

    private void HandleValidationException(ExceptionContext context)
    {
        var exception = (ValidationException)context.Exception;

        var details = new ValidationProblemDetails( exception.Errors );

        if (_settings.mostrar_descripcion_badrequest.Equals( 1 ))
        {
            ResBadRequestException resBadRequestException = new();
            ResBadRequestExceptionHandler( resBadRequestException );
            resBadRequestException.obj_res_detalle_errores = (Dictionary<string, string[]>)details.Errors;
            context.Result = new BadRequestObjectResult( resBadRequestException );
        }
        else
        {
            ResException resException = new();
            ResExceptionHandler( resException, System.Net.HttpStatusCode.BadRequest.ToString() );
            context.Result = new ObjectResult( resException );
        }

        context.HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
        context.ExceptionHandled = true;
    }

    #endregion

    #region ModelStateException

    private void HandleInvalidModelStateException(ExceptionContext context)
    {
        var details = new ValidationProblemDetails( context.ModelState );

        if (_settings.mostrar_descripcion_badrequest.Equals( 1 ))
        {
            ResBadRequestException resBadRequestException = new();
            ResBadRequestExceptionHandler( resBadRequestException );
            resBadRequestException.obj_res_detalle_errores = (Dictionary<string, string[]>)details.Errors;
            context.Result = new BadRequestObjectResult( resBadRequestException );
        }
        else
        {
            ResException resException = new();
            ResExceptionHandler( resException, System.Net.HttpStatusCode.BadRequest.ToString() );
            context.Result = new ObjectResult( resException );
        }

        context.HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
        context.ExceptionHandled = true;
    }

    #endregion

    #region UnauthorizedAccessException

    private static void HandleUnauthorizedAccessException(ExceptionContext context)
    {
        ResException resException = new();
        ResExceptionHandler( resException, System.Net.HttpStatusCode.Unauthorized.ToString() );

        context.HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.Unauthorized;
        context.Result = new ObjectResult( resException );
        context.ExceptionHandled = true;
    }

    #endregion

    #region ForbiddenAccessException

    private static void HandleForbiddenAccessException(ExceptionContext context)
    {
        ResException resException = new();
        ResExceptionHandler( resException, System.Net.HttpStatusCode.Forbidden.ToString() );

        context.HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
        context.Result = new ObjectResult( resException );
        context.ExceptionHandled = true;
    }

    #endregion

    #region NotFoundException

    private static void HandleNotFoundException(ExceptionContext context)
    {
        ResException resException = new();
        ResExceptionHandler( resException, System.Net.HttpStatusCode.NotFound.ToString() );

        context.HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.NotFound;
        context.Result = new ObjectResult( resException );
        context.ExceptionHandled = true;
    }

    #endregion

    #region Generic Answer

    private static void ResArgumentExceptionHandler(IException resArgumentException)
    {
        resArgumentException.dt_res_fecha_msj_crea = DateTimeFilter;
        resArgumentException.str_res_codigo = "500";
        resArgumentException.str_res_id_servidor = System.Net.HttpStatusCode.InternalServerError.ToString();
        resArgumentException.str_res_estado_transaccion = "ERR";
        resArgumentException.str_res_info_adicional = "Ocurrió un problema, intente nuevamente más tarde";
    }

    private static void ResExceptionHandler(IException resException, string httpStatusCode)
    {
        resException.dt_res_fecha_msj_crea = DateTimeFilter;
        resException.str_res_codigo = "500";
        resException.str_res_id_servidor = httpStatusCode;
        resException.str_res_estado_transaccion = "ERR";
        resException.str_res_info_adicional = "Ocurrió un problema, intente nuevamente más tarde";
    }

    private static void ResBadRequestExceptionHandler(IException resBadRequestException)
    {
        resBadRequestException.dt_res_fecha_msj_crea = DateTimeFilter;
        resBadRequestException.str_res_codigo = "400";
        resBadRequestException.str_res_id_servidor = System.Net.HttpStatusCode.BadRequest.ToString();
        resBadRequestException.str_res_estado_transaccion = "ERR";
        resBadRequestException.str_res_info_adicional = "Petición Inválida";
    }

    #endregion
}