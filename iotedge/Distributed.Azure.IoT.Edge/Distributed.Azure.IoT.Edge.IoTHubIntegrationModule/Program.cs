// Local run cmd line.
// dapr run --app-id iot-hub-integration-module --app-protocol http --app-port 5000 --components-path=../../../deployment/helm/iot-edge-accelerator/templates/dapr -- dotnet run -- -p "<Device Connection String>" [-m "messaging"] [-t "telemetry"] [-r "MessageSubscription/telemetry"]

using CommandLine;

using Distributed.Azure.IoT.Edge.Common.Device;
using Distributed.Azure.IoT.Edge.IoTHubIntegrationModule;

var builder = WebApplication.CreateBuilder(args);

IoTHubIntegrationParameters? parameters = null;

ParserResult<IoTHubIntegrationParameters> result = Parser.Default.ParseArguments<IoTHubIntegrationParameters>(args)
                .WithParsed(parsedParams =>
                {
                    parameters = parsedParams;
                    builder.Services.AddScoped<IoTHubIntegrationParameters>(sp => parameters);
                    builder.Services.AddSingleton<IDeviceClient, DeviceClientWrapper>(sp => new DeviceClientWrapper(parameters?.PrimaryConnectionString));
                })
                .WithNotParsed(errors =>
                {
                    Environment.Exit(1);
                });

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCloudEvents();

app.UseRouting();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
