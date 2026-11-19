using System;
using System.Collections.Generic;
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
    public class MessageRepository
    {
        private readonly ApplicationDbContext _context;
        public MessageRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<PropertyMessagesDTO>> GetPropertyMessagesByUserID(int userID)
        {
            var result = await _context.Properties
                .Where(p => p.UserID == userID && p.Messages.Any())
                .Include(p => p.Messages)
                    .ThenInclude(m => m.FromUser)
                .Select(p => new PropertyMessagesDTO
                {
                    PropertyID = p.PropertyID.ToString(),
                    PropertyDesc = p.Description,
                    Price = p.Price,
                    Location = p.Location,
                    MessageList = p.Messages
                        .OrderByDescending(m => m.CreatedDate)
                        .Select(m => new MessageDTO
                        {
                            FromUserID = m.FromUser.UserID,
                            SenderFirstName = m.FromUser.FirstName,
                            SenderLastName = m.FromUser.LastName,
                            SenderEmail = m.FromUser.Email,
                            MessageDesc = m.MessageDesc,
                            CreatedDate = m.CreatedDate
                        }).ToList()
                })
                .ToListAsync();

            // Attach the first image (if exists) for each property
            foreach (var item in result)
            {
                var firstImage = await _context.Attachments
                    .Where(a => a.ParentID.ToString() == item.PropertyID && a.ParentType == (int)CommonEnum.AttachmentParentType.PropertyImage)
                    .OrderBy(a => a.CreatedDate)
                    .Select(a => a.FilePath)
                    .FirstOrDefaultAsync();

                item.PropertyImage = firstImage ?? "";
            }

            return result;
        }

        public async Task<bool> SendMessage(Messages message)
        {
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task DeleteByPropertyID(int propertyID)
        {
            var messageList = _context.Messages.Where(w => w.PropertyID == propertyID).ToList();

            foreach (var msg in messageList)
            {
                _context.Messages.Remove(msg);
                await _context.SaveChangesAsync();
            }
        }
    }
}