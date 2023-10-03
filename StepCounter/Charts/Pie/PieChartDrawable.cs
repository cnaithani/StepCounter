using System;
using CT = System.Drawing;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Converters;
using Microsoft.Maui.Graphics.Text;

namespace StepCounter.Charts.Pie
{
	internal class PieChartDrawable : View, IDrawable
    {

        public static readonly BindableProperty DisplayProperty = BindableProperty.Create(nameof(Display),
        typeof(string),
        typeof(PieChartDrawable),
        string.Empty);
        public string Display
        {
            get { return (string)GetValue(DisplayProperty); }
            set {
                SetValue(DisplayProperty, value);
                OnPropertyChanged(nameof(DisplayProperty));

            }
        }


        public static readonly BindableProperty BackGroundColorProperty = BindableProperty.Create(nameof(BackGroundColor),
        typeof(Color),
        typeof(PieChartDrawable),
        Color.FromRgba(255, 255, 255, 125));
        public Color BackGroundColor
        {
            get { return (Color)GetValue(BackGroundColorProperty); }
            set
            {
                SetValue(BackGroundColorProperty, value);
                OnPropertyChanged(nameof(BackGroundColorProperty));
            }
        }

        public static readonly BindableProperty PrimaryColorProperty = BindableProperty.Create(nameof(PrimaryColor),
        typeof(Color),
        typeof(PieChartDrawable),
        Color.FromRgba(97, 1, 238, 125));
        public Color PrimaryColor
        {
            get { return (Color)GetValue(PrimaryColorProperty); }
            set
            {
                SetValue(PrimaryColorProperty, value);
                OnPropertyChanged(nameof(PrimaryColorProperty));
            }
        }

        public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor),
        typeof(Color),
        typeof(PieChartDrawable),
        Color.FromRgba(255, 255, 255, 255));
        public Color TextColor
        {
            get { return (Color)GetValue(TextColorProperty); }
            set
            {
                SetValue(TextColorProperty, value);
                OnPropertyChanged(nameof(TextColorProperty));
            }
        }

        public static readonly BindableProperty PercentProperty = BindableProperty.Create(nameof(Percent),
        typeof(decimal),
        typeof(PieChartDrawable),
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


        /// <summary>
        /// Converts degrees around a circle to a Point
        /// </summary>
        /// <param name="degrees">degree around a circle from zero to 360</param>
        /// <param name="radius">distance from the center of the circle</param>
        /// <param name="rect">rectange that contains the circle</param>
        /// <returns></returns>
        private PointF PointFromDegrees(float degrees, float radius, RectF rect, int padding = 0)
        {
            const int offset = 90;
            var x = (float)(rect.Center.X + (radius + padding) * Math.Cos((degrees - offset) * (Math.PI / 180)));
            var y = (float)(rect.Center.Y + (radius + padding) * Math.Sin((degrees - offset) * (Math.PI / 180)));
            return new PointF(x, y);
        }
        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.ResetState();

            var primaryColorLight = PrimaryColor.AddLuminosity((float).5);
            var primaryColorDark = PrimaryColor.AddLuminosity((float)1);
            var backGroundColor = BackGroundColor;
            canvas.FontColor = TextColor;

            int diameter = (int)System.Math.Min(dirtyRect.Width, dirtyRect.Height);
            int offset = diameter / 2;
            var radius = diameter + diameter / 4;
            var center = new PointF(offset + dirtyRect.Center.X + offset / 2 + offset/5, offset + dirtyRect.Center.Y + offset / 2 + offset / 5);

            //Draw Outer Circle 
            var radialGradientPaint = new RadialGradientPaint
            {
                EndColor = primaryColorLight,
                StartColor = primaryColorLight
            };

            var radialRectangle = new RectF(dirtyRect.Center.X - radius, dirtyRect.Center.Y - radius, radius * 2, radius * 2);
            canvas.SetFillPaint(radialGradientPaint, radialRectangle);
            canvas.FillCircle(center, radius);


            //Draw Inner Circle 
            var radialGradientPaintInner = new RadialGradientPaint
            {
                EndColor = backGroundColor,
                StartColor = backGroundColor
            };

            var radialRectangleInner = new RectF(dirtyRect.Center.X - radius + 100, dirtyRect.Center.Y - radius + 100, radius * 2 - 200, radius * 2 - 200);
            canvas.SetFillPaint(radialGradientPaintInner, radialRectangleInner);
            canvas.FillCircle(center, radius-100);

            canvas.StrokeColor = TextColor;
            canvas.FontSize = 96;
            canvas.FontColor = TextColor;
            canvas.DrawString(Display== string.Empty || Display == "0" ? "Fetching..": Display,
                    offset + dirtyRect.Center.X + offset / 2 + offset / 5, offset + dirtyRect.Center.Y + offset / 2 + offset / 5,
                   HorizontalAlignment.Center);

            canvas.StrokeColor = TextColor;
            canvas.FontSize = 48;
            canvas.FontColor = TextColor;
            canvas.DrawString(Display == string.Empty || Display == "0" ? "" : "Steps",
                    offset + dirtyRect.Center.X + offset / 2 + offset / 5, offset + dirtyRect.Center.Y + offset / 2 + offset / 5 + 50,
                   HorizontalAlignment.Center);

            /*
            var scale = 100f / Points.Select(x => x.Value).Sum();
            */
            /*
            //Draw first initial line
            canvas.StrokeColor = Colors.White;
            canvas.DrawLine(
                new PointF(center.X, center.Y - radius),
                center);
            */
            /*
            var lineDegrees = 0f;
            var textDegrees = 0f;
            var textRadiusPadding = Convert.ToInt32(dirtyRect.Width / 10);
            //Draw splits into pie using 𝝅
            
            for (var i = 0; i < Points.Count; i++)
            {
                var point = Points.ElementAt(i);
                lineDegrees += 360 * (point.Value * scale / 100);
                textDegrees += (360 * (point.Value * scale / 100) / 2);
                var lineStartingPoint = PointFromDegrees(lineDegrees, radius, dirtyRect);
                var textPoint = PointFromDegrees(textDegrees, radius, dirtyRect, textRadiusPadding);
                var valuePoint = new PointF(textPoint.X, textPoint.Y + 15);
                canvas.DrawLine(
                        lineStartingPoint,
                        center);
                canvas.DrawString(point.Key,
                    textPoint.X,
                    textPoint.Y,
                    HorizontalAlignment.Center);
                canvas.DrawString(point.Value.ToString(),
                    valuePoint.X,
                    valuePoint.Y,
                   HorizontalAlignment.Center);
                textDegrees += (360 * (point.Value * scale / 100) / 2);
            }
            */
        }
    }
}

