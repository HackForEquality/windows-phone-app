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

namespace YesEquality.Controls
{
    public partial class BadgePicker : UserControl
    {
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
                BadgeImage.Source = new BitmapImage(value);
            }
        }

        public BadgePicker()
        {
            InitializeComponent();
        }

        private async void BadgeImage_Tap(object sender, System.Windows.Input.GestureEventArgs e)
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
            await ImageBounce.BeginAsync().ConfigureAwait(false);
        }
    }
}
