namespace SecondMonitor.ViewModels.Colors
{
    using DataModel.BasicProperties;

    public class BasicColorPaletteProvider : AbstractColorPaletteProvider
    {
        public BasicColorPaletteProvider()
        {
            Colors = new[]
            {
                ColorDto.FromHex("#ffe6194B"),
                ColorDto.FromHex("#ff3cb44b"),
                ColorDto.FromHex("#ffffe119"),
                ColorDto.FromHex("#ff4363d8"),
                ColorDto.FromHex("#fff58231"),
                ColorDto.FromHex("#ff911eb4"),
                ColorDto.FromHex("#ff42d4f4"),
                ColorDto.FromHex("#fff032e6"),
                ColorDto.FromHex("#ffbfef45"),
                ColorDto.FromHex("#fffabebe"),
                ColorDto.FromHex("#ff469990"),
                ColorDto.FromHex("#ffe6beff"),
                ColorDto.FromHex("#ff9A6324"),
                ColorDto.FromHex("#fffffac8"),
                ColorDto.FromHex("#ff800000"),
                ColorDto.FromHex("#ffaaffc3"),
                ColorDto.FromHex("#ff808000"),
                ColorDto.FromHex("#ffffd8b1"),
                ColorDto.FromHex("#ff000075"),

            };
        }

        protected override ColorDto[] Colors { get; }
    }
}