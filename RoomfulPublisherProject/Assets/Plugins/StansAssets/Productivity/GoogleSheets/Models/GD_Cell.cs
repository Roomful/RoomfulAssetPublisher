using UnityEngine;
using System;
using System.Collections;


namespace SA.Productivity.GoogleSheets {

[Serializable]
public class GD_Cell  {

        [SerializeField] int m_col = 1;
        [SerializeField] int m_row = 1;
        [SerializeField] string m_key = "A1";



    	public GD_Cell() {

    	}

        public GD_Cell(int colIndex, int rowIndex) {
    		m_col = colIndex;
    		m_row = rowIndex;

    		GenerateKey ();
    	}

    	public GD_Cell(string CellKey) {
    		key = CellKey;
    	}

    	
    	public int row {
    		get {
    			return m_row;
    		}
    		set {
    			m_row = value;
    			GenerateKey ();
    		}
    	}


    	public int col {
    		get {
    			return m_col;
    		}
    		set {
    			m_col = value;
    			GenerateKey ();
    		}
    	}


    	public string key {
    		get {
    			return m_key;
    		}
    		set {
    			m_key = value;
    			int startIndex = m_key.IndexOfAny("123456789".ToCharArray());
    			string columnStr;
    			int row = 0;
    			try{
    				columnStr = m_key.Substring(0, startIndex);
    				row = Int32.Parse(m_key.Substring(startIndex));
    			} catch (Exception) {
    				columnStr = string.Empty;
    				row = 0;
    			}
    			
    			columnStr = columnStr.ToUpperInvariant ();
    			int column = 0;
    			for (int i = 0; i < columnStr.Length; i++) {
    				column *= 26;
    				column += (columnStr[i] - 'A' + 1);
    			}

    			this.m_col = column != 0 ? column : this.m_col;
    			this.m_row = row != 0 ? row : this.m_row;
    		}
    	}


        public GD_Cell Copy() {
            return new GD_Cell(col, row);
        }

    	private void GenerateKey() {
    		char ch = (char)('A' + m_col - 1);
    		m_key = ch + m_row.ToString();
    	}

    }

}

