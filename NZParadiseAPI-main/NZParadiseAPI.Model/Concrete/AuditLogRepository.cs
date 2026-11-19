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
    public class AuditLogRepository
    {
        private readonly ApplicationDbContext _context;
        public AuditLogRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Save(AuditLogs log)
        {
            await _context.AuditLog.AddAsync(log);
            await _context.SaveChangesAsync();
        }
    }
}