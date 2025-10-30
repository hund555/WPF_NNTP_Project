using System.IO;
using System.Net.Sockets;
using System.Text;

namespace WPF_NNTP_Project.Models
{
    /// <summary>
    /// Dette stykke kode er lavet af Mikkel C. R. D24
    /// </summary>
    public sealed class NNTPConnection
    {
        private static readonly Lazy<NNTPConnection> _instance =
            new Lazy<NNTPConnection>(() => new NNTPConnection());

        public static NNTPConnection Instance => _instance.Value;

        private TcpClient? _client;
        private NetworkStream? _stream;
        private StreamReader? _reader;
        private StreamWriter? _writer;
        private string? _responseData;

        private NNTPConnection() { }

        public bool IsConnected => _client != null && _client.Connected;

        public async Task<string> ConnectAsync(string host, int port)
        {
            _client = new TcpClient();
            _client.ReceiveTimeout = 3000;
            _client.SendTimeout = 3000;

            await _client.ConnectAsync(host, port);

            _stream = _client.GetStream();
            _reader = new StreamReader(_stream, Encoding.ASCII);
            _writer = new StreamWriter(_stream, Encoding.ASCII) { NewLine = "\r\n", AutoFlush = true };

            _responseData = await _reader.ReadLineAsync();
            return _responseData ?? "No response";
        }

        public async Task<string> SendCommandAsync(string command)
        {
            if (_writer == null || _reader == null)
                throw new InvalidOperationException("Not connected to server.");

            await _writer.WriteLineAsync(command);
            _responseData = await _reader.ReadLineAsync();
            return _responseData ?? "";
        }

        public string LastResponse => _responseData ?? "";

        public StreamReader? Reader => _reader;
        public StreamWriter? Writer => _writer;
        public TcpClient? Client => _client;

        public void Disconnect()
        {
            _reader?.Close();
            _writer?.Close();
            _stream?.Close();
            _client?.Close();
        }
    }
}