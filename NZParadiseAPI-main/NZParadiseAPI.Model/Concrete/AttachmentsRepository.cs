using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NZParadiseAPI.Library;
using NZParadiseAPI.Model.Abstract;
using NZParadiseAPI.Model.Entities;

namespace NZParadiseAPI.Model.Concrete
{
    public class AttachmentsRepository
    {
        private readonly ApplicationDbContext _context;
        public AttachmentsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Save(Attachments attachmentData)
        {
            _context.Attachments.Add(attachmentData);
            await _context.SaveChangesAsync();
        }

        public async Task<Attachments?> GetAttachmentByID(int attachmentID)
        {
            return await _context.Attachments.Where(x => x.AttachmentID == attachmentID).FirstOrDefaultAsync();
        }

        public async Task<bool> Delete(Attachments attachmentData)
        {
            _context.Attachments.Remove(attachmentData);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
