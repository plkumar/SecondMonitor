namespace SecondMonitor.WindowsControls.WPF.WindowFormHost
{
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Forms.Integration;
    using System.Windows.Media;
    using Point = System.Windows.Point;
    using Size = System.Windows.Size;

    public class ScrollViewerWindowsFormsHost : WindowsFormsHost
    {
        private Rect _lastBoundingBox;
        private bool _fullyIntersect = true;

        public ScrollViewerWindowsFormsHost()
        {
        }

        protected override void OnWindowPositionChanged(Rect rcBoundingBox)
        {
            if (_lastBoundingBox.Equals(rcBoundingBox))
            {
                return;
            }
            _lastBoundingBox = rcBoundingBox;
            Graphics g = this.Child.CreateGraphics();
            double xScale = g.DpiX / 96.0;
            double yScale = g.DpiY / 96.0;
            base.OnWindowPositionChanged(rcBoundingBox);
            //rcBoundingBox = new Rect(new Point(rcBoundingBox.X, rcBoundingBox.Y), new Size( rcBoundingBox.Width, rcBoundingBox.Height));
            if (ParentScrollViewer == null && !TryFindScrollViewer())
            {
                return;
            }

            GeneralTransform tr = ParentScrollViewer.TransformToAncestor(MainWindow);
            var scrollRect = new Rect( new Size(ParentScrollViewer.ViewportWidth, ParentScrollViewer.ViewportHeight));
            scrollRect = tr.TransformBounds(scrollRect);
            scrollRect = new Rect(new Point(scrollRect.X * xScale, scrollRect.Y * yScale), new Size(scrollRect.Width * xScale, scrollRect.Height * yScale));
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
                intersect = new Rect(new Point(intersect.X / xScale, intersect.Y / yScale), new Size(intersect.Width / xScale, intersect.Height / yScale));
                intersect = tr.TransformBounds(intersect);
                intersect = new Rect(new Point(intersect.X * xScale, intersect.Y * yScale), new Size(intersect.Width * xScale, intersect.Height * yScale));
                //InvalidateVisual();
            }
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