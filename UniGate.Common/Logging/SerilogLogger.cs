using Serilog;
using Serilog.Formatting.Json;

namespace UniGate.Common.Logging;

public static class SerilogLogger
{
    public static void ConfigureLogging()
    {
        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.File(new JsonFormatter(), "logs/log.json", rollingInterval: RollingInterval.Day)
            .WriteTo.Seq("http://localhost:5341")
            .CreateLogger();
    }
}