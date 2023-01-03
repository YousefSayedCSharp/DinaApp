namespace DinaApp.Models;

public class BookAction
{
//ده باستخدمه في إضافة المطبوع 
    public int Id { get; set; } 
    public int BookId { get; set; } 
    public int CategoryId { get; set; } 
    public int EmployeeId { get; set; } 
    public int Count { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;
}
