using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TONBRAINS.QUANTON.Core.DAL;
using TONBRAINS.QUANTON.Core.Handlers;
using TONBRAINS.QUANTON.Core.Helpers;
using TONBRAINS.QUANTON.Core.Interfaces;
using TONBRAINS.TONOPS.Core.Models;
using TONBRAINS.TONOPS.Core.Services;

namespace TONBRAINS.QUANTON.Core.Services
{
    public class AccountSvc : IAccountSvc
    {
        private QuantonDbContext _context { get; set; }

        public AccountSvc()
        {
            _context = new QuantonDbContext(GlobalAppConfHdlr.GetQunatonDbContextOption());
        }






       // public async Task<SmartAccountWithFirstKeyMdl> GetSmartAccount(string id) => await new SmartAccountDbSvc().GetSmartAccountWithFirstKey(id);

       // public async Task<long> GetBalance(string smartAccountId) => await new SmartAccountDbSvc().GetAccountNetworkBalance(smartAccountId);

        //public async Task<bool> CreatePaymentLog(string smartAccountId, string amount, string paymentLogId)
        //{
        //    try
        //    {
        //        long transferBalance;

        //        // do we need currency conversion?
        //        switch (amount)
        //        {
        //            case "9.98": transferBalance = 100; break;
        //            case "99.98": transferBalance = 1000; break;
        //            case "999.98": transferBalance = 10000; break;
        //            default: return false;
        //        }

        //        var transferLog = new TransferLog
        //        {
        //            Id = IdGenerator.Generate(),
        //            RecipientSmartAccountId = smartAccountId,
        //            CreationDate = DateTime.UtcNow,
        //            TransferBalance = transferBalance,
        //            PaymentLogId = paymentLogId
        //        };

        //        await _context.TransferLogs.AddAsync(transferLog);
        //        await _context.SaveChangesAsync();

        //        return true;
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e);
        //        return false;
        //    }
        //}

        //public async Task<TransferLog> CreateTransferLog(string smartAccountId, long transferBalance, string email, string phone)
        //{
        //    var transferLog = new TransferLog
        //    {
        //        Id = IdGenerator.Generate(),
        //        SmartAccountId = smartAccountId,
        //        CreationDate = DateTime.UtcNow,
        //        Token = GenerateToken(),
        //        TransferBalance = transferBalance,
        //        Email = email,
        //        Phone = phone,
        //    };

        //    await _context.TransferLogs.AddAsync(transferLog);
        //    await _context.SaveChangesAsync();

        //    return transferLog;
        //}

        //private string GenerateToken()
        //{
        //    return IdGenerator.Generate().ToString();
        //}

        //public async Task<List<TransferLogView>> GetTransferLogs(string smartAccountId)
        //    => await _context.ViewTransferLogs.Where(i => i.SmartAccountId == smartAccountId).ToListAsync();

        //public async Task<TransferLogView> SetTransferStatus(string id, string smartAccountId, string status)
        //{
        //    if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(smartAccountId)) return null;

        //    var item = await _context.TransferLogs.FirstOrDefaultAsync(i =>
        //        i.Id == id &&
        //        i.SmartAccountId == smartAccountId &&
        //        !i.IsCanceled &&
        //        string.IsNullOrEmpty(i.RecipientSmartAccountId)
        //    );
        //    if (item == null) return null;

        //    switch (status)
        //    {
        //        case "play": item.IsPaused = false; break;
        //        case "pause": item.IsPaused = true; break;
        //        case "stop": item.IsCanceled = true; break;
        //        default: return null;
        //    }

        //    _context.TransferLogs.Update(item);
        //    await _context.SaveChangesAsync();

        //    return await _context.ViewTransferLogs.FirstOrDefaultAsync(i => i.Id == id);
        //}

        //public async Task<TransferLog> ReceiveToken(string smartAccountId, string token)
        //{
        //    if (string.IsNullOrEmpty(smartAccountId) || string.IsNullOrEmpty(token)) return null;

        //    var item = await _context.TransferLogs.FirstOrDefaultAsync(i => i.Token == token && i.SmartAccountId != smartAccountId);
        //    if (item == null || item.IsPaused || item.IsCanceled) return null;

        //    item.RecipientSmartAccountId = smartAccountId;

        //    _context.TransferLogs.Update(item);
        //    await _context.SaveChangesAsync();

        //    return item;
        //}

        ////TODO: only for robot method!
        //public async Task TransferBalance()
        //{
        //    var items = await _context.TransferLogs
        //        .Where(i => !i.SendingDate.HasValue && !string.IsNullOrEmpty(i.RecipientSmartAccountId) && !i.IsPaused && !i.IsCanceled)
        //        .ToListAsync();
        //    if (!items.Any()) return;

        //    var smartAccountDbSvc = new SmartAccountDbSvc();
        //    foreach (var item in items)
        //    {
        //        var result = string.IsNullOrEmpty(item.SmartAccountId)
        //            ? await smartAccountDbSvc.ExecutePayment(item.RecipientSmartAccountId, item.TransferBalance)
        //            : await smartAccountDbSvc.ExecuteTransfer(item.SmartAccountId, item.RecipientSmartAccountId, item.TransferBalance);

        //        if (result) item.SendingDate = DateTime.UtcNow;
        //        else item.IsCanceled = true;
        //    }

        //    _context.TransferLogs.UpdateRange(items);
        //    await _context.SaveChangesAsync();
        //}


    }
}
