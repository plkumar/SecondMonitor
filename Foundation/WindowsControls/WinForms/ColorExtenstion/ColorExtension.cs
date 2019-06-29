namespace SecondMonitor.WindowsControls.WinForms.ColorExtenstion
{
    using System.Windows.Media;

    public static class ColorExtension
    {
        public static System.Drawing.Color ToWinFormColor(this Color color)
        {
            return System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
        }
    }
}