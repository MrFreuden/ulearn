using System;
using System.Collections.Generic;
using System.Linq;

namespace Clones;

public class CloneVersionSystem : ICloneVersionSystem
{
    private readonly CommandFactory _commandFactory;
    private Dictionary<int, Clone> _clonesById;
    public CloneVersionSystem()
    {
        Clone.ResetId();
        _clonesById = new Dictionary<int, Clone>();
        _clonesById.Add(1, new Clone());
        _commandFactory = new CommandFactory(_clonesById);
    }

    public string Execute(string query)
    {
        var command = _commandFactory.CreateCommand(query.Split(' '));
        var result = command.Execute();
        return result;
    }
}

public class Clone
{
    private static int _iD = 0;
    private Stack<int> _progsId;
    private Stack<ICommand> _commands;
    private Stack<ICommand> _undoCommads;
    public int Id { get; }
    public List<int> ProgsId { get { return _progsId.ToList(); } }

    public Clone()
    {
        _iD++;
        Id = _iD;
        _progsId = new Stack<int>();
        _commands = new Stack<ICommand>();
        _undoCommads = new Stack<ICommand>();
    
}

    public Clone(Stack<int> progsId, Stack<ICommand> commands, Stack<ICommand> undoCommands)
    {
        _iD++;
        Id = _iD;
        _progsId = new Stack<int>(progsId);
        _commands = new Stack<ICommand>(commands);
        _undoCommads = new Stack<ICommand>(undoCommands);
    }

    public void AddProgId(int progId)
    {
        _progsId.Push(progId);
    }

    public void RemoveProgId(int progId)
    {
        if (_progsId.Peek() == progId)
        {
            _progsId.Pop();
        }
    }

    public void AddCommand(ICommand command)
    {
        _commands.Push(command);
        _undoCommads.Clear();
    }

    public ICommand PopLastCommand()
    {
        return _commands.Pop();
    }

    public void AddUndoCommand(ICommand command)
    {
        _undoCommads.Push(command);
    }

    public ICommand PopLastUndoCommand()
    {
        return _undoCommads.Pop();
    }

    public Clone CopyClone()
    {
        var newCommands = new Stack<ICommand>(_commands.Select(c => c.CopyWithNewId(_iD + 1)));
        var newUndoCommands = new Stack<ICommand>(_undoCommads.Select(c => c.CopyWithNewId(_iD + 1)));
        return new Clone(_progsId, newCommands, newUndoCommands);
    }

    public static void ResetId()
    {
        _iD = 0;
    }
}

public class LearnCommand : ICommand
{
    private Dictionary<int, Clone> _clones;
    private int _idClone;
    private int _idProg;
    public LearnCommand(Dictionary<int, Clone> clones, int idClone, int idProg)
    {
        _clones = clones;
        _idClone = idClone;
        _idProg = idProg;
    }

    public string Execute()
    {
        if (!_clones.TryGetValue(_idClone, out Clone clone))
        {
            throw new ArgumentException($"Clone with id {_idClone} does not exist");
        }

        clone.AddProgId(_idProg);

        clone.AddCommand(this);

        return null;
    }

    public void Undo()
    {
        if (!_clones.TryGetValue(_idClone, out Clone clone))
        {
            throw new ArgumentException($"Clone with id {_idClone} does not exist");
        }

        clone.RemoveProgId(_idProg);
    }

    public void Redo()
    {
        Execute();
    }

    public ICommand CopyWithNewId(int newId)
    {
        return new LearnCommand(_clones, newId, _idProg);
    }
}

public class RollbackCommand : ICommand
{
    private Dictionary<int, Clone> _clones;
    private int _idClone;
    private ICommand _lastCommand;

    public RollbackCommand(Dictionary<int, Clone> clones, int idClone)
    {
        _clones = clones;
        _idClone = idClone;
    }

    public string Execute()
    {
        if (!_clones.TryGetValue(_idClone, out Clone clone))
        {
            throw new ArgumentException($"Clone with id {_idClone} does not exist");
        }

        _lastCommand = clone.PopLastCommand();
        _lastCommand.Undo();
        clone.AddUndoCommand(_lastCommand);
        return null;
    }

    public void Undo()
    {
        _lastCommand.Redo();
    }

    public void Redo()
    {
        Execute();
    }

    public ICommand CopyWithNewId(int newId)
    {
        return new RollbackCommand(_clones, newId);
    }
}

public class RelearnCommand : ICommand
{
    private Dictionary<int, Clone> _clones;
    private int _idClone;

    public RelearnCommand(Dictionary<int, Clone> clones, int idClone)
    {
        _clones = clones;
        _idClone = idClone;
    }

    public string Execute()
    {
        if (!_clones.TryGetValue(_idClone, out Clone clone))
        {
            throw new ArgumentException($"Clone with id {_idClone} does not exist");
        }
        var command = clone.PopLastUndoCommand();
        command.Redo();
        clone.AddCommand(command);
        return null;
    }

    public void Undo()
    {
        Execute();
    }

    public void Redo()
    {
        Execute();
    }

    public ICommand CopyWithNewId(int newId)
    {
        return new RelearnCommand(_clones, newId);
    }
}

public class CloneCommand : ICommand
{
    private Dictionary<int, Clone> _clones;
    private int _idClone;
    private Clone _copyClone;
    private int _idCopyClone;

    public CloneCommand(Dictionary<int, Clone> clones, int idClone)
    {
        _clones = clones;
        _idClone = idClone;
    }

