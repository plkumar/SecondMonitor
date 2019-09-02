namespace SecondMonitor.F12019Connector
{
    using System;
    using System.Net.Sockets;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Threading.Tasks;
    using Datamodel;
    using PluginManager.GameConnector.Udp;

    internal class F12019UdpReceiver
    {
        private readonly int _port;
        private readonly UdpReceiver _udpReceiver;
        private readonly DataSetCompositor _dataSetCompositor;

        internal F12019UdpReceiver(Action<AllPacketsComposition> sessionStartedHandler, Action<AllPacketsComposition> newDataHandler, int connectionPort)
        {
            _port = connectionPort;
            _udpReceiver = new UdpReceiver(_port);
            _dataSetCompositor = new DataSetCompositor(sessionStartedHandler, newDataHandler);
        }

        public async Task ReceiveLoop(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                UdpReceiveResult receiveResult = await _udpReceiver.Receive(cancellationToken).ConfigureAwait(false);
                byte[] rawPacked = receiveResult.Buffer;
                PacketHeader header = DeserializePackedHeader(rawPacked);
                switch (header.MPacketId)
                {
                    case 0:
                        PacketMotionData packetMotionData = Deserialize<PacketMotionData>(rawPacked);
                        _dataSetCompositor.ProcessPacket(packetMotionData);
                        break;
                    case 1:
                        PacketSessionData packetSessionData = Deserialize<PacketSessionData>(rawPacked);
                        _dataSetCompositor.ProcessPacket(packetSessionData);
                        break;
                    case 2:
                        PacketLapData packetLapData = Deserialize<PacketLapData>(rawPacked);
                        _dataSetCompositor.ProcessPacket(packetLapData);
                        break;
                    case 3:
                        PacketEventData packetEventData = Deserialize<PacketEventData>(rawPacked);
                        _dataSetCompositor.ProcessPacket(packetEventData);
                        break;
                    case 4:
                        PacketParticipantsData packetParticipantsData = Deserialize<PacketParticipantsData>(rawPacked);
                        _dataSetCompositor.ProcessPacket(packetParticipantsData);
                        break;
                    case 5:
                        PacketCarSetupData packetCarSetupData = Deserialize<PacketCarSetupData>(rawPacked);
                        _dataSetCompositor.ProcessPacket(packetCarSetupData);
                        break;
                    case 6:
                        PacketCarTelemetryData packetCarTelemetryData = Deserialize<PacketCarTelemetryData>(rawPacked);
                        _dataSetCompositor.ProcessPacket(packetCarTelemetryData);
                        break;
                    case 7:
                        PacketCarStatusData packetCarStatusData = Deserialize<PacketCarStatusData>(rawPacked);
                        _dataSetCompositor.ProcessPacket(packetCarStatusData);
                        break;
                }
            }
        }

        private PacketHeader DeserializePackedHeader(byte[] rawData)
        {
            return Deserialize<PacketHeader>(rawData);
        }

        private T Deserialize<T>(byte[] rawData) where T: struct
        {
            GCHandle telemetryHandle = GCHandle.Alloc(rawData, GCHandleType.Pinned);
            try
            {
                T udpTelemetry = (T)Marshal.PtrToStructure(telemetryHandle.AddrOfPinnedObject(), typeof(T));
                return udpTelemetry;
            }
            finally
            {
                telemetryHandle.Free();
            }
        }
    }
}