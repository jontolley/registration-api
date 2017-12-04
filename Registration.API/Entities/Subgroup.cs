﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Registration.API.Entities
{
    public class Subgroup
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int PinNumber { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(200)]
        public string Description { get; set; }

        [ForeignKey("GroupId")]
        public Group Group { get; set; }
        public int GroupId { get; set; }
        
        public ICollection<UserSubgroup> UserSubgroups { get; set; }
               = new List<UserSubgroup>();
    }
}
