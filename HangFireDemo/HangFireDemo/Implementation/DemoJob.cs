using Hangfire;
using Hangfire.Logging.LogProviders;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using Serilog;

namespace HangFireDemo.Implementation
{

    public interface IDemoJob
    {   [AutomaticRetry(Attempts = 5)]
       // [Queue("critical")]
        void UpTestJobRun(TestParams param);
    }

    public class DemoJob : IDemoJob
    {
        public DemoJob()
        {
        }
        public void UpTestJobRun(TestParams param)
        {  
            Log.Information("Demo Job is running with params {0}, {1}", param.FirstName, param.LastName);
        }
    }

    public class TestParams
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
