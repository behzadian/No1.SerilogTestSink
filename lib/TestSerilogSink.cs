using Serilog.Core;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace No1.SerilogTestSink;

public class TestSerilogSink : ILogEventSink
{
	public static IList<LogEvent> LogEvents { get; } = [];

	public void Emit(LogEvent logEvent) {
		LogEvents.Add(logEvent);
	}
}