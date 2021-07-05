using Braintree;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TONBRAINS.QUANTON.Core.Services
{
    public class BraintreeServerSvc
    {
        //public BraintreeGateway gateway = new BraintreeGateway
        //{
        //    Environment = Braintree.Environment.SANDBOX,
        //    MerchantId = "sbm9sk2wvjqy6y8j",
        //    PublicKey = "3n9gmtm2t2km6p2k",
        //    PrivateKey = "17cee0a8f4a9d7524422ece2c23e0c1d"
        //};


        public BraintreeGateway GetGateway()
        {

            var gateway = new BraintreeGateway
            {
                Environment = Braintree.Environment.SANDBOX,
                MerchantId = "sbm9sk2wvjqy6y8j",
                PublicKey = "3n9gmtm2t2km6p2k",
                PrivateKey = "17cee0a8f4a9d7524422ece2c23e0c1d"
            };
            return gateway;
        }


        public async Task<bool>  SubmitTransaction(string amount, string nonce, string deviceData)
        {

            var request = new TransactionRequest
            {
                Amount = Convert.ToDecimal(amount),
                PaymentMethodNonce = nonce,
                DeviceData = deviceData,
                Options = new TransactionOptionsRequest
                {
                    SubmitForSettlement = true
                }
            };

            Result<Transaction> result = await GetGateway().Transaction.SaleAsync(request);

            if (result.IsSuccess())
            {
                Transaction transaction = result.Target;
                Debug.WriteLine("Success!: " + transaction.Id);
                return true;
            }
            else if (result.Transaction != null)
            {
                Transaction transaction = result.Transaction;
                Debug.WriteLine("Error processing transaction:");
                Debug.WriteLine("Status: " + transaction.Status);
                Debug.WriteLine("Code: " + transaction.ProcessorResponseCode);
                Debug.WriteLine("Text: " + transaction.ProcessorResponseText);
            }
            else
            {
                foreach (ValidationError error in result.Errors.DeepAll())
                {
                    Debug.WriteLine("Attribute: " + error.Attribute);
                    Debug.WriteLine("Code: " + error.Code);
                    Debug.WriteLine("Message: " + error.Message);
                }
            }

            return false;
        }

    }
}
