using NZParadiseAPI.Model.Entities;

namespace NZParadiseAPI.Model
{
    public class DashboardSummaryDto
    {
        public List<CountGroupDto> ListedPropertiesByType { get; set; }
        public List<CountGroupDto> WishlistedPropertiesByType { get; set; }
        public List<MessageDto> RecentMessages { get; set; }
        public List<TrendDto> WishlistTrend { get; set; }
        public int TotalProperties { get; set; }
        public int TotalWishlist { get; set; }
    }

    public class CountGroupDto
    {
        public string Type { get; set; }
        public int Count { get; set; }
    }

    public class MessageDto
    {
        public int Id { get; set; }
        public string Sender { get; set; }
        public string Preview { get; set; }
        public DateTime? CreatedDate { get; set; }
    }

    public class TrendDto
    {
        public string Month { get; set; }
        public int Count { get; set; }
    }
}