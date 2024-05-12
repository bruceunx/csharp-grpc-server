using Sharp7;

namespace Server;

public struct Data
{
    public int Value1;
    public float Value2;
    public bool IsOpen;
}

public class Worker
{
    S7Client client;
    string _address;
    byte[] _buffer1 = new byte[100];
    byte[] _buffer2 = new byte[100];

    CancellationTokenSource cancellationTokenSource = new();

    object _lock = new();

    Data _data = new();

    public Worker(string address)
    {
        client = new S7Client();
        _address = address;
    }

    public async Task Start()
    {
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
        cancellationTokenSource.Cancel();
    }

    public Data GetData()
    {
        lock (_lock)
        {
            return _data;
        }
    }

    private bool Connect()
    {
        if (client.Connected)
            return true;
        var result = client.ConnectTo(_address, 0, 1);
        return result == 0;
    }

    private bool Read()
    {
        int result = 0;
        result += client.DBRead(1, 0, 100, _buffer1);
        result += client.DBRead(1, 0, 100, _buffer2);
        return result == 0;
    }

    private void ProcessData()
    {
        int value1 = S7.GetIntAt(_buffer1, 0);
        float value2 = S7.GetRealAt(_buffer2, 0);
        string message = S7.GetStringAt(_buffer1, 4);
        bool isSuccess = S7.GetBitAt(_buffer2, 4, 0);
        lock (_lock)
        {
            _data = new Data
            {
                Value1 = value1,
                Value2 = value2,
                IsOpen = isSuccess
            };
        }
    }
}
