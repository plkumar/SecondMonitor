namespace SecondMonitor.ViewModels.Colors
{
    using System.Collections.Generic;
    using System.Windows.Media;
    using DataModel.BasicProperties;

    public interface IColorPaletteProvider
    {
        int PaletteSize { get; }

        Color GetNext();
        ColorDto GetNextAsDto();

        IEnumerable<ColorDto> GetAllColors();

        void Reset();
    }
}