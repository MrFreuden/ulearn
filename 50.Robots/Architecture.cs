namespace Generics.Robots;

public interface IRobotAI<out TCommand>
{
    TCommand GetCommand();
}

public abstract class RobotAI<TCommand> : IRobotAI<TCommand>
{
    public abstract TCommand GetCommand();
}

public class ShooterAI : RobotAI<IShooterMoveCommand>
{
    private int _counter = 1;

    public override IShooterMoveCommand GetCommand()
    {
        return ShooterCommand.ForCounter(_counter++);
    }
}

public class BuilderAI : RobotAI<IMoveCommand>
{
    private int _counter = 1;

    public override IMoveCommand GetCommand()
    {
        return BuilderCommand.ForCounter(_counter++);
    }
}

public interface IDevice<in TCommand>
{
    string ExecuteCommand(TCommand command);
}

public abstract class Device<TCommand> : IDevice<TCommand>
{
    public abstract string ExecuteCommand(TCommand command);
}

public class Mover : Device<IMoveCommand>
{
    public override string ExecuteCommand(IMoveCommand _command)
    {
        var command = _command ?? throw new ArgumentException();
        return $"MOV {command.Destination.X}, {command.Destination.Y}";
    }
}

public class ShooterMover : Device<IShooterMoveCommand>
{
    public override string ExecuteCommand(IShooterMoveCommand _command)
    {
        var command = _command ?? throw new ArgumentException();
        var hide = command.ShouldHide ? "YES" : "NO";
        return $"MOV {command.Destination.X}, {command.Destination.Y}, USE COVER {hide}";
    }
}

public class Robot<TCommand>
{
    private readonly IRobotAI<TCommand> _ai;
    private readonly IDevice<TCommand> _device;

    public Robot(IRobotAI<TCommand> ai, IDevice<TCommand> executor)
    {
        _ai = ai;
        _device = executor;
    }

    public IEnumerable<string> Start(int steps)
    {
        for (int i = 0; i < steps; i++)
        {
            var command = _ai.GetCommand();
            if (command == null)
                break;
            yield return _device.ExecuteCommand(command);
        }
    }
}

public static class Robot
{
    public static Robot<TCommand> Create<TCommand>(IRobotAI<TCommand> ai, IDevice<TCommand> executor)
        where TCommand : IMoveCommand
    {
        return new Robot<TCommand>(ai, executor);
    }
}