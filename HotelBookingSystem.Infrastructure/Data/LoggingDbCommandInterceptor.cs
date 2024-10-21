using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using System.Data.Common;


namespace HotelBookingSystem.Infrastructure.Data
{
    public class LoggingDbCommandInterceptor : DbCommandInterceptor
    {
        private readonly ILogger<LoggingDbCommandInterceptor> _logger;

        public LoggingDbCommandInterceptor(ILogger<LoggingDbCommandInterceptor> logger)
        {
            _logger = logger;
        }

        public override async ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(
            DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation($"Executing command: {command.CommandText}");
            return await base.ReaderExecutingAsync(command, eventData, result, cancellationToken);
        }

        public override InterceptionResult<int> NonQueryExecuting(
            DbCommand command, CommandEventData eventData, InterceptionResult<int> result)
        {
            _logger.LogInformation($"Executing non-query command: {command.CommandText}");
            return base.NonQueryExecuting(command, eventData, result);
        }

        public override InterceptionResult<object> ScalarExecuting(
            DbCommand command, CommandEventData eventData, InterceptionResult<object> result)
        {
            _logger.LogInformation($"Executing scalar command: {command.CommandText}");
            return base.ScalarExecuting(command, eventData, result);
        }
    }
}