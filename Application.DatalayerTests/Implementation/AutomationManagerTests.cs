using Microsoft.VisualStudio.TestTools.UnitTesting;
using Application.Manager.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Utility.Translators;
using Application.DTO.Automation;
using Application.Utility.Logging;
using MongoRepository;
using Application.DTO.Conversion;
using Application.Tests.Helper;
using System.IO;

namespace Application.Manager.Implementation.Tests
{
    [TestClass()]
    public class AutomationManagerTests
    {
        AutomationManager _automationManager { get; set; }
        public AutomationManagerTests()
        {
            IEntityTranslatorService translatorService = new EntityTranslatorService();

            translatorService.RegisterEntityTranslator(new AutomationTranslator());

            _automationManager = new AutomationManager(new MongoRepository<AutomationSnapshot>(), translatorService, new CrucialLogger());

        }

        public AutomationSnapshot BuildSampleData()
        {
            return new AutomationSnapshot()
            {
                CreatedBy = "ekriesw",
                CreatedOn = DateTime.UtcNow,
                IsActive = true,
                isLatestVersion = true,
                ModifiedBy = "ekriesw",
                ModifiedOn = DateTime.UtcNow,
                name = "Test Automation",
                runbookContent = "",
                runbookException = "",
                summary = "Summary",
                title = "Test Title"
            };
        }

        [TestMethod()]
        public void AutomationManagerTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetbyIdTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetSnapshotbyIdTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void AddTest()
        {
            
            AutomationSnapshot input = this.BuildSampleData();
            var Id = _automationManager.Add(input);
            AutomationSnapshot result = _automationManager.GetSnapshotbyId(Id);
            Assert.IsNotNull(result);
            input.Id = result.Id;
            input.CreatedOn = result.CreatedOn;
            AssertHelper.HasEqualFieldValues<AutomationSnapshot>(input, result);
        }

        [TestMethod()]
        public void AddTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void UpdateTest()
        {
            AutomationSnapshot input = _automationManager.GetSnapshotbyId("590a882f9a8c492558bc73ca");
            string dir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            using (StreamReader reader = new StreamReader(@"C:\Eswar\Projects\Dream\Application.DatalayerTests\SampleData\Runbook-xml.txt"))
            {
                input.runbookContent = reader.ReadToEnd();
            }
            var result = _automationManager.Update(input);

            AssertHelper.HasEqualFieldValues<AutomationSnapshot>(input, result);
        }

        [TestMethod()]
        public void UpdateTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetSnapshotsTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SaveTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SaveTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void DeleteTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void DeleteTest1()
        {
            Assert.Fail();
        }
    }
}