    public string Execute()
    {
        if (!_clones.TryGetValue(_idClone, out Clone clone))
        {
            throw new ArgumentException($"Clone with id {_idClone} does not exist");
        }

        _copyClone = clone.CopyClone();
        _idCopyClone = _copyClone.Id;
        _clones.Add(_idCopyClone, _copyClone);
        return null;
    }

    public void Undo()
    {
        _clones.Remove(_idCopyClone);
    }

    public void Redo()
    {
        _clones[_idCopyClone] = _copyClone;
    }

    public ICommand CopyWithNewId(int newId)
    {
        return new CloneCommand(_clones, newId);
    }
}

public class CheckCommand : ICommand
{
    private Dictionary<int, Clone> _clones;
    private int _idClone;

    public CheckCommand(Dictionary<int, Clone> clones, int idClone)
    {
        _clones = clones;
        _idClone = idClone;
    }

    public string Execute()
    {
        if (!_clones.TryGetValue(_idClone, out Clone clone))
        {
            throw new ArgumentException($"Clone with id {_idClone} does not exist");
        }
        var progs = clone.ProgsId;
        if (progs.Count < 1)
        {
            return "basic";
        }
        return progs.First().ToString();
    }

    public void Undo()
    {
        
    }

    public void Redo()
    {
        Execute();
    }

    public ICommand CopyWithNewId(int newId)
    {
        return new CheckCommand(_clones, newId);
    }
}

public interface ICommand
{
    string Execute();
    void Undo();
    void Redo();
    ICommand CopyWithNewId(int newId);
}

public interface ICommandFactory
{
    ICommand CreateCommand(string[] args);
}

public class CommandFactory : ICommandFactory
{
    private readonly Dictionary<string, ICommandFactory> _commandFactories;

    public CommandFactory(Dictionary<int, Clone> clones)
    {
        _commandFactories = new Dictionary<string, ICommandFactory>(StringComparer.OrdinalIgnoreCase)
        {
            { "learn", new LearnCommandFactory(clones) },
            { "rollback", new RollbackCommandFactory(clones) },
            { "relearn", new RelearnCommandFactory(clones) },
            { "clone", new CloneCommandFactory(clones) },
            { "check", new CheckCommandFactory(clones) }
        };
    }

    public ICommand CreateCommand(string[] args)
    {
        if (args == null || args.Length == 0)
        {
            throw new ArgumentException("No command provided.");
        }

        string commandName = args[0].ToLower();

        if (!_commandFactories.TryGetValue(commandName, out var factory))
        {
            throw new ArgumentException($"Unknown command: {commandName}");
        }

        return factory.CreateCommand(args);
    }
}

public class LearnCommandFactory : ICommandFactory
{
    private readonly Dictionary<int, Clone> _clones;
    public LearnCommandFactory(Dictionary<int, Clone> clones)
    {
        _clones = clones;
    }

    public ICommand CreateCommand(string[] args)
    {
        if (args.Length != 3)
            throw new ArgumentException("Learn command requires exactly 2 arguments.");
        if (!int.TryParse(args[1], out int cloneId))
            throw new ArgumentException("Invalid ID for Learn command.");
        if (!int.TryParse(args[2], out int progId))
            throw new ArgumentException("Invalid progID for Learn command.");
        return new LearnCommand(_clones, cloneId, progId);
    }
}

public class RollbackCommandFactory : ICommandFactory
{
    private readonly Dictionary<int, Clone> _clones;
    public RollbackCommandFactory(Dictionary<int, Clone> clones)
    {
        _clones = clones;
    }

    public ICommand CreateCommand(string[] args)
    {
        if (args.Length != 2)
            throw new ArgumentException("Rollback command requires exactly 1 argument.");
        if (!int.TryParse(args[1], out int cloneId))
            throw new ArgumentException("Invalid ID for Rollback command.");
        return new RollbackCommand(_clones, cloneId);
    }
}

public class RelearnCommandFactory : ICommandFactory
{
    private readonly Dictionary<int, Clone> _clones;
    public RelearnCommandFactory(Dictionary<int, Clone> clones)
    {
        _clones = clones;
    }

    public ICommand CreateCommand(string[] args)
    {
        if (args.Length != 2)
            throw new ArgumentException("Relearn command requires exactly 1 argument.");
        if (!int.TryParse(args[1], out int cloneId))
            throw new ArgumentException("Invalid ID for Relearn command.");
        return new RelearnCommand(_clones, cloneId);
    }
}

public class CloneCommandFactory : ICommandFactory
{
    private readonly Dictionary<int, Clone> _clones;
    public CloneCommandFactory(Dictionary<int, Clone> clones)
    {
        _clones = clones;
    }

    public ICommand CreateCommand(string[] args)
    {
        if (args.Length != 2)
            throw new ArgumentException("Clone command requires exactly 1 argument.");
        if (!int.TryParse(args[1], out int cloneId))
            throw new ArgumentException("Invalid ID for Clone command.");
        return new CloneCommand(_clones, cloneId);
    }
}

public class CheckCommandFactory : ICommandFactory
{
    private readonly Dictionary<int, Clone> _clones;
    public CheckCommandFactory(Dictionary<int, Clone> clones)
    {
        _clones = clones;
    }

    public ICommand CreateCommand(string[] args)
    {
        if (args.Length != 2)
            throw new ArgumentException("Check command requires exactly 1 argument.");
        if (!int.TryParse(args[1], out int cloneId))
            throw new ArgumentException("Invalid ID for Check command.");
        return new CheckCommand(_clones, cloneId);
    }
}
