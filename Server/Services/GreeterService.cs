using Grpc.Core;

namespace Server.Services;

public class GreeterService : Greeter.GreeterBase
{
    private readonly ILogger<GreeterService> _logger;

    public GreeterService(ILogger<GreeterService> logger)
    {
        _logger = logger;
    }

    public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
    {
        return Task.FromResult(new HelloReply { Message = "Hello " + request.Name });
    }

    public override async Task Greeting(
        HelloRequest request,
        IServerStreamWriter<HelloReply> response,
        ServerCallContext context
    )
    {
        for (int i = 0; i < 10; i++)
        {
            await response.WriteAsync(new HelloReply { Message = "Hello " + request.Name + i });
            await Task.Delay(1000);
        }
    }
}
