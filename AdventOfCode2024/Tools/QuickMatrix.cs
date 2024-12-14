using AdventOfCode2024.Extensions;
using System.Diagnostics;
using System.Drawing;

namespace AdventOfCode2024.Tools;

public enum TouchingMode
{
    Horizontal,
    Vertical,
    Diagonal,
    All
}

public class QuickMatrix
{
    private bool[,] _touchingSearch;

    private Dictionary<TouchingMode, List<Point>> _touchingMode = new()
    {
        { TouchingMode.Horizontal, new() { new Point(-1, 0), new Point(1, 0) } },
        { TouchingMode.Vertical, new() { new Point(0, -1), new Point(0, 1) } },
        { TouchingMode.Diagonal, new() { new Point(-1, -1), new Point(1, 1), new Point(-1, 1), new Point(1, -1) } },
        { TouchingMode.All, new() { new Point(-1, -1), new Point(0, -1), new Point(1, -1), new Point(-1, 0), new Point(1, 0), new Point(-1, 1), new Point(0, 1), new Point(1, 1) } }
    };

    private CellInfo[,] _data;

    public class CellInfo(int x, int y, string value)
    {
        public Point Position { get; private set; } = new Point(x, y);
        public string Value { get; set; } = value;
        public object? Tag { get; set; } = null;
        public bool IsValid => Position.X >= 0;

        public override string ToString()
        {
            return Value == null ? "NULL" : Value.ToString();
        }
    }

    public int ColCount { get; private set; }

    public int RowCount { get; private set; }

    public List<List<CellInfo>> Rows { get; private set; } = [];

    public List<List<int>> RowsInt
    {
        get { return Rows.ConvertAll(r => r.ConvertAll(v => int.Parse(v.Value))); }
    }

    public List<List<CellInfo>> Cols { get; private set; } = [];

    public List<List<int>> ColInt
    {
        get { return Cols.ConvertAll(c => c.ConvertAll(v => int.Parse(v.Value))); }
    }

    public List<CellInfo> Cells { get; private set; } = [];

    public QuickMatrix()
    {
        _data = new CellInfo[0, 0];
        ColCount = 0;
        RowCount = 0;
    }

    public QuickMatrix(int col, int row, string defaultValue = "")
    {
        ColCount = col;
        RowCount = row;
        _data = new CellInfo[col, row];
        for (int y = 0; y < RowCount; y++)
        {
            for (int x = 0; x < ColCount; x++)
            {
                _data[x, y] = new CellInfo(x, y, defaultValue);
            }
        }

        // Compute other properties
        ComputeOtherProperties();
    }

    public QuickMatrix(int col, int row, List<Point> filledCells, string filledCellsValue, string emptyCellsValue = "")
    {
        ColCount = col;
        RowCount = row;
        _data = new CellInfo[col, row];
        for (int y = 0; y < RowCount; y++)
        {
            for (int x = 0; x < ColCount; x++)
            {
                _data[x, y] = new CellInfo(x, y, emptyCellsValue);
            }
        }
        foreach (Point p in filledCells)
        {
            _data[p.X, p.Y].Value = filledCellsValue;
        }

        // Compute other properties
        ComputeOtherProperties();
    }

    public QuickMatrix(List<string> rawData, string separator = "", bool removeEmpty = false)
    {
        // Extract data
        _data = new CellInfo[0, 0];
        for (int y = 0; y < rawData.Count; y++)
        {
            if (y == 0)
            {
                ColCount = separator == string.Empty ? rawData.Max(v => v.Length) : rawData.Max(v => v.Split(separator).Length);
                RowCount = rawData.Count;
                _data = new CellInfo[ColCount, RowCount];
            }
            string line = rawData[y];
            if (string.IsNullOrEmpty(separator))
            {
                // Each char in a new box
                for (int x = 0; x < line.Length; x++)
                {
                    _data[x, y] = new(x, y, line[x].ToString());
                }
            }
            else
            {
                // Each item in a cell
                string[] values = line.Split(separator, removeEmpty ? StringSplitOptions.RemoveEmptyEntries : StringSplitOptions.None);
                for (int x = 0; x < values.Length; x++)
                {
                    _data[x, y] = new(x, y, values[x]);
                }
            }
        }

        // Compute other properties
        ComputeOtherProperties();
    }

    public void ClearAllTags()
    {
        foreach (CellInfo cell in Cells)
        {
            cell.Tag = null;
        }
    }

