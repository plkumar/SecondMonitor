namespace SecondMonitor.WindowsControls.WPF.WindowFormHost
{
    using System;
    using System.Runtime.InteropServices;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Forms.Integration;
    using System.Windows.Media;

    public class ScrollViewerWindowsFormsHost : WindowsFormsHost
    {
        private Rect _lastBoundingBox;
        private bool _fullyIntersect = true;

        protected override void OnWindowPositionChanged(Rect rcBoundingBox)
        {
            if (_lastBoundingBox.Equals(rcBoundingBox))
            {
                return;
            }

            base.OnWindowPositionChanged(rcBoundingBox);

            if (ParentScrollViewer == null && !TryFindScrollViewer())
            {
                return;
            }

            GeneralTransform tr = ParentScrollViewer.TransformToAncestor(MainWindow);
            var scrollRect = new Rect(new Size(ParentScrollViewer.ViewportWidth, ParentScrollViewer.ViewportHeight));
            scrollRect = tr.TransformBounds(scrollRect);
            var intersect = Rect.Intersect(scrollRect, rcBoundingBox);
            if (intersect.Equals(rcBoundingBox))
            {
                if (!_fullyIntersect)
                {
                    InvalidateVisual();
                }

                _fullyIntersect = true;
            }
            else
            {
                _fullyIntersect = false;
            }

            if (!intersect.IsEmpty)
            {
                tr = MainWindow.TransformToDescendant(this);
                intersect = tr.TransformBounds(intersect);
                //InvalidateVisual();
            }

            _lastBoundingBox = rcBoundingBox;
            SetRegion(intersect);
        }

        protected override void OnVisualParentChanged(DependencyObject oldParent)
        {
            base.OnVisualParentChanged(oldParent);
            TryFindScrollViewer();
        }

        private bool TryFindScrollViewer()
        {
            ParentScrollViewer = null;

            var p = VisualTreeHelper.GetParent(this);
            while (p != null)
            {
                if (p is ScrollViewer)
                {
                    ParentScrollViewer = (ScrollViewer) p;
                    return true;
                }

                p = VisualTreeHelper.GetParent(p);
            }

            return false;
        }

        private void SetRegion(Rect intersect)
        {
            using (var graphics = System.Drawing.Graphics.FromHwnd(Handle))
            {
                SetWindowRgn(Handle, (new System.Drawing.Region(ConvertRect(intersect))).GetHrgn(graphics), true);
            }
        }

        static System.Drawing.RectangleF ConvertRect(Rect r)
        {
            return new System.Drawing.RectangleF((float)r.X, (float)r.Y, (float)r.Width, (float)r.Height);
        }

        private Window _mainWindow;
        Window MainWindow
        {
            get
            {
                if (_mainWindow == null)
                {
                    _mainWindow = Window.GetWindow(this);
                }

                return _mainWindow;
            }
        }

        ScrollViewer ParentScrollViewer { get; set; }

        [DllImport("User32.dll", SetLastError = true)]
        public static extern int SetWindowRgn(IntPtr hWnd, IntPtr hRgn, bool bRedraw);
    }
}