namespace Generics.Tables;

public class Table<TRow, TColumn, TValue>
{
    private readonly Dictionary<TRow, Dictionary<TColumn, TValue>> _data = new();
    private readonly HashSet<TColumn> _columns = new();

    public OpenIndexer Open => new(this);
    public ExistedIndexer Existed => new(this);
    public IEnumerable<TRow> Rows => _data.Keys;
    public IEnumerable<TColumn> Columns => _columns;

    public void AddRow(TRow row)
    {
        if (!_data.ContainsKey(row))
        {
            _data[row] = new Dictionary<TColumn, TValue>();
        }
    }

    public void AddColumn(TColumn column)
    {
        _columns.Add(column);
    }

    public class OpenIndexer
    {
        private readonly Table<TRow, TColumn, TValue> _table;

        public OpenIndexer(Table<TRow, TColumn, TValue> table)
        {
            _table = table;
        }

        public TValue this[TRow rowIndex, TColumn columnIndex]
        {
            get
            {
                if (_table._data.ContainsKey(rowIndex) && _table._data[rowIndex].ContainsKey(columnIndex))
                    return _table._data[rowIndex][columnIndex];
                return default;
            }
            set
            {
                if (!_table._data.ContainsKey(rowIndex))
                    _table.AddRow(rowIndex);

                if (!_table._columns.Contains(columnIndex))
                    _table.AddColumn(columnIndex);

                _table._data[rowIndex][columnIndex] = value;
            }
        }
    }

    public class ExistedIndexer
    {
        private readonly Table<TRow, TColumn, TValue> _table;

        public ExistedIndexer(Table<TRow, TColumn, TValue> table)
        {
            _table = table;
        }

        public TValue this[TRow rowIndex, TColumn columnIndex]
        {
            get
            {
                if (_table._data.ContainsKey(rowIndex) && _table._data[rowIndex].ContainsKey(columnIndex))
                    return _table._data[rowIndex][columnIndex];

                else if (_table._data.ContainsKey(rowIndex) && _table._columns.Contains(columnIndex))
                    return default;

                throw new ArgumentException("Строка или столбец не существуют.");
            }
            set
            {
                if (!_table._data.ContainsKey(rowIndex))
                    throw new ArgumentException("Строка не существует.");

                if (!_table._columns.Contains(columnIndex))
                    throw new ArgumentException("Столбец не существует.");

                _table._data[rowIndex][columnIndex] = value;
            }
        }
    }
}