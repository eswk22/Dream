using Application.DTO.Automation;
using Application.Repository;
using Application.Snapshot;
using Application.Utility.Logging;
using Application.Utility.Translators;
using MongoRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Manager.Implementation
{
    public class AutomationManager : IAutomationBusinessManager, IAutomationServiceManager
    {

        private readonly IRepository<AutomationSnapshot> _IAutomationRepository;
        private readonly IEntityTranslatorService _translatorService;
        private readonly ILogger _logger;

        public AutomationManager(IRepository<AutomationSnapshot> iAutomationRepository,
            IEntityTranslatorService translatorService, ILogger logger)
        {
            _translatorService = translatorService;
            _IAutomationRepository = iAutomationRepository;
            _logger = logger;
        }

        public AutomationDTO GetbyId(string Id)
        {
            AutomationDTO result = null;
            try
            {
                result = _translatorService.Translate<AutomationDTO>(this.GetSnapshotbyId(Id));
            }
            catch (Exception ex)
            {
                _logger.Error("Unable to translate to Automation dto", ex, Id);
            }
            return result;
        }

        public AutomationSnapshot GetSnapshotbyId(string Id)
        {
            AutomationSnapshot result = null;
            try
            {
                result = _IAutomationRepository.GetById(Id);
            }
            catch (Exception ex)
            {
                _logger.Error("Unable to Read the Automation", ex, Id);
            }
            return result;
        }


        public string Add(AutomationDTO Automationmessage)
        {
            string result = null;
            try
            {
                AutomationSnapshot snapshot = _translatorService.Translate<AutomationSnapshot>(Automationmessage);
                result = this.Add(snapshot);
            }
            catch (Exception ex)
            {
                _logger.Error("Unable to translate to snapshot", ex, Automationmessage);
            }
            return result;
        }

        public string Add(AutomationSnapshot AutomationSnapshot)
        {
            string result = null;
            try
            {
                result = _IAutomationRepository.Add(AutomationSnapshot).Id;
            }
            catch (Exception ex)
            {
                _logger.Error("Unable to Add the Automation", ex, AutomationSnapshot);
            }
            return result;
        }


        public AutomationDTO Update(AutomationDTO Automationmessage)
        {
            AutomationDTO result = null;
            try
            {
                AutomationSnapshot snapshot = _translatorService.Translate<AutomationSnapshot>(Automationmessage);
                result = _translatorService.Translate<AutomationDTO>(this.Update(snapshot));
            }
            catch (Exception ex)
            {
                _logger.Error("Unable to translate the Automation DTO", ex, Automationmessage);
            }
            return result;
        }


        public AutomationSnapshot Update(AutomationSnapshot AutomationSnapshot)
        {
            AutomationSnapshot result = null;
            try
            {
                result = _IAutomationRepository.Update(AutomationSnapshot);
            }
            catch (Exception ex)
            {
                _logger.Error("Unable to update the Automation", ex, AutomationSnapshot);
            }
            return result;
        }

 
        public IEnumerable<AutomationDTO> Get()
        {
            IEnumerable<AutomationDTO> result = null;
            try
            {
                result = this.GetSnapshots().Select(m => _translatorService.Translate<AutomationDTO>(m)).AsEnumerable();
            }
            catch (Exception ex)
            {
                _logger.Error("Unable to read the Automation", ex);
            }

            return result;
        }



        public IEnumerable<AutomationSnapshot> GetSnapshots()
        {
            IEnumerable<AutomationSnapshot> result = null;
            try
            {
                result = _IAutomationRepository.GetAll();
            }
            catch (Exception ex)
            {
                _logger.Error("Unable to read the Automation", ex);
            }

            return result;
        }



    
       



   
  
        public AutomationDTO Save(AutomationDTO Automationmessage)
        {
            AutomationDTO result = null;
            try
            {
                _logger.Info("Test message");
                AutomationSnapshot snapshot = _translatorService.Translate<AutomationSnapshot>(Automationmessage);
                result = _translatorService.Translate<AutomationDTO>(this.Save(snapshot));
            }
            catch (Exception ex)
            {
                _logger.Error("Unable to save the Automation", ex, Automationmessage);
            }
            return result;
        }


        public AutomationSnapshot Save(AutomationSnapshot Automationmessage)
        {
            AutomationSnapshot result = null;
            try
            {
                _logger.Info("Test message");
                if (Automationmessage.Id == string.Empty || Automationmessage.Id == null)
                {
                    Automationmessage.CreatedOn = DateTime.UtcNow;
                    result = _IAutomationRepository.Add(Automationmessage);
                }
                else
                {
                    Automationmessage.ModifiedOn = DateTime.UtcNow;
                    result = _IAutomationRepository.Update(Automationmessage);
                }

            }
            catch (Exception ex)
            {
                _logger.Error("Unable to save the Automation", ex, Automationmessage);
            }
            return result;
        }

        public AutomationDTO Delete(AutomationDTO Automationmessage)
        {
            Automationmessage.IsActive = false;
            return this.Update(Automationmessage);
        }

        public AutomationSnapshot Delete(AutomationSnapshot AutomationSnapshot)
        {
            AutomationSnapshot.IsActive = false;
            return this.Update(AutomationSnapshot);
        }


    }
}
