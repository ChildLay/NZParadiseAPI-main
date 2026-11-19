
using NZParadiseAPI.Model.Entities;

namespace NZParadiseAPI.Model
{
    public class PropertyDTO
    {
        public int PropertyID { get; set; }
        public string Description { get; set; } = "";
        public decimal Price { get; set; }
        public string Location { get; set; } = "";
        public int Bedroom { get; set; }
        public int Bathroom { get; set; }
        public int Parking { get; set; }
        public decimal Area { get; set; }
        public string? PropertyFeatureIDs { get; set; }
        public DateTime? AvailableFromDate { get; set; }
        public bool IsInWishlist { get; set; }
        public string PropertyTypeName { get; set; } = "";
        public int PropertyTypeID { get; set; }
        public UserDTO? User { get; set; }
        public List<AttachmentDTO>? Attachments { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}