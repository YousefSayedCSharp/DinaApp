namespace DinaApp.Models;

public class AppModel
{
    public int Id { get; set; }
    //app id
    public string AppId { get; set; }
    //app name
    public string AppName { get; set; }
    //description
    public string AppDescription { get; set; }
    //url
    public string AppUrl { get; set; }
    //date time activation
    public string AppFinish { get; set; }
    //Version
    public string AppVersion { get; set; }
    //app stop or not stopd true = un stop
    public bool IsActive { get; set; } = true;
}
