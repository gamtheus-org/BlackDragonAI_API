using System.ComponentModel.DataAnnotations;

namespace BlackDragonAIAPI.Models;

public record BannedTerm
{
    [Key] public string Term { get; set; }
}