using System.Windows.Controls;

namespace SecondMonitor.WindowsControls.WPF.Layouts
{
    using System.Collections.Generic;
    using System.Windows;
    using ViewModels;

    /// <summary>
    /// Interaction logic for ViewSequenceControl.xaml
    /// </summary>
    public partial class ViewSequenceControl : UserControl
    {
        public static readonly DependencyProperty CurrentViewProperty = DependencyProperty.Register("CurrentView", typeof(IViewModel), typeof(ViewSequenceControl), new PropertyMetadata(CurrentViewPropertyChanged) );

        public static readonly DependencyProperty ViewsProperty = DependencyProperty.Register("Views", typeof(List<IViewModel>), typeof(ViewSequenceControl), new PropertyMetadata(ViewPropertyChanged));
        public static readonly DependencyProperty IsNextButtonEnabledProperty = DependencyProperty.Register("IsNextButtonEnabled", typeof(bool), typeof(ViewSequenceControl), new PropertyMetadata(default(bool)));
        public static readonly DependencyProperty IsPreviousButtonEnabledProperty = DependencyProperty.Register("IsPreviousButtonEnabled", typeof(bool), typeof(ViewSequenceControl));

        public bool IsPreviousButtonEnabled
        {
            get => (bool) GetValue(IsPreviousButtonEnabledProperty);
            set => SetValue(IsPreviousButtonEnabledProperty, value);
        }

        public bool IsNextButtonEnabled
        {
            get => (bool) GetValue(IsNextButtonEnabledProperty);
            set => SetValue(IsNextButtonEnabledProperty, value);
        }

        public List<IViewModel> Views
        {
            get => (List<IViewModel>) GetValue(ViewsProperty);
            set => SetValue(ViewsProperty, value);
        }

        public IViewModel CurrentView
        {
            get => (IViewModel) GetValue(CurrentViewProperty);
            set => SetValue(CurrentViewProperty, value);
        }

        private int CurrentViewIndex { get; set; }

        public ViewSequenceControl()
        {
            InitializeComponent();
        }

        private void PreviousButtonClick(object sender, RoutedEventArgs e)
        {
            CurrentViewIndex--;
            CurrentView = Views[CurrentViewIndex];
        }

        private void NextButtonClick(object sender, RoutedEventArgs e)
        {
            CurrentViewIndex++;
            CurrentView = Views[CurrentViewIndex];
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
            if (CurrentView == null)
            {
                IsNextButtonEnabled = false;
                IsPreviousButtonEnabled = false;
                return;
            }

            IsPreviousButtonEnabled = CurrentViewIndex > 0;
            IsNextButtonEnabled = CurrentViewIndex < Views.Count - 1;
        }

        private void InitializeViews(List<IViewModel> views)
        {
            if (views == null)
            {
                CurrentView = null;
                return;
            }

            CurrentViewIndex = 0;
            CurrentView = views[CurrentViewIndex];
        }
    }
}
