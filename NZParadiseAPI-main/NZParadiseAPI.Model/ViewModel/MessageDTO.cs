
using NZParadiseAPI.Model.Entities;

namespace NZParadiseAPI.Model
{
    public class MessageDTO
    {
        public int FromUserID { get; set; }
        public string SenderFirstName { get; set; }
        public string SenderLastName { get; set; }
        public string SenderEmail { get; set; }
        public string MessageDesc { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}