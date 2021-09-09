namespace SA.Productivity.GoogleSheets {

    [System.Serializable]
    public class GD_CellDictionaryRange
    {
    	public GD_CellRange CellRange;

        public int RowShift = 0;
        public int ColumnShift = 0;

        public GD_CellDictionaryRange () : this(new GD_CellRange(), 0, 0)	{

    	}

        public GD_CellDictionaryRange (GD_CellRange range, int colShiftValue, int rowShiftValue)	{
    		CellRange = range;
    		ColumnShift = colShiftValue;
    		RowShift = rowShiftValue;
    	}

        public GD_Cell GetValueCellForKey(GD_Cell key) {
            GD_Cell val = new GD_Cell(key.col + ColumnShift, key.row + RowShift);
            return val;
        }

        public GD_CellRange GetCellRangeWithOffset() {
            GD_CellRange newCellRange = new GD_CellRange ();
    		newCellRange.UseLinebreak = CellRange.UseLinebreak;
    		newCellRange.LineLength = CellRange.LineLength;
    		newCellRange.Direction = CellRange.Direction;
    		newCellRange.StartCell.col = CellRange.StartCell.col + ColumnShift;
    		newCellRange.StartCell.row = CellRange.StartCell.row + RowShift;

    		return newCellRange;
    	}
    }
}
