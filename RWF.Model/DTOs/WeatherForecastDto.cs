namespace RWF.Model.DTOs;

public class WeatherForecastDto
{
    public int CityId { get; set; }
    public string WeekDay { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public decimal MinTemp { get; set; }
    public decimal MaxTemp { get; set; }
}