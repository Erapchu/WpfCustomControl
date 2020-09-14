using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace WpfCustomControl
{
    class SpinnerImage : Image
    {
        public SpinnerImage()
        {
            this.Source = new BitmapImage(new Uri("pack://application:,,,/WpfEmptyApp;component/searchx64.png"));
            this.Name = "Spinner";
            this.Width = 64;
            this.Height = 64;

            NameScope.SetNameScope(this, new NameScope());
            RotateTransform myRotateTransform = new RotateTransform(0);
            this.RenderTransform = myRotateTransform;
            this.RenderTransformOrigin = new Point(0.5, 0.5);
            this.RegisterName("MyAnimatedTransform", myRotateTransform);

            DoubleAnimation myRotateAboutCenterAnimation = new DoubleAnimation();
            Storyboard.SetTargetName(myRotateAboutCenterAnimation, "MyAnimatedTransform");
            Storyboard.SetTargetProperty(myRotateAboutCenterAnimation, new PropertyPath(RotateTransform.AngleProperty));
            myRotateAboutCenterAnimation.From = 0.0;
            myRotateAboutCenterAnimation.By = 10;
            myRotateAboutCenterAnimation.To = 360;
            myRotateAboutCenterAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(1000));
            myRotateAboutCenterAnimation.RepeatBehavior = RepeatBehavior.Forever;

            // Create a Storyboard to contain the animations and
            // add the animations to the Storyboard.
            Storyboard myStoryboard = new Storyboard();
            myStoryboard.Children.Add(myRotateAboutCenterAnimation);

            // Create an EventTrigger and a BeginStoryboard action to
            // start the storyboard.
            EventTrigger myEventTrigger = new EventTrigger();
            myEventTrigger.RoutedEvent = FrameworkElement.LoadedEvent;
            myEventTrigger.SourceName = Name;
            BeginStoryboard myBeginStoryboard = new BeginStoryboard();
            myBeginStoryboard.Storyboard = myStoryboard;
            myEventTrigger.Actions.Add(myBeginStoryboard);

            this.Triggers.Add(myEventTrigger);
        }
    }
}
