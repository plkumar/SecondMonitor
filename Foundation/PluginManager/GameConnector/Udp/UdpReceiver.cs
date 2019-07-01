namespace SecondMonitor.PluginManager.GameConnector.Udp
{
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;
    using System.Threading.Tasks;
    using Contracts.Async;

    public class UdpReceiver
    {
        private readonly IPEndPoint _sockedAddress;
        private readonly UdpClient _udpClient;

        public UdpReceiver(int port)
        {
            _sockedAddress = new IPEndPoint(IPAddress.Any, port);
            _udpClient = new UdpClient();
            _udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            _udpClient.ExclusiveAddressUse = false;
            _udpClient.Client.Bind(_sockedAddress);
        }

        public async Task<UdpReceiveResult> Receive(CancellationToken cancellationToken)
        {
            return await _udpClient.ReceiveAsync().WithCancellation(cancellationToken);
        }
    }
}