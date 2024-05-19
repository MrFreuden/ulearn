using System;
using Avalonia.Controls;
using Avalonia.Media;
using RefactorMe.Common;

namespace RefactorMe
{
    class Painter
    {
        public static float X, Y;
        static IGraphics graphics;

        public static void InitializeGraphics(IGraphics graphic)
        {
            graphics = graphic;
            graphics.Clear(Colors.Black);
        }

        public static void SetPosition(float x0, float y0)
        {
            X = x0; 
            Y = y0;
        }

        public static void DrawLine(Pen pen, double length, double angleInDegrees)
        {
            //Делает шаг длиной length в направлении angle и рисует пройденную траекторию
            var angle = angleInDegrees * (Math.PI / 180.0);
            var x1 = (float)(X + length * Math.Cos(angle));
            var y1 = (float)(Y + length * Math.Sin(angle));
            graphics.DrawLine(pen, X, Y, x1, y1);
            X = x1;
            Y = y1;
        }

        public static void ChangePosition(double length, double angleInDegrees)
        {
            var angle = angleInDegrees * (Math.PI / 180.0);
            X = (float)(X + length * Math.Cos(angle)); 
            Y = (float)(Y + length * Math.Sin(angle));
        }
    }
    
    public class ImpossibleSquare
    {
        public const float ScaleFactor = 0.04f;
        public const float LineScaleFactor = 0.375f;

        public static void Draw(int width, int height, double angle, IGraphics graphics)
        {
            Painter.InitializeGraphics(graphics);
            var size = Math.Min(width, height);
            SetStartPosition(width, height, size);

            var yellowPen = new Pen(Brushes.Yellow);

            DrawSide(yellowPen, size, 0);
            Painter.ChangePosition(size * ScaleFactor, -180);
            Painter.ChangePosition(size * ScaleFactor * Math.Sqrt(2), 135);

            DrawSide(yellowPen, size, -90);
            Painter.ChangePosition(size * ScaleFactor, -270);
            Painter.ChangePosition(size * ScaleFactor * Math.Sqrt(2), 45);

            DrawSide(yellowPen, size, 180);
            Painter.ChangePosition(size * ScaleFactor, 0);
            Painter.ChangePosition(size * ScaleFactor * Math.Sqrt(2), 315);

            DrawSide(yellowPen, size, 90);
        }

        private static void DrawSide(Pen pen, int size, double angleTurnInDegrees)
        {
            var horizontalLine = size * LineScaleFactor;
            var diagonalLine = size * ScaleFactor * Math.Sqrt(2);
            var vertiсalLine = size * LineScaleFactor - size * ScaleFactor;

            Painter.DrawLine(pen, horizontalLine, 0 + angleTurnInDegrees);
            Painter.DrawLine(pen, diagonalLine, 45 + angleTurnInDegrees);
            Painter.DrawLine(pen, horizontalLine, 180 + angleTurnInDegrees);
            Painter.DrawLine(pen, vertiсalLine, 90 + angleTurnInDegrees);
        }

        private static void SetStartPosition(int width, int height, int size)
        {
            var diagonalLength = Math.Sqrt(2) * (size * LineScaleFactor + size * ScaleFactor) / 2;
            var x0 = (float)(diagonalLength * Math.Cos(Math.PI / 4 + Math.PI)) + width / 2f;
            var y0 = (float)(diagonalLength * Math.Sin(Math.PI / 4 + Math.PI)) + height / 2f;
            Painter.SetPosition(x0, y0);
        }
    }
}