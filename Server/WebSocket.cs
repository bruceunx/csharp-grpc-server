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

    public WebSocket(string username, string password, string url)
    {
        _username = username;
        _password = password;
        _url = url;
    }

    public async void GetWsAddrAsync()
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

        var response = await client.ExecuteAsync<List<Post>>(request);

        if (response.IsSuccessful)
        {
            foreach (var post in response.Data!)
            {
                Console.WriteLine(post.title);
            }
        }
    }
}
