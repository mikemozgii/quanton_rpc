using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TONBRAINS.TONOPS.Core.DAL;
using TONBRAINS.TONOPS.Core.Models;
using TONBRAINS.TONOPS.Core.Services;

namespace TONBRAINS.QUANTON.Core.Services
{
    public class UserSvc
    {

        public UserDbSvc _UserDbSvc { get; set; }
        public QuantchainSvc _QuantchainSvc { get; set; }

        public UserSvc()
        {
            _UserDbSvc = new UserDbSvc();
            _QuantchainSvc = new QuantchainSvc();
        }

        public async Task<bool> InitQuantonUserBaseAccount(string userId)
        {
            var r = _QuantchainSvc.CreateQuantonUserBaseAccount(userId);

            if (r != null)
            {
                var user = _UserDbSvc.GetById(userId);
                user.SmartAccountId = r.Id;
                return await new UserDbSvc().UpdateAsync(user);
            }

            return false;
        }

        public async Task<bool> InitQuantonUserBaseAccountByMnemonicPhrase(string userId, string mnemonicPhrase)
        {

            var r = _QuantchainSvc.CreateQuantonUserBaseAccountByMnemonicPhrase(userId, mnemonicPhrase);

            if (r != null)
            {
                var user = _UserDbSvc.GetById(userId);
                user.SmartAccountId = r.Id;
                return await new UserDbSvc().UpdateAsync(user);
            }

            return false;
        }

        public async Task<bool> InitQuantonUserBaseAccountBySecretAndPublicKey(string userId, string secretKey, string publicKey)
        {

            var r = _QuantchainSvc.CreateQuantonUserBaseAccountBySecretAndPublicKey(userId, secretKey, publicKey);

            if (r != null)
            {
                var user = _UserDbSvc.GetById(userId);
                user.SmartAccountId = r.Id;
                return await new UserDbSvc().UpdateAsync(user);
            }

            return false;
        }


        public async Task<bool> SendAmountFromGiver(string userId, long amount)
        {
            var user = _UserDbSvc.GetById(userId);
            var r = _QuantchainSvc.SendAmountFromGiver(user.SmartAccountId, amount);
            return await Task.FromResult(r);
        }


        public SmartAccountMdl GetQuantonUserBaseAccountState(string userId)
        {
            var user = _UserDbSvc.GetById(userId);
            var rs = _QuantchainSvc.GetQuantonUserBaseAccountState(user.SmartAccountId);
            if (rs.Any())
            {
                return rs.First().Value;
            }
            return null;
        }


        public TransferDeaction InitTokenTransfer(string userId, long amount, string email, string phone)
        {
            var user = _UserDbSvc.GetById(userId);
            var r = _QuantchainSvc.InitTokenTransfer(user.SmartAccountId, amount, email, phone);
            return r;
        }


        public bool ChangeTokenTransferStatus(string userId, string deactionId, string status)
        {
            var user = _UserDbSvc.GetById(userId);

            return status switch
            {
                "WaitingForAuthToken" => _QuantchainSvc.ResumeTokenTransfer(user.SmartAccountId, deactionId),
                "Paused" => _QuantchainSvc.PauseTokenTransfer(user.SmartAccountId, deactionId),
                "Stop" => _QuantchainSvc.DeleteTokenTransfer(user.SmartAccountId, deactionId),
                _ => false,
            };
        }

        public TransferDeaction CompleteTokenTransfer(string userId, string authToken)
        {
            var user = _UserDbSvc.GetById(userId);
            return _QuantchainSvc.CompleteTokenTransfer(user.SmartAccountId, authToken);
        }


        public IEnumerable<TransferDeaction> GetTokenTransferStates(string userId)
        {
            var user = _UserDbSvc.GetById(userId);
            return _QuantchainSvc.GetTokenTransferStates(user.SmartAccountId);
        }


        public TransferDeaction InitPayment(string userId, string paymentAmount)
        {
            long amount = 0;

            switch (paymentAmount)
            {
                case "9.98": amount = 100; break;
                case "99.98": amount = 1000; break;
                case "999.98": amount = 10000; break;
            }

            var user = _UserDbSvc.GetById(userId);
            return _QuantchainSvc.InitPayment(user.SmartAccountId, amount);
        }

        public string GetUserBaseAccountByMnemonicPhrase(string phrase)
        {
            return _QuantchainSvc.GetUserBaseAccountByMnemonicPhrase(phrase);
        }
        public string GetUserBaseAccountBySecretAndPublicKey(string secretKey, string publicKey)
        {
            return _QuantchainSvc.GetUserBaseAccountBySecretAndPublicKey(secretKey, publicKey);
        }

        //public string signInUser()
        //{ 


        //}



    }
}
