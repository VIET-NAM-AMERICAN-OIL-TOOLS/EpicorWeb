
﻿using System.ComponentModel.DataAnnotations;

namespace EpicorWeb.Models
{
    public class Category
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public int? ParentID { get; set; }
        [Required]
        public int? Level { get; set; }
        [Required]
        public string? Url { get; set; }
    } 
}