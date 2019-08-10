namespace SecondMonitor.Contracts.SelectionCounter
{
    public class SelectionItemCounter<T>
    {
        public SelectionItemCounter(T item)
        {
            Item = item;
            Counter = 1;
        }

        public int Counter { get; private set; }

        public bool IsSelected => Counter == 0;

        public void AddSelection()
        {
            Counter++;
        }

        public bool RemoveSelection()
        {
            Counter--;
            return Counter == 0;
        }

        public T Item { get; }
    }
}