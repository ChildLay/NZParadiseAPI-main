using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NZParadiseAPI.Model.Entities
{
    public class Wishlist
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WishlistID { get; set; }
        [Required]
        public int PropertyID { get; set; }
        [Required]
        public int UserID { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }
        [ForeignKey("PropertyID")]
        public virtual Properties Property { get; set; }
    }
}
