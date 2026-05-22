using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;
using System;

namespace No1.SerilogTestSink;

public static class TestLoggerConfigurationExtensions
{
	public static LoggerConfiguration TestSerilog(this LoggerSinkConfiguration sinkConfiguration) {
		ArgumentNullException.ThrowIfNull(sinkConfiguration);
		return sinkConfiguration.Sink(new TestSerilogSink());
	}
}