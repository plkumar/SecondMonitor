namespace SecondMonitor.F12019Connector
{
    using System;
    using System.Linq;
    using Datamodel;
    using PluginManager.Extensions;

    internal class DataSetCompositor
    {
        private bool _resetWhenData;

        private readonly Action<AllPacketsComposition> _sessionStartedHandler;
        private readonly Action<AllPacketsComposition> _newDataHandler;

        private bool _isPacketCarSetupDataFilled;
        private bool _isPacketCarStatusDataFilled;
        private bool _isPacketCarTelemetryDataFilled;
        private bool _isPacketParticipantsDataFilled;
        private bool _isPacketLapDataFilled;
        private bool _isPacketMotionDataFilled;
        private bool _isPacketSessionDataFilled;

        public DataSetCompositor(Action<AllPacketsComposition> sessionStartedHandler, Action<AllPacketsComposition> newDataHandler)
        {
            _sessionStartedHandler = sessionStartedHandler;
            _newDataHandler = newDataHandler;
            AllPacketsComposition = new AllPacketsComposition();
            _resetWhenData = true;
        }


        private AllPacketsComposition AllPacketsComposition { get; }

        private bool IsAllPacketsFilled =>
            _isPacketCarSetupDataFilled &&
            _isPacketCarStatusDataFilled &&
            _isPacketCarTelemetryDataFilled &&
            _isPacketParticipantsDataFilled &&
            _isPacketLapDataFilled &&
            _isPacketMotionDataFilled &&
            _isPacketSessionDataFilled;

        private void Reset()
        {
            _isPacketCarSetupDataFilled = false;
            _isPacketCarStatusDataFilled = false;
            _isPacketCarTelemetryDataFilled = false;
            _isPacketParticipantsDataFilled = false;
            _isPacketLapDataFilled = false;
            _isPacketMotionDataFilled = false;
            _isPacketSessionDataFilled = false;
            AllPacketsComposition.AdditionalData.RetiredDrivers = Enumerable.Repeat(false, 20).ToArray();
            _resetWhenData = true;
        }

        internal void ProcessPacket(PacketParticipantsData packetParticipantsData)
        {
            AllPacketsComposition.PacketParticipantsData = packetParticipantsData;
            _isPacketParticipantsDataFilled = true;
        }

        internal void ProcessPacket(PacketEventData packetEventData)
        {

            if (packetEventData.MEventStringCode.FromArray() == "SSTA")
            {
                Reset();
                return;
            }

            if (packetEventData.MEventStringCode.FromArray() == "RTMT")
            {
                AllPacketsComposition.AdditionalData.RetiredDrivers[packetEventData.MEventDetails.retirement.vehicleIdx] = true;
                return;
            }
        }

        internal void ProcessPacket(PacketCarSetupData packetCarSetupData)
        {
            AllPacketsComposition.PacketCarSetupData = packetCarSetupData;
            _isPacketCarSetupDataFilled = true;
        }

        public void ProcessPacket(PacketMotionData packetMotionData)
        {
            AllPacketsComposition.PacketMotionData = packetMotionData;
            _isPacketMotionDataFilled = true;
        }

        internal void ProcessPacket(PacketCarTelemetryData packetCarTelemetryData)
        {
            AllPacketsComposition.PacketCarTelemetryData = packetCarTelemetryData;
            _isPacketCarTelemetryDataFilled = true;
        }

        internal void ProcessPacket(PacketCarStatusData packetCarStatusData)
        {
            AllPacketsComposition.PacketCarStatusData = packetCarStatusData;
            _isPacketCarStatusDataFilled = true;

        }

        internal void ProcessPacket(PacketSessionData packetSessionData)
        {
            AllPacketsComposition.PacketSessionData = packetSessionData;
            _isPacketSessionDataFilled = true;
        }

        internal void ProcessPacket(PacketLapData packetLapData)
        {
            AllPacketsComposition.PacketLapData = packetLapData;
            _isPacketLapDataFilled = true;
            HandleCompletePacket();
        }

        private void HandleCompletePacket()
        {
            if (!IsAllPacketsFilled)
            {
                return;
            }

            if (_resetWhenData)
            {
                _sessionStartedHandler(AllPacketsComposition);
                _resetWhenData = false;
                return;
            }

            _newDataHandler(AllPacketsComposition);
        }
    }
}