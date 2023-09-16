using System;
using Microsoft.Maui.Graphics;

namespace StepCounter.Charts.Pie
{
	internal class PieChartDrawable : View, IDrawable
    {
        int xOffset = 300;
        int yOffset = 200;
        public static readonly BindableProperty PointsProperty = BindableProperty.Create(nameof(Points),
            typeof(Dictionary<string, float>),
            typeof(PieChartDrawable),
            new Dictionary<string, float>());
        public Dictionary<string, float> Points
        {
            get => (Dictionary<string, float>)GetValue(PointsProperty);
            set => SetValue(PointsProperty, value);
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
            var radius = dirtyRect.Width ;
            var purple = Color.FromRgba(178, 127, 255, 125);
            var translucent = Color.FromRgba(235, 222, 255, 0);
            var white = Color.FromRgba(0, 0, 0, 255);
            canvas.FontColor = Color.FromArgb("#7F2CF6");
            var center = new PointF(xOffset + dirtyRect.Center.X, yOffset + dirtyRect.Center.Y);
            //Draw Outer Circle 
            var radialGradientPaint = new RadialGradientPaint
            {
                EndColor = purple,
                StartColor = purple
            };

            var radialRectangle = new RectF(dirtyRect.Center.X - radius, dirtyRect.Center.Y - radius, radius * 2, radius * 2);
            canvas.SetFillPaint(radialGradientPaint, radialRectangle);
            canvas.FillCircle(center, radius);


            //Draw Inner Circle 
            var radialGradientPaintInner = new RadialGradientPaint
            {
                EndColor = white,
                StartColor = white
            };

            var radialRectangleInner = new RectF(dirtyRect.Center.X - radius + 100, dirtyRect.Center.Y - radius + 100, radius * 2 - 200, radius * 2 - 200);
            canvas.SetFillPaint(radialGradientPaintInner, radialRectangleInner);
            canvas.FillCircle(center, radius-100);

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

