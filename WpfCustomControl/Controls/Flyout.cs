﻿using System;
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
        private SplineDoubleKeyFrame _fadeOutFrame;
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

            if (_flyoutRoot is null)
                return;

            _flyoutContent = GetTemplateChild("PART_Content") as FrameworkElement;
            _showStoryboard = GetTemplateChild("ShowStoryboard") as Storyboard;
            _hideStoryboard = GetTemplateChild("HideStoryboard") as Storyboard;
            _hideFrame = GetTemplateChild("hideFrame") as SplineDoubleKeyFrame;
            _hideFrameY = GetTemplateChild("hideFrameY") as SplineDoubleKeyFrame;
            _showFrame = GetTemplateChild("showFrame") as SplineDoubleKeyFrame;
            _showFrameY = GetTemplateChild("showFrameY") as SplineDoubleKeyFrame;
            _fadeOutFrame = GetTemplateChild("fadeOutFrame") as SplineDoubleKeyFrame;

            if (_hideFrame is null || _showFrame is null || _hideFrameY is null || _showFrameY is null)
                return;

            ApplyAnimation(Position, AnimateOpacity);
        }

        public static readonly RoutedEvent IsOpenChangedEvent = EventManager.RegisterRoutedEvent(nameof(IsOpenChanged), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Flyout));

        public event RoutedEventHandler IsOpenChanged
        {
            add { AddHandler(IsOpenChangedEvent, value); }
            remove { RemoveHandler(IsOpenChangedEvent, value); }
        }

        public static readonly DependencyProperty AnimateOpacityProperty = DependencyProperty.Register(
            nameof(AnimateOpacity),
            typeof(bool),
            typeof(Flyout),
            new FrameworkPropertyMetadata(false, UpdateOpacityChange));

        private static void UpdateOpacityChange(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if (!(dependencyObject is Flyout flyout))
                return;

            if (flyout._flyoutRoot is null
                || flyout._fadeOutFrame is null
                || System.ComponentModel.DesignerProperties.GetIsInDesignMode(flyout))
                return;

            if (!flyout.AnimateOpacity)
            {
                flyout._fadeOutFrame.Value = 1;
                flyout._flyoutRoot.Opacity = 1;
            }
            else
            {
                flyout._fadeOutFrame.Value = 0;
                if (!flyout.IsOpen)
                {
                    flyout._flyoutRoot.Opacity = 0;
                }
            }
        }

        /// <summary>
        /// Gets or sets whether this <see cref="Flyout"/> animates the opacity when opening/closing the <see cref="Flyout"/>.
        /// </summary>
        public bool AnimateOpacity
        {
            get => (bool)GetValue(AnimateOpacityProperty);
            set => SetValue(AnimateOpacityProperty, value);
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
                        flyout.ApplyAnimation(flyout.Position, flyout.AnimateOpacity);
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

            flyout.RaiseEvent(new RoutedEventArgs(IsOpenChangedEvent, flyout));
        }

        private void ApplyAnimation(Position position, bool animateOpacity, bool resetShowFrame = true)
        {
            if (_flyoutRoot is null
                || _hideFrame is null
                || _showFrame is null
                || _hideFrameY is null
                || _showFrameY is null)
                return;

            _showFrame.Value = 0;

            if (!animateOpacity)
            {
                _fadeOutFrame.Value = 1;
                _flyoutRoot.Opacity = 1;
            }
            else
            {
                _fadeOutFrame.Value = 0;
                if (!IsOpen)
                {
                    _flyoutRoot.Opacity = 0;
                }
            }

            switch (position)
            {
                default:
                    this.HorizontalAlignment = this.Margin.Right <= 0 ? this.HorizontalContentAlignment != HorizontalAlignment.Stretch ? HorizontalAlignment.Left : this.HorizontalContentAlignment : HorizontalAlignment.Stretch;
                    this.VerticalAlignment = VerticalAlignment.Stretch;
                    this._hideFrame.Value = -this._flyoutRoot.ActualWidth - this.Margin.Left;
                    if (resetShowFrame)
                    {
                        this._flyoutRoot.RenderTransform = new TranslateTransform(-this._flyoutRoot.ActualWidth, 0);
                    }

                    break;
                case Position.Right:
                    this.HorizontalAlignment = this.Margin.Left <= 0 ? this.HorizontalContentAlignment != HorizontalAlignment.Stretch ? HorizontalAlignment.Right : this.HorizontalContentAlignment : HorizontalAlignment.Stretch;
                    this.VerticalAlignment = VerticalAlignment.Stretch;
                    this._hideFrame.Value = this._flyoutRoot.ActualWidth + this.Margin.Right;
                    if (resetShowFrame)
                    {
                        this._flyoutRoot.RenderTransform = new TranslateTransform(this._flyoutRoot.ActualWidth, 0);
                    }

                    break;
                case Position.Top:
                    this.HorizontalAlignment = HorizontalAlignment.Stretch;
                    this.VerticalAlignment = this.Margin.Bottom <= 0 ? this.VerticalContentAlignment != VerticalAlignment.Stretch ? VerticalAlignment.Top : this.VerticalContentAlignment : VerticalAlignment.Stretch;
                    this._hideFrameY.Value = -this._flyoutRoot.ActualHeight - 1 - this.Margin.Top;
                    if (resetShowFrame)
                    {
                        this._flyoutRoot.RenderTransform = new TranslateTransform(0, -this._flyoutRoot.ActualHeight - 1);
                    }

                    break;
                case Position.Bottom:
                    this.HorizontalAlignment = HorizontalAlignment.Stretch;
                    this.VerticalAlignment = this.Margin.Top <= 0 ? this.VerticalContentAlignment != VerticalAlignment.Stretch ? VerticalAlignment.Bottom : this.VerticalContentAlignment : VerticalAlignment.Stretch;
                    this._hideFrameY.Value = this._flyoutRoot.ActualHeight + this.Margin.Bottom;
                    if (resetShowFrame)
                    {
                        this._flyoutRoot.RenderTransform = new TranslateTransform(0, this._flyoutRoot.ActualHeight);
                    }

                    break;
            }
        }

        public static readonly DependencyProperty AnimateOnPositionChangeProperty = DependencyProperty.Register(
            nameof(AnimateOnPositionChange),
            typeof(bool),
            typeof(Flyout),
            new PropertyMetadata(true));

        /// <summary>
        /// Gets or sets whether this <see cref="Flyout"/> uses the open/close animation when changing the <see cref="Position"/> property (default is true).
        /// </summary>
        public bool AnimateOnPositionChange
        {
            get => (bool)GetValue(AnimateOnPositionChangeProperty);
            set => SetValue(AnimateOnPositionChangeProperty, value);
        }

        public static readonly DependencyProperty PositionProperty = DependencyProperty.Register(
            nameof(Position),
            typeof(Position),
            typeof(Flyout),
            new PropertyMetadata(Position.Left, OnPositionPropertyChanged));

        private static void OnPositionPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var flyout = (Flyout)dependencyObject;
            var wasOpen = flyout.IsOpen;
            if (wasOpen && flyout.AnimateOnPositionChange)
            {
                flyout.ApplyAnimation((Position)e.NewValue, flyout.AnimateOpacity);
                VisualStateManager.GoToState(flyout, "Hide", true);
            }
            else
            {
                flyout.ApplyAnimation((Position)e.NewValue, flyout.AnimateOpacity, false);
            }

            if (wasOpen && flyout.AnimateOnPositionChange)
            {
                flyout.ApplyAnimation((Position)e.NewValue, flyout.AnimateOpacity);
                VisualStateManager.GoToState(flyout, "Show", true);
            }
        }

        /// <summary>
        /// Gets or sets the position of this <see cref="Flyout"/> inside the <see cref="FlyoutsControl"/>.
        /// </summary>
        public Position Position
        {
            get => (Position)GetValue(PositionProperty);
            set => SetValue(PositionProperty, value);
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
