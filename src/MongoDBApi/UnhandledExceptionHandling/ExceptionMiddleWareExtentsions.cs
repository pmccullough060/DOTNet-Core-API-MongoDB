using System;
using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using MongoDBApi.Objects;

namespace MongoDBApi.UnhandledExceptionHandling
{
    public static class ExceptionMiddlewareExtentsions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    
                    if(contextFeature.Error is ArgumentException)
                    {
                        await context.Response.WriteAsync(new ErrorDetails()
                        {
                            StatusCode = (int)HttpStatusCode.UnprocessableEntity,
                            Message = "Unable to process the supplied arguments"
                        }.ToString());
                    }
                    else if(contextFeature != null)
                    {
                        await context.Response.WriteAsync(new ErrorDetails()
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = "Internal Server Error"
                        }.ToString());
                    }
                });
            });
        }
    }

    //here we are:
    //creating an extention method to register the UseExceptionHanlder middleware.
    //Populated the status code and content type of out response - the ErrorDetails object.
    //return the response wiht the custom created object.








}