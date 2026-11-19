namespace NZParadiseAPI.Model
{
    public class UserDTO
    {
        public int UserID { get; set; }
        public int UserTypeID { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string? AgencyName { get; set; }
        public string Email { get; set; } = "";
        public string PhoneNo { get; set; } = "";
        public DateTime CreatedDate { get; set; }
        public int PropertyCount { get; set; }
    }
}