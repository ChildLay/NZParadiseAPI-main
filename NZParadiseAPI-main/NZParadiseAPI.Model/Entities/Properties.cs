using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NZParadiseAPI.Model.Entities
{
    public class Properties
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PropertyID { get; set; }

        [Required]
        public int UserID { get; set; }

        [Required]
        public int PropertyTypeID { get; set; }

        [Required]
        [MaxLength(300)]
        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        [MaxLength(100)]
        public string Location { get; set; }

        [Required]
        public int Bedroom { get; set; }

        [Required]
        public int Bathroom { get; set; }

        [Required]
        public int Parking { get; set; }

        [Required]
        public decimal Area { get; set; }

        [MaxLength(50)]
        public string? PropertyFeatureIDs { get; set; }

        public DateTime? AvailableFromDate { get; set; }

        [Required]
        public int Status { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        // Relationships
        [ForeignKey("UserID")]
        public virtual Users? User { get; set; }

        [ForeignKey("PropertyTypeID")]
        //[JsonIgnore]
        public virtual PropertyTypes? PropertyType { get; set; }

        [NotMapped]
        public bool IsInWishlist { get; set; }

        public virtual List<Attachments>? Attachments { get; set; }

        public virtual List<Messages>? Messages { get; set; }
    }
}
