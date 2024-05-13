using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows;

namespace TilerMain
{
    class Animation
    {
        static public void PageSilderMoveing(FrameworkElement canvas, int end)
        {
            DoubleAnimation animation = new()
            {
                To = end,
                Duration = new Duration(TimeSpan.FromSeconds(0.2)),
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseInOut }
            };
            canvas.BeginAnimation(Canvas.TopProperty, animation);
        }

        static public void FrameMoving(FrameworkElement widget, double? from)
        {
            DoubleAnimation animation = new()
            {
                From = from,
                To = 0,
                Duration = new Duration(TimeSpan.FromSeconds(0.2)),
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
            };
            widget.BeginAnimation(Canvas.BottomProperty, animation);
        }

        static public void RightMoving(FrameworkElement widget, double? from,double? to,double time = 0.2)
        {
            DoubleAnimation animation = new()
            {
                From = from,
                To = to,
                Duration = new Duration(TimeSpan.FromSeconds(time)),
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
            };
            widget.BeginAnimation(Canvas.RightProperty, animation);
        }

        static public void ChangeOP(FrameworkElement widget, double? start, double? end, double time)
        {
            DoubleAnimation animation = new()
            {
                From = start,
                To = end,
                Duration = new Duration(TimeSpan.FromSeconds(time))
            };
            widget.BeginAnimation(UIElement.OpacityProperty, animation);
        }
    }
}
