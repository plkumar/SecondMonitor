namespace SecondMonitor.WindowsControls.WPF.Behaviors
{
    using System;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Interactivity;

    public class FontSizeByHeightBehavior : Behavior<TextBlock>
    {
        private Grid _parrentGrid;

        protected override void OnAttached()
        {
            base.OnAttached();
            Subscribe();
        }

        private void Subscribe()
        {

            if (AssociatedObject == null)
            {
                return;
            }

            if (AssociatedObject.Parent is Grid grid)
            {
                _parrentGrid = grid;
                var pd =  DependencyPropertyDescriptor.FromProperty(FrameworkElement.HeightProperty, typeof(FrameworkElement));
                pd.AddValueChanged(_parrentGrid, AssociatedObjectOnSizeChanged);
            }
        }

        private void AssociatedObjectOnSizeChanged(object sender, EventArgs e)
        {
            if (AssociatedObject.Height == 0)
            {
                return;
            }

            double desiredFontSize = Math.Min(Math.Floor(_parrentGrid.Height / AssociatedObject.FontFamily.LineSpacing) * 0.8, 26);
            AssociatedObject.FontSize = desiredFontSize > 0 ? desiredFontSize : 1;
        }

        private void UnSubscribe()
        {
            if (AssociatedObject == null || _parrentGrid == null)
            {
                return;
            }

            _parrentGrid.SizeChanged -= AssociatedObjectOnSizeChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
        }
    }
}