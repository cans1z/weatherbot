using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace weatherbot.Models;

public class Time
{
    [Key] public int Id { get; set; }

    public int UserId { get; set; }

    [Column("Time")]
    [DefaultValue("7:00")]
    public string TimeStr { get; set; } = "7:00";
}