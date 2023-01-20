#region

using System.ComponentModel.DataAnnotations;

#endregion

namespace RWF.Model.Entities;

public class EntityBase
{
    [Key] public int Id { get; set; }
}