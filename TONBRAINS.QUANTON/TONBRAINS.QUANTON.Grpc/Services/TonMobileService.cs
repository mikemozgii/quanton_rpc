using Grpc.Core;
using Newtonsoft.Json;
using SqlKata;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TONBRAINS.QUANTON.Core.DAL;
using TONBRAINS.QUANTON.Core.Enums;
using TONBRAINS.QUANTON.Core.Handlers;
using TONBRAINS.QUANTON.Core.Helpers;
using TONBRAINS.QUANTON.Core.Interfaces;
using TONBRAINS.QUANTON.Core.Services;
using TONBRAINS.QUANTON.Grpc.Helpers;
using TONBRAINS.QUANTON.Grpc.Models;

namespace TONBRAINS.QUANTON.Grpc
{
    public class TonMobileService : TonMobile.TonMobileBase
    {
        private readonly IActionLogSvc _actionLogSvc;
        private readonly IAccountSvc _accountSvc;

        public TonMobileService(IActionLogSvc actionLogSvc, IAccountSvc accountSvc)
        {
            _actionLogSvc = actionLogSvc ?? throw new ArgumentNullException(nameof(actionLogSvc));
            _accountSvc = accountSvc ?? throw new ArgumentNullException(nameof(accountSvc));
        }

        private static User GetSession(ServerCallContext context)
        {
            //WORKAROUND: while not working session provider
            if (!context.UserState.ContainsKey("session")) return new User { Id = "-MR1BLTt0BHIIa03i5GV" };

            return JsonConvert.DeserializeObject<User>(context.UserState["session"].ToString());
        }

        public override async Task<SignInReply> PublicSecretKeySignIn(PublicSecretKeySignInRequest request, ServerCallContext context)
        {
            if (string.IsNullOrWhiteSpace(request.PublicKey) || request.PublicKey.Length != 64 || string.IsNullOrWhiteSpace(request.SecretKey) || request.SecretKey.Length != 64)
            {
                return new SignInReply { Token = null };
            }

            var session = GetSession(context);
            var saId = new UserSvc().GetUserBaseAccountBySecretAndPublicKey(request.SecretKey.Replace("\n", ""), request.PublicKey.Replace("\n", ""));
            if (saId != null)
            {
                var user = new UserDbSvc().GetBySmartAccountId(saId);
                if (user != null)
                {
                    await _actionLogSvc.SaveActionLog(context, ActionLogs.signIn, user.Id, session.Id);
                    return new SignInReply { Token = JwtService.CreateToken(user), Name = user.Name, Email = user.ExternalData };
                }
            }
   

            var newUser = new User
            {
                Id = IdGenerator.Generate(),
                Name = "QUANTON USER",
                ExternalData = $"{IdGenerator.Generate()}@quanton",
                ExternalId = "internal",
                CreationDate = DateTime.UtcNow
            };

            await _actionLogSvc.SaveActionLog(context, ActionLogs.registerNewUser, newUser.SmartAccountId, newUser.Id);
            var r = new UserDbSvc().Add(newUser);
            await new UserSvc().InitQuantonUserBaseAccountBySecretAndPublicKey(newUser.Id, request.SecretKey, request.PublicKey);
            await new UserSvc().SendAmountFromGiver(newUser.Id, 10000);

            if (r)
            {
                var user = new UserDbSvc().GetById(newUser.Id);
                await _actionLogSvc.SaveActionLog(context, ActionLogs.signIn, newUser.Id, newUser.Id);
                return new SignInReply { Token = JwtService.CreateToken(user), Name = user.Name, Email = user.ExternalData };
            }
            return new SignInReply { Token = null };
        }

        public override async Task<SignInReply> MnemonicPhraseSignIn(MnemonicPhraseSignInRequest request, ServerCallContext context)
        {
            if (string.IsNullOrWhiteSpace(request.Phrase) || request.Phrase.Split(" ").Count() != 12)
            {

                return new SignInReply { Token = null };
            }

            var session = GetSession(context);
            var saId = new UserSvc().GetUserBaseAccountByMnemonicPhrase(request.Phrase);
            if (saId != null)
            {
                var user = new UserDbSvc().GetBySmartAccountId(saId);
                if (user != null)
                {
                    await _actionLogSvc.SaveActionLog(context, ActionLogs.signIn, user.Id, session.Id);
                    return new SignInReply { Token = JwtService.CreateToken(user), Name = user.Name, Email = user.ExternalData };
                }
            }


            var newUser = new User
            {
                Id = IdGenerator.Generate(),
                Name = "QUANTON USER",
                ExternalData = $"{IdGenerator.Generate()}@quanton",
                ExternalId = "internal",
                CreationDate = DateTime.UtcNow
            };

            await _actionLogSvc.SaveActionLog(context, ActionLogs.registerNewUser, newUser.SmartAccountId, newUser.Id);
            var r = new UserDbSvc().Add(newUser);
            await new UserSvc().InitQuantonUserBaseAccountByMnemonicPhrase(newUser.Id, request.Phrase);
            await new UserSvc().SendAmountFromGiver(newUser.Id, 10000);

            if (r)
            {
                var user = new UserDbSvc().GetById(newUser.Id);
                await _actionLogSvc.SaveActionLog(context, ActionLogs.signIn, newUser.Id, newUser.Id);
                return new SignInReply { Token = JwtService.CreateToken(user), Name = user.Name, Email = user.ExternalData };
            }

            return new SignInReply { Token = null };
        }

