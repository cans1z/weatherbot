using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace weatherbot.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public long TgId { get; set; }

        [DefaultValue("Moscow")]
        public string? City { get; set; } = "Moscow";

        [DefaultValue("default")]
        public string State { get; set; } = "default";
    }
}
