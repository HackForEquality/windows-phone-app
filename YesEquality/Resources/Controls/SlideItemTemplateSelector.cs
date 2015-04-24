using System;
using System.Windows;
using Telerik.Windows.Controls;
using YesEquality.Models;

namespace YesEquality.Controls
{
    public class SlideItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate LinkTemplate { get; set; }
        public DataTemplate DefaultTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var currentItem = item as SlideModel;
            if (currentItem == null) return DefaultTemplate;

            if (currentItem.Type == SlideModelType.HasLink)
            {
                return LinkTemplate;
            }

            return DefaultTemplate;
        }
    }
}
