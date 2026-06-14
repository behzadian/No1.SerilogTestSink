using Serilog.Core;
using Serilog.Events;
using System.Collections.Concurrent;

namespace No1.SerilogTestSink;

public class TestSerilogSink : ILogEventSink
{
	public static ConcurrentQueue<LogEvent> LogEvents { get; } = [];

	public void Emit(LogEvent logEvent) {
		LogEvents.Enqueue(logEvent);
	}
}