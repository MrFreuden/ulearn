using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Clones;

public class CloneVersionSystem : ICloneVersionSystem
{
    private readonly List<Clone> _clones;
    public CloneVersionSystem()
    {
        _clones = new List<Clone> { new Clone() };
    }

    public string Execute(string query)
    {
        var queryArguments = query.Split(' ');
        var cloneNumber = int.Parse(queryArguments[1]) - 1;

        switch (queryArguments[0])
        {
            case "clone":
                _clones.Add(new Clone(_clones[cloneNumber]));
                return null;

            case "learn":
                var programNumber = int.Parse(queryArguments[2]);
                _clones[cloneNumber].Learn(programNumber);
                return null;

            case "rollback":
                _clones[cloneNumber].RollBack();
                return null;

            case "relearn":
                _clones[cloneNumber].Relearn();
                return null;

            case "check":
                return _clones[cloneNumber].Check();
        }
        return null;
    }
}

public class Clone
{
    private readonly ListStack<int> _learnedPrograms;
    private readonly ListStack<int> _rollBackPrograms;

    public Clone()
    {
        _learnedPrograms = new();
        _rollBackPrograms = new();
    }

    public Clone(Clone originalClone)
    {
        _learnedPrograms = new ListStack<int>(originalClone._learnedPrograms);
        _rollBackPrograms = new ListStack<int>(originalClone._rollBackPrograms);
    }

    public void Learn(int programId)
    {
        _rollBackPrograms.Clear();
        _learnedPrograms.Push(programId);
    }

    public void RollBack()
    {
        if (!_learnedPrograms.IsEmpty)
        {
            _rollBackPrograms.Push(_learnedPrograms.Pop());
        }
    }

    public void Relearn()
    {
        if (!_rollBackPrograms.IsEmpty)
        {
            _learnedPrograms.Push(_rollBackPrograms.Pop());
        }
    }

    public string Check()
    {
        return _learnedPrograms.IsEmpty ? "basic" : _learnedPrograms.Peek().ToString();
    }
}

public class ListStack<T>
{
    private StackItem<T> _top;

    public bool IsEmpty => _top == null;

    public ListStack()
    {

    }

    public ListStack(ListStack<T> listStack)
    {
        _top = listStack._top;
    }

    public void Push(T value)
    {
        _top = new StackItem<T> { Value = value, Next = _top };
    }

    public T Pop()
    {
        if (IsEmpty) throw new InvalidOperationException("Stack is empty.");
        var result = _top.Value;
        _top = _top.Next;
        return result;
    }

    public T Peek()
    {
        if (IsEmpty) throw new InvalidOperationException("Stack is empty.");
        return _top.Value;
    }

    public void Clear()
    {
        _top = null;
    }
}

public class StackItem<T>
{
    public T Value { get; set; }
    public StackItem<T> Next { get; set; }
}