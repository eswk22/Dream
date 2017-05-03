using Microsoft.VisualStudio.TestTools.UnitTesting;
using Application.Manager.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Utility.Translators;
using Application.Repository;
using Application.Utility.Logging;
using Application.DTO.Conversion;
using Application.DTO.Gateway;
using Application.Tests.Helper;
using MongoRepository;
using Application.Snapshot;

namespace Application.Manager.Implementation.Tests
{
    [TestClass()]
    public class GatewayManagerTests
    {

        GatewayManager _gatewayManager { get; set; }

        public GatewayManagerTests()
        {
            IEntityTranslatorService translatorService = new EntityTranslatorService();

            translatorService.RegisterEntityTranslator(new GatewayTranslator());

            _gatewayManager = new GatewayManager(new MongoRepository<GatewaySnapshot>(), translatorService, new CrucialLogger());

        }

        private GatewayDTO BuildSampleData()
        {
            GatewayDTO dto = new GatewayDTO()
            {
                AutomationId = new Guid().ToString(),
                CreatedBy = "ekriesw",
                CreatedOn = DateTime.UtcNow,
                EventId = "",
                GatewayName = "mssql",
                Type = "mssql",
                Interval = 300,
                IsActive = true,
                LastRunTime = DateTime.UtcNow,
                Name = "TestGateway",
                Order = 1,
                Query = "Select TOP 1 * from LEC_Access.OrderData ",
                Script = "",
                Status = "New",
                UpdatedBy = "",
                UpdatedOn = DateTime.UtcNow
            };
            return dto;
        }

        [TestMethod()]
        public void GatewayManagerTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetbyIdTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void AddTest()
        {
            GatewayDTO input = this.BuildSampleData();
            var Id = _gatewayManager.Add(input);
            GatewayDTO result = _gatewayManager.GetbyId(Id);
            Assert.IsNotNull(result);
            input.Id = result.Id;
            input.CreatedOn = result.CreatedOn;
            AssertHelper.HasEqualFieldValues<GatewayDTO>(input, result);
        }

        [TestMethod()]
        public void UpdateTest()
        {
            GatewayDTO input = _gatewayManager.GetbyId("5908f97c9a8c4b24bc6a95e8");
            input.Query = "Select Top 1 * from LEC_Access_Dev.dbo.OrderData";
            var result = _gatewayManager.Update(input);
         
           AssertHelper.HasEqualFieldValues<GatewayDTO>(input, result);
        }

        [TestMethod()]
        public void GetTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetbyStatusTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetbyStatusTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SaveTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void DeleteTest()
        {
            Assert.Fail();
        }
    }
}