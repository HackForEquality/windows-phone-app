using System;

namespace YesEquality.Models
{
    public class SlideModel
    {
        public string Title { get; set; }
        public Uri ImagePath { get; set; }
        public string ImageText { get; set; }
        public string BackgroundColour { get; set; }
        public SlideModelType Type { get; set; }
    }

    public enum SlideModelType
    {
        Default,
        HasLink,
    }
}
