using System;
using Microsoft.Maui.Graphics.Text;

namespace StepCounter.Charts.Pie
{
    internal class PieChartGraphicsView : GraphicsView
    {

        static Color primaryColor;
        static Color backGroundColor;
        static Color textColor;

        public static readonly BindableProperty DisplayProperty = BindableProperty.Create(nameof(Display),
        typeof(string),
        typeof(PieChartGraphicsView),
        string.Empty,
        propertyChanged: async (bindable, oldValue, newValue) =>
        {
            var chartView = ((PieChartGraphicsView)bindable);
            chartView.PieChartDrawable.Display = (string)newValue;

            GetColors();
            chartView.PieChartDrawable.BackGroundColor = backGroundColor;
            chartView.PieChartDrawable.PrimaryColor = primaryColor;
            chartView.PieChartDrawable.TextColor = textColor;

            chartView.Invalidate();
        });
        public String Display
        {
            get => (string)GetValue(DisplayProperty);
            set => SetValue(DisplayProperty, value);
        }

        private static void GetColors()
        {
            var colorDict = Application.Current.Resources.MergedDictionaries.Where(x => x.Source.ToString().Contains("Colors"))?.First();
            backGroundColor = (Color)colorDict["SurfaceL"];
            primaryColor = (Color)colorDict["PrimaryL"];
            textColor = (Color)colorDict["OnSurfaceL"];
        }


        public static readonly BindableProperty PercentProperty = BindableProperty.Create(nameof(Percent),
        typeof(decimal),
        typeof(PieChartGraphicsView),
        (Decimal)0.0);
        public decimal Percent
        {
            get { return (decimal)GetValue(PercentProperty); }
            set
            {
                SetValue(PercentProperty, value);
                OnPropertyChanged(nameof(PercentProperty));

            }
        }


        public PieChartGraphicsView() => Drawable = PieChartDrawable;
        public PieChartDrawable PieChartDrawable = new PieChartDrawable();
    }
}

