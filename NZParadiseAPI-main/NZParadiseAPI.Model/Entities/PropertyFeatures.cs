using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NZParadiseAPI.Model.Entities
{
    public class PropertyFeatures
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PropertyFeatureID { get; set; }
        [Required]
        [MaxLength(100)]
        public string Description { get; set; }
    }
}
