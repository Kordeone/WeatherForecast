namespace RWF.Common.Models;

public class ErrorLog
{
    public string StackTrace { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public DateTime? RegisterDate { get; set; }
}