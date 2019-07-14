namespace SecondMonitor.ViewModels.Colors
{
    using DataModel.BasicProperties;

    public class RedGreenGradientPalette : AbstractColorPaletteProvider
    {
        public RedGreenGradientPalette()
        {
            Colors = new[]
            {
                ColorDto.FromHex("#ff0C8D37"),
                ColorDto.FromHex("#ff1FA30B"),
                ColorDto.FromHex("#ff1FA30B"),
                ColorDto.FromHex("#ffD1C307"),
                ColorDto.FromHex("#ffE86E03"),
                ColorDto.FromHex("#ffFF0000"),
            };
        }

        protected override ColorDto[] Colors { get; }
    }
}