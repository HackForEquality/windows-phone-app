using Caliburn.Micro.BindableAppBar;
using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace YesEquality.Controls
{
    public class ThemeableBindableAppBar : BindableAppBar
    {
        public ThemeableBindableAppBar()
        {
            this.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            // Light theme
            this.MatchOverriddenTheme();
            this.StateChanged += (o, args) => this.MatchOverriddenTheme();
            this.Invalidated += (o, args) => this.MatchOverriddenTheme();

            this.BarOpacity = 0.8;
        }
    }
}
