
﻿using System.ComponentModel.DataAnnotations;

namespace EpicorWeb.Models
{
    public class Users
    {
        [Key]
        public string? ID { get; set; }
        [Required]
        public string? Name { get; set; }
        
        public string? GroupName { get; set; }
        
        public int? GroupID { get; set; }

        public Groups? Groups { get; set; }


    }
}
