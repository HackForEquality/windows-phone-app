using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using YesEquality.Extensions;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Diagnostics;
using System.Windows.Media.Animation;
using Telerik.Windows.Controls;

namespace YesEquality.Controls
{
    public partial class BadgePicker : UserControl
    {
        private List<Point> points;
        
        private List<Uri> badges;
        public List<Uri> Badges 
        { 
            get
            {
                return badges;
            }
            set
            {
                badges = value;

                if (badges != null && badges.Any())
                {
                    SelectedBadge = badges.First();
                }
            }
        }

        private Uri selectedBadge;
        public Uri SelectedBadge
        {
            get
            {
                return selectedBadge;
            }
            set
            {
                selectedBadge = value;
                Badge.Source = new BitmapImage(value);
            }
        }
       
        public BadgePicker()
        {
            InitializeComponent();

            setupSnapPoints();

            // Set default badge position
            Canvas.SetLeft(Container, points[2].X);
            Canvas.SetTop(Container, points[2].Y);
        }

        public void ShowTooltip()
        {
            RadToolTipService.SetTimeout(Container, TimeSpan.FromSeconds(2));
            RadToolTipService.Open(Container);
        }

        private void setupSnapPoints()
        {
            // 12px margin
            points = new List<Point>();
            points.Add(new Point(12, 12)); // top left
            points.Add(new Point(268, 12)); // top right
            points.Add(new Point(12, 268)); // bottom left
            points.Add(new Point(268, 268)); // bottom right
        }

        private async void Badge_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Uri badge;
            var index = Badges.IndexOf(SelectedBadge);

            if (Badges.Count == (index + 1))
            {
                badge = Badges[0];
            }
            else
            {
                badge = Badges[index + 1];
            }

            SelectedBadge = badge;
            await BadgeBounce.BeginAsync().ConfigureAwait(false);
        }

        private async void Container_ManipulationCompleted(object sender, System.Windows.Input.ManipulationCompletedEventArgs e)
        {
            if (e.TotalManipulation.Translation.X == 0 && e.TotalManipulation.Translation.Y == 0) return;

            FrameworkElement element = sender as FrameworkElement;
            double left = Canvas.GetLeft(element);
            double top = Canvas.GetTop(element);
            var elementPosition = new Point(left, top);

            // Find nearest point
            var distances = new Dictionary<Point, Double>();
            points.ForEach((point) => distances.Add(point, distanceTo(elementPosition, point)));

            var nearest = (from distance in distances
                           orderby distance.Value ascending
                           select distance.Key).First();

            // Animate to nearest point
            BadgeSnapX.To = nearest.X;
            BadgeSnapY.To = nearest.Y;
            await BadgeSnap.BeginAsync();

            //Debug.WriteLine("Snapping to: " + nearest.X + ", " + nearest.Y);
            //Canvas.SetLeft(element, nearest.X);
            //Canvas.SetTop(element, nearest.Y);
        }

        private void Container_ManipulationDelta(object sender, System.Windows.Input.ManipulationDeltaEventArgs e)
        {
            // Close tooltip if open when dragging starts
            RadToolTipService.Close();

            FrameworkElement element = sender as FrameworkElement;
            double left = Canvas.GetLeft(element);
            double top = Canvas.GetTop(element);

            left += e.DeltaManipulation.Translation.X;
            top += e.DeltaManipulation.Translation.Y;

            //if (left < 0)
            //{
            //    left = 0;
            //}
            //else if (left > (LayoutRoot.ActualWidth - element.ActualWidth))
            //{
            //    left = LayoutRoot.ActualWidth - element.ActualWidth;
            //}

            //if (top < 0)
            //{
            //    top = 0;
            //}
            //else if (top > (LayoutRoot.ActualHeight - element.ActualHeight))
            //{
            //    top = LayoutRoot.ActualHeight - element.ActualHeight;
            //}

            Debug.WriteLine("(x,y): " + left + ", " + top);
            Canvas.SetLeft(element, left);
            Canvas.SetTop(element, top);
        }

        private double distanceTo(Point one, Point two)
        {
            return Math.Sqrt(Math.Pow(one.X - two.X, 2) + Math.Pow(one.Y - two.Y, 2));
        }
    }
}
