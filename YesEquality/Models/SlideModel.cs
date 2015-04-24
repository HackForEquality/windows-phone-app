using System;

namespace YesEquality.Models
{
    public class SlideModel
    {
        public SlideModel()
        {
            TextColour = "White";
            Type = SlideModelType.Default;
        }

        public string Title { get; set; }
        public Uri ImagePath { get; set; }
        public string ImageText { get; set; }
        public string BackgroundColour { get; set; }
        public string TextColour { get; set; }
        public SlideModelType Type { get; set; }
    }
    
    public enum SlideModelType
    {
        Default,
        HasLink,
    }
}
