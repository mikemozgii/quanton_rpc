using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TONBRAINS.QUANTON.Core.DAL;
using TONBRAINS.QUANTON.Core.Handlers;
using TONBRAINS.TONOPS.Core.DAL;
using TONBRAINS.TONOPS.Core.Models;
using TONBRAINS.TONOPS.WebApp.WebApp.Helpers;

namespace TONBRAINS.QUANTON.Core.Services
{
    public class UserDbSvc
    {
        private QuantonDbContext _context { get; set; }

        public UserDbSvc()
        {
            _context = new QuantonDbContext(GlobalAppConfHdlr.GetQunatonDbContextOption());
        }

        public bool Add(User e)
        {
             _context.Users.Add(e);
             _context.SaveChanges();
            return true;
        }

        public async Task<bool> AddAsync(User e)
        {
            _context.Users.Add(e);
            return await _context.SaveChangesAsync() > 1;
        }

        public IEnumerable<User> GetByIds(params string[] ids)
        {
            return _context.Users.Where(q => ids.Contains(q.Id)).ToList();
        }

        public User GetById(string id)
        {
            return GetByIds(new string[]{ id }).FirstOrDefault();
        }

        public User GetByExternalId(string externalId)
        {
            return _context.Users.FirstOrDefault(q=>q.ExternalId == externalId);
        }

        public User GetBySmartAccountId(string saId)
        {
            return _context.Users.FirstOrDefault(q => q.SmartAccountId == saId);
        }

        public bool Update(params User[] entities)
        {
            _context.Users.UpdateRange(entities);
            return _context.SaveChanges() > 0;
        }

        public async Task<bool> UpdateAsync(params User[] entities)
        {
            _context.Users.UpdateRange(entities);
            return await _context.SaveChangesAsync() > 0;
        }


        public bool Delete(params User[] entities)
        {
            _context.Users.RemoveRange(entities);
            return _context.SaveChanges() > 0;
        }

        public bool DeleteByIds(params string[] ids)
        {
            var es = GetByIds(ids);
            _context.Users.RemoveRange(es);
            return _context.SaveChanges() > 0;
        }

    }
}
