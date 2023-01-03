namespace DinaApp.Models;

public class Book
{
    public int Id { get; set; } 
    public int BookId { get; set; } 
    public string Name { get; set; }
    public int CategoryId { get; set; }
    public int YearId { get; set; }
    public int PaperCount { get; set; }
    public int Count { get; set; }
    public int Finish { get; set; }
    public int UnFinish { get; set; }
    public string Date { get; set; }
    public bool IsShow { get; set; } = true;
}
