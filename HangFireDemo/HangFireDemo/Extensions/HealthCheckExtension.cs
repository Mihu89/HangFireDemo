using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using Hangfire;

namespace HangFireDemo.Extensions
{
    public static class HealthCheckExtension
    {
        public static IServiceCollection AddHelthCheckService(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddHealthChecks()
            //    .AddHangfire(opt => {
            //        opt.MaximumJobsFailed = 10;
            //    }, name: "hangire100")
            //    .AddSqlServer;

            return services;
        }
    }
}
