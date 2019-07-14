namespace SecondMonitor.ViewModels.Colors
{
    using DataModel.BasicProperties;

    public class BlueGreenGradientPalette : AbstractColorPaletteProvider
    {
        public BlueGreenGradientPalette()
        {
            Colors = new[]
            {
                ColorDto.FromHex("#FF000FFF"),
                ColorDto.FromHex("#ff035BE8"),
                ColorDto.FromHex("#ff0795D1"),
                ColorDto.FromHex("#ff09BAB6"),
                ColorDto.FromHex("#ff0BA36F"),
                ColorDto.FromHex("#ff0C8D37"),
            };
        }

        protected override ColorDto[] Colors { get; }
    }
}