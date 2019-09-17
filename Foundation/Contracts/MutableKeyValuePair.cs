namespace SecondMonitor.Contracts
{
    using System;

    [Serializable]
    public class MutableKeyValuePair<TK, TV>
    {
        public MutableKeyValuePair()
        {

        }

        public MutableKeyValuePair(TK key, TV value)
        {
            Key = key;
            Value = value;
        }

        public TK Key
        { get; set; }

        public TV Value
        { get; set; }
    }
}