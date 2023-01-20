#region

#endregion

namespace RWF.Model.Entities;

public class Province : EntityBase
{
    public string Name { get; set; } = string.Empty;
    public string Capital { get; set; } = string.Empty;
    public int Code { get; set; }
    public int CityCount { get; set; }
    public decimal Overall { get; set; }
    public ICollection<City> Cities { get; set; } = null!;
}