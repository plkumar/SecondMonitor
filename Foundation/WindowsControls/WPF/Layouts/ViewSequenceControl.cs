namespace SecondMonitor.WindowsControls.WPF.Layouts
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Animation;
    using ViewModels;

    public class ViewSequenceControl : Control
    {
        private const string NextButtonName = "PART_NextButton";
        private const string PreviousButtonName = "PART_PreviousButton";
        private const string Grid1Name = "PART_Grid1";
        private const string Grid2Name = "PART_Grid2";

        private static readonly TimeSpan AnimationTime = TimeSpan.FromSeconds(1);

        public static readonly DependencyProperty CurrentView1Property = DependencyProperty.Register("CurrentView1", typeof(IViewModel), typeof(ViewSequenceControl), new PropertyMetadata(CurrentViewPropertyChanged));
        public static readonly DependencyProperty CurrentView2Property = DependencyProperty.Register("CurrentView2", typeof(IViewModel), typeof(ViewSequenceControl), new PropertyMetadata(CurrentViewPropertyChanged));
        public static readonly DependencyProperty ViewsProperty = DependencyProperty.Register("Views", typeof(List<IViewModel>), typeof(ViewSequenceControl), new PropertyMetadata(ViewPropertyChanged));
        public static readonly DependencyProperty IsNextButtonEnabledProperty = DependencyProperty.Register("IsNextButtonEnabled", typeof(bool), typeof(ViewSequenceControl), new PropertyMetadata(default(bool)));
        public static readonly DependencyProperty IsPreviousButtonEnabledProperty = DependencyProperty.Register("IsPreviousButtonEnabled", typeof(bool), typeof(ViewSequenceControl));
        public static readonly DependencyProperty CloseButtonCommandProperty = DependencyProperty.Register("CloseButtonCommand", typeof(ICommand), typeof(ViewSequenceControl), new PropertyMetadata(default(ICommand)));
        public static readonly DependencyProperty StartFromLastProperty = DependencyProperty.Register("StartFromLast", typeof(bool), typeof(ViewSequenceControl), new PropertyMetadata(ViewPropertyChanged));



        public ICommand CloseButtonCommand
        {
            get => (ICommand) GetValue(CloseButtonCommandProperty);
            set => SetValue(CloseButtonCommandProperty, value);
        }

        private bool _isSecondViewActive;
        private Grid _grid1;
        private TranslateTransform _grid1Transform;
        private Grid _grid2;
        private TranslateTransform _grid2Transform;

        static ViewSequenceControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ViewSequenceControl), new
                FrameworkPropertyMetadata(typeof(ViewSequenceControl)));
        }

        public bool IsPreviousButtonEnabled
        {
            get => (bool)GetValue(IsPreviousButtonEnabledProperty);
            set => SetValue(IsPreviousButtonEnabledProperty, value);
        }

        public bool IsNextButtonEnabled
        {
            get => (bool)GetValue(IsNextButtonEnabledProperty);
            set => SetValue(IsNextButtonEnabledProperty, value);
        }

        public bool StartFromLast
        {
            get => (bool)GetValue(StartFromLastProperty);
            set => SetValue(StartFromLastProperty, value);
        }

        public List<IViewModel> Views
        {
            get => (List<IViewModel>)GetValue(ViewsProperty);
            set => SetValue(ViewsProperty, value);
        }

        public IViewModel CurrentView1
        {
            get => (IViewModel)GetValue(CurrentView1Property);
            set => SetValue(CurrentView1Property, value);
        }

        public IViewModel CurrentView2
        {
            get => (IViewModel)GetValue(CurrentView2Property);
            set => SetValue(CurrentView2Property, value);
        }

        private int CurrentViewIndex { get; set; }


        protected override void OnTemplateChanged(ControlTemplate oldTemplate, ControlTemplate newTemplate)
        {
            Unsubscribe(oldTemplate);
            base.OnTemplateChanged(oldTemplate, newTemplate);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Subscribe(Template);
        }

        private void Unsubscribe(ControlTemplate template)
        {
            if (template == null)
            {
                return;
            }

            if (template.FindName(NextButtonName, this) is Button nextButton)
            {
                nextButton.Click -= NextButtonClick;
            }

            if (template.FindName(PreviousButtonName, this) is Button previousButton)
            {
                previousButton.Click -= PreviousButtonClick;
            }
        }

        private void Subscribe(ControlTemplate template)
        {
            if (template == null)
            {
                return;
            }

            if (template.FindName(NextButtonName, this) is Button nextButton)
            {
                nextButton.Click += NextButtonClick;
            }

            if (template.FindName(PreviousButtonName, this) is Button previousButton)
            {
                previousButton.Click += PreviousButtonClick;
            }


            _grid1 = template.FindName(Grid1Name, this) as Grid;
            if (_grid1 != null)
            {
                _grid1Transform = new TranslateTransform(0,0);
                _grid1.RenderTransform = _grid1Transform;
            }

            _grid2 = template.FindName(Grid2Name, this) as Grid;
            if (_grid2 != null)
            {
                _grid2Transform = new TranslateTransform(0, 0);
                _grid2.RenderTransform = _grid2Transform;
                _grid2.Visibility = Visibility.Collapsed;
            }
        }

        private static void ViewPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ViewSequenceControl viewSequenceControl)
            {
                viewSequenceControl.InitializeViews(e.NewValue as List<IViewModel>);
            }
        }

        private static void CurrentViewPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ViewSequenceControl viewSequenceControl)
            {
                viewSequenceControl.SetNavigationButtons();
            }
        }

        private void SetNavigationButtons()
        {
            if (GetActiveView() == null)
            {
                IsNextButtonEnabled = false;
                IsPreviousButtonEnabled = false;
                return;
            }

            IsPreviousButtonEnabled = CurrentViewIndex > 0;
            IsNextButtonEnabled = CurrentViewIndex < Views.Count - 1;
        }

        private void ScrollOut(Grid grid, bool scrollLeft)
        {
            DoubleAnimation newAnimation = scrollLeft ? new DoubleAnimation(0, -ActualWidth, new Duration(AnimationTime)) : new DoubleAnimation(0, ActualWidth, new Duration(AnimationTime));
            newAnimation.EasingFunction = new SineEase(){EasingMode = EasingMode.EaseInOut};
            newAnimation.Completed += ScrollOutCompleted;
             grid.RenderTransform.BeginAnimation(TranslateTransform.XProperty, newAnimation);
        }

        private void ScrollIn(Grid grid, bool scrollLeft)
        {
            grid.Visibility = Visibility.Visible;

            DoubleAnimation newAnimation = scrollLeft ? new DoubleAnimation(ActualWidth, 0, new Duration(AnimationTime)) : new DoubleAnimation(-ActualWidth, 0, new Duration(AnimationTime));
            newAnimation.EasingFunction = new SineEase() {EasingMode = EasingMode.EaseInOut};
            grid.RenderTransform.BeginAnimation(TranslateTransform.XProperty, newAnimation);
        }

        private void MoveForward()
        {
            Grid activeGrid = GetActiveGrid();
            Grid inActiveGrid = GetInactiveGrid();
            CurrentViewIndex++;
            if (_isSecondViewActive)
            {
                _isSecondViewActive = false;
                CurrentView1 = Views[CurrentViewIndex];
            }
            else
            {
                _isSecondViewActive = true;
                CurrentView2 = Views[CurrentViewIndex];
            }


            ScrollOut(activeGrid, true);
            ScrollIn(inActiveGrid, true);
            SetNavigationButtons();
        }

        private void MoveBackward()
        {
            Grid activeGrid = GetActiveGrid();
            Grid inActiveGrid = GetInactiveGrid();

            CurrentViewIndex--;
            if (_isSecondViewActive)
            {
                _isSecondViewActive = false;
                CurrentView1 = Views[CurrentViewIndex];
            }
            else
            {
                _isSecondViewActive = true;
                CurrentView2 = Views[CurrentViewIndex];
            }

            ScrollOut(activeGrid, false);
            ScrollIn(inActiveGrid, false);
            SetNavigationButtons();
        }

        private void ScrollOutCompleted(object sender, EventArgs e)
        {
            GetInactiveGrid().Visibility = Visibility.Collapsed;
            SetNavigationButtons();
        }

        private void InitializeViews(List<IViewModel> views)
        {
            if (views == null || views.Count == 0)
            {
                CurrentView1 = null;
                return;
            }

            CurrentViewIndex = StartFromLast ? views.Count - 1 : 0;

            CurrentView1 = views[CurrentViewIndex];
        }

        private IViewModel GetActiveView()
        {
            return _isSecondViewActive ? CurrentView2 : CurrentView1;
        }

        private IViewModel GetInactiveView()
        {
            return _isSecondViewActive ? CurrentView1 : CurrentView2;
        }

        private Grid GetActiveGrid()
        {
            return _isSecondViewActive ? _grid2 : _grid1;
        }

        private Grid GetInactiveGrid()
        {
            return _isSecondViewActive ? _grid1 : _grid2;
        }

        private void PreviousButtonClick(object sender, RoutedEventArgs e)
        {
            MoveBackward();
        }

        private void NextButtonClick(object sender, RoutedEventArgs e)
        {
            MoveForward();
        }
    }
}