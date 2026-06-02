using Serilog.Core;
using Serilog.Events;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace No1.SerilogTestSink;

public class TestSerilogSink : ILogEventSink
{
	public static ConcurrentQueue<LogEvent> LogEvents { get; } = [];

	public void Emit(LogEvent logEvent) {
		LogEvents.Enqueue(logEvent);
	}
}