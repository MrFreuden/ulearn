using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using FractalPainting.App.Fractals;
using FractalPainting.Infrastructure.Common;
using FractalPainting.Infrastructure.UiActions;
using FractalPainting.UI;
using Ninject;
using Ninject.Extensions.Factory;
using System;
using System.Linq;
using Ninject.Extensions.Conventions;

namespace FractalPainting.App;

public static class DIContainerTask
{
    public static MainWindow CreateMainWindow()
    {
        var kernel = ConfigureContainer();
        return kernel.Get<MainWindow>();
    }

    public static StandardKernel ConfigureContainer()
    {
        var container = new StandardKernel();

        container.Bind(c => c.FromThisAssembly().SelectAllClasses().EndingWith("Action").BindAllInterfaces());

        container.Bind<IImageController, AvaloniaImageController>().To<AvaloniaImageController>().InSingletonScope();
        container.Bind<Palette>().ToSelf().InSingletonScope();
        container.Bind<IDragonPainterFactory>().ToFactory();

        container.Bind<SettingsManager>()
            .ToMethod(context => new SettingsManager(new XmlObjectSerializer(), new FileBlobStorage()))
            .InSingletonScope();
        container.Bind<AppSettings>()
            .ToMethod(context => context.Kernel.Get<SettingsManager>().Load())
        .InSingletonScope();
        container.Bind<ImageSettings>().ToMethod(context => context.Kernel.Get<AppSettings>().ImageSettings)
            .InSingletonScope();

        container.Bind<MainWindow>().ToSelf().InSingletonScope();
        container.Bind<Window>().ToMethod(context => context.Kernel.Get<MainWindow>());
        container.Bind<Func<Window>>().ToMethod(context => () => context.Kernel.Get<MainWindow>());

        return container;
    }
}

public class DragonFractalAction : IUiAction
{
    public MenuCategory Category => MenuCategory.Fractals;
    public event EventHandler? CanExecuteChanged;
    private readonly Func<Window> _window;
    private readonly IDragonPainterFactory _painterFactory;
    private readonly Func<DragonSettings, DragonPainter> _funcPainterFactory;

    public DragonFractalAction(IDragonPainterFactory imageController, Func<Window> window)
    {
        _painterFactory = imageController;
        _window = window;
    }

    //public DragonFractalAction(Func<DragonSettings, DragonPainter> funcFactory, Func<Window> window)
    //   {
    //       _funcPainterFactory = funcFactory;
    //       _window = window;
    //   }

    public string Name => "Дракон";

    public bool CanExecute(object? parameter)
    {
        return true;
    }

    public async void Execute(object? parameter)
    {
        var s = _window();
        var dragonSettings = CreateRandomSettings();
        await new SettingsForm(dragonSettings).ShowDialog(_window());
        var painter = _painterFactory.Create(dragonSettings);
        //var painter = _funcPainterFactory(dragonSettings);
        painter.Paint();
    }

    private static DragonSettings CreateRandomSettings()
    {
        return new DragonSettingsGenerator(new Random()).Generate();
    }
}

public interface IDragonPainterFactory
{
    DragonPainter Create(DragonSettings dragonSettings);
}

public class KochFractalAction : IUiAction
{
    public MenuCategory Category => MenuCategory.Fractals;
    public event EventHandler? CanExecuteChanged;

    private readonly Lazy<KochPainter> _kochPainter;


    public KochFractalAction(Lazy<KochPainter> kochPainter)
    {
        _kochPainter = kochPainter;
    }

    public string Name => "Кривая Коха";

    public bool CanExecute(object? parameter)
    {
        return true;
    }

    public void Execute(object? parameter)
    {
        _kochPainter.Value.Paint();
    }
}

public class DragonPainter
{
    private readonly IImageController imageController;
    private readonly DragonSettings settings;
    private readonly Palette palette;

    public DragonPainter(IImageController imageController, DragonSettings settings, Palette palette)
    {
        this.imageController = imageController;
        this.settings = settings;
        this.palette = palette;
    }

    public void Paint()
    {
        using var ctx = imageController.CreateDrawingContext();
        var imageSize = imageController.GetImageSize();
        var size = Math.Min(imageSize.Width, imageSize.Height) / 2.1f;

        ctx.FillRectangle(new SolidColorBrush(palette.BackgroundColor),
            new Rect(0, 0, imageSize.Width, imageSize.Height));

        DrawSegment(ctx, imageSize, size);

        imageController.UpdateUi();
    }

    private void DrawSegment(DrawingContext ctx, Avalonia.Size imageSize, double size)
    {
        var r = new Random();
        var cosa = (float)Math.Cos(settings.Angle1);
        var sina = (float)Math.Sin(settings.Angle1);
        var cosb = (float)Math.Cos(settings.Angle2);
        var sinb = (float)Math.Sin(settings.Angle2);
        var shiftX = settings.ShiftX * size * 0.8f;
        var shiftY = settings.ShiftY * size * 0.8f;
        var scale = settings.Scale;
        var p = new Avalonia.Point(0, 0);
        foreach (var i in Enumerable.Range(0, settings.IterationsCount))
        {
            ctx.FillRectangle(new SolidColorBrush(palette.PrimaryColor),
                new Rect(imageSize.Width / 3f + p.X, imageSize.Height / 2f + p.Y, 1, 1));

            if (r.Next(0, 2) == 0)
                p = new Avalonia.Point(scale * (p.X * cosa - p.Y * sina), scale * (p.X * sina + p.Y * cosa));
            else
                p = new Avalonia.Point(scale * (p.X * cosb - p.Y * sinb) + (float)shiftX,
                    scale * (p.X * sinb + p.Y * cosb) + (float)shiftY);
        }
    }
}