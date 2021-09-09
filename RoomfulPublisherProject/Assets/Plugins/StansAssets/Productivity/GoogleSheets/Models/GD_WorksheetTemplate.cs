
namespace SA.Productivity.GoogleSheets {

    [System.Serializable]
    public class GD_WorksheetTemplate {
        
    	public string ListName;
    	public int ListId;
    	
        public GD_WorksheetTemplate(string name, int id) {
    		ListName = name;
    		ListId = id;
    	}
    }

}