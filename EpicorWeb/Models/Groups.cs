using System.ComponentModel.DataAnnotations;

﻿namespace EpicorWeb.Models
{
    public class Groups
    {
            [Key]
        public int ID { get; set; }
            [Required]
        public string? Name { get; set; }
            [Required]
        public string? Description { get; set; }
    }
}
