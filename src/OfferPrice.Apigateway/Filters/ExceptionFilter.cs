using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using OfferPrice.Apigateway.Exceptions;

namespace OfferPrice.Apigateway.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        private readonly ProblemDetailsFactory _problemDetails;

        public ExceptionFilter(ProblemDetailsFactory problemDetails)
        {
            _problemDetails = problemDetails;
        }

        public void OnException(ExceptionContext context)
        {
            switch (context.Exception)
            {
                case TokenException tokenException:
                    context.Result = new ObjectResult(
                        _problemDetails.CreateProblemDetails(context.HttpContext, 400, context.Exception.Message));
                    break;
                default:
                    context.Result = new ObjectResult(_problemDetails.CreateProblemDetails(context.HttpContext, 500));
                    break;
            }
        }
    }
}
