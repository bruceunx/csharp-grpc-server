using Server;
using Server.Services;

class Program
{
    static async Task Main(string[] args)
    {
        // var ws = new WebSocket("admin", "admin", "https://jsonplaceholder.typicode.com/posts");
        // ws.GetWsAddrAsync();
        // await Task.Delay(5000);

        // var entites = await db.GetAllAsync();
        // PrintEntities(entites);
        // await db.InsertAsync("BruceLi", 27);
        // entites = await db.GetAllAsync();
        // PrintEntities(entites);

        // var db = new DataBase();
        // var worker = new NewsTask("hello");
        // Console.WriteLine("start worker");
        // var task = worker.Start();
        //
        // Console.WriteLine("now is worker");
        //
        // for (int i = 1; i <= 10; i++)
        // {
        //     Console.WriteLine("in loop");
        //     Console.WriteLine(worker.GetData());
        //     await Task.Delay(1000);
        // }
        //
        // worker.Stop();
        // await Task.Delay(1000);
        //
        // task = worker.Start();
        //
        // await Task.Delay(6000);
        //
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
        //
        // worker.Stop();
        // await task;
    }

    public static void PrintEntities(List<Entity> entities)
    {
        foreach (var entity in entities)
        {
            Console.WriteLine(entity);
        }
    }
}
