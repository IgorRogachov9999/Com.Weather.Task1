using Com.Weather.Task1.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace Com.Weather.Task1.WeatherFunction.Infrastructure
{
    public abstract class FunctionBase
    {
        protected async Task<ActionResult> ExecuteAsync(
            HttpRequest req,
            ILogger log,
            Func<Task<ActionResult>> handler)
        {
            try
            {
                return await handler();
            }
            catch (EntityNotFoundException ex)
            {
                log.LogWarning(new EventId(ex.ErrorCode, ex.Message), ex, ex.Message);
                return new NotFoundObjectResult(ex.Message);
            }
            catch (InvalidArgumentException ex)
            {
                log.LogWarning(new EventId(ex.ErrorCode, ex.Message), ex, ex.Message);
                return new BadRequestObjectResult(ex.Message);
            }
            catch (Exception ex)
            {
                log.LogError(new EventId(500, ex.Message), ex, ex.Message);
                return new InternalServerErrorResult();
            }
        }
    }
}
