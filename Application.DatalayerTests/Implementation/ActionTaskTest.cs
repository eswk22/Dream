using Microsoft.VisualStudio.TestTools.UnitTesting;
using Application.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Messages;
using Application.Repository;
using Application.Utility.Translators;
using Application.Utility.Logging;
using Application.Manager.Conversion;
using Application.Tests.Helper;

namespace Application.Manager.Tests
{
    [TestClass()]
    public class ActionTaskTest
    {
        ActionTaskManager _actionTaskManager { get; set; }

        public ActionTaskTest()
        {
            IEntityTranslatorService translatorService = new EntityTranslatorService();
         
            translatorService.RegisterEntityTranslator(new CompilationResultTranslator());
            translatorService.RegisterEntityTranslator(new CompilationArgumentTranslator());
            translatorService.RegisterEntityTranslator(new ActionTaskTranslator());
            translatorService.RegisterEntityTranslator(new ActionTasklistTranslator());

            _actionTaskManager = new ActionTaskManager(new ActionTaskRepository(), translatorService, new CrucialLogger());

        }


        private ActionTaskMessage ActionTaskData()
        {
            return new ActionTaskMessage()
            {
                AccessCode = "",
                Actiontype = "Remote",
                Codelanguage = "Csharp",
                CreatedBy = "ekriesw",
                CreatedOn = DateTime.UtcNow,
                Description = "",
                Inputs = null,
                IsActive = true,
                menupath = "",
                MockInputs = null,
                module = "",
                Name = "ActionTask1",
                Outputs = null,
                RemoteCode = "",
                Results = null,
                Summary = "",
                TimeOut = 300,
                UpdatedBy = "",
                UpdatedOn = DateTime.UtcNow
            };
        }


        [TestMethod()]
        public void GetbyIdTest()
        {
            ActionTaskMessage input = this.ActionTaskData();
            ActionTaskMessage savedData = _actionTaskManager.Save(input);
            Assert.IsNotNull(savedData);
            input.ActionId = savedData.ActionId;
            

            string actionId = savedData.ActionId;
            ActionTaskMessage result = _actionTaskManager.GetbyId(actionId);
            Assert.IsNotNull(result);
            input.CreatedOn = result.CreatedOn;
            input.UpdatedOn = result.UpdatedOn;
            AssertHelper.HasEqualFieldValues<ActionTaskMessage>(input, result);
        }

        [TestMethod()]
        public void AddTest()
        {

            ActionTaskMessage input = this.ActionTaskData();
            var actionId = _actionTaskManager.Add(input);
            ActionTaskMessage result = _actionTaskManager.GetbyId(actionId);
            Assert.IsNotNull(result);
            input.ActionId = result.ActionId;
            input.CreatedOn = result.CreatedOn;
            input.UpdatedOn = result.UpdatedOn;
            AssertHelper.HasEqualFieldValues<ActionTaskMessage>(input, result);


        }

        [TestMethod()]
        public void UpdateTest()
        {
            ActionTaskMessage input = this.ActionTaskData();
            ActionTaskMessage savedData = _actionTaskManager.Save(input);
            Assert.IsNotNull(savedData);
            input.ActionId = savedData.ActionId;
            input.CreatedOn = savedData.CreatedOn;
            input.UpdatedOn = savedData.UpdatedOn;
            input.AccessCode = "Test";
            ActionTaskMessage result = _actionTaskManager.Update(input);
            Assert.IsNotNull(result);
            input.UpdatedOn = result.UpdatedOn;
            AssertHelper.HasEqualFieldValues<ActionTaskMessage>(input, result);

        }

        [TestMethod()]
        public void GetTest()
        {
            IEnumerable<ActionTasklist> result = _actionTaskManager.Get();

            Assert.IsNotNull(result);

            if (result.Count() == 0)
            {
                Assert.Fail("Count Zero");
            }
        }

        [TestMethod()]
        public void SaveTest()
        {
            ActionTaskMessage input = this.ActionTaskData();
            var result = _actionTaskManager.Save(input);
            Assert.IsNotNull(result);
            input.ActionId = result.ActionId;
            input.CreatedOn = result.CreatedOn;
            input.UpdatedOn = result.UpdatedOn;
            AssertHelper.HasEqualFieldValues<ActionTaskMessage>(input, result);
        }

        [TestMethod()]
        public void DeleteTest()
        {
            ActionTaskMessage input = this.ActionTaskData();
            ActionTaskMessage savedData = _actionTaskManager.Save(input);
            Assert.IsNotNull(savedData);
            input.ActionId = savedData.ActionId;
            input.CreatedOn = savedData.CreatedOn;
            input.UpdatedOn = savedData.UpdatedOn;
            input.IsActive = false;
            ActionTaskMessage result = _actionTaskManager.Delete(input);
            Assert.IsFalse(result.IsActive);

        }
    }
}