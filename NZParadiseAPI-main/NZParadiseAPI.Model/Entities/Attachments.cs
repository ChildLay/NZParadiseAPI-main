using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace NZParadiseAPI.Model.Entities
{
    public class Attachments
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AttachmentID { get; set; }
        //[Required]
        // [MaxLength(100)]
        // public string Description { get; set; }
        [Required]
        [MaxLength(200)]
        public string FilePath { get; set; }
        [Required]
        public int ParentID { get; set; }
        [Required]
        public int ParentType { get; set; }
        [Required]
        public int UserID { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }
        //public DateTime? ModifiedDate { get; set; }

        [ForeignKey("ParentID")]
        [JsonIgnore]
        public virtual Properties? Property { get; set; } 
    }
}
