// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.DependencyInjection;
using Serilog;
using InsurancePlanForPatient;

var services = new ServiceCollection();
services.AddTransient<App>();
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("log-app.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

Log.Information("Console started");
var serviceProvider = services.BuildServiceProvider();
await serviceProvider.GetService<App>().RunAsync();


