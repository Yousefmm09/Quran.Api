using Serilog;

namespace Quran.Services.Loggs
{
    public static class LogException
    {
        public static void Logs(Exception exception, string? context = null)
        {
            Log.Error(exception,
                "An exception occurred{Context}",
                context is null ? string.Empty : $" | Context: {context}");
        }
    }
}
