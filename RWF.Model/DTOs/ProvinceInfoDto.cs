namespace RWF.Model.DTOs;

public class ProvinceInfoDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Capital { get; set; } = string.Empty;
    public int Code { get; set; }
    public int CityCount { get; set; }
    public decimal Overall { get; set; }
}