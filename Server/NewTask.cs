namespace Server;

public class NewsTask
{
    string _address;
    string _data = "";

    CancellationTokenSource cancellationTokenSource = new();

    object _lock = new();

    public NewsTask(string address)
    {
        _address = address;
    }

    public async Task Start()
    {
        cancellationTokenSource = new();
        await Task.Run(
            () =>
            {
                while (!cancellationTokenSource.IsCancellationRequested)
                {
                    if (Connect() && Read())
                    {
                        ProcessData();
                    }
                    Task.Delay(1000).Wait();
                }
            },
            cancellationTokenSource.Token
        );
    }

    public void Stop()
    {
        Console.WriteLine("Stop worker");
        cancellationTokenSource.Cancel();
    }

    public string GetData()
    {
        lock (_lock)
        {
            return _data;
        }
    }

    private bool Connect()
    {
        return true;
    }

    private bool Read()
    {
        return true;
    }

    private void ProcessData()
    {
        lock (_lock)
        {
            Console.WriteLine("Processing data");
            DateTime now = DateTime.Now;
            _data = now.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}
