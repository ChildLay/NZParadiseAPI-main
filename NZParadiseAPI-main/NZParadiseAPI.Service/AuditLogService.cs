
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
    public class AuditLogService
    {
        private readonly AuditLogRepository _repo;
        public AuditLogService(ApplicationDbContext context)
        {
            _repo = new AuditLogRepository(context);
        }

        public async Task WriteLog(int userID, string logDescription)
        {
            try
            {
                await _repo.Save(new AuditLogs
                {
                    Description = logDescription.Length > 500 ? logDescription[..500] : logDescription,
                    UserID = userID,
                    CreatedDate = DateTime.Now
                });
            }
            catch (Exception)
            {
                // do nothing for logging error for now
            }
        }
    }
}