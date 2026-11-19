using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NZParadiseAPI.Model.Concrete;
using NZParadiseAPI.Model;
using NZParadiseAPI.Library;
using NZParadiseAPI.Model.Entities;
using Azure;
using Microsoft.AspNetCore.Http;

namespace NZParadiseAPI.Service
{
    public class PropertyService
    {
        private readonly AttachmentsRepository _attachmentRepo;
        private readonly PropertiesRepository _repo;
        private readonly AzureBlobService _blobService;
        private readonly WishlistsRepository _wishlistRepo;
        private readonly MessageRepository _messageRepo;
        private readonly AuditLogService _logService;
        public PropertyService(ApplicationDbContext context, AzureBlobService blobService)
        {
            _messageRepo = new MessageRepository(context);
            _repo = new PropertiesRepository(context);
            _wishlistRepo = new WishlistsRepository(context);
            _blobService = blobService;
            _attachmentRepo = new AttachmentsRepository(context);
            _logService = new AuditLogService(context);
        }

        public async Task<ResponseObject> GetPropertyDetails(int id)
        {
            return await _repo.GetPropertyDetails(id);
        }

        public async Task<ResponseObject> GetPropertyList(PropertyFilter filter)
        {
            return await _repo.GetPropertyList(filter);
        }

        public async Task<ResponseObject> RegisterNewProperty(Properties aProperty, List<IFormFile> files)
        {
            var response = await _repo.RegisterNewProperty(aProperty);
            if (response.Status == (int)CommonEnum.ResponseStatus.Success)
            {
                // save images
                if (files.Count > 0)
                {
                    await UploadAttachments(aProperty.PropertyID, aProperty.UserID, files);
                }
            }

            return response;
        }

        public async Task<ResponseObject> UpdateProperty(Properties aProperty, List<IFormFile> files, List<int> removedFileIDs)
        {
            var response = await _repo.UpdateProperty(aProperty);
            if (response.Status == (int)CommonEnum.ResponseStatus.Success)
            {
                // remove images (if any)
                if (removedFileIDs.Count > 0)
                {
                    await RemoveAttachments(aProperty.PropertyID, aProperty.UserID, removedFileIDs);
                }

                // save images
                if (files.Count > 0)
                {
                    await UploadAttachments(aProperty.PropertyID, aProperty.UserID, files);
                }

                await _logService.WriteLog(aProperty.UserID, $"Property updated. d:{(aProperty.Description.Length > 30 ? (aProperty.Description[..30] + "...") : aProperty.Description)} i:{aProperty.PropertyID}, p:{aProperty.Price}, pk:{aProperty.Parking}, bd:{aProperty.Bedroom}, bth:{aProperty.Bathroom}, a:{aProperty.Area}l:{aProperty.Location}an:{files.Count}ar:{removedFileIDs.Count}");
            }
            return response;
        }

        private async Task RemoveAttachments(int propertyID, int userID, List<int> removedFileIDs)
        {
            var uploadedUrls = new List<string>();

            foreach (int attachmentId in removedFileIDs)
            {
                Attachments? removeAttachment = await _attachmentRepo.GetAttachmentByID(attachmentId);
                if (removeAttachment != null)
                {
                    // remove from azure
                    await _blobService.DeleteFileAsync(removeAttachment.FilePath); // remove from Azure Blob
                    await _attachmentRepo.Delete(removeAttachment);

                }
            }

            if (uploadedUrls.Count > 0)
            {
                foreach (string url in uploadedUrls)
                {
                    Attachments attachmentData = new Attachments
                    {
                        //Description = "Property Image",
                        FilePath = url,
                        ParentID = propertyID,
                        ParentType = (int)CommonEnum.AttachmentParentType.PropertyImage,
                        UserID = userID,
                        CreatedDate = DateTime.Now
                    };

                    await _attachmentRepo.Save(attachmentData);
                }
            }
        }

