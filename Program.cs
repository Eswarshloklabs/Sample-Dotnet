using App.Metrics.AspNetCore;
using App.Metrics.Formatters.Prometheus;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Sentry;

namespace Jenkin_project
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using (SentrySdk.Init(options =>
            {
                options.Dsn = "https://4d6ab91677563229e81328621e49c3b3@o4505780885585920.ingest.sentry.io/4505781358166016"; options.Debug = true;

                options.AutoSessionTracking = true;

                options.IsGlobalModeEnabled = false;

                options.CaptureFailedRequests = true;

                options.EnableTracing = true;

                options.TracesSampleRate = 0.1;
            }))
            {
                var builder = WebApplication.CreateBuilder(args);
                builder.WebHost.UseSentry();

                builder.WebHost.UseMetricsWebTracking();

                builder.WebHost.UseMetrics(options =>
                {
                    options.EndpointOptions = endpointOptions =>
                    {
                        endpointOptions.MetricsTextEndpointOutputFormatter = new MetricsPrometheusTextOutputFormatter();
                        endpointOptions.MetricsEndpointOutputFormatter = new MetricsPrometheusProtobufOutputFormatter();
                        endpointOptions.EnvironmentInfoEndpointEnabled = false;
                    };
                });

                builder.Services.AddRazorPages();

                builder.Services.AddMetrics();

                builder.Services.Configure<KestrelServerOptions>(options =>
                {
                    options.AllowSynchronousIO = true;
                });


                var app = builder.Build();
                app.UseSentryTracing();


                if (!app.Environment.IsDevelopment())
                {
                    app.UseExceptionHandler("/Error");
                }

                app.UseStaticFiles();

                app.UseRouting();

                app.UseAuthorization();

                app.MapRazorPages();

                app.Map("/api/throw-exception", app =>
                {
                    app.Run(async context =>
                    {
                        throw new Exception("This is a deliberate exception for testing Sentry.");
                    });
                });

                app.Run();
            }
        }
    }
}