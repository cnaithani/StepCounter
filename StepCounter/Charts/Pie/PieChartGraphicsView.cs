using System;
namespace StepCounter.Charts.Pie
{
    internal class PieChartGraphicsView : GraphicsView
    {

        public static readonly BindableProperty DisplayProperty = BindableProperty.Create(nameof(Display),
        typeof(string),
        typeof(PieChartGraphicsView),
        string.Empty,
        propertyChanged: async (bindable, oldValue, newValue) =>
        {
            var chartView = ((PieChartGraphicsView)bindable);
            chartView.PieChartDrawable.Display = (string)newValue;
        });
        public String Display
        {
            get => (string)GetValue(DisplayProperty);
            set => SetValue(DisplayProperty, value);
        }

        public PieChartGraphicsView() => Drawable = PieChartDrawable;
        public PieChartDrawable PieChartDrawable = new PieChartDrawable();
    }
}

