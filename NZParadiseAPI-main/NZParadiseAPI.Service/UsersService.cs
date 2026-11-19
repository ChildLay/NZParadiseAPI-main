using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NZParadiseAPI.Model;
using NZParadiseAPI.Library;
using NZParadiseAPI.Model.Concrete;
using NZParadiseAPI.Model.Entities;


namespace NZParadiseAPI.Service
{
    public class UsersService
    {
        private readonly UsersRepository _userRepository;
        private readonly JwtSettings _jwtSettings;
        private readonly ApplicationDbContext _context;
        private readonly AuditLogService _logService;
        public UsersService(ApplicationDbContext context, JwtSettings jwtSettings)
        {
            _context = context;
            _userRepository = new UsersRepository(context);
            _jwtSettings = jwtSettings;
            _logService = new AuditLogService(context);
        }

        public async Task<ResponseObject> RegisterNewUser(Users user)
        {
            var (isValid, message) = await IsValidNewUser(user);
            if (!isValid)
            {
                return new ResponseObject { Status = 0, Message = message };
            }
            else
            {
                user.CreatedDate = DateTime.Now;
                user.Password = Utility.HashString(user.Password); // encrypt password
                // bool isValid = Utility.IsValidHash(clientHashedPwd, user.Password);

                try
                {
                    user = await _userRepository.Save(user);

                    return new ResponseObject
                    {
                        Data = user,
                        Status = (int)CommonEnum.ResponseStatus.Success
                    };
                }
                catch (Exception ex)
                {
                    return new ResponseObject
                    {
                        Message = ex.Message,
                        Status = (int)CommonEnum.ResponseStatus.Failed
                    };
                }
            }
        }

        public async Task<ResponseObject> GetUserByID(int id)
        {
            Users? user = await _userRepository.GetUserByID(id);

            return new ResponseObject
            {
                Data = user != null ? DTOHelper.ConvertToUserDTO(user) : null,
                Status = user == null ? (int)CommonEnum.ResponseStatus.Failed : (int)CommonEnum.ResponseStatus.Success,
                Message = user == null ? "User not found" : ""
            };
        }

        public async Task<ResponseObject> GetUserList(int adminUserID)
        {
            Users? user = await _userRepository.GetUserByID(adminUserID);
            if (user == null || user.UserTypeID != (int)CommonEnum.UserType.Admin)
            {
                return new ResponseObject
                {
                    Status = (int)CommonEnum.ResponseStatus.Failed,
                    Message = "You are not authorized to view this data"
                };
            }

            List<Users> userList = await _userRepository.GetUserList();

            List<UserDTO> dtoList = userList.Select(user => new UserDTO
            {
                UserID = user.UserID,
                UserTypeID = user.UserTypeID,
                FirstName = user.FirstName,
                LastName = user.LastName,
                AgencyName = user.AgencyName,
                Email = user.Email,
                PhoneNo = user.PhoneNo,
                CreatedDate = user.CreatedDate,
                PropertyCount = _context.Properties.Count(p => p.UserID == user.UserID)
            }).ToList();

            return new ResponseObject
            {
                Status = (int)CommonEnum.ResponseStatus.Success,
                Data = dtoList
            };
        }

        public async Task<ResponseObject> ValidateLogin(string email, string password)
        {
            ResponseObject obj = new ResponseObject();
            var user = await _userRepository.GetUserByEmail(email);
            if (user == null)
            {
                obj.Status = (int)CommonEnum.ResponseStatus.Failed;
                obj.Message = "User not found";
            }
            else
            {
                bool isValidPassword = Utility.IsValidHash(password, user.Password);
                if (!isValidPassword)
                {
                    obj.Status = (int)CommonEnum.ResponseStatus.Failed;
                    obj.Message = "Incorrect password";
                }
                else
                {
                    // valid email and password
                    var token = Utility.GenerateJWTToken(email, _jwtSettings.Key, _jwtSettings.Issuer, _jwtSettings.Audience);
                    obj.Status = (int)CommonEnum.ResponseStatus.Success;
                    obj.Message = "success";
                    obj.Data = new
                    {
                        User = DTOHelper.ConvertToUserDTO(user),
                        token
                    };

                    await _logService.WriteLog(user.UserID, "Login to system");
                }
            }

            return obj;
        }

        private async Task<(bool isValid, string message)> IsValidNewUser(Users user)
        {
            if (string.IsNullOrWhiteSpace(user.FirstName) ||
                string.IsNullOrWhiteSpace(user.LastName) ||
                string.IsNullOrWhiteSpace(user.Password) ||
                string.IsNullOrWhiteSpace(user.Email))
            {
                return (false, "First Name, Last Name, Password or Email cannot be empty!");
            }

            var existingUser = await _userRepository.GetUserByEmail(user.Email);
            if (existingUser != null)
            {
                return (false, "Email already registered!");
            }

            return (true, "");
        }

        public async Task<ResponseObject> UpdateUser(Users user)
        {
            Users? existingUser = await _userRepository.GetUserByID(user.UserID);
            if (IsValidExistingUser(user, existingUser, out string message) && existingUser != null)
            {
                existingUser.UserTypeID = user.UserTypeID;
                existingUser.FirstName = user.FirstName;
                existingUser.LastName = user.LastName;
                existingUser.AgencyName = user.AgencyName;
                existingUser.PhoneNo = user.PhoneNo;
                existingUser.Email = user.Email;
                existingUser.Gender = user.Gender;
                existingUser.ModifiedDate = DateTime.Now;

                bool passwordChanged = false;
                if (!string.IsNullOrWhiteSpace(user.Password) && user.Password != "qm6K0DKJqgKcac4P42flvijZ5I7wFe")
                {
                    passwordChanged = true;
                    existingUser.Password = Utility.HashString(user.Password);
                }

                Users updatedUser = await _userRepository.Update(existingUser);

                await _logService.WriteLog(user.UserID, $"Profile updated. f:{user.FirstName}, l:{user.LastName}, e:{user.Email}, p:{user.PhoneNo}, a:{user.AgencyName}, p:{Convert.ToInt32(passwordChanged)}");

                return new ResponseObject
                {
                    Status = (int)CommonEnum.ResponseStatus.Success,
                    Data = DTOHelper.ConvertToUserDTO(updatedUser)
                };
            }
            else
            {
                return new ResponseObject
                {
                    Status = (int)CommonEnum.ResponseStatus.Failed,
                    Message = message
                };
            }
        }

        private bool IsValidExistingUser(Users user, Users? existingUser, out string message)
        {
            if (string.IsNullOrWhiteSpace(user.FirstName) ||
                string.IsNullOrWhiteSpace(user.LastName) ||
                string.IsNullOrWhiteSpace(user.Email) ||
                string.IsNullOrWhiteSpace(user.CurrentPassword)
                )
            {
                message = "First Name, Last Name, Email or Password cannot be empty!";
                return false;
            }
            // check user ID
            if (existingUser == null)
            {
                message = "User not found";
                return false;
            }
            // check current password
            if (!Utility.IsValidHash(user.CurrentPassword, existingUser.Password))
            {
                message = "Incorrect password";
                return false;
            }

            message = "";
            return true;
        }
    }
}
