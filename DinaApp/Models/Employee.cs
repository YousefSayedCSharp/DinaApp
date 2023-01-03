namespace DinaApp.Models;

public class Employee
{
    public int Id { get; set; } 
    public string Name { get; set; }
    public string Password { get; set; }
    public bool IsAdmin { get; set; }
    public bool IsActive { get; set; } = true;
    public string AppId { get; set; }
}
