using System.Net.WebSockets;
using System.Text;
using System.Xml;
using RestSharp;
using RestSharp.Authenticators.Digest;
using RestSharp.Serializers.NewtonsoftJson;

// using RestSharp.Serializers.Xml;

public struct Post
{
    public int userId;
    public int id;
    public string title;
    public string body;
}

class WebSocket
{
    private readonly string _username;
    private readonly string _password;
    private readonly string _url;
    private readonly string _data_url;
    private ClientWebSocket webSocket = new();

    public WebSocket(string username, string password, string url)
    {
        _username = username;
        _password = password;
        _url = url;
        _data_url = _url;
    }

    public async Task Subscribe()
    {
        var ws = Getws();
        webSocket.Options.AddSubProtocol("robapi2_subscription");
        webSocket.Options.SetRequestHeader("Cookie", "session_id=abc123");
        var serverUrl = new Uri(ws);
        await webSocket.ConnectAsync(serverUrl, CancellationToken.None);
        await ReceiveMessage(webSocket);
    }

    public async Task UnSubscribe()
    {
        if (webSocket.State != WebSocketState.Open)
        {
            return;
        }
        await webSocket.CloseAsync(
            WebSocketCloseStatus.NormalClosure,
            "Closing",
            CancellationToken.None
        );
    }

    private async Task ReceiveMessage(ClientWebSocket webSocket)
    {
        byte[] buffer = new byte[1024];
        WebSocketReceiveResult result = await webSocket.ReceiveAsync(
            new ArraySegment<byte>(buffer),
            CancellationToken.None
        );
        string message = Encoding.UTF8.GetString(buffer, 0, result.Count);
        // get data
    }

    public string Getws()
    {
        while (true)
        {
            var content = GetWsAddr();
            var ws = ParseXml(content);
            if (ws != "error")
            {
                return ws;
            }
            Thread.Sleep(6000);
        }
    }

    private void GetData()
    {
        var restOptiosn = new RestClientOptions(_data_url)
        {
            Authenticator = new DigestAuthenticator(_username, _password)
        };
        var client = new RestClient(
            restOptiosn,
            configureSerialization: s => s.UseNewtonsoftJson()
        );
        var request = new RestRequest("", Method.Get);

        var response = client.Execute<List<Post>>(request);

        Console.WriteLine(response.Content);
    }

    public string GetWsAddr()
    {
        var restOptiosn = new RestClientOptions(_url)
        {
            Authenticator = new DigestAuthenticator(_username, _password)
        };

        var client = new RestClient(
            restOptiosn,
            configureSerialization: s => s.UseNewtonsoftJson()
        );
        var request = new RestRequest("", Method.Get);

        var response = client.Execute<List<Post>>(request);

        return response.Content!;
    }

    public string ParseXml(string text)
    {
        XmlDocument xmlDoc = new();
        xmlDoc.LoadXml(text);

        XmlNode? node = xmlDoc.SelectSingleNode("//Book[@id='1']");

        if (node != null)
        {
            return "ws://localhost:8080";
        }
        return "error";
    }
}
