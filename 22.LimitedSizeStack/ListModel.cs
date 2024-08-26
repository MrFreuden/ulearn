using System;
using System.Collections.Generic;

namespace LimitedSizeStack;

public class ListModel<TItem>
{
	public List<TItem> Items { get; }
	public int UndoLimit;
    private LimitedSizeStack<Action> _undoActions;

    public ListModel(int undoLimit) : this(new List<TItem>(), undoLimit)
	{
	}

	public ListModel(List<TItem> items, int undoLimit)
	{
		Items = items;
		UndoLimit = undoLimit;
        _undoActions = new LimitedSizeStack<Action>(undoLimit);
    }

	public void AddItem(TItem item)
	{
		Items.Add(item);
        _undoActions.Push(() => Items.RemoveAt(Items.Count - 1));
    }

	public void RemoveItem(int index)
    {
        var removedItem = Items[index];
        Items.RemoveAt(index);
        _undoActions.Push(() => Items.Insert(index, removedItem));
    }

	public bool CanUndo()
	{
		return _undoActions.Count > 0;
    }

	public void Undo()
	{
		if (CanUndo())
		{
            _undoActions.Pop().Invoke();
        }
    }
}