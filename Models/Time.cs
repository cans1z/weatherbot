using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace weatherbot.Models
{
    public class Time
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        
        [Column("Time")]
        [DefaultValue("7:00")]
        public string TimeStr { get; set; } = "7:00";
    }
}
