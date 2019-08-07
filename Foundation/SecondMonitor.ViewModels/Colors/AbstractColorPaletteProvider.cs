namespace SecondMonitor.ViewModels.Colors
{
    using System.Collections.Generic;
    using System.Windows.Media;
    using DataModel.BasicProperties;

    public abstract class AbstractColorPaletteProvider : IColorPaletteProvider
    {
        private int _currentIndex;

        protected AbstractColorPaletteProvider()
        {
            _currentIndex = 0;
        }

        protected abstract ColorDto[] Colors { get; }

        public int PaletteSize => Colors.Length;

        public Color GetNext()
        {
            return GetNextAsDto().ToColor();
        }

        public ColorDto GetNextAsDto()
        {
            try
            {
                return Colors[_currentIndex];
            }
            finally
            {
                _currentIndex = (_currentIndex + 1) % Colors.Length;
            }
        }

        public IEnumerable<ColorDto> GetAllColors()
        {
            return Colors;
        }


        public void Reset()
        {
            _currentIndex = 0;
        }
    }
}