using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NZParadiseAPI.Library;
using NZParadiseAPI.Model.Abstract;
using NZParadiseAPI.Model.Entities;

namespace NZParadiseAPI.Model.Concrete
{
    public class PropertiesRepository : IPropertiesRepository
    {
        private readonly ApplicationDbContext _context;
        public PropertiesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        private bool IsValidProperty(Properties property, out string message)
        {
            if (string.IsNullOrWhiteSpace(property.Description))
            {
                message = "Property description cannot be empty!";
                return false;
            }
            if (string.IsNullOrWhiteSpace(property.Location))
            {
                message = "Location cannot be empty!";
                return false;
            }
            message = "";
            return true;
        }

        public async Task<ResponseObject> RegisterNewProperty(Properties property)
        {
            try
            {
                if (!IsValidProperty(property, out string msg))
                {
                    return new ResponseObject
                    {
                        Status = (int)CommonEnum.ResponseStatus.Failed,
                        Message = msg
                    };
                }
                else
                {
                    property.CreatedDate = DateTime.Now;
                    _context.Properties.Add(property);
                    await _context.SaveChangesAsync();

                    return new ResponseObject
                    {
                        Status = (int)CommonEnum.ResponseStatus.Success,
                        Data = property
                    };
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<List<PropertyFeatures>> GetPropertyFeatures()
        {
            return await _context.PropertyFeatures.ToListAsync();
        }

        public async Task<List<PropertyTypes>> GetPropertyTypes()
        {
            return await _context.PropertyTypes.ToListAsync();
        }

        public async Task<ResponseObject> UpdateProperty(Properties property)
        {
            if (!IsValidProperty(property, out string msg))
            {
                return new ResponseObject
                {
                    Status = (int)CommonEnum.ResponseStatus.Failed,
                    Message = msg
                };
            }
            else
            {
                Properties existing = await _context.Properties.Where(x => x.PropertyID == property.PropertyID).FirstOrDefaultAsync();
                if (existing == null)
                {
                    return new ResponseObject
                    {
                        Status = (int)CommonEnum.ResponseStatus.Failed,
                        Message = "Property not found"
                    };
                }
                else
                {
                    existing.ModifiedDate = DateTime.Now;
                    existing.PropertyType = property.PropertyType;
                    existing.PropertyTypeID = property.PropertyTypeID;
                    existing.Description = property.Description;
                    existing.Price = property.Price;
                    existing.Location = property.Location;
                    existing.Bathroom = property.Bathroom;
                    existing.Bedroom = property.Bedroom;
                    existing.Parking = property.Parking;
                    existing.Area = property.Area;
                    existing.PropertyFeatureIDs = property.PropertyFeatureIDs;
                    existing.AvailableFromDate = property.AvailableFromDate;
                    existing.Status = property.Status;

                    await _context.SaveChangesAsync();

                    return new ResponseObject
                    {
                        Status = (int)CommonEnum.ResponseStatus.Success,
                        Data = existing
                    };
                }
            }
        }

        public async Task<Properties> GetPropertyByID(int propertyID)
        {
            return await _context.Properties.FirstAsync(x => x.PropertyID == propertyID);
        }

        public async Task Delete(Properties prop)
        {
            _context.Properties.Remove(prop);
            await _context.SaveChangesAsync();
        }

        public async Task<ResponseObject> GetPropertyDetails(int propertyID)
        {
            Properties? p = await _context.Properties
                .Include(x => x.User)
                .Include(x => x.Attachments)
                .Include(x => x.PropertyType)
                .FirstOrDefaultAsync(x => x.PropertyID == propertyID);

            PropertyDTO? result = null;
            if (p != null)
            {
                result = new PropertyDTO
                {
                    PropertyID = p.PropertyID,
                    Description = p.Description,
                    Price = p.Price,
                    Location = p.Location,
                    Bedroom = p.Bedroom,
                    Bathroom = p.Bathroom,
                    Parking = p.Parking,
                    Area = p.Area,
                    PropertyFeatureIDs = p.PropertyFeatureIDs,
                    AvailableFromDate = p.AvailableFromDate,
                    CreatedDate = p.CreatedDate,
                    PropertyTypeID = p.PropertyTypeID,
                    PropertyTypeName = p.PropertyType != null ? p.PropertyType.Description : "",
                    User = p.User == null? null: DTOHelper.ConvertToUserDTO(p.User),
                    Attachments = p.Attachments?.Select(a => new AttachmentDTO
                    {
                        AttachmentID = a.AttachmentID,
                        FilePath = a.FilePath
                    }).ToList()
                };
            }

            return new ResponseObject
            {
                Data = result,
                Status = p == null ? (int)CommonEnum.ResponseStatus.Failed : (int)CommonEnum.ResponseStatus.Success,
                Message = p == null ? "Property not found" : ""
            };
        }

        public async Task<DashboardSummaryDto> GetDashboardData(int userID)
        {
            var listedGroup = await _context.Properties
                .Where(p => p.UserID == userID && p.PropertyType != null)
                .GroupBy(p => p.PropertyType.Description)
                .Select(g => new CountGroupDto { Type = g.Key, Count = g.Count() })
                .ToListAsync();

            var wishlistedGroup = await _context.Wishlists
                .Where(w => w.UserID == userID)
                .GroupBy(w => w.Property.PropertyType.Description)
                .Select(g => new { type = g.Key, count = g.Count() })
                .ToListAsync();

            var recentMessages = await _context.Messages
                .Where(m => m.Property.UserID == userID)
                .OrderByDescending(m => m.CreatedDate)
                .Take(3)
                .Select(m => new
                {
                    id = m.MessageID,
                    sender = m.FromUser.FirstName + " " + m.FromUser.LastName,
                    preview = m.MessageDesc.Substring(0, 50),
                    createdDate = m.CreatedDate
                })
                .ToListAsync();

            var wishlistTrend = await _context.Wishlists
                .Where(w => w.Property.UserID == userID)
                .GroupBy(w => new { w.CreatedDate.Year, w.CreatedDate.Month })
                .Select(g => new
                {
                    month = $"{CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(g.Key.Month)}",
                    count = g.Count()
                })
                .ToListAsync();

            int totalProperties = await _context.Properties.CountAsync(p => p.UserID == userID);
            int totalWishlist = await _context.Wishlists.CountAsync(w => w.UserID == userID);

            return new DashboardSummaryDto
            {
                ListedPropertiesByType = listedGroup.Select(g => new CountGroupDto { Type = g.Type, Count = g.Count }).ToList(),
                WishlistedPropertiesByType = wishlistedGroup.Select(g => new CountGroupDto { Type = g.type, Count = g.count }).ToList(),
                RecentMessages = recentMessages.Select(m => new MessageDto { Id = m.id, Sender = m.sender, Preview = m.preview, CreatedDate = m.createdDate }).ToList(),
                WishlistTrend = wishlistTrend.Select(w => new TrendDto { Month = w.month, Count = w.count }).ToList(),
                TotalProperties = totalProperties,
                TotalWishlist = totalWishlist
            };
        }


        public async Task<ResponseObject> GetPropertyList(PropertyFilter filter)
        {
            try
            {
                var query = _context.Properties
                    .Include(x => x.User)
                    .Include(x => x.PropertyType)
                    .Include(x => x.Attachments)
                    .AsQueryable();

                if (!string.IsNullOrEmpty(filter.Description))
                {
                    query = query.Where(p => p.Description.Contains(filter.Description));
                }
                if (!string.IsNullOrEmpty(filter.Location))
                {
                    query = query.Where(p => p.Location.Contains(filter.Location));
                }
                if (filter.BedRoom.HasValue)
                {
                    query = query.Where(p => p.Bedroom == filter.BedRoom.Value);
                }
                if (filter.BathRoom.HasValue)
                {
                    query = query.Where(p => p.Bathroom == filter.BathRoom.Value);
                }
                if (filter.Parking.HasValue)
                {
                    query = query.Where(p => p.Parking == filter.Parking.Value);
                }
                if (filter.PropertyTypeID != null && filter.PropertyTypeID != 0)
                {
                    query = query.Where(p => p.PropertyTypeID == filter.PropertyTypeID);
                }
                if (filter.mode == "myListings" && filter.UserID.HasValue)
                {
                    query = query.Where(p => p.UserID == filter.UserID.Value);
                }
                if (filter.MinPrice != null && Convert.ToInt32(filter.MinPrice) > 0)
                {
                    query = query.Where(p => p.Price >= filter.MinPrice);
                }
                if (filter.MaxPrice != null && Convert.ToInt32(filter.MaxPrice) > 0)
                {
                    query = query.Where(p => p.Price <= filter.MaxPrice);
                }

                var wishlistedPropertyIDs = _context.Wishlists
                    .Where(w => filter.UserID.HasValue && w.UserID == filter.UserID.Value)
                    .Select(w => w.PropertyID);

                if (filter.mode == "wishlist" && filter.UserID.HasValue)
                {
                    query = query.Where(p => wishlistedPropertyIDs.Contains(p.PropertyID));
                }

                var properties = await query
                    .OrderByDescending(p => p.PropertyID)
                    .ToListAsync();

                var wishlistedSet = new HashSet<int>(wishlistedPropertyIDs);

                var results = properties.Select(p => new PropertyDTO
                {
                    PropertyID = p.PropertyID,
                    Description = p.Description,
                    Price = p.Price,
                    Location = p.Location,
                    Bedroom = p.Bedroom,
                    Bathroom = p.Bathroom,
                    Parking = p.Parking,
                    Area = p.Area,
                    PropertyFeatureIDs = p.PropertyFeatureIDs,
                    AvailableFromDate = p.AvailableFromDate,
                    CreatedDate = p.CreatedDate,
                    IsInWishlist = wishlistedSet.Contains(p.PropertyID),
                    PropertyTypeID = p.PropertyTypeID,
                    PropertyTypeName = p.PropertyType != null ? p.PropertyType.Description : "",
                    User = p.User == null ? null : new UserDTO
                    {
                        UserID = p.User.UserID,
                        FirstName = p.User.FirstName,
                        LastName = p.User.LastName,
                        AgencyName = p.User.AgencyName,
                        Email = p.User.Email,
                        PhoneNo = p.User.PhoneNo
                    },
                    Attachments = p.Attachments?.Take(1).Select(a => new AttachmentDTO
                    {
                        AttachmentID = a.AttachmentID,
                        FilePath = a.FilePath
                    }).ToList()
                }).ToList();

                return new ResponseObject
                {
                    Status = (int)CommonEnum.ResponseStatus.Success,
                    Message = $"Found {results.Count}",
                    Data = results
                };
            }
            catch (Exception ex)
            {
                return new ResponseObject
                {
                    Status = (int)CommonEnum.ResponseStatus.Failed,
                    Message = $"Error: {ex.Message}"
                };
            }
        }

    }
}
