using System;
using System.Windows.Controls;
using Telerik.Windows.Controls;

namespace YesEquality.Views
{
    public partial class BadgeListView : UserControl
    {
        public BadgeListView()
        {
            InitializeComponent();
         
            // Setup tilt animation
            InteractionEffectManager.AllowedTypes.Add(typeof(RadDataBoundListBoxItem));
        }
    }
}
