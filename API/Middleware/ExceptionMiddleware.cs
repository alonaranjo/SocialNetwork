﻿using System.Net;
using System.Text.Json;
using API.Errors;

namespace API.Middelware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IHostEnvironment _env;
    private const string CONTENTTYPE = "application/json";
    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            context.Response.ContentType = CONTENTTYPE;
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;           
            var response = new ApiException(context.Response.StatusCode, ex.Message, GetDetailsByEnvironment(ex.StackTrace?.ToString()));               
            var options = new JsonSerializerOptions{ PropertyNamingPolicy = JsonNamingPolicy.CamelCase };   
            var json = JsonSerializer.Serialize(response, options); 
            await context.Response.WriteAsync(json);
        }
    }

    private string GetDetailsByEnvironment(string details)
    {
        if (_env.IsDevelopment())
        {
            return details;
        } 
        return "Internal Server Error";
    }
}