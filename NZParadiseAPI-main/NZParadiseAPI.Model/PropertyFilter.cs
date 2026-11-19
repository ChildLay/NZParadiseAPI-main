using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NZParadiseAPI.Model
{
    public class PropertyFilter
    {
        public int? UserID { get; set; }
        public string? Description { get; set; }
        public int? BedRoom { get; set; }
        public int? BathRoom { get; set; }
        public int? PropertyTypeID { get; set; }
        public string? Location { get; set; }
        public int? Area { get; set; }
        public int? Parking { get; set; }
        public string mode { get; set; }
        public int? MinPrice { get; set; }
        public int? MaxPrice { get; set; }
    }
}
