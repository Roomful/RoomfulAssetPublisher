namespace SA.Productivity.GoogleSheets {


    [System.Serializable]
    public class GD_CellRange  {

    	public GD_RanageDirection Direction;
    	public GD_Cell StartCell;

    	public bool UseLinebreak = false;
    	public int LineLength = 1;

        public GD_CellRange() :this("A1") {

    	}

    	public GD_CellRange(string StartCellKey) :this(StartCellKey, GD_RanageDirection.Row) {

    	}

        public GD_CellRange(string StartCellKey, GD_RanageDirection rangeDirection) {
    		Direction = rangeDirection;
    		StartCell = new GD_Cell (StartCellKey);
    	}

    }

}