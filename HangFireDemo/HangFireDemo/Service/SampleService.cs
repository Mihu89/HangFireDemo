using Hangfire;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Topshelf;
using Topshelf.Logging;

namespace HangFireDemo.Service
{
    public class SampleService : ServiceControl
    {
        readonly bool _throwOnStart;
        readonly bool _throwOnStop;
        readonly bool _throwUnhandled;

        static readonly LogWriter _log = HostLogger.Get<SampleService>();

        BackgroundJobServer _server;

        string[] _args;

        IWebHostBuilder _host;


        public SampleService(bool throwOnStart, bool throwOnStop, bool throwUnhandled, string[] args)
        {
            _throwOnStart = throwOnStart;
            _throwOnStop = throwOnStop;
            _throwUnhandled = throwUnhandled;
            _args = args;
        }

        public bool Start(HostControl hostControl)
        {
            _log.Info(" SampleService is starting ...");
            hostControl.RequestAdditionalTime(TimeSpan.FromSeconds(10));

            Thread.Sleep(1000);

            if (_throwOnStart)
            {
                _log.Info(" Throwing as requested");

            }

            ThreadPool.QueueUserWorkItem(x =>
            {
                Thread.Sleep(3000);
                if (_throwUnhandled)
                {
                    throw new InvalidOperationException("Throw Unhandled in Random Thred");
                }

                _log.Info("Requesting stop");
                hostControl.Stop();
            });

            _log.Info("SampleService started");
            Program.CreateHostBuilder(_args).Build().Run();

            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            _log.Info("SampleService Stopped");

            if (_throwOnStop)
            {
              //
            }
            return true;
        }

        public bool Pause(HostControl hostControl)
        {
            _log.Info(" SampleService Paused");

            return true;
        }

        public bool Continue(HostControl hostControl)
        {
            _log.Info("SampleService Continued");

            return true;
        }
    }
}
