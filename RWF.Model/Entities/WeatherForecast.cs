namespace RWF.Model.Entities;

public class WeatherForecast : EntityBase
{
    public string WeekDay { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public decimal MinTemp { get; set; }
    public decimal MaxTemp { get; set; }
}