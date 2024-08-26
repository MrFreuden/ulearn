using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LimitedSizeStack
{
    public class ListModelWithPatternCommand<TItem>
    {
        public List<TItem> Items { get; }
        public int UndoLimit;
        private LimitedSizeStack<ICommand> _undoCommands;

        public ListModelWithPatternCommand(int undoLimit) : this(new List<TItem>(), undoLimit)
        {
        }

        public ListModelWithPatternCommand(List<TItem> items, int undoLimit)
        {
            Items = items;
            UndoLimit = undoLimit;
            _undoCommands = new LimitedSizeStack<ICommand>(undoLimit);
        }

        public void AddItem(TItem item)
        {
            ExecuteCommand(new AddCommand<TItem>(Items, item));
        }

        public void RemoveItem(int index)
        {
            ExecuteCommand(new RemoveCommand<TItem>(Items, index));
        }

        public bool CanUndo()
        {
            return _undoCommands.Count > 0;
        }

        public void Undo()
        {
            if (CanUndo())
            {
                _undoCommands.Pop().Undo();
            }
        }

        private void ExecuteCommand(ICommand command)
        {
            command.Execute();
            _undoCommands.Push(command);
        }
    }

    public interface ICommand
    {
        void Execute();
        void Undo();
    }

    public class AddCommand<TItem> : ICommand
    {
        private readonly List<TItem> _items;
        private readonly TItem _item;

        public AddCommand(List<TItem> items, TItem item)
        {
            _items = items ?? throw new ArgumentNullException(nameof(items));
            _item = item;
        }

        public void Execute()
        {
            _items.Add(_item);
        }

        public void Undo()
        {
            _items.RemoveAt(_items.Count - 1);
        }
    }

    public class RemoveCommand<TItem> : ICommand
    {
        private readonly List<TItem> _items;
        private readonly int _itemIndex;
        private TItem _deletedItem;

        public RemoveCommand(List<TItem> items, int itemIndex)
        {
            _items = items ?? throw new ArgumentNullException(nameof(items));
            _itemIndex = itemIndex;
        }

        public void Execute()
        {
            _deletedItem = _items[_itemIndex];
            _items.RemoveAt(_itemIndex);
        }

        public void Undo()
        {
            _items.Insert(_itemIndex, _deletedItem);
        }
    }
}
