using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server;

class SocketClient
{
    private Socket _socket;
    private IPAddress _ipAddress;
    private int _port;

    public SocketClient(string address, int port)
    {
        _ipAddress = IPAddress.Parse(address);
        _port = port;
        _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    }

    public void Connect()
    {
        try
        {
            _socket.Connect(_ipAddress, _port);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public void Send(string message)
    {
        if (_socket == null || !_socket.Connected)
            return;

        byte[] data = Encoding.ASCII.GetBytes(message);
        _socket.Send(data);
    }

    public byte[]? Receive()
    {
        if (_socket == null || !_socket.Connected)
            return null;
        try
        {
            byte[] buffer = new byte[1024];
            int bytesReceived = _socket.Receive(buffer);
            return buffer;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }

    public void Close()
    {
        _socket.Shutdown(SocketShutdown.Both);
        _socket.Close();
    }
}