        public override async Task<SignInReply> SignIn(SignInRequest request, ServerCallContext context)
        {
            var session = GetSession(context);
            var user = new UserDbSvc().GetByExternalId(request.Id);

            if (user != null)
            {

                if (!string.IsNullOrWhiteSpace(user.SmartAccountId))
                {
                    await _actionLogSvc.SaveActionLog(context, ActionLogs.signIn, user.Id, session.Id);
                    return new SignInReply { Token = JwtService.CreateToken(user), Name = user.Name, Email = user.ExternalData };
                }
                else
                {
                    await new UserSvc().InitQuantonUserBaseAccount(user.Id);
                    await new UserSvc().SendAmountFromGiver(user.Id, 10000);

                    user = new UserDbSvc().GetByExternalId(user.ExternalId);
                    await _actionLogSvc.SaveActionLog(context, ActionLogs.signIn, user.Id, user.Id);
                    return new SignInReply { Token = JwtService.CreateToken(user), Name = user.Name, Email = user.ExternalData };
                }


       
            }


            var newUser = new User
            {
                Id = IdGenerator.Generate(),
                Name = request.UserName,
                ExternalData = request.Data,
                ExternalId = request.Id,
                CreationDate = DateTime.UtcNow
            };

            
             await _actionLogSvc.SaveActionLog(context, ActionLogs.registerNewUser, newUser.SmartAccountId, newUser.Id);
             var r = new UserDbSvc().Add(newUser);
             await  new UserSvc().InitQuantonUserBaseAccount(newUser.Id);          
             await new UserSvc().SendAmountFromGiver(newUser.Id, 10000);

            if (r)
            {
                user = new UserDbSvc().GetByExternalId(newUser.ExternalId);
                await _actionLogSvc.SaveActionLog(context, ActionLogs.signIn, newUser.Id, newUser.Id);
                return new SignInReply { Token = JwtService.CreateToken(user), Name = user.Name, Email = user.ExternalData };
            }
         

            //TODO: delete!
            //await _accountSvc.TransferBalance();



            return new SignInReply { Token = null };
        }

        public override async Task<TestReply> Test(TestRequest request, ServerCallContext context)
        {
            var session = GetSession(context);
            var user = await KataHelpers.ExecuteQueryFirst<User>(new Query("users").Where("id", session.Id));
            return new TestReply { };
        }

        public override async Task<GetSmartAccountReply> GetSmartAccount(GetSmartAccountRequest request, ServerCallContext context)
        {
            var session = GetSession(context);
            var accountState =  new UserSvc().GetQuantonUserBaseAccountState(session.Id);

            return await Task.FromResult(new GetSmartAccountReply
            {
                Address = accountState.Address,
                MnemonicPhrase = accountState.Phrase == null ? "" : accountState.Phrase,
                PublicKey = accountState.PublicKey,
                SecretKey = accountState.SecretKey,
            });
        }

        public override async Task<GetBalanceReply> GetBalance(GetBalanceRequest request, ServerCallContext context)
        {
            var session = GetSession(context);
            var accountState = new UserSvc().GetQuantonUserBaseAccountState(session.Id);
            return await Task.FromResult(new GetBalanceReply
            {
                Balance = accountState.Balance - 1
            });
        }

        public override async Task<PaymentReply> Payment(PaymentRequest request, ServerCallContext context)
        {
            var session = GetSession(context);
            var result = await new BraintreeServerSvc().SubmitTransaction(request.Amount, request.Nonce, request.ClientData);
            if (!result) return new PaymentReply { Success = false };

            var transferLog = new UserSvc().InitPayment(session.Id, request.Amount);
            if (transferLog == null) return new PaymentReply { Success = false };

            await _actionLogSvc.SaveActionLog(context, ActionLogs.initPayment, transferLog.Id, session.Id);

            return new PaymentReply { Success = true };
        }

        public override async Task<TransferBalanceReply> TransferBalance(TransferBalanceRequest request, ServerCallContext context)
        {
            var session = GetSession(context);

            var transferLog = new UserSvc().InitTokenTransfer(session.Id, request.Amount, request.Email, request.Phone);
            await _actionLogSvc.SaveActionLog(context, ActionLogs.initTransferDeaction, transferLog.Id, session.Id);

            return new TransferBalanceReply { Token = transferLog.AuthToken };
        }

