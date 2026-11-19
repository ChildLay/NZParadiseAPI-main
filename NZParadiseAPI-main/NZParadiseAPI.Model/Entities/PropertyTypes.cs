using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NZParadiseAPI.Model.Entities
{
    public class PropertyTypes
    {
        [Key]
        public int PropertyTypeID { get; set; }
        [Required]
        [MaxLength(100)]
        public string Description { get; set; }
    }
}
