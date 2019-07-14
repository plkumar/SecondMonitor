using System;
using System.Xml.Serialization;

namespace SecondMonitor.DataModel.BasicProperties
{
    using System.Windows.Media;

    [Serializable]
    public sealed class ColorDto
    {
        [XmlAttribute]
        public byte Alpha { get; set; }

        [XmlAttribute]
        public byte Red { get; set; }

        [XmlAttribute]
        public byte Green { get; set; }

        [XmlAttribute]
        public byte Blue { get; set; }

        public Color ToColor()
        {
            return Color.FromArgb(Alpha, Red, Green, Blue);
        }

        public SolidColorBrush ToSolidColorBrush()
        {
            return new SolidColorBrush(ToColor());
        }

        public static ColorDto FromColor(Color color)
        {
            return new ColorDto()
            {
                Alpha = color.A,
                Blue = color.B,
                Green = color.G,
                Red = color.R,
            };
        }

        public static ColorDto FromHex(string hex)
        {
            hex = hex.Replace("#", "");
            return new ColorDto()
            {
                Alpha = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.AllowHexSpecifier),
                Red = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.AllowHexSpecifier),
                Green = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.AllowHexSpecifier),
                Blue = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.AllowHexSpecifier),
            };
        }
    }
}