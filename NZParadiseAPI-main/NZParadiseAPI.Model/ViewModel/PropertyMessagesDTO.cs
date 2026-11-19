using NZParadiseAPI.Model.Entities;

namespace NZParadiseAPI.Model
{
    public class PropertyMessagesDTO
    {
        public string PropertyID { get; set; }
        public string PropertyDesc { get; set; }
        public string PropertyImage { get; set; }
        public decimal Price { get; set; }
        public string Location {get;set;}
        public List<MessageDTO> MessageList { get; set; }
    }
}