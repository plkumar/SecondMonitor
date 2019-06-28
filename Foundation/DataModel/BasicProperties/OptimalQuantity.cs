namespace SecondMonitor.DataModel.BasicProperties
{
    using System;
    using ProtoBuf;

    [Serializable]
    [ProtoContract]
    public sealed class OptimalQuantity<T> where T : class, IQuantity, new()
    {

        [ProtoMember(1)]
        public T ActualQuantity { get; set; } = (T)new T().ZeroQuantity;

        [ProtoMember(2)]
        public T IdealQuantity { get; set; } = (T)new T().ZeroQuantity;

        [ProtoMember(3)]
        public T IdealQuantityWindow { get; set; } = (T)new T().ZeroQuantity;

    }
}