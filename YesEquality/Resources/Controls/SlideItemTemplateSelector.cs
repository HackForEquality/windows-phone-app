using System;
using System.Windows;
using Telerik.Windows.Controls;
using YesEquality.Models;

namespace YesEquality.Controls
{
    public class SlideItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate LinkItemTemplateTwo { get; set; }
        public DataTemplate NormalItemTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var currentItem = item as SlideModel;
            if(currentItem == null) return NormalItemTemplate;

            if (currentItem.Type == SlideModelType.HasLink)
            {
                return LinkItemTemplateTwo;
            }
            
            return NormalItemTemplate;
        }
    }
}