        public override async Task<TransferLogsReply> TransferLogs(TransferLogsRequest request, ServerCallContext context)
        {
            var reply = new TransferLogsReply();
            var session = GetSession(context);

            var transferLogs = await Task.FromResult(
                new UserSvc().GetTokenTransferStates(session.Id)
            );
            if (!transferLogs.Any()) return reply;

            var date = transferLogs.First().Date.ToDateTimeValue().Date;
            foreach (var item in transferLogs)
            {
                var itemDate = item.Date.ToDateTimeValue();

                if (date != itemDate.Date) reply.TransferLogs.Add(new TransferLogs { Date = date.ToString("MMM d, yyyy") });
                date = itemDate.Date;

                reply.TransferLogs.Add(new TransferLogs
                {
                    Id = item.Id,
                    Time = itemDate.ToString("HH:mm"),
                    Amount = item.Amount,
                    Status = item.Status.ToString(),
                    Email = item.Email.ToGrpcValue(),
                    Phone = item.Phone.ToGrpcValue(),
                    IsPayment = item.IsPayment,
                    IsRecipient = item.IsRecipient
                });
            }
            reply.TransferLogs.Add(new TransferLogs { Date = date.ToString("MMM d, yyyy") });

            return reply;
        }

        public override async Task<AnnouncementsReply> GetAnnouncements(AnnouncementsRequest request, ServerCallContext context)
        {
            var reply = new AnnouncementsReply();
            var query = new Query("announcements")
                .Select("id", "title", "data", "url")
                .SelectRaw("extract(epoch from date) as date")
                .OrderByDesc("date");
            var data = await KataHelpers.ExecuteQuery<AnnouncementItem>(query);

            if(data.Any()) reply.Items.AddRange(data);

            return reply;
        }

        public override async Task<SetTransferStatusReply> SetTransferStatus(SetTransferStatusRequest request, ServerCallContext context)
        {
            var session = GetSession(context);

            var result = new UserSvc().ChangeTokenTransferStatus(session.Id, request.Id, request.Status);
            if (!result) return new SetTransferStatusReply();

            var status = "";
            switch (request.Status)
            {
                case "WaitingForAuthToken": status = ActionLogs.resumeTransferDeaction; break;
                case "Paused": status = ActionLogs.pauseTransferDeaction; break;
                case "Stop": status = ActionLogs.deleteTransferDeaction; break;
            }

            await _actionLogSvc.SaveActionLog(context, status, request.Id, session.Id);

            return new SetTransferStatusReply { Status = request.Status };
        }

        public override async Task<ReceiveTokenReply> ReceiveToken(ReceiveTokenRequest request, ServerCallContext context)
        {
            var session = GetSession(context);

            var result = new UserSvc().CompleteTokenTransfer(session.Id, request.Token);
            if (result != null)
            {
                await _actionLogSvc.SaveActionLog(context, ActionLogs.completeTransferDeaction, result.Id, session.Id);
            }
            else
            {
                // need to know who is dossing or hacing system
                await _actionLogSvc.SaveActionLog(context, ActionLogs.completeTransferDeaction, request.Token, session.Id);
            }

            return new ReceiveTokenReply { Ok = result != null };
        }

        public override async Task<SaveIssueReply> SaveIssue(SaveIssueRequest request, ServerCallContext context)
        {
            var session = GetSession(context);

            var filesBuilder = new StringBuilder("{");
            if(request.Files.Any())
            {
                filesBuilder.Append(string.Join(",", request.Files.Select(i => $"\"{i.Key}\": \"{i.Value}\"")));
            }
            filesBuilder.Append("}");

            var model = new IssueModel
            {
                Id = IdGenerator.Generate(),
                Date = DateTime.UtcNow,
                Description = request.Description,
                Title = request.Title,
                UserId = session.Id,
                Files = filesBuilder.ToString()
            };

            await KataHelpers.ExecuteInsertOrUpdate(model, new Query("issues"));

            var reply = new SaveIssueReply { Message = $"Issue {model.Id} was created!" };

            return reply;
        }

        public override async Task<ExistContactsReply> ExistContacts(ExistContactsRequest request, ServerCallContext context)
        {
            var reply = new ExistContactsReply();

            var existEmails = (await KataHelpers.ExecuteQuery<ExistEmailModel>(new Query("exist_emails"))).Select(i => i.Email);
            var existPhones = (await KataHelpers.ExecuteQuery<ExistPhoneModel>(new Query("exist_phones"))).Select(i => i.Phone);

            var emails = request.Emails.Where(i => existEmails.Contains(i));
            var phones = request.Phones.Where(i => existPhones.Contains(i));

            reply.Emails.AddRange(emails);
            reply.Phones.AddRange(phones);

            return reply;
        }

        public override async Task<AppVersionReply> AppVersion(AppVersionRequest request, ServerCallContext context)
        {

     
            return await Task.FromResult(new AppVersionReply
            {
                Version = GlobalAppConfHdlr.AppVersion
            }); ;
        }
    }
}
