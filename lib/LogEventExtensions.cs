using Microsoft.Extensions.Logging;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace No1.SerilogTestSink;

public static class LogEventExtensions
{
	const BindingFlags MethodBindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic;

	public static bool Match<T>(this LogEvent logEvent, string methodName) {
		ArgumentNullException.ThrowIfNull(logEvent);
		ArgumentException.ThrowIfNullOrWhiteSpace(methodName);

		var methodInfo = typeof(T).GetMethod(methodName, MethodBindingFlags) ?? throw new ArgumentException($"Method with name {methodName} not found", nameof(methodName));
		var loggerMessage = methodInfo.GetCustomAttribute<LoggerMessageAttribute>() ?? throw new ArgumentException($"Method with name {methodName} does not have LoggerMessageAttribute attribute", nameof(methodName));
		var messageMatched = logEvent.MessageTemplate.Text.Equals(loggerMessage.Message, StringComparison.Ordinal);
		var levelMatched = (int)logEvent.Level == (int)loggerMessage.Level;

		return messageMatched && levelMatched;
	}
}