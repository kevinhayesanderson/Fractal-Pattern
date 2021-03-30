using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Pattern
{
    public partial class MainWindow : Window
    {
        private readonly bool drawLine = true;
        private readonly int i = 10;
        private Point A = new();
        private Point B = new();
        private Point C = new();

        public MainWindow()
        {
            InitializeComponent();
            CalculateInitialPoints();
            DrawTriangle();
            DrawPoints(DrawRandomPoint(drawLine), drawLine);
        }

        private void CalculateInitialPoints()
        {
            Canvas.SetTop(canvas, 0);
            Canvas.SetLeft(canvas, 0);
            Canvas.SetBottom(canvas, canvas.Height);
            Canvas.SetRight(canvas, canvas.Width);

            double top = Canvas.GetTop(canvas);
            double bottom = Canvas.GetBottom(canvas);
            double left = Canvas.GetLeft(canvas);
            double right = Canvas.GetRight(canvas);

            A = new(top, right / 2);
            B = new(bottom, left);
            C = new(bottom, right);
        }

        private void DrawTriangle()
        {
            Polyline ABC = new()
            {
                Points = new PointCollection(new List<Point>() { A, B, C, A }),
                Stroke = Brushes.Red,
                StrokeThickness = 2,
                StrokeLineJoin = PenLineJoin.Bevel
            };
            canvas.Children.Add(ABC);
            Canvas.SetTop(ABC, 0);
            Canvas.SetLeft(ABC, 0);
        }

        private Point GetRandomPoint(Point A, Point B, Point C)
        {
            double r1 = new Random().NextDouble();
            double r2 = new Random().NextDouble();

            double sqrtR1 = Math.Sqrt(r1);

            double x = ((1 - sqrtR1) * A.X) + (sqrtR1 * (1 - r2) * B.X) + (sqrtR1 * r2 * C.X);
            double y = ((1 - sqrtR1) * A.Y) + (sqrtR1 * (1 - r2) * B.Y) + (sqrtR1 * r2 * C.Y);

            // https://stackoverflow.com/a/19654424
            //P(x) = (1 - sqrt(r1)) * A(x) + (sqrt(r1) * (1 - r2)) * B(x) + (sqrt(r1) * r2) * C(x)
            //P(y) = (1 - sqrt(r1)) * A(y) + (sqrt(r1) * (1 - r2)) * B(y) + (sqrt(r1) * r2) * C(y)

            return new Point(x, y);
        }

        private void DrawPoint(Point point, Canvas canvas)
        {
            var ellipse = new Ellipse() { Width = 5, Height = 5, Stroke = new SolidColorBrush(Colors.Black), Fill = new SolidColorBrush(Colors.Black) };
            canvas.Children.Add(ellipse);
            Canvas.SetLeft(ellipse, point.X);
            Canvas.SetTop(ellipse, point.Y);
        }

        private void DrawLine(Point a, Point b)
        {
            Line ab = new()
            {
                X1 = a.X,
                X2 = b.X,
                Y1 = a.Y,
                Y2 = b.Y,
                Stroke = Brushes.Blue,
                StrokeThickness = 2,
                StrokeLineJoin = PenLineJoin.Bevel
            };
            canvas.Children.Add(ab);
            Canvas.SetTop(ab, 0);
            Canvas.SetLeft(ab, 0);
        }

        private Point DrawRandomPoint(bool drawLine)
        {
            Point initialRandomPoint = GetRandomPoint(A, B, C);
            DrawPoint(initialRandomPoint, canvas);
            if (drawLine)
                DrawLine(A, initialRandomPoint);
            return initialRandomPoint;
        }

        private Point MidPoint(Point a, Point b) => new((a.X + b.X) / 2, (a.Y + b.Y) / 2);

        private void DrawPoints(Point initialRandomPoint, bool drawLine)
        {
            for (int n = 0; n < i; n++)
            {
                initialRandomPoint = MidPoint(A, initialRandomPoint);
                DrawPoint(initialRandomPoint, canvas);
                if (drawLine)
                    DrawLine(B, initialRandomPoint);
                initialRandomPoint = MidPoint(B, initialRandomPoint);
                DrawPoint(initialRandomPoint, canvas);
                if (drawLine)
                    DrawLine(C, initialRandomPoint);
                initialRandomPoint = MidPoint(C, initialRandomPoint);
                DrawPoint(initialRandomPoint, canvas);
                if (drawLine)
                    DrawLine(A, initialRandomPoint);
            }
        }
    }
}