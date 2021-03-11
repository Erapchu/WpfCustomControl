using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace WpfCustomControl.Controls
{
    public class Flyout : ContentControl
    {
        private Storyboard _showStoryboard;
        private Storyboard _hideStoryboard;
        private SplineDoubleKeyFrame _hideFrame;
        private SplineDoubleKeyFrame _hideFrameY;
        private SplineDoubleKeyFrame _showFrame;
        private SplineDoubleKeyFrame _showFrameY;
        private FrameworkElement _flyoutRoot;
        private FrameworkElement _flyoutContent;

        static Flyout()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Flyout), new FrameworkPropertyMetadata(typeof(Flyout)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _flyoutRoot = GetTemplateChild("PART_Root") as FrameworkElement;
            _flyoutContent = GetTemplateChild("PART_Content") as FrameworkElement;
            _showStoryboard = GetTemplateChild("ShowStoryboard") as Storyboard;
            _hideStoryboard = GetTemplateChild("HideStoryboard") as Storyboard;
            _hideFrame = GetTemplateChild("hideFrame") as SplineDoubleKeyFrame;
            _hideFrameY = GetTemplateChild("hideFrameY") as SplineDoubleKeyFrame;
            _showFrame = GetTemplateChild("showFrame") as SplineDoubleKeyFrame;
            _showFrameY = GetTemplateChild("showFrameY") as SplineDoubleKeyFrame;

            ApplyAnimation();
        }

        /// <summary>
        /// Gets or sets whether this <see cref="Flyout"/> should be visible or not.
        /// </summary>
        public bool IsOpen
        {
            get => (bool)GetValue(IsOpenProperty);
            set => SetValue(IsOpenProperty, value);
        }

        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(
            nameof(IsOpen),
            typeof(bool),
            typeof(Flyout),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnIsOpenPropertyChanged));

        private static void OnIsOpenPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var flyout = (Flyout)dependencyObject;

            Action openedChangedAction = () =>
            {
                if (e.NewValue != e.OldValue)
                {
                    if ((bool)e.NewValue)
                    {
                        if (flyout._hideStoryboard != null)
                        {
                            // don't let the storyboard end it's completed event
                            // otherwise it could be hidden on start
                            flyout._hideStoryboard.Completed -= flyout.HideStoryboardCompleted;
                        }

                        flyout.Visibility = Visibility.Visible;
                        flyout.ApplyAnimation();
                        flyout.TryFocusElement();
                        if (flyout._showStoryboard != null)
                        {
                            flyout._showStoryboard.Completed += flyout.ShowStoryboardCompleted;
                        }
                    }
                    else
                    {
                        if (flyout._showStoryboard != null)
                        {
                            flyout._showStoryboard.Completed -= flyout.ShowStoryboardCompleted;
                        }

                        if (flyout._hideStoryboard != null)
                        {
                            flyout._hideStoryboard.Completed += flyout.HideStoryboardCompleted;
                        }
                        else
                        {
                            flyout.Hide();
                        }
                    }

                    VisualStateManager.GoToState(flyout, (bool)e.NewValue == false ? "Hide" : "Show", true);
                }
            };

            flyout.Dispatcher.BeginInvoke(DispatcherPriority.Background, openedChangedAction);
        }

        private void ApplyAnimation(bool resetShowFrame = true)
        {
            _showFrame.Value = 0;

            HorizontalAlignment = Margin.Right <= 0 ? HorizontalContentAlignment != HorizontalAlignment.Stretch ? HorizontalAlignment.Left : HorizontalContentAlignment : HorizontalAlignment.Stretch;
            VerticalAlignment = VerticalAlignment.Stretch;
            _hideFrame.Value = -_flyoutRoot.ActualWidth - Margin.Left;
            if (resetShowFrame)
            {
                _flyoutRoot.RenderTransform = new TranslateTransform(-_flyoutRoot.ActualWidth, 0);
            }
        }

        private void HideStoryboardCompleted(object sender, EventArgs e)
        {
            _hideStoryboard.Completed -= HideStoryboardCompleted;
            Hide();
        }

        private void ShowStoryboardCompleted(object sender, EventArgs e)
        {
            _showStoryboard.Completed -= ShowStoryboardCompleted;
        }

        private void Hide()
        {
            // hide the flyout, we should get better performance and prevent showing the flyout on any resizing events
            Visibility = Visibility.Hidden;
        }

        private void TryFocusElement()
        {
            // first focus itself
            Focus();

            if (_flyoutContent != null)
                _flyoutContent.MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
        }
    }
}
