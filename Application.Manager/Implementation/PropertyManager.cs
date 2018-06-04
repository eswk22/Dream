using Application.DTO.Property;
using Application.Manager.BusinessContract;
using Application.Manager.ServiceContract;
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
    public class PropertyManager : IPropertyBusinessManager, IPropertyServiceManager
    {

        private readonly IRepository<PropertySnapshot> _IPropertyRepository;
        private readonly IEntityTranslatorService _translatorService;
        private readonly ILogger _logger;

        public PropertyManager(IRepository<PropertySnapshot> iPropertyRepository,
            IEntityTranslatorService translatorService, ILogger logger)
        {
            _translatorService = translatorService;
            _IPropertyRepository = iPropertyRepository;
            _logger = logger;
        }

        public PropertyDTO GetbyId(string Id)
        {
            PropertyDTO result = null;
            try
            {
                result = _translatorService.Translate<PropertyDTO>(this.GetSnapshotbyId(Id));
            }
            catch (Exception ex)
            {
                _logger.Error("Unable to translate to Property dto", ex, Id);
            }
            return result;
        }

        public PropertySnapshot GetSnapshotbyId(string Id)
        {
            PropertySnapshot result = null;
            try
            {
                result = _IPropertyRepository.GetById(Id);
            }
            catch (Exception ex)
            {
                _logger.Error("Unable to Read the Property", ex, Id);
            }
            return result;
        }


        public string Add(PropertyDTO Propertymessage)
        {
            string result = null;
            try
            {
                PropertySnapshot snapshot = _translatorService.Translate<PropertySnapshot>(Propertymessage);
                result = this.Add(snapshot);
            }
            catch (Exception ex)
            {
                _logger.Error("Unable to translate to snapshot", ex, Propertymessage);
            }
            return result;
        }

        public string Add(PropertySnapshot PropertySnapshot)
        {
            string result = null;
            try
            {
                result = _IPropertyRepository.Add(PropertySnapshot).Id;
            }
            catch (Exception ex)
            {
                _logger.Error("Unable to Add the Property", ex, PropertySnapshot);
            }
            return result;
        }


        public PropertyDTO Update(PropertyDTO Propertymessage)
        {
            PropertyDTO result = null;
            try
            {
                PropertySnapshot snapshot = _translatorService.Translate<PropertySnapshot>(Propertymessage);
                result = _translatorService.Translate<PropertyDTO>(this.Update(snapshot));
            }
            catch (Exception ex)
            {
                _logger.Error("Unable to translate the Property DTO", ex, Propertymessage);
            }
            return result;
        }


        public PropertySnapshot Update(PropertySnapshot PropertySnapshot)
        {
            PropertySnapshot result = null;
            try
            {
                result = _IPropertyRepository.Update(PropertySnapshot);
            }
            catch (Exception ex)
            {
                _logger.Error("Unable to update the Property", ex, PropertySnapshot);
            }
            return result;
        }


        public IEnumerable<PropertyDTO> GetProperties()
        {
            IEnumerable<PropertyDTO> result = null;
            try
            {
                result = this.GetSnapshots().Select(m => _translatorService.Translate<PropertyDTO>(m)).AsEnumerable();
            }
            catch (Exception ex)
            {
                _logger.Error("Unable to read the Property", ex);
            }

            return result;
        }



        public IEnumerable<PropertySnapshot> GetSnapshots()
        {
            IEnumerable<PropertySnapshot> result = null;
            try
            {
                result = _IPropertyRepository.GetAll();
            }
            catch (Exception ex)
            {
                _logger.Error("Unable to read the Property", ex);
            }

            return result;
        }










        public PropertyDTO Save(PropertyDTO Propertymessage)
        {
            PropertyDTO result = null;
            try
            {
                _logger.Info("Test message");
                PropertySnapshot snapshot = _translatorService.Translate<PropertySnapshot>(Propertymessage);
                result = _translatorService.Translate<PropertyDTO>(this.Save(snapshot));
            }
            catch (Exception ex)
            {
                _logger.Error("Unable to save the Property", ex, Propertymessage);
            }
            return result;
        }


        public PropertySnapshot Save(PropertySnapshot Propertymessage)
        {
            PropertySnapshot result = null;
            try
            {
                _logger.Info("Test message");
                if (Propertymessage.Id == string.Empty || Propertymessage.Id == null)
                {
                    Propertymessage.CreatedOn = DateTime.UtcNow;
                    result = _IPropertyRepository.Add(Propertymessage);
                }
                else
                {
                    Propertymessage.ModifiedOn = DateTime.UtcNow;
                    result = _IPropertyRepository.Update(Propertymessage);
                }

            }
            catch (Exception ex)
            {
                _logger.Error("Unable to save the Property", ex, Propertymessage);
            }
            return result;
        }

        public PropertyDTO Delete(PropertyDTO Propertymessage)
        {
            Propertymessage.IsActive = false;
            return this.Update(Propertymessage);
        }

        public PropertySnapshot Delete(PropertySnapshot PropertySnapshot)
        {
            PropertySnapshot.IsActive = false;
            return this.Update(PropertySnapshot);
        }


    }
}
