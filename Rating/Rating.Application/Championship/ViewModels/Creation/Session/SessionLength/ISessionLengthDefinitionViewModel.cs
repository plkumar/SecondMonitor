namespace SecondMonitor.Rating.Application.Championship.ViewModels.Creation.Session.SessionLength
{
    using SecondMonitor.ViewModels;

    public interface ISessionLengthDefinitionViewModel : IViewModel
    {
        string LengthKind { get;  }

        string GetTextualDescription(double layoutLength);
    }
}