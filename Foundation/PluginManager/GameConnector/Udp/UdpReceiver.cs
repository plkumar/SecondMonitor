namespace SecondMonitor.PluginManager.GameConnector.Udp
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;
    using System.Threading.Tasks;
    using Contracts.Async;

    public class UdpReceiver
    {

        private readonly int _port;
        private readonly Lazy<UdpClient> _udpClientLazy;

        public UdpReceiver(int port)
        {
            _port = port;
            _udpClientLazy = new Lazy<UdpClient>(CreateUdpClient);
        }

        private UdpClient UdpClient => _udpClientLazy.Value;

        private UdpClient CreateUdpClient()
        {
            IPEndPoint sockedAddress = new IPEndPoint(IPAddress.Any, _port);
            UdpClient udpClient = new UdpClient();
            udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            udpClient.ExclusiveAddressUse = false;
            udpClient.Client.Bind(sockedAddress);
            return udpClient;
        }

        public async Task<UdpReceiveResult> Receive(CancellationToken cancellationToken)
        {
            return await UdpClient.ReceiveAsync().WithCancellation(cancellationToken);
        }
    }
}