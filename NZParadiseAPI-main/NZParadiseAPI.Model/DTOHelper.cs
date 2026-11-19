using NZParadiseAPI.Model.Entities;

namespace NZParadiseAPI.Model
{
    public static class DTOHelper
    {
        public static UserDTO ConvertToUserDTO(Users user)
        {
            return new UserDTO
            {
                UserID = user.UserID,
                UserTypeID = user.UserTypeID,
                FirstName = user.FirstName,
                LastName = user.LastName,
                AgencyName = user.AgencyName,
                Email = user.Email,
                PhoneNo = user.PhoneNo,
                CreatedDate = user.CreatedDate
            };
        }
    }
}
