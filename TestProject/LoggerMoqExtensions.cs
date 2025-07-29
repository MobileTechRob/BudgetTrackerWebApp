using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
    public static class LoggerMoqExtensions
    {
        public static void SetupLogCapture<T>(
            this Mock<ILogger<T>> mockLogger,
            List<string> logList)
        {
            mockLogger
                .Setup(x => x.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((state, _) => true),
                    It.IsAny<Exception>(),                    
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()))
                .Callback<LogLevel, EventId, object, Exception, Delegate>(
                    (level, eventId, state, exception, formatter) =>
                    {
                        logList.Add("");
                    });
        }
    }
}
