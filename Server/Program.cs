using Server.Services;
using Sharp7;

var client = new S7Client();
int result = client.ConnectTo("127.0.0.1", 0, 1);

if (result == 0)
{
    Console.WriteLine("Connected to 127.0.0.1");
}
else
{
    Console.WriteLine(client.ErrorText(result));
    Console.ReadKey();
}

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<GreeterService>();
app.MapGet(
    "/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909"
);

app.Run();