        private async Task UploadAttachments(int propertyID, int userID, List<IFormFile> files)
        {
            var uploadedUrls = new List<string>();

            foreach (var file in files)
            {
                var url = await _blobService.UploadFileAsync(file);
                uploadedUrls.Add(url);
            }

            if (uploadedUrls.Count > 0)
            {
                foreach (string url in uploadedUrls)
                {
                    Attachments attachmentData = new Attachments
                    {
                        //Description = "Property Image",
                        FilePath = url,
                        ParentID = propertyID,
                        ParentType = (int)CommonEnum.AttachmentParentType.PropertyImage,
                        UserID = userID,
                        CreatedDate = DateTime.Now
                    };

                    await _attachmentRepo.Save(attachmentData);
                }
            }
        }

        public async Task<ResponseObject> GetReferences()
        {
            var data = new
            {
                propertyTypes = await _repo.GetPropertyTypes(),
                propertyFeatures = await _repo.GetPropertyFeatures()
            };

            return new ResponseObject
            {
                Status = (int)CommonEnum.ResponseStatus.Success,
                Data = data
            };
        }

        public async Task<ResponseObject> GetDashboardData(int userID)
        {
            var result = await _repo.GetDashboardData(userID);
            return new ResponseObject
            {
                Status = (int)CommonEnum.ResponseStatus.Success,
                Data = result
            };
        }

        public async Task<ResponseObject> DeleteProperty(int propertyID, int userID)
        {
            Properties prop = await _repo.GetPropertyByID(propertyID);

            if (prop == null)
            {
                return new ResponseObject
                {
                    Status = (int)CommonEnum.ResponseStatus.Failed,
                    Message = "Property not found"
                };
            }
            else if (prop.UserID != userID)
            {
                return new ResponseObject
                {
                    Status = (int)CommonEnum.ResponseStatus.Failed,
                    Message = "Only property owner can delete their properties"
                };
            }
            else
            {
                string delPropertyInfo = $"i:{prop.PropertyID}, d:{(prop.Description.Length > 30 ? (prop.Description[..30] + "...") : prop.Description)}p:{prop.Price}, pk:{prop.Parking}, bd:{prop.Bedroom}, bth:{prop.Bathroom}, a:{prop.Area}l:{prop.Location}";

                // 1. Delete attachments and files from Azure Blob
                var attachments = prop.Attachments?.ToList() ?? new List<Attachments>();
                foreach (var att in attachments)
                {
                    await _blobService.DeleteFileAsync(att.FilePath);
                    await _attachmentRepo.Delete(att);
                }

                // 2. Delete wishlists
                // var wishlists = _context.Wishlists
                //     .Where(w => w.PropertyID == propertyID).ToList();
                // foreach (var w in wishlists)
                // {
                //     await _wishlistRepo.Delete(w);
                // }
                await _wishlistRepo.DeleteByPropertyID(propertyID);

                // 3. Delete messages
                // var messages = _context.Messages
                //     .Where(m => m.PropertyID == propertyID).ToList();
                // _context.Messages.RemoveRange(messages);
                await _messageRepo.DeleteByPropertyID(propertyID);

                // 4. Delete the property itself
                // _context.Properties.Remove(prop);
                // await _context.SaveChangesAsync();
                await _repo.Delete(prop);

                await _logService.WriteLog(userID, $@"Property deleted. {delPropertyInfo}");

                return new ResponseObject
                {
                    Status = (int)CommonEnum.ResponseStatus.Success,
                    Message = $"Property {propertyID} and all related data deleted"
                };
            }
        }

        public async Task<ResponseObject> ToggleWishlist(int propertyID, int userID, bool addToWishlist)
        {
            bool isSuccess = false;
            var wishListData = await _wishlistRepo.GetWishlistData(userID, propertyID);

            if (wishListData == null && addToWishlist == true)
            {
                wishListData = new Wishlist
                {
                    PropertyID = propertyID,
                    UserID = userID,
                    CreatedDate = DateTime.Now
                };

                isSuccess = await _wishlistRepo.Save(wishListData);
            }
            else
            {
                if (wishListData != null)
                {
                    isSuccess = await _wishlistRepo.Delete(wishListData);
                }
            }

            return new ResponseObject
            {
                Status = isSuccess ? (int)CommonEnum.ResponseStatus.Success : (int)CommonEnum.ResponseStatus.Failed,
                Message = "OK",
                Data = addToWishlist
            };
        }
    }
}
