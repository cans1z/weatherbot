using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace weatherbot.Models;

public class User
{
    [Key] public int Id { get; set; }

    public long TgId { get; set; }

    [DefaultValue("Moscow")] public string? City { get; set; } = "Moscow";

    [DefaultValue("default")] public string State { get; set; } = "default";
}