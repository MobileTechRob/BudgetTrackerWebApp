using Microsoft.Extensions.Logging;

public class MyLoggingProvider : ILoggerProvider
{
    public ILogger CreateLogger(string categoryName)
    {
        //throw new NotImplementedException();
        return new CustomLogger(categoryName);
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}