    public void RotateCounterClockwise()
    {
        int width = _data.GetLength(0);
        int height = _data.GetLength(1);
        CellInfo[,] rotated = new CellInfo[height, width];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                rotated[y, width - 1 - x] = _data[x, y];
            }
        }
        _data = rotated;
        ColCount = height;
        RowCount = width;

        // Compute other properties
        ComputeOtherProperties();
    }

    public void RotateClockwise()
    {
        int width = _data.GetLength(0);
        int height = _data.GetLength(1);
        CellInfo[,] rotated = new CellInfo[height, width];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                rotated[height - 1 - y, x] = _data[x, y];
            }
        }
        _data = rotated;
        ColCount = height;
        RowCount = width;

        // Compute other properties
        ComputeOtherProperties();
    }

    public void Rotate180Degree()
    {
        int width = _data.GetLength(0);
        int height = _data.GetLength(1);
        CellInfo[,] rotated = new CellInfo[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                rotated[width - 1 - x, height - 1 - y] = _data[x, y];
            }
        }
        _data = rotated;

        // Compute other properties
        ComputeOtherProperties();
    }

    public void FlipHorizontal()
    {
        int width = _data.GetLength(0);
        int height = _data.GetLength(1);
        CellInfo[,] rotated = new CellInfo[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                rotated[width - 1 - x, y] = _data[x, y];
            }
        }
        _data = rotated;

        // Compute other properties
        ComputeOtherProperties();
    }

    public void FlipVertical()
    {
        int width = _data.GetLength(0);
        int height = _data.GetLength(1);
        CellInfo[,] rotated = new CellInfo[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                rotated[x, height - 1 - y] = _data[x, y];
            }
        }
        _data = rotated;

        // Compute other properties
        ComputeOtherProperties();
    }

    public void Transpose()
    {
        int width = _data.GetLength(0);
        int height = _data.GetLength(1);
        CellInfo[,] rotated = new CellInfo[height, width];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                rotated[y, x] = _data[x, y];
            }
        }
        _data = rotated;
        ColCount = height;
        RowCount = width;

        // Compute other properties
        ComputeOtherProperties();
    }

    public CellInfo Cell(Point p) => Cell(p.X, p.Y);

    public CellInfo Cell(int x, int y)
    {
        if (x < 0 || x >= ColCount || y < 0 || y >= RowCount)
        {
            return new CellInfo(-1, -1, "");
        }
        return _data[x, y];
    }

    public string CellStr(Point p, string valueIfNull = "") => CellStr(p.X, p.Y, valueIfNull);

    public string CellStr(int x, int y, string valueIfNull = "")
    {
        if (x < 0 || x >= ColCount || y < 0 || y >= RowCount)
        {
            return valueIfNull;
        }
        return _data[x, y].Value;
    }

    private void ComputeOtherProperties()
    {
        // Create rows
        Rows = [];
        for (int y = 0; y < RowCount; y++)
        {
            List<CellInfo> row = [];
            for (int x = 0; x < ColCount; x++)
            {
                row.Add(_data[x, y]);
            }
            Rows.Add(row);
        }

        // Create columns
        Cols = [];
        for (int x = 0; x < ColCount; x++)
        {
            List<CellInfo> col = [];
            for (int y = 0; y < RowCount; y++)
            {
                col.Add(_data[x, y]);
            }
            Cols.Add(col);
        }

        // Create cells
        Cells = [];
        for (int y = 0; y < RowCount; y++)
        {
            for (int x = 0; x < ColCount; x++)
            {
                Cells.Add(_data[x, y]);
            }
        }
    }

    public void DebugPrint()
    {
        foreach (List<CellInfo> row in Rows)
        {
            Debug.WriteLine(row.Aggregate("", (acc, cell) => acc + cell.Value));
        }
    }

    public List<CellInfo> GetTouchingCellsWithValue(Point position, TouchingMode touchingMode)
    {
        _touchingSearch = new bool[ColCount, RowCount];
        return SearchTouchingCellsWithValue(position, touchingMode);
    }

    private List<CellInfo> SearchTouchingCellsWithValue(Point position, TouchingMode touchingMode)
    {
        // Already visited
        if (_touchingSearch[position.X, position.Y])
        {
            return [];
        }
        _touchingSearch[position.X, position.Y] = true;
        List<CellInfo> result = [Cell(position)];
        string targetValue = result[0].Value;
        foreach (Point move in _touchingMode[touchingMode])
        {
            Point nextPosition = position.Add(move);
            if (Cell(nextPosition).IsValid && Cell(nextPosition).Value == targetValue)
            {
                result.AddRange(SearchTouchingCellsWithValue(nextPosition, touchingMode));
            }
        }
        return result;
    }
}