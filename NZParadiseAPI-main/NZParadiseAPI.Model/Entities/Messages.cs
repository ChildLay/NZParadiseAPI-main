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
    public class Messages
    {
        [Key]
        public int MessageID { get; set; }

        [Required]
        public int PropertyID { get; set; }

        public int? FromUserID { get; set; } // Nullable

        [Required]
        [MaxLength(300)]
        public string MessageDesc { get; set; }

        public DateTime CreatedDate { get; set; }

        [ForeignKey("PropertyID")]
        public virtual Properties Property { get; set; }

        [ForeignKey("FromUserID")]
        public virtual Users FromUser { get; set; }
    }
}
