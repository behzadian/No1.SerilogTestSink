# No1.SerilogTestSink

With SerilogTestSink, you can check wether specific message is being logged or not.

## Usage

#### Add SerilogTestSink to your serilog configuration:

Add `No1.SerilogTestSink` to project's dependencies. Add below lines to your .csproj file:
```
<ItemGroup Condition="'$(IsTestProject)' == 'true'">
	<PackageReference Include="No1.SerilogTestSink" Version="1.0.0.1" />
</ItemGroup>
```

Please note this dependency only when added to dependencies that project is being tested. Note to (IsTestProject variable)

Then add below configs in your test configuration. For example in `appsettings.test.json`:

```json
"Serilog": {
	"Using": [ "No1.SerilogTestSink" ],
	"WriteTo": [
		{ "Name": "TestSerilog" }
	]
}
```

Now you can check logged messages in your tests like:

```cs
[Fact]
public async Task WhenDebugMessageLogPostedThenLoggerSkips() {
	// Arrange
	var request = new HttpRequestMessage(HttpMethod.Get, "/LocalOnly/Test/" + nameof(TestController.LogDebug));

	// Act
	var response = await client.SendAsync(request);

	// Assert
	Assert.True(TestSerilogSink.LogEvents.Any());
	Assert.DoesNotContain(TestSerilogSink.LogEvents, x => x.Match<TestLogTemplates>(nameof(TestLogTemplates.LogDebug)));
}

[Fact]
public async Task WhenInfromationMessageLogPostedThenLoggerPersist() {
	// Arrange
	var request = new HttpRequestMessage(HttpMethod.Get, "/LocalOnly/Test/" + nameof(TestController.LogInformation));

	// Act
	var response = await client.SendAsync(request);

	// Assert
	Assert.Contains(TestSerilogSink.LogEvents, x => x.Match<TestLogTemplates>(nameof(TestLogTemplates.LogInformation)));
}

[Fact]
public async Task WhenAnEndpointThrowsExceptionThenExceptionWillBeLogged() {
	// Arrange
	var request = new HttpRequestMessage(HttpMethod.Get, "/LocalOnly/Test/" + nameof(TestController.ThrowException));

	// Act
	var response = await client.SendAsync(request);

	// Assert
	Assert.Contains(TestSerilogSink.LogEvents, x => x.Level == LogEventLevel.Error && x.Exception?.GetType() == typeof(Exception));
}
```

### What is method `Match`?

It is optimized to define a method for eveny log message like below:
```cs
internal partial class TestLogTemplates
{
	protected TestLogTemplates()
	{
	}

	[LoggerMessage(Level = LogLevel.Trace, Message = "LogTrace is being called at {instant}")]
	internal static partial void LogTrace(ILogger logger, Instant instant);

	[LoggerMessage(Level = LogLevel.Debug, Message = "LogDebug is being called at {instant}")]
	internal static partial void LogDebug(ILogger logger, Instant instant);

	[LoggerMessage(Level = LogLevel.Information, Message = "LogInformation is being called at {instant}")]
	internal static partial void LogInformation(ILogger logger, Instant instant);

	[LoggerMessage(Level = LogLevel.Warning, Message = "LogWarning is being called at {instant}")]
	internal static partial void LogWarning(ILogger logger, Instant instant);

	[LoggerMessage(Level = LogLevel.Error, Message = "LogError is being called at {instant}")]
	internal static partial void LogError(ILogger logger, Instant instant);

	[LoggerMessage(Level = LogLevel.Critical, Message = "LogCritical is being called at {instant}")]
	internal static partial void LogCritical(ILogger logger, Instant instant);
}
```

So I used this methods to post log messages and then used below method to make my tests more clear. It is up to you to use it or not:

```
LogEventExtensions.Match

```


### Building

For enabling Husky.NET run:

```
dotnet new tool-manifest
dotnet tool install Husky
git add .husky/pre-commit
```

After this, Husky formats the code in staged files based on .editorconfig configurations just before commit.