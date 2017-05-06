# Useful.LoggerFactory.Log4net
Log4net ILoggerFactory using the new Microsoft logging library in C# for .NET 4.5.1, 4.5.2, 4.6

<image src="https://ci.appveyor.com/api/projects/status/github/Tazmainiandevil/Useful.LoggerFactory.Log4net?branch=master&svg=true">
<a href="https://badge.fury.io/nu/Useful.LoggerFactory.Log4net"><img src="https://badge.fury.io/nu/Useful.LoggerFactory.Log4net.svg" alt="NuGet version" height="18"></a>

Working with Log4net for many years really helped move this code forward and it's good to see that it can still be used with the new Microsoft logging library by implementing ILoggerFactory and ILogger.

Example usage in a Service Fabric service:

Add a file to PackageRoot\Config for the service

e.g. testconfig.xml
```C#
public TestStatelessService(StatelessServiceContext context)
            : base(context)
{
  var configPkg = context.CodePackageActivationContext.GetConfigurationPackageObject("Config");
  var customConfigFilePath = configPkg.Path + @"\testconfig.xml";  

  var services = ConfigureServices();
  services.GetService<ILoggerFactory>()
          .AddLog4Net(customConfigFilePath);
}

internal static IServiceProvider ConfigureServices()
{            
  return new ServiceCollection()
      .AddLogging()
      .AddSingleton<ILoggerFactory, LoggerFactory>()
      .BuildServiceProvider();
}
```

This can be combined with the official [Microsoft Application Insights Log4NetAppender] (https://www.nuget.org/packages/Microsoft.ApplicationInsights.Log4NetAppender/)


Happy Logging!!!
