﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Repository;
using Application.Messages;
using Application.Snapshot;
using Application.Utility.Translators;
using Application.Utility.Logging;
using EasyNetQ;
using System.Linq.Expressions;

namespace Application.Manager
{
    public class ActionTaskManager : IActionTaskManager
    {
        private readonly IActionTaskRepository _IActionTaskRepository;
        private readonly IEntityTranslatorService _translatorService;
        private readonly ILogger _logger;
     //   private readonly IBus _bus;

        public ActionTaskManager(IActionTaskRepository iActionTaskRepository,
            IEntityTranslatorService translatorService, ILogger logger)
        {
            _translatorService = translatorService;
            _IActionTaskRepository = iActionTaskRepository;
            _logger = logger;
  //          _bus = bus;
        }

        public ActionTaskMessage GetbyId(string Id)
        {
            ActionTaskMessage result = null;
            try
            {
                result = _translatorService.Translate<ActionTaskMessage>(_IActionTaskRepository.GetById(Id));
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public string Add(ActionTaskMessage actiontaskmessage)
        {
            string result = null;
            try
            {
                ActionTaskSnapshot snapshot = _translatorService.Translate<ActionTaskSnapshot>(actiontaskmessage);
                result = _IActionTaskRepository.Add(snapshot).Id;
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public ActionTaskMessage Update(ActionTaskMessage actiontaskmessage)
        {
            ActionTaskMessage result = null;
            try
            {
                ActionTaskSnapshot snapshot = _translatorService.Translate<ActionTaskSnapshot>(actiontaskmessage);
                result = _translatorService.Translate<ActionTaskMessage>(_IActionTaskRepository.Update(snapshot));
            }
            catch (Exception ex)
            {

            }
            return result;
        }


        public IEnumerable<ActionTasklist> Get()
        {
            IEnumerable<ActionTasklist> result = null;
            try
            {
                result = _IActionTaskRepository.GetAll().Select(m => _translatorService.Translate<ActionTasklist>(m)).AsEnumerable();
            }
            catch (Exception ex)
            {
            }

            return result;
        }


        public ActionTaskMessage Save(ActionTaskMessage actiontaskmessage)
        {
            ActionTaskMessage result = null;
            try
            {
                _logger.Info("Test message");
                ActionTaskSnapshot serviceDTO = _translatorService.Translate<ActionTaskSnapshot>(actiontaskmessage);
                if (actiontaskmessage.ActionId == string.Empty || actiontaskmessage.ActionId == null)
                {
                    serviceDTO.CreatedOn = DateTime.UtcNow;
                    result = _translatorService.Translate<ActionTaskMessage>(_IActionTaskRepository.Add(serviceDTO));
                }
                else
                {
                    serviceDTO.UpdatedOn = DateTime.UtcNow;
                    result = _translatorService.Translate<ActionTaskMessage>(_IActionTaskRepository.Update(serviceDTO));
                }

            }
            catch (Exception ex)
            {
                _logger.Error("", ex);
            }
            return result;
        }

        public ActionTaskMessage Delete(ActionTaskMessage actiontaskmessage)
        {
            actiontaskmessage.IsActive = false;
            return this.Update(actiontaskmessage);
        }


        public bool executeCode(ActionTaskMessage actiontaskmessage)
        {
            bool result = true;
            ActionTaskCallerMessage caller;
            try
            {
                caller = new ActionTaskCallerMessage()
                {
                    ActionIdInRunBook = "456349",
                    ActionId = actiontaskmessage.ActionId,
                    Inputs = new Common.DictionaryWithDefault<string, dynamic>(),
                    IncidentId = "04580934"
                };
       //         _bus.Publish<ActionTaskCallerMessage>(caller);
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }

    }
}
