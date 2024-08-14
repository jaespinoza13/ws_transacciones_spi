﻿using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Common.Behaviours;

public class UnhandledExceptionBehavior<TRequest, TResponse>(ILogger<UnhandledExceptionBehavior<TRequest, TResponse>> logger) : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (Exception ex)
        {
            var requestName = typeof(TRequest).Name;

            logger.LogError( ex, "Application Request: Sucedio una exception para el request {Name} {@Request}", requestName, request );

            throw;
        }
    }
}