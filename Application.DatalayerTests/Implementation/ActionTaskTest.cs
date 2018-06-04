using Microsoft.VisualStudio.TestTools.UnitTesting;
using Application.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Messages;
using Application.Utility.Translators;
using Application.Utility.Logging;
using Application.Manager.Conversion;
using Application.Tests.Helper;
using Application.Manager.Implementation;
using Application.DTO.Conversion;
using Application.DTO;

namespace Application.Manager.Tests
{
    [TestClass()]
    public class ActionTaskTest
    {
        IActionTaskBusinessManager _actionTaskManager { get; set; }

        public ActionTaskTest()
        {
            IEntityTranslatorService translatorService = new EntityTranslatorService();
         
            translatorService.RegisterEntityTranslator(new CompilationResultTranslator());
            translatorService.RegisterEntityTranslator(new CompilationArgumentTranslator());
            translatorService.RegisterEntityTranslator(new ActionTaskTranslator());
            translatorService.RegisterEntityTranslator(new ActionTasklistTranslator());

         //   _actionTaskManager = new ActionTaskManager(new ActionTaskRepository(), translatorService, new CrucialLogger());

        }


        private ActionTaskDTO ActionTaskData()
        {
            return new ActionTaskDTO()
            {
                LocalCode = "",
                Type = "Remote",
                LocalLanguage = "Csharp",
                CreatedBy = "ekriesw",
                CreatedOn = DateTime.UtcNow,
                Description = "",
            //    Inputs = null,
                IsActive = true,
                FolderPath = "",
             //   MockInputs = null,
                Namespace = "",
                Name = "ActionTask1",
            //    Outputs = null,
                RemoteCode = "",
            //    Results = null,
                Summary = "",
                Timeout = 300,
                ModifiedBy = "",
                ModifiedOn = DateTime.UtcNow
            };
        }


        [TestMethod()]
        public void GetbyIdTest()
        {
            ActionTaskDTO input = this.ActionTaskData();
            ActionTaskDTO savedData = _actionTaskManager.Save(input);
            Assert.IsNotNull(savedData);
            input.ActionTaskId = savedData.ActionTaskId;
            

            string actionId = savedData.ActionTaskId;
            ActionTaskDTO result = _actionTaskManager.GetbyId(actionId);
            Assert.IsNotNull(result);
            input.CreatedOn = result.CreatedOn;
            input.ModifiedOn = result.ModifiedOn;
            AssertHelper.HasEqualFieldValues<ActionTaskDTO>(input, result);
        }

        [TestMethod()]
        public void AddTest()
        {

            ActionTaskDTO input = this.ActionTaskData();
            var actionId = _actionTaskManager.Add(input);
            ActionTaskDTO result = _actionTaskManager.GetbyId(actionId);
            Assert.IsNotNull(result);
            input.ActionTaskId = result.ActionTaskId;
            input.CreatedOn = result.CreatedOn;
            input.ModifiedOn = result.ModifiedOn;
            AssertHelper.HasEqualFieldValues<ActionTaskDTO>(input, result);


        }

        [TestMethod()]
        public void UpdateTest()
        {
            ActionTaskDTO input = this.ActionTaskData();
            ActionTaskDTO savedData = _actionTaskManager.Save(input);
            Assert.IsNotNull(savedData);
            input.ActionTaskId = savedData.ActionTaskId;
            input.CreatedOn = savedData.CreatedOn;
            input.ModifiedOn = savedData.ModifiedOn;
            input.LocalCode = "Test";
            ActionTaskDTO result = _actionTaskManager.Update(input);
            Assert.IsNotNull(result);
            input.ModifiedOn = result.ModifiedOn;
            AssertHelper.HasEqualFieldValues<ActionTaskDTO>(input, result);

        }

        [TestMethod()]
        public void GetTest()
        {
            IEnumerable<ActionTaskDTO> result = _actionTaskManager.Get();

            Assert.IsNotNull(result);

            if (result.Count() == 0)
            {
                Assert.Fail("Count Zero");
            }
        }

        [TestMethod()]
        public void SaveTest()
        {
            ActionTaskDTO input = this.ActionTaskData();
            var result = _actionTaskManager.Save(input);
            Assert.IsNotNull(result);
            input.ActionTaskId = result.ActionTaskId;
            input.CreatedOn = result.CreatedOn;
            input.ModifiedOn = result.ModifiedOn;
            AssertHelper.HasEqualFieldValues<ActionTaskDTO>(input, result);
        }

        [TestMethod()]
        public void DeleteTest()
        {
            ActionTaskDTO input = this.ActionTaskData();
            ActionTaskDTO savedData = _actionTaskManager.Save(input);
            Assert.IsNotNull(savedData);
            input.ActionTaskId = savedData.ActionTaskId;
            input.CreatedOn = savedData.CreatedOn;
            input.ModifiedOn = savedData.ModifiedOn;
            input.IsActive = false;
            ActionTaskDTO result = _actionTaskManager.Delete(input);
            Assert.IsFalse(result.IsActive);

        }
    }
}