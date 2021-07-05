using NUnit.Framework;
using TONBRAINS.QUANTON.Core.Services;

namespace TONBRAINS.QUANTON.UnitTest.Core
{
    public class UserSvcUnitTest
    {

        public UserSvc UserSvc { get; set; }

        [SetUp]
        public void Setup()
        {
            UserSvc = new UserSvc();
        }

        [Test]
        public void Test1()
        {
            //var BraintreeServerSvc = new BraintreeServerSvc();
            //BraintreeServerSvc.SubmitTransaction();
            var r = UserSvc.InitQuantonUserBaseAccount("116751397340319332707");
            Assert.Pass();
        }
    }
}