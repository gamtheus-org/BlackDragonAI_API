using System.ComponentModel.DataAnnotations;

namespace BlackDragonAIAPI.Models
{
    public class BannedTerm
    {
        [Key] public string Term { get; set; }
    }
}