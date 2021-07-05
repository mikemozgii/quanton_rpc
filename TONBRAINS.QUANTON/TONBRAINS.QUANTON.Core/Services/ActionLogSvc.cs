using Grpc.Core;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TONBRAINS.QUANTON.Core.DAL;
using TONBRAINS.QUANTON.Core.Handlers;
using TONBRAINS.QUANTON.Core.Helpers;
using TONBRAINS.QUANTON.Core.Interfaces;

namespace TONBRAINS.QUANTON.Core.Services
{
    public class ActionLogSvc: IActionLogSvc
    {
        private QuantonDbContext _context { get; set; }

        public ActionLogSvc()
        {
            _context = new QuantonDbContext(GlobalAppConfHdlr.GetQunatonDbContextOption());
        }

        public async Task SaveActionLog(
            ServerCallContext context,
            string action,
            string actionId,
            string userId)
        {
            var actionLog = new ActionLog()
            {
                Id = IdGenerator.Generate(),
                Action = action,
                ActionId = actionId,
                UserId = userId,
                IpAddress = context.GetHttpContext().Connection.RemoteIpAddress.ToString() ?? "",
                CreationDate = DateTime.UtcNow
            };

            var properties = actionLog.GetType().GetProperties().Where(p => Attribute.IsDefined(p, typeof(DisplayAttribute)));
            foreach (var property in properties)
            {
                var fieldName = (property.GetCustomAttributes(typeof(DisplayAttribute), false)[0] as DisplayAttribute).Name;
                var value = GetValueFromMetadata(context.RequestHeaders, fieldName);

                if (value != null) property.SetValue(actionLog, value);
            }

            await _context.ActionLogs.AddAsync(actionLog);
            await _context.SaveChangesAsync();
        }

        private string GetValueFromMetadata(Metadata headers, string name)
        {
            var metadataEntry = headers.FirstOrDefault(m => string.Equals(m.Key, name, StringComparison.Ordinal));
            if (metadataEntry == null || metadataEntry.Value == null) return null;

            return metadataEntry.Value;
        }
    }
}
