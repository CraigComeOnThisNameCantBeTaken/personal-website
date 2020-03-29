using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Language.Flow;

namespace DataAccess.Tests.Extensions
{
    // Moq does not provide a way to mock and verify extension methods
    // these extensions can be used to simplify the process since only 'Log' is not
    // an extension method
    public static class MockLoggerExtensions
    {
        public static void VerifyLog<T>(this Mock<ILogger<T>> MockLogger, LogLevel level, string message, Times times)
        {
            MockLogger.Verify(x => x.Log(
                level,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => string.Equals(message, o.ToString(), StringComparison.InvariantCultureIgnoreCase)),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
                times
            );
        }
    }
}
