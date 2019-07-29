namespace SecondMonitor.ViewModels
{
    using System.Collections.Generic;

    public class NamedCollectionViewModel<T> : AbstractViewModel where T : IViewModel
    {
        public string Title { get; set; }

        public IReadOnlyCollection<T> ChildViewModels { get; set; }
    }
}