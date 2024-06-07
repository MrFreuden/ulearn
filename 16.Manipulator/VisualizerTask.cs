using System;
using System.Globalization;
using Avalonia;
using Avalonia.Input;
using Avalonia.Media;

namespace Manipulation;

public static class VisualizerTask
{
	public static double X = 220;
	public static double Y = -100;
	public static double Alpha = 0.05;
	public static double Wrist = 2 * Math.PI / 3;
	public static double Elbow = 3 * Math.PI / 4;
	public static double Shoulder = Math.PI / 2;

	public static Brush UnreachableAreaBrush = new SolidColorBrush(Color.FromArgb(255, 255, 230, 230));
	public static Brush ReachableAreaBrush = new SolidColorBrush(Color.FromArgb(255, 230, 255, 230));
	public static Pen ManipulatorPen = new Pen(Brushes.Black, 3);
	public static Brush JointBrush = new SolidColorBrush(Colors.Gray);

	public static void KeyDown(Visual visual, KeyEventArgs key)
	{
		var step = Math.PI / 360;
        switch (key.Key)
		{
			case Key.Q:
				Shoulder += step;
				break;
            case Key.A:
                Shoulder -= step;
                break;
            case Key.W:
                Elbow += step;
                break;
            case Key.S:
                Elbow -= step;
                break;
        }
		Wrist = - Alpha - Shoulder - Elbow;
		visual.InvalidateVisual();
	}

	public static void MouseMove(Visual visual, PointerEventArgs e)
	{
		var pointerPos = e.GetPosition(visual);
		var shoulderPos = GetShoulderPos(visual);
		var mathPoint = ConvertWindowToMath(pointerPos, shoulderPos);
		X = mathPoint.X; 
		Y = mathPoint.Y;
		
		UpdateManipulator();
		visual.InvalidateVisual();
	}

	public static void MouseWheel(Visual visual, PointerWheelEventArgs e)
	{
		// TODO: Измените Alpha, используя e.Delta.Y — размер прокрутки колеса мыши

		UpdateManipulator();
		visual.InvalidateVisual();
	}

	public static void UpdateManipulator()
	{
		// Вызовите ManipulatorTask.MoveManipulatorTo и обновите значения полей Shoulder, Elbow и Wrist, 
		// если они не NaN. Это понадобится для последней задачи.
	}

	public static void DrawManipulator(DrawingContext context, Point shoulderPos)
	{
		var joints = AnglesToCoordinatesTask.GetJointPositions(Shoulder, Elbow, Wrist);

		DrawReachableZone(context, ReachableAreaBrush, UnreachableAreaBrush, shoulderPos, joints);

		var formattedText = new FormattedText(
			$"X={X:0}, Y={Y:0}, Alpha={Alpha:0.00}",
			CultureInfo.InvariantCulture,
			FlowDirection.LeftToRight,
			Typeface.Default,
			18,
			Brushes.DarkRed
		)
		{
			TextAlignment = TextAlignment.Center
		};
		context.DrawText(formattedText, new Point(10, 10));

		// Нарисуйте сегменты манипулятора методом ccontext.DrawLine(ManipulatorPen, ...)
		// Нарисуйте суставы манипулятора окружностями методом context.DrawEllipse(JointBrush, null, ...)
		// Не забудьте сконвертировать координаты из логических в оконные
	}

	private static void DrawReachableZone(
		DrawingContext context,
		Brush reachableBrush,
		Brush unreachableBrush,
		Point shoulderPos,
		Point[] joints)
	{
		var rmin = Math.Abs(Manipulator.UpperArm - Manipulator.Forearm);
		var rmax = Manipulator.UpperArm + Manipulator.Forearm;
		var mathCenter = new Point(joints[2].X - joints[1].X, joints[2].Y - joints[1].Y);
		var windowCenter = ConvertMathToWindow(mathCenter, shoulderPos);
		context.DrawEllipse(reachableBrush,
			null,
			new Point(windowCenter.X, windowCenter.Y),
			rmax, rmax);
		context.DrawEllipse(unreachableBrush,
			null,
			new Point(windowCenter.X, windowCenter.Y),
			rmin, rmin);
	}

	public static Point GetShoulderPos(Visual visual)
	{
		return new Point(visual.Bounds.Width / 2, visual.Bounds.Height / 2);
	}

	public static Point ConvertMathToWindow(Point mathPoint, Point shoulderPos)
	{
		return new Point(mathPoint.X + shoulderPos.X, shoulderPos.Y - mathPoint.Y);
	}

	public static Point ConvertWindowToMath(Point windowPoint, Point shoulderPos)
	{
		return new Point(windowPoint.X - shoulderPos.X, shoulderPos.Y - windowPoint.Y);
	}